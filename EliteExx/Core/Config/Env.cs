using System;

namespace Zw.EliteExx.Core.Config
{
    public class Env
    {
        public string AppFolder { get; }

        public Env(string appFolder)
        {
            this.AppFolder = appFolder;
        }
    }
}
