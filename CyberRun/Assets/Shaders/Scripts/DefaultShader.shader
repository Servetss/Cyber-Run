Shader "Level/DefaultShader"
{
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_MainColor("Main Color", Color) = (1,1,1,1)

		_EmissionMap("Emission Map", 2D) = "black" {}
		_EmissionColor("Emission Color", Color) = (0,0,0,0)

		_Curvature("Curvature", Float) = 0.001

		_PlayerPos("World player Position", vector) = (0,0,0,0)

		[Toggle(REDIFY_ON)] _Player("Player", Int) = 0
	}

		SubShader{
			Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
			LOD 100
			CGPROGRAM
			#pragma target 2.0
			#include "UnityCG.cginc"
			#pragma surface surf Lambert  vertex:vert addshadow

			#pragma shader_feature REDIFY_ON


			uniform sampler2D _MainTex;
			uniform sampler2D _EmissionMap;
			uniform half4 _MainColor;
			uniform half4 _EmissionColor;

			uniform float _Curvature;

			int _Player;
			float4 _PlayerPos;



			struct Input {
				float2 uv_MainTex;
				float2 uv_BumpMap;
				float2 uv_Detail;
				float3 worldPos;
			};


			void vert(inout appdata_full v)
			{
				float4 worldSpace = mul(unity_ObjectToWorld, v.vertex);
				worldSpace.xyz -= _WorldSpaceCameraPos.xyz;

				worldSpace = float4(0.0, (worldSpace.z * worldSpace.z) * -_Curvature, 0.0, 0.0);

				v.vertex += mul(unity_ObjectToWorld, worldSpace);
			}

			void surf(Input IN, inout SurfaceOutput o) {
				half4 c = tex2D(_MainTex, IN.uv_MainTex);
				half4 m = tex2D(_EmissionMap, IN.uv_MainTex);
				bool Player = false;

				o.Albedo = c * _MainColor;
				o.Emission = m * _EmissionColor * 0.5;


#if REDIFY_ON
				Player = true;
#endif
				if (!Player)
				{
					float dist22 = (IN.worldPos.z - _PlayerPos.z) / 10;
					if (dist22 < 10)
					{
						clip(1);
						o.Alpha = c.a;
					}
					else
						clip(-1);
				}
				else
					o.Alpha = c.a;

			}
			ENDCG
	}
		Fallback "Mobile/Diffuse"
}
