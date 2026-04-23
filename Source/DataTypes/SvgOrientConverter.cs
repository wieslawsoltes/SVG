using System;
using System.ComponentModel;
using System.Globalization;
using Svg;

namespace Svg.DataTypes
{
    public sealed class SvgOrientConverter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value == null)
                return new SvgOrient();

            if (!(value is string))
                throw new ArgumentOutOfRangeException("value must be a string.");

            switch (value.ToString())
            {
                case "auto":
                    return new SvgOrient(true);
                case "auto-start-reverse":
                    return new SvgOrient(true, true);
                default:
                    var rawValue = value.ToString().Trim();
                    var suffix = rawValue.EndsWith("grad", StringComparison.OrdinalIgnoreCase)
                        ? "grad"
                        : rawValue.EndsWith("deg", StringComparison.OrdinalIgnoreCase)
                            ? "deg"
                            : rawValue.EndsWith("rad", StringComparison.OrdinalIgnoreCase)
                                ? "rad"
                                : string.Empty;
                    var numericValue = suffix.Length == 0
                        ? rawValue
                        : rawValue.Substring(0, rawValue.Length - suffix.Length);

                    float fTmp;
                    if (!float.TryParse(numericValue, NumberStyles.Float, CultureInfo.InvariantCulture, out fTmp))
                        throw new ArgumentOutOfRangeException("value must be a valid float.");

                    var angle = suffix switch
                    {
                        "rad" => (float)(fTmp * (180d / Math.PI)),
                        "grad" => fTmp * 0.9f,
                        _ => fTmp
                    };

                    return new SvgOrient(angle)
                    {
                        RawValue = rawValue
                    };
            }
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
