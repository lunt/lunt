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
using System.Globalization;
using System.Text.RegularExpressions;

namespace Lunt.IO.Globbing
{
    internal sealed class Scanner
    {
        private readonly string _pattern;
        private readonly Regex _identifierRegex;
        private int _sourceIndex;
        private char _currentCharacter;
        private string _currentContent;
        private TokenKind _currentKind;

        public Scanner(string pattern)
        {
            if (pattern == null)
            {
                throw new ArgumentNullException("pattern");
            }
            _pattern = pattern;
            _sourceIndex = 0;
            _currentContent = string.Empty;
            _currentCharacter = _pattern[_sourceIndex];
            _identifierRegex = new Regex("^[0-9a-zA-Z\\. _-]$", RegexOptions.Compiled);
        }

        public Token Scan()
        {
            _currentContent = string.Empty;
            _currentKind = ScanToken();

            return new Token(_currentKind, _currentContent);
        }

        public Token Peek()
        {
            var index = _sourceIndex;
            var token = Scan();
            _sourceIndex = index;
            _currentCharacter = _pattern[_sourceIndex];
            return token;
        }

        private TokenKind ScanToken()
        {
            if (IsAlphaNumberic(_currentCharacter))
            {
                while (IsAlphaNumberic(_currentCharacter))
                {
                    TakeCharacter();
                }
                return TokenKind.Identifier;
            }

            if (_currentCharacter == '*')
            {
                TakeCharacter();
                if (_currentCharacter == '*')
                {
                    TakeCharacter();
                    return TokenKind.DirectoryWildcard;
                }
                return TokenKind.Wildcard;
            }
            if (_currentCharacter == '?')
            {
                TakeCharacter();
                return TokenKind.CharacterWildcard;
            }
            if (_currentCharacter == '/' || _currentCharacter == '\\')
            {
                TakeCharacter();
                return TokenKind.PathSeparator;
            }
            if (_currentCharacter == ':')
            {
                TakeCharacter();
                return TokenKind.WindowsRoot;
            }
            if (_currentCharacter == '\0')
            {
                return TokenKind.EndOfText;
            }

            throw new NotSupportedException("Unknown token");
        }

        private bool IsAlphaNumberic(char character)
        {
            return _identifierRegex.IsMatch(character.ToString(CultureInfo.InvariantCulture));
        }

        private void TakeCharacter()
        {
            if (_currentCharacter == '\0')
            {
                return;
            }

            _currentContent += _currentCharacter;
            if (_sourceIndex == _pattern.Length - 1)
            {
                _currentCharacter = '\0';
                return;
            }

            _currentCharacter = _pattern[++_sourceIndex];
        }
    }
}
