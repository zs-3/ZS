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
	/// Provides System.Environment Functions For Small Basic.
	/// </summary>
	[SmallBasicType]
	public static class ZSEnvironment
	{
		/// <summary>
		/// Gets the command line for the application.
		/// </summary>
		/// <return>The command line arguments for the application.</return>
		public static Primitive CommandLine {
			get {
				return string.Join(" ", Environment.GetCommandLineArgs());
			}
		}

		/// <summary>
		/// Gets or sets the fully qualified path of the current working directory.
		/// </summary>
		/// <return>The path of the current working directory.</return>
		public static Primitive CurrentDirectory {
			get {
				return Environment.CurrentDirectory;
			}
			set {
				Environment.CurrentDirectory = value;
			}
		}

		/// <summary>
		/// Gets the name of the machine on which the application is running.
		/// </summary>
		/// <return>The name of the machine.</return>
		public static Primitive MachineName {
			get {
				return Environment.MachineName;
			}
		}

		/// <summary>
		/// Gets a string representing the newline character(s) used by the current environment.
		/// </summary>
		/// <return>The newline characters used by the current environment.</return>
		public static Primitive NewLine {
			get {
				return Environment.NewLine;
			}
		}

		/// <summary>
		/// Gets an OperatingSystem object that describes the current platform.
		/// </summary>
		/// <return>The current platform as a string.</return>
		public static Primitive OSVersion {
			get {
				return Environment.OSVersion.ToString();
			}
		}

		/// <summary>
		/// Gets the number of processors on the current machine.
		/// </summary>
		/// <return>The number of processors.</return>
		public static Primitive ProcessorCount {
			get {
				return Environment.ProcessorCount;
			}
		}

		/// <summary>
		/// Gets a string representation of the current call stack.
		/// </summary>
		/// <value>The current call stack as a string.</value>
		public static Primitive StackTrace {
			get {
				return Environment.StackTrace;
			}
		}

		/// <summary>
		/// Gets the full path of the system directory.
		/// </summary>
		/// <value>The path to the system directory.</value>
		public static Primitive SystemDirectory {
			get {
				return Environment.SystemDirectory;
			}
		}

		/// <summary>
		/// Gets the number of milliseconds elapsed since the system started.
		/// </summary>
		/// <value>The number of milliseconds since the system started.</value>
		public static Primitive TickCount {
			get {
				return Environment.TickCount;
			}
		}

		/// <summary>
		/// Gets the domain name of the current user.
		/// </summary>
		/// <value>The domain name of the current user.</value>
		public static Primitive UserDomainName {
			get {
				return Environment.UserDomainName;
			}
		}

		/// <summary>
		/// Gets the user name of the current thread.
		/// </summary>
		/// <value>The user name of the current thread.</value>
		public static Primitive UserName {
			get {
				return Environment.UserName;
			}
		}

		/// <summary>
		/// Gets the version of the common language runtime (CLR) that is installed on the operating system.
		/// </summary>
		/// <value>The version of the CLR.</value>
		public static Primitive Version {
			get {
				return Environment.Version.ToString();
			}
		}

		/// <summary>
		/// Gets the amount of physical memory allocated for the process.
		/// </summary>
		/// <value>The amount of physical memory allocated for the process, in bytes.</value>
		public static Primitive WorkingSet {
			get {
				return Environment.WorkingSet.ToString();
			}
		}

		/// <summary>
		/// Terminates the process and gives the exit code to the operating system.
		/// </summary>
		/// <param name="exitCode">The exit code to pass to the operating system.</param>
		public static void Exit(Primitive exitCode)
		{
			Environment.Exit(exitCode);
		}

		/// <summary>
		/// Returns the command-line arguments for the process as a single string with arguments separated by a delimiter.
		/// </summary>
		/// <returns>A string representing the command-line arguments, separated by commas.</returns>
		public static Primitive GetCommandLineArgs()
		{
			string[] args = Environment.GetCommandLineArgs();
			string result = string.Join(",", args);
			return result;
		}

		/// <summary>
		/// Retrieves the value of an environment variable.
		/// </summary>
		/// <param name="variable">The name of the environment variable.</param>
		/// <returns>The value of the environment variable.</returns>
		public static Primitive GetEnvironmentVariable(Primitive variable)
		{
			return Environment.GetEnvironmentVariable(variable);
		}

		/// <summary>
		/// Retrieves the value of an environment variable, using the specified target.
		/// </summary>
		/// <param name="variable">The name of the environment variable.</param>
		/// <param name="target">The target for the environment variable.</param>
		/// <returns>The value of the environment variable.</returns>
		public static Primitive GetEnvironmentVariable(Primitive variable, EnvironmentVariableTarget target)
		{
			return Environment.GetEnvironmentVariable(variable, target);
		}

        

		/// <summary>
		/// Returns the names of the logical drives on the current machine.
		/// </summary>
		/// <returns>An array of strings representing the names of the logical drives.</returns>
		public static Primitive GetLogicalDrives()
		{
			return Environment.GetLogicalDrives().ToString();
		}

		/// <summary>
		/// Retrieves the value of a resource string by its key.Currently WILL Give Nothing
		/// </summary>
		/// <param name="key">The key of the resource string.</param>
		/// <returns>The value of the resource string.</returns>
		public static Primitive GetResourceString(Primitive key)
		{
			// Note: `GetResourceString` is not a method of `System.Environment` in .NET.
			// This is just a placeholder for completeness.
			// You might need to implement your own resource string retrieval method.
			return string.Empty; 
		}

		/// <summary>
		/// Sets the value of an environment variable.
		/// </summary>
		/// <param name="variable">The name of the environment variable.</param>
		/// <param name="value">The value to set for the environment variable.</param>
		public static void SetEnvironmentVariable(Primitive variable, Primitive value)
		{
			Environment.SetEnvironmentVariable(variable, value);
		}

		/// <summary>
		/// Sets the value of an environment variable, using the specified target.
		/// </summary>
		/// <param name="variable">The name of the environment variable.</param>
		/// <param name="value">The value to set for the environment variable.</param>
		/// <param name="target">The target for the environment variable.</param>
		public static void SetEnvironmentVariable(Primitive variable, Primitive value, EnvironmentVariableTarget target)
		{
			Environment.SetEnvironmentVariable(variable, value, target);
		}
	}
}