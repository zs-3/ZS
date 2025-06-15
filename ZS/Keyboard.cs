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
}