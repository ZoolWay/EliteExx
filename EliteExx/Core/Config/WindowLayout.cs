using System;

namespace Zw.EliteExx.Core.Config
{
    public class WindowLayout
    {
        public double? Left { get; }
        public double? Top { get; }
        public double? Width { get; }
        public double? Height { get; }

        public WindowLayout(double? left, double? top, double? width, double? height)
        {
            this.Left = left;
            this.Top = top;
            this.Width = width;
            this.Height = height;
        }
    }
}
