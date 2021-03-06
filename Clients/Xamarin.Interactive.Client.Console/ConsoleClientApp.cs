﻿//
// Author:
//   Aaron Bockover <abock@xamarin.com>
//
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

using Xamarin.Interactive.Client.Updater;
using Xamarin.Interactive.Core;
using Xamarin.Interactive.IO;
using Xamarin.Interactive.Preferences;
using Xamarin.Interactive.SystemInformation;

namespace Xamarin.Interactive.Client.Console
{
    sealed class ConsoleClientApp : ClientApp
    {
        sealed class ConsoleHostEnvironment : HostEnvironment
        {
            public override HostOS OSName { get; }
                = Environment.OSVersion.Platform == PlatformID.Unix
                    ? HostOS.macOS
                    : HostOS.Windows;

            public override string OSVersionString { get; } = Environment.OSVersion.VersionString;
            public override Version OSVersion { get; } = Environment.OSVersion.Version;
            public override int? ProcessorCount { get; } = Environment.ProcessorCount;
        }

        sealed class ConsoleFileSystem : DotNetFileSystem
        {
            public override QuarantineInfo GetQuarantineInfo (FilePath path) => null;

            public override void StripQuarantineInfo (FilePath path)
            {
            }
        }

        protected override ClientAppPaths CreateClientAppPaths ()
        {
            var basePath = FilePath.Build (
                Environment.GetFolderPath (Environment.SpecialFolder.LocalApplicationData),
                "com.xamarin.workbooks.console");

            return new ClientAppPaths (
                basePath.Combine ("logs"),
                basePath.Combine ("preferences"),
                basePath.Combine ("cache"));
        }

        protected override IPreferenceStore CreatePreferenceStore ()
            => new MemoryOnlyPreferenceStore ();

        protected override HostEnvironment CreateHostEnvironment ()
            => new ConsoleHostEnvironment ();

        protected override IFileSystem CreateFileSystem ()
            => new ConsoleFileSystem ();

        protected override ClientWebServer CreateClientWebServer ()
            => new ClientWebServer (FilePath.Empty);

        protected override UpdaterService CreateUpdaterService ()
            => null;
    }
}