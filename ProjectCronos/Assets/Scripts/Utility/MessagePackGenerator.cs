using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public class MessagePackGenerator
{
    [MenuItem("MessagePack/CodeGenerate")]
    private static void GenerateMessagePack()
    {
        ExecuteMessagePackCodeGenerator();
    }

    private static void ExecuteMessagePackCodeGenerator()
    {
        UnityEngine.Debug.Log($"{nameof(ExecuteMessagePackCodeGenerator)} : start");

        var exProcess = new Process();

        var psi = new ProcessStartInfo()
        {
            CreateNoWindow = true,
            WindowStyle = ProcessWindowStyle.Hidden,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            FileName = "dotnet",
            Arguments =
                $@"mpc -i ""{Application.dataPath}/MasterData/TableDefines"" -o ""{Application.dataPath}/MasterData/Generated/MessagePack""",
        };

        var p = Process.Start(psi);

        p.EnableRaisingEvents = true;
        p.Exited += (object sender, System.EventArgs e) =>
        {
            var data = p.StandardOutput.ReadToEnd();
            UnityEngine.Debug.Log($"{data}");
            UnityEngine.Debug.Log($"{nameof(ExecuteMessagePackCodeGenerator)} : end");
            p.Dispose();
            p = null;
        };
    }
}
