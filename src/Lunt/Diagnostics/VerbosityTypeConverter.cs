using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace Lunt.Diagnostics
{
    internal sealed class VerbosityTypeConverter : TypeConverter
    {
        private readonly Dictionary<string, Verbosity> _lookup;

        public VerbosityTypeConverter()
        {
            _lookup = new Dictionary<string, Verbosity>(StringComparer.OrdinalIgnoreCase);
            _lookup.Add("q", Verbosity.Quiet);
            _lookup.Add("quiet", Verbosity.Quiet);
            _lookup.Add("m", Verbosity.Minimal);
            _lookup.Add("minimal", Verbosity.Minimal);
            _lookup.Add("n", Verbosity.Normal);
            _lookup.Add("normal", Verbosity.Normal);
            _lookup.Add("v", Verbosity.Verbose);
            _lookup.Add("verbose", Verbosity.Verbose);
            _lookup.Add("d", Verbosity.Diagnostic);
            _lookup.Add("diagnostic", Verbosity.Diagnostic);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof (string))
            {
                return true;
            }
            return false;
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value != null)
            {
                var stringValue = value as string;
                if (stringValue != null)
                {
                    Verbosity verbosity;
                    if (_lookup.TryGetValue(stringValue, out verbosity))
                    {
                        return verbosity;
                    }
                }
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}