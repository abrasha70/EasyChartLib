using System.Drawing;

namespace EasyChartLib
{
    internal class LoadedSettings
    {
        public ChartSettings Raw { get; private set; }
        public Font Font { get; set; }


        public LoadedSettings(ChartSettings chartSettings)
        {
            Raw = chartSettings;
            Font = new Font(SystemFonts.DefaultFont.FontFamily, chartSettings.FontSize);
        }


    }



}