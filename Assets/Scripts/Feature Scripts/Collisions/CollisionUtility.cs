using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public partial class CollisionUtility
    {
        private readonly MainScript mainScriptComponent;
        
        private PixelMovement[] collisionArray;

        private readonly Texture2D collisionTexture;
        private readonly Rect[] collisionRects;

        public CollisionUtility(MainScript mainScript)
        {
            mainScriptComponent = mainScript;

            collisionArray = new PixelMovement[8];

            collisionRects = new Rect[8];
            for (int i = 0; i < 8; i++)
            {
                collisionRects[i] = new Rect((i % 2) * 16, ((7 - i) / 2) * 16, 16, 16);
            }

            collisionTexture = new Texture2D(2, 4, TextureFormat.RGBAFloat, false, false);
            collisionTexture.Apply();
        }

        public (PixelMovement, float) DetectCollision(Vector3 newPosition)
        {
            collisionArray = GetFromTexture(new Vector2(mainScriptComponent.mainTexture.width / 2 - newPosition.x - 0.5f, mainScriptComponent.mainTexture.height / 2 - newPosition.y - 1));
        
            return (collisionArray.Max(), collisionArray.Count(c => c == PixelMovement.LIQUID) / 8f);
        }

        private PixelMovement[] GetFromTexture(Vector2 position)
        {
            var rectReadTexture = new Rect(position, new Vector2(2, 4));
        
            RenderTexture.active = mainScriptComponent.mainTexturePrevFrame;
        
            collisionTexture.ReadPixels(rectReadTexture, 0, 0);
            var pixels = collisionTexture.GetPixels();
            GL.Flush();
            RenderTexture.active = null;

            return pixels.Select(p => DecodePixel(p)).ToArray();
        }

        private PixelMovement DecodePixel(Color p)
        {
            if (p.a < 0.5f) return PixelMovement.EMPTY;//empty
            if (Near(p.a, 1) && Near(p.g, 0.6f) && Near(p.b, 0.6f)) return PixelMovement.EMPTY;//belts
            if (Near(p.a, 1) && Near(p.g, 0.1f) && Near(p.b, 0.1f)) return PixelMovement.EMPTY;//shop
            if (Near(p.a, 1) && Near(p.g, 0.5f) && Near(p.b, 0.5f)) return PixelMovement.EMPTY;//mined
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
