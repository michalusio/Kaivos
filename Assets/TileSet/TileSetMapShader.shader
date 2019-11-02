Shader "Hidden/TileSetMapShader"
{
    Properties
    {
        _MainTex ("Computed Texture", 2D) = "white" {}
		_ShadowTex ("Computed Shadow Texture", 2D) = "white" {}
		_TileTex ("Tile Set Texture", 2D) = "white" {}
		_MachineTex ("Machine Tile Set Texture", 2D) = "white" {}
		_Sizes ("Texture Sizes", Vector) = (0, 0, 0, 0)
		_Sizes2 ("Texture Sizes 2", Vector) = (0, 0, 0, 0)
		_PlayerPos ("Player Position", Vector) = (0, 0, 0, 0)
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
			#include "..\noiseSimplex.cginc"
			#include "..\blockList.cginc"
			
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

            sampler2D _MainTex;
			sampler2D _TileTex;
			sampler2D _ShadowTex;
			sampler2D _MachineTex;
			float4 _Sizes;
			float4 _Sizes2;
			float4 _PlayerPos;
			
			float2 decodeShop(float4 shopColor)
			{
				float id = shopColor.x * 64;
				return float2(trunc(id / 8),7 -  trunc(id % 8)) * 4;
			}
			
            float4 frag (v2f i) : SV_Target
            {
                float4 col = tex2D(_MainTex, i.uv);
				float4 shadow = saturate(tex2D(_ShadowTex, i.uv));
				float2 pos = float2(0, 0);
				bool wasMined = false;
				if (IS_MINED(col))
				{
					col.yzw = float3(0.4, 0.4, 1.0);
					wasMined = true;
				}
				if (IS_EMPTY(col) && i.uv.y * _Sizes.y < 981)
				{
					pos = EMPTY_POS;
				}
				else if (IS_EQUAL(col, GRASS))
				{
					pos = GRASS_POS;
				}
				else if (IS_EQUAL(col, DIRT))
				{
					pos = DIRT_POS;
				}
				else if (IS_EQUAL(col, ROCK1))
				{
					pos = ROCK1_POS;
				}
				else if (IS_EQUAL(col, ROCK2))
				{
					pos = ROCK2_POS;
				}
				else if (IS_EQUAL(col, BEDROCK))
				{
					pos = BEDROCK_POS;
				}
				else if (IS_EQUAL(col, IRON))
				{
					pos = IRON_POS;
				}
				else if (IS_EQUAL(col, COAL))
				{
					pos = COAL_POS;
				}
				else if (IS_EQUAL(col, COPPER))
				{
					pos = COPPER_POS;
				}
				else if (IS_EQUAL(col, GOLD))
				{
					pos = GOLD_POS;
				}
				else if (IS_EQUAL(col, LAVA))
				{
					pos = LAVA_POS;
				}
				else if (IS_EQUAL(col, WATER))
				{
					pos = WATER_POS;
				}
				else if (IS_EQUAL(col, BELT_LEFT))
				{
					pos = BELT_LEFT_POS;
				}
				else if (IS_EQUAL(col, BELT_RIGHT))
				{
					pos = BELT_RIGHT_POS;
				}
				else if (IS_EQUAL(col, BELT_UP))
				{
					pos = BELT_UP_POS;
				}
				else if (IS_EQUAL(col, LADDER))
				{
					pos = LADDER_POS;
				}
				else if (IS_EQUAL(col, TORCH))
				{
					pos = TORCH_POS;
				}
				else if (IS_SHOP(col))
				{
					pos = SHOP_POS_START + decodeShop(col);
					col = tex2D(_MachineTex, (((i.uv * _Sizes.xy * 4) % 4) + float2(pos.x, _Sizes2.y - 4 - pos.y))/_Sizes2.xy);
					return col * shadow;
				}
				else if (IS_TREENEMY(col))
				{
					pos = TREENEMY_CORE_POS;
				}
				else return col * shadow;
				
				float4 colAbove = tex2D(_MainTex, i.uv + float2(0, 1.0/_Sizes.y));
				float4 colLeft = tex2D(_MainTex, i.uv - float2(1.0/_Sizes.x, 0));
				float4 colRight = tex2D(_MainTex, i.uv + float2(1.0/_Sizes.x, 0));
				float4 colBelow = tex2D(_MainTex, i.uv - float2(0, 1.0/_Sizes.y));
				
				if (wasMined)
				{
					bool leftMined = IS_MINED(colLeft);
					bool rightMined = IS_MINED(colRight);
					if (IS_MINED(colAbove))
					{
						pos.y += 4;
					}
					else
					{
						if (leftMined)
						{
							if (rightMined)
							{
								pos.y += 4;
							}
							else
							{
								pos.y += 16;
							}
						}
						else if (rightMined)
						{
							pos.y += 12;
						}
						else
						{
							pos.y += 8;
						}
					}
				}
				float2 parallaxPos = _PlayerPos.xy * PARALLAX_SCALE;
				float noise;
				float2 position = trunc(i.uv * _Sizes.xy);
				if (IS_ANIMATED(col))
				{
					noise = trunc(_Time.z * 5) * 4;
					if (!IS_BELT(col))
					{
						noise += trunc((snoise(position) + 1) * 4) * 4;
					}
					col = tex2D(_TileTex, (((i.uv * _Sizes.xy * 4) % 4) + float2(pos.x + noise, _Sizes.w - 4 - pos.y))/_Sizes.zw);
				}
				else if (IS_TREENEMY(col))
				{
					float ratio = col.y * 0.5;
					
					float2 fraction = ((i.uv * _Sizes.xy * 4) % 4);
					
					int treenemyJoining = (IS_TREENEMY(colAbove) ? 1 : 0) + (IS_TREENEMY(colBelow) ? 2 : 0)
										+ (IS_TREENEMY(colLeft) ? 4 : 0) + (IS_TREENEMY(colRight) ? 8 : 0) + (IS_TREENEMY_CORE(col) ? 16 : 0);
					switch(treenemyJoining)
					{
						default:
							pos = float2(0, 168);
							break;//CORE
						case 0:
							pos = float2(0, 140);
							break;//no connections
						case 1:
							pos = float2(0, 172);
							break;//connection above
						case 2:
							pos = float2(0, 180);
							break;//connection below
						case 3:
							pos = float2(0, 160);
							break;//connection above and below
						case 4:
							pos = float2(0, 184);
							break;//connection left
						case 5:
							pos = float2(0, 156);
							break;//connection left and above
						case 6:
							pos = float2(0, 132);
							break;//connection left and below
						case 7:
							pos = float2(0, 144);
							break;//connection left, above and below
						case 8:
							pos = float2(0, 176);
							break;//connection right
						case 9:
							pos = float2(0, 148);
							break;//connection right and above
						case 10:
							pos = float2(0, 124);
							break;//connection right and below
						case 11:
							pos = float2(0, 136);
							break;//connection right, above and below
						case 12:
							pos = float2(0, 164);
							break;//connection left and right
						case 13:
							pos = float2(0, 152);
							break;//connection left, right and above
						case 14:
							pos = float2(0, 128);
							break;//connection left, right and below
						case 15:
							pos = float2(0, 140);
							break;//connections everywhere
					}
					
					if (IS_TREENEMY_CORE(col))
					{
						float noise = trunc(_Time.z * 5) * 4;
						col = tex2D(_TileTex, (fraction + float2(pos.x + noise, _Sizes.w - 4 - pos.y))/_Sizes.zw);
					}
					else
					{
						float noise = trunc((snoise(position) + 1) * 4) * 4;
						col = tex2D(_TileTex, (fraction + float2(pos.x + noise, _Sizes.w - 4 - pos.y))/_Sizes.zw);
					}
					
					col.g = lerp(col.g, sqrt(col.g), ratio);
				}
				else
				{
					if (IS_EMPTY(col))
					{
						position = trunc(i.uv * _Sizes.xy - parallaxPos/4);
						noise = trunc((snoise(position) + 1) * 4) * 4;
						col = tex2D(_TileTex, (((i.uv * _Sizes.xy * 4 - parallaxPos) % 4) + float2(pos.x + noise, _Sizes.w - 4 - pos.y))/_Sizes.zw);
					}
					else
					{
						noise = trunc((snoise(position) + 1) * 4) * 4;
						col = tex2D(_TileTex, (((i.uv * _Sizes.xy * 4) % 4) + float2(pos.x + noise, _Sizes.w - 4 - pos.y))/_Sizes.zw);
					}
				}
				if (i.uv.y * _Sizes.y < 981)
				{
					pos = EMPTY_POS;
					position = trunc(i.uv * _Sizes.xy - parallaxPos/4);
					float4 emptyCol = tex2D(_TileTex, (((i.uv * _Sizes.xy * 4 - parallaxPos) % 4) + float2(pos.x + trunc((snoise(position) + 1) * 4) * 4, _Sizes.w - 4 - pos.y))/_Sizes.zw);
					return float4(lerp(emptyCol.rgb, col.rgb, step(0.5, col.a)), 1.0) * shadow;
				}
				return col * shadow;
            }
            ENDCG
        }
    }
}
