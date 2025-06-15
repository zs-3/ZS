using Microsoft.SmallBasic.Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace ZS
{
	/// <summary>
	/// Store varibles and acces even after exe close.
	/// </summary>
    [SmallBasicType]
    public static class ZSVar
    {
        private static string defaultFile;
        private static string currentFile;

        static ZSVar()
        {
            string exePath = Assembly.GetEntryAssembly().Location;
            defaultFile = Path.ChangeExtension(exePath, ".zsbin");
            currentFile = defaultFile;
            EnsureFileExists();
        }

        /// <summary>
        /// Sets a custom binary variable file instead of the default (exename.zsbin).
        /// </summary>
        /// <param name="path">The full file path to use as the .zsbin store.</param>
        public static void SetFile(Primitive path)
        {
            try {
                currentFile = path;
                EnsureFileExists();
            } catch (Exception ex) {
                ZSUtilities.OnError(ex);
            }
        }

        /// <summary>
        /// Resets to use the default .zsbin file based on the EXE name.
        /// </summary>
        public static void ResetFile()
        {
            try {
                currentFile = defaultFile;
            } catch (Exception ex) {
                ZSUtilities.OnError(ex);
            }
        }

        /// <summary>
        /// Saves a variable to the .zsbin file. Replaces if it already exists.
        /// </summary>
        /// <param name="name">The variable name.</param>
        /// <param name="value">The variable value as a string.</param>
        public static void Save(Primitive name, Primitive value)
        {
            try {
                var all = LoadAll();
                all[name] = value;

                using (var fs = new FileStream(currentFile, FileMode.Create, FileAccess.Write))
                using (var bw = new BinaryWriter(fs))
                {
                    foreach (var pair in all)
                    {
                        var keyBytes = Encoding.UTF8.GetBytes(pair.Key);
                        var valBytes = Encoding.UTF8.GetBytes(pair.Value);
                        bw.Write(keyBytes.Length);
                        bw.Write(keyBytes);
                        bw.Write(valBytes.Length);
                        bw.Write(valBytes);
                    }
                }
            } catch (Exception ex) {
                ZSUtilities.OnError(ex);
            }
        }

        /// <summary>
        /// Gets the value of a stored variable from the .zsbin file.
        /// </summary>
        /// <param name="name">The variable name.</param>
        /// <returns>The stored value, or empty string if not found or error.</returns>
        public static Primitive Get(Primitive name)
        {
            try {
                using (var fs = new FileStream(currentFile, FileMode.Open, FileAccess.Read))
                using (var br = new BinaryReader(fs))
                {
                    while (fs.Position < fs.Length)
                    {
                        int nameLen = br.ReadInt32();
                        string entryName = Encoding.UTF8.GetString(br.ReadBytes(nameLen));

                        int valLen = br.ReadInt32();
                        byte[] valBytes = br.ReadBytes(valLen);

                        if (entryName == name)
                        {
                            return Encoding.UTF8.GetString(valBytes);
                        }
                    }
                }
            } catch (Exception ex) {
                ZSUtilities.OnError(ex);
            }
            return "";
        }

        /// <summary>
        /// Deletes a variable from the .zsbin file.
        /// </summary>
        /// <param name="name">The variable name to delete.</param>
        public static void Delete(Primitive name)
        {
            try {
                var all = LoadAll();
                if (all.ContainsKey(name))
                {
                    all.Remove(name);

                    using (var fs = new FileStream(currentFile, FileMode.Create, FileAccess.Write))
                    using (var bw = new BinaryWriter(fs))
                    {
                        foreach (var pair in all)
                        {
                            var keyBytes = Encoding.UTF8.GetBytes(pair.Key);
                            var valBytes = Encoding.UTF8.GetBytes(pair.Value);
                            bw.Write(keyBytes.Length);
                            bw.Write(keyBytes);
                            bw.Write(valBytes.Length);
                            bw.Write(valBytes);
                        }
                    }
                }
            } catch (Exception ex) {
                ZSUtilities.OnError(ex);
            }
        }

        /// <summary>
        /// Checks whether a variable exists in the .zsbin file.
        /// </summary>
        /// <param name="name">The variable name to check.</param>
        /// <returns>True if the variable exists, otherwise false.</returns>
        public static Primitive	Exists(Primitive name)
        {
            try {
                using (var fs = new FileStream(currentFile, FileMode.Open, FileAccess.Read))
                using (var br = new BinaryReader(fs))
                {
                    while (fs.Position < fs.Length)
                    {
                        int nameLen = br.ReadInt32();
                        string entryName = Encoding.UTF8.GetString(br.ReadBytes(nameLen));

                        int valLen = br.ReadInt32();
                        fs.Position += valLen;

                        if (entryName == name)
                        	return true.ToString();
                    }
                }
            } catch (Exception ex) {
                ZSUtilities.OnError(ex);
            }
        	return false.ToString();
        }

        /// <summary>
        /// Lists all stored variable names in the .zsbin file.
        /// </summary>
        /// <returns>An array of variable names.</returns>
        public static Primitive ListAll()
        {
            try {
                Primitive keys = "";
                int index = 1;
                using (var fs = new FileStream(currentFile, FileMode.Open, FileAccess.Read))
                using (var br = new BinaryReader(fs))
                {
                    while (fs.Position < fs.Length)
                    {
                        int nameLen = br.ReadInt32();
                        string name = Encoding.UTF8.GetString(br.ReadBytes(nameLen));
                        int valLen = br.ReadInt32();
                        fs.Position += valLen;
                        keys[index] = name;
                        ++index;
                    }
                }
                return keys;
            } catch (Exception ex) {
                ZSUtilities.OnError(ex);
                return "Error";
            }
        }

        private static Dictionary<string, string> LoadAll()
        {
            var map = new Dictionary<string, string>();
            try {
                using (var fs = new FileStream(currentFile, FileMode.Open, FileAccess.Read))
                using (var br = new BinaryReader(fs))
                {
                    while (fs.Position < fs.Length)
                    {
                        int nameLen = br.ReadInt32();
                        string name = Encoding.UTF8.GetString(br.ReadBytes(nameLen));
                        int valLen = br.ReadInt32();
                        string value = Encoding.UTF8.GetString(br.ReadBytes(valLen));
                        map[name] = value;
                    }
                }
            } catch (Exception ex) {
                ZSUtilities.OnError(ex);
            }
            return map;
        }

        private static void EnsureFileExists()
        {
            try {
                if (!System.IO.File.Exists(currentFile))
                {
                    System.IO.File.Create(currentFile).Dispose();
                }
            } catch (Exception ex) {
                ZSUtilities.OnError(ex);
            }
        }
    }
}
