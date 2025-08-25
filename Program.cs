using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;

namespace Dexec;

internal partial class Program
{
    [GeneratedRegex(@"#!(?:\s+)?(?<binary>.+)")]
    public static partial Regex HeaderRegex();
    [GeneratedRegex(@"\(env:(?<env_var>.+)\)")]
    public static partial Regex VarRegex();
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            if(OperatingSystem.IsWindows()) Setup();
            return;
        }
        if (!File.Exists(args[0])) return;
        string? firstLine = File.ReadLines(args[0]).FirstOrDefault();
        if(firstLine == null) return;
        Match headerMatch = HeaderRegex().Match(firstLine);
        if(!headerMatch.Success) return;
        firstLine = headerMatch.Groups["binary"].Value;
        MatchCollection envVars = VarRegex().Matches(firstLine);
        foreach (Match match in envVars)
        {
            var value = Environment.GetEnvironmentVariable(match.Groups["env_var"].Value);
            value ??= Environment.GetEnvironmentVariable(match.Groups["env_var"].Value, EnvironmentVariableTarget.User);
            value ??= Environment.GetEnvironmentVariable(match.Groups["env_var"].Value, EnvironmentVariableTarget.Machine);
            if (value is null) return;
            firstLine = firstLine.Replace(match.Value, value);
        }
        Process process = new();
        string[] split = firstLine.Split(' ');
        if (split.Length > 1)
            process.StartInfo = new ProcessStartInfo(split[0], string.Join(' ', [.. split[1..], args[0]]));
        else
            process.StartInfo = new ProcessStartInfo(firstLine, args[0]);
        process.StartInfo.UseShellExecute = true;
        process.StartInfo.CreateNoWindow = true;
        process.Start();
    }

    [SupportedOSPlatform("Windows")]
    static void Setup()
    {
        RegistryKey? key = Registry.ClassesRoot.OpenSubKey(@"*\shell", true);
        if(key != null)
        {
            var localRoot = key.CreateSubKey("Dexec", true);
            if (localRoot == null)
                return;
            localRoot.SetValue(null, "Dexec");
            localRoot.SetValue("Icon", Path.Combine(AppContext.BaseDirectory, "Dexec.exe"));
            localRoot.CreateSubKey("command", true).SetValue("", $"\"{Path.Combine(AppContext.BaseDirectory, "Dexec.exe")}\" \"%1\"");
        }
    }
}
