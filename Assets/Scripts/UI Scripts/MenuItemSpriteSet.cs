﻿using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class MenuItemSpriteSet : MonoBehaviour
{
    public BlockType SetTo;
    private bool _initialized;

    public enum BlockType
    {
        GRASS,
        DIRT,
        ROCK1,
        ROCK2,
        BEDROCK,

        COPPER,
        IRON,
        COAL,
        GOLD,

        LAVA,
        WATER,

        LADDER,
        TORCH,

        BELT_LEFT,
        BELT_RIGHT,
        BELT_UP,

        TREENEMY_CORE,
        TREENEMY_BRANCH
    }

    private static readonly Dictionary<BlockType, int> BlockTypeY = new Dictionary<BlockType, int>
    {
        {BlockType.GRASS, 0},
        {BlockType.DIRT, 4},
        {BlockType.ROCK1, 8},
        {BlockType.ROCK2, 12},
        {BlockType.BEDROCK, 16},

        {BlockType.COPPER, 60},
        {BlockType.IRON, 40},
        {BlockType.COAL, 20},
        {BlockType.GOLD, 80},

        {BlockType.LAVA, 100},
        {BlockType.WATER, 104},

        {BlockType.LADDER, 120},
        {BlockType.TORCH, 188},

        {BlockType.BELT_LEFT, 108},
        {BlockType.BELT_RIGHT, 112},
        {BlockType.BELT_UP, 116},

        {BlockType.TREENEMY_CORE, 168},
        {BlockType.TREENEMY_BRANCH, 140}
    };

    void Update()
    {
        if (!_initialized)
        {
            if (ClassManager.DrawingScript != null)
            {
                var image = GetComponent<RawImage>();
                var tileTexture = ClassManager.DrawingScript.TileSetMapMaterial.GetTexture("_TileTex");
                image.uvRect = new Rect(0, (tileTexture.height - 4 - (float)BlockTypeY[SetTo])/tileTexture.height, 4f/tileTexture.width, 4f/tileTexture.height);
                _initialized = true;
            }
        }
    }
}

