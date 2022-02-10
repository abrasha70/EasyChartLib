namespace EasyChartLib.PercentageGraphics
{
    public class Alignment
    {
        public VerticalAlignment Vertical { get; set; } = VerticalAlignment.BelowPoint;
        public HorizontalAlignment Horizontal { get; set; } = HorizontalAlignment.RightToPoint;
    }

    public enum VerticalAlignment
    {
        BelowPoint,
        CenteredToPoint,
        AbovePoint
    }

    public enum HorizontalAlignment
    {
        RightToPoint,
        CenteredToPoint,
        LeftToPoint
    }

}
