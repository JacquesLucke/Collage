using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Collage.Gui;
using Microsoft.Xna.Framework.Graphics;

namespace Collage
{
    public class TexturedButton : Button
    {
        Texture2D texture;

        public TexturedButton(DataAccess dataAccess, Texture2D texture)
            : base(dataAccess)
        {
            this.texture = texture;
        }

        public void Draw()
        {
            dataAccess.SpriteBatch.Draw(texture, rectangle, GetColorStatus());
        }
    }
}
