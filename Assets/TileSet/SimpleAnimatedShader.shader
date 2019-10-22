Shader "Hidden/SimpleAnimatedShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags{"Queue"="Transparent" "RenderType"="Transparent"}
        Cull Off ZWrite Off ZTest Off
		Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
				float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				float4 color : COLOR;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
				o.color = v.color;
                return o;
            }

            sampler2D _MainTex;

            fixed4 frag (v2f i) : SV_Target
            {
				float timeMod = 0;
				if (dot(i.color, i.color) > 3)
				{
					timeMod = trunc(_Time.y*10)/8;
				}
                fixed4 col = i.color * tex2D(_MainTex, float2(i.uv.x + timeMod, i.uv.y));
                return col;
            }
            ENDCG
        }
    }
}
