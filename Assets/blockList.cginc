#define SQRT2 1.41421356237
#define ORE_SIZE 60.0
#define MAPGEN_IRON_H 0.5
#define MAPGEN_COAL_H 0.5
#define MAPGEN_COPPER_H 0.2
#define MAPGEN_GOLD_H 0.8

#define PARALLAX_SCALE 0.5

#define EMPTY_POS float2(0, 128)

#define GRASS float4(0, 0.2, 0.2, 1.0)
#define DIRT float4(0.1, 0.2, 0.2, 1.0)
#define ROCK1 float4(0.2, 0.2, 0.2, 1.0)
#define ROCK2 float4(0.3, 0.2, 0.2, 1.0)
#define BEDROCK float4(0.4, 0.2, 0.2, 1.0)
#define GRASS_POS float2(0, 0)
#define DIRT_POS float2(0, 4)
#define ROCK1_POS float2(0, 8)
#define ROCK2_POS float2(0, 12)
#define BEDROCK_POS float2(0, 16)

#define LADDER float4(0, 0.3, 0.3, 1.0)
#define LADDER_POS float2(0, 120)
#define TORCH float4(0.1, 0.3, 0.3, 1.0)
#define TORCH_POS float2(0, 124)

#define COPPER float4(0, 0.4, 0.4, 1.0)
#define IRON float4(0.1, 0.4, 0.4, 1.0)
#define COAL float4(0.2, 0.4, 0.4, 1.0)
#define GOLD float4(0.3, 0.4, 0.4, 1.0)
#define COPPER_POS float2(0, 60)
#define IRON_POS float2(0, 40)
#define COAL_POS float2(0, 20)
#define GOLD_POS float2(0, 80)



#define BELT_LEFT float4(0, 0.6, 0.6, 1.0)
#define BELT_RIGHT float4(0.1, 0.6, 0.6, 1.0)
#define BELT_UP float4(0.2, 0.6, 0.6, 1.0)
#define BELT_LEFT_POS float2(0, 108)
#define BELT_RIGHT_POS float2(0, 112)
#define BELT_UP_POS float2(0, 116)

#define WATER float4(0, 0.7, 0.7, 1.0)
#define LAVA float4(1, 0.7, 0.7, 1.0)
#define WATER_POS float2(0, 104)
#define LAVA_POS float2(0, 100)


#define SHOP_POS_START float2(0, 0)

#define IS_EQUAL(x, y) all(abs(x - y)<0.01)
#define IS_EMPTY(x) (x.a < 0.5)

#define IS_SHOP(x) IS_EQUAL(x.yzw, float3(0.1, 0.1, 1.0))
#define IS_LIGHTED(x) (IS_SHOP(x) || IS_EQUAL(x, TORCH))
#define IS_GLOWING(x) IS_EQUAL(x, LAVA)
#define IS_LIQUID(x) IS_EQUAL(x.yzw, float3(0.7, 0.7, 1.0))
#define IS_MINEABLE(x) IS_EQUAL(x.yzw, float3(0.4, 0.4, 1.0))
#define IS_MINED(x) IS_EQUAL(x.yzw, float3(0.5, 0.5, 1.0))
#define IS_BELT(x) IS_EQUAL(x.yzw, float3(0.6, 0.6, 1.0))
#define IS_ANIMATED(x) (IS_LIQUID(x) || IS_BELT(x) || IS_EQUAL(x, TORCH))
#define IS_MOVABLE(x) (IS_LIQUID(x) || IS_MINED(x))