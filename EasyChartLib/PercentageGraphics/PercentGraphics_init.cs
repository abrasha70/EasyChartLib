using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace EasyChartLib.PercentageGraphics
{
    internal partial class PercentGraphics
    {
        private readonly Bitmap _bmp;
        private readonly Graphics _gfx;
        private readonly RectangleF _workingArea;

        public float ScaleWidth { get; set; } = 100f;
        public float ScaleHeight { get; set; } = 100f;


        public PercentGraphics(Bitmap bmp, RectangleF workingArea)
        {
            _bmp = bmp;
            _gfx = Graphics.FromImage(bmp);
            _workingArea = workingArea;
        }

        public PercentGraphics(Bitmap bmp)
        {
            _bmp = bmp;
            _gfx = Graphics.FromImage(bmp);
            _workingArea = new Rectangle(0, 0, bmp.Width, bmp.Height);
        }

        public PercentGraphics(Bitmap bmp, ActualMargin margin)
        {
            _bmp = bmp;
            _gfx = Graphics.FromImage(bmp);
            _workingArea = new RectangleF(margin.Left, margin.Top, bmp.Width - margin.GetWidthMargin(), bmp.Height - margin.GetHeightMargin());
        }




        private RectangleF GetActualRect(RectangleF rect)
        {
            var actualLoc = GetActualPoint(rect.Location);
            var actualSize = GetActualSize(rect.Size);
            var actualRect = new RectangleF(actualLoc, actualSize);
            return actualRect;
        }

        private PointF GetActualPoint(PointF point)
        {
            var actualX = _workingArea.Left + _workingArea.Width * point.X / ScaleWidth;
            var actualY = _workingArea.Top + _workingArea.Height * point.Y / ScaleHeight;
            return new PointF(actualX, actualY);
        }

        private SizeF GetActualSize(SizeF size)
        {
            var actualWidth = _workingArea.Width * size.Width / ScaleWidth;
            var actualHeight = _workingArea.Height * size.Height / ScaleHeight;
            var actualSize = new SizeF(actualWidth, actualHeight);
            return actualSize;
        }

        private SizeF GetRelativeSize(SizeF size)
        {
            var relWidth = size.Width / _workingArea.Width * ScaleWidth;
            var relHeight = size.Height / _workingArea.Height * ScaleHeight;
            var relSize = new SizeF(relWidth, relHeight);
            return relSize;
        }
    }
}
