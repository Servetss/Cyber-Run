Shader "Level/Dissolve2"
{
	Properties{
			_MainColor("Main Color", Color) = (1,1,1,1)
			_MainTex("Base (RGB)", 2D) = "white" {}


			[Header(Mask Texture)]
			[NoScaleOffset] _Mask1("Mask To Dissolve 1", 2D) = "white" {}
			[NoScaleOffset] _Mask2("Mask To Dissolve 2", 2D) = "white" {}
			[NoScaleOffset] _Mask3("Mask To Dissolve 3", 2D) = "white" {}
			[NoScaleOffset] _Mask4("Mask To Dissolve 4", 2D) = "white" {}
			_MaskSelected("Type Mask", Int) = 0

				[Space]
			_LineTexture("Line Texture", 2D) = "white" {}
			_Range("Range", Range(0,3)) = 0

			_LineSize("LineSize", Float) = 0.001
			_Color("Line Color", Color) = (1,1,1,1)

			_PlayerPos("World player Position", vector) = (0,0,0,0)

			_EmissionMap("Emission Map", 2D) = "black" {}
			_SecondEmission("Second Emission Map", 2D) = "black" {}
			_EmissionColor("Emission Color", Color) = (0,0,0)

			_EndKind("Type of End Level Scene", Int) = 0
	}

		SubShader{
			Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
			LOD 100
			CGPROGRAM
			#pragma target 2.0
			#include "UnityCG.cginc"
			#pragma surface surf Lambert  vertex:myvert addshadow

			sampler2D _MainTex;
			sampler2D _LineTexture;

			sampler2D _Mask;
			sampler2D _Mask1;
			sampler2D _Mask2;
			sampler2D _Mask3;
			sampler2D _Mask4;

			half4 _Color;
			half4 _MainColor;
			float _Range;
			float _LineSize;
			float4 _PlayerPos;

			int _MaskSelected;
			int _EndKind;

			sampler2D _EmissionMap;
			sampler2D _SecondEmission;
			float4 _EmissionColor;

			struct Input {
				float2 uv_MainTex;
				float2 uv_Detail;
				float3 worldPos; //add this and Unity will set it automatically
				float4 uv_GrabTex;
				float2 ul_LineTexture;
			};

			void myvert(inout appdata_full v, out Input data) {
				UNITY_INITIALIZE_OUTPUT(Input, data);
			
			}



			void surf(Input IN, inout SurfaceOutput o) {

				half4 m;   // = tex2D(_Mask, IN.uv_MainTex);
				half4 lc;  // = tex2D(_Mask, IN.uv_MainTex - _LineSize);
				half4 lc2; // = tex2D(_Mask, IN.uv_MainTex + _LineSize);
				switch (_MaskSelected)
				{
				case 0:
					m = tex2D(_Mask1, IN.uv_MainTex);
					lc = tex2D(_Mask1, IN.uv_MainTex - _LineSize);
					lc2 = tex2D(_Mask1, IN.uv_MainTex + _LineSize);
					break;
				case 1:
					m = tex2D(_Mask2, IN.uv_MainTex);
					lc = tex2D(_Mask2, IN.uv_MainTex - _LineSize);
					lc2 = tex2D(_Mask2, IN.uv_MainTex + _LineSize);
					break;
				case 2:
					m = tex2D(_Mask3, IN.uv_MainTex);
					lc = tex2D(_Mask3, IN.uv_MainTex - _LineSize);
					lc2 = tex2D(_Mask3, IN.uv_MainTex + _LineSize);
					break;
				case 3:
					m = tex2D(_Mask4, IN.uv_MainTex);
					lc = tex2D(_Mask4, IN.uv_MainTex - _LineSize);
					lc2 = tex2D(_Mask4, IN.uv_MainTex + _LineSize);
					break;
				default:
					m = tex2D(_Mask1, IN.uv_MainTex);
					lc = tex2D(_Mask1, IN.uv_MainTex - _LineSize);
					lc2 = tex2D(_Mask1, IN.uv_MainTex + _LineSize);
					break;
				}
				

				half4 c = tex2D(_MainTex, IN.uv_MainTex);
				half4 lc3 = tex2D(_LineTexture, IN.uv_MainTex + _SinTime) * _Color;


				o.Albedo = c * _MainColor;
				o.Alpha = c.a;
				o.Emission = tex2D(_EmissionMap, IN.uv_MainTex).rgb * _EmissionColor / 5;


				switch (_EndKind)
				{
				case 0:
					float factor = (1 - m.rgb.x) + (1 - m.rgb.y) + ( 1 - m.rgb.z);
					float dist22 = (IN.worldPos.z - _PlayerPos.z) / 15;
					if (factor >= dist22)
					{
						float factor2 = (1 - lc.rgb.x) + (1 - lc.rgb.y) + (1 - lc.rgb.z);
						float factor3 = (1 - lc2.rgb.x) + (1 - lc2.rgb.y) + (1- lc2.rgb.z);

						if (factor2 - 0.1 < dist22 || factor3 - 0.1  < dist22)
						{
							o.Albedo = lc3;
							o.Emission = _EmissionColor.rgb;
						}
						else
						{
							clip(-1);
						}
					}
					break;
				case 1:
					float transition4 = distance(1.5, IN.worldPos.y);
					float d1 = IN.worldPos.z - _PlayerPos.z - 15;

					d1 = d1 > 2 ? d1 :	2;

					float cond = -2 + (transition4 + (m) * (d1 / 3));

					if (cond > 0 && cond < 0.2)
					{
						o.Albedo = lc3;
						o.Emission = _EmissionColor.rgb;
					}
					else if(cond <= 0)
						clip(-1);


					break;
				case 3:

					half dissolve_value3 = tex2D(_Mask, IN.uv_MainTex).r; //Get how much we have to dissolve based on our dissolve texture
					float d = abs(IN.worldPos.z - _PlayerPos.z) - 10;

					d = d > 0 ? d : 0;

					clip((dissolve_value3 - d) / 5); //Dissolve!
					break;
				default:
					break;
				}

				//// ----- 1 -----
				//float factor = m.rgb.x + m.rgb.y + m.rgb.z;
				//if (factor >= _Range)
				//{
				//	float factor2 = lc.rgb.x + lc.rgb.y + lc.rgb.z;
				//	float factor3 = lc2.rgb.x + lc2.rgb.y + lc2.rgb.z;
				//	if (factor2 - 0.1 < _Range || factor3 - 0.1  < _Range)
				//	{
				//		o.Albedo = lc3;

				//		o.Emission = _EmissionColor.rgb * 10;
				//	}
				//	else
				//	{	
				//		clip(-1);
				//	}
				//}
				//// -----------------

				//// ------- 2 -------
				//half dissolve_value = tex2D(_Mask1, IN.uv_MainTex).x;

				//float dist = distance(_PlayerPos, IN.worldPos);
				//float dist2 = abs(IN.worldPos.z - _PlayerPos.z) - 4;

				//dist2 = dist2 > 0 ? dist2 : 0;

				//clip(dissolve_value - dist / (dist2 * 4)); //"6" is the maximum distance where your object will start showing


				////Set albedo, alpha, smoothness etc[...]
				//// ----------

				//// ------- 3 -------
				//half dissolve_value = tex2D(_MainTex, IN.uv_MainTex).x;
				//float transition = distance(_PlayerPos, IN.worldPos);

				//clip(-5 * _Range + (transition + (tex2D(_Mask1, IN.uv_MainTex)) * 15 ));

				//// --------------

				////------ 4 ----------
				//half dissolve_value = tex2D(_Mask1, IN.uv_MainTex).r; //Get how much we have to dissolve based on our dissolve texture
				//clip(dissolve_value - _Range); //Dissolve!
				////-------------------


				//// ------ 5 ---------

				//half dissolve_value = tex2D(_MainTex, IN.uv_MainTex).y;
				//float transition = distance(2, IN.worldPos.y);

				//clip(-2 + (transition + (tex2D(_Mask1, IN.uv_MainTex)) * _Range));
				//// ------------------


				// ---------- 6 -----------
				//o.Alpha = 0.0;
				// ------------------------

			}

			ENDCG
	}
		Fallback "Diffuse"
}
