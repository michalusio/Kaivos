#include "..\..\noiseSimplex.cginc"
#include "..\..\blockList.cginc"

RWTexture2D<float4> FrameBefore;
RWTexture2D<float4> NewFrame;
int seed;
float Time;

static const int3 eee = int3(1, 0, -1);

float seedNoise(float2 pos)
{
	return snoise(pos + seed);
}

float seedNoise(float3 pos)
{
	return snoise(pos + seed);
}

bool rand(float2 pos)
{
	return seedNoise(Time * 37 + pos) > 0;
}

bool rand(float2 pos, int times)
{
	return rand(pos + times * 223);
}