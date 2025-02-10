namespace EasyChartLib
{

    internal class DirectionObj
    {
        public EDirection Direction { get; private set; }

        public DirectionObj(EDirection direction)
        {
            Direction = direction;
        }

        public bool IsVertical
        {
            get
            {
                if (Direction == EDirection.BottomToTop) return true;
                if (Direction == EDirection.TopToBottom) return true;
                return false;
            }
        }

        public bool IsReversed
        {
            get
            {
                if (Direction == EDirection.BottomToTop) return true;
                if (Direction == EDirection.RightToLeft) return true;
                return false;
            }
        }
    }
}
