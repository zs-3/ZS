using System;
using Microsoft.SmallBasic.Library;
using System.IO.Compression;
using System.IO;
using System.Collections.Generic;

namespace ZS
{
    /// <summary>
    /// The Class for Zip and UnZip of Files.
    /// </summary>
    [SmallBasicType]
    public static class ZSZip
    {
        // Dictionary to keep track of created ZipArchive instances
        private static Dictionary<string, ZipArchive> _zips = new Dictionary<string, ZipArchive>();

        /// <summary>
        /// Create a new ZIP archive.
        /// </summary>
        /// <param name="Name">The identifier name for the ZIP archive.</param>
        /// <param name="Path">The path where the ZIP archive will be saved.</param>
        /// <returns>The name of the ZIP archive created.</returns>
        public static Primitive CreateZip(Primitive Name, Primitive Path)
        {
            FileStream zipToOpen = new FileStream(Path, FileMode.Create);
            ZipArchive zip = new ZipArchive(zipToOpen, ZipArchiveMode.Create);
            _zips[Name] = zip; // Add the new zip to the dictionary
            return Name;
        }
        
        /// <summary>
        /// Open a ZIP archive.
        /// </summary>
        /// <param name="Name">The identifier name for the ZIP archive.</param>
        /// <param name="Path">The path where the ZIP archive is.</param>
        /// <returns>The name of the ZIP archive.</returns>
        public static Primitive OpenZip(Primitive Name, Primitive Path)
        {
            FileStream zipToOpen = new FileStream(Path, FileMode.Open);
            ZipArchive zip = new ZipArchive(zipToOpen, ZipArchiveMode.Update);
            _zips[Name] = zip; // Add the new zip to the dictionary
            return Name;
        }

        /// <summary>
        /// Add a file to the ZIP archive.
        /// </summary>
        /// <param name="Name">The name of the ZIP archive to add the file to.</param>
        /// <param name="FilePath">The file path of the file to add to the ZIP archive.</param>
        public static void AddFile(Primitive Name, Primitive FilePath)
        {
        	if (_zips.ContainsKey(Name))
            {
                ZipArchive zip = _zips[Name];
                string fileName = Path.GetFileName(FilePath);
                
                // Create a new entry in the zip archive
                ZipArchiveEntry entry = zip.CreateEntry(fileName, CompressionLevel.Fastest);

                // Write the file contents into the ZIP entry
                using (FileStream fs = new FileStream(FilePath, FileMode.Open))
                using (Stream zipStream = entry.Open())
                {
                    fs.CopyTo(zipStream); // Copy the file into the ZIP archive
                }
            }
            else
            {
                throw new Exception("The specified ZIP archive does not exist.");
            }
        }

        /// <summary>
        /// Close the ZIP archive and save it.
        /// </summary>
        /// <param name="Name">The name of the ZIP archive to close.</param>
        public static void CloseZip(Primitive Name)
        {
            if (_zips.ContainsKey(Name))
            {
                ZipArchive zip = _zips[Name];
                zip.Dispose(); // Close and save the zip archive
            }
            else
            {
                throw new Exception("The specified ZIP archive does not exist.");
            }
        }

        /// <summary>
        /// Extract a ZIP archive to a specified directory.
        /// </summary>
        /// <param name="ZipPath">The path to the ZIP file to extract.</param>
        /// <param name="ExtractPath">The directory where files should be extracted.</param>
        public static void ExtractZip(Primitive ZipPath, Primitive ExtractPath)
        {
            ZipFile.ExtractToDirectory(ZipPath, ExtractPath);
        }

        /// <summary>
        /// List all files currently added to a ZIP archive.
        /// </summary>
        /// <param name="Name">The name of the ZIP archive.</param>
        /// <returns>A string containing all file names in the ZIP archive.</returns>
        public static Primitive ListFiles(Primitive Name)
        {
            if (_zips.ContainsKey(Name))
            {
                ZipArchive zip = _zips[Name];
                List<string> files = new List<string>();

                foreach (var entry in zip.Entries)
                {
                    files.Add(entry.FullName);
                }

                return string.Join(", ", files);
            }
            else
            {
                throw new Exception("The specified ZIP archive does not exist.");
            }
        }
        
        /// <summary>
        /// Make A Zip file from a directory whole content will be added.
        /// </summary>
        /// <param name="SourceDir">The Directory Path.</param>
        /// <param name="ZipName">The Zip Name to be created.</param>
        /// <param name="Level">Compression Level :
        /// 0 = Fastest.
        /// 1 = No Compression.
        /// 2 = Optimal.
        ///</param>
        /// <param name="Includebase">True or False will you want they folder itself then its content into zip or the content of folder directly.</param>
        public static void CreateFromDir(Primitive SourceDir,Primitive ZipName,Primitive Level,Primitive Includebase)
        {
        	if (Level == "0") {
        		ZipFile.CreateFromDirectory(SourceDir,ZipName,CompressionLevel.Fastest,bool.Parse(Includebase));
        	}
        	if (Level == "1") {
        		ZipFile.CreateFromDirectory(SourceDir,ZipName,CompressionLevel.NoCompression,bool.Parse(Includebase));
        	}
        	if (Level == "2") {
        		ZipFile.CreateFromDirectory(SourceDir,ZipName,CompressionLevel.Optimal,bool.Parse(Includebase));
        	}        	
        }
        
        
    }
}
