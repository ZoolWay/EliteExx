﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace Zw.EliteExx.Ui
{
    public class ShellViewModel : Screen, IShell
    {
        private static readonly log4net.ILog log = global::log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ShellViewModel()
        {
            log.Debug("Creating Shell");
        }
    }
}
