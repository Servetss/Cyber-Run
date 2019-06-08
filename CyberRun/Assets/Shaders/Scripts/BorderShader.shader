Shader "Level/Border"
{

	Properties{
		_MainColor("Main Color", Color) = (1,1,1,1)
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Mask("Mask To Dissolve", 2D) = "white" {}
		_EmissionColor("Line Color", Color) = (1,1,1,1)
	}

		SubShader{
			Tags {"RenderType" = "Opaque"}
			LOD 300
			CGPROGRAM
			#pragma target 2.0
			#include "UnityCG.cginc"
			#pragma surface surf Lambert alpha 


			sampler2D _MainTex;
			half4 _MainColor;

			sampler2D _Mask;
			half4 _EmissionColor;



			struct Input {
				float2 uv_MainTex;
				float2 uv_BumpMap;
				float2 uv_Detail;
			};

			void surf(Input IN, inout SurfaceOutput o) {
				half4 c = 1 - tex2D(_MainTex, IN.uv_MainTex);
				//(transition4 + (tex2D(_Mask, IN.uv_MainTex))
				//float2 coords = IN.uv_MainTex;
				

				o.Albedo = c * _MainColor;
				
				o.Emission = c.rgb * _EmissionColor ;
				o.Alpha = c;

			}
			ENDCG
	}
		Fallback "Diffuse"
}
