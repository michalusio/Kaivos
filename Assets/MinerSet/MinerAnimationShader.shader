Shader "Hidden/MinerAnimationShader"
{
    Properties
    {
        _IdleTex ("Idle Texture", 2D) = "white" {}
		_MoveTex ("Move Texture", 2D) = "white" {}
		_ShadowTex ("Shadow Texture", 2D) = "white" {}
		_PositionSpeedDirection ("Position", Vector) = (0, 0, 1.0, 1.0)
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
			sampler2D _ShadowTex;
			float4 _PositionSpeedDirection;
			float4 _ShadowTex_TexelSize;
			
            float4 frag (v2f i) : SV_Target
            {
				uint idleW, moveW, h;
				idleW = 48;
				moveW = 96;
				h = 16;
				
				float2 _Position = _PositionSpeedDirection.xy;
				float _MoveSpeed = _PositionSpeedDirection.z;
				float _MoveDirection = _PositionSpeedDirection.w;
				
				
				float moveDirSign = sign(_MoveDirection);
				_MoveDirection = abs(_MoveDirection);
				float4 col;
				if (abs(_MoveSpeed) > 0.1)
				{
					col = tex2D(_MoveTex, (float2(trunc(_Time.w * 3 * _MoveDirection), 0) + i.uv * int2(moveDirSign, 1)) / float2(6, 1));
				}
				else
				{
					col = tex2D(_IdleTex, (float2(trunc(_Time.w * 3 * _MoveDirection), 0) + i.uv * int2(moveDirSign, 1)) / float2(3, 1));
				}
				float2 coords =(_Position + float2(512, 512) + i.uv / float2(32, 16)) * _ShadowTex_TexelSize.xy;
                return col * saturate(tex2D(_ShadowTex, float2(1 - coords.x, coords.y)));
            }
            ENDCG
        }
    }
}
