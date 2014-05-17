using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using Lunt.Diagnostics;
using Lunt.IO;

namespace Lake.Arguments
{
    internal sealed class ArgumentParser : IArgumentParser
    {
        private readonly IBuildLog _log;
        private readonly List<ArgumentDescription> _descriptions;

        public ArgumentParser(IBuildLog log)
        {
            _log = log;
            _descriptions = new List<ArgumentDescription>();

            AddOption(new[] {"input", "i"},
                @"-input=target", false,
                @"Sets the input directory.",
                o => o.InputDirectory);

            AddOption(new[] {"output", "o"},
                @"-output=target", false,
                @"Sets the output directory.",
                o => o.OutputDirectory);

            AddOption(new[] { "probing", "p" },
                @"-probing=target", false,
                @"Sets the assembly probing directory.",
                o => o.ProbingDirectory);

            AddOption(new[] {"verbosity", "v"},
                @"-verbosity=value", false,
                @"Specifies the amount of information to display.",
                o => o.Verbosity);

            AddOption(new[] {"help", "h", "?"},
                "-help", false,
                @"Displays usage information.",
                o => o.ShowHelp);

            AddOption(new[] {"version", "ver"},
                "-version", false,
                @"Displays version information.",
                o => o.ShowVersion);

            AddOption(new[] {"rebuild", "r"},
                "-rebuild", false,
                @"Performs a non incremental build.",
                o => o.Rebuild);

            AddOption(new[] { "colors", "c" },
                "-colors", false,
                @"Outputs the build log with colors.",
                o => o.Colors);
        }

        public LakeOptions Parse(IEnumerable<string> args)
       {
            var options = new LakeOptions();
            var isParsingOptions = true;

            foreach (var arg in args)
            {
                if (isParsingOptions)
                {
                    if (IsOption(arg))
                    {
                        if (!ParseOption(arg, options))
                        {
                            return null;
                        }
                        continue;
                    }

                    isParsingOptions = false;

                    // Quoted?
                    var buildConfiguration = arg;
                    if (buildConfiguration.StartsWith("\"", StringComparison.OrdinalIgnoreCase) 
                        && buildConfiguration.EndsWith("\"", StringComparison.OrdinalIgnoreCase))
                    {
                        buildConfiguration = buildConfiguration.Trim('"');
                    }

                    options.BuildConfiguration = new FilePath(buildConfiguration);
                }
                else
                {
                    if (options.BuildConfiguration != null)
                    {
                        _log.Error("More than one build configuration specified.");
                        return null;
                    }                    
                }
            }

            return options;
        }

        private static bool IsOption(string arg)
        {
            if (string.IsNullOrWhiteSpace(arg))
            {
                return false;
            }
            if (arg[0] != '-')
            {
                return false;
            }
            return true;
        }

        private bool ParseOption(string arg, LakeOptions options)
        {
            string name, value;

            var separatorIndex = arg.IndexOfAny(new[] {'='});
            if (separatorIndex < 0)
            {
                name = arg.Substring(1);
                value = string.Empty;
            }
            else
            {
                name = arg.Substring(1, separatorIndex - 1);
                value = arg.Substring(separatorIndex + 1);
            }

            if (value.Length > 2)
            {
                if (value[0] == '\"' && value[value.Length - 1] == '\"')
                {
                    value = value.Substring(1, value.Length - 2);
                }
            }

            foreach (var command in _descriptions)
            {
                foreach (var @switch in command.Names)
                {
                    if (@switch.Equals(name, StringComparison.OrdinalIgnoreCase))
                    {
                        // Is the property a boolean and this is a switch?
                        if (command.Property.PropertyType == typeof (bool) && string.IsNullOrWhiteSpace(value))
                        {
                            command.Property.SetValue(options, true, null);
                            return true;
                        }

                        var converter = TypeDescriptor.GetConverter(command.Property.PropertyType);
                        if (!converter.CanConvertFrom(typeof (string)))
                        {
                            const string format = "Cannot convert '{0}' to an instance of {1}.";
                            _log.Error(format, value, command.Property.PropertyType.FullName);
                            return false;
                        }
                        var convertedValue = converter.ConvertFromInvariantString(value);
                        command.Property.SetValue(options, convertedValue, null);
                        return true;
                    }
                }
            }

            _log.Error("Unknown option: {0}", name);
            return false;
        }

        private void AddOption<TValue>(string[] names, string parameter, bool required, string description, Expression<Func<LakeOptions, TValue>> action)
        {
            var property = GetProperty(action);
            _descriptions.Add(new ArgumentDescription(names, parameter, required, description, property));
        }

        private static PropertyInfo GetProperty<TValue>(Expression<Func<LakeOptions, TValue>> expression)
        {
            if (expression.Body.NodeType == ExpressionType.MemberAccess)
            {
                var member = ((MemberExpression) expression.Body).Member as PropertyInfo;
                if (member != null)
                {
                    return member;
                }
            }
            if (expression.Body.NodeType == ExpressionType.Convert
                && ((UnaryExpression) expression.Body).Operand.NodeType == ExpressionType.MemberAccess)
            {
                var member = ((MemberExpression) ((UnaryExpression) expression.Body).Operand).Member as PropertyInfo;
                if (member != null)
                {
                    return member;
                }
            }
            throw new ArgumentException("Argument 'expression' is malformed.", "expression");
        }
    }
}