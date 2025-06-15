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



	/// <summary>
	/// The ZSKeyboard extension provides functionalities to interact with keyboard inputs.
	/// It includes methods for checking the state of modifier keys (Alt, Shift, Ctrl, Windows) and sending key presses programmatically.
	/// </summary>
	[SmallBasicType]
	public static class ZSKeyboard
	{
		private static string lastKeyPressed;
		private static bool isAltDown;
		private static bool isShiftDown;
		private static bool isCtrlDown;
		private static bool isWindowsKeyDown;

		private static Form form = new Form();

		static ZSKeyboard()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			form.KeyDown += Form_KeyDown;
			form.KeyUp += Form_KeyUp;

			System.Threading.Thread thread = new System.Threading.Thread(() => {
				Application.Run(form);
			});

			thread.SetApartmentState(System.Threading.ApartmentState.STA);
			thread.Start();
		}

		/// <summary>
		/// Gets the last key pressed by the user.
		/// </summary>
		public static Primitive LastKeyPressed {
			get { return lastKeyPressed; }
		}

		/// <summary>
		/// Checks if the Alt key is currently pressed down.
		/// </summary>
		/// <returns>True if the Alt key is pressed, otherwise false.</returns>
		public static Primitive IsAltDown()
		{
			return isAltDown;
		}

		/// <summary>
		/// Checks if the Shift key is currently pressed down.
		/// </summary>
		/// <returns>True if the Shift key is pressed, otherwise false.</returns>
		public static Primitive IsShiftDown()
		{
			return isShiftDown;
		}

		/// <summary>
		/// Checks if the Ctrl key is currently pressed down.
		/// </summary>
		/// <returns>True if the Ctrl key is pressed, otherwise false.</returns>
		public static Primitive IsCtrlDown()
		{
			return isCtrlDown;
		}

		/// <summary>
		/// Checks if the Windows key is currently pressed down.
		/// </summary>
		/// <returns>True if the Windows key is pressed, otherwise false.</returns>
		public static Primitive IsWindowsKeyDown()
		{
			return isWindowsKeyDown;
		}

		/// <summary>
		/// Sends a key press to the system.
		/// </summary>
		/// <param name="key">The key to send.</param>
		public static void SendKey(Primitive key)
		{
			SendKeys.SendWait(key.ToString());
		}

		/// <summary>
		/// Toggles the state of the Caps Lock key.
		/// </summary>
		public static void ToggleCapsLock()
		{
			SendKeys.SendWait("{CAPSLOCK}");
		}

		/// <summary>
		/// Simulates pressing a combination of keys.
		/// </summary>
		/// <param name="keyCombination">The key combination to press (e.g., "^C" for Ctrl+C, "%{F4}" for Alt+F4).</param>
		/// <example>
		/// "^C" = Ctrl+C
		/// "%F4" = Alt+F4
		/// "+A" = Shift+A
		/// "{DEL}" = Delete key
		/// "{ENTER}" = Enter key
		/// "{TAB}" = Tab key
		/// "{ESC}" = Escape key
		/// "{BACKSPACE}" = Backspace key
		/// </example>
		public static void SendKeyCombination(Primitive keyCombination)
		{
			SendKeys.SendWait(keyCombination.ToString());
		}

		/// <summary>
		/// Checks if the Caps Lock is currently on.
		/// </summary>
		/// <returns>True if Caps Lock is on, otherwise false.</returns>
		public static Primitive IsCapsLockOn()
		{
			return Control.IsKeyLocked(Keys.CapsLock);
		}

		/// <summary>
		/// Checks if the Num Lock is currently on.
		/// </summary>
		/// <returns>True if Num Lock is on, otherwise false.</returns>
		public static Primitive IsNumLockOn()
		{
			return Control.IsKeyLocked(Keys.NumLock);
		}

		/// <summary>
		/// Checks if the Scroll Lock is currently on.
		/// </summary>
		/// <returns>True if Scroll Lock is on, otherwise false.</returns>
		public static Primitive IsScrollLockOn()
		{
			return Control.IsKeyLocked(Keys.Scroll);
		}

		/// <summary>
		/// Simulates typing a string of text.
		/// </summary>
		/// <param name="text">The text to type.</param>
		public static void TypeText(Primitive text)
		{
			SendKeys.SendWait(text.ToString());
		}

		/// <summary>
		/// Sets the keyboard layout.
		/// </summary>
		/// <param name="layout">The keyboard layout (e.g., "00000409" for US layout).</param>
		public static void SetKeyboardLayout(Primitive layout)
		{
			LoadKeyboardLayout(layout.ToString(), KLF_ACTIVATE);
		}

		/// <summary>
		/// Simulates pressing down a key.
		/// </summary>
		/// <param name="key">The key to press down.</param>
		public static void SimulateKeyDown(Primitive key)
		{
			SendKeys.SendWait("{" + key + " down}");
		}

		/// <summary>
		/// Simulates releasing a key.
		/// </summary>
		/// <param name="key">The key to release.</param>
		public static void SimulateKeyUp(Primitive key)
		{
			SendKeys.SendWait("{" + key + " up}");
		}

		/// <summary>
		/// Waits for a specified amount of time.
		/// </summary>
		/// <param name="milliseconds">The amount of time to wait in milliseconds.</param>
		public static void Wait(Primitive milliseconds)
		{
			System.Threading.Thread.Sleep(milliseconds);
		}

		/// <summary>
		/// Simulates pressing and releasing a key.
		/// </summary>
		/// <param name="key">The key to press and release.</param>
		public static void PressAndReleaseKey(Primitive key)
		{
			SendKeys.SendWait("{" + key + "}");
		}

		/// <summary>
		/// Simulates pressing a key multiple times.
		/// </summary>
		/// <param name="key">The key to press.</param>
		/// <param name="times">The number of times to press the key.</param>
		public static void PressKeyMultipleTimes(Primitive key, Primitive times)
		{
			for (int i = 0; i < times; i++) {
				SendKeys.SendWait(key.ToString());
			}
		}

		private static void Form_KeyDown(object sender, KeyEventArgs e)
		{
			lastKeyPressed = e.KeyCode.ToString();
			isAltDown = e.Alt;
			isShiftDown = e.Shift;
			isCtrlDown = e.Control;
			isWindowsKeyDown = (e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin);
		}

		private static void Form_KeyUp(object sender, KeyEventArgs e)
		{
			isAltDown = e.Alt;
			isShiftDown = e.Shift;
			isCtrlDown = e.Control;
			isWindowsKeyDown = (e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin);
		}

		[DllImport("user32.dll")]
		private static extern IntPtr LoadKeyboardLayout(string pwszKLID, uint Flags);

		private const uint KLF_ACTIVATE = 0x00000001;
		
		/// <summary>
		/// Simulates pressing the Enter key.
		/// </summary>
		public static void SimulateEnterKeyPress()
		{
			SendKeys.SendWait("{ENTER}");
		}

		/// <summary>
		/// Simulates pressing the Escape key.
		/// </summary>
		public static void SimulateEscapeKeyPress()
		{
			SendKeys.SendWait("{ESC}");
		}

		/// <summary>
		/// Simulates pressing the Tab key.
		/// </summary>
		public static void SimulateTabKeyPress()
		{
			SendKeys.SendWait("{TAB}");
		}

		/// <summary>
		/// Simulates pressing the Backspace key.
		/// </summary>
		public static void SimulateBackspaceKeyPress()
		{
			SendKeys.SendWait("{BACKSPACE}");
		}

		/// <summary>
		/// Simulates pressing the Delete key.
		/// </summary>
		public static void SimulateDeleteKeyPress()
		{
			SendKeys.SendWait("{DEL}");
		}

		/// <summary>
		/// Gets the current state of a specified key.
		/// </summary>
		/// <param name="key">The key to check the state for (e.g., "A", "B", "1", "2").</param>
		/// <returns>True if the key is pressed, otherwise false.</returns>
		public static bool GetKeyState(Primitive key)
		{
			int vKey;

			if (key is int) {
				vKey = (int)key;
			} else if (key is string) {
				if (!int.TryParse((string)key, out vKey)) {
					throw new ArgumentException("Invalid key format. Key must be a valid integer or string representation of an integer.");
				}
			} else {
				throw new ArgumentException("Invalid key type. Key must be either an integer or a string.");
			}

			return (GetAsyncKeyState(vKey) & 0x8000) != 0;
		}

		[DllImport("user32.dll")]
		private static extern short GetAsyncKeyState(int vKey);


		/// <summary>
		/// Retrieves the current keyboard layout.
		/// </summary>
		/// <returns>The current keyboard layout as a string.</returns>
		public static string GetKeyboardLayout()
		{
			return InputLanguage.CurrentInputLanguage.Culture.DisplayName;
		}


		/// <summary>
		/// Retrieves the language of the active keyboard layout.
		/// </summary>
		/// <returns>The language of the active keyboard layout as a string.</returns>
		public static string GetActiveKeyboardLayoutLanguage()
		{
			return InputLanguage.CurrentInputLanguage.LayoutName;
		}

		/// <summary>
		/// Retrieves the current state of all keyboard keys.
		/// </summary>
		/// <returns>An array of booleans indicating the state of each key.</returns>
		public static bool[] GetKeyboardState()
		{
			bool[] keyStates = new bool[256];

			for (int i = 0; i < 256; i++) {
				keyStates[i] = (GetAsyncKeyState(i) & 0x8000) != 0;
			}

			return keyStates;
		}


		/// <summary>
		/// Retrieves the identifier of the active keyboard layout.
		/// </summary>
		/// <returns>The identifier of the active keyboard layout as an integer.</returns>
		public static int GetActiveKeyboardLayoutId()
		{
			return InputLanguage.CurrentInputLanguage.Handle.ToInt32();
		}

		/// <summary>
		/// Retrieves the language code (LCID) of the current keyboard layout.
		/// </summary>
		/// <returns>The language code (LCID) of the current keyboard layout.</returns>
		public static int GetCurrentKeyboardLayoutLanguageCode()
		{
			return InputLanguage.CurrentInputLanguage.Culture.LCID;
		}



		/// <summary>
		/// Clears the last recorded key press.
		/// </summary>
		public static void ClearLastKeyPressed()
		{
			lastKeyPressed = null; // Or set it to an empty string if preferred
		}


		/// <summary>
		/// Retrieves the number of installed keyboard layouts.
		/// </summary>
		/// <returns>The number of installed keyboard layouts.</returns>
		public static int GetKeyboardLayoutsCount()
		{
			return InputLanguage.InstalledInputLanguages.Count;
		}

		
		/// <summary>
		/// Retrieves the identifier (HKL) of the current keyboard layout.
		/// </summary>
		/// <returns>The identifier (HKL) of the current keyboard layout.</returns>
		public static IntPtr GetCurrentKeyboardLayoutId()
		{
			return InputLanguage.CurrentInputLanguage.Handle;
		}
		
		
	}

	
	/// <summary>
	/// Provides a comprehensive set of text manipulation functions, enhancing the capability of Small Basic programs to process and transform strings with ease.
	/// </summary>
	[SmallBasicType]
	public static class ZSText
	{
		/// <summary>
		/// Converts the input text to uppercase.
		/// </summary>
		/// <param name="input">The text to be converted.</param>
		/// <returns>The uppercase version of the input text.</returns>
		public static Primitive ToUpper(Primitive input)
		{
			return input.ToString().ToUpper();
		}

		/// <summary>
		/// Converts the input text to lowercase.
		/// </summary>
		/// <param name="input">The text to be converted.</param>
		/// <returns>The lowercase version of the input text.</returns>
		public static Primitive ToLower(Primitive input)
		{
			return input.ToString().ToLower();
		}

		/// <summary>
		/// Extracts a substring from the input text.
		/// </summary>
		/// <param name="input">The text to extract from.</param>
		/// <param name="startIndex">The starting index of the substring.</param>
		/// <param name="length">The length of the substring.</param>
		/// <returns>The extracted substring.</returns>
		public static Primitive Substring(Primitive input, Primitive startIndex, Primitive length)
		{
			string str = input.ToString();
			int start = startIndex;
			int len = length;
			return str.Substring(start, len);
		}

		/// <summary>
		/// Checks if the input text contains the specified value.
		/// </summary>
		/// <param name="input">The text to search in.</param>
		/// <param name="value">The value to search for.</param>
		/// <returns>True if the input text contains the value, otherwise false.</returns>
		public static Primitive Contains(Primitive input, Primitive value)
		{
			return input.ToString().Contains(value.ToString());
		}

		/// <summary>
		/// Finds the index of the specified value in the input text.
		/// </summary>
		/// <param name="input">The text to search in.</param>
		/// <param name="value">The value to find.</param>
		/// <returns>The index of the value in the input text, or -1 if not found.</returns>
		public static Primitive IndexOf(Primitive input, Primitive value)
		{
			return input.ToString().IndexOf(value.ToString());
		}

		/// <summary>
		/// Replaces occurrences of a specified value in the input text with another value.
		/// </summary>
		/// <param name="input">The text to be modified.</param>
		/// <param name="oldValue">The value to be replaced.</param>
		/// <param name="newValue">The value to replace with.</param>
		/// <returns>The modified text with replacements.</returns>
		public static Primitive Replace(Primitive input, Primitive oldValue, Primitive newValue)
		{
			return input.ToString().Replace(oldValue.ToString(), newValue.ToString());
		}

		/// <summary>
		/// Gets the length of the input text.
		/// </summary>
		/// <param name="input">The text to measure.</param>
		/// <returns>The length of the input text.</returns>
		public static Primitive Length(Primitive input)
		{
			return input.ToString().Length;
		}

		/// <summary>
		/// Trims leading and trailing whitespace from the input text.
		/// </summary>
		/// <param name="input">The text to be trimmed.</param>
		/// <returns>The trimmed text.</returns>
		public static Primitive Trim(Primitive input)
		{
			return input.ToString().Trim();
		}

		/// <summary>
		/// Splits the input text into an array of substrings based on a delimiter.
		/// </summary>
		/// <param name="input">The text to be split.</param>
		/// <param name="delimiter">The delimiter to split by.</param>
		/// <returns>An array of substrings.</returns>
		public static Primitive Split(Primitive input, Primitive delimiter)
		{
			string[] parts = input.ToString().Split(new string[] { delimiter.ToString() }, StringSplitOptions.None);
			Primitive array = new Primitive();
			for (int i = 0; i < parts.Length; i++) {
				array[i + 1] = parts[i];
			}
			return array;
		}

		/// <summary>
		/// Converts a string to camelCase format.
		/// </summary>
		/// <param name="input">The input text to convert.</param>
		/// <returns>The input text in camelCase format.</returns>
		public static Primitive ToCamelCase(Primitive input)
		{
			string text = input.ToString().Trim();

			if (string.IsNullOrEmpty(text)) {
				return "";
			}

			string[] words = text.Split(new char[] {
				' ',
				'\t',
				'\n',
				'\r',
				'_',
				'-'
			}, StringSplitOptions.RemoveEmptyEntries);

			if (words.Length == 0) {
				return "";
			}

			string result = words[0].ToLower();
			for (int i = 1; i < words.Length; i++) {
				result += words[i].Substring(0, 1).ToUpper() + words[i].Substring(1).ToLower();
			}

			return result;
		}



		/// <summary>
		/// Reverses the input text.
		/// </summary>
		/// <param name="input">The text to be reversed.</param>
		/// <returns>The reversed text.</returns>
		public static Primitive Reverse(Primitive input)
		{
			char[] charArray = input.ToString().ToCharArray();
			System.Array.Reverse(charArray); // Specify System.Array to avoid ambiguity
			return new string(charArray);
		}

		/// <summary>
		/// Converts the first letter of each word in the input text to uppercase.
		/// </summary>
		/// <param name="input">The text to be converted.</param>
		/// <returns>The converted text with each word's first letter in uppercase.</returns>
		public static Primitive ToTitleCase(Primitive input)
		{
			string[] words = input.ToString().Split(' ');
			for (int i = 0; i < words.Length; i++) {
				if (words[i].Length > 0) {
					words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
				}
			}
			return string.Join(" ", words);
		}

		/// <summary>
		/// Checks if the input text starts with the specified value.
		/// </summary>
		/// <param name="input">The text to check.</param>
		/// <param name="value">The value to check for.</param>
		/// <returns>True if the input text starts with the value, otherwise false.</returns>
		public static Primitive StartsWith(Primitive input, Primitive value)
		{
			return input.ToString().StartsWith(value.ToString());
		}

		/// <summary>
		/// Checks if the input text ends with the specified value.
		/// </summary>
		/// <param name="input">The text to check.</param>
		/// <param name="value">The value to check for.</param>
		/// <returns>True if the input text ends with the value, otherwise false.</returns>
		public static Primitive EndsWith(Primitive input, Primitive value)
		{
			return input.ToString().EndsWith(value.ToString());
		}

		/// <summary>
		/// Encodes the input text into HTML entities.
		/// </summary>
		/// <param name="input">The text to be encoded.</param>
		/// <returns>The HTML-encoded text.</returns>
		public static Primitive HtmlEncode(Primitive input)
		{
			return System.Web.HttpUtility.HtmlEncode(input.ToString());
		}

		/// <summary>
		/// Decodes the input text from HTML entities.
		/// </summary>
		/// <param name="input">The HTML-encoded text to be decoded.</param>
		/// <returns>The decoded text.</returns>
		public static Primitive HtmlDecode(Primitive input)
		{
			return System.Web.HttpUtility.HtmlDecode(input.ToString());
		}

		/// <summary>
		/// Encodes the input text into URL format.
		/// </summary>
		/// <param name="input">The text to be URL-encoded.</param>
		/// <returns>The URL-encoded text.</returns>
		public static Primitive UrlEncode(Primitive input)
		{
			return System.Web.HttpUtility.UrlEncode(input.ToString());
		}

		/// <summary>
		/// Decodes the input text from URL format.
		/// </summary>
		/// <param name="input">The URL-encoded text to be decoded.</param>
		/// <returns>The decoded text.</returns>
		public static Primitive UrlDecode(Primitive input)
		{
			return System.Web.HttpUtility.UrlDecode(input.ToString());
		}
		
		/// <summary>
		/// Counts the occurrences of a substring within the input text.
		/// </summary>
		/// <param name="text">The text to search within.</param>
		/// <param name="substring">The substring to count.</param>
		/// <returns>The number of occurrences of the substring in the text.</returns>
		public static Primitive CountOccurrences(Primitive text, Primitive substring)
		{
			string inputText = text.ToString();
			string searchSubstring = substring.ToString();

			int count = 0;
			int index = inputText.IndexOf(searchSubstring);
			while (index != -1) {
				count++;
				index = inputText.IndexOf(searchSubstring, index + 1);
			}

			return count;
		}
		
		/// <summary>
		/// Checks if the input text starts with the specified prefix.
		/// </summary>
		/// <param name="text">The text to check.</param>
		/// <param name="prefix">The prefix to check against.</param>
		/// <returns>True if the text starts with the prefix, otherwise false.</returns>
		public static Primitive StartsWithPrefix(Primitive text, Primitive prefix)
		{
			string inputText = text.ToString();
			string checkPrefix = prefix.ToString();

			return inputText.StartsWith(checkPrefix);
		}
		
		/// <summary>
		/// Checks if the input text ends with the specified suffix.
		/// </summary>
		/// <param name="text">The text to check.</param>
		/// <param name="suffix">The suffix to check against.</param>
		/// <returns>True if the text ends with the suffix, otherwise false.</returns>
		public static Primitive EndsWithSuffix(Primitive text, Primitive suffix)
		{
			string inputText = text.ToString();
			string checkSuffix = suffix.ToString();

			return inputText.EndsWith(checkSuffix);
		}
		
		
		/// <summary>
		/// Gets the newline character "\n".
		/// </summary>
		public static Primitive NewLine {
			get { return "\n"; }
		}

		/// <summary>
		/// Gets the comma character ",".
		/// </summary>
		public static Primitive Comma {
			get { return ","; }
		}

		/// <summary>
		/// Gets the tab character "\t".
		/// </summary>
		public static Primitive Tab {
			get { return "\t"; }
		}

		/// <summary>
		/// Gets the colon character ":".
		/// </summary>
		public static Primitive Colon {
			get { return ":"; }
		}

		/// <summary>
		/// Gets the semicolon character ";".
		/// </summary>
		public static Primitive Semicolon {
			get { return ";"; }
		}

		/// <summary>
		/// Gets the dash character "-".
		/// </summary>
		public static Primitive Dash {
			get { return "-"; }
		}

		/// <summary>
		/// Gets the underscore character "_".
		/// </summary>
		public static Primitive Underscore {
			get { return "_"; }
		}

		/// <summary>
		/// Gets the period character ".".
		/// </summary>
		public static Primitive Period {
			get { return "."; }
		}

		/// <summary>
		/// Gets the question mark character "?".
		/// </summary>
		public static Primitive QuestionMark {
			get { return "?"; }
		}

		/// <summary>
		/// Gets the exclamation mark character "!".
		/// </summary>
		public static Primitive ExclamationMark {
			get { return "!"; }
		}

		/// <summary>
		/// Gets the quotation mark character '"'.
		/// </summary>
		public static Primitive QuotationMark {
			get { return "\""; }
		}

		/// <summary>
		/// Gets the single quote character '\''.
		/// </summary>
		public static Primitive SingleQuote {
			get { return "'"; }
		}

		/// <summary>
		/// Gets the parentheses opening character '('.
		/// </summary>
		public static Primitive ParenthesesOpen {
			get { return "("; }
		}

		/// <summary>
		/// Gets the parentheses closing character ')'.
		/// </summary>
		public static Primitive ParenthesesClose {
			get { return ")"; }
		}

		/// <summary>
		/// Gets the square bracket opening character '['.
		/// </summary>
		public static Primitive BracketOpen {
			get { return "["; }
		}

		/// <summary>
		/// Gets the square bracket closing character ']'.
		/// </summary>
		public static Primitive BracketClose {
			get { return "]"; }
		}

		/// <summary>
		/// Gets the curly brace opening character '{'.
		/// </summary>
		public static Primitive BraceOpen {
			get { return "{"; }
		}

		/// <summary>
		/// Gets the curly brace closing character '}'.
		/// </summary>
		public static Primitive BraceClose {
			get { return "}"; }
		}

		/// <summary>
		/// Gets the angle bracket opening character "<".
		/// </summary>
		public static Primitive AngleBracketOpen {
			get { return "<"; }
		}

		/// <summary>
		/// Gets the angle bracket closing character '>'.
		/// </summary>
		public static Primitive AngleBracketClose {
			get { return ">"; }
		}

		
	}
	
	/// <summary>
	/// Provides various dialog utilities such as message boxes, input dialogs, file dialogs, and color dialogs.
	/// </summary>
	[SmallBasicType]
	public static class ZSDialog
	{
		/// <summary>
		/// Shows a message box with the specified text.
		/// </summary>
		/// <param name="text">The text to display in the message box.</param>
		public static void ShowMessageBox(Primitive text)
		{
			MessageBox.Show(text);
		}

		/// <summary>
		/// Shows a custom input dialog with the specified prompt text.
		/// </summary>
		/// <param name="prompt">The prompt text to display in the input dialog.</param>
		/// <returns>The text entered by the user.</returns>
		public static Primitive ShowInputDialog(Primitive prompt)
		{
			string input = ShowInputDialogInternal(prompt);
			return new Primitive(input);
		}

		/// <summary>
		/// Shows a file open dialog and returns the selected file path.
		/// </summary>
		/// <returns>The path of the selected file.</returns>
		public static Primitive ShowOpenFileDialog()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			Primitive result = "";
			if (openFileDialog.ShowDialog() == DialogResult.OK) {
				result = openFileDialog.FileName;
			}
			return result;
		}

		/// <summary>
		/// Shows a file save dialog and returns the selected file path.
		/// </summary>
		/// <returns>The path of the file to save.</returns>
		public static Primitive ShowSaveFileDialog()
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			Primitive result = "";
			if (saveFileDialog.ShowDialog() == DialogResult.OK) {
				result = saveFileDialog.FileName;
			}
			return result;
		}

		/// <summary>
		/// Shows a color dialog and returns the selected color as a string.
		/// </summary>
		/// <returns>The selected color in hexadecimal format (e.g., #FF0000 for red).</returns>
		public static Primitive ShowColorDialog()
		{
			ColorDialog colorDialog = new ColorDialog();
			if (colorDialog.ShowDialog() == DialogResult.OK) {
				return new Primitive(ColorTranslator.ToHtml(colorDialog.Color));
			}
			return new Primitive(string.Empty);
		}

		private static string ShowInputDialogInternal(string prompt)
		{
			Form form = new Form();
			Label label = new Label();
			TextBox textBox = new TextBox();
			Button buttonOk = new Button();
			Button buttonCancel = new Button();

			form.Text = "Input Dialog";
			label.Text = prompt;
			textBox.Text = string.Empty;

			buttonOk.Text = "OK";
			buttonCancel.Text = "Cancel";
			buttonOk.DialogResult = DialogResult.OK;
			buttonCancel.DialogResult = DialogResult.Cancel;

			label.SetBounds(9, 20, 372, 13);
			textBox.SetBounds(12, 36, 372, 20);
			buttonOk.SetBounds(228, 72, 75, 23);
			buttonCancel.SetBounds(309, 72, 75, 23);

			label.AutoSize = true;
			textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
			buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

			form.ClientSize = new System.Drawing.Size(396, 107);
			form.Controls.AddRange(new Control[] {
				label,
				textBox,
				buttonOk,
				buttonCancel
			});
			form.ClientSize = new System.Drawing.Size(System.Math.Max(300, label.Right + 10), form.ClientSize.Height);
			form.FormBorderStyle = FormBorderStyle.FixedDialog;
			form.StartPosition = FormStartPosition.CenterScreen;
			form.MinimizeBox = false;
			form.MaximizeBox = false;
			form.AcceptButton = buttonOk;
			form.CancelButton = buttonCancel;

			DialogResult dialogResult = form.ShowDialog();
			return textBox.Text;
		}
    
    
		/// <summary>
		/// Shows a Yes/No dialog with the specified question text.
		/// </summary>
		/// <param name="question">The question text to display in the dialog.</param>
		/// <returns>Returns "Yes" if Yes is clicked, otherwise returns "No".</returns>
		public static Primitive ShowYesNoDialog(Primitive question)
		{
			DialogResult result = MessageBox.Show(question, "Yes/No Dialog", MessageBoxButtons.YesNo);
			return new Primitive(result == DialogResult.Yes ? "Yes" : "No");
		}

		/// <summary>
		/// Shows a folder browser dialog and returns the selected folder path.
		/// </summary>
		/// <returns>The path of the selected folder.</returns>
		public static Primitive ShowFolderBrowserDialog()
		{
			FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
			Primitive result = "";
			if (folderBrowserDialog.ShowDialog() == DialogResult.OK) {
				result = folderBrowserDialog.SelectedPath;
			}
			return result;
		}

		/// <summary>
		/// Shows an error dialog with the specified error message.
		/// </summary>
		/// <param name="errorMessage">The error message to display in the dialog.</param>
		public static void ShowErrorDialog(Primitive errorMessage)
		{
			MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		/// <summary>
		/// Shows a warning dialog with the specified warning message.
		/// </summary>
		/// <param name="warningMessage">The warning message to display in the dialog.</param>
		public static void ShowWarningDialog(Primitive warningMessage)
		{
			MessageBox.Show(warningMessage, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}

		/// <summary>
		/// Shows an information dialog with the specified information message.
		/// </summary>
		/// <param name="informationMessage">The information message to display in the dialog.</param>
		public static void ShowInformationDialog(Primitive informationMessage)
		{
			MessageBox.Show(informationMessage, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		/// <summary>
		/// Shows a custom message box with specified text, title, and buttons.
		/// </summary>
		/// <param name="text">The text to display in the message box.</param>
		/// <param name="title">The title of the message box.</param>
		/// <param name="buttons">The buttons to display in the message box (OK, OKCancel, YesNo).</param>
		/// <returns>The text of the button that was clicked.</returns>
		public static Primitive ShowCustomMessageBox(Primitive text, Primitive title, Primitive buttons)
		{
			MessageBoxButtons messageBoxButtons = MessageBoxButtons.OK;
			if (buttons == "OKCancel")
				messageBoxButtons = MessageBoxButtons.OKCancel;
			else if (buttons == "YesNo")
				messageBoxButtons = MessageBoxButtons.YesNo;

			DialogResult result = MessageBox.Show(text, title, messageBoxButtons);
			return new Primitive(result.ToString());
		}

		/// <summary>
		/// Shows a progress dialog with a specified message and duration.
		/// </summary>
		/// <param name="message">The message to display in the progress dialog.</param>
		/// <param name="durationInSeconds">The duration in seconds for which the progress dialog should be displayed.</param>
		public static void ShowProgressDialog(Primitive message, Primitive durationInSeconds)
		{
			Form progressDialog = new Form();
			Label label = new Label();
			ProgressBar progressBar = new ProgressBar();
			System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

			progressDialog.Text = "Progress";
			label.Text = message;
			progressBar.Style = ProgressBarStyle.Marquee;

			label.SetBounds(9, 20, 372, 13);
			progressBar.SetBounds(12, 36, 372, 20);

			label.AutoSize = true;
			progressBar.Anchor = progressBar.Anchor | AnchorStyles.Right;

			progressDialog.ClientSize = new System.Drawing.Size(396, 75);
			progressDialog.Controls.AddRange(new Control[] { label, progressBar });
			progressDialog.ClientSize = new System.Drawing.Size(System.Math.Max(300, label.Right + 10), progressDialog.ClientSize.Height);
			progressDialog.FormBorderStyle = FormBorderStyle.FixedDialog;
			progressDialog.StartPosition = FormStartPosition.CenterScreen;
			progressDialog.MinimizeBox = false;
			progressDialog.MaximizeBox = false;

			timer.Interval = (int)durationInSeconds * 1000;
			timer.Tick += (sender, e) => {
				timer.Stop();
				progressDialog.Close();
			};

			timer.Start();
			progressDialog.ShowDialog();
		}

		/// <summary>
		/// Shows a font dialog and returns the selected font as a string.
		/// </summary>
		/// <returns>The selected font in the format "FontName, Size, Style".</returns>
		public static Primitive ShowFontDialog()
		{
			FontDialog fontDialog = new FontDialog();
			if (fontDialog.ShowDialog() == DialogResult.OK) {
				Font selectedFont = fontDialog.Font;
				return new Primitive(string.Format("{0}, {1}, {2}", selectedFont.Name, selectedFont.Size, selectedFont.Style));
			}
			return new Primitive(string.Empty);
		}


		/// <summary>
		/// Shows a time picker dialog and returns the selected time.
		/// </summary>
		/// <returns>The selected time in the format "HH:mm:ss".</returns>
		public static Primitive ShowTimePickerDialog()
		{
			DateTimePicker timePicker = new DateTimePicker();
			timePicker.Format = DateTimePickerFormat.Time;
			timePicker.ShowUpDown = true;

			Form form = new Form();
			Button buttonOk = new Button();
			Button buttonCancel = new Button();

			form.Text = "Time Picker";
			timePicker.Value = DateTime.Now;

			buttonOk.Text = "OK";
			buttonCancel.Text = "Cancel";
			buttonOk.DialogResult = DialogResult.OK;
			buttonCancel.DialogResult = DialogResult.Cancel;

			timePicker.SetBounds(12, 12, 200, 20);
			buttonOk.SetBounds(228, 12, 75, 23);
			buttonCancel.SetBounds(309, 12, 75, 23);

			buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

			form.ClientSize = new System.Drawing.Size(396, 45);
			form.Controls.AddRange(new Control[] { timePicker, buttonOk, buttonCancel });
			form.FormBorderStyle = FormBorderStyle.FixedDialog;
			form.StartPosition = FormStartPosition.CenterScreen;
			form.MinimizeBox = false;
			form.MaximizeBox = false;
			form.AcceptButton = buttonOk;
			form.CancelButton = buttonCancel;

			DialogResult dialogResult = form.ShowDialog();
			if (dialogResult == DialogResult.OK) {
				return new Primitive(timePicker.Value.ToString("HH:mm:ss"));
			}
			return new Primitive(string.Empty);
		}

		/// <summary>
		/// Shows a date picker dialog and returns the selected date.
		/// </summary>
		/// <returns>The selected date in the format "yyyy-MM-dd".</returns>
		public static Primitive ShowDatePickerDialog()
		{
			DateTimePicker datePicker = new DateTimePicker();
			datePicker.Format = DateTimePickerFormat.Short;

			Form form = new Form();
			Button buttonOk = new Button();
			Button buttonCancel = new Button();

			form.Text = "Date Picker";
			datePicker.Value = DateTime.Now;

			buttonOk.Text = "OK";
			buttonCancel.Text = "Cancel";
			buttonOk.DialogResult = DialogResult.OK;
			buttonCancel.DialogResult = DialogResult.Cancel;

			datePicker.SetBounds(12, 12, 200, 20);
			buttonOk.SetBounds(228, 12, 75, 23);
			buttonCancel.SetBounds(309, 12, 75, 23);

			buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

			form.ClientSize = new System.Drawing.Size(396, 45);
			form.Controls.AddRange(new Control[] { datePicker, buttonOk, buttonCancel });
			form.FormBorderStyle = FormBorderStyle.FixedDialog;
			form.StartPosition = FormStartPosition.CenterScreen;
			form.MinimizeBox = false;
			form.MaximizeBox = false;
			form.AcceptButton = buttonOk;
			form.CancelButton = buttonCancel;

			DialogResult dialogResult = form.ShowDialog();
			if (dialogResult == DialogResult.OK) {
				return new Primitive(datePicker.Value.ToString("yyyy-MM-dd"));
			}
			return new Primitive(string.Empty);
		}

		/// <summary>
		/// Shows a confirmation dialog with specified text and title.
		/// </summary>
		/// <param name="text">The text to display in the confirmation dialog.</param>
		/// <param name="title">The title of the confirmation dialog.</param>
		/// <returns>Returns true if Yes is clicked, otherwise returns false.</returns>
		public static Primitive ShowConfirmationDialog(Primitive text, Primitive title)
		{
			DialogResult result = MessageBox.Show(text, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			return new Primitive(result == DialogResult.Yes);
		}


		/// <summary>
		/// Shows a color dialog and returns the selected color as a string in the format "R,G,B".
		/// </summary>
		/// <returns>The selected color or an empty string if canceled.</returns>
		public static Primitive ShowCustomColorDialog()
		{
			ColorDialog colorDialog = new ColorDialog();
			if (colorDialog.ShowDialog() == DialogResult.OK) {
				Color color = colorDialog.Color;
				return new Primitive(string.Format("{0},{1},{2}", color.R, color.G, color.B));
			}
			return new Primitive(string.Empty);
		}

		/// <summary>
		/// Shows an open file dialog with multi-select enabled and returns the selected file paths.
		/// </summary>
		/// <param name="filter">The file types filter in the format "Display Name1|Pattern1|Display Name2|Pattern2|...".</param>
		/// <returns>A semicolon-separated list of selected file paths or an empty string if canceled.</returns>
		/// <remarks>
		/// Filters should be specified in pairs where:
		/// - Display Name: The name shown in the dialog's filter dropdown.
		/// - Pattern: The file pattern to filter files by extension (e.g., "*.txt", "*.jpg").
		/// Multiple filters can be separated by vertical bars ('|'). For example:
		/// "Text Files|*.txt|All Files|*.*"
		/// </remarks>
		public static Primitive ShowMultiSelectOpenFileDialog(Primitive filter)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog {
				Filter = filter,
				Multiselect = true
			};

			if (openFileDialog.ShowDialog() == DialogResult.OK) {
				return new Primitive(string.Join(";", openFileDialog.FileNames));
			}

			return new Primitive(string.Empty);
		}

		/// <summary>
		/// Shows an input box with a multi-line text box for larger input.
		/// </summary>
		/// <param name="prompt">The prompt to display in the input box.</param>
		/// <param name="title">The title of the input box.</param>
		/// <returns>The user input or an empty string if canceled.</returns>
		public static Primitive ShowMultiLineInputBox(Primitive prompt, Primitive title)
		{
			Form form = new Form();
			Label label = new Label();
			TextBox textBox = new TextBox();
			Button buttonOk = new Button();
			Button buttonCancel = new Button();

			form.Text = title;
			label.Text = prompt;
			textBox.Multiline = true;

			buttonOk.Text = "OK";
			buttonCancel.Text = "Cancel";
			buttonOk.DialogResult = DialogResult.OK;
			buttonCancel.DialogResult = DialogResult.Cancel;

			label.SetBounds(9, 20, 372, 13);
			textBox.SetBounds(12, 36, 372, 200);
			buttonOk.SetBounds(228, 250, 75, 23);
			buttonCancel.SetBounds(309, 250, 75, 23);

			label.AutoSize = true;
			textBox.Anchor = textBox.Anchor | AnchorStyles.Bottom | AnchorStyles.Right;
			buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

			form.ClientSize = new System.Drawing.Size(396, 285);
			form.Controls.AddRange(new Control[] {
				label,
				textBox,
				buttonOk,
				buttonCancel
			});
			form.ClientSize = new System.Drawing.Size(System.Math.Max(300, label.Right + 10), form.ClientSize.Height);
			form.FormBorderStyle = FormBorderStyle.FixedDialog;
			form.StartPosition = FormStartPosition.CenterScreen;
			form.MinimizeBox = false;
			form.MaximizeBox = false;
			form.AcceptButton = buttonOk;
			form.CancelButton = buttonCancel;

			DialogResult dialogResult = form.ShowDialog();
			if (dialogResult == DialogResult.OK) {
				return new Primitive(textBox.Text);
			}
			return new Primitive(string.Empty);
		}

		/// <summary>
		/// Shows a password input dialog and returns the entered password.
		/// </summary>
		/// <param name="prompt">The prompt to display in the input dialog.</param>
		/// <param name="title">The title of the input dialog.</param>
		/// <returns>The entered password or an empty string if canceled.</returns>
		public static Primitive ShowPasswordInputDialog(Primitive prompt, Primitive title)
		{
			Form form = new Form();
			Label label = new Label();
			TextBox textBox = new TextBox();
			Button buttonOk = new Button();
			Button buttonCancel = new Button();

			form.Text = title;
			label.Text = prompt;
			textBox.PasswordChar = '*';

			buttonOk.Text = "OK";
			buttonCancel.Text = "Cancel";
			buttonOk.DialogResult = DialogResult.OK;
			buttonCancel.DialogResult = DialogResult.Cancel;

			label.SetBounds(9, 20, 372, 13);
			textBox.SetBounds(12, 36, 372, 20);
			buttonOk.SetBounds(228, 72, 75, 23);
			buttonCancel.SetBounds(309, 72, 75, 23);

			label.AutoSize = true;
			textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
			buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

			form.ClientSize = new System.Drawing.Size(396, 107);
			form.Controls.AddRange(new Control[] {
				label,
				textBox,
				buttonOk,
				buttonCancel
			});
			form.ClientSize = new System.Drawing.Size(System.Math.Max(300, label.Right + 10), form.ClientSize.Height);
			form.FormBorderStyle = FormBorderStyle.FixedDialog;
			form.StartPosition = FormStartPosition.CenterScreen;
			form.MinimizeBox = false;
			form.MaximizeBox = false;
			form.AcceptButton = buttonOk;
			form.CancelButton = buttonCancel;

			DialogResult dialogResult = form.ShowDialog();
			if (dialogResult == DialogResult.OK) {
				return new Primitive(textBox.Text);
			}
			return new Primitive(string.Empty);
		}

		/// <summary>
		/// Shows an input dialog with options and returns the selected option.
		/// </summary>
		/// <param name="prompt">The prompt to display in the input dialog.</param>
		/// <param name="title">The title of the input dialog.</param>
		/// <param name="options">The list of options to choose from.</param>
		/// <returns>The selected option or an empty string if canceled.</returns>
		public static Primitive ShowInputDialogWithOptions(Primitive prompt, Primitive title, Primitive[] options)
		{
			Form form = new Form();
			Label label = new Label();
			ComboBox comboBox = new ComboBox();
			Button buttonOk = new Button();
			Button buttonCancel = new Button();

			form.Text = title;
			label.Text = prompt;

			// Convert Primitive[] to object[]
			object[] optionsObjects = new object[options.Length];
			for (int i = 0; i < options.Length; i++) {
				optionsObjects[i] = options[i].ToString();  // Adjust this line to convert Primitive to appropriate object type
			}

			comboBox.Items.AddRange(optionsObjects);
			comboBox.DropDownStyle = ComboBoxStyle.DropDownList;

			buttonOk.Text = "OK";
			buttonCancel.Text = "Cancel";
			buttonOk.DialogResult = DialogResult.OK;
			buttonCancel.DialogResult = DialogResult.Cancel;

			label.SetBounds(9, 20, 372, 13);
			comboBox.SetBounds(12, 36, 372, 21);
			buttonOk.SetBounds(228, 72, 75, 23);
			buttonCancel.SetBounds(309, 72, 75, 23);

			label.AutoSize = true;
			comboBox.Anchor = comboBox.Anchor | AnchorStyles.Right;
			buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

			form.ClientSize = new System.Drawing.Size(396, 107);
			form.Controls.AddRange(new Control[] {
				label,
				comboBox,
				buttonOk,
				buttonCancel
			});
			form.ClientSize = new System.Drawing.Size(System.Math.Max(300, label.Right + 10), form.ClientSize.Height);
			form.FormBorderStyle = FormBorderStyle.FixedDialog;
			form.StartPosition = FormStartPosition.CenterScreen;
			form.MinimizeBox = false;
			form.MaximizeBox = false;
			form.AcceptButton = buttonOk;
			form.CancelButton = buttonCancel;

			DialogResult dialogResult = form.ShowDialog();
			if (dialogResult == DialogResult.OK) {
				return new Primitive(comboBox.SelectedItem != null ? comboBox.SelectedItem.ToString() : string.Empty);
			}
			return new Primitive(string.Empty);
		}


		/// <summary>
		/// Shows a file dialog to select an image file and returns the selected file path.
		/// </summary>
		/// <returns>The path of the selected image file or an empty string if canceled.</returns>
		public static Primitive ShowImageFileDialog()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Image Files (*.bmp;*.jpg;*.jpeg;*.png)|*.BMP;*.JPG;*.JPEG;*.PNG";

			if (openFileDialog.ShowDialog() == DialogResult.OK) {
				return new Primitive(openFileDialog.FileName);
			}
			return new Primitive(string.Empty);
		}
	}
	
	/// <summary>
	/// Provides Image Functions For Small Basic.
	/// </summary>
	[SmallBasicType]
	public static class ZSImage
	{
		/// <summary>
		/// Determines if a sub-image is present within a main image.
		/// </summary>
		/// <param name="mainImagePath">The file path of the main image.</param>
		/// <param name="subImagePath">The file path of the sub-image.</param>
		/// <returns>True if the sub-image is found within the main image, otherwise false.</returns>
		public static Primitive IsImageInImage(Primitive mainImagePath, Primitive subImagePath)
		{
			string mainImagePathStr = mainImagePath.ToString();
			string subImagePathStr = subImagePath.ToString();

			if (!System.IO.File.Exists(mainImagePathStr) || !System.IO.File.Exists(subImagePathStr)) {
				return false;
			}

			using (Bitmap mainImage = new Bitmap(mainImagePathStr))
			using (Bitmap subImage = new Bitmap(subImagePathStr)) {
				for (int y = 0; y <= mainImage.Height - subImage.Height; y++) {
					for (int x = 0; x <= mainImage.Width - subImage.Width; x++) {
						if (IsMatch(mainImage, subImage, x, y)) {
							return true;
						}
					}
				}
				return false;
			}
		}

		private static bool IsMatch(Bitmap mainImage, Bitmap subImage, int startX, int startY)
		{
			for (int y = 0; y < subImage.Height; y++) {
				for (int x = 0; x < subImage.Width; x++) {
					if (mainImage.GetPixel(startX + x, startY + y) != subImage.GetPixel(x, y)) {
						return false;  // Early termination on the first mismatch
					}
				}
			}
			return true;
		}
	
		/// <summary>
		/// Captures the entire screen and saves it as a PNG file.
		/// </summary>
		/// <param name="filePath">The file path where the screenshot will be saved.</param>
		/// <returns>True if the screen capture was successful; otherwise, false.</returns>
		public static Primitive CaptureScreen(Primitive filePath)
		{
			try {
				Rectangle bounds = Screen.PrimaryScreen.Bounds;
				using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height)) {
					using (Graphics g = Graphics.FromImage(bitmap)) {
						g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
					}
					bitmap.Save(filePath, ImageFormat.Png);
					return true; // Return true indicating successful capture
				}
			} catch (Exception ex) {
				MessageBox.Show("Error capturing screen: " + ex.Message);
				return false; // Return false indicating failure
			}
		}


		/// <summary>
		/// Resizes an image to the specified width and height.
		/// </summary>
		/// <param name="inputFilePath">The file path of the input image.</param>
		/// <param name="outputFilePath">The file path where the resized image will be saved.</param>
		/// <param name="width">The width of the resized image.</param>
		/// <param name="height">The height of the resized image.</param>
		/// <returns>True if the image resizing was successful; otherwise, false.</returns>
		public static Primitive ResizeImage(Primitive inputFilePath, Primitive outputFilePath, Primitive width, Primitive height)
		{
			try {
				// Convert Primitive types to appropriate types
				string inputPath = inputFilePath.ToString();
				string outputPath = outputFilePath.ToString();
				int resizeWidth = (int)width;
				int resizeHeight = (int)height;

				using (Bitmap original = new Bitmap(inputPath)) {
					using (Bitmap resized = new Bitmap(resizeWidth, resizeHeight)) {
						using (Graphics g = Graphics.FromImage(resized)) {
							g.DrawImage(original, 0, 0, resizeWidth, resizeHeight);
						}
						resized.Save(outputPath, ImageFormat.Png);
						return true; // Return true indicating successful resize
					}
				}
			} catch (Exception ex) {
				MessageBox.Show("Error resizing image: " + ex.Message);
				return false; // Return false indicating failure
			}
		}

		/// <summary>
		/// Gets the width of an image.
		/// </summary>
		/// <param name="filePath">The file path of the image.</param>
		/// <returns>The width of the image.</returns>
		public static Primitive GetImageWidth(Primitive filePath)
		{
			try {
				using (Bitmap image = new Bitmap(filePath)) {
					return image.Width;
				}
			} catch (Exception ex) {
				MessageBox.Show("Error getting image width: " + ex.Message);
				return -1; // Return -1 to indicate an error
			}
		}

		/// <summary>
		/// Gets the height of an image.
		/// </summary>
		/// <param name="filePath">The file path of the image.</param>
		/// <returns>The height of the image.</returns>
		public static Primitive GetImageHeight(Primitive filePath)
		{
			try {
				using (Bitmap image = new Bitmap(filePath)) {
					return image.Height;
				}
			} catch (Exception ex) {
				MessageBox.Show("Error getting image height: " + ex.Message);
				return -1; // Return -1 to indicate an error
			}
		}
	}
	
	
	/// <summary>
	/// Provides File Functions For Small Basic.
	/// </summary>
	[SmallBasicType]
	public static class ZSFile
	{
		/// <summary>
		/// Reads the content of a file.
		/// </summary>
		/// <param name="filePath">The path of the file to read.</param>
		/// <returns>The content of the file.</returns>
		public static Primitive ReadFile(Primitive filePath)
		{
			try {
				return System.IO.File.ReadAllText(filePath);
			} catch (Exception ex) {
				return "Error: " + ex.Message;
			}
		}

		/// <summary>
		/// Writes content to a file.
		/// </summary>
		/// <param name="filePath">The path of the file to write to.</param>
		/// <param name="content">The content to write.</param>
		/// <returns>"Success" if the operation was successful; otherwise, an error message.</returns>
		public static Primitive WriteFile(Primitive filePath, Primitive content)
		{
			try {
				System.IO.File.WriteAllText(filePath, content);
				return "Success";
			} catch (Exception ex) {
				return "Error: " + ex.Message;
			}
		}

		/// <summary>
		/// Copies a file to a new location.
		/// </summary>
		/// <param name="sourcePath">The path of the file to copy.</param>
		/// <param name="destinationPath">The path where the file will be copied to.</param>
		/// <returns>"Success" if the operation was successful; otherwise, an error message.</returns>
		public static Primitive CopyFile(Primitive sourcePath, Primitive destinationPath)
		{
			try {
				System.IO.File.Copy(sourcePath, destinationPath, true);
				return "Success";
			} catch (Exception ex) {
				return "Error: " + ex.Message;
			}
		}

		/// <summary>
		/// Deletes a file.
		/// </summary>
		/// <param name="filePath">The path of the file to delete.</param>
		/// <returns>"Success" if the operation was successful; otherwise, an error message.</returns>
		public static Primitive DeleteFile(Primitive filePath)
		{
			try {
				System.IO.File.Delete(filePath);
				return "Success";
			} catch (Exception ex) {
				return "Error: " + ex.Message;
			}
		}

		/// <summary>
		/// Checks if a file exists.
		/// </summary>
		/// <param name="filePath">The path of the file to check.</param>
		/// <returns>True if the file exists; otherwise, false.</returns>
		public static Primitive FileExists(Primitive filePath)
		{
			try {
				return System.IO.File.Exists(filePath);
			} catch (Exception ex) {
				return "Error: " + ex.Message;
			}
		}

		/// <summary>
		/// Moves a file to a new location.
		/// </summary>
		/// <param name="sourcePath">The path of the file to move.</param>
		/// <param name="destinationPath">The path where the file will be moved to.</param>
		/// <returns>"Success" if the operation was successful; otherwise, an error message.</returns>
		public static Primitive MoveFile(Primitive sourcePath, Primitive destinationPath)
		{
			try {
				System.IO.File.Move(sourcePath, destinationPath);
				return "Success";
			} catch (Exception ex) {
				return "Error: " + ex.Message;
			}
		}

		/// <summary>
		/// Creates a directory.
		/// </summary>
		/// <param name="directoryPath">The path of the directory to create.</param>
		/// <returns>"Success" if the operation was successful; otherwise, an error message.</returns>
		public static Primitive CreateDirectory(Primitive directoryPath)
		{
			try {
				Directory.CreateDirectory(directoryPath);
				return "Success";
			} catch (Exception ex) {
				return "Error: " + ex.Message;
			}
		}

		/// <summary>
		/// Lists all files in a directory.
		/// </summary>
		/// <param name="directoryPath">The path of the directory to list files from.</param>
		/// <returns>A comma-separated string of file paths.</returns>
		public static Primitive ListFiles(Primitive directoryPath)
		{
			try {
				string[] files = Directory.GetFiles(directoryPath);
				return string.Join(",", files);
			} catch (Exception ex) {
				return "Error: " + ex.Message;
			}
		}
	
		/// <summary>
		/// Renames a file.
		/// </summary>
		/// <param name="currentFilePath">The current path of the file to rename.</param>
		/// <param name="newFilePath">The new path and name for the file.</param>
		/// <returns>"Success" if the operation was successful; otherwise, an error message.</returns>
		public static Primitive RenameFile(Primitive currentFilePath, Primitive newFilePath)
		{
			try {
				System.IO.File.Move(currentFilePath, newFilePath);
				return "Success";
			} catch (Exception ex) {
				return "Error: " + ex.Message;
			}
		}
		
		/// <summary>
		/// Checks if a directory exists.
		/// </summary>
		/// <param name="directoryPath">The path of the directory to check.</param>
		/// <returns>True if the directory exists; otherwise, false.</returns>
		public static Primitive DirectoryExists(Primitive directoryPath)
		{
			try {
				return Directory.Exists(directoryPath);
			} catch (Exception ex) {
				return "Error: " + ex.Message;
			}
		}

		/// <summary>
		/// Deletes a directory.
		/// </summary>
		/// <param name="directoryPath">The path of the directory to delete.</param>
		/// <returns>"Success" if the operation was successful; otherwise, an error message.</returns>
		public static Primitive DeleteDirectory(Primitive directoryPath)
		{
			try {
				Directory.Delete(directoryPath, true); // Set true to delete recursively
				return "Success";
			} catch (Exception ex) {
				return "Error: " + ex.Message;
			}
		}

		/// <summary>
		/// Moves a directory to a new location.
		/// </summary>
		/// <param name="sourcePath">The current path of the directory to move.</param>
		/// <param name="destinationPath">The new path for the directory.</param>
		/// <returns>"Success" if the operation was successful; otherwise, an error message.</returns>
		public static Primitive MoveDirectory(Primitive sourcePath, Primitive destinationPath)
		{
			try {
				Directory.Move(sourcePath, destinationPath);
				return "Success";
			} catch (Exception ex) {
				return "Error: " + ex.Message;
			}
		}

		/// <summary>
		/// Retrieves the last write time of a file.
		/// </summary>
		/// <param name="filePath">The path of the file to retrieve the last write time from.</param>
		/// <returns>A string representation of the last write time; or an error message if an exception occurs.</returns>
		public static Primitive GetLastWriteTime(Primitive filePath)
		{
			try {
				DateTime lastWriteTime = System.IO.File.GetLastWriteTime(filePath);
				return lastWriteTime.ToString();
			} catch (Exception ex) {
				return "Error: " + ex.Message;
			}
		}

		/// <summary>
		/// Retrieves the size of a file.
		/// </summary>
		/// <param name="filePath">The path of the file to get the size of.</param>
		/// <returns>The size of the file in bytes, or an error message if an exception occurs.</returns>
		public static Primitive GetFileSize(Primitive filePath)
		{
			try {
				long size = new FileInfo(filePath).Length;
				return (Primitive)(decimal)size; // Explicitly cast long to decimal
			} catch (Exception ex) {
				return "Error: " + ex.Message;
			}
		}

		/// <summary>
		/// Retrieves the extension of a file.
		/// </summary>
		/// <param name="filePath">The path of the file to get the extension of.</param>
		/// <returns>The extension of the file, or an error message if an exception occurs.</returns>
		public static Primitive GetFileExtension(Primitive filePath)
		{
			try {
				string extension = Path.GetExtension(filePath);
				return extension;
			} catch (Exception ex) {
				return "Error: " + ex.Message;
			}
		}

		/// <summary>
		/// Creates a text file with specified content.
		/// </summary>
		/// <param name="filePath">The path of the file to create.</param>
		/// <param name="content">The content to write to the file.</param>
		/// <returns>"Success" if the operation was successful; otherwise, an error message.</returns>
		public static Primitive CreateTextFile(Primitive filePath, Primitive content)
		{
			try {
				System.IO.File.WriteAllText(filePath, content);
				return "Success";
			} catch (Exception ex) {
				return "Error: " + ex.Message;
			}
		}

		/// <summary>
		/// Appends content to an existing file.
		/// </summary>
		/// <param name="filePath">The path of the file to append to.</param>
		/// <param name="content">The content to append.</param>
		/// <returns>"Success" if the operation was successful; otherwise, an error message.</returns>
		public static Primitive AppendToFile(Primitive filePath, Primitive content)
		{
			try {
				System.IO.File.AppendAllText(filePath, content);
				return "Success";
			} catch (Exception ex) {
				return "Error: " + ex.Message;
			}
		}

		/// <summary>
		/// Retrieves the creation time of a file.
		/// </summary>
		/// <param name="filePath">The path of the file to retrieve the creation time from.</param>
		/// <returns>A string representation of the creation time, or an error message if an exception occurs.</returns>
		public static Primitive GetFileCreationTime(Primitive filePath)
		{
			try {
				DateTime creationTime = System.IO.File.GetCreationTime(filePath);
				return creationTime.ToString();
			} catch (Exception ex) {
				return "Error: " + ex.Message;
			}
		}

		/// <summary>
		/// Reads all lines from a file.
		/// </summary>
		/// <param name="filePath">The path of the file to read from.</param>
		/// <returns>A list of all lines in the file, or an error message if an exception occurs.</returns>
		public static Primitive ReadAllLines(Primitive filePath)
		{
			try {
				string[] lines = System.IO.File.ReadAllLines(filePath);
				Primitive array = new Primitive();
				for (int i = 0; i < lines.Length; i++) {
					array[i] = lines[i];
				}
				return array;
			} catch (Exception ex) {
				return "Error: " + ex.Message;
			}
		}

		/// <summary>
		/// Retrieves the attributes of a file.
		/// </summary>
		/// <param name="filePath">The path of the file to get the attributes of.</param>
		/// <returns>The attributes of the file, or an error message if an exception occurs.</returns>
		public static Primitive GetFileAttributes(Primitive filePath)
		{
			try {
				FileAttributes attributes = System.IO.File.GetAttributes(filePath);
				return attributes.ToString();
			} catch (Exception ex) {
				return "Error: " + ex.Message;
			}
		}

		/// <summary>
		/// Retrieves the last access time of a file.
		/// </summary>
		/// <param name="filePath">The path of the file to retrieve the last access time from.</param>
		/// <returns>A string representation of the last access time, or an error message if an exception occurs.</returns>
		public static Primitive GetFileLastAccessTime(Primitive filePath)
		{
			try {
				DateTime lastAccessTime = System.IO.File.GetLastAccessTime(filePath);
				return lastAccessTime.ToString();
			} catch (Exception ex) {
				return "Error: " + ex.Message;
			}
		}

		/// <summary>
		/// Deletes a directory and optionally all its contents.
		/// </summary>
		/// <param name="directoryPath">The path of the directory to delete.</param>
		/// <param name="recursive">True to delete the directory, its subdirectories, and all files; otherwise, false.</param>
		/// <returns>"Success" if the operation was successful; otherwise, an error message.</returns>
		public static Primitive DeleteDirectory(Primitive directoryPath, Primitive recursive)
		{
			try {
				Directory.Delete(directoryPath, recursive);
				return "Success";
			} catch (Exception ex) {
				return "Error: " + ex.Message;
			}
		}

		/// <summary>
		/// Retrieves the file name and extension of a file path.
		/// </summary>
		/// <param name="filePath">The path of the file to retrieve the name from.</param>
		/// <returns>The file name and extension, or an error message if an exception occurs.</returns>
		public static Primitive GetFileName(Primitive filePath)
		{
			try {
				string fileName = Path.GetFileName(filePath);
				return fileName;
			} catch (Exception ex) {
				return "Error: " + ex.Message;
			}
		}
	}
		
	
	
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
	
	/// <summary>
	/// Provides System.Environment Functions For Small Basic.
	/// </summary>
	[SmallBasicType]
	public class ZSEnvironment
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
		public static long WorkingSet {
			get {
				return Environment.WorkingSet;
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
		/// Returns the command-line arguments for the process.
		/// </summary>
		/// <returns>An array of strings representing the command-line arguments.</returns>
		public static string[] GetCommandLineArgs()
		{
			return Environment.GetCommandLineArgs();
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
		public static string[] GetLogicalDrives()
		{
			return Environment.GetLogicalDrives();
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
	
	/// <summary>
	/// Provides System.Math Functions For Small Basic.
	/// </summary>
	[SmallBasicType]
	public static class ZSMath
	{
		/// <summary>
		/// Returns the absolute value of a specified number.
		/// </summary>
		/// <param name="value">A number whose absolute value is to be found.</param>
		/// <returns>The absolute value of the specified number.</returns>
		public static Primitive Abs(Primitive value)
		{
			return System.Math.Abs((double)value);
		}

		/// <summary>
		/// Returns the angle whose cosine is the specified number.
		/// </summary>
		/// <param name="d">A number representing a cosine, where d must be greater than or equal to -1, but less than or equal to 1.</param>
		/// <returns>The angle, measured in radians, whose cosine is the specified number.</returns>
		public static Primitive Acos(Primitive d)
		{
			return System.Math.Acos((double)d);
		}

		/// <summary>
		/// Returns the angle whose sine is the specified number.
		/// </summary>
		/// <param name="d">A number representing a sine, where d must be greater than or equal to -1, but less than or equal to 1.</param>
		/// <returns>The angle, measured in radians, whose sine is the specified number.</returns>
		public static Primitive Asin(Primitive d)
		{
			return System.Math.Asin((double)d);
		}

		/// <summary>
		/// Returns the angle whose tangent is the specified number.
		/// </summary>
		/// <param name="d">A number representing a tangent.</param>
		/// <returns>The angle, measured in radians, whose tangent is the specified number.</returns>
		public static Primitive Atan(Primitive d)
		{
			return System.Math.Atan((double)d);
		}

		/// <summary>
		/// Returns the angle whose tangent is the quotient of two specified numbers.
		/// </summary>
		/// <param name="y">The y-coordinate of a point.</param>
		/// <param name="x">The x-coordinate of a point.</param>
		/// <returns>The angle, measured in radians, whose tangent is the quotient of two specified numbers.</returns>
		public static Primitive Atan2(Primitive y, Primitive x)
		{
			return System.Math.Atan2((double)y, (double)x);
		}

		/// <summary>
		/// Returns the smallest integer greater than or equal to the specified number.
		/// </summary>
		/// <param name="a">A number.</param>
		/// <returns>The smallest integer greater than or equal to the specified number.</returns>
		public static Primitive Ceiling(Primitive a)
		{
			return System.Math.Ceiling((double)a);
		}

		/// <summary>
		/// Returns the cosine of the specified angle.
		/// </summary>
		/// <param name="d">An angle, measured in radians.</param>
		/// <returns>The cosine of the specified angle.</returns>
		public static Primitive Cos(Primitive d)
		{
			return System.Math.Cos((double)d);
		}

		/// <summary>
		/// Returns the hyperbolic cosine of the specified angle.
		/// </summary>
		/// <param name="value">An angle, measured in radians.</param>
		/// <returns>The hyperbolic cosine of the specified angle.</returns>
		public static Primitive Cosh(Primitive value)
		{
			return System.Math.Cosh((double)value);
		}

		/// <summary>
		/// Returns e raised to the specified power.
		/// </summary>
		/// <param name="d">A number specifying a power.</param>
		/// <returns>The number e raised to the specified power.</returns>
		public static Primitive Exp(Primitive d)
		{
			return System.Math.Exp((double)d);
		}

		/// <summary>
		/// Returns the largest integer less than or equal to the specified number.
		/// </summary>
		/// <param name="d">A number.</param>
		/// <returns>The largest integer less than or equal to the specified number.</returns>
		public static Primitive Floor(Primitive d)
		{
			return System.Math.Floor((double)d);
		}

		/// <summary>
		/// Returns the remainder resulting from the division of a specified number by another specified number.
		/// </summary>
		/// <param name="x">A dividend.</param>
		/// <param name="y">A divisor.</param>
		/// <returns>A number equal to x - (y * Q), where Q is the quotient of x / y rounded to the nearest integer.</returns>
		public static Primitive IEEERemainder(Primitive x, Primitive y)
		{
			return System.Math.IEEERemainder((double)x, (double)y);
		}

		/// <summary>
		/// Returns the natural (base e) logarithm of a specified number.
		/// </summary>
		/// <param name="d">A number whose logarithm is to be found.</param>
		/// <returns>The natural (base e) logarithm of the specified number.</returns>
		public static Primitive Log(Primitive d)
		{
			return System.Math.Log((double)d);
		}

		/// <summary>
		/// Returns the base 10 logarithm of a specified number.
		/// </summary>
		/// <param name="d">A number whose logarithm is to be found.</param>
		/// <returns>The base 10 logarithm of the specified number.</returns>
		public static Primitive Log10(Primitive d)
		{
			return System.Math.Log10((double)d);
		}

		/// <summary>
		/// Returns the larger of two specified numbers.
		/// </summary>
		/// <param name="val1">The first of two numbers to compare.</param>
		/// <param name="val2">The second of two numbers to compare.</param>
		/// <returns>The larger of the two numbers.</returns>
		public static Primitive Max(Primitive val1, Primitive val2)
		{
			return System.Math.Max((double)val1, (double)val2);
		}

		/// <summary>
		/// Returns the smaller of two specified numbers.
		/// </summary>
		/// <param name="val1">The first of two numbers to compare.</param>
		/// <param name="val2">The second of two numbers to compare.</param>
		/// <returns>The smaller of the two numbers.</returns>
		public static Primitive Min(Primitive val1, Primitive val2)
		{
			return System.Math.Min((double)val1, (double)val2);
		}

		/// <summary>
		/// Returns a specified number raised to the specified power.
		/// </summary>
		/// <param name="x">A double-precision floating-point number to be raised to a power.</param>
		/// <param name="y">A double-precision floating-point number that specifies a power.</param>
		/// <returns>The number x raised to the power y.</returns>
		public static Primitive Pow(Primitive x, Primitive y)
		{
			return System.Math.Pow((double)x, (double)y);
		}

		/// <summary>
		/// Rounds a specified number to the nearest integer.
		/// </summary>
		/// <param name="a">A number to be rounded.</param>
		/// <returns>The integer nearest to the specified number.</returns>
		public static Primitive Round(Primitive a)
		{
			return System.Math.Round((double)a);
		}

		/// <summary>
		/// Returns a value indicating the sign of a number.
		/// </summary>
		/// <param name="value">A signed number.</param>
		/// <returns>A number indicating the sign of the specified number.</returns>
		public static Primitive Sign(Primitive value)
		{
			return System.Math.Sign((double)value);
		}

		/// <summary>
		/// Returns the sine of the specified angle.
		/// </summary>
		/// <param name="a">An angle, measured in radians.</param>
		/// <returns>The sine of the specified angle.</returns>
		public static Primitive Sin(Primitive a)
		{
			return System.Math.Sin((double)a);
		}

		/// <summary>
		/// Returns the hyperbolic sine of the specified angle.
		/// </summary>
		/// <param name="value">An angle, measured in radians.</param>
		/// <returns>The hyperbolic sine of the specified angle.</returns>
		public static Primitive Sinh(Primitive value)
		{
			return System.Math.Sinh((double)value);
		}

		/// <summary>
		/// Returns the square root of a specified number.
		/// </summary>
		/// <param name="d">A number.</param>
		/// <returns>The square root of the specified number.</returns>
		public static Primitive Sqrt(Primitive d)
		{
			return System.Math.Sqrt((double)d);
		}

		/// <summary>
		/// Returns the tangent of the specified angle.
		/// </summary>
		/// <param name="a">An angle, measured in radians.</param>
		/// <returns>The tangent of the specified angle.</returns>
		public static Primitive Tan(Primitive a)
		{
			return System.Math.Tan((double)a);
		}

		/// <summary>
		/// Returns the hyperbolic tangent of the specified angle.
		/// </summary>
		/// <param name="value">An angle, measured in radians.</param>
		/// <returns>The hyperbolic tangent of the specified angle.</returns>
		public static Primitive Tanh(Primitive value)
		{
			return System.Math.Tanh((double)value);
		}

		/// <summary>
		/// Calculates the integral part of a specified number.
		/// </summary>
		/// <param name="d">A number to truncate.</param>
		/// <returns>The integral part of the specified number.</returns>
		public static Primitive Truncate(Primitive d)
		{
			return System.Math.Truncate((double)d);
		}

		/// <summary>
		/// Represents the natural logarithmic base, specified by the constant, e.
		/// </summary>
		public static Primitive E {
			get {
				return System.Math.E;
			}
                     
		}
        
        
        
        
        
		/// <summary>
		/// Represents the ratio of the circumference of a circle to its diameter, specified by the constant, PI.
		/// </summary>
		public static Primitive PI {
			get {
				return System.Math.PI;
        	  
			}
                     
		}
	}
	
    
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
	
	/// <summary>
	/// Provides methods and properties for working with globalization, including culture information, date and time formatting, number formatting, and text information.
	/// This class also includes support for various calendars such as Gregorian, Hijri, Chinese, and Korean.
	/// </summary>
	[SmallBasicType]
	public static class ZSGlobalization
	{
		private static CultureInfo cultureInfo = CultureInfo.CurrentCulture;

		/// <summary>
		/// Gets or sets the current culture. Example: "en-US"
		/// </summary>
		/// <example>
		/// Get the current culture: <br />
		/// <code>currentCulture = ZSGlobalization.CurrentCulture</code><br />
		/// Set the current culture to French (France): <br />
		/// <code>ZSGlobalization.CurrentCulture = "fr-FR"</code>
		/// </example>
		public static Primitive CurrentCulture {
			get { return cultureInfo.Name; }
			set { cultureInfo = new CultureInfo((string)value); }
		}

		/// <summary>
		/// Gets the name of the calendar used by the current culture. Example: "GregorianCalendar"
		/// </summary>
		public static Primitive Calendar {
			get { return cultureInfo.Calendar.ToString(); }
		}

		/// <summary>
		/// Gets or sets the date and time pattern for short dates. Example: "MM/dd/yyyy"
		/// </summary>
		/// <example>
		/// Get the short date pattern: <br />
		/// <code>shortDatePattern = ZSGlobalization.ShortDatePattern</code><br />
		/// Set the short date pattern to day/month/year: <br />
		/// <code>ZSGlobalization.ShortDatePattern = "dd/MM/yyyy"</code>
		/// </example>
		public static Primitive ShortDatePattern {
			get { return cultureInfo.DateTimeFormat.ShortDatePattern; }
			set { cultureInfo.DateTimeFormat.ShortDatePattern = (string)value; }
		}

		/// <summary>
		/// Gets or sets the date and time pattern for long dates. Example: "dddd, MMMM dd, yyyy"
		/// </summary>
		/// <example>
		/// Get the long date pattern: <br />
		/// <code>longDatePattern = ZSGlobalization.LongDatePattern</code><br />
		/// Set the long date pattern to day of week, day month year: <br />
		/// <code>ZSGlobalization.LongDatePattern = "dddd, dd MMMM yyyy"</code>
		/// </example>
		public static Primitive LongDatePattern {
			get { return cultureInfo.DateTimeFormat.LongDatePattern; }
			set { cultureInfo.DateTimeFormat.LongDatePattern = (string)value; }
		}

		/// <summary>
		/// Gets or sets the number decimal separator. Example: "."
		/// </summary>
		/// <example>
		/// Get the number decimal separator: <br />
		/// <code>decimalSeparator = ZSGlobalization.NumberDecimalSeparator</code><br />
		/// Set the number decimal separator to a comma: <br />
		/// <code>ZSGlobalization.NumberDecimalSeparator = ","</code>
		/// </example>
		public static Primitive NumberDecimalSeparator {
			get { return cultureInfo.NumberFormat.NumberDecimalSeparator; }
			set { cultureInfo.NumberFormat.NumberDecimalSeparator = (string)value; }
		}

		/// <summary>
		/// Gets or sets the currency symbol. Example: "$"
		/// </summary>
		/// <example>
		/// Get the currency symbol: <br />
		/// <code>currencySymbol = ZSGlobalization.CurrencySymbol</code><br />
		/// Set the currency symbol to Euro: <br />
		/// <code>ZSGlobalization.CurrencySymbol = "€"</code>
		/// </example>
		public static Primitive CurrencySymbol {
			get { return cultureInfo.NumberFormat.CurrencySymbol; }
			set { cultureInfo.NumberFormat.CurrencySymbol = (string)value; }
		}


		/// <summary>
		/// Gets the text information (casing) of the current culture. Example: "Invariant"
		/// </summary>
		/// <example>
		/// Get the text info: <br />
		/// <code>textInfo = ZSGlobalization.TextInfo</code>
		/// </example>
		public static Primitive TextInfo {
			get { return cultureInfo.TextInfo.ToString(); }
		}

		/// <summary>
		/// Sets the text information (casing) of the culture. Example: "tr-TR"
		/// </summary>
		/// <example>
		/// Set the text info to a new culture: <br />
		/// <code>ZSGlobalization.SetTextInfo("tr-TR")</code>
		/// </example>
		/// <param name="cultureName">The name of the culture whose text info is to be used.</param>
		//public static void SetTextInfo(Primitive cultureName)
		//{
		//  cultureInfo = new CultureInfo(cultureInfo.Name)
		//  {
		//      TextInfo = new CultureInfo((string)cultureName).TextInfo
		//  };
		//}

		/// <summary>
		/// Gets the ISO 639-1 two-letter code for the language of the current culture. Example: "en"
		/// </summary>
		public static Primitive TwoLetterISOLanguageName {
			get { return cultureInfo.TwoLetterISOLanguageName; }
		}

		/// <summary>
		/// Gets the ISO 639-2 three-letter code for the language of the current culture. Example: "eng"
		/// </summary>
		public static Primitive ThreeLetterISOLanguageName {
			get { return cultureInfo.ThreeLetterISOLanguageName; }
		}

		/// <summary>
		/// Gets the Windows three-letter code for the language of the current culture. Example: "ENU"
		/// </summary>
		public static Primitive ThreeLetterWindowsLanguageName {
			get { return cultureInfo.ThreeLetterWindowsLanguageName; }
		}

		/// <summary>
		/// Gets the native name of the language of the current culture. Example: "English"
		/// </summary>
		public static Primitive NativeName {
			get { return cultureInfo.NativeName; }
		}

		// Additional calendar support

		/// <summary>
		/// Gets the current date in the Hijri calendar.
		/// </summary>
		public static Primitive HijriDate {
			get {
				HijriCalendar hijriCalendar = new HijriCalendar();
				DateTime dateTime = DateTime.Now;
				return hijriCalendar.GetYear(dateTime) + "/" + hijriCalendar.GetMonth(dateTime) + "/" + hijriCalendar.GetDayOfMonth(dateTime);
			}
		}

		/// <summary>
		/// Gets the current date in the Chinese calendar.
		/// </summary>
		public static Primitive ChineseDate {
			get {
				ChineseLunisolarCalendar chineseCalendar = new ChineseLunisolarCalendar();
				DateTime dateTime = DateTime.Now;
				return chineseCalendar.GetYear(dateTime) + "/" + chineseCalendar.GetMonth(dateTime) + "/" + chineseCalendar.GetDayOfMonth(dateTime);
			}
		}

		/// <summary>
		/// Gets the current date in the Gregorian calendar.
		/// </summary>
		public static Primitive GregorianDate {
			get {
				GregorianCalendar gregorianCalendar = new GregorianCalendar();
				DateTime dateTime = DateTime.Now;
				return gregorianCalendar.GetYear(dateTime) + "/" + gregorianCalendar.GetMonth(dateTime) + "/" + gregorianCalendar.GetDayOfMonth(dateTime);
			}
		}

		/// <summary>
		/// Gets the current date in the Korean calendar.
		/// </summary>
		public static Primitive KoreanDate {
			get {
				KoreanCalendar koreanCalendar = new KoreanCalendar();
				DateTime dateTime = DateTime.Now;
				return koreanCalendar.GetYear(dateTime) + "/" + koreanCalendar.GetMonth(dateTime) + "/" + koreanCalendar.GetDayOfMonth(dateTime);
			}
		}
	}
	
		
	
	
}
