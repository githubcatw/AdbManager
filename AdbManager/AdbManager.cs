using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADBSharp {

    /// <summary>
    /// Thrown when an invaild ADB package is supplied.
    /// </summary>
    public class InvalidOrDummyAdbPackageException : Exception {

        public InvalidOrDummyAdbPackageException()
        {
        }

        public InvalidOrDummyAdbPackageException(string message)
            : base(message)
        {
        }

        public InvalidOrDummyAdbPackageException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    /// <summary>
    /// A class to do ADB and Fastboot operations from C#.
    /// </summary>
    class ADB {

        private string[] AdbFiles = {
            "adb.exe",
            "fastboot.exe",
            "AdbWinApi.dll",
            "AdbWinUsbApi.dll"
        };

        /// <summary>
        /// The standard output of the latest command.
        /// </summary>
        public string LastStdout;
        
        /// <summary>
        /// The standard error of the latest command.
        /// </summary>
        public string LastStderr;

        /// <summary>
        /// Initialize ADB and Fastboot.
        /// </summary>
        public void Init() {
            if (!Directory.Exists(Path.GetTempPath() + @"\adbmicro")) {
                File.WriteAllBytes(Path.GetTempPath() + @"\adb.zip", AdbManager.Properties.Resources.AM_adb);
                using (ZipFile zip = ZipFile.Read(Path.GetTempPath() + @"\adb.zip")) {
                    zip.ExtractAll(Path.GetTempPath() + @"\adbmicro");
                }
                var correctFiles = 0;
                foreach (string f in AdbFiles) {
                    if (File.Exists(Path.GetTempPath() + @"\adbmicro\" + f)) {
                        correctFiles += 1;
                    }
                }
                if (correctFiles <= 0) {
                    throw new InvalidOrDummyAdbPackageException();
                }
            }
        }

        /// <summary>
        /// Run ADB with arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public void RunAdb(string args) {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = Path.GetTempPath() + @"\adbmicro\adb.exe",
                Arguments = args,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };
            var process = Process.Start(processStartInfo);
            LastStdout = process.StandardOutput.ReadToEnd();
            LastStderr = process.StandardError.ReadToEnd();
            process.WaitForExit();
        }

        /// <summary>
        /// Run Fastboot with arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public void RunFastboot(string args) {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = Path.GetTempPath() + @"\adbmicro\fastboot.exe",
                Arguments = args,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };
            var process = Process.Start(processStartInfo);
            LastStdout = process.StandardOutput.ReadToEnd();
            LastStderr = process.StandardError.ReadToEnd();
            process.WaitForExit();
        }
    }
}
