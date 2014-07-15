using Collage.Gui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Collage
{
    public class TextButton : Button
    {
        Texture2D texture;
        Color backgroundColor;
        SpriteFont font;
        string text;

        public TextButton(DataAccess dataAccess, string text)
            : base(dataAccess)
        {
            this.texture = dataAccess.Content.GetImageSource("empty").Texture;
            this.font = dataAccess.Content.GetSpriteFont("normal font");
            this.text = text;
            this.backgroundColor = Color.FromNonPremultiplied(180, 180, 180, 255);
        }

        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set { backgroundColor = value; }
        }

        public void Draw()
        {
            Vector2 position = Utils.CenterText(text, font, rectangle);
            dataAccess.SpriteBatch.Draw(texture, rectangle, Utils.MultiplyColors(backgroundColor, GetColorStatus()));
            dataAccess.SpriteBatch.DrawString(font, text, position, Color.FromNonPremultiplied(20, 20, 20, 255));
        }
    }
}
