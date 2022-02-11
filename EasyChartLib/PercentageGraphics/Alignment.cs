namespace EasyChartLib.PercentageGraphics
{
    internal class Alignment
    {
        public VerticalAlignment Vertical { get; set; } = VerticalAlignment.BelowPoint;
        public HorizontalAlignment Horizontal { get; set; } = HorizontalAlignment.RightToPoint;
    }

    internal enum VerticalAlignment
    {
        BelowPoint,
        CenteredToPoint,
        AbovePoint
    }

    internal enum HorizontalAlignment
    {
        RightToPoint,
        CenteredToPoint,
        LeftToPoint
    }

}
