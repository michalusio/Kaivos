using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public partial class CollisionUtility
    {
        private PixelMovement[] collisionArray;
        private readonly Rect[] collisionRects;

        public CollisionUtility()
        {
            collisionArray = new PixelMovement[8];

            collisionRects = new Rect[8];
            for (int i = 0; i < 8; i++)
            {
                collisionRects[i] = new Rect((i % 2) * 16, ((7 - i) / 2) * 16, 16, 16);
            }
        }

        public (PixelMovement, float) DetectCollision(Vector3 newPosition)
        {
            var playerPos = new Vector2(ClassManager.MainScript.mainTexture.width / 2 - newPosition.x, ClassManager.MainScript.mainTexture.height / 2 - newPosition.y);
            var pixels = ClassManager.MapReadService.GetFromTexture(playerPos + new Vector2(0.5f, 1), new Vector2Int(2, 4));
            collisionArray = ToMovement(pixels);
        
            return (collisionArray.Max(), collisionArray.Count(c => c == PixelMovement.LIQUID) / 8f);
        }

        private PixelMovement[] ToMovement(Color[] pixels)
        {
            return pixels.Select(p => DecodePixel(p)).ToArray();
        }

        private PixelMovement DecodePixel(Color p)
        {
            if (p.a < 0.5f) return PixelMovement.EMPTY;//empty
            if (Near(p.a, 1) && Near(p.g, 0.6f) && Near(p.b, 0.6f)) return PixelMovement.EMPTY;//belts
            if (Near(p.a, 1) && Near(p.g, 0.1f) && Near(p.b, 0.1f)) return PixelMovement.EMPTY;//shop
            if (Near(p.a, 1) && Near(p.g, 0.5f) && Near(p.b, 0.5f)) return PixelMovement.EMPTY;//mined
            if (Near(p.a, 1) && Near(p.b, 0.9f)) return PixelMovement.EMPTY;//crafts
            if (Near(p.a, 1) && Near(p.g, 0.5f) && Near(p.b, 0.2f)) return PixelMovement.EMPTY;//tree trunk
            if (Near(p.a, 1) && Near(p.g, 0.6f) && Near(p.b, 0.2f)) return PixelMovement.EMPTY;//tree leaves
            if (Near(p.a, 1) && Near(p.r, 0.1f) && Near(p.g, 0.3f) && Near(p.b, 0.3f)) return PixelMovement.EMPTY;//torch
            if (Near(p.a, 1) && Near(p.g, 0.7f) && Near(p.b, 0.7f)) return PixelMovement.LIQUID;//liquid
            if (Near(p.a, 1) && Near(p.r, 0) && Near(p.g, 0.3f) && Near(p.b, 0.3f)) return PixelMovement.LADDER;//ladder
            return PixelMovement.SOLID;
        }

        private bool Near(float x, float y)
        {
            return Mathf.Abs(x - y) < 0.05f;
        }

        public List<(Texture2D, Rect)> GetCollisionDebug()
        {
            return collisionArray.Select(c => c == 0 ? Texture2D.whiteTexture : Texture2D.normalTexture).Zip(collisionRects, (a, b) => (a, b)).ToList();
        }
    }
}
