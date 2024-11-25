using System;
using System.Text.RegularExpressions;
using Microsoft.SmallBasic.Library;

namespace ZS
{
	

	/// <summary>
	/// Regex In SB.
	/// </summary>
	public static class ZSRegex
	{
		/// <summary>
		/// Matches the regex pattern in the input string and returns true if it finds a match.
		/// </summary>
		public static Primitive IsMatch(Primitive input, Primitive pattern)
		{
			return Regex.IsMatch(input.ToString(), pattern.ToString()) ? "true" : "false";
		}

		/// <summary>
		/// Searches the input string for the first occurrence of the regex pattern and returns the matched value.
		/// </summary>
		public static Primitive Match(Primitive input, Primitive pattern)
		{
			Match match = Regex.Match(input.ToString(), pattern.ToString());
			return match.Success ? match.Value : "";
		}

		/// <summary>
		/// Replaces all occurrences of the regex pattern in the input string with the replacement string.
		/// </summary>
		public static Primitive Replace(Primitive input, Primitive pattern, Primitive replacement)
		{
			return Regex.Replace(input.ToString(), pattern.ToString(), replacement.ToString());
		}

		/// <summary>
		/// Splits the input string based on the regex pattern and returns an array of substrings.
		/// </summary>
		public static Primitive[] Split(Primitive input, Primitive pattern)
		{
			string[] result = Regex.Split(input.ToString(), pattern.ToString());
			Primitive[] primitives = new Primitive[result.Length];
        
			for (int i = 0; i < result.Length; i++) {
				primitives[i] = result[i];
			}
			return primitives;
		}

		/// <summary>
		/// Finds all matches of the regex pattern in the input string and returns an array of matched values.
		/// </summary>
		public static Primitive[] FindAll(Primitive input, Primitive pattern)
		{
			MatchCollection matches = Regex.Matches(input.ToString(), pattern.ToString());
			Primitive[] primitives = new Primitive[matches.Count];
        
			for (int i = 0; i < matches.Count; i++) {
				primitives[i] = matches[i].Value;
			}
			return primitives;
		}

		/// <summary>
		/// Gets the groups captured by the regex pattern from the input string and returns an array of group values.
		/// </summary>
		public static Primitive[] GetGroups(Primitive input, Primitive pattern)
		{
			Match match = Regex.Match(input.ToString(), pattern.ToString());
			if (!match.Success) {
				return new Primitive[0]; // Return an empty array if no match is found.
			}

			Primitive[] groups = new Primitive[match.Groups.Count];
			for (int i = 0; i < match.Groups.Count; i++) {
				groups[i] = match.Groups[i].Value;
			}
			return groups;
		}

		/// <summary>
		/// Returns the number of matches found for the regex pattern in the input string.
		/// </summary>
		public static Primitive CountMatches(Primitive input, Primitive pattern)
		{
			MatchCollection matches = Regex.Matches(input.ToString(), pattern.ToString());
			return matches.Count;
		}

		/// <summary>
		/// Validates if the entire input string matches the regex pattern.
		/// </summary>
		public static Primitive IsExactMatch(Primitive input, Primitive pattern)
		{
			return Regex.IsMatch(input.ToString(), "^" + pattern.ToString() + "$") ? "true" : "false";
		}

		/// <summary>
		/// Checks if the input string starts with the regex pattern.
		/// </summary>
		public static Primitive StartsWith(Primitive input, Primitive pattern)
		{
			return Regex.IsMatch(input.ToString(), "^" + pattern.ToString()) ? "true" : "false";
		}

		/// <summary>
		/// Checks if the input string ends with the regex pattern.
		/// </summary>
		public static Primitive EndsWith(Primitive input, Primitive pattern)
		{
			return Regex.IsMatch(input.ToString(), pattern.ToString() + "$") ? "true" : "false";
		}

		/// <summary>
		/// Returns a string that contains the input string with all whitespace removed.
		/// </summary>
		public static Primitive RemoveWhitespace(Primitive input)
		{
			return Regex.Replace(input.ToString(), @"\s+", "");
		}

		/// <summary>
		/// Escapes special regex characters in the input string.
		/// </summary>
		public static Primitive Escape(Primitive input)
		{
			return Regex.Escape(input.ToString());
			
		}
	}
}