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
using Lunt.IO;
using Lunt.Tests.Integration.Utilities;
using Xunit;

namespace Lunt.Tests.Integration.Tests
{
    using Lunt.IO;
    using Lunt.Tests.Integration.Utilities;

    public class LuntApplicationTests
    {
        [Fact, IntegrationTest]
        public void Should_Build_Asset()
        {
            using (var context = new IntegrationContext())
            {
                // Given, When
                var result = context.ExecuteBuild("build.config");

                // Then
                context.Assert.ApplicationExitedWithoutError(result);
                context.Assert.TargetFileExists("Output/Text/Asset.dat");
                context.Assert.HasContent("Output/Text/Asset.dat", "HELLO WORLD");
            }
        }

        [Fact, IntegrationTest]
        public void Should_Build_Asset_Using_Specified_Processor()
        {
            using (var context = new IntegrationContext())
            {
                // Given, When
                var result = context.ExecuteBuild("build_reverse.config");

                // Then
                context.Assert.ApplicationExitedWithoutError(result);
                context.Assert.TargetFileExists("Output/Text/Asset.dat");
                context.Assert.HasContent("Output/Text/Asset.dat", "DLROW OLLEH");
            }
        }

#if !UNIX

        [Fact, IntegrationTest]
        public void Should_Not_Rebuild_File_If_Asset_And_Dependencies_Are_Unchanged()
        {
            using (var context = new IntegrationContext())
            {
                // Given
                context.ExecuteBuild("build_reverse.config");

                using (var scope = new StringTraceListenerScope())
                {
                    // When
                    context.ExecuteBuild("build_reverse.config");

                    // Then
                    Assert.True(scope.Messages.Contains("Skipped @Text/Asset.txt (no change)"));
                }
            }
        }

        [Fact, IntegrationTest]
        public void Should_Rebuild_File_If_Dependency_Have_Changed()
        {
            using (var context = new IntegrationContext())
            {
                // Given
                context.ExecuteBuild("build_reverse.config");

                var metadataFilePath = context.GetTargetPath("Assets/Text/Asset.txt.metadata");
                System.IO.File.WriteAllText(metadataFilePath, "THIS WAS NEW!");
                
                using (var scope = new StringTraceListenerScope())
                {
                    // When
                    context.ExecuteBuild("build_reverse.config");

                    // Then
                    Assert.True(scope.Messages.Contains("The file size of dependency 'Text/Asset.txt.metadata' has changed. Rebuilding asset."));
                }
            }
        }

#endif

        [Fact, IntegrationTest]
        public void Should_Build_Asset_With_Glob()
        {
            using (var context = new IntegrationContext())
            {
                // Given, When
                var result = context.ExecuteBuild("build_glob.config");

                // Then
                context.Assert.ApplicationExitedWithoutError(result);
                context.Assert.TargetFileExists("Output/Text/Asset.dat");
                context.Assert.TargetFileExists("Output/Text/Other.dat");
            }
        }

        [Fact, IntegrationTest, GitHubIssue(20)]
        public void Should_Build_Asset_With_Glob_When_Input_Directory_Has_Parent_Reference()
        {
            using (var context = new IntegrationContext())
            {
                // Given, When
                var options = IntegrationHelper.CreateOptions(context, "build_glob.config");
                options.InputDirectory = new DirectoryPath(options.InputDirectory.FullPath.Replace("Assets", "Assets/../Assets"));
                var result = context.RunApplication(options);

                // Then
                context.Assert.ApplicationExitedWithoutError(result);
                context.Assert.TargetFileExists("Output/Text/Asset.dat");
                context.Assert.TargetFileExists("Output/Text/Other.dat");
                context.Assert.TargetFileExists("Output/Text/More/Hello.dat");
            }
        }

        [Fact, IntegrationTest, GitHubIssue(20)]
        public void Should_Build_Asset_With_Glob_When_Output_Directory_Has_Parent_Reference()
        {
            using (var context = new IntegrationContext())
            {
                // Given, When
                var options = IntegrationHelper.CreateOptions(context, "build_glob.config");
                options.OutputDirectory = new DirectoryPath(options.OutputDirectory.FullPath.Replace("Assets", "Output/../Output"));
                var result = context.RunApplication(options);

                // Then
                context.Assert.ApplicationExitedWithoutError(result);
                context.Assert.TargetFileExists("Output/Text/Asset.dat");
                context.Assert.TargetFileExists("Output/Text/Other.dat");
                context.Assert.TargetFileExists("Output/Text/More/Hello.dat");
            }
        }

        [Fact, IntegrationTest, GitHubIssue(21)]
        public void Should_Include_Current_Path_When_Globbing()
        {
            using (var context = new IntegrationContext())
            {
                // Given, When
                var options = IntegrationHelper.CreateOptions(context, "build_glob_2.config");
                options.OutputDirectory = new DirectoryPath(options.OutputDirectory.FullPath.Replace("Assets", "Output/../Output"));
                var result = context.RunApplication(options);

                // Then
                context.Assert.ApplicationExitedWithoutError(result);
                context.Assert.TargetFileExists("Output/Text/Asset.dat");
                context.Assert.TargetFileExists("Output/Text/Other.dat");
                context.Assert.TargetFileExists("Output/Text/More/Hello.dat");
            }
        }
    }
}
