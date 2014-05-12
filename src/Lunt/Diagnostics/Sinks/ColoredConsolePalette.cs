using System;
using System.Collections.Generic;

namespace Lunt.Diagnostics
{
    internal class ColoredConsolePalette
    {
        public ConsoleColor Background { get; set; }
        public ConsoleColor Foreground { get; set; }
        public ConsoleColor ArgumentBackground { get; set; }
        public ConsoleColor ArgumentForeground { get; set; }

        public ColoredConsolePalette(ConsoleColor background, ConsoleColor foreground, 
            ConsoleColor argumentBackground, ConsoleColor argumentForeground)
        {
            Background = background;
            Foreground = foreground;
            ArgumentBackground = argumentBackground;
            ArgumentForeground = argumentForeground;
        }

        public static IDictionary<LogLevel, ColoredConsolePalette> GetColorfulPalette()
        {
            var palette = new Dictionary<LogLevel, ColoredConsolePalette>();
            palette.Add(LogLevel.Error, new ColoredConsolePalette(ConsoleColor.DarkRed, ConsoleColor.White, ConsoleColor.Red, ConsoleColor.White));
            palette.Add(LogLevel.Warning, new ColoredConsolePalette(ConsoleColor.Black, ConsoleColor.Yellow, ConsoleColor.Black, ConsoleColor.Yellow));
            palette.Add(LogLevel.Information, new ColoredConsolePalette(ConsoleColor.Black, ConsoleColor.White, ConsoleColor.DarkBlue, ConsoleColor.White));
            palette.Add(LogLevel.Verbose, new ColoredConsolePalette(ConsoleColor.Black, ConsoleColor.Gray, ConsoleColor.Black, ConsoleColor.White));
            palette.Add(LogLevel.Debug, new ColoredConsolePalette(ConsoleColor.Black, ConsoleColor.DarkGray, ConsoleColor.Black, ConsoleColor.Gray));
            return palette;
        }

        public static IDictionary<LogLevel, ColoredConsolePalette> GetGreyscalePalette()
        {
            var palette = new Dictionary<LogLevel, ColoredConsolePalette>();
            palette.Add(LogLevel.Error, new ColoredConsolePalette(ConsoleColor.Black, ConsoleColor.Gray, ConsoleColor.Black, ConsoleColor.White));
            palette.Add(LogLevel.Warning, new ColoredConsolePalette(ConsoleColor.Black, ConsoleColor.Gray, ConsoleColor.Black, ConsoleColor.White));
            palette.Add(LogLevel.Information, new ColoredConsolePalette(ConsoleColor.Black, ConsoleColor.Gray, ConsoleColor.Black, ConsoleColor.White));
            palette.Add(LogLevel.Verbose, new ColoredConsolePalette(ConsoleColor.Black, ConsoleColor.Gray, ConsoleColor.Black, ConsoleColor.White));
            palette.Add(LogLevel.Debug, new ColoredConsolePalette(ConsoleColor.Black, ConsoleColor.Gray, ConsoleColor.Black, ConsoleColor.White));
            return palette;
        }
    }
}
