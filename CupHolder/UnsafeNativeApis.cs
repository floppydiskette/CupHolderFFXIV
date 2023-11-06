using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace CupHolder; 

public class UnsafeNativeApis {
    internal static class InternalAPIs {
        
        [DllImport("winmm.dll", EntryPoint = "mciSendStringA", CharSet = CharSet.Ansi)]
        internal static extern int mciSendString(string lpstrCommand,
                                                 StringBuilder? lpstrReturnString,
                                                 int uReturnLength,
                                                 IntPtr hwndCallback);
    }

    public static bool CheckForWine() {
        string tempName = Guid.NewGuid().ToString();
        if (File.Exists("Z:/bin/uname") && File.Exists("Z:/bin/sh")) {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "Z:/bin/bash";
            cmd.StartInfo.Arguments = $"-c \"/bin/uname -a >/tmp/{tempName}\"";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.RedirectStandardError = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            Thread.Sleep(100);

            if (File.Exists($"Z:/tmp/{tempName}")) {
                string filetext = File.ReadAllText($"Z:/tmp/{tempName}");
                Plugin.PluginLog.Debug($"uname returned: {filetext}");
                return true;
            }
            Plugin.PluginLog.Debug("uname did not return anything");
        }
        return false;
    }
    
    public static bool WineIsLinux() {
        string tempName = Guid.NewGuid().ToString();
        if (File.Exists("Z:/bin/uname") && File.Exists("Z:/bin/sh")) {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "Z:/bin/bash";
            cmd.StartInfo.Arguments = $"-c \"/bin/uname -a >/tmp/{tempName}\"";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.RedirectStandardError = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            Thread.Sleep(100);

            if (File.Exists($"Z:/tmp/{tempName}")) {
                string filetext = File.ReadAllText($"Z:/tmp/{tempName}");
                return filetext.Contains("Linux");
            }
            Plugin.PluginLog.Debug("uname did not return anything");
        }
        return false;
    }

    public static void OpenCupholderWindows() {
        InternalAPIs.mciSendString("set cdaudio door open", null, 0, 0);
    }

    public static void OpenCupholderLinux() {
        Process cmd = new Process();
        cmd.StartInfo.FileName = "cmd.exe";
        cmd.StartInfo.RedirectStandardInput = true;
        cmd.StartInfo.RedirectStandardOutput = true;
        cmd.StartInfo.CreateNoWindow = true;
        cmd.StartInfo.UseShellExecute = false;
        cmd.Start();

        cmd.StandardInput.WriteLine("start /unix /usr/bin/eject");
        cmd.StandardInput.Flush();
        cmd.StandardInput.Close();
    }
    
    public static void OpenCupholderMacOS() {
        Process cmd = new Process();
        cmd.StartInfo.FileName = "cmd.exe";
        cmd.StartInfo.RedirectStandardInput = true;
        cmd.StartInfo.RedirectStandardOutput = true;
        cmd.StartInfo.CreateNoWindow = true;
        cmd.StartInfo.UseShellExecute = false;
        cmd.Start();

        cmd.StandardInput.WriteLine("start /unix /usr/bin/drutil tray open");
        cmd.StandardInput.Flush();
        cmd.StandardInput.Close();
    }
}
