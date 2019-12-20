using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class MapReadService
    {
        public Color[] GetFromTexture(Vector2 position, Vector2Int size)
        {
            var rectReadTexture = new Rect(position - ((Vector2) size) / 4, size);
        
            RenderTexture.active = ClassManager.MainScript.mainTexturePrevFrame;

            Texture2D collisionTexture = new Texture2D(size.x, size.y, TextureFormat.RGBAFloat, false, false);
            collisionTexture.Apply();
            collisionTexture.ReadPixels(rectReadTexture, 0, 0);
            collisionTexture.Apply();
            var pixels = collisionTexture.GetPixels();
            GL.Flush();
            RenderTexture.active = null;

            return pixels.ToArray();
        }

        public static bool ColorNear(Color a, Color b)
        {
            return Mathf.Abs(a.r - b.r) + Mathf.Abs(a.g - b.g) + Mathf.Abs(a.b - b.b) + Mathf.Abs(a.a - b.a) < 0.01f;
        }
    }
}
