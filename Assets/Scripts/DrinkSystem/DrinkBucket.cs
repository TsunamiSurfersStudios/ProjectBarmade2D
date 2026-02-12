using System.Collections.Generic;
using UnityEngine;

namespace DrinkSystem
{
    public class DrinkBucket : MonoBehaviour
    {
        List<Color32> colors;
        bool isHovered = false;
        void Start()
        {
            colors = new List<Color32>();
        }

        public void AddColor(Color32 color)
        {
            colors.Add(color);
            UpdateColor();
        }

        private void UpdateColor()
        {
            if (colors.Count == 0) return;
            int r = 0, g = 0, b = 0;
            foreach (Color32 color in colors)
            {
                r += color.r;
                g += color.g;
                b += color.b;
            }
            r /= colors.Count;
            g /= colors.Count;
            b /= colors.Count;
            Color32 mixedColor = new Color32((byte)r, (byte)g, (byte)b, 255);
            gameObject.GetComponent<SpriteRenderer>().color = mixedColor;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            isHovered = true;
            Debug.Log("Hovering over Drink Bucket");
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            isHovered = false;
            Debug.Log("Not hovering over Drink Bucket");
        }

        public bool IsHovered()
        {
            return isHovered;
        }
    }
}
