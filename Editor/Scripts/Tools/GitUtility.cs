using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using Debug = UnityEngine.Debug;

namespace GoodHub.Core.Editor
{
    public static class GitUtilities
    {
        [MenuItem("Git/Pull")]
        private static void GitPull()
        {
            try
            {
                // Configure the process start info
                ProcessStartInfo processInfo = new ProcessStartInfo
                {
                    FileName = "git",
                    Arguments = "pull",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                // Start the process
                using (Process process = new Process())
                {
                    process.StartInfo = processInfo;
                    process.Start();

                    // Capture the output and error messages
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    process.WaitForExit();

                    // Log the results
                    if (!string.IsNullOrEmpty(output))
                    {
                        Debug.Log($"Git Pull Output:\n{output}");
                    }

                    if (!string.IsNullOrEmpty(error))
                    {
                        Debug.LogError($"Git Pull Error:\n{error}");
                    }

                    if (process.ExitCode != 0)
                    {
                        Debug.LogError($"Git Pull failed with exit code {process.ExitCode}.");
                    }
                    else
                    {
                        Debug.Log("Git Pull completed successfully.");
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"An error occurred while executing Git Pull: {ex.Message}");
            }
        }
    }
}