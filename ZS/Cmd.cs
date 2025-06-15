using System;
using System.Globalization;
using System.Net.Mail;
using System.ServiceProcess;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.SmallBasic.Library;
using System.Windows.Forms;
using System.Web;
using System.IO;
using System.Drawing;
using System.Drawing.Text;
using System.Net.NetworkInformation;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Net;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using Microsoft.Win32;
using Microsoft.VisualBasic;


namespace ZS
{
	/// <summary>
	/// Provides methods to execute command line instructions from Small Basic.
	/// </summary>
	[SmallBasicType]
	public static class ZSCmd
	{
		
		/// <summary>
		/// Executes a batch script directly from a single string.
		/// </summary>
		/// <param name="script">The batch script to execute.</param>
		/// <returns>A string containing the output and errors from the batch script execution.</returns>
		public static Primitive RunBatchScript(Primitive script)
		{
			try {
				using (Process process = new Process()) {
					process.StartInfo.FileName = "cmd.exe";
					process.StartInfo.Arguments = "/c \"" + script + "\"";
					process.StartInfo.RedirectStandardOutput = true;
					process.StartInfo.RedirectStandardError = true;
					process.StartInfo.UseShellExecute = false;
					process.StartInfo.CreateNoWindow = true;

					process.Start();
					string output = process.StandardOutput.ReadToEnd();
					string error = process.StandardError.ReadToEnd();

					process.WaitForExit();

					return new Primitive("Output:\n" + output + "\nError:\n" + error);
				}
			} catch (Exception ex) {
				return new Primitive("Exception:\n" + ex.Message);
			}
		}
		
		/// <summary>
		/// Executes a batch script provided as an array of strings.
		/// Saves the script in a temporary file, runs it, returns the output, and deletes the script file.
		/// </summary>
		/// <param name="scriptLines">Array of strings, each representing a line of the batch script.</param>
		/// <returns>A string containing the output and errors from the batch script execution.</returns>
		public static Primitive RunBatchScriptFromArray(Primitive scriptLines)
		{
			string tempFilePath = Path.Combine(Path.GetTempPath(), "tempScript.bat");

			try {
				// Combine script lines into a single script and write to temp file
				using (StreamWriter writer = new StreamWriter(tempFilePath)) {
					int count = (int)scriptLines.GetItemCount();
					for (int i = 1; i <= count; i++) {
						writer.WriteLine(scriptLines[i].ToString());
					}
				}

				using (Process process = new Process()) {
					process.StartInfo.FileName = "cmd.exe";
					process.StartInfo.Arguments = "/c \"" + tempFilePath + "\"";
					process.StartInfo.RedirectStandardOutput = true;
					process.StartInfo.RedirectStandardError = true;
					process.StartInfo.UseShellExecute = false;
					process.StartInfo.CreateNoWindow = true;

					process.Start();
					string output = process.StandardOutput.ReadToEnd();
					string error = process.StandardError.ReadToEnd();

					process.WaitForExit();

					return new Primitive("Output:\n" + output + "\nError:\n" + error);
				}
			} catch (Exception ex) {
				return new Primitive("Exception:\n" + ex.Message);
			} finally {
				// Delete the temporary batch script file
				if (System.IO.File.Exists(tempFilePath)) {
					System.IO.File.Delete(tempFilePath);
				}
			}
		}
		
		/// <summary>
		/// Executes a command in the command line and returns the output.
		/// </summary>
		/// <param name="command">The command to execute.</param>
		/// <returns>The output of the command execution.</returns>
		public static Primitive Execute(Primitive command)
		{
			string cmdOutput = ExecuteCommand(command.ToString());
			return new Primitive(cmdOutput);
		}

		/// <summary>
		/// Copies a file from source to destination using command line.
		/// </summary>
		/// <param name="source">Source file path.</param>
		/// <param name="destination">Destination file path.</param>
		/// <returns>Output of the command execution.</returns>
		public static Primitive CopyFile(Primitive source, Primitive destination)
		{
			string cmdOutput = ExecuteCommand("copy \"" + source + "\" \"" + destination + "\"");
			return new Primitive(cmdOutput);
		}

