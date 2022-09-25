using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public class MasterMemoryGenerator
{
    [MenuItem("MasterMemory/CodeGenerate")]
    public static void GenerateMasterMemory()
    {
        ExecuteMasterMemoryCodeGenerator();
    }

    private static void ExecuteMasterMemoryCodeGenerator()
    {
        UnityEngine.Debug.Log($"{nameof(ExecuteMasterMemoryCodeGenerator)} : start");

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
                $@"dotnet-mmgen -i ""{Application.dataPath}/MasterData/TableDefines"" -o ""{Application.dataPath}/MasterData/Generated/MasterMemory"" -c -n Generated",
        };

        var p = Process.Start(psi);

        p.EnableRaisingEvents = true;
        p.Exited += (object sender, System.EventArgs e) =>
        {
            var data = p.StandardOutput.ReadToEnd();
            UnityEngine.Debug.Log($"{data}");
            UnityEngine.Debug.Log($"{nameof(ExecuteMasterMemoryCodeGenerator)} : end");
            p.Dispose();
            p = null;
        };
    }
}
