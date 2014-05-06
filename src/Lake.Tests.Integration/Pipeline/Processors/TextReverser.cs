using System;
using System.ComponentModel;
using Lunt;

namespace Lake.Tests.Integration.Pipeline
{
    [DisplayName("Text Reverser")]
    public class TextReverser : LuntProcessor<string>
    {
        public override string Process(LuntContext context, string value)
        {
            char[] charArray = value.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}