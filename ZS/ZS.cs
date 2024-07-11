using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.SmallBasic.Library;
using System.Windows.Forms;
using System.Web;
using System.IO;

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
	
	
}
