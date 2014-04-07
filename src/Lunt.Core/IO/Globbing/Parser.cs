// ﻿
// Copyright (c) 2013 Patrik Svensson, Kevin Thompson
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

///////////////////////////////////////////////////////////////////////
// Portions of this code was ported from glob-js by Kevin Thompson.
// https://github.com/kthompson/glob-js
///////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

namespace Lunt.IO.Globbing
{
    internal sealed class Parser
    {
        private readonly Scanner _scanner;
        private readonly IBuildEnvironment _environment;
        private Token _currentToken;

        public Parser(Scanner scanner, IBuildEnvironment environment)
        {
            _scanner = scanner;
            _environment = environment;
            _currentToken = null;
        }

        public List<Node> Parse()
        {
            this.Accept();

            // Parse the root.
            var items = new List<Node> { this.ParseRoot() };
            if (items.Count == 1 && items[0] is RelativeRoot)
            {
                items.Add(this.ParseSegment());
            }

            // Parse all path segments.
            while (_currentToken.Kind == TokenKind.PathSeparator)
            {
                this.Accept();
                items.Add(this.ParseSegment());
            }

            // Not an end of text token?
            if (_currentToken.Kind != TokenKind.EndOfText)
            {
                throw new InvalidOperationException("Expected EOT");
            }

            // Return the path node.
            return items;
        }

        private RootNode ParseRoot()
        {
            if (_environment.IsUnix())
            {
                // Starts with a separator?
                if (_currentToken.Kind == TokenKind.PathSeparator)
                {
                    return new UnixRoot();
                }
            }
            else
            {
                // Starts with a separator?
                if (_currentToken.Kind == TokenKind.PathSeparator)
                {
                    if (_scanner.Peek().Kind == TokenKind.PathSeparator)
                    {
                        throw new LuntException("UNC paths are not supported.");
                    }
                    return new WindowsRoot(string.Empty);
                }

                // Is this a drive?
                if (_currentToken.Kind == TokenKind.Identifier &&
                    _currentToken.Value.Length == 1 &&
                    _scanner.Peek().Kind == TokenKind.WindowsRoot)
                {
                    var identifier = this.ParseIdentifier();
                    this.Accept(TokenKind.WindowsRoot);
                    return new WindowsRoot(identifier.Identifier);
                }
            }

            // Starts with an identifier?
            if (_currentToken.Kind == TokenKind.Identifier)
            {
                // Is the identifer indicating a current directory?
                if (_currentToken.Value == ".")
                {
                    this.Accept();
                    if (_currentToken.Kind != TokenKind.PathSeparator)
                    {
                        throw new InvalidOperationException();
                    }
                    this.Accept();
                }
                return new RelativeRoot();
            }

            throw new NotImplementedException();
        }

        private Node ParseSegment()
        {
            if (_currentToken.Kind == TokenKind.DirectoryWildcard)
            {
                this.Accept();
                return new WildcardSegmentNode();
            }

            var items = new List<Node>();
            while (true)
            {
                switch (_currentToken.Kind)
                {
                    case TokenKind.Identifier:
                    case TokenKind.CharacterWildcard:
                    case TokenKind.Wildcard:
                        items.Add(this.ParseSubSegment());
                        continue;
                }
                break;
            }
            return new SegmentNode(items);
        }

        private Node ParseSubSegment()
        {
            switch (_currentToken.Kind)
            {
                case TokenKind.Identifier:
                    return this.ParseIdentifier();
                case TokenKind.CharacterWildcard:
                case TokenKind.Wildcard:
                    return this.ParseWildcard(_currentToken.Kind);
            }

            throw new NotSupportedException("Unable to parse sub segment.");
        }

        private IdentifierNode ParseIdentifier()
        {
            if (_currentToken.Kind == TokenKind.Identifier)
            {
                var identifier = new IdentifierNode(_currentToken.Value);
                this.Accept();
                return identifier;
            }
            throw new InvalidOperationException("Unable to parse identifier.");
        }

        private Node ParseWildcard(TokenKind kind)
        {
            this.Accept(kind);
            return new WildcardNode(kind);
        }

        private void Accept(TokenKind kind)
        {
            if (_currentToken.Kind == kind)
            {
                this.Accept();
                return;
            }
            throw new InvalidOperationException("Unexpected token kind.");
        }

        private void Accept()
        {
            _currentToken = _scanner.Scan();
        }
    }
}
