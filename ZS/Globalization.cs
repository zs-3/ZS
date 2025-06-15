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
using System.Collections.Generic;
using System.Net;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using Microsoft.Win32;
using Microsoft.VisualBasic;




namespace ZS
{
	/// <summary>
	/// Provides methods and properties for working with globalization, including culture information, date and time formatting, number formatting, and text information.
	/// This class also includes support for various calendars such as Gregorian, Hijri, Chinese, and Korean.
	/// </summary>
	[SmallBasicType]
	public static class ZSGlobalization
	{
		private static CultureInfo cultureInfo = CultureInfo.CurrentCulture;

		/// <summary>
		/// Gets or sets the current culture. Example: "en-US"
		/// </summary>
		/// <example>
		/// Get the current culture: <br />
		/// <code>currentCulture = ZSGlobalization.CurrentCulture</code><br />
		/// Set the current culture to French (France): <br />
		/// <code>ZSGlobalization.CurrentCulture = "fr-FR"</code>
		/// </example>
		public static Primitive CurrentCulture {
			get { return cultureInfo.Name; }
			set { cultureInfo = new CultureInfo((string)value); }
		}

		/// <summary>
		/// Gets the name of the calendar used by the current culture. Example: "GregorianCalendar"
		/// </summary>
		public static Primitive Calendar {
			get { return cultureInfo.Calendar.ToString(); }
		}

		/// <summary>
		/// Gets or sets the date and time pattern for short dates. Example: "MM/dd/yyyy"
		/// </summary>
		/// <example>
		/// Get the short date pattern: <br />
		/// <code>shortDatePattern = ZSGlobalization.ShortDatePattern</code><br />
		/// Set the short date pattern to day/month/year: <br />
		/// <code>ZSGlobalization.ShortDatePattern = "dd/MM/yyyy"</code>
		/// </example>
		public static Primitive ShortDatePattern {
			get { return cultureInfo.DateTimeFormat.ShortDatePattern; }
			set { cultureInfo.DateTimeFormat.ShortDatePattern = (string)value; }
		}

		/// <summary>
		/// Gets or sets the date and time pattern for long dates. Example: "dddd, MMMM dd, yyyy"
		/// </summary>
		/// <example>
		/// Get the long date pattern: <br />
		/// <code>longDatePattern = ZSGlobalization.LongDatePattern</code><br />
		/// Set the long date pattern to day of week, day month year: <br />
		/// <code>ZSGlobalization.LongDatePattern = "dddd, dd MMMM yyyy"</code>
		/// </example>
		public static Primitive LongDatePattern {
			get { return cultureInfo.DateTimeFormat.LongDatePattern; }
			set { cultureInfo.DateTimeFormat.LongDatePattern = (string)value; }
		}

		/// <summary>
		/// Gets or sets the number decimal separator. Example: "."
		/// </summary>
		/// <example>
		/// Get the number decimal separator: <br />
		/// <code>decimalSeparator = ZSGlobalization.NumberDecimalSeparator</code><br />
		/// Set the number decimal separator to a comma: <br />
		/// <code>ZSGlobalization.NumberDecimalSeparator = ","</code>
		/// </example>
		public static Primitive NumberDecimalSeparator {
			get { return cultureInfo.NumberFormat.NumberDecimalSeparator; }
			set { cultureInfo.NumberFormat.NumberDecimalSeparator = (string)value; }
		}

		/// <summary>
		/// Gets or sets the currency symbol. Example: "$"
		/// </summary>
		/// <example>
		/// Get the currency symbol: <br />
		/// <code>currencySymbol = ZSGlobalization.CurrencySymbol</code><br />
		/// Set the currency symbol to Euro: <br />
		/// <code>ZSGlobalization.CurrencySymbol = "€"</code>
		/// </example>
		public static Primitive CurrencySymbol {
			get { return cultureInfo.NumberFormat.CurrencySymbol; }
			set { cultureInfo.NumberFormat.CurrencySymbol = (string)value; }
		}


