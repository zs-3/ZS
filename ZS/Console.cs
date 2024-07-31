using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.SmallBasic.Library;

namespace ZS
{
	[SmallBasicType]
	public static class ZSConsole
	{

		/// <summary>
		/// Writes the specified string value to the console.
		/// </summary>
		/// <param name="text">The string value to write to the console.</param>
		public static void Write(Primitive text)
		{
			Console.Write(text.ToString());
		}

		/// <summary>
		/// Writes the specified string value to the console, followed by the current line terminator.
		/// </summary>
		/// <param name="text">The string value to write to the console.</param>
		public static void WriteLine(Primitive text)
		{
			Console.WriteLine(text.ToString());
		}

		/// <summary>
		/// Reads the next line of characters from the standard input stream.
		/// </summary>
		/// <returns>The next line of characters from the input stream, or null if no more lines are available.</returns>
		public static Primitive ReadLine()
		{
			return Console.ReadLine();
		}

		/// <summary>
		/// Reads the next character from the standard input stream.
		/// </summary>
		/// <returns>The next character from the input stream, or -1 if no more characters are available.</returns>
		public static Primitive Read()
		{
			return Console.Read();
		}

		/// <summary>
		/// Clears the console buffer and corresponding console window of display information.
		/// </summary>
		public static void Clear()
		{
			Console.Clear();
		}

		/// <summary>
		/// Gets or sets the title of the console window.
		/// </summary>
		public static Primitive Title {
			get { return Console.Title; }
			set { Console.Title = value; }
		}

		/// <summary>
		/// Gets or sets the column position of the cursor within the buffer area.
		/// </summary>
		public static Primitive CursorLeft {
			get { return Console.CursorLeft; }
			set { Console.CursorLeft = value; }
		}

		/// <summary>
		/// Gets or sets the row position of the cursor within the buffer area.
		/// </summary>
		public static Primitive CursorTop {
			get { return Console.CursorTop; }
			set { Console.CursorTop = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the cursor is visible.
		/// </summary>
		public static Primitive CursorVisible {
			get { return Console.CursorVisible; }
			set { Console.CursorVisible = value; }
		}

		/// <summary>
		/// Gets or sets the height of the buffer area.
		/// </summary>
		public static Primitive BufferHeight {
			get { return Console.BufferHeight; }
			set { Console.BufferHeight = value; }
		}

		/// <summary>
		/// Gets or sets the width of the buffer area.
		/// </summary>
		public static Primitive BufferWidth {
			get { return Console.BufferWidth; }
			set { Console.BufferWidth = value; }
		}

		/// <summary>
		/// Gets or sets the height of the console window.
		/// </summary>
		public static Primitive WindowHeight {
			get { return Console.WindowHeight; }
			set { Console.WindowHeight = value; }
		}

		/// <summary>
		/// Gets or sets the width of the console window.
		/// </summary>
		public static Primitive WindowWidth {
			get { return Console.WindowWidth; }
			set { Console.WindowWidth = value; }
		}

		/// <summary>
		/// Gets or sets the foreground color of the console.
		/// </summary>
		public static Primitive ForegroundColor {
			get { return Console.ForegroundColor.ToString(); }
			set { Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), value); }
		}

		/// <summary>
		/// Gets or sets the background color of the console.
		/// </summary>
		public static Primitive BackgroundColor {
			get { return Console.BackgroundColor.ToString(); }
			set { Console.BackgroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), value); }
		}

		/// <summary>
		/// Plays the sound of a beep through the console speaker.
		/// </summary>
		public static void Beep()
		{
			Console.Beep();
		}

		/// <summary>
		/// Plays the sound of a beep of a specified frequency and duration through the console speaker.
		/// </summary>
		/// <param name="frequency">The frequency of the beep, ranging from 37 to 32767 hertz.</param>
		/// <param name="duration">The duration of the beep in milliseconds.</param>
		public static void Beep(Primitive frequency, Primitive duration)
		{
			Console.Beep(frequency, duration);
		}

		/// <summary>
		/// Sets the position of the cursor.
		/// </summary>
		/// <param name="left">The column position of the cursor.</param>
		/// <param name="top">The row position of the cursor.</param>
		public static void SetCursorPosition(Primitive left, Primitive top)
		{
			Console.SetCursorPosition(left, top);
		}

		/// <summary>
		/// Sets the size of the buffer area.
		/// </summary>
		/// <param name="width">The width of the buffer area.</param>
		/// <param name="height">The height of the buffer area.</param>
		public static void SetBufferSize(Primitive width, Primitive height)
		{
			Console.SetBufferSize(width, height);
		}

		/// <summary>
		/// Sets the size of the console window.
		/// </summary>
		/// <param name="width">The width of the console window.</param>
		/// <param name="height">The height of the console window.</param>
		public static void SetWindowSize(Primitive width, Primitive height)
		{
			Console.SetWindowSize(width, height);
		}

		/// <summary>
		/// Sets the foreground and background console colors to their defaults.
		/// </summary>
		public static void ResetColor()
		{
			Console.ResetColor();
		}

		/// <summary>
		/// Gets the number of lines of buffer space.
		/// </summary>
		/// <returns>The number of lines of buffer space.</returns>
		public static Primitive BufferLineCount()
		{
			return Console.BufferHeight;
		}

		/// <summary>
		/// Writes a formatted string to the console.
		/// </summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="args">An array of objects to write using format.</param>
		public static void WriteFormat(Primitive format, params object[] args)
		{
			Console.Write(format, args);
		}

		/// <summary>
		/// Writes the text representation of the specified array of objects to the console, followed by the current line terminator.
		/// </summary>
		/// <param name="args">An array of objects to write.</param>
		public static void WriteLineObjects(params object[] args)
		{
			Console.WriteLine(string.Join(" ", args));
		}

		/// <summary>
		/// Reads the next character or function key pressed by the user. The pressed key is displayed in the console window.
		/// </summary>
		/// <returns>A System.ConsoleKeyInfo object describing the System.ConsoleKey constant and Unicode character, if any, that correspond to the pressed console key.</returns>
		public static Primitive ReadKey()
		{
			var keyInfo = Console.ReadKey();
			return keyInfo.Key.ToString();
		}

	}
}
