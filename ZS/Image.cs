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
	/// Provides Image Functions For Small Basic.
	/// </summary>
	[SmallBasicType]
	public static class ZSImage
	{
		/// <summary>
		/// Determines if a sub-image is present within a main image.
		/// </summary>
		/// <param name="mainImagePath">The file path of the main image.</param>
		/// <param name="subImagePath">The file path of the sub-image.</param>
		/// <returns>True if the sub-image is found within the main image, otherwise false.</returns>
		public static Primitive IsImageInImage(Primitive mainImagePath, Primitive subImagePath)
		{
			string mainImagePathStr = mainImagePath.ToString();
			string subImagePathStr = subImagePath.ToString();

			if (!System.IO.File.Exists(mainImagePathStr) || !System.IO.File.Exists(subImagePathStr)) {
				return false;
			}

			using (Bitmap mainImage = new Bitmap(mainImagePathStr))
			using (Bitmap subImage = new Bitmap(subImagePathStr)) {
				for (int y = 0; y <= mainImage.Height - subImage.Height; y++) {
					for (int x = 0; x <= mainImage.Width - subImage.Width; x++) {
						if (IsMatch(mainImage, subImage, x, y)) {
							return true;
						}
					}
				}
				return false;
			}
		}

		private static bool IsMatch(Bitmap mainImage, Bitmap subImage, int startX, int startY)
		{
			for (int y = 0; y < subImage.Height; y++) {
				for (int x = 0; x < subImage.Width; x++) {
					if (mainImage.GetPixel(startX + x, startY + y) != subImage.GetPixel(x, y)) {
						return false;  // Early termination on the first mismatch
					}
				}
			}
			return true;
		}
	
		/// <summary>
		/// Captures the entire screen and saves it as a PNG file.
		/// </summary>
		/// <param name="filePath">The file path where the screenshot will be saved.</param>
		/// <returns>True if the screen capture was successful; otherwise, false.</returns>
		public static Primitive CaptureScreen(Primitive filePath)
		{
			try {
				Rectangle bounds = Screen.PrimaryScreen.Bounds;
				using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height)) {
					using (Graphics g = Graphics.FromImage(bitmap)) {
						g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
					}
					bitmap.Save(filePath, ImageFormat.Png);
					return true; // Return true indicating successful capture
				}
			} catch (Exception ex) {
				MessageBox.Show("Error capturing screen: " + ex.Message);
				return false; // Return false indicating failure
			}
		}


		/// <summary>
		/// Resizes an image to the specified width and height.
		/// </summary>
		/// <param name="inputFilePath">The file path of the input image.</param>
		/// <param name="outputFilePath">The file path where the resized image will be saved.</param>
		/// <param name="width">The width of the resized image.</param>
		/// <param name="height">The height of the resized image.</param>
		/// <returns>True if the image resizing was successful; otherwise, false.</returns>
		public static Primitive ResizeImage(Primitive inputFilePath, Primitive outputFilePath, Primitive width, Primitive height)
		{
			try {
				// Convert Primitive types to appropriate types
				string inputPath = inputFilePath.ToString();
				string outputPath = outputFilePath.ToString();
				int resizeWidth = (int)width;
				int resizeHeight = (int)height;

				using (Bitmap original = new Bitmap(inputPath)) {
					using (Bitmap resized = new Bitmap(resizeWidth, resizeHeight)) {
						using (Graphics g = Graphics.FromImage(resized)) {
							g.DrawImage(original, 0, 0, resizeWidth, resizeHeight);
						}
						resized.Save(outputPath, ImageFormat.Png);
						return true; // Return true indicating successful resize
					}
				}
			} catch (Exception ex) {
				MessageBox.Show("Error resizing image: " + ex.Message);
				return false; // Return false indicating failure
			}
		}

		/// <summary>
		/// Gets the width of an image.
		/// </summary>
		/// <param name="filePath">The file path of the image.</param>
		/// <returns>The width of the image.</returns>
		public static Primitive GetImageWidth(Primitive filePath)
		{
			try {
				using (Bitmap image = new Bitmap(filePath)) {
					return image.Width;
				}
			} catch (Exception ex) {
				MessageBox.Show("Error getting image width: " + ex.Message);
				return -1; // Return -1 to indicate an error
			}
		}

		/// <summary>
		/// Gets the height of an image.
		/// </summary>
		/// <param name="filePath">The file path of the image.</param>
		/// <returns>The height of the image.</returns>
		public static Primitive GetImageHeight(Primitive filePath)
		{
			try {
				using (Bitmap image = new Bitmap(filePath)) {
					return image.Height;
				}
			} catch (Exception ex) {
				MessageBox.Show("Error getting image height: " + ex.Message);
				return -1; // Return -1 to indicate an error
			}
		}
	}
}