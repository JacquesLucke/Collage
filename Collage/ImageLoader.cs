using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Collage
{
    public class ImageLoader
    {
        string fileName = "";
        DataAccess dataAccess;

        public ImageLoader(DataAccess dataAccess, string fileName)
        {
            this.fileName = fileName;
            this.dataAccess = dataAccess;
        }

        public Texture2D Load()
        {
            FileStream fs = new FileStream(fileName, FileMode.Open);
            Texture2D texture = Texture2D.FromStream(dataAccess.GraphicsDevice, fs);
            fs.Close();
            return texture;
        }
    }
}
