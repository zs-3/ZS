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
	/// Provides methods and properties for managing processes.
	/// This class allows you to start new processes, manage process information, and interact with running processes.
	/// It includes functionalities for:
	/// - Starting processes with or without arguments
	/// - Retrieving process IDs and names
	/// - Getting and setting process-related information such as file path, arguments, priority, and more
	/// </summary>
	[SmallBasicType]
	public static class ZSProcess
	{
		private static Process process = new Process();

		/// <summary>
		/// Starts a new process with the specified executable file path.
		/// </summary>
		/// <param name="filePath">The path of the executable file to start. Example: "C:\\Windows\\System32\\notepad.exe"</param>
		public static void StartProcess(Primitive filePath)
		{
			Process.Start((string)filePath);
		}

		/// <summary>
		/// Starts a new process with the specified executable file path and arguments.
		/// </summary>
		/// <param name="filePath">The path of the executable file to start. Example: "C:\\Windows\\System32\\cmd.exe"</param>
		/// <param name="arguments">The arguments to pass to the executable file. Example: "/c echo Hello World"</param>
		public static void StartProcessWithArgs(Primitive filePath, Primitive arguments)
		{
			Process.Start((string)filePath, (string)arguments);
		}

		/// <summary>
		/// Starts a new process with the specified ProcessStartInfo.
		/// </summary>
		/// <param name="filePath">The path of the executable file to start. Example: "C:\\Windows\\System32\\notepad.exe"</param>
		/// <param name="arguments">The arguments to pass to the executable file. Example: ""</param>
		public static void StartProcessWithInfo(Primitive filePath, Primitive arguments)
		{
			ProcessStartInfo startInfo = new ProcessStartInfo {
				FileName = (string)filePath,
				Arguments = (string)arguments
			};
			Process.Start(startInfo);
		}

		/// <summary>
		/// Gets or sets the file path of the application to start.
		/// </summary>
		/// <example>
		/// To set the file path: <br />
		/// ZSProcess.FilePath = "C:\\Windows\\System32\\notepad.exe"
		/// </example>
		public static Primitive FilePath {
			get { return process.StartInfo.FileName; }
			set { process.StartInfo.FileName = (string)value; }
		}

		/// <summary>
		/// Gets or sets the arguments to pass to the executable file.
		/// </summary>
		/// <example>
		/// To set arguments: <br />
		/// ZSProcess.Arguments = "/c echo Hello World"
		/// </example>
		public static Primitive Arguments {
			get { return process.StartInfo.Arguments; }
			set { process.StartInfo.Arguments = (string)value; }
		}

		/// <summary>
		/// Gets or sets a value that indicates whether to use the operating system shell to start the process.
		/// </summary>
		/// <example>
		/// To enable shell execution: <br />
		/// ZSProcess.UseShellExecute = true
		/// </example>
		public static Primitive UseShellExecute {
			get { return process.StartInfo.UseShellExecute; }
			set { process.StartInfo.UseShellExecute = (bool)value; }
		}

		/// <summary>
		/// Gets or sets a value that determines whether to redirect standard input, output, and error streams.
		/// </summary>
		/// <example>
		/// To enable redirection: <br />
		/// ZSProcess.RedirectStandardOutput = true
		/// </example>
		public static Primitive RedirectStandardOutput {
			get { return process.StartInfo.RedirectStandardOutput; }
			set { process.StartInfo.RedirectStandardOutput = (bool)value; }
		}

		/// <summary>
		/// Gets or sets the value of the process priority.
		/// </summary>
		/// <example>
		/// To set priority class: <br />
		/// ZSProcess.PriorityClass = "High"
		/// </example>
		public static Primitive PriorityClass {
			get { return process.PriorityClass.ToString(); }
			set { process.PriorityClass = (ProcessPriorityClass)Enum.Parse(typeof(ProcessPriorityClass), (string)value); }
		}

		/// <summary>
		/// Gets or sets a value that indicates whether to create a new window for the process.
		/// </summary>
		/// <example>
		/// To disable window creation: <br />
		/// ZSProcess.CreateNoWindow = true
		/// </example>
		public static Primitive CreateNoWindow {
			get { return process.StartInfo.CreateNoWindow; }
			set { process.StartInfo.CreateNoWindow = (bool)value; }
		}

		/// <summary>
		/// Gets a list of process IDs for all running processes on the local machine.
		/// </summary>
		/// <returns>A comma-separated string of process IDs. Example: "1234,5678"</returns>
		public static Primitive GetProcessIds()
		{
			Process[] processes = Process.GetProcesses();
			string ids = string.Join(",", System.Array.ConvertAll(processes, p => p.Id.ToString()));
			return ids;
		}

		/// <summary>
		/// Gets the process name for a specified process ID.
		/// </summary>
		/// <param name="id">The process ID. Example: 1234</param>
		/// <returns>The process name. Example: "notepad"</returns>
		public static Primitive GetProcessName(Primitive id)
		{
			try {
				Process process = Process.GetProcessById((int)id);
				return process.ProcessName;
			} catch {
				return "Invalid Process ID";
			}
		}

		/// <summary>
		/// Gets the process ID of the currently managed process.
		/// </summary>
		/// <returns>The process ID. Example: 1234</returns>
		public static Primitive GetProcessId()
		{
			return process.Id;
		}

		/// <summary>
		/// Gets the start time of the currently managed process.
		/// </summary>
		/// <returns>The start time of the process. Example: "2024-07-24 14:30:00"</returns>
		public static Primitive GetStartTime()
		{
			return process.StartTime.ToString();
		}

		/// <summary>
		/// Gets the exit time of the currently managed process.
		/// </summary>
		/// <returns>The exit time of the process. Example: "2024-07-24 15:00:00"</returns>
		public static Primitive GetExitTime()
		{
			return process.ExitTime.ToString();
		}

		/// <summary>
		/// Gets a value indicating whether the currently managed process has exited.
		/// </summary>
		/// <returns>true if the process has exited; otherwise, false.</returns>
		public static Primitive HasExited()
		{
			return process.HasExited;
		}

		/// <summary>
		/// Gets the title of the main window of the currently managed process.
		/// </summary>
		/// <returns>The main window title. Example: "Untitled - Notepad"</returns>
		public static Primitive GetMainWindowTitle()
		{
			return process.MainWindowTitle;
		}

		/// <summary>
		/// Gets a value indicating whether the currently managed process is responding.
		/// </summary>
		/// <returns>true if the process is responding; otherwise, false.</returns>
		public static Primitive IsResponding()
		{
			return process.Responding;
		}

		/// <summary>
		/// Gets the total processor time for the currently managed process.
		/// </summary>
		/// <returns>The total processor time. Example: "00:00:01.2345678"</returns>
		public static Primitive GetTotalProcessorTime()
		{
			return process.TotalProcessorTime.ToString();
		}
	}
}