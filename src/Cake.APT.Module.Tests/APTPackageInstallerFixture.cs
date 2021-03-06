using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Packaging;
using Cake.Testing;
using NSubstitute;

namespace Cake.APT.Module.Tests
{
    /// <summary>
    /// Fixture used for testing APTPackageInstaller
    /// </summary>
    internal sealed class APTPackageInstallerFixture
    {
        public ICakeEnvironment Environment { get; set; }
        public IFileSystem FileSystem { get; set; }
        public IProcessRunner ProcessRunner { get; set; }
        public IAPTContentResolver ContentResolver { get; set; }
        public ICakeLog Log { get; set; }

        public PackageReference Package { get; set; }
        public PackageType PackageType { get; set; }
        public DirectoryPath InstallPath { get; set; }

        public ICakeConfiguration Config { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="APTPackageInstallerFixture"/> class.
        /// </summary>
        internal APTPackageInstallerFixture()
        {
            Environment = FakeEnvironment.CreateUnixEnvironment();
            FileSystem = new FakeFileSystem(Environment);
            ProcessRunner = Substitute.For<IProcessRunner>();
            ContentResolver = Substitute.For<IAPTContentResolver>();
            Log = new FakeLog();
            Config = Substitute.For<ICakeConfiguration>();
            Package = new PackageReference("apt:?package=xcowsay");
            PackageType = PackageType.Addin;
            InstallPath = new DirectoryPath("./fake-path");
        }

        /// <summary>
        /// Create the installer.
        /// </summary>
        /// <returns>The APT package installer.</returns>
        internal APTPackageInstaller CreateInstaller()
        {
            return new APTPackageInstaller(Environment, ProcessRunner, Log, ContentResolver, Config);
        }

        /// <summary>
        /// Installs the specified resource at the given location.
        /// </summary>
        /// <returns>The installed files.</returns>
        internal IReadOnlyCollection<IFile> Install()
        {
            var installer = CreateInstaller();
            return installer.Install(Package, PackageType, InstallPath);
        }

        /// <summary>
        /// Determines whether this instance can install the specified resource.
        /// </summary>
        /// <returns><c>true</c> if this installer can install the specified resource; otherwise <c>false</c>.</returns>
        internal bool CanInstall()
        {
            var installer = CreateInstaller();
            return installer.CanInstall(Package, PackageType);
        }

        internal ProcessArgumentBuilder GetArguments()
        {
            var installer = CreateInstaller();
            return installer.GetArguments(Package, Config);
        }
    }
}