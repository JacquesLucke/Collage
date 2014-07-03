using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Collage
{
    public class AutoPositonOperator : ICollageOperator
    {
        DataAccess dataAccess;
        CollageEditData editData;

        Vector2[] startPositions;
        List<Image> startOrder;

        public AutoPositonOperator() { }

        public void SetData(DataAccess dataAccess, CollageEditData editData)
        {
            this.dataAccess = dataAccess;
            this.editData = editData;
        }
        public bool CanStart()
        {
            return dataAccess.Keymap["auto position"].IsCombinationPressed(dataAccess.Input);
        }

        public bool Start()
        {
            int amount = editData.Collage.Images.Count;

            // save state before
            startPositions = new Vector2[amount];
            startOrder = new List<Image>(editData.Collage.Images);
            for (int i = 0; i < amount; i++)
            {
                startPositions[i] = editData.Collage.Images[i].Center;
            }

            int lines = (int)Math.Min(Math.Max(1, Math.Floor(Math.Sqrt(amount / editData.Collage.AspectRatio))), amount);
            int[] imagesPerLine = new int[lines];
            int needToAddAmount = amount % Math.Max(1, lines);
            List<Image> imagesBefore = new List<Image>(editData.Collage.Images);
            List<Image> imageBuffer = new List<Image>();
            editData.Collage.Images.Clear();

            for (int i = 0; i < lines; i++)
            {
                imagesPerLine[i] = amount / lines;
                if (needToAddAmount > 0)
                {
                    if (dataAccess.Random.NextDouble() < 1 / (float)lines || needToAddAmount == lines - i)
                    {
                        imagesPerLine[i]++;
                        needToAddAmount--;
                    }
                }

                for(int j = 0; j < imagesPerLine[i]; j++)
                {
                    Image image = imagesBefore.First();
                    imagesBefore.Remove(image);
                    imageBuffer.Add(image);

                    // position
                    Vector2 newCenter = new Vector2();
                    newCenter.X = 1 / (float)imagesPerLine[i] * (j + 1) - 1 / (float)imagesPerLine[i] / 2;
                    newCenter.Y = (1 / (float)(lines + 1) * i + 1 / (float)(lines + 1) - 0.5f) * MathHelper.Clamp(200 / (float)amount, 1.02f, 1.15f) + 0.5f;
                    // randomize positon
                    newCenter.X += (float)(dataAccess.Random.NextDouble() * 2 - 1) / imagesPerLine[i] / 2f;
                    newCenter.Y += (float)(dataAccess.Random.NextDouble() * 2 - 1) / lines / 2f;

                    image.Center = newCenter;

                    // scale
                    float newWidth = newWidth = 1 / (float)imagesPerLine[i] * 1.7f;
                    newWidth += (float)(dataAccess.Random.NextDouble() * 2 - 1) * newWidth / 2;
                    image.Width = newWidth;

                    // rotation
                    image.Rotation = (float)(dataAccess.Random.NextDouble() * 2 - 1) / 10;
                }
            }

            for (int i = 0; i < amount;i++)
            {
                int index = dataAccess.Random.Next(imageBuffer.Count);
                editData.Collage.Images.Add(imageBuffer[index]);
                imageBuffer.Remove(imageBuffer[index]);
            }


                return false;
        }
    }
}
