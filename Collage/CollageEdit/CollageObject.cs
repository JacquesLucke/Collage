using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Collage
{
    public class CollageObject
    {
        List<Image> images;
        CollageData data;

        public CollageObject() 
        {
            images = new List<Image>();

            data = new CollageData();
            SetDefaultData();
        }
        public void SetDefaultData()
        {
            data.AspectRatio = 1.8f;
            data.BackgroundColor = Color.FromNonPremultiplied(88, 123, 150, 255);
        }

        public float AspectRatio { get { return data.AspectRatio; } set { data.AspectRatio = value; } }
        public Color BackgroundColor { get { return data.BackgroundColor; } set { data.BackgroundColor = value; } }
        public List<Image> Images { get { return images; } }

        public CollageData Data { get { return data; } set { data = value; } }
    }
}
