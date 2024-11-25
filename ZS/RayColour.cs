using System;
using Microsoft.SmallBasic.Library;
using Raylib_cs;
using System.Globalization;

namespace ZS
{
	/// <summary>
	/// Raylib Colours.
	/// </summary>
	[SmallBasicType]
	public static class ZSRayColour
	{
		[HideFromIntellisense]
		private static Color HexToRaylibColor(string hex)
		{
			// Remove the '#' if present
			if (hex.StartsWith("#")) {
				hex = hex.Substring(1);
			}

			// Parse the color based on length (6 for RGB, 8 for RGBA)
			byte r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
			byte g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
			byte b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
			byte a = 255; // Default alpha value

			if (hex.Length == 8) { // If there's an alpha value
				a = byte.Parse(hex.Substring(6, 2), NumberStyles.HexNumber);
			}

			// Return a Raylib color
			return new Color(r, g, b, a);
		}
	}
}