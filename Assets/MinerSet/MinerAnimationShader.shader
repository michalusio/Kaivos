Shader "Hidden/MinerAnimationShader"
{
    Properties
    {
        _IdleTex ("Idle Texture", 2D) = "white" {}
		_MoveTex ("Move Texture", 2D) = "white" {}
		_MoveDirection ("Move Direction", Float) = 1.0
		_MoveSpeed ("Move Speed", Float) = 1.0
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
            #pragma fragment frag alpha:blend

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
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _IdleTex;
			sampler2D _MoveTex;
			float _MoveSpeed;
			float _MoveDirection;
			
            float4 frag (v2f i) : SV_Target
            {
				uint idleW, moveW, h;
				idleW = 48;
				moveW = 96;
				h = 16;
				float moveDirSign = sign(_MoveDirection);
				_MoveDirection = abs(_MoveDirection);
				if (abs(_MoveSpeed) > 0.1)
				{
					return tex2D(_MoveTex, (float2(trunc(_Time.w * 3 * _MoveDirection), 0) + i.uv * int2(moveDirSign, 1)) / float2(6, 1));
				}
                return tex2D(_IdleTex, (float2(trunc(_Time.w * 3 * _MoveDirection), 0) + i.uv * int2(moveDirSign, 1)) / float2(3, 1));
            }
            ENDCG
        }
    }
}
