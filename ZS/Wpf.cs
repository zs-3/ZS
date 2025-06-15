using Microsoft.SmallBasic.Library;
using System;
using System.Reflection;
using System.IO;
using System.Windows.Markup;
using System.Linq;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Windows.Forms.Integration;
using System.Collections;
using System.Windows;
using System.Windows.Shapes;
using Microsoft.SmallBasic.Library.Internal;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Data;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using System.Windows.Threading;
using ZS;

namespace ZS
{
	//-------------------------------------ZS--------------------------------------------------------
	
	/// <summary>
	/// Wpf And GW.
	/// </summary>
	[SmallBasicType]
	public static class ZSWpf
	{


        
		private static Type GraphicsWindowType = typeof(GraphicsWindow);
		private static Dictionary<string, UIElement> _objectsMap = (Dictionary<string, UIElement>)GraphicsWindowType.GetField("_objectsMap", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.IgnoreCase).GetValue(null);
		private static UIElement obj;
		
		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
		
		[DllImport("user32.dll", SetLastError = true)]
		private static extern IntPtr SetParent(IntPtr child, IntPtr newParent);

		[DllImport("user32.dll")]
		private static extern IntPtr SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
		
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr GetClassLongPtr(IntPtr hWnd, int nIndex);

		

		
		[HideFromIntellisense]
		public static Window GetWindowFromHwnd(IntPtr hwnd)
		{
			HwndSource hwndSource = HwndSource.FromHwnd(hwnd);

			if (hwndSource != null) {
				return hwndSource.RootVisual as Window;
			} else {
				return null;
			}
		}

		[HideFromIntellisense]
		public static Window Verify()
		{
			IntPtr _hWnd = FindWindow(null, GraphicsWindow.Title);
			return GetWindowFromHwnd(_hWnd);
		}
		
		[HideFromIntellisense]
		public static IntPtr GWHandle()
		{
			return FindWindow(null, GraphicsWindow.Title);
		}

		[HideFromIntellisense]
		public static Canvas GetGWCanvas()
		{
			return (Canvas)ZSReflection.GetFieldValue(typeof(GraphicsWindow), "_mainCanvas");
		}
        
		[HideFromIntellisense]
		public static void AddControlGW(string name, Control control)
		{
			ZSReflection.InvokeMethod(typeof(GraphicsWindow), "AddControl", name, control);
		}
		
		[HideFromIntellisense]
		public static UIElement GetControl(string Name)
		{
			UIElement uielement;
			if (!_objectsMap.TryGetValue(Name, out uielement)) {
				return uielement;
			}
			return uielement;
		}
		
		
		/// <summary>
		/// Changes the value of the internal static field '_windowVisible' in the GraphicsWindow class.
		/// </summary>
		/// <param name="value">The value to set (true or false).</param>
		[HideFromIntellisense]
		private static void SetWindowVisible(bool value)
		{
			// Step 1: Get the type that contains the internal static field
			Type graphicsWindowType = typeof(GraphicsWindow); // Adjust this to the actual class name

			// Step 2: Access the field using reflection
			FieldInfo windowVisibleField = graphicsWindowType.GetField("_windowVisible",
				                               BindingFlags.NonPublic | BindingFlags.Static);

			if (windowVisibleField != null) {
				// Step 3: Set the field's value to the passed 'value' parameter
				windowVisibleField.SetValue(null, value); // `null` because it's a static field
			}
		}
		
		[HideFromIntellisense]
		public static void SetGWCanvas(Canvas canvas)
		{
			Window win = Verify();
			win.Dispatcher.Invoke(() => {
				ZSReflection.SetFieldValue(typeof(GraphicsWindow), "_mainCanvas", canvas);
			});
		}
		
		[HideFromIntellisense]
		public static void SetGWWindow(Window Win)
		{
			
			ZSReflection.SetFieldValue(typeof(GraphicsWindow), "_window", Win);
		}
		
		[HideFromIntellisense]
		public static string GenerateNewName(string prefix)
		{
			return (string)ZSReflection.InvokeMethod(typeof(Shapes), "GenerateNewName", prefix);
		}
		
		/// <summary>
		/// Loads a style from a specified XAML file path and style name.
		/// </summary>
		/// <param name="xamlFilePath">The file path of the XAML file.</param>
		/// <param name="styleName">The name of the style to retrieve.</param>
		/// <returns>The style if found, otherwise null.</returns>
		[HideFromIntellisense]
		public static Style GetStyleFromXaml(string xamlFilePath, string styleName)
		{
			// Check if the file exists
			if (!System.IO.File.Exists(xamlFilePath)) {
				return null; // Return null if the file does not exist
			}

			// Load the XAML file
			using (FileStream fs = new FileStream(xamlFilePath, FileMode.Open, FileAccess.Read)) {
				// Parse the XAML file into a ResourceDictionary
				ResourceDictionary resourceDictionary = (ResourceDictionary)XamlReader.Load(fs);

				// Return the style if found, otherwise return null
				return resourceDictionary.Contains(styleName) ? resourceDictionary[styleName] as Style : null;
			}
		}
        
		/// <summary>
		/// Sets the Title for the Graphic Window.
		/// </summary>
		/// <param name="Title">The Title</param>
		public static void Title(Primitive Title)
		{
			
			Window _win = Verify();
			if (_win != null) {
				_win.Dispatcher.Invoke(() => {
					_win.Title = Title;
				});
			}
		}

		
       
		
		
		/// <summary>
		/// Sets a style from a XAML file to a shape or control.
		/// </summary>
		/// <param name="shapeName">The control or shape name.</param>
		/// <param name="xaml">The XAML file name.</param>
		/// <param name="style">The style name.</param>
		public static void SetStyle(Primitive shapeName, Primitive xaml, Primitive style)
		{
			Window _win = Verify();
			_win.Dispatcher.Invoke(() => {
				UIElement ui = GetControl(shapeName);

				if (ui is Control) {
					Control control = (Control)ui;
					control.Style = GetStyleFromXaml(xaml, style);
				} else if (ui is Shape) {
					Shape shape = (Shape)ui;
					shape.Style = GetStyleFromXaml(xaml, style);
				}
			});
		}
		
		

		
        
	}
}
