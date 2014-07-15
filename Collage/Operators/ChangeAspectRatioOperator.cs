using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
namespace Collage
{
    public class ChangeAspectRatioOperator : IDrawableCollageOperator
    {
        DataAccess dataAccess;
        CollageEditData editData;
        float startAspectRatio;
        TexturedButton okButton;
        TexturedButton cancelButton;
        TexturedButton rightMoveButton;
        TexturedButton downMoveButton;

        List<TextButton> presetButtons;
        List<KeyValuePair<string, float>> presets;
        SpriteFont font;

        public ChangeAspectRatioOperator() { }

        public void SetData(DataAccess dataAccess, CollageEditData editData)
        {
            this.dataAccess = dataAccess;
            this.editData = editData;

            okButton = new TexturedButton(dataAccess, dataAccess.Content.GetImageSource("check icon").Texture);
            cancelButton = new TexturedButton(dataAccess, dataAccess.Content.GetImageSource("delete icon").Texture);
            rightMoveButton = new TexturedButton(dataAccess, dataAccess.Content.GetImageSource("right icon").Texture);
            downMoveButton = new TexturedButton(dataAccess, dataAccess.Content.GetImageSource("down icon").Texture);

            font = dataAccess.Content.GetSpriteFont("normal font");
            
            presets = new List<KeyValuePair<string, float>>();
            presets.Add(new KeyValuePair<string, float>("1:1", 1f));
            presets.Add(new KeyValuePair<string, float>("2:1", 2f));

            presetButtons = new List<TextButton>();
            for(int i = 0; i< presets.Count; i++)
            {
                KeyValuePair<string, float> pair = presets[i];
                TextButton button = new TextButton(dataAccess, pair.Key);
                button.Rectangle = new Rectangle(10 + i * 70, 10, 60, 45);
                button.BackgroundColor = Color.FromNonPremultiplied(180, 227, 127, 255);
                presetButtons.Add(button);
            }
        }

        public bool Start()
        {
            startAspectRatio = editData.Collage.AspectRatio;
            SetButtonPositions();
            return true;
        }

        public bool Update()
        {
            // allow to zoom and move the view
            editData.DrawRectangle.Zoom(-dataAccess.Input.ScrollWheelDifference / 10f, dataAccess.Input.MousePositionVector);
            if (dataAccess.Input.IsMiddleButtonDown) editData.DrawRectangle.Move(dataAccess.Input.MouseDifferenceVector);

            UpdateButtons();

            if(IsCanceled())
            {
                editData.DrawRectangle.AspectRatio = startAspectRatio;
                return false;
            }

            if (IsConfirmed())
            {
                Command command = new Command(ExecuteAspectRatioChange, ExecuteAspectRatioChange, editData.Collage.AspectRatio, "Change Aspect Ratio");
                command.SetUndoData(startAspectRatio);
                editData.UndoManager.AddCommand(command);
                return false;
            }
            else
            {
                Rectangle newDrawPosition = editData.DrawRectangle.Rectangle;

                // change size
                if (rightMoveButton.IsDown) newDrawPosition.Width += dataAccess.Input.MouseDifferencePoint.X;
                if (downMoveButton.IsDown) newDrawPosition.Height += dataAccess.Input.MouseDifferencePoint.Y;
                // set minimum size
                if (newDrawPosition.Width < 50) newDrawPosition.Width = 50;
                if (newDrawPosition.Height < 50) newDrawPosition.Height = 50;

                editData.DrawRectangle.SetRectangle(newDrawPosition);

                SetButtonPositions();
                return true;
            }
        }

        public void Draw()
        {
            dataAccess.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
            okButton.Draw();
            cancelButton.Draw();
            rightMoveButton.Draw();
            downMoveButton.Draw();

            foreach (TextButton button in presetButtons)
            {
                button.Draw();
            }
            dataAccess.SpriteBatch.End();
        }

        private bool IsCanceled()
        {
            return cancelButton.IsReleased || dataAccess.Input.IsKeyDown(Keys.Escape);
        }
        private bool IsConfirmed()
        {
            return okButton.IsReleased || dataAccess.Input.IsKeyDown(Keys.Enter);
        }

        private void UpdateButtons()
        {
            okButton.Update();
            cancelButton.Update();
            rightMoveButton.Update();
            downMoveButton.Update();

            foreach(TextButton button in presetButtons)
            {
                button.Update();
            }

            for (int i = 0; i < presetButtons.Count; i++ )
            {
                if (presetButtons[i].IsDown) editData.DrawRectangle.AspectRatio = presets[i].Value;
            }
        }
        private void SetButtonPositions()
        {
            rightMoveButton.Rectangle = CalculateHandlePositionRight();
            downMoveButton.Rectangle = CalculateHandlePositionDown();
            okButton.Rectangle = CalculateButtonPositionOk();
            cancelButton.Rectangle = CalculateButtonPositionCancel();
        }

        private Rectangle CalculateHandlePositionRight()
        {
            Rectangle position = new Rectangle();
            position.X = editData.DrawRectangle.Rectangle.Right + 10;
            position.Y = editData.DrawRectangle.Rectangle.Center.Y - 25;
            position.Width = 50;
            position.Height = 50;

            return position;
        }
        private Rectangle CalculateHandlePositionDown()
        {
            Rectangle position = new Rectangle();
            position.X = editData.DrawRectangle.Rectangle.Center.X - 25;
            position.Y = editData.DrawRectangle.Rectangle.Bottom + 10;
            position.Width = 50;
            position.Height = 50;

            return position;
        }
        private Rectangle CalculateButtonPositionOk()
        {
            Rectangle viewport = dataAccess.GraphicsDevice.Viewport.Bounds;

            Rectangle position = new Rectangle();
            position.X = viewport.Right - 50;
            position.Y = viewport.Bottom - 50;
            position.Width = 40;
            position.Height = 40;
            return position;
        }
        private Rectangle CalculateButtonPositionCancel()
        {
            Rectangle viewport = dataAccess.GraphicsDevice.Viewport.Bounds;

            Rectangle position = new Rectangle();
            position.X = viewport.Right - 110;
            position.Y = viewport.Bottom - 50;
            position.Width = 40;
            position.Height = 40;
            return position;
        }


        public object ExecuteAspectRatioChange(object newAspectRatio)
        {
            editData.DrawRectangle.AspectRatio = (float)newAspectRatio;
            return null;
        }
    }
}
