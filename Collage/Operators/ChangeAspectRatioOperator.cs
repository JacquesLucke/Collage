﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
            okButton = new TexturedButton(dataAccess, @"F:\Content\Icons\Check.png", new Point(10, 10));
            cancelButton = new TexturedButton(dataAccess, @"F:\Content\Icons\Delete.png", new Point(70, 10));
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
            // update buttons
            okButton.Update();
            cancelButton.Update();
            rightMoveButton.Update();
            downMoveButton.Update();

            if(cancelButton.IsDown)
            {
                editData.DrawRectangle.AspectRatio = startAspectRatio;
                return false;
            }

            if (!okButton.IsDown)
            {
                Rectangle newDrawPosition = editData.DrawRectangle.Rectangle;

                // change size
                if (rightMoveButton.IsDown) newDrawPosition.Width += dataAccess.Input.MouseDifferencePoint.X;
                if(downMoveButton.IsDown)  newDrawPosition.Height += dataAccess.Input.MouseDifferencePoint.Y;
                // set minimum size
                if (newDrawPosition.Width < 50) newDrawPosition.Width = 50;
                if (newDrawPosition.Height < 50) newDrawPosition.Height = 50;

                editData.DrawRectangle.SetRectangle(newDrawPosition);

                // recalculate position of the buttons
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
            cancelButton.Draw();
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
            editData.DrawRectangle.AspectRatio = (float)newAspectRatio;
            return null;
        }
    }
}
