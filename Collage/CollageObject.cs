﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Collage
{
    public class CollageObject
    {
        float aspectRatio = 1.8f;
        Color backgroundColor = Color.DarkOrange;

        public CollageObject() { }

        public float AspectRatio { get { return aspectRatio; } }

        public Color BackgroundColor { get { return backgroundColor; } set { backgroundColor = value; } }
    }
}
