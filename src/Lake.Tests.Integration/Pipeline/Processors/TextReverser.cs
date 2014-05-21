using System;
using System.ComponentModel;
using Lunt;

namespace Lake.Tests.Integration.Pipeline
{
    [DisplayName("Text Reverser")]
    public class TextReverser : Processor<string>
    {
        public override string Process(Context context, string value)
        {
            var charArray = value.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}