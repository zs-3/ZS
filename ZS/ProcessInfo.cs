using System;
using System.Diagnostics;
using Microsoft.SmallBasic.Library;

namespace ZS
{

	/// <summary>
	/// Provides ProcessInfo Functions from Small Basic.
	/// First Set All the setting then call the method ZSProcessInfo.Start()
	/// </summary>
	[SmallBasicType]
	public static class ZSProcessInfo
	{
		private static ProcessStartInfo startInfo = new ProcessStartInfo();
		private static System.Collections.Generic.Dictionary<int, Process> processes = new System.Collections.Generic.Dictionary<int, Process>();

		/// <summary>
		/// Gets or sets the set of command-line arguments to use when starting the application.
		/// Setting For ZSProcess.Start()
		/// </summary>
		public static Primitive Arguments {
			get { return startInfo.Arguments; }
			set { startInfo.Arguments = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether to start the process in a new window.
		/// Setting For ZSProcess.Start()
		/// </summary>
		public static Primitive CreateNoWindow {
			get { return startInfo.CreateNoWindow; }
			set { startInfo.CreateNoWindow = value; }
		}

		/// <summary>
		/// Gets or sets the domain to use when starting the process.
		/// Setting For ZSProcess.Start()
		/// </summary>
		public static Primitive Domain {
			get { return startInfo.Domain; }
			set { startInfo.Domain = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether to use the operating system shell to start the process.
		/// Setting For ZSProcess.Start()
		/// </summary>
		public static Primitive UseShellExecute {
			get { return startInfo.UseShellExecute; }
			set { startInfo.UseShellExecute = value; }
		}

		/// <summary>
		/// Gets or sets the application or document to start.
		/// Setting For ZSProcess.Start()
		/// </summary>
		public static Primitive FileName {
			get { return startInfo.FileName; }
			set { startInfo.FileName = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the error output of an application is written to the StandardError stream.
		/// Setting For ZSProcess.Start()
		/// </summary>
		public static Primitive RedirectStandardError {
			get { return startInfo.RedirectStandardError; }
			set { startInfo.RedirectStandardError = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the input for an application is read from the StandardInput stream.
		/// Setting For ZSProcess.Start()
		/// </summary>
		public static Primitive RedirectStandardInput {
			get { return startInfo.RedirectStandardInput; }
			set { startInfo.RedirectStandardInput = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the output of an application is written to the StandardOutput stream.
		/// Setting For ZSProcess.Start()
		/// </summary>
		public static Primitive RedirectStandardOutput {
			get { return startInfo.RedirectStandardOutput; }
			set { startInfo.RedirectStandardOutput = value; }
		}

		/// <summary>
		/// Gets or sets the user name to use when starting the process.
		/// Setting For ZSProcess.Start()
		/// </summary>
		public static Primitive UserName {
			get { return startInfo.UserName; }
			set { startInfo.UserName = value; }
		}

		/// <summary>
		/// Gets or sets the password for the user name when starting the process.
		/// Note: For security reasons, setting passwords is not directly supported in this wrapper.
		/// Setting For ZSProcess.Start()
		/// </summary>
		public static System.Security.SecureString Password {
			set { startInfo.Password = value; }
			get { return startInfo.Password; }
		}

		/// <summary>
		/// Gets or sets the verb to use when opening the application or document specified by the FileName property.
		/// Setting For ZSProcess.Start()
		/// </summary>
		public static Primitive Verb {
			get { return startInfo.Verb; }
			set { startInfo.Verb = value; }
		}

		/// <summary>
		/// Gets or sets the Window style to use for the process.
		/// Setting For ZSProcess.Start()
		/// </summary>
		public static Primitive WindowStyle {
			get { return (int)startInfo.WindowStyle; }
			set { startInfo.WindowStyle = (ProcessWindowStyle)Enum.Parse(typeof(ProcessWindowStyle), value); }
		}

		/// <summary>
		/// Gets or sets the working directory for the process to be started.
		/// Setting For ZSProcess.Start()
		/// </summary>
		public static Primitive WorkingDirectory {
			get { return startInfo.WorkingDirectory; }
			set { startInfo.WorkingDirectory = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the output of an application is written to the StandardOutput stream.
		/// Setting For ZSProcess.Start()
		/// </summary>
		public static System.Text.Encoding StandardOutputEncoding {
			set { startInfo.StandardOutputEncoding = value; }
			get { return startInfo.StandardOutputEncoding; }
		}

		/// <summary>
		/// Gets or sets the environment variables that apply to this process and its child processes.
		/// Use a semicolon (;) to separate multiple variables.
		/// Example: "Path=C:\Windows\System32;TEMP=C:\Temp"
		/// Setting For ZSProcess.Start()
		/// </summary>
		public static Primitive EnvironmentVariables {
			set { 
				var variables = value.ToString().Split(';');
				foreach (var variable in variables) {
					var keyValue = variable.Split('=');
					if (keyValue.Length == 2) {
						startInfo.EnvironmentVariables[keyValue[0]] = keyValue[1];
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether an error dialog box is displayed to the user if the process cannot be started.
		/// Setting For ZSProcess.Start()
		/// </summary>
		public static Primitive ErrorDialog {
			get { return startInfo.ErrorDialog; }
			set { startInfo.ErrorDialog = value; }
		}

		/// <summary>
		/// Starts the process using the specified settings.
		/// </summary>
		public static Primitive Start()
		{
			try {
				Process process = Process.Start(startInfo);
				processes[process.Id] = process;
				return process.Id.ToString();
			} catch (Exception ex) {
				return ex.TargetSite + " : " + ex.Message;
			}
		}
	
		/// <summary>
		/// Gets the output of the process by its ID.
		/// Returns the standard output if available, otherwise returns an error message.
		/// </summary>
		/// <param name="processId">The ID of the process.</param>
		/// <returns>A string containing the process output or an error message.</returns>
		public static Primitive GetOutput(Primitive processId)
		{
			try {
				int id = processId;
				if (processes.ContainsKey(id)) {
					Process process = processes[id];
					if (process.HasExited) {
						return process.StandardOutput.ReadToEnd();
					} else {
						return "Error: Process is still running.";
					}
				} else {
					return "Error: Invalid process ID.";
				}
			} catch (Exception ex) {
				return "Error: " + ex.Message;
			}
		}

	
		/// <summary>
		/// Gets or sets a value indicating whether the Windows user profile should be loaded.
		/// </summary>
		public static Primitive LoadUserProfile {
			get { return startInfo.LoadUserProfile; }
			set { startInfo.LoadUserProfile = value; }
		}

		/// <summary>
		/// Gets or sets the password in clear text to use when starting the process.
		/// </summary>
		public static Primitive PasswordInClearText {
			set { startInfo.PasswordInClearText = value; }
		}
	
		/// <summary>
		/// Starts the process using the specified settings and returns the standard output.
		/// Returns the standard output if the process starts and completes successfully, otherwise returns the error message.
		/// </summary>
		/// <returns>A string containing the process output or an error message.</returns>
		public static Primitive StartAndGetOutput()
		{
			try {
				startInfo.RedirectStandardOutput = true;
				startInfo.RedirectStandardError = true;
				startInfo.UseShellExecute = false;
			
        
				using (Process process = Process.Start(startInfo)) {
					process.WaitForExit();
					if (process.ExitCode == 0) {
						return process.StandardOutput.ReadToEnd();
					} else {
						return process.StandardError.ReadToEnd();
					}
				}
			} catch (Exception ex) {
				return "Error: " + ex.Message;
			}
		}
	
	

	}
}