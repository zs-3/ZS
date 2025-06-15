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
}