		/// <summary>
		/// Moves a file from source to destination using command line.
		/// </summary>
		/// <param name="source">Source file path.</param>
		/// <param name="destination">Destination file path.</param>
		/// <returns>Output of the command execution.</returns>
		public static Primitive MoveFile(Primitive source, Primitive destination)
		{
			string cmdOutput = ExecuteCommand("move \"" + source + "\" \"" + destination + "\"");
			return new Primitive(cmdOutput);
		}

		/// <summary>
		/// Deletes a file using command line.
		/// </summary>
		/// <param name="filePath">File path to delete.</param>
		/// <returns>Output of the command execution.</returns>
		public static Primitive DeleteFile(Primitive filePath)
		{
			string cmdOutput = ExecuteCommand("del \"" + filePath + "\"");
			return new Primitive(cmdOutput);
		}

		/// <summary>
		/// Renames a file or directory using command line.
		/// </summary>
		/// <param name="oldPath">Current path of the file or directory.</param>
		/// <param name="newPath">New path to rename to.</param>
		/// <returns>Output of the command execution.</returns>
		public static Primitive Rename(Primitive oldPath, Primitive newPath)
		{
			string cmdOutput = ExecuteCommand("ren \"" + oldPath + "\" \"" + newPath + "\"");
			return new Primitive(cmdOutput);
		}

		/// <summary>
		/// Creates a directory using command line.
		/// </summary>
		/// <param name="directoryPath">Directory path to create.</param>
		/// <returns>Output of the command execution.</returns>
		public static Primitive CreateDirectory(Primitive directoryPath)
		{
			string cmdOutput = ExecuteCommand("mkdir \"" + directoryPath + "\"");
			return new Primitive(cmdOutput);
		}

		/// <summary>
		/// Removes a directory using command line.
		/// </summary>
		/// <param name="directoryPath">Directory path to remove.</param>
		/// <returns>Output of the command execution.</returns>
		public static Primitive RemoveDirectory(Primitive directoryPath)
		{
			string cmdOutput = ExecuteCommand("rmdir \"" + directoryPath + "\"");
			return new Primitive(cmdOutput);
		}

		/// <summary>
		/// Lists contents of a directory using command line.
		/// </summary>
		/// <param name="directoryPath">Directory path to list.</param>
		/// <returns>Output of the command execution.</returns>
		public static Primitive ListDirectoryContents(Primitive directoryPath)
		{
			string cmdOutput = ExecuteCommand("dir \"" + directoryPath + "\"");
			return new Primitive(cmdOutput);
		}

		/// <summary>
		/// Changes current directory using command line.
		/// </summary>
		/// <param name="directoryPath">Directory path to change to.</param>
		/// <returns>Output of the command execution.</returns>
		public static Primitive ChangeDirectory(Primitive directoryPath)
		{
			string cmdOutput = ExecuteCommand("cd \"" + directoryPath + "\"");
			return new Primitive(cmdOutput);
		}

		/// <summary>
		/// Pings a host using command line.
		/// </summary>
		/// <param name="host">Host to ping.</param>
		/// <returns>Output of the command execution.</returns>
		public static Primitive PingHost(Primitive host)
		{
			string cmdOutput = ExecuteCommand("ping " + host);
			return new Primitive(cmdOutput);
		}

		/// <summary>
		/// Executes a command using the command line and captures the output.
		/// </summary>
		/// <param name="command">The command to execute.</param>
		/// <returns>The output of the command execution.</returns>
		private static string ExecuteCommand(string command)
		{
			StringBuilder output = new StringBuilder();
			Process process = new Process();
			process.StartInfo.FileName = "cmd.exe";
			process.StartInfo.Arguments = "/C " + command;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.RedirectStandardError = true;
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.CreateNoWindow = true;
			process.OutputDataReceived += (sender, args) => output.AppendLine(args.Data);
			process.ErrorDataReceived += (sender, args) => output.AppendLine(args.Data);
			process.Start();
			process.BeginOutputReadLine();
			process.BeginErrorReadLine();
			process.WaitForExit();
			return output.ToString();
		}
	}
}