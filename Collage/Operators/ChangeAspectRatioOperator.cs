using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Collage
{
    public class ChangeAspectRatioOperator : IDrawableCollageOperator
    {
        DataAccess dataAccess;
        CollageEditData editData;
        float startAspectRatio;
        TexturedButton okButton;
        TexturedButton rightMoveButton;
        TexturedButton downMoveButton;

        public ChangeAspectRatioOperator() { }

        public void SetData(DataAccess dataAccess, CollageEditData editData)
        {
            this.dataAccess = dataAccess;
            this.editData = editData;
            okButton = new TexturedButton(dataAccess, @"F:\Content\Icons\Check.png", new Point(0, 0));
            rightMoveButton = new TexturedButton(dataAccess, @"F:\Content\Icons\Right.png", new Point(0, 0));
            downMoveButton = new TexturedButton(dataAccess, @"F:\Content\Icons\Down.png", new Point(0, 0));
        }

        public bool Start()
        {
            startAspectRatio = editData.Collage.AspectRatio;
            rightMoveButton.Rectangle = CalculateHandlePositionRight();
            downMoveButton.Rectangle = CalculateHandlePositionDown();

            return true;
        }

        public bool Update()
        {
            if (!okButton.IsMouseOverAndPressed)
            {
                rightMoveButton.Update();
                downMoveButton.Update();

                if (rightMoveButton.IsDown)
                {
                    Rectangle newDrawPosition = editData.DrawRectangle.Rectangle;
                    newDrawPosition.Width += dataAccess.Input.MouseDifferencePoint.X;
                    if (newDrawPosition.Width < 50) newDrawPosition.Width = 50;
                    editData.DrawRectangle.SetRectangle(newDrawPosition);
                }
                if(downMoveButton.IsDown)
                {
                    Rectangle newDrawPosition = editData.DrawRectangle.Rectangle;
                    newDrawPosition.Height += dataAccess.Input.MouseDifferencePoint.Y;
                    if (newDrawPosition.Height < 50) newDrawPosition.Height = 50;
                    editData.DrawRectangle.SetRectangle(newDrawPosition);
                }

                rightMoveButton.Rectangle = CalculateHandlePositionRight();
                downMoveButton.Rectangle = CalculateHandlePositionDown();
                return true;
            }
            else
            {
                Command command = new Command(ExecuteAspectRatioChange, ExecuteAspectRatioChange, editData.Collage.AspectRatio, "Change Aspect Ratio");
                command.SetUndoData(startAspectRatio);
                editData.UndoManager.AddCommand(command);
                return false;
            }
        }

        public void Draw()
        {
            dataAccess.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
            okButton.Draw();
            rightMoveButton.Draw();
            downMoveButton.Draw();
            dataAccess.SpriteBatch.End();
        }

        public Rectangle CalculateHandlePositionRight()
        {
            Rectangle position = new Rectangle();
            position.X = editData.DrawRectangle.Rectangle.Right + 10;
            position.Y = editData.DrawRectangle.Rectangle.Center.Y - 25;
            position.Width = 50;
            position.Height = 50;

            return position;
        }

        public Rectangle CalculateHandlePositionDown()
        {
            Rectangle position = new Rectangle();
            position.X = editData.DrawRectangle.Rectangle.Center.X - 25;
            position.Y = editData.DrawRectangle.Rectangle.Bottom + 10;
            position.Width = 50;
            position.Height = 50;

            return position;
        }

        public object ExecuteAspectRatioChange(object newAspectRatio)
        {
            editData.Collage.AspectRatio = (float)newAspectRatio;
            return null;
        }
    }
}
