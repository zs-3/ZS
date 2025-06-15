using Microsoft.SmallBasic.Library;
using System.Windows.Forms;
using System.Drawing;

namespace ZS
{
	/// <summary>
	/// Clipboard Functions In Small Basic.
	/// </summary>
	[SmallBasicType]
	public static class ZSClipboard
	{
		
		/// <summary>
		/// Gets The Text in clipboard.
		/// </summary>
		/// <returns>The text</returns>
		public static Primitive GetText()
		{
			return  Clipboard.GetText();
		}
		
		/// <summary>
		/// Clears the clipboard.
		/// </summary>
		public static void Clear()
		{
			Clipboard.Clear();
		}
		
		/// <summary>
		/// Set the text to clipboard.
		/// </summary>
		/// <param name="Text">the text to set</param>
		public static void SetText(Primitive Text)
		{
			Clipboard.SetText(Text.ToString());
		}
		
		/// <summary>
		/// True or False
		/// </summary>
		/// <returns>True or False</returns>
		public static Primitive ContainsAudio()
		{
			return  Clipboard.ContainsAudio().ToString();
		}
		
		/// <summary>
		/// True or False
		/// </summary>
		/// <returns>True or False</returns>
		public static Primitive ContainsFileDropList()
		{
			return  Clipboard.ContainsFileDropList().ToString();
		}
		
		/// <summary>
		/// True or False
		/// </summary>
		/// <returns>True or False</returns>
		public static Primitive ContainsImage()
		{
			return  Clipboard.ContainsImage().ToString();
		}
		
		/// <summary>
		/// True or False
		/// </summary>
		/// <returns>True or False</returns>
		public static Primitive ContainsText()
		{
			return  Clipboard.ContainsText().ToString();
		}
		
		/// <summary>
		/// Get file drop list.
		/// </summary>
		/// <returns>the list</returns>
		public static Primitive GetFileDropList()
		{
			return  Clipboard.GetFileDropList().ToString();
		}
		
		/// <summary>
		/// Get the image in clipboard.
		/// </summary>
		/// <param name="Path">the path to save the image</param>
		public static void GetImage(Primitive Path)
		{
			Clipboard.GetImage().Save(Path.ToString());
		}
		
		/// <summary>
		/// Set clipboard image
		/// </summary>
		/// <param name="Path">The image path</param>
		public static void SetImage(Primitive Path)
		{
			Image myimage = Image.FromFile(Path.ToString());
			Clipboard.SetImage(myimage);
		}
		
	}
	
}