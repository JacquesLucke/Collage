using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Collage
{
    public class ContentHelper
    {
        GraphicsDevice graphicsDevice;
        ContentManager content;

        Dictionary<string, Effect> effects;
        Dictionary<string, ImageSource> imageSources;
        Dictionary<string, SpriteFont> spriteFonts;

        public ContentHelper(GraphicsDevice graphicsDevice, ContentManager content)
        {
            this.graphicsDevice = graphicsDevice;
            this.content = content;

            effects = new Dictionary<string, Effect>();
            imageSources = new Dictionary<string, ImageSource>();
            spriteFonts = new Dictionary<string, SpriteFont>();
        }

        public void LoadEffect(string fileName, string effectName)
        {
            if (!effects.ContainsKey(effectName))
            {
                // load the image effect
                BinaryReader br = new BinaryReader(new FileStream(fileName, FileMode.Open));
                Effect effect = new Effect(graphicsDevice, br.ReadBytes((int)br.BaseStream.Length));
                br.Close();

                effects.Add(effectName, effect);
            }
        }
        public void LoadImagesSource(string fileName, string sourceName)
        {
            if(!imageSources.ContainsKey(sourceName))
            {
                ImageSource imageSource = new ImageSource(graphicsDevice, fileName);
                imageSource.Load();
                imageSources.Add(sourceName, imageSource);
            }
        }
        public void LoadSpriteFont(string fileName, string spriteFontName)
        {
            if(!spriteFonts.ContainsKey(spriteFontName))
            {
                SpriteFont font = content.Load<SpriteFont>(fileName);
                spriteFonts.Add(spriteFontName, font);
            }
        }

        public Effect GetEffect(string effectName)
        {
            return effects[effectName];
        }
        public ImageSource GetImageSource(string sourceName)
        {
            return imageSources[sourceName];
        }
        public SpriteFont GetSpriteFont(string spriteFontName)
        {
            return spriteFonts[spriteFontName];
        }
    }
}
