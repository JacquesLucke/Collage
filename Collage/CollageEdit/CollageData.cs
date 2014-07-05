using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Collage
{
    public class CollageData
    {
        public float AspectRatio { get; set; }
        public Color BackgroundColor { get; set; }

        public CollageData GetCopy()
        {
            CollageData data = new CollageData();
            data.AspectRatio = AspectRatio;
            data.BackgroundColor = BackgroundColor;
            return data;
        }
    }
}
