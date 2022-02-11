using System;

namespace EasyChartLib.PercentageGraphics
{
    internal class ActualMargin
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }

        public ActualMargin(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }
        public ActualMargin(int margin)
        {
            Left = margin;
            Top = margin;
            Right = margin;
            Bottom = margin;
        }

        public int GetWidthMargin()
        {
            return Left + Right;
        }

        public int GetHeightMargin()
        {
            return Top + Bottom;
        }
    }
}