﻿using System.Collections.Generic;

namespace Collage
{
    class SelectAllOperator : ICollageOperator
    {
        DataAccess dataAccess;
        CollageEditData editData;

        public SelectAllOperator() { }

        public void SetData(DataAccess dataAccess, CollageEditData editData)
        {
            this.dataAccess = dataAccess;
            this.editData = editData;
        }

        public bool Start()
        {
            Command command = new Command(ExecuteSelectAll, ExecuteUndoSelectAll, null, "Select All");
            editData.UndoManager.ExecuteAndAddCommand(command);
            return false;
        }

        public object ExecuteSelectAll(object empty)
        {
            List<Image> oldSelection = new List<Image>();
            oldSelection.AddRange(editData.SelectedImages);

            editData.SelectedImages.Clear();
            if(oldSelection.Count != editData.Collage.Images.Count) editData.SelectedImages.AddRange(editData.Collage.Images); // deselect all when all are selected

            return oldSelection;
        }
        public object ExecuteUndoSelectAll(object oldSelection)
        {
            editData.SelectedImages.Clear();
            editData.SelectedImages.AddRange((List<Image>)oldSelection);

            return null;
        }
    }
}
