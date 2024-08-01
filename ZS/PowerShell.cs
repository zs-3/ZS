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
			try {
				using (Process process = new Process()) {
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
			} catch (Exception ex) {
				return "Exception:\n" + ex.Message;
			}
		}
    
    
		/// <summary>
		/// Executes a PowerShell script provided as an array of strings.
		/// </summary>
		/// <param name="scriptLines">Array of strings, each representing a line of the PowerShell script.</param>
		/// <returns>A string containing the output and errors from the PowerShell script execution.</returns>
		public static Primitive RunPowerShellScriptFromArray(Primitive[] scriptLines)
		{
			try {
				// Combine script lines into a single script
				string script = string.Empty;
				foreach (Primitive line in scriptLines) {
					script += line.ToString() + Environment.NewLine;
				}

				using (Process process = new Process()) {
					process.StartInfo.FileName = "powershell.exe";
					process.StartInfo.Arguments = "-NoProfile -ExecutionPolicy Bypass -Command \"" + script + "\"";
					process.StartInfo.RedirectStandardOutput = true;
					process.StartInfo.RedirectStandardError = true;
					process.StartInfo.UseShellExecute = false;
					process.StartInfo.CreateNoWindow = true;

					process.Start();
					string output = process.StandardOutput.ReadToEnd();
					string error = process.StandardError.ReadToEnd();

					process.WaitForExit();

					return "Output:\n" + output + "\nError:\n" + error;
				}
			} catch (Exception ex) {
				return "Exception:\n" + ex.Message;
			}
		}
	
		/// <summary>
		/// Executes a PowerShell script file and returns the output.
		/// </summary>
		/// <param name="filePath">The path to the PowerShell script file.</param>
		/// <returns>A string containing the output and errors from the PowerShell script execution.</returns>
		public static Primitive RunPowerShellFile(Primitive filePath)
		{
			try {
				// Validate the file path
				if (string.IsNullOrWhiteSpace(filePath)) {
					return "Error: File path is null or empty.";
				}

				if (!System.IO.File.Exists(filePath)) {
					return "Error: File not found at path " + filePath + ".";
				}

				using (Process process = new Process()) {
					process.StartInfo.FileName = "powershell.exe";
					process.StartInfo.Arguments = string.Format("-NoProfile -ExecutionPolicy Bypass -File \"{0}\"", filePath);
					process.StartInfo.RedirectStandardOutput = true;
					process.StartInfo.RedirectStandardError = true;
					process.StartInfo.UseShellExecute = false;
					process.StartInfo.CreateNoWindow = true;

					process.Start();
					string output = process.StandardOutput.ReadToEnd();
					string error = process.StandardError.ReadToEnd();

					process.WaitForExit();

					return "Output:\n" + output + "\nError:\n" + error;
				}
			} catch (Exception ex) {
				return "Exception:\n" + ex.Message;
			}
		}
	
		
	
	
	
	}
	
}
