﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Collage
{
    public class AutoPositonOperator : ICollageOperator
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
            return false;
        }

        public bool Start()
        {
            return false;
        }
    }
}
