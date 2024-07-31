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