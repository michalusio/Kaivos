﻿Shader "Hidden/TileSetMapShader"
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
				float id = shopColor.x * 15;
				float noise = (trunc(_Time.z * 5) % 5) * 3;
				return float2(trunc(id / 5) + noise, 4 - trunc(id % 5)) * 4;
			}
			
			float2 decodeJunction(float4 junctionColor)
			{
				float id = junctionColor.x * 9;
				float noise = (trunc(_Time.z * 8) % 8) * 3;
				return float2(trunc(id / 3) + noise, 2 - trunc(id % 3)) * 4;
			}
			
			float2 decodeForge(float4 forgeColor)
			{
				float id = forgeColor.x * 9;
				float noise = (trunc(_Time.z * 5) % 5) * 3;
				return float2(trunc(id / 3) + noise, 2 - trunc(id % 3)) * 4;
			}
			
            float4 frag (v2f i) : SV_Target
            {
				float2 position = trunc(i.uv * _Sizes.xy);
				float2 parallaxPos = trunc(_PlayerPos.xy * 4) * PARALLAX_SCALE / 4;
				float3 coordTable = float3(1.0 / _Sizes.x, 0, 1.0 / _Sizes.y);
				float4 colAbove = tex2D(_MainTex, i.uv + coordTable.yz);
				float4 colLeft = tex2D(_MainTex, i.uv - coordTable.xy);
				float4 colRight = tex2D(_MainTex, i.uv + coordTable.xy);
				float4 colBelow = tex2D(_MainTex, i.uv - coordTable.yz);
				
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
					if ((IS_EQUAL(colLeft, BELT_LEFT) || IS_EQUAL(colLeft, BELT_UP)) && (IS_EQUAL(colRight, BELT_LEFT) || IS_EQUAL(colRight, BELT_UP)))
					{
						pos = BELT_LEFT_JOINED_POS;
					}
					else
					{
						pos = BELT_LEFT_POS;
					}
				}
				else if (IS_EQUAL(col, BELT_RIGHT))
				{
					if ((IS_EQUAL(colLeft, BELT_RIGHT) || IS_EQUAL(colLeft, BELT_UP)) && (IS_EQUAL(colRight, BELT_RIGHT) || IS_EQUAL(colRight, BELT_UP)))
					{
						pos = BELT_RIGHT_JOINED_POS;
					}
					else
					{
						pos = BELT_RIGHT_POS;
					}
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
				else if (IS_EQUAL(col, TREE_TRUNK))
				{
					pos = TREE_TRUNK_POS;
				}
				else if (IS_EQUAL(col, TREE_LEAVES))
				{
					pos = TREE_LEAVES_POS;
				}
				else if (IS_EQUAL(col, IRON_BAR))
				{
					pos = IRON_BAR_POS;
				}
				else if (IS_EQUAL(col, COPPER_BAR))
				{
					pos = COPPER_BAR_POS;
				}
				else if (IS_EQUAL(col, GOLD_BAR))
				{
					pos = GOLD_BAR_POS;
				}
				else if (IS_EQUAL(col, STEEL_BAR))
				{
					pos = STEEL_BAR_POS;
				}
				else if (IS_SHOP(col))
				{
					pos = SHOP_POS_START + decodeShop(col);
				}
				else if (IS_JUNCTION(col))
				{
					pos = JUNCTION_POS_START + decodeJunction(col);
				}
				else if (IS_FORGE(col))
				{
					pos = FORGE_POS_START + decodeForge(col);
				}
				else if (IS_TREENEMY(col))
				{
					pos = TREENEMY_CORE_POS;
				}
				else return col * shadow;
				
				if (IS_STRUCTURE(col))
				{
					col = tex2D(_MachineTex, (((i.uv * _Sizes.xy * 4) % 4) + float2(pos.x, _Sizes2.y - 4 - pos.y)) / _Sizes2.xy);
					if (IS_EQUAL(colRight, LAVA) || IS_EQUAL(colLeft, LAVA) || IS_EQUAL(colAbove, LAVA))
					{
						float noise = trunc((snoise(position) + 1) * 4) * 4 + trunc(_Time.z * 5) * 4;
						pos = LAVA_POS;
						position = trunc(i.uv * _Sizes.xy);
						float4 emptyCol = tex2D(_TileTex, (((i.uv * _Sizes.xy * 4) % 4) + float2(pos.x + noise, _Sizes.w - 4 - pos.y))/_Sizes.zw);
						return float4(lerp(emptyCol.rgb, col.rgb, step(0.5, col.a)), 1.0) * shadow;
					}
					if (IS_EQUAL(colRight, WATER) || IS_EQUAL(colLeft, WATER) || IS_EQUAL(colAbove, WATER))
					{
						float noise = trunc((snoise(position) + 1) * 4) * 4 + trunc(_Time.z * 5) * 4;
						pos = WATER_POS;
						position = trunc(i.uv * _Sizes.xy);
						float4 emptyCol = tex2D(_TileTex, (((i.uv * _Sizes.xy * 4) % 4) + float2(pos.x + noise, _Sizes.w - 4 - pos.y))/_Sizes.zw);
						return float4(lerp(emptyCol.rgb, col.rgb, step(0.5, col.a)), 1.0) * shadow;
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
				
				float noise;
				if (IS_ANIMATED(col))
				{
					noise = trunc(_Time.z * 5) * 4;
					if (!IS_BELT(col))
					{
						noise += trunc((snoise(position * 231) + 1) * 4) * 4;
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
						position = trunc(i.uv * _Sizes.xy - parallaxPos * 0.25);
						noise = trunc((snoise(position) + 1) * 4) * 4;
						col = tex2D(_TileTex, (((i.uv * _Sizes.xy * 4 - parallaxPos) % 4) + float2(pos.x + noise, _Sizes.w - 4 - pos.y)) / _Sizes.zw);
					}
					else
					{
						noise = trunc((snoise(position) + 1) * 4) * 4;
						col = tex2D(_TileTex, (((i.uv * _Sizes.xy * 4) % 4) + float2(pos.x + noise, _Sizes.w - 4 - pos.y))/_Sizes.zw);
					}
				}
				if (IS_EQUAL(colRight, LAVA) || IS_EQUAL(colLeft, LAVA) || IS_EQUAL(colAbove, LAVA))
				{
					float noise = trunc((snoise(position) + 1) * 4) * 4 + trunc(_Time.z * 5) * 4;
					pos = LAVA_POS;
					position = trunc(i.uv * _Sizes.xy);
					float4 emptyCol = tex2D(_TileTex, (((i.uv * _Sizes.xy * 4) % 4) + float2(pos.x + noise, _Sizes.w - 4 - pos.y))/_Sizes.zw);
					return float4(lerp(emptyCol.rgb, col.rgb, step(0.5, col.a)), 1.0) * shadow;
				}
				if (IS_EQUAL(colRight, WATER) || IS_EQUAL(colLeft, WATER) || IS_EQUAL(colAbove, WATER))
				{
					float noise = trunc((snoise(position) + 1) * 4) * 4 + trunc(_Time.z * 5) * 4;
					pos = WATER_POS;
					position = trunc(i.uv * _Sizes.xy);
					float4 emptyCol = tex2D(_TileTex, (((i.uv * _Sizes.xy * 4) % 4) + float2(pos.x + noise, _Sizes.w - 4 - pos.y))/_Sizes.zw);
					return float4(lerp(emptyCol.rgb, col.rgb, step(0.5, col.a)), 1.0) * shadow;
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
