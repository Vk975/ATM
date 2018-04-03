﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATM.Web.Models
{
    public class DisposableBase : IDisposable
    {
        public void Dispose()
        {
            if (this != null)
                GC.SuppressFinalize(this);
        }


    }
}
