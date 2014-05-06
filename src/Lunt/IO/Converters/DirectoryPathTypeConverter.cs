using System;
using System.ComponentModel;
using System.Globalization;

namespace Lunt.IO
{
    internal sealed class DirectoryPathTypeConverter : TypeConverter
    {
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
                    return new DirectoryPath(stringValue);
                }
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}