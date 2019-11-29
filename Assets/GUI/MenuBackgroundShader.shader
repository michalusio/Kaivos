Shader "Unlit/MenuBackgroundShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Sizes ("ScreenSize/TileSetSize", Vector) = (0, 0, 0, 0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
			#include "../noiseSimplex.cginc"
            #define BLOCK_POS float2(0, 8)

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float4 _Sizes;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				i.uv /= 32;
				float2 pos = BLOCK_POS;
				float2 position = trunc(i.uv * _Sizes.xy);
				float noise = trunc((snoise(position) + 1) * 4) * 4;
				float4 col = tex2D(_MainTex, ((((i.uv + 1) * _Sizes.xy * 4) % 4) + float2(pos.x + noise, _Sizes.w - 4 - pos.y)) / _Sizes.zw);
                return col;
            }
            ENDCG
        }
    }
}
