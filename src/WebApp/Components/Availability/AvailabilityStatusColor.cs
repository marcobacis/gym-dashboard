using ApexCharts;

namespace WebApp.Components.UI;

public class AvailabilityStatusColor
{
    private const double MaxSeats = 128.0;
    
    private static readonly List<(double, string)> Ranges = new()
    {
        (30 / MaxSeats, "#FF0000"),    // Red
        (80 / MaxSeats, "#FFA500"),   // Orange
        (1, "#00FF00")     // Green
    };

    public static List<PlotOptionsHeatmapColorScaleRange> GetHeatMapRanges()
    {
        var ranges = new List<PlotOptionsHeatmapColorScaleRange>();

        ranges.Add(new PlotOptionsHeatmapColorScaleRange
        {
            From = -10,
            To = -1,
            Color = "#FFFFFF"
        });
        for (int i = 0; i < 130; i += 5)
        {
            ranges.Add(new PlotOptionsHeatmapColorScaleRange
            {
                From = i,
                To = i + 5,
                Color = AvailabilityStatusColor.GetColorForAvailability(i)
            });
        }

        return ranges;
    }
    

    public static string GetColorForAvailability(double seats)
    {
        double t = Math.Clamp(seats / MaxSeats, 0.0, 1.0);
        return InterpolateColor(t);
    }
    
    private static string InterpolateColor(double t)
    {
        if (t <= Ranges[0].Item1)
        {
            return Ranges[0].Item2;
        }
        for (int i = 0; i < Ranges.Count - 1; i++)
        {
            var (t1, c1) = Ranges[i];
            var (t2, c2) = Ranges[i + 1];

            if (t >= t1 && t <= t2)
            {
                double localT = (t - t1) / (t2 - t1);
                return ColorBlend(c1, c2, localT);
            }
        }

        return Ranges.Last().Item2;
    }

    private static string ColorBlend(string c1, string c2, double t)
    {
        int r1 = Convert.ToInt32(c1.Substring(1, 2), 16);
        int g1 = Convert.ToInt32(c1.Substring(3, 2), 16);
        int b1 = Convert.ToInt32(c1.Substring(5, 2), 16);

        int r2 = Convert.ToInt32(c2.Substring(1, 2), 16);
        int g2 = Convert.ToInt32(c2.Substring(3, 2), 16);
        int b2 = Convert.ToInt32(c2.Substring(5, 2), 16);

        int r = (int)(r1 + (r2 - r1) * t);
        int g = (int)(g1 + (g2 - g1) * t);
        int b = (int)(b1 + (b2 - b1) * t);

        return $"#{r:X2}{g:X2}{b:X2}";
    }
}