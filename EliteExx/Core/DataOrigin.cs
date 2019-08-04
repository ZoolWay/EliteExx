using System;

namespace Zw.EliteExx.Core
{
    [Flags]
    public enum DataOrigin
    {
        NotSet = 0,
        Unknown = 1,
        EliteJournal = 2,
        Edsm = 4,
    }
}
