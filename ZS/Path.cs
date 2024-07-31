using System;
using System.IO;
using Microsoft.SmallBasic.Library;

namespace ZS
{
	/// <summary>
	/// Provides System.IO.Path Functions in small basic.
	/// </summary>
	[SmallBasicType]
	public static class ZSPath
	{
		/// <summary>
		/// Changes the extension of a path string.
		/// </summary>
		/// <param name="path">The path information to modify.</param>
		/// <param name="extension">The new extension. Specify null to remove an existing extension from path.</param>
		/// <returns>The modified path information.</returns>
		public static Primitive ChangeExtension(Primitive path, Primitive extension)
		{
			return Path.ChangeExtension(path.ToString(), extension.ToString());
		}

		/// <summary>
		/// Combines two strings into a path.
		/// </summary>
		/// <param name="path1">The first path to combine.</param>
		/// <param name="path2">The second path to combine.</param>
		/// <returns>The combined paths.</returns>
		public static Primitive Combine(Primitive path1, Primitive path2)
		{
			return Path.Combine(path1.ToString(), path2.ToString());
		}

		/// <summary>
		/// Combines three strings into a path.
		/// </summary>
		/// <param name="path1">The first path to combine.</param>
		/// <param name="path2">The second path to combine.</param>
		/// <param name="path3">The third path to combine.</param>
		/// <returns>The combined paths.</returns>
		public static Primitive Combine3(Primitive path1, Primitive path2, Primitive path3)
		{
			return Path.Combine(path1.ToString(), path2.ToString(), path3.ToString());
		}

		/// <summary>
		/// Combines four strings into a path.
		/// </summary>
		/// <param name="path1">The first path to combine.</param>
		/// <param name="path2">The second path to combine.</param>
		/// <param name="path3">The third path to combine.</param>
		/// <param name="path4">The fourth path to combine.</param>
		/// <returns>The combined paths.</returns>
		public static Primitive Combine4(Primitive path1, Primitive path2, Primitive path3, Primitive path4)
		{
			return Path.Combine(path1.ToString(), path2.ToString(), path3.ToString(), path4.ToString());
		}

		/// <summary>
		/// Returns the directory information for the specified path string.
		/// </summary>
		/// <param name="path">The path of a file or directory.</param>
		/// <returns>The directory information.</returns>
		public static Primitive GetDirectoryName(Primitive path)
		{
			return Path.GetDirectoryName(path.ToString());
		}

		/// <summary>
		/// Returns the extension of the specified path string.
		/// </summary>
		/// <param name="path">The path string from which to get the extension.</param>
		/// <returns>The extension of the specified path.</returns>
		public static Primitive GetExtension(Primitive path)
		{
			return Path.GetExtension(path.ToString());
		}

		/// <summary>
		/// Returns the file name and extension of the specified path string.
		/// </summary>
		/// <param name="path">The path string from which to obtain the file name and extension.</param>
		/// <returns>The characters after the last directory character in path.</returns>
		public static Primitive GetFileName(Primitive path)
		{
			return Path.GetFileName(path.ToString());
		}

		/// <summary>
		/// Returns the file name of the specified path string without the extension.
		/// </summary>
		/// <param name="path">The path of the file.</param>
		/// <returns>The string returned by GetFileName, minus the last period and all characters following it.</returns>
		public static Primitive GetFileNameWithoutExtension(Primitive path)
		{
			return Path.GetFileNameWithoutExtension(path.ToString());
		}

		/// <summary>
		/// Returns the absolute path for the specified path string.
		/// </summary>
		/// <param name="path">The file or directory for which to obtain absolute path information.</param>
		/// <returns>The fully qualified location of path.</returns>
		public static Primitive GetFullPath(Primitive path)
		{
			return Path.GetFullPath(path.ToString());
		}

		/// <summary>
		/// Returns an array containing the characters that are not allowed in file names.
		/// </summary>
		/// <returns>An array containing the characters that are not allowed in file names.</returns>
		public static Primitive GetInvalidFileNameChars()
		{
			return new Primitive(new string(Path.GetInvalidFileNameChars()));
		}

		/// <summary>
		/// Returns an array containing the characters that are not allowed in path names.
		/// </summary>
		/// <returns>An array containing the characters that are not allowed in path names.</returns>
		public static Primitive GetInvalidPathChars()
		{
			return new Primitive(new string(Path.GetInvalidPathChars()));
		}

		/// <summary>
		/// Gets the root directory information of the specified path.
		/// </summary>
		/// <param name="path">The path from which to obtain root directory information.</param>
		/// <returns>A string containing the root directory of path.</returns>
		public static Primitive GetPathRoot(Primitive path)
		{
			return Path.GetPathRoot(path.ToString());
		}

		/// <summary>
		/// Returns a random file name.
		/// </summary>
		/// <returns>A random file name.</returns>
		public static Primitive GetRandomFileName()
		{
			return Path.GetRandomFileName();
		}

		/// <summary>
		/// Creates a uniquely named, zero-byte temporary file on disk and returns the full path of that file.
		/// </summary>
		/// <returns>The full path of the temporary file.</returns>
		public static Primitive GetTempFileName()
		{
			return Path.GetTempFileName();
		}

		/// <summary>
		/// Returns the path of the current system's temporary folder.
		/// </summary>
		/// <returns>The path to the temporary folder.</returns>
		public static Primitive GetTempPath()
		{
			return Path.GetTempPath();
		}

		/// <summary>
		/// Determines whether a path includes a file name extension.
		/// </summary>
		/// <param name="path">The path to search for an extension.</param>
		/// <returns>true if the path includes a file name extension; otherwise, false.</returns>
		public static Primitive HasExtension(Primitive path)
		{
			return Path.HasExtension(path.ToString());
		}


		/// <summary>
		/// Gets a value indicating whether the specified path string contains a root.
		/// </summary>
		/// <param name="path">The path to test.</param>
		/// <returns>true if path contains a root; otherwise, false.</returns>
		public static Primitive IsPathRooted(Primitive path)
		{
			return Path.IsPathRooted(path.ToString());
		}

	}
}
