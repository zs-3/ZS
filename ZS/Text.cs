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
using System.Text.RegularExpressions;
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
		/// Splits the input text into an array of substrings based on a delimiter.
		/// Removes The Empty.
		/// </summary>
		/// <param name="input">The text to be split.</param>
		/// <param name="delimiter">The delimiter to split by.</param>
		/// <returns>An array of substrings.</returns>
		public static Primitive SplitWithoutEmpty(Primitive input, Primitive delimiter)
		{
			string[] parts = input.ToString().Split(new string[] { delimiter.ToString() }, StringSplitOptions.RemoveEmptyEntries);
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