using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Collage
{
    public class AutoPositonOperator : ICollageOperator, ISpecialOperatorStart
    {
        DataAccess dataAccess;
        CollageEditData editData;

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
            List<object> currentData = GetCurrentData();
            List<object> calculationData = CalculateAutoPosition();

            Command command = new Command(ExecuteAutoPosition, ExecuteAutoPosition, calculationData, "Auto Position");
            command.SetUndoData(currentData);
            editData.UndoManager.ExecuteAndAddCommand(command);

            return false;
        }

        public List<object> GetCurrentData()
        {
            List<Image> order = new List<Image>(editData.Collage.Images);
            List<ImageData> imageDataList = new List<ImageData>(order.Count);
            for(int i = 0; i < order.Count; i++) imageDataList.Add(order[i].Data);

            return new List<object>() { order, imageDataList };
        }
        public List<object> CalculateAutoPosition()
        {
            int amount = editData.Collage.Images.Count;
            List<Image> order = new List<Image>();
            List<ImageData> imageDataList = new List<ImageData>();

            int lines = (int)Math.Min(Math.Max(1, Math.Floor(Math.Sqrt(amount / editData.Collage.AspectRatio))), amount);
            int needToAddAmount = amount % Math.Max(1, lines);

            for (int i = 0; i < lines; i++)
            {
                // calculates how many images will go in this line
                int imagesInLine = amount / lines;
                if (needToAddAmount > 0)
                {
                    if (dataAccess.Random.NextDouble() < 1 / (float)lines || needToAddAmount == lines - i)
                    {
                        imagesInLine++;
                        needToAddAmount--;
                    }
                }

                // calculates an ImageData object for every Image in this line
                for (int j = 0; j < imagesInLine; j++)
                {
                    ImageData data = new ImageData();

                    // position
                    Vector2 newCenter = new Vector2();
                    newCenter.X = 1 / (float)imagesInLine * (j + 0.5f);
                    newCenter.Y = (1 / (float)(lines + 1) * (i + 1) - 0.5f) * MathHelper.Clamp(200 / (float)amount, 1.02f, 1.15f) + 0.5f;
                    newCenter.X += (float)(dataAccess.Random.NextDouble() * 2 - 1) / imagesInLine / 2f;  // randomize X direction
                    newCenter.Y += (float)(dataAccess.Random.NextDouble() * 2 - 1) / lines / 2f;         // randomize Y direction
                    data.Center = newCenter;

                    // rotation
                    data.Rotation = (float)(dataAccess.Random.NextDouble() * 2 - 1) / 10;   // random amount of rotation

                    // scale
                    float newWidth = newWidth = 1 / (float)imagesInLine * 1.7f;
                    newWidth += (float)(dataAccess.Random.NextDouble() * 2 - 1) * newWidth / 2; // randomize scale
                    data.Width = newWidth;

                    imageDataList.Add(data);
                }
            }

            // change order
            List<Image> orderBuffer = new List<Image>(editData.Collage.Images);
            List<ImageData> dataBuffer = new List<ImageData>(imageDataList);
            imageDataList.Clear();
            for (int i = 0; i < amount; i++)
            {
                int index = dataAccess.Random.Next(orderBuffer.Count);
                // order
                order.Add(orderBuffer[index]);
                orderBuffer.Remove(orderBuffer[index]);
                // image data
                imageDataList.Add(dataBuffer[index]);
                dataBuffer.Remove(dataBuffer[index]);
            }

            List<object> calculationData = new List<object>() { order, imageDataList };
            return calculationData;
        }

        public object ExecuteAutoPosition(object rawData)
        {
            List<object> data = (List<object>)rawData;
            List<Image> newOrder = (List<Image>)data[0];
            List<ImageData> newDataList = (List<ImageData>)data[1];

            editData.Collage.Images.Clear();
            editData.Collage.Images.AddRange(newOrder);
            for(int i = 0; i< editData.Collage.Images.Count; i++)
            {
                editData.Collage.Images[i].Data = newDataList[i];
            }

            return null;
        }
    }
}
