using Microsoft.Xna.Framework;

namespace Collage
{
    public class ImageData
    {
        public Vector2 Center { get; set; }
        public float Width { get; set; }
        public float Rotation { get; set; }

        public ImageData GetCopy()
        {
            ImageData imageData = new ImageData();
            imageData.Center = Center;
            imageData.Width = Width;
            imageData.Rotation = Rotation;

            return imageData;
        }
    }
}
