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

        public float FullScale { get; set; } = 100f;


        public PercentGraphics(Bitmap bmp, RectangleF workingArea)
        {
            _bmp = bmp;
            _gfx = Graphics.FromImage(bmp);
            _gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            _workingArea = workingArea;
        }

        public PercentGraphics(Bitmap bmp)
        {
            _bmp = bmp;
            _gfx = Graphics.FromImage(bmp);
            _gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            _workingArea = new Rectangle(0, 0, bmp.Width, bmp.Height);
        }

        public PercentGraphics(Bitmap bmp, ActualMargin margin)
        {
            _bmp = bmp;
            _gfx = Graphics.FromImage(bmp);
            _gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            _workingArea = margin.GetMarginedRectangle(bmp.Size);
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
            return GetActualPoint(point.X, point.Y);
        }

        private PointF GetActualPoint(float x, float y)
        {
            var actualX = _workingArea.Left + _workingArea.Width * x / FullScale;
            var actualY = _workingArea.Top + _workingArea.Height * y / FullScale;
            return new PointF(actualX, actualY);
        }

        private SizeF GetActualSize(SizeF size)
        {
            var actualWidth = GetActualWidth(size.Width);
            var actualHeight = GetActualHeight(size.Height);
            var actualSize = new SizeF(actualWidth, actualHeight);
            return actualSize;
        }

        private SizeF GetRelativeSize(SizeF size)
        {
            var relWidth = size.Width / _workingArea.Width * FullScale;
            var relHeight = size.Height / _workingArea.Height * FullScale;
            var relSize = new SizeF(relWidth, relHeight);
            return relSize;
        }

        private float GetActualWidth(float width)
        {
            return _workingArea.Width * width / FullScale;
        }

        private float GetActualHeight(float height)
        {
            return _workingArea.Height * height / FullScale;
        }


    }
}
