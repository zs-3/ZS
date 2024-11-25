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
	/// Provides General Utilities Functions For Small Basic.
	/// </summary>
	[SmallBasicType]
	public static class ZSUtilities
	{
		[DllImport("user32.dll", SetLastError = true)]
		private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);

		private const uint MOUSEEVENTF_MOVE = 0x0001;
		private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
		private const uint MOUSEEVENTF_LEFTUP = 0x0004;
		private const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
		private const uint MOUSEEVENTF_RIGHTUP = 0x0010;
		
		[HideFromIntellisense]
		public static void OnError(Exception ex)
		{
			TextWindow.WriteLine("Method Name : " + ex.TargetSite.Name);
			TextWindow.WriteLine("Error Message : " + ex.Message);
			TextWindow.WriteLine("Stack Trace : " + ex.StackTrace);
			TextWindow.WriteLine("/n" + "Exception to string : " + ex.ToString());
		}

		/// <summary>
		/// Init zs utilities for any exception in program.
		/// </summary>
		public static void Init()
		{
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
			System.Windows.Application app = (System.Windows.Application)ZSReflection.GetFieldValue(typeof(Microsoft.SmallBasic.Library.Internal.SmallBasicApplication),"_application");
		}
		
		
		private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			// Handle non-UI thread exceptions
			Exception ex = (Exception)e.ExceptionObject;
			ZSUtilities.OnError(ex);
		}
		
		/// <summary>
		/// Sends a left mouse click at the specified screen coordinates.
		/// </summary>
		/// <param name="x">The x-coordinate of the screen position.</param>
		/// <param name="y">The y-coordinate of the screen position.</param>
		public static void SendLeftClick(Primitive x, Primitive y)
		{
			SetCursorPosition((uint)x, (uint)y);
			mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)x, (uint)y, 0, UIntPtr.Zero);
		}

		/// <summary>
		/// Sends a right mouse click at the specified screen coordinates.
		/// </summary>
		/// <param name="x">The x-coordinate of the screen position.</param>
		/// <param name="y">The y-coordinate of the screen position.</param>
		public static void SendRightClick(Primitive x, Primitive y)
		{
			SetCursorPosition((uint)x, (uint)y);
			mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, (uint)x, (uint)y, 0, UIntPtr.Zero);
		}

		/// <summary>
		/// Sets the cursor position to the specified screen coordinates.
		/// </summary>
		/// <param name="x">The x-coordinate of the screen position.</param>
		/// <param name="y">The y-coordinate of the screen position.</param>
		private static void SetCursorPosition(uint x, uint y)
		{
			System.Windows.Forms.Cursor.Position = new System.Drawing.Point((int)x, (int)y);
		}
	
	
		/// <summary>
		/// Returns the current system time as a string.
		/// </summary>
		/// <returns>The current system time in HH:mm:ss format.</returns>
		public static Primitive GetCurrentTime()
		{
			return DateTime.Now.ToString("HH:mm:ss");
		}

		/// <summary>
		/// Returns the current system date as a string.
		/// </summary>
		/// <returns>The current system date in yyyy-MM-dd format.</returns>
		public static Primitive GetCurrentDate()
		{
			return DateTime.Now.ToString("yyyy-MM-dd");
		}

		/// <summary>
		/// Generates a random number between the specified minimum and maximum values.
		/// </summary>
		/// <param name="min">The minimum value.</param>
		/// <param name="max">The maximum value.</param>
		/// <returns>A random number between min and max.</returns>
		public static Primitive GenerateRandomNumber(Primitive min, Primitive max)
		{
			Random random = new Random();
			return random.Next((int)min, (int)max + 1);
		}

		/// <summary>
		/// Converts the given string to uppercase.
		/// </summary>
		/// <param name="input">The string to convert.</param>
		/// <returns>The uppercase version of the input string.</returns>
		public static Primitive ConvertToUpperCase(Primitive input)
		{
			return input.ToString().ToUpper();
		}

		/// <summary>
		/// Converts the given string to lowercase.
		/// </summary>
		/// <param name="input">The string to convert.</param>
		/// <returns>The lowercase version of the input string.</returns>
		public static Primitive ConvertToLowerCase(Primitive input)
		{
			return input.ToString().ToLower();
		}

		/// <summary>
		/// Throws an exception with the specified message.
		/// </summary>
		/// <param name="message">The message for the exception.</param>
		public static void ThrowException(Primitive message)
		{
			throw new Exception(message.ToString());
		}

		/// <summary>
		/// Calculates the square root of a number.
		/// </summary>
		/// <param name="a">The number.</param>
		/// <returns>The square root of a.</returns>
		public static Primitive CalculateSquareRoot(Primitive a)
		{
			return System.Math.Sqrt(a);
		}

		/// <summary>
		/// Gets the machine name of the current computer.
		/// </summary>
		/// <returns>The machine name.</returns>
		public static Primitive GetMachineName()
		{
			return Environment.MachineName;
		}

		/// <summary>
		/// Gets the operating system version of the current computer.
		/// </summary>
		/// <returns>The operating system version.</returns>
		public static Primitive GetOSVersion()
		{
			return Environment.OSVersion.ToString();
		}

		/// <summary>
		/// Gets the value of an environment variable.
		/// </summary>
		/// <param name="variable">The name of the environment variable.</param>
		/// <returns>The value of the environment variable.</returns>
		public static Primitive GetEnvironmentVariable(Primitive variable)
		{
			return Environment.GetEnvironmentVariable(variable);
		}

		/// <summary>
		/// Opens the specified URL in the default web browser.
		/// </summary>
		/// <param name="url">The URL to open.</param>
		public static void OpenUrl(Primitive url)
		{
			System.Diagnostics.Process.Start(url.ToString());
		}

		/// <summary>
		/// Gets the current working directory.
		/// </summary>
		/// <returns>The current working directory.</returns>
		public static Primitive GetCurrentDirectory()
		{
			return Environment.CurrentDirectory;
		}

		/// <summary>
		/// Gets the user name of the currently logged-in user.
		/// </summary>
		/// <returns>The user name.</returns>
		public static Primitive GetUserName()
		{
			return Environment.UserName;
		}

		/// <summary>
		/// Gets the current culture name.
		/// </summary>
		/// <returns>The name of the current culture.</returns>
		public static Primitive GetCurrentCulture()
		{
			return System.Globalization.CultureInfo.CurrentCulture.Name;
		}

		/// <summary>
		/// Generates a random GUID (Globally Unique Identifier).
		/// </summary>
		/// <returns>A random GUID as a string.</returns>
		public static Primitive GetRandomGuid()
		{
			return Guid.NewGuid().ToString();
		}

		/// <summary>
		/// Plays a simple beep sound.
		/// </summary>
		public static void PlayBeep()
		{
			Console.Beep();
		}

		/// <summary>
		/// Checks if a file exists at the specified path.
		/// </summary>
		/// <param name="filePath">The path of the file.</param>
		/// <returns>True if the file exists, otherwise false.</returns>
		public static Primitive IsFileExists(Primitive filePath)
		{
			return System.IO.File.Exists(filePath.ToString());
		}

		/// <summary>
		/// Gets the path of the temporary folder.
		/// </summary>
		/// <returns>The path of the temporary folder.</returns>
		public static Primitive GetTempPath()
		{
			return System.IO.Path.GetTempPath();
		}

		/// <summary>
		/// Gets a list of all drive names on the current computer.
		/// </summary>
		/// <returns>A comma-separated list of drive names.</returns>
		public static Primitive GetAllDrives()
		{
			var drives = System.IO.DriveInfo.GetDrives();
			string driveNames = string.Join(",", drives.Select(d => d.Name));
			return driveNames;
		}

		/// <summary>
		/// Creates a directory at the specified path.
		/// </summary>
		/// <param name="path">The path of the directory to create.</param>
		public static void CreateDirectory(Primitive path)
		{
			System.IO.Directory.CreateDirectory(path.ToString());
		}

		/// <summary>
		/// Gets the process ID of the current process.
		/// </summary>
		/// <returns>The process ID of the current process.</returns>
		public static Primitive GetCurrentProcessId()
		{
			return System.Diagnostics.Process.GetCurrentProcess().Id;
		}

		/// <summary>
		/// Gets the path of the system directory.
		/// </summary>
		/// <returns>The path of the system directory.</returns>
		public static Primitive GetSystemDirectory()
		{
			return Environment.SystemDirectory;
		}

		/// <summary>
		/// Gets the path of the user profile directory.
		/// </summary>
		/// <returns>The path of the user profile directory.</returns>
		public static Primitive GetUserProfileDirectory()
		{
			return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
		}

		/// <summary>
		/// Gets the current UTC date and time.
		/// </summary>
		/// <returns>The current UTC date and time in yyyy-MM-dd HH:mm:ss format.</returns>
		public static Primitive GetUtcNow()
		{
			return DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
		}

		/// <summary>
		/// Gets the number of currently running processes on the system.
		/// </summary>
		/// <returns>The number of running processes.</returns>
		public static Primitive GetProcessCount()
		{
			return System.Diagnostics.Process.GetProcesses().Length;
		}


		/// <summary>
		/// Gets the ID of the current thread.
		/// </summary>
		/// <returns>The ID of the current thread.</returns>
		public static Primitive GetCurrentThreadId()
		{
			return System.Threading.Thread.CurrentThread.ManagedThreadId;
		}

		/// <summary>
		/// Gets the current local date and time.
		/// </summary>
		/// <returns>The current local date and time in yyyy-MM-dd HH:mm:ss format.</returns>
		public static Primitive GetLocalTime()
		{
			return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		}

		/// <summary>
		/// Gets the number of milliseconds elapsed since the system started.
		/// </summary>
		/// <returns>The number of milliseconds since the system started.</returns>
		public static Primitive GetTickCount()
		{
			return Environment.TickCount;
		}

		/// <summary>
		/// Checks if the current operating system is 64-bit.
		/// </summary>
		/// <returns>True if the operating system is 64-bit, otherwise false.</returns>
		public static Primitive CheckIs64BitOperatingSystem()
		{
			return Environment.Is64BitOperatingSystem;
		}

		/// <summary>
		/// Checks if the current process is running in 64-bit mode.
		/// </summary>
		/// <returns>True if the process is 64-bit, otherwise false.</returns>
		public static Primitive CheckIs64BitProcess()
		{
			return Environment.Is64BitProcess;
		}

		/// <summary>
		/// Gets the number of logical processors on the current machine.
		/// </summary>
		/// <returns>The number of logical processors.</returns>
		public static Primitive GetLogicalProcessors()
		{
			return Environment.ProcessorCount;
		}

		/// <summary>
		/// Gets the name of the current application domain.
		/// </summary>
		/// <returns>The name of the current application domain.</returns>
		public static Primitive GetAppDomainName()
		{
			return AppDomain.CurrentDomain.FriendlyName;
		}

		/// <summary>
		/// Gets the version of the currently executing assembly.
		/// </summary>
		/// <returns>The version of the current assembly.</returns>
		public static Primitive GetAssemblyVersion()
		{
			return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
		}

		/// <summary>
		/// Checks if the application is running with administrative privileges.
		/// </summary>
		/// <returns>True if running as an administrator, otherwise false.</returns>
		public static Primitive IsRunningAsAdmin()
		{
			var identity = System.Security.Principal.WindowsIdentity.GetCurrent();
			var principal = new System.Security.Principal.WindowsPrincipal(identity);
			return principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
		}

		/// <summary>
		/// Gets the current system time zone.
		/// </summary>
		/// <returns>The name of the current time zone.</returns>
		public static Primitive GetSystemTimeZone()
		{
			return TimeZoneInfo.Local.StandardName;
		}

		/// <summary>
		/// Gets a list of all logical drives on the system.
		/// </summary>
		/// <returns>A comma-separated list of drive names.</returns>
		public static Primitive GetLogicalDrives()
		{
			return string.Join(",", Environment.GetLogicalDrives());
		}

		/// <summary>
		/// Gets the title of the currently active window.
		/// </summary>
		/// <returns>The title of the active window.</returns>
		public static Primitive GetActiveWindowTitle()
		{
			const int nChars = 256;
			StringBuilder Buff = new StringBuilder(nChars);
			IntPtr handle = GetForegroundWindow();
			if (GetWindowText(handle, Buff, nChars) > 0) {
				return Buff.ToString();
			}
			return string.Empty;
		}

		[DllImport("user32.dll")]
		private static extern IntPtr GetForegroundWindow();

		[DllImport("user32.dll")]
		private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

		/// <summary>
		/// Captures a segment of the screen and saves it as an image file.
		/// </summary>
		/// <param name="x">The x-coordinate of the upper-left corner of the segment.</param>
		/// <param name="y">The y-coordinate of the upper-left corner of the segment.</param>
		/// <param name="width">The width of the segment.</param>
		/// <param name="height">The height of the segment.</param>
		/// <param name="filePath">The path where the image file will be saved.</param>
		public static void CaptureScreenSegment(Primitive x, Primitive y, Primitive width, Primitive height, Primitive filePath)
		{
			int startX = x;
			int startY = y;
			int segmentWidth = width;
			int segmentHeight = height;

			Bitmap bitmap = new Bitmap(segmentWidth, segmentHeight);
			Graphics graphics = Graphics.FromImage(bitmap);
			graphics.CopyFromScreen(startX, startY, 0, 0, new Size(segmentWidth, segmentHeight));
			bitmap.Save(filePath.ToString(), System.Drawing.Imaging.ImageFormat.Png);
			bitmap.Dispose();
			graphics.Dispose();
		}

		/// <summary>
		/// Checks if a debugger is attached to the process.
		/// </summary>
		/// <returns>True if a debugger is attached, otherwise false.</returns>
		public static Primitive IsDebuggerAttached()
		{
			return System.Diagnostics.Debugger.IsAttached;
		}

		/// <summary>
		/// Gets the ID of the current managed thread.
		/// </summary>
		/// <returns>The ID of the current managed thread.</returns>
		public static Primitive GetCurrentManagedThreadId()
		{
			return System.Threading.Thread.CurrentThread.ManagedThreadId;
		}

		/// <summary>
		/// Gets the path of the system directory.
		/// </summary>
		/// <returns>The path of the system directory.</returns>
		public static Primitive GetSystemDirectoryPath()
		{
			return Environment.SystemDirectory;
		}

		/// <summary>
		/// Gets the domain name associated with the current user.
		/// </summary>
		/// <returns>The domain name of the current user.</returns>
		public static Primitive GetUserDomainName()
		{
			return Environment.UserDomainName;
		}

		/// <summary>
		/// Gets the version of the operating system.
		/// </summary>
		/// <returns>The version of the operating system.</returns>
		public static Primitive GetOsVersion()
		{
			return Environment.OSVersion.ToString();
		}

		/// <summary>
		/// Checks if the current process is running in user-interactive mode.
		/// </summary>
		/// <returns>True if the process is running in user-interactive mode, otherwise false.</returns>
		public static Primitive GetUserInteractive()
		{
			return Environment.UserInteractive;
		}

		/// <summary>
		/// Gets the system uptime in seconds.
		/// </summary>
		/// <returns>The system uptime in seconds.</returns>
		public static Primitive GetSystemUptime()
		{
			return Environment.TickCount / 1000;
		}

		/// <summary>
		/// Gets the current UI culture of the operating system.
		/// </summary>
		/// <returns>The name of the current UI culture.</returns>
		public static Primitive GetCurrentUICulture()
		{
			return System.Globalization.CultureInfo.CurrentUICulture.Name;
		}

		/// <summary>
		/// Gets a list of installed font names on the system.
		/// </summary>
		/// <returns>A comma-separated list of installed font names.</returns>
		public static Primitive GetInstalledFontNames()
		{
			var fonts = new List<string>();
			using (var fontCollection = new InstalledFontCollection()) {
				foreach (var fontFamily in fontCollection.Families) {
					fonts.Add(fontFamily.Name);
				}
			}
			return string.Join(",", fonts);
		}

		/// <summary>
		/// Gets a list of network interface names on the system.
		/// </summary>
		/// <returns>A comma-separated list of network interface names.</returns>
		public static Primitive GetNetworkInterfaceNames()
		{
			var interfaces = NetworkInterface.GetAllNetworkInterfaces();
			var interfaceNames = interfaces.Select(ni => ni.Name).ToArray();
			return string.Join(",", interfaceNames);
		}

		/// <summary>
		/// Gets a list of running process names on the system.
		/// </summary>
		/// <returns>A comma-separated list of running process names.</returns>
		public static Primitive GetRunningProcesses()
		{
			var processes = Process.GetProcesses();
			var processNames = processes.Select(p => p.ProcessName).ToArray();
			return string.Join(",", processNames);
		}

		/// <summary>
		/// Gets the battery charge status.
		/// </summary>
		/// <returns>The battery charge status as a percentage.</returns>
		public static Primitive GetBatteryStatus()
		{
			var powerStatus = SystemInformation.PowerStatus;
			return powerStatus.BatteryLifePercent * 100;
		}

		/// <summary>
		/// Gets the current text content from the clipboard.
		/// </summary>
		/// <returns>The text content from the clipboard, or an empty string if the clipboard is empty or does not contain text.</returns>
		public static Primitive GetClipboardText()
		{
			string clipboardText = string.Empty;
			if (Clipboard.ContainsText()) {
				clipboardText = Clipboard.GetText();
			}
			return clipboardText;
		}

		/// <summary>
		/// Sets the specified text content to the clipboard.
		/// </summary>
		/// <param name="text">The text content to set to the clipboard.</param>
		public static void SetClipboardText(Primitive text)
		{
			Clipboard.SetText(text.ToString());
		}

		/// <summary>
		/// Gets the current CPU usage percentage.
		/// </summary>
		/// <returns>The current CPU usage percentage.</returns>
		public static Primitive GetCpuUsage()
		{
			var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
			cpuCounter.NextValue();
			System.Threading.Thread.Sleep(1000); // Allow time to collect data
			return cpuCounter.NextValue();
		}

		/// <summary>
		/// Gets the MAC address of the first operational network interface.
		/// </summary>
		/// <returns>The MAC address as a string, or an empty string if no operational network interface is found.</returns>
		public static Primitive GetMacAddress()
		{
			var interfaces = NetworkInterface.GetAllNetworkInterfaces();
			foreach (var ni in interfaces) {
				if (ni.OperationalStatus == OperationalStatus.Up) {
					return string.Join(":", ni.GetPhysicalAddress().GetAddressBytes().Select(b => b.ToString("X2")));
				}
			}
			return string.Empty;
		}

		/// <summary>
		/// Gets the local IP address of the machine.
		/// </summary>
		/// <returns>The local IP address, or an empty string if no network connection is found.</returns>
		public static Primitive GetIpAddress()
		{
			string localIp = string.Empty;
			var host = Dns.GetHostEntry(Dns.GetHostName());
			foreach (var ip in host.AddressList) {
				if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) {
					localIp = ip.ToString();
					break;
				}
			}
			return localIp;
		}

		/// <summary>
		/// Gets the system drive letter (e.g., "C").
		/// </summary>
		/// <returns>The system drive letter.</returns>
		public static Primitive GetSystemDrive()
		{
			return Path.GetPathRoot(Environment.SystemDirectory);
		}

		/// <summary>
		/// Gets the name of the current user.
		/// </summary>
		/// <returns>The name of the current user.</returns>
		public static Primitive GetCurrentUser()
		{
			return Environment.UserName;
		}

		/// <summary>
		/// Gets the size of the system's memory page in bytes.
		/// </summary>
		/// <returns>The size of the memory page in bytes.</returns>
		public static Primitive GetSystemPageSize()
		{
			return Environment.SystemPageSize;
		}

		/// <summary>
		/// Gets the memory usage of the current process in bytes.
		/// </summary>
		/// <returns>The memory usage of the current process in bytes.</returns>
		public static Primitive GetCurrentProcessMemoryUsage()
		{
			using (var process = Process.GetCurrentProcess()) {
				return (Primitive)(double)process.WorkingSet64;
			}
		}

		/// <summary>
		/// Downloads a file from the specified URL to the specified destination path.
		/// </summary>
		/// <param name="url">The URL of the file to download.</param>
		/// <param name="destinationPath">The path where the file will be saved.</param>
		public static void DownloadFile(Primitive url, Primitive destinationPath)
		{
			using (var client = new WebClient()) {
				client.DownloadFile(url.ToString(), destinationPath.ToString());
			}
		}

		/// <summary>
		/// Gets the external IP address of the machine.
		/// </summary>
		/// <returns>The external IP address as a string.</returns>
		public static Primitive GetExternalIpAddress()
		{
			using (var client = new WebClient()) {
				string ip = client.DownloadString("http://icanhazip.com").Trim();
				return ip;
			}
		}

		/// <summary>
		/// Gets the machine GUID (Globally Unique Identifier).
		/// </summary>
		/// <returns>The machine GUID as a string.</returns>
		public static Primitive GetMachineGuid()
		{
			string key = @"SOFTWARE\Microsoft\Cryptography";
			string value = "MachineGuid";

			using (var registryKey = Registry.LocalMachine.OpenSubKey(key)) {
				if (registryKey != null) {
					object machineGuid = registryKey.GetValue(value);
					if (machineGuid != null) {
						return machineGuid.ToString();
					}
				}
			}
			return string.Empty;
		}

		/// <summary>
		/// Gets a list of running services on the machine.
		/// </summary>
		/// <returns>A comma-separated list of running service names.</returns>
		public static Primitive GetRunningServices()
		{
			var services = ServiceController.GetServices();
			var runningServices = services.Where(s => s.Status == ServiceControllerStatus.Running)
                                  .Select(s => s.ServiceName)
                                  .ToArray();
			return string.Join(",", runningServices);
		}

		/// <summary>
		/// Captures a screenshot of the active window and saves it as an image file.
		/// </summary>
		/// <param name="filePath">The path where the image file will be saved.</param>
		public static void CaptureActiveWindow(Primitive filePath)
		{
			var handle = GetForegroundWindow();
			RECT rect;
			GetWindowRect(handle, out rect);

			int width = rect.Right - rect.Left;
			int height = rect.Bottom - rect.Top;

			var bitmap = new Bitmap(width, height);
			var graphics = Graphics.FromImage(bitmap);
			graphics.CopyFromScreen(rect.Left, rect.Top, 0, 0, new Size(width, height));
			bitmap.Save(filePath.ToString(), System.Drawing.Imaging.ImageFormat.Png);
			bitmap.Dispose();
			graphics.Dispose();
		}


		[DllImport("user32.dll")]
		private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

		private struct RECT
		{
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;
		}

		/// <summary>
		/// Starts a process with the specified arguments.
		/// </summary>
		/// <param name="processName">The name of the process to start.</param>
		/// <param name="arguments">The arguments to pass to the process.</param>
		public static void StartProcessWithArguments(Primitive processName, Primitive arguments)
		{
			var startInfo = new ProcessStartInfo {
				FileName = processName.ToString(),
				Arguments = arguments.ToString(),
				UseShellExecute = true
			};
			Process.Start(startInfo);
		}

		/// <summary>
		/// Kills a process by its name.
		/// </summary>
		/// <param name="processName">The name of the process to kill.</param>
		public static void KillProcessByName(Primitive processName)
		{
			var processes = Process.GetProcessesByName(processName.ToString());
			foreach (var process in processes) {
				process.Kill();
			}
		}

		/// <summary>
		/// Gets a list of all files in the current directory.
		/// </summary>
		/// <returns>A comma-separated list of file names in the current directory.</returns>
		public static Primitive GetCurrentDirectoryFiles()
		{
			var files = Directory.GetFiles(Environment.CurrentDirectory);
			return string.Join(",", files.Select(Path.GetFileName));
		}

		/// <summary>
		/// Sends an email using SMTP.
		/// </summary>
		/// <param name="to">The recipient email address.</param>
		/// <param name="subject">The email subject.</param>
		/// <param name="body">The email body.</param>
		/// <param name="smtpServer">The SMTP server address.</param>
		/// <param name="smtpPort">The SMTP server port.</param>
		/// <param name="username">The SMTP server username.</param>
		/// <param name="password">The SMTP server password.</param>
		public static void SendEmail(Primitive to, Primitive subject, Primitive body, Primitive smtpServer, Primitive smtpPort, Primitive username, Primitive password)
		{
			var mail = new MailMessage();
			mail.To.Add(to.ToString());
			mail.Subject = subject.ToString();
			mail.Body = body.ToString();
			mail.From = new MailAddress(username.ToString());

			var smtp = new SmtpClient(smtpServer.ToString(), smtpPort);
			smtp.Credentials = new System.Net.NetworkCredential(username.ToString(), password.ToString());
			smtp.EnableSsl = true;
			smtp.Send(mail);
			
		}
		
		/// <summary>
		/// Generates a unique identifier (UUID) as a string.
		/// </summary>
		/// <returns>A UUID string.</returns>
		public static Primitive GenerateUUID()
		{
			return Guid.NewGuid().ToString();
		}

		/// <summary>
		/// Converts a number from one base to another base.
		/// </summary>
		/// <param name="number">The number to convert.</param>
		/// <param name="fromBase">The base of the input number.</param>
		/// <param name="toBase">The base to convert the number to.</param>
		/// <returns>The converted number as a string.</returns>
		public static Primitive ConvertBase(Primitive number, Primitive fromBase, Primitive toBase)
		{
			var numberString = number.ToString();
			var fromBaseInt = (int)fromBase;
			var toBaseInt = (int)toBase;
			var decimalValue = Convert.ToInt32(numberString, fromBaseInt);
			return Convert.ToString(decimalValue, toBaseInt);
		}

		/// <summary>
		/// Gets a random item from an array of items.
		/// </summary>
		/// <param name="items">An array of items.</param>
		/// <returns>A random item from the array.</returns>
		public static Primitive GetRandomItem(Primitive[] items)
		{
			var random = new Random();
			return items[random.Next(items.Length)];
		}

		/// <summary>
		/// Checks if a given string is a palindrome.
		/// </summary>
		/// <param name="text">The string to check.</param>
		/// <returns>True if the string is a palindrome, otherwise false.</returns>
		public static Primitive IsPalindrome(Primitive text)
		{
			var str = text.ToString();
			var reversed = new string(str.Reverse().ToArray());
			return str.Equals(reversed, StringComparison.OrdinalIgnoreCase);
		}

		/// <summary>
		/// Calculates the sum of the digits of a given number.
		/// </summary>
		/// <param name="number">The number whose digits are to be summed.</param>
		/// <returns>The sum of the digits.</returns>
		public static Primitive SumOfDigits(Primitive number)
		{
			return number.ToString().Sum(c => c - '0');
		}

		/// <summary>
		/// Generates a Fibonacci sequence of a given length.
		/// </summary>
		/// <param name="length">The length of the Fibonacci sequence.</param>
		/// <returns>An array containing the Fibonacci sequence.</returns>
		public static Primitive[] GenerateFibonacciSequence(Primitive length)
		{
			int n = (int)length;
			var sequence = new int[n];
			if (n > 0)
				sequence[0] = 0;
			if (n > 1)
				sequence[1] = 1;
			for (int i = 2; i < n; i++) {
				sequence[i] = sequence[i - 1] + sequence[i - 2];
			}
			return sequence.Cast<Primitive>().ToArray();
		}

		/// <summary>
		/// Checks if a given number is a prime number.
		/// </summary>
		/// <param name="number">The number to check.</param>
		/// <returns>True if the number is prime, otherwise false.</returns>
		public static Primitive IsPrime(Primitive number)
		{
			int n = (int)number;
			if (n <= 1)
				return false;
			if (n <= 3)
				return true;
			if (n % 2 == 0 || n % 3 == 0)
				return false;
			for (int i = 5; i * i <= n; i += 6) {
				if (n % i == 0 || n % (i + 2) == 0)
					return false;
			}
			return true;
		}

		/// <summary>
		/// Calculates the Levenshtein distance between two strings.
		/// </summary>
		/// <param name="source">The source string.</param>
		/// <param name="target">The target string.</param>
		/// <returns>The Levenshtein distance between the two strings.</returns>
		public static Primitive LevenshteinDistance(Primitive source, Primitive target)
		{
			var s = source.ToString();
			var t = target.ToString();
			var dp = new int[s.Length + 1, t.Length + 1];
			for (int i = 0; i <= s.Length; i++)
				dp[i, 0] = i;
			for (int j = 0; j <= t.Length; j++)
				dp[0, j] = j;
			for (int i = 1; i <= s.Length; i++) {
				for (int j = 1; j <= t.Length; j++) {
					var cost = (s[i - 1] == t[j - 1]) ? 0 : 1;
					dp[i, j] = System.Math.Min(System.Math.Min(dp[i - 1, j] + 1, dp[i, j - 1] + 1), dp[i - 1, j - 1] + cost);
				}
			}
			return dp[s.Length, t.Length];
		}

		/// <summary>
		/// Converts a given string to title case (each word starts with a capital letter).
		/// </summary>
		/// <param name="text">The text to convert.</param>
		/// <returns>The text in title case.</returns>
		public static Primitive ConvertToTitleCase(Primitive text)
		{
			var words = text.ToString().ToLower().Split(' ');
			var titleCased = words.Select(word => char.ToUpper(word[0]) + word.Substring(1)).Aggregate((current, next) => current + " " + next);
			return titleCased;
		}

		/// <summary>
		/// Calculates the factorial of a given number.
		/// </summary>
		/// <param name="number">The number to calculate the factorial for.</param>
		/// <returns>The factorial of the number.</returns>
		public static Primitive Factorial(Primitive number)
		{
			int n = (int)number;
			if (n < 0)
				throw new ArgumentException("Number must be non-negative.");
			return Enumerable.Range(1, n).Aggregate(1, (acc, x) => acc * x);
		}

		/// <summary>
		/// Finds the median value in a given array of numbers.
		/// </summary>
		/// <param name="array">The array of numbers.</param>
		/// <returns>The median value of the array.</returns>
		public static Primitive FindMedian(Primitive[] array)
		{
			var sorted = array.OrderBy(n => n).ToArray();
			int middle = sorted.Length / 2;
			if (sorted.Length % 2 == 0) {
				return (sorted[middle - 1] + sorted[middle]) / 2;
			}
			return sorted[middle];
		}

		/// <summary>
		/// Checks if a given number is an Armstrong number (Narcissistic number).
		/// </summary>
		/// <param name="number">The number to check.</param>
		/// <returns>True if the number is an Armstrong number, otherwise false.</returns>
		public static Primitive IsArmstrong(Primitive number)
		{
			var numStr = number.ToString();
			var numLength = numStr.Length;
			var sum = numStr.Sum(digit => (int)System.Math.Pow(digit - '0', numLength));
			return sum == (int)number;
		}

		/// <summary>
		/// Counts the number of words in a given string.
		/// </summary>
		/// <param name="text">The text to count words in.</param>
		/// <returns>The number of words in the text.</returns>
		public static Primitive CountWords(Primitive text)
		{
			return text.ToString().Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
		}

		/// <summary>
		/// Generates a list of prime numbers up to a specified number.
		/// </summary>
		/// <param name="limit">The upper limit to generate prime numbers up to.</param>
		/// <returns>An array of prime numbers up to the specified limit.</returns>
		public static Primitive[] GeneratePrimesUpTo(Primitive limit)
		{
			var num = (int)limit;
			var primes = new List<int>();
			for (int i = 2; i <= num; i++) {
				if (IsPrime(i))
					primes.Add(i);
			}
			return primes.Cast<Primitive>().ToArray();
		}

		/// <summary>
		/// Calculates the length of the hypotenuse in a right triangle given the lengths of the other two sides.
		/// </summary>
		/// <param name="a">The length of one side.</param>
		/// <param name="b">The length of the other side.</param>
		/// <returns>The length of the hypotenuse.</returns>
		public static Primitive CalculateHypotenuse(Primitive a, Primitive b)
		{
			return System.Math.Sqrt(System.Math.Pow((double)a, 2) + System.Math.Pow((double)b, 2));
		}

		/// <summary>
		/// Counts the number of vowels in a given string.
		/// </summary>
		/// <param name="text">The text to count vowels in.</param>
		/// <returns>The number of vowels in the text.</returns>
		public static Primitive CountVowels(Primitive text)
		{
			var vowels = "aeiouAEIOU";
			return text.ToString().Count(c => vowels.Contains(c));
		}

		/// <summary>
		/// Finds all substrings of a specified length from a given string.
		/// </summary>
		/// <param name="text">The text to extract substrings from.</param>
		/// <param name="length">The length of each substring.</param>
		/// <returns>An array of substrings of the specified length.</returns>
		public static Primitive[] FindSubstrings(Primitive text, Primitive length)
		{
			var str = text.ToString();
			var substrings = new List<string>();
			for (int i = 0; i <= str.Length - (int)length; i++) {
				substrings.Add(str.Substring(i, (int)length));
			}
			return substrings.Cast<Primitive>().ToArray();
		}

		/// <summary>
		/// Converts RGB color values to a hex color code.
		/// </summary>
		/// <param name="red">The red component (0-255).</param>
		/// <param name="green">The green component (0-255).</param>
		/// <param name="blue">The blue component (0-255).</param>
		/// <returns>The hex color code as a string.</returns>
		public static Primitive RGBToHex(Primitive red, Primitive green, Primitive blue)
		{
			int r = (int)red;
			int g = (int)green;
			int b = (int)blue;
			return string.Format("#{0:X2}{1:X2}{2:X2}", r, g, b);
		}

		/// <summary>
		/// Checks if a given string represents a numeric value.
		/// </summary>
		/// <param name="text">The string to check.</param>
		/// <returns>True if the string is numeric, otherwise false.</returns>
		public static Primitive IsNumeric(Primitive text)
		{
			double number;
			return double.TryParse(text.ToString(), out number);
		}

		/// <summary>
		/// Removes duplicate values from an array.
		/// </summary>
		/// <param name="array">The array with possible duplicate values.</param>
		/// <returns>An array with duplicates removed.</returns>
		public static Primitive[] RemoveDuplicates(Primitive[] array)
		{
			return array.Distinct().ToArray();
		}

		/// <summary>
		/// Gets the name of the day of the week for a given date.
		/// </summary>
		/// <param name="date">The date to get the day of the week for.</param>
		/// <returns>The name of the day of the week.</returns>
		public static Primitive GetDayOfWeek(Primitive date)
		{
			DateTime dt = DateTime.Parse(date.ToString());
			return dt.DayOfWeek.ToString();
		}

		/// <summary>
		/// Sorts an array of numbers in descending order.
		/// </summary>
		/// <param name="array">The array to sort.</param>
		/// <returns>The sorted array in descending order.</returns>
		public static Primitive[] SortDescending(Primitive[] array)
		{
			return array.OrderByDescending(n => n).ToArray();
		}

		/// <summary>
		/// Calculates the age of a person given their birthdate.
		/// </summary>
		/// <param name="birthdate">The birthdate to calculate the age from.</param>
		/// <returns>The age in years.</returns>
		public static Primitive CalculateAge(Primitive birthdate)
		{
			DateTime dob = DateTime.Parse(birthdate.ToString());
			DateTime today = DateTime.Today;
			int age = today.Year - dob.Year;
			if (dob.Date > today.AddYears(-age))
				age--;
			return age;
		}

		/// <summary>
		/// Checks if a given string contains only letters.
		/// </summary>
		/// <param name="text">The string to check.</param>
		/// <returns>True if the string contains only letters, otherwise false.</returns>
		public static Primitive ContainsOnlyLetters(Primitive text)
		{
			return text.ToString().All(char.IsLetter);
		}

		/// <summary>
		/// Calculates compound interest over a specified number of periods.
		/// </summary>
		/// <param name="principal">The principal amount.</param>
		/// <param name="rate">The annual interest rate (as a decimal).</param>
		/// <param name="periods">The number of periods per year.</param>
		/// <param name="years">The number of years the money is invested for.</param>
		/// <returns>The compound interest amount.</returns>
		public static Primitive CalculateCompoundInterest(Primitive principal, Primitive rate, Primitive periods, Primitive years)
		{
			double p = (double)principal;
			double r = (double)rate;
			double n = (double)periods;
			double t = (double)years;
			double compoundInterest = p * System.Math.Pow(1 + r / n, n * t) - p;
			return compoundInterest;
		}

		/// <summary>
		/// Converts a given string to Base64 encoding.
		/// </summary>
		/// <param name="text">The string to encode.</param>
		/// <returns>The Base64 encoded string.</returns>
		public static Primitive ConvertToBase64(Primitive text)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(text.ToString());
			return Convert.ToBase64String(bytes);
		}

		/// <summary>
		/// Finds the maximum value in an array of numbers.
		/// </summary>
		/// <param name="array">The array to search.</param>
		/// <returns>The maximum value in the array.</returns>
		public static Primitive FindMax(Primitive[] array)
		{
			return array.Max();
		}

		/// <summary>
		/// Finds the minimum value in an array of numbers.
		/// </summary>
		/// <param name="array">The array to search.</param>
		/// <returns>The minimum value in the array.</returns>
		public static Primitive FindMin(Primitive[] array)
		{
			return array.Min();
		}

		/// <summary>
		/// Calculates the greatest common divisor (GCD) of two integers using the Euclidean algorithm.
		/// </summary>
		/// <param name="a">The first integer.</param>
		/// <param name="b">The second integer.</param>
		/// <returns>The GCD of the two integers.</returns>
		public static Primitive CalculateGCD(Primitive a, Primitive b)
		{
			int x = (int)a;
			int y = (int)b;
			while (y != 0) {
				int temp = y;
				y = x % y;
				x = temp;
			}
			return x;
		}

		/// <summary>
		/// Generates a random password of a specified length containing letters and digits.
		/// </summary>
		/// <param name="length">The length of the password.</param>
		/// <returns>The generated password.</returns>
		public static Primitive GenerateRandomPassword(Primitive length)
		{
			int len = (int)length;
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			Random random = new Random();
			return new string(Enumerable.Repeat(chars, len)
        .Select(s => s[random.Next(s.Length)]).ToArray());
		}

		/// <summary>
		/// Converts a DateTime object to ISO 8601 format (yyyy-MM-ddTHH:mm:ssZ).
		/// </summary>
		/// <param name="dateTime">The DateTime object to convert.</param>
		/// <returns>The ISO 8601 formatted date string.</returns>
		public static Primitive ToIso8601(Primitive dateTime)
		{
			DateTime dt = DateTime.Parse(dateTime.ToString());
			return dt.ToString("yyyy-MM-ddTHH:mm:ssZ");
		}

		/// <summary>
		/// Merges two arrays into a single array.
		/// </summary>
		/// <param name="array1">The first array.</param>
		/// <param name="array2">The second array.</param>
		/// <returns>The merged array.</returns>
		public static Primitive[] MergeArrays(Primitive[] array1, Primitive[] array2)
		{
			return array1.Concat(array2).ToArray();
		}

		/// <summary>
		/// Checks if a given number is even.
		/// </summary>
		/// <param name="number">The number to check.</param>
		/// <returns>True if the number is even, otherwise false.</returns>
		public static Primitive IsEven(Primitive number)
		{
			int num = (int)number;
			return num % 2 == 0;
		}

		/// <summary>
		/// Checks if a given number is odd.
		/// </summary>
		/// <param name="number">The number to check.</param>
		/// <returns>True if the number is odd, otherwise false.</returns>
		public static Primitive IsOdd(Primitive number)
		{
			int num = (int)number;
			return num % 2 != 0;
		}

		/// <summary>
		/// Converts a Base64 encoded string back to its original string representation.
		/// </summary>
		/// <param name="base64">The Base64 encoded string.</param>
		/// <returns>The decoded string.</returns>
		public static Primitive ConvertFromBase64(Primitive base64)
		{
			byte[] bytes = Convert.FromBase64String(base64.ToString());
			return Encoding.UTF8.GetString(bytes);
		}

		/// <summary>
		/// Gets a color from its RGB components.
		/// </summary>
		/// <param name="red">The red component (0-255).</param>
		/// <param name="green">The green component (0-255).</param>
		/// <param name="blue">The blue component (0-255).</param>
		/// <returns>The color in string format.</returns>
		public static Primitive GetColorFromRGB(Primitive red, Primitive green, Primitive blue)
		{
			Color color = Color.FromArgb((int)red, (int)green, (int)blue);
			return color.ToArgb().ToString();
		}

		/// <summary>
		/// Converts a color from its RGB components to a hexadecimal string.
		/// </summary>
		/// <param name="color">The color in RGB string format.</param>
		/// <returns>The color in hexadecimal format.</returns>
		public static Primitive ConvertColorToHex(Primitive color)
		{
			Color c = ColorTranslator.FromHtml(color.ToString());
			return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
		}

		/// <summary>
		/// Converts a hexadecimal color string to an RGB color.
		/// </summary>
		/// <param name="hexColor">The hexadecimal color string (e.g., "#RRGGBB").</param>
		/// <returns>An array containing the RGB values.</returns>
		public static Primitive[] HexToRGB(Primitive hexColor)
		{
			string hex = hexColor.ToString().TrimStart('#');
			int r = Convert.ToInt32(hex.Substring(0, 2), 16);
			int g = Convert.ToInt32(hex.Substring(2, 2), 16);
			int b = Convert.ToInt32(hex.Substring(4, 2), 16);
			return new Primitive[] { r, g, b };
		}

		
		
		
		
	
	}
}