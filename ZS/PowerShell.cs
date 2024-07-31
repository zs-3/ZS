using System;
using Microsoft.SmallBasic.Library;
using System.Diagnostics;

namespace ZS
{
    /// <summary>
    /// Provides methods for running PowerShell commands.
    /// </summary>
    [SmallBasicType]
    public class ZSPowerShell
    {
        /// <summary>
        /// Executes a PowerShell command and returns the result.
        /// </summary>
        /// <param name="command">The PowerShell command to execute.</param>
        /// <returns>A string containing the output and errors from the PowerShell command.</returns>
        /// <remarks>
        /// This method starts a new PowerShell process to execute the specified command. 
        /// Output and errors are combined into a single string.
        /// </remarks>
        public static Primitive Run(Primitive command)
        {
            try
            {
                using (Process process = new Process())
                {
                    process.StartInfo.FileName = "powershell.exe";
                    process.StartInfo.Arguments = "-NoProfile -ExecutionPolicy Bypass -Command \"" + command + "\"";
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;
                    
                    process.Start();
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    
                    process.WaitForExit();
                    
                    // Combine output and error into a single result
                    return "Output:\n" + output + "\nError:\n" + error;
                }
            }
            catch (Exception ex)
            {
                return "Exception:\n" + ex.Message;
            }
        }
    }
}
