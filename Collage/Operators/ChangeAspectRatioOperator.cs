using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

        public ChangeAspectRatioOperator() { }

        public void SetData(DataAccess dataAccess, CollageEditData editData)
        {
            this.dataAccess = dataAccess;
            this.editData = editData;
            okButton = new TexturedButton(dataAccess, @"Content\Images\Check.png", new Point(10, 10));
            cancelButton = new TexturedButton(dataAccess, @"Content\Images\Delete.png", new Point(70, 10));
            rightMoveButton = new TexturedButton(dataAccess, @"Content\Images\Right.png", new Point(0, 0));
            downMoveButton = new TexturedButton(dataAccess, @"Content\Images\Down.png", new Point(0, 0));
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
