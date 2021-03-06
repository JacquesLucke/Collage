﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Collage
{
    class RandomSelectionOperator : IDrawableCollageOperator
    {
        DataAccess dataAccess;
        CollageEditData editData;
        List<Image> selectionBefore;
        List<Image> randomOrder;
        float selectionFraction;
        SpriteFont font;
        Texture2D emptyTexture;

        public RandomSelectionOperator() { }

        public void SetData(DataAccess dataAccess, CollageEditData editData)
        {
            this.dataAccess = dataAccess;
            this.editData = editData;

            font = dataAccess.Content.GetSpriteFont("normal font");
            emptyTexture = dataAccess.Content.GetImageSource("empty").Texture;
        }

        public bool Start()
        {
            selectionFraction = 0.5f;
            selectionBefore = new List<Image>(editData.SelectedImages);

            List<Image> imageListCopy = new List<Image>(editData.Collage.Images);
            randomOrder = new List<Image>();
            for (int i = imageListCopy.Count - 1; i >= 0; i--)
            {
                int index = dataAccess.Random.Next(i);
                randomOrder.Add(imageListCopy[index]);
                imageListCopy.RemoveAt(index);
            }
            
            return true;
        }

        public bool Update()
        {
            if(dataAccess.Input.IsKeyPressed(Keys.Escape))
            {
                editData.SelectedImages.Clear();
                editData.SelectedImages.AddRange(selectionBefore);
                return false;
            }

            if (dataAccess.Input.IsLeftButtonReleased || dataAccess.Input.AreKeysPressed(Keys.Enter))
            {
                Command command = new Command(ExecuteSelectionChange, ExecuteSelectionChange, new List<Image>(editData.SelectedImages), "Random Selection");
                command.SetUndoData(selectionBefore);
                editData.UndoManager.AddCommand(command);
                return false;
            }
            else
            {
                selectionFraction = dataAccess.Input.MousePositionVector.X / (float)dataAccess.GraphicsDevice.Viewport.Width;
                selectionFraction = MathHelper.Clamp(selectionFraction, 0, 1);

                editData.SelectedImages.Clear();
                editData.SelectedImages.AddRange(randomOrder.GetRange(0, (int)Math.Floor(randomOrder.Count * selectionFraction)));
                return true;
            }
        }

        public void Draw()
        {
            Rectangle textBackgroundRec = new Rectangle(0, 0, dataAccess.GraphicsDevice.Viewport.Width, 25);

            string text = (int)Math.Round(selectionFraction * 100) + " % - " + editData.SelectedImages.Count + " / " + editData.Collage.Images.Count;
            Vector2 position = new Vector2(Utils.CenterTextHorizontal(text, font, textBackgroundRec), -3);

            dataAccess.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
            dataAccess.SpriteBatch.Draw(emptyTexture, textBackgroundRec, Color.FromNonPremultiplied(240, 240, 240, 255));
            dataAccess.SpriteBatch.DrawString(font, text, position, Color.FromNonPremultiplied(20, 20, 20, 255));
            dataAccess.SpriteBatch.End();
        }

        public object ExecuteSelectionChange(object newSelection)
        {
            editData.SelectedImages.Clear();
            editData.SelectedImages.AddRange((List<Image>)newSelection);
            return null;
        }
    }
}
