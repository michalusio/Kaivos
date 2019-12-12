#define SQRT2 1.41421356237
#define MAPGEN_IRON_ORE_SIZE 60.0
#define MAPGEN_COAL_ORE_SIZE 100.0
#define MAPGEN_COPPER_ORE_SIZE 60.0
#define MAPGEN_GOLD_ORE_SIZE 40.0
#define MAPGEN_IRON_H 0.5
#define MAPGEN_COAL_H 0.5
#define MAPGEN_COPPER_H 0.2
#define MAPGEN_GOLD_H 0.8
#define MAPGEN_SURFACE_TREE_COUNT 64
#define MAPGEN_TREENEMY_COUNT 64
#define MAPGEN_TREENEMY_BRANCH_COUNT 6
#define MAPGEN_TREENEMY_BRANCH_LENGTH 30


#define PARALLAX_SCALE 0.5

#define EMPTY_POS float2(0, 192)

#define GRASS float4(0, 0.2, 0.2, 1.0)
#define DIRT float4(0.1, 0.2, 0.2, 1.0)
#define ROCK1 float4(0.2, 0.2, 0.2, 1.0)
#define ROCK2 float4(0.3, 0.2, 0.2, 1.0)
#define BEDROCK float4(0.4, 0.2, 0.2, 1.0)
#define TREE_TRUNK float4(0.0, 0.5, 0.2, 1.0)
#define TREE_LEAVES float4(0.0, 0.6, 0.2, 1.0)
#define GRASS_POS float2(0, 0)
#define DIRT_POS float2(0, 4)
#define ROCK1_POS float2(0, 8)
#define ROCK2_POS float2(0, 12)
#define BEDROCK_POS float2(0, 16)
#define TREE_TRUNK_POS float2(0, 196)
#define TREE_LEAVES_POS float2(0, 200)

#define LADDER float4(0, 0.3, 0.3, 1.0)
#define TORCH float4(0.1, 0.3, 0.3, 1.0)
#define LADDER_POS float2(0, 120)
#define TORCH_POS float2(0, 188)

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

#define TREENEMY_CORE_ASLEEP float4(0.0, 0.0, 0.8, 1.0)
#define TREENEMY_CORE_WOKEN float4(0.0, 0.1, 0.8, 1.0)
#define TREENEMY_BRANCH_ASLEEP float4(0.1, 0.0, 0.8, 1.0)
#define TREENEMY_BRANCH_ALERT float4(0.1, 0.1, 0.8, 1.0)
#define TREENEMY_CORE_POS float2(0, 168)
#define TREENEMY_BRANCH_POS float2(0, 124)

#define COPPER_BAR float4(0.0, 0.0, 0.9, 1.0)
#define IRON_BAR float4(0.1, 0.0, 0.9, 1.0)
#define GOLD_BAR float4(0.2, 0.0, 0.9, 1.0)
#define STEEL_BAR float4(0.3, 0.0, 0.9, 1.0)
#define COPPER_BAR_POS float2(0, 212)
#define IRON_BAR_POS float2(0, 220)
#define GOLD_BAR_POS float2(0, 216)
#define STEEL_BAR_POS float2(0, 224)

#define SHOP_POS_START float2(0, 0)
#define SHOP_PLACEMENT int2(508, 1000)
#define JUNCTION_POS_START float2(0, 20)
#define FORGE_POS_START float2(0, 32)

#define JUNCTION_MID float4(4 / 9.0, 0.2, 0.1, 1.0)
#define FORGE_MID float4(4 / 9.0, 0.3, 0.1, 1.0)

#define MINED(u) (float4(u.x, 0.5, 0.5, 1.0))

#define IS_EQUAL(x, y) all(abs(x - y)<0.01)
#define IS_EMPTY(x) (x.a < 0.5)
#define IS_TRANSPARENT(x) (IS_EMPTY(x) || IS_EQUAL(x, TREE_LEAVES))
#define IS_STRUCTURE(u) (IS_SHOP(u) || IS_JUNCTION(u) || IS_FORGE(u))
#define IS_COMPONENT(u) IS_EQUAL(u.zw, float2(0.9, 1.0))
#define IS_TREENEMY_CORE(u) IS_EQUAL(u.xzw, float3(0.0, 0.8, 1.0))
#define IS_TREENEMY_BRANCH(u) IS_EQUAL(u.xzw, float3(0.1, 0.8, 1.0))
#define IS_TREENEMY_TRIGGER(u) (abs(u.y - 1.0) < 0.05)
#define IS_TREENEMY(u) IS_EQUAL(u.zw, float2(0.8, 1.0))
#define IS_SHOP(x) IS_EQUAL(x.yzw, float3(0.1, 0.1, 1.0))
#define IS_JUNCTION(x) IS_EQUAL(x.yzw, float3(0.2, 0.1, 1.0))
#define IS_FORGE(x) IS_EQUAL(x.yzw, float3(0.3, 0.1, 1.0))
#define IS_LIGHTED(x) (IS_SHOP(x) || IS_EQUAL(x, TORCH))
#define IS_GLOWING(x) IS_EQUAL(x, LAVA)
#define IS_LIQUID(x) IS_EQUAL(x.yzw, float3(0.7, 0.7, 1.0))
#define IS_MINEABLE(x) IS_EQUAL(x.yzw, float3(0.4, 0.4, 1.0))
#define IS_MINED(x) IS_EQUAL(x.yzw, float3(0.5, 0.5, 1.0))
#define IS_BELT(x) IS_EQUAL(x.yzw, float3(0.6, 0.6, 1.0))
#define IS_ANIMATED(x) (IS_LIQUID(x) || IS_BELT(x) || IS_EQUAL(x, TORCH))
#define IS_MOVABLE(x) (IS_LIQUID(x) || IS_MINED(x) || IS_COMPONENT(x))