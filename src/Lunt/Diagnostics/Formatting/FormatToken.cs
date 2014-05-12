namespace Lunt.Diagnostics.Formatting
{
    internal abstract class FormatToken
    {
        public abstract string Render(object[] args);
    }
}