		/// <summary>
		/// Gets the text information (casing) of the current culture. Example: "Invariant"
		/// </summary>
		/// <example>
		/// Get the text info: <br />
		/// <code>textInfo = ZSGlobalization.TextInfo</code>
		/// </example>
		public static Primitive TextInfo {
			get { return cultureInfo.TextInfo.ToString(); }
		}

		/// <summary>
		/// Sets the text information (casing) of the culture. Example: "tr-TR"
		/// </summary>
		/// <example>
		/// Set the text info to a new culture: <br />
		/// <code>ZSGlobalization.SetTextInfo("tr-TR")</code>
		/// </example>
		/// <param name="cultureName">The name of the culture whose text info is to be used.</param>
		//public static void SetTextInfo(Primitive cultureName)
		//{
		//  cultureInfo = new CultureInfo(cultureInfo.Name)
		//  {
		//      TextInfo = new CultureInfo((string)cultureName).TextInfo
		//  };
		//}

		/// <summary>
		/// Gets the ISO 639-1 two-letter code for the language of the current culture. Example: "en"
		/// </summary>
		public static Primitive TwoLetterISOLanguageName {
			get { return cultureInfo.TwoLetterISOLanguageName; }
		}

		/// <summary>
		/// Gets the ISO 639-2 three-letter code for the language of the current culture. Example: "eng"
		/// </summary>
		public static Primitive ThreeLetterISOLanguageName {
			get { return cultureInfo.ThreeLetterISOLanguageName; }
		}

		/// <summary>
		/// Gets the Windows three-letter code for the language of the current culture. Example: "ENU"
		/// </summary>
		public static Primitive ThreeLetterWindowsLanguageName {
			get { return cultureInfo.ThreeLetterWindowsLanguageName; }
		}

		/// <summary>
		/// Gets the native name of the language of the current culture. Example: "English"
		/// </summary>
		public static Primitive NativeName {
			get { return cultureInfo.NativeName; }
		}

		// Additional calendar support

		/// <summary>
		/// Gets the current date in the Hijri calendar.
		/// </summary>
		public static Primitive HijriDate {
			get {
				HijriCalendar hijriCalendar = new HijriCalendar();
				DateTime dateTime = DateTime.Now;
				return hijriCalendar.GetYear(dateTime) + "/" + hijriCalendar.GetMonth(dateTime) + "/" + hijriCalendar.GetDayOfMonth(dateTime);
			}
		}

		/// <summary>
		/// Gets the current date in the Chinese calendar.
		/// </summary>
		public static Primitive ChineseDate {
			get {
				ChineseLunisolarCalendar chineseCalendar = new ChineseLunisolarCalendar();
				DateTime dateTime = DateTime.Now;
				return chineseCalendar.GetYear(dateTime) + "/" + chineseCalendar.GetMonth(dateTime) + "/" + chineseCalendar.GetDayOfMonth(dateTime);
			}
		}

		/// <summary>
		/// Gets the current date in the Gregorian calendar.
		/// </summary>
		public static Primitive GregorianDate {
			get {
				GregorianCalendar gregorianCalendar = new GregorianCalendar();
				DateTime dateTime = DateTime.Now;
				return gregorianCalendar.GetYear(dateTime) + "/" + gregorianCalendar.GetMonth(dateTime) + "/" + gregorianCalendar.GetDayOfMonth(dateTime);
			}
		}

		/// <summary>
		/// Gets the current date in the Korean calendar.
		/// </summary>
		public static Primitive KoreanDate {
			get {
				KoreanCalendar koreanCalendar = new KoreanCalendar();
				DateTime dateTime = DateTime.Now;
				return koreanCalendar.GetYear(dateTime) + "/" + koreanCalendar.GetMonth(dateTime) + "/" + koreanCalendar.GetDayOfMonth(dateTime);
			}
		}
		
		/// <summary>
		/// Get All Cultures.
		/// </summary>
		/// <returns>The Array of Cultures Name.</returns>
		public static Primitive GetCultures()
		{
			Primitive cultures = new Primitive();
			Primitive i = 0;
			foreach (CultureInfo culture in CultureInfo.GetCultures(CultureTypes.AllCultures)) {
				++i;
				cultures[i] = culture.Name;
			}
			return cultures;
		}
	}
	
		
	
	
}
