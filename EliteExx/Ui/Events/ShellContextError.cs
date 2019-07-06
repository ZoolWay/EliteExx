using System;

namespace Zw.EliteExx.Ui.Events
{
    public class ShellContextError
    {
        public string Message { get; }
        public string Origin { get; }

        public ShellContextError(string message, string origin)
        {
            this.Message = message;
            this.Origin = origin;
        }
    }
}
