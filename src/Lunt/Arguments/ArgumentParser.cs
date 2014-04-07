// ﻿
// Copyright (c) 2013 Patrik Svensson
// 
// This file is part of Lunt.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using Lunt.Diagnostics;
using Lunt.IO;

namespace Lunt.Arguments
{
    using Lunt.Diagnostics;
    using Lunt.IO;

    internal sealed class ArgumentParser : IArgumentParser
    {
        private readonly IBuildLog _log;
        private readonly List<ArgumentDescription> _descriptions;

        public ArgumentParser(IBuildLog log)
        {
            _log = log;
            _descriptions = new List<ArgumentDescription>();

            this.AddOption(new[] {"input", "i"},
                @"-input=target", false,
                @"Sets the input directory.",
                o => o.InputDirectory);

            this.AddOption(new[] {"output", "o"},
                @"-output=target", false,
                @"Sets the output directory.",
                o => o.OutputDirectory);

            this.AddOption(new[] { "probing", "p" },
                @"-probing=target", false,
                @"Sets the assembly probing directory.",
                o => o.ProbingDirectory);

            this.AddOption(new[] {"verbosity", "v"},
                @"-verbosity=value", false,
                @"Specifies the amount of information to display.",
                o => o.Verbosity);

            this.AddOption(new[] {"help", "h", "?"},
                "-help", false,
                @"Displays usage information.",
                o => o.ShowHelp);

            this.AddOption(new[] {"version", "ver"},
                "-version", false,
                @"Displays version information.",
                o => o.ShowVersion);

            this.AddOption(new[] {"rebuild", "r"},
                "-rebuild", false,
                @"Performs a non incremental build.",
                o => o.Rebuild);
        }

        public LuntOptions Parse(string[] args)
        {
            var options = new LuntOptions();
            var isParsingOptions = true;

            foreach (var arg in args)
            {
                if (isParsingOptions)
                {
                    if (IsOption(arg))
                    {
                        if (!this.ParseOption(arg, options))
                        {
                            return null;
                        }
                        continue;
                    }

                    isParsingOptions = false;

                    if (options.BuildConfiguration != null)
                    {
                        _log.Error("More than one build configuration specified.");
                        return null;
                    }

                    // Quoted?
                    var buildConfiguration = arg;
                    if (buildConfiguration.StartsWith("\"") && buildConfiguration.EndsWith("\""))
                    {
                        buildConfiguration = buildConfiguration.Trim('"');
                    }

                    options.BuildConfiguration = new FilePath(buildConfiguration);
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

        private bool ParseOption(string arg, LuntOptions options)
        {
            string name, value;

            int separatorIndex = arg.IndexOfAny(new[] {'='});
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
                        object convertedValue = converter.ConvertFromInvariantString(value);
                        command.Property.SetValue(options, convertedValue, null);
                        return true;
                    }
                }
            }

            _log.Error("Unknown option: {0}", name);
            return false;
        }

        private void AddOption<TValue>(string[] names, string parameter, bool required, string description, Expression<Func<LuntOptions, TValue>> action)
        {
            var property = GetProperty(action);
            _descriptions.Add(new ArgumentDescription(names, parameter, required, description, property));
        }

        private static PropertyInfo GetProperty<TValue>(Expression<Func<LuntOptions, TValue>> expression)
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