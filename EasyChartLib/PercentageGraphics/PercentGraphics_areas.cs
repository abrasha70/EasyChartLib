using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace EasyChartLib.PercentageGraphics
{
    internal partial class PercentGraphics
    {

        public PercentGraphics CreateSubArea(RectangleF rect)
        {
            var actualRect = GetActualRect(rect);
            var actualArea = new PercentGraphics(_bmp, actualRect);
            return actualArea;
        }
        public PercentGraphics CreateSubArea(float x, float y, float width, float height)
        {
            var actualRect = new RectangleF(x, y, width, height);
            return CreateSubArea(actualRect);
        }


        public List<PercentGraphics> HorizontalSplit(float splitSize)
        {
            var parts = new List<PercentGraphics>();
            parts.Add(CreateSubArea(0, 0, splitSize, ScaleHeight));
            parts.Add(CreateSubArea(splitSize, 0, ScaleWidth - splitSize, ScaleHeight));
            return parts;
        }

        public List<PercentGraphics> HorizontalMultiSplit(int count)
        {
            var partWidth = ScaleWidth / count;
            var parts = Enumerable.Range(0, count).Select((index) => CreateSubArea(index * partWidth, 0, partWidth, ScaleHeight));
            return parts.ToList();
        }

        public List<PercentGraphics> HorizontalMultiSplit(List<float?> lengthSizes)
        {
            var resized = AutoResizeLengths(lengthSizes, ScaleWidth);

            var parts = new List<PercentGraphics>();
            var position = 0f;
            foreach (var length in resized)
            {
                var newPart = CreateSubArea(position, 0, length, ScaleHeight);
                position += length;
                parts.Add(newPart);
            }

            return parts;
        }


        public List<PercentGraphics> VerticalSplit(float splitSize)
        {
            var parts = new List<PercentGraphics>();
            parts.Add(CreateSubArea(0, 0, ScaleWidth, splitSize));
            parts.Add(CreateSubArea(0, splitSize, ScaleWidth, ScaleHeight - splitSize));
            return parts;
        }

        public List<PercentGraphics> VerticalMultiSplit(int count)
        {
            var partHeight = ScaleHeight / count;
            var parts = Enumerable.Range(0, count).Select((index) => CreateSubArea(0, index * partHeight, ScaleWidth, partHeight));
            return parts.ToList();
        }

        public List<PercentGraphics> VerticalMultiSplit(List<float?> lengthSizes)
        {
            var resized = AutoResizeLengths(lengthSizes, ScaleHeight);

            var parts = new List<PercentGraphics>();
            var position = 0f;
            foreach (var length in resized)
            {
                var newPart = CreateSubArea(0, position, ScaleWidth, length);
                position += length;
                parts.Add(newPart);
            }

            return parts;
        }


        private List<float> AutoResizeLengths(List<float?> lengthSizes, float scaleSize)
        {
            var newItems = lengthSizes.Select(item => item ?? 0).ToList();

            var total = newItems.Sum(length => length);
            var remainedLength = scaleSize - total;
            if (remainedLength < 0) throw new System.OverflowException("Total length sizes exceeded Scale");


            if (remainedLength > 0)
            {
                var autoResizeIndexes = newItems.Select((length, index) => index).Where(index => newItems[index] == 0).ToList();
                if (autoResizeIndexes.Count == 0) throw new System.ArithmeticException("No auto-resize item for remained length");
                var autoResizeLength = remainedLength / autoResizeIndexes.Count;
                foreach (var index in autoResizeIndexes)
                {
                    newItems[index] = autoResizeLength;
                }
            }

            return newItems;
        }




    }
}
