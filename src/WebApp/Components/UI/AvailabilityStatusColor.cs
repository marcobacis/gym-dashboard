namespace WebApp.Components.UI;

public class AvailabilityStatusColor
{
    private const double _maxSeats = 125.0;
    
    private static readonly List<(double, string)> _ranges = new()
    {
        (30 / 125.0, "#FF0000"),    // Red
        (80 / 125.0, "#FFA500"),   // Orange
        (1, "#00FF00")     // Green
    };
    
    public static string GetColorForAvailability(double seats)
    {
        double t = Math.Clamp(seats / _maxSeats, 0.0, 1.0);
        return InterpolateColor(t);
    }
    
    private static string InterpolateColor(double t)
    {
        if (t <= _ranges[0].Item1)
        {
            return _ranges[0].Item2;
        }
        for (int i = 0; i < _ranges.Count - 1; i++)
        {
            var (t1, c1) = _ranges[i];
            var (t2, c2) = _ranges[i + 1];

            if (t >= t1 && t <= t2)
            {
                double localT = (t - t1) / (t2 - t1);
                return ColorBlend(c1, c2, localT);
            }
        }

        return _ranges.Last().Item2;
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