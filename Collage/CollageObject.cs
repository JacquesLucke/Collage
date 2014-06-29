using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Collage
{
    public class CollageObject
    {
        float aspectRatio = 1.8f;
        Color backgroundColor = Color.DarkOrange;
        List<Image> images = new List<Image>();

        public CollageObject() { }

        public float AspectRatio { get { return aspectRatio; } }
        public Color BackgroundColor { get { return backgroundColor; } set { backgroundColor = value; } }
        public List<Image> Images { get { return images; } }
    }
}
