Shader "Level/NextLevel"
{
    Properties
    {
		_MainTex("Texture", 2D) = "white" {}

		_CharacterPosistion("Char Pos", vector) = (0,0,0,0)
		_CircleRadius("Spotlight size", Range(0,20)) = 3
		_RingSize("Ring size", Range(0, 5)) = 1
		_ColorTint("Outside of the spotlight color", Color) = (0,0,0,0)

		_EmissionMap("Emission Map", 2D) = "black" {}
		_EmissionColor("Emission Color", Color) = (0,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque"}
        LOD 100

			
		Pass
		{
			CGPROGRAM
			#pragma  vertex vert 
			#pragma  fragment frag


            #include "UnityCG.cginc"


            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				float3 worldPos : TEXCOORD1; // World Position of that Vertex 
            };


            sampler2D _MainTex;
            float4 _MainTex_ST;

			float4 _CharacterPosistion;
			float _CircleRadius;
			float _RingSize;
			float4 _ColorTint;

			sampler2D _EmissionMap;
			float4 _EmissionColor;

            v2f vert (appdata v)
            {
                v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

				//o.Emission = tex2D(_EmissionMap, IN.uv_MainTex).rgb * _EmissionColor.rgb * 10;
				
                
				return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
				fixed4 col = _ColorTint;
				
				float dist = distance(i.worldPos, _CharacterPosistion.xyz);
				
				if (dist < _CircleRadius)
					col = tex2D(_MainTex, i.uv);
				else if (dist > _CircleRadius && dist < _CircleRadius + _RingSize)
				{
					float blendStrength = dist - _CircleRadius;
					col = lerp(tex2D(_MainTex, i.uv), _ColorTint, blendStrength / _RingSize);
				}

                return col;
            }


            ENDCG
        }
    }
		
}
