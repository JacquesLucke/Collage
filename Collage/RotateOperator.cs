using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Collage
{
    public class RotateOperator : IUpdateableCollageOperator
    {
        DataAccess dataAccess;
        CollageEditData editData;

        float[] startRotation, rotationOffset;

        public RotateOperator() { }

        public void SetData(DataAccess dataAccess, CollageEditData editData)
        {
            this.dataAccess = dataAccess;
            this.editData = editData;
        }
        public bool CanStart()
        {
            return dataAccess.Input.IsRightButtonPressed && editData.SelectedImages.Count > 0;
        }

        public bool Start()
        {
            startRotation = new float[editData.SelectedImages.Count];
            rotationOffset = new float[editData.SelectedImages.Count];
            for (int i = 0; i < editData.SelectedImages.Count; i++)
            {
                startRotation[i] = editData.SelectedImages[i].Rotation;
                Vector2 difference = dataAccess.Input.MousePositionVector - editData.SelectedImages[i].GetCenterInBoundary(editData.DrawRectangle.Rectangle);
                rotationOffset[i] = -(float)Math.Atan2(difference.Y, difference.X) + editData.SelectedImages[i].Rotation;
            }
            return true;
        }

        public bool Update()
        {
            foreach(Image image in editData.SelectedImages)
            {
                Vector2 difference = dataAccess.Input.MousePositionVector - image.GetCenterInBoundary(editData.DrawRectangle.Rectangle);
                image.Rotation = (float)Math.Atan2(difference.Y, difference.X) + rotationOffset[editData.SelectedImages.IndexOf(image)];
            }

            bool continueRotation = dataAccess.Input.IsRightButtonDown;
            if(!continueRotation)
            {
                float[] newRotation = new float[editData.SelectedImages.Count];
                for (int i = 0; i < editData.SelectedImages.Count; i++)
            {
                newRotation[i] = editData.SelectedImages[i].Rotation;
            }
                Command command = new Command(ExecuteScale, ExecuteScale, newRotation, "Scale Selected Images");
                command.SetUndoData(startRotation);
                editData.UndoManager.AddCommand(command);
            }
            return continueRotation;
        }

        public object ExecuteScale(object newRotation)
        {
            for (int i = 0; i < editData.SelectedImages.Count; i++)
            {
                editData.SelectedImages[i].Rotation = ((float[])newRotation)[i];
            }
            return null;
        }
    }
}
