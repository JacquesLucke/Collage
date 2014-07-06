using System;

namespace Collage
{
    public class ScaleOperator : IUpdateableCollageOperator, ISpecialOperatorStart
    {
        DataAccess dataAccess;
        CollageEditData editData;

        DateTime startTime;
        float[] startWidth;

        public ScaleOperator() { }

        public void SetData(DataAccess dataAccess, CollageEditData editData)
        {
            this.dataAccess = dataAccess;
            this.editData = editData;
        }
        public bool CanStart()
        {
            return dataAccess.Input.ScrollWheelDifference != 0 && !dataAccess.Input.IsStrg && editData.SelectedImages.Count > 0;
        }

        public bool Start()
        {
            startTime = DateTime.Now;

            startWidth = new float[editData.SelectedImages.Count];
            for (int i = 0; i < editData.SelectedImages.Count; i++)
            {
                startWidth[i] = editData.SelectedImages[i].Width;
            }
            return true;
        }

        public bool Update()
        {
            if (dataAccess.Input.ScrollWheelDifference != 0) startTime = DateTime.Now;
            foreach (Image image in editData.SelectedImages)
            {
                image.Width *= (dataAccess.Input.ScrollWheelDifference / 2000f) + 1;
                image.Width = Math.Max(image.Width, 0.01f);
            }

            bool continueScale = (DateTime.Now - startTime).Milliseconds < 300          // stop when the time is up
                && !dataAccess.Input.IsStrg                                             // stop when the user wants to zoom instead of scale
                && !(dataAccess.Input.IsLeftButtonDown && (!editData.SelectedImages.Contains(editData.ImageUnderMouse) || editData.ImageUnderMouse == null)); // stop when the user clicks somewhere, where no selection is

            if (!continueScale)
            {
                float[] newWidth = new float[editData.SelectedImages.Count];
                for (int i = 0; i < editData.SelectedImages.Count; i++)
                {
                    newWidth[i] = editData.SelectedImages[i].Width;
                }
                Command command = new Command(ExecuteScale, ExecuteScale, newWidth, "Scale Selected Images");
                command.SetUndoData(startWidth);
                editData.UndoManager.AddCommand(command);
            }
            return continueScale;
        }

        public object ExecuteScale(object newWidth)
        {
            for (int i = 0; i < editData.SelectedImages.Count; i++)
            {
                editData.SelectedImages[i].Width = ((float[])newWidth)[i];
            }
            return null;
        }
    }
}
