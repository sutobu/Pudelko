using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace PudelkoLibrary
{
    public enum UnitOfMeasure
    {
        milimeter,
        centimeter,
        meter
    }

    public class  Pudelko : IFormattable, IEquatable<Pudelko>
    {
        private const double DefaultDimension = 0.1;
        private const double MaxDimension = 10.0;

        private readonly double a, b, c;

        public double A => Math.Round(a, 3);
        public double B => Math.Round(b, 3);
        public double C => Math.Round(c, 3);
    }

    public Pudelko(double? a = null, double? b = null, double? c = null, UnitOfMeasure unit = UnitOfMeasure.meter)
    {
        
        this.a = ConvertToMeters(a ?? DefaultDimension, unit);
        this.b = ConvertToMeters(b ?? DefaultDimension, unit);
        this.c = ConvertToMeters(c ?? DefaultDimension, unit);
        Unit = unit;

        ValidateDimensions(this.a, this.b, this.c);
    }

    private double ConvertToMeters(double value, UnitOfMeasure unit)
    {
        return unit switch
        {
            UnitOfMeasure.milimeter => value / 1000,
            UnitOfMeasure.centimeter => value / 100,
            UnitOfMeasure.meter => value,
            _ => throw new ArgumentOutOfRangeException(nameof(unit), "Invalid unit of measure")
        };
    }

    private void ValidateDimensions(params double[] dimensions )
    {
        foreach(var dimension in dimensions)
        {
            if (dimension <= 0 || dimension > MaxDimension)
                throw new ArgumentOutOfRangeException("Dimensions must be greater than zero and not exceed 10 meters.");
        }
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        if (string.IsNullOrEmpty(format))
            format = "m";

        return format switch
        {
            "m" => $"{A:F3} m × {B:F3} m × {C:F3} m",
            "cm" => $"{(A * 100):F1} cm × {(B * 100):F1} cm × {(C * 100):F1} cm",
            "mm" => $"{(A * 1000):F0} mm × {(B * 1000):F0} mm × {(C * 1000):F0} mm",
            _ => throw new FormatException($"The '{format}' format string is not supported.")
        };
    }

    public double Objetosc => Math.Round(A * B * C, 9);
    public double Pole => Math.Round(2 * (A * B + A * C + B * C), 6);

    public bool Equals(Pudelko other)
    {
        if (other == null) return false;
        
        var dimensions = new[] { A, B, C };
        var otherDimensions = new[] { other.A, other.B, other.C };

        Array.Sort(dimensions);
        Array.Sort(otherDimensions);
        return dimensions.SequenceEqual(otherDimensions);
    }

    public override bool Equals(object obj)
    {
        if (obj is Pudelko other)
            return Equals(other);
        return false;
    }

    public override int GetHashCode()
    {
        var dimensions = new[] { A, B, C };
        Array.Sort(dimensions);
        return HashCode.Combine(dimensions[0], dimensions[1], dimensions[2]);
    }
}




