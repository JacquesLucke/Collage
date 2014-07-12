using Microsoft.Xna.Framework;
namespace Collage
{
    public class ChangeAspectRatioOperator : IDrawableCollageOperator
    {
        DataAccess dataAccess;
        CollageEditData editData;
        float startAspectRatio;
        TexturedButton okButton;
        TexturedButton moveHandle;
        bool moveHandlePressed = false;

        public ChangeAspectRatioOperator() { }

        public void SetData(DataAccess dataAccess, CollageEditData editData)
        {
            this.dataAccess = dataAccess;
            this.editData = editData;

            okButton = new TexturedButton(dataAccess, new ImageSource(dataAccess, 1, 1, Color.Firebrick), new Rectangle(10, 200, 100, 50));
            moveHandle = new TexturedButton(dataAccess, new ImageSource(dataAccess, 1, 1, Color.AntiqueWhite), new Rectangle(300, 300, 50, 50));
        }

        public bool Start()
        {
            startAspectRatio = editData.Collage.AspectRatio;
            moveHandlePressed = false;

            moveHandle.Position = CalculateMoveHandlePosition();

            return true;
        }

        public bool Update()
        {
            if (!okButton.IsPressed)
            {
                if (moveHandle.IsDown) moveHandlePressed = true;
                if (dataAccess.Input.IsLeftButtonReleased) moveHandlePressed = false;
                if (moveHandlePressed)
                {
                    Rectangle r = editData.DrawRectangle.Rectangle;
                    r.Width += dataAccess.Input.MouseDifferencePoint.X;
                    if (r.Width < 50) r.Width = 50;
                    editData.DrawRectangle.SetRectangle(r);

                    moveHandle.Position = CalculateMoveHandlePosition();
                }
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
            dataAccess.SpriteBatch.Begin();
            okButton.Draw();
            moveHandle.Draw();
            dataAccess.SpriteBatch.End();
        }

        public FloatRectangle CalculateMoveHandlePosition()
        {
            FloatRectangle position = new FloatRectangle();
            position.X = editData.DrawRectangle.Rectangle.Right + 10;
            position.Y = editData.DrawRectangle.Center.Y - 25;
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
