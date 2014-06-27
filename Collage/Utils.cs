using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Collage
{
    public abstract class Utils
    {
        public static string FileTypesToFilter(params FileTypes[] types)
        {
            string filter = "";
            for(int i = 0; i< types.Length; i++)
            {
                FileTypes type = types[i];
                switch (type)
                {
                    case FileTypes.Images:
                        filter += "Images|*.jpg;*.png;*.bmp"; break;
                    case FileTypes.JPG:
                        filter += "Jpg|*.jpg"; break;
                    case FileTypes.PNG:
                        filter += "Png|*.png"; break;
                    case FileTypes.BMP:
                        filter += "Bmp|*.bmp"; break;
                }
                if (i < types.Length - 1) filter += "|";
            }
            return filter;
        }
    }
}
