namespace MottSchottkyAnalizer.Math;

public static class Approximation
{
    /// <summary>
    /// Calculates linear least squares approximation y = a·x + b.
    /// </summary>
    /// <param name="points">Input data points</param>
    /// <returns>Linear fit result with coefficients and errors</returns>
    public static LinearFitResult FitLine(IEnumerable<DataPoint> points)
    {
        int n = points.Count();

        if (n < 3)
        {
            throw new ArgumentException("At least 3 points are required", nameof(points));
        }

        double sumX = points.Sum(p => p.X);
        double sumY = points.Sum(p => p.Y);
        double sumXY = points.Select(p => p.X * p.Y).Sum();
        double sumX2 = points.Select(p => p.X * p.X).Sum();

        double a = CalculateSlope(n, sumX, sumY, sumXY, sumX2);
        double b = CalculateIntercept(n, sumX, sumY, a);

        double meanX = sumX / n;

        double rss = CalculateResidualSum(points, a, b);
        double sxx = CalculateSxx(points, meanX);

        double s = FitError(rss, n);
        double sa = SlopeError(s, sxx);
        double sb = InterceptError(s, n, meanX, sxx);

        return new LinearFitResult
        {
            Slope = a,
            Intercept = b,
            FitError = s,
            SlopeError = sa,
            InterceptError = sb
        };
    }

    /// <summary>
    /// Calculates slope a.
    /// </summary>
    public static double CalculateSlope(int n, double sumX, double sumY, double sumXY, double sumX2)
    {
        double denominator = n * sumX2 - sumX * sumX;

        if (System.Math.Abs(denominator) < 1e-20)
        {
            throw new InvalidOperationException("Degenerate data: cannot compute slope");
        }

        return (n * sumXY - sumX * sumY) / denominator;
    }

    /// <summary>
    /// Calculates intercept b.
    /// </summary>
    public static double CalculateIntercept(int n, double sumX, double sumY, double slope)
    {
        return (sumY - slope * sumX) / n;
    }

    /// <summary>
    /// Calculates sum of X.
    /// </summary>
    public static double SumX(DataPoint[] points)
    {
        double sum = 0.0;

        for (int i = 0; i < points.Length; i++)
        {
            sum += points[i].X;
        }

        return sum;
    }

    /// <summary>
    /// Calculates sum of Y.
    /// </summary>
    public static double SumY(DataPoint[] points)
    {
        double sum = 0.0;

        for (int i = 0; i < points.Length; i++)
        {
            sum += points[i].Y;
        }

        return sum;
    }

    /// <summary>
    /// Calculates sum of X*Y.
    /// </summary>
    public static double SumXY(DataPoint[] points)
    {
        double sum = 0.0;

        for (int i = 0; i < points.Length; i++)
        {
            sum += points[i].X * points[i].Y;
        }

        return sum;
    }

    /// <summary>
    /// Calculates sum of X^2.
    /// </summary>
    public static double SumX2(DataPoint[] points)
    {
        double sum = 0.0;

        for (int i = 0; i < points.Length; i++)
        {
            sum += points[i].X * points[i].X;
        }

        return sum;
    }

    /// <summary>
    /// Calculates residual sum of squares.
    /// </summary>
    public static double CalculateResidualSum(IEnumerable<DataPoint> points, double a, double b)
    {
        double rss = 0.0;

        foreach (DataPoint point in points)
        {
            double yCalc = a * point.X + b;
            double residual = point.Y - yCalc;

            rss += residual * residual;
        }

        return rss;
    }

    /// <summary>
    /// Calculates Sxx = Σ(x - meanX)^2.
    /// </summary>
    public static double CalculateSxx(IEnumerable<DataPoint> points, double meanX)
    {
        double sxx = 0.0;

        foreach (DataPoint point in points)
        {
            double dx = point.X - meanX;
            sxx += dx * dx;
        }

        return sxx;
    }

    /// <summary>
    /// Calculates fit error (standard deviation of residuals).
    /// </summary>
    public static double FitError(double rss, int n)
    {
        return System.Math.Sqrt(rss / (n - 2));
    }

    /// <summary>
    /// Calculates slope error.
    /// </summary>
    public static double SlopeError(double fitError, double sxx)
    {
        return fitError / System.Math.Sqrt(sxx);
    }

    /// <summary>
    /// Calculates intercept error.
    /// </summary>
    public static double InterceptError(double fitError, int n, double meanX, double sxx)
    {
        return fitError * System.Math.Sqrt(1.0 / n + (meanX * meanX) / sxx);
    }
}
