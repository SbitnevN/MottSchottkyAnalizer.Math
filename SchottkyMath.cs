namespace MottSchottkyAnalizer.Math;

public static class SchottkyMath
{
    /// <summary>
    /// Calculates angular frequency ω = 2πf.
    /// </summary>
    /// <param name="frequency">Frequency in Hz</param>
    /// <returns>Angular frequency in rad/s</returns>
    public static double AngularFrequency(double frequency)
    {
        return 2 * System.Math.PI * frequency;
    }

    /// <summary>
    /// Calculates capacitance using the relation C = -1 / (ω · Im(Z)),
    /// where ω is the angular frequency.
    /// </summary>
    /// <param name="frequency">Signal frequency in Hz.</param>
    /// <param name="impedanceImaginary">Imaginary part of impedance (Im(Z)) in Ohms.</param>
    /// <returns>Capacitance in Farads</returns>
    public static double Capacitance(double frequency, double impedanceImaginary)
    {
        return -1.0d / (AngularFrequency(frequency) * impedanceImaginary);
    }
}
