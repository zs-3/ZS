using Microsoft.SmallBasic.Library;
using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Collections.Generic;

namespace ZS
{
	/// <summary>
	/// The Isolated Storage For Small Basic.
	/// You Can Store Your Varibles Here For Permenatly Even if the exe close.
	/// Default File Name Is None.
	/// Use Delete Storage For Deleting The File.
	/// </summary>
	[SmallBasicType]
	public static class ZSStorage
	{
		private static string fileName = "";

        /// <summary>
        /// Sets A Value for a variable. 
        /// </summary>
        /// <param name="Name">The Name Of Variable.</param>
        /// <param name="value">The value to store.</param>
		public static void SetValue(Primitive Name, Primitive value)
		{
			var data = LoadData();
			data[Name.ToString()] = value.ToString();
			SaveData(data);
		}

		/// <summary>
		/// Gets the value for a give variable.
		/// </summary>
		/// <param name="Name">The name of variable.</param>
		/// <returns>The value for variable or a empty string if no variable with name.</returns>
		public static Primitive GetValue(Primitive Name)
		{
			var data = LoadData();
			if (data.ContainsKey(Name.ToString())) { 
				return data[Name.ToString()]; 
			}
			return "";
		}

        /// <summary>
        /// List the varibles stored in file.
        /// </summary>
        /// <returns></returns>
		public static Primitive ListVars()
		{
			var data = LoadData(); // Load existing data
			string varList = string.Join(", ", data.Keys); // Create a comma-separated list of variable names
			return varList; // Return the list of variable names
		}

		/// <summary>
		/// Deletes the file.
		/// </summary>
		public static void DeleteStorageFile()
		{
			using (var isoStore = IsolatedStorageFile.GetUserStoreForAssembly()) {
				if (isoStore.FileExists(fileName)) {
					isoStore.DeleteFile(fileName); // Delete the storage file
				}
			}
		}

		/// <summary>
		/// Set the file name for current.
		/// </summary>
		/// <param name="FileName">the file name</param>
		public static void SetFileName(Primitive FileName)
		{
			if (!string.IsNullOrEmpty(FileName.ToString())) {
				fileName = FileName.ToString(); // Set the file name to the user-defined name
			}
		}

		// Helper method to load data from isolated storage
		private static Dictionary<string, string> LoadData()
		{
			var data = new Dictionary<string, string>();

			using (var isoStore = IsolatedStorageFile.GetUserStoreForAssembly()) {
				if (isoStore.FileExists(fileName)) {
					using (var stream = new IsolatedStorageFileStream(fileName, FileMode.Open, isoStore))
					using (var reader = new StreamReader(stream)) {
						string line;
						while ((line = reader.ReadLine()) != null) {
							var parts = line.Split(new[] { '=' }, 2);
							if (parts.Length == 2) {
								data[parts[0]] = parts[1]; // Store the variable-value pair
							}
						}
					}
				}
			}
			return data;
		}

		// Helper method to save data to isolated storage
		private static void SaveData(Dictionary<string, string> data)
		{
			using (var isoStore = IsolatedStorageFile.GetUserStoreForAssembly())
			using (var stream = new IsolatedStorageFileStream(fileName, FileMode.Create, isoStore))
			using (var writer = new StreamWriter(stream)) {
				foreach (var kvp in data) {
					writer.WriteLine(kvp.Key + "=" + kvp.Value); // Save key-value pairs
				}
			}
		}
		
	}
}