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
	/// Provides System.Math Functions For Small Basic.
	/// </summary>
	[SmallBasicType]
	public static class ZSMath
	{
		/// <summary>
		/// Returns the absolute value of a specified number.
		/// </summary>
		/// <param name="value">A number whose absolute value is to be found.</param>
		/// <returns>The absolute value of the specified number.</returns>
		public static Primitive Abs(Primitive value)
		{
			return System.Math.Abs((double)value);
		}

		/// <summary>
		/// Returns the angle whose cosine is the specified number.
		/// </summary>
		/// <param name="d">A number representing a cosine, where d must be greater than or equal to -1, but less than or equal to 1.</param>
		/// <returns>The angle, measured in radians, whose cosine is the specified number.</returns>
		public static Primitive Acos(Primitive d)
		{
			return System.Math.Acos((double)d);
		}

		/// <summary>
		/// Returns the angle whose sine is the specified number.
		/// </summary>
		/// <param name="d">A number representing a sine, where d must be greater than or equal to -1, but less than or equal to 1.</param>
		/// <returns>The angle, measured in radians, whose sine is the specified number.</returns>
		public static Primitive Asin(Primitive d)
		{
			return System.Math.Asin((double)d);
		}

		/// <summary>
		/// Returns the angle whose tangent is the specified number.
		/// </summary>
		/// <param name="d">A number representing a tangent.</param>
		/// <returns>The angle, measured in radians, whose tangent is the specified number.</returns>
		public static Primitive Atan(Primitive d)
		{
			return System.Math.Atan((double)d);
		}

		/// <summary>
		/// Returns the angle whose tangent is the quotient of two specified numbers.
		/// </summary>
		/// <param name="y">The y-coordinate of a point.</param>
		/// <param name="x">The x-coordinate of a point.</param>
		/// <returns>The angle, measured in radians, whose tangent is the quotient of two specified numbers.</returns>
		public static Primitive Atan2(Primitive y, Primitive x)
		{
			return System.Math.Atan2((double)y, (double)x);
		}

		/// <summary>
		/// Returns the smallest integer greater than or equal to the specified number.
		/// </summary>
		/// <param name="a">A number.</param>
		/// <returns>The smallest integer greater than or equal to the specified number.</returns>
		public static Primitive Ceiling(Primitive a)
		{
			return System.Math.Ceiling((double)a);
		}

		/// <summary>
		/// Returns the cosine of the specified angle.
		/// </summary>
		/// <param name="d">An angle, measured in radians.</param>
		/// <returns>The cosine of the specified angle.</returns>
		public static Primitive Cos(Primitive d)
		{
			return System.Math.Cos((double)d);
		}

		/// <summary>
		/// Returns the hyperbolic cosine of the specified angle.
		/// </summary>
		/// <param name="value">An angle, measured in radians.</param>
		/// <returns>The hyperbolic cosine of the specified angle.</returns>
		public static Primitive Cosh(Primitive value)
		{
			return System.Math.Cosh((double)value);
		}

		/// <summary>
		/// Returns e raised to the specified power.
		/// </summary>
		/// <param name="d">A number specifying a power.</param>
		/// <returns>The number e raised to the specified power.</returns>
		public static Primitive Exp(Primitive d)
		{
			return System.Math.Exp((double)d);
		}

		/// <summary>
		/// Returns the largest integer less than or equal to the specified number.
		/// </summary>
		/// <param name="d">A number.</param>
		/// <returns>The largest integer less than or equal to the specified number.</returns>
		public static Primitive Floor(Primitive d)
		{
			return System.Math.Floor((double)d);
		}

		/// <summary>
		/// Returns the remainder resulting from the division of a specified number by another specified number.
		/// </summary>
		/// <param name="x">A dividend.</param>
		/// <param name="y">A divisor.</param>
		/// <returns>A number equal to x - (y * Q), where Q is the quotient of x / y rounded to the nearest integer.</returns>
		public static Primitive IEEERemainder(Primitive x, Primitive y)
		{
			return System.Math.IEEERemainder((double)x, (double)y);
		}

		/// <summary>
		/// Returns the natural (base e) logarithm of a specified number.
		/// </summary>
		/// <param name="d">A number whose logarithm is to be found.</param>
		/// <returns>The natural (base e) logarithm of the specified number.</returns>
		public static Primitive Log(Primitive d)
		{
			return System.Math.Log((double)d);
		}

		/// <summary>
		/// Returns the base 10 logarithm of a specified number.
		/// </summary>
		/// <param name="d">A number whose logarithm is to be found.</param>
		/// <returns>The base 10 logarithm of the specified number.</returns>
		public static Primitive Log10(Primitive d)
		{
			return System.Math.Log10((double)d);
		}

		/// <summary>
		/// Returns the larger of two specified numbers.
		/// </summary>
		/// <param name="val1">The first of two numbers to compare.</param>
		/// <param name="val2">The second of two numbers to compare.</param>
		/// <returns>The larger of the two numbers.</returns>
		public static Primitive Max(Primitive val1, Primitive val2)
		{
			return System.Math.Max((double)val1, (double)val2);
		}

		/// <summary>
		/// Returns the smaller of two specified numbers.
		/// </summary>
		/// <param name="val1">The first of two numbers to compare.</param>
		/// <param name="val2">The second of two numbers to compare.</param>
		/// <returns>The smaller of the two numbers.</returns>
		public static Primitive Min(Primitive val1, Primitive val2)
		{
			return System.Math.Min((double)val1, (double)val2);
		}

		/// <summary>
		/// Returns a specified number raised to the specified power.
		/// </summary>
		/// <param name="x">A double-precision floating-point number to be raised to a power.</param>
		/// <param name="y">A double-precision floating-point number that specifies a power.</param>
		/// <returns>The number x raised to the power y.</returns>
		public static Primitive Pow(Primitive x, Primitive y)
		{
			return System.Math.Pow((double)x, (double)y);
		}

		/// <summary>
		/// Rounds a specified number to the nearest integer.
		/// </summary>
		/// <param name="a">A number to be rounded.</param>
		/// <returns>The integer nearest to the specified number.</returns>
		public static Primitive Round(Primitive a)
		{
			return System.Math.Round((double)a);
		}

		/// <summary>
		/// Returns a value indicating the sign of a number.
		/// </summary>
		/// <param name="value">A signed number.</param>
		/// <returns>A number indicating the sign of the specified number.</returns>
		public static Primitive Sign(Primitive value)
		{
			return System.Math.Sign((double)value);
		}

		/// <summary>
		/// Returns the sine of the specified angle.
		/// </summary>
		/// <param name="a">An angle, measured in radians.</param>
		/// <returns>The sine of the specified angle.</returns>
		public static Primitive Sin(Primitive a)
		{
			return System.Math.Sin((double)a);
		}

		/// <summary>
		/// Returns the hyperbolic sine of the specified angle.
		/// </summary>
		/// <param name="value">An angle, measured in radians.</param>
		/// <returns>The hyperbolic sine of the specified angle.</returns>
		public static Primitive Sinh(Primitive value)
		{
			return System.Math.Sinh((double)value);
		}

		/// <summary>
		/// Returns the square root of a specified number.
		/// </summary>
		/// <param name="d">A number.</param>
		/// <returns>The square root of the specified number.</returns>
		public static Primitive Sqrt(Primitive d)
		{
			return System.Math.Sqrt((double)d);
		}

		/// <summary>
		/// Returns the tangent of the specified angle.
		/// </summary>
		/// <param name="a">An angle, measured in radians.</param>
		/// <returns>The tangent of the specified angle.</returns>
		public static Primitive Tan(Primitive a)
		{
			return System.Math.Tan((double)a);
		}

		/// <summary>
		/// Returns the hyperbolic tangent of the specified angle.
		/// </summary>
		/// <param name="value">An angle, measured in radians.</param>
		/// <returns>The hyperbolic tangent of the specified angle.</returns>
		public static Primitive Tanh(Primitive value)
		{
			return System.Math.Tanh((double)value);
		}

		/// <summary>
		/// Calculates the integral part of a specified number.
		/// </summary>
		/// <param name="d">A number to truncate.</param>
		/// <returns>The integral part of the specified number.</returns>
		public static Primitive Truncate(Primitive d)
		{
			return System.Math.Truncate((double)d);
		}

		/// <summary>
		/// Represents the natural logarithmic base, specified by the constant, e.
		/// </summary>
		public static Primitive E {
			get {
				return System.Math.E;
			}
                     
		}
        
        
        
        
        
		/// <summary>
		/// Represents the ratio of the circumference of a circle to its diameter, specified by the constant, PI.
		/// </summary>
		public static Primitive PI {
			get {
				return System.Math.PI;
        	  
			}
                     
		}
	}
}