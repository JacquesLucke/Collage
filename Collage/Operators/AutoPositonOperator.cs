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

        List<Image> startOrder;
        Vector2[] startCenters;
        float[] startRotations;
        float[] startWidths;

        List<Image> afterOrder;
        Vector2[] afterCenters;
        float[] afterRotations;
        float[] afterWidths;

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
            startCenters = new Vector2[amount];
            startRotations = new float[amount];
            startWidths = new float[amount];
            startOrder = new List<Image>(editData.Collage.Images);
            for (int i = 0; i < amount; i++)
            {
                startCenters[i] = editData.Collage.Images[i].Center;
                startRotations[i] = editData.Collage.Images[i].Rotation;
                startWidths[i] = editData.Collage.Images[i].Width;
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

                for (int j = 0; j < imagesPerLine[i]; j++)
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

            for (int i = 0; i < amount; i++)
            {
                int index = dataAccess.Random.Next(imageBuffer.Count);
                editData.Collage.Images.Add(imageBuffer[index]);
                imageBuffer.Remove(imageBuffer[index]);
            }

            // save state after
            afterCenters = new Vector2[amount];
            afterRotations = new float[amount];
            afterWidths = new float[amount];
            afterOrder = new List<Image>(editData.Collage.Images);
            for (int i = 0; i < amount; i++)
            {
                afterCenters[i] = editData.Collage.Images[i].Center;
                afterRotations[i] = editData.Collage.Images[i].Rotation;
                afterWidths[i] = editData.Collage.Images[i].Width;
            }

            List<object> newData = new List<object>() {afterOrder, afterCenters, afterRotations, afterWidths};
            List<object> startData = new List<object>() { startOrder, startCenters, startRotations, startWidths };

            Command command = new Command(ExecuteAutoPosition, ExecuteAutoPosition, newData, "Auto Position");
            command.SetUndoData(startData);
            editData.UndoManager.AddCommand(command);

            return false;
        }

        public object ExecuteAutoPosition(object rawData)
        {
            List<object> data = (List<object>)rawData;
            List<Image> newOrder = (List<Image>)data[0];
            Vector2[] newCenters = (Vector2[])data[1];
            float[] newRotations = (float[])data[2];
            float[] newWidths = (float[])data[3];

            editData.Collage.Images.Clear();
            editData.Collage.Images.AddRange(newOrder);
            for(int i = 0; i< editData.Collage.Images.Count; i++)
            {
                editData.Collage.Images[i].Center = newCenters[i];
                editData.Collage.Images[i].Rotation = newRotations[i];
                editData.Collage.Images[i].Width = newWidths[i];
            }

            return null;
        }
    }
}
