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
	/// <summary>
	/// Extended Controls For Graphics Window.
	/// </summary>
	[SmallBasicType]
	public static class ZSControl
	{
		private static Assembly entryAssembly = Assembly.GetEntryAssembly();
		private static Type mainModule = entryAssembly.EntryPoint.DeclaringType;
		private static Dictionary<string, MethodInfo> _Subs = new Dictionary<string, MethodInfo>();
		
		private static Control GetControl(string Shape)
		{
			UIElement ui = ZSWpf.GetControl(Shape);
			Control control = (Control)ui;
			return control;
		}
		
		private static Shape GetShape(string Shape)
		{
			UIElement ui = ZSWpf.GetControl(Shape);
			Shape shape = (Shape)ui;
			return shape;
		}
		
		private static System.Delegate CreateDeglateForSub(string Sub)
		{
			return (SmallBasicCallback)System.Delegate.CreateDelegate(typeof(SmallBasicCallback), mainModule.GetMethod(Sub, BindingFlags.Static | BindingFlags.NonPublic));
		}
		
		[HideFromIntellisense]
		public static FrameworkElement GetCtrl(Primitive Shape)
		{
			try {
				UIElement shape = ZSWpf.GetControl(Shape);
				FrameworkElement fe = shape as FrameworkElement;
				return fe;
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
			}
			return null;
		}
		
		[HideFromIntellisense]
		public static MethodInfo GetSub(string Sub)
		{
			return mainModule.GetMethod(Sub, BindingFlags.IgnoreCase | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
		}
		
		//-----------------------------Events-----------------------------------------------------------
		private static void KeyDown(object sender, RoutedEventArgs e)
		{
			FrameworkElement fe = sender as FrameworkElement;
			_Subs[fe.Name].Invoke(null, null);
		}
		
		
		/// <summary>
		/// Add Key Down Event To Shape.
		/// </summary>
		/// <param name="Shape">The Shape Name</param>
		/// <param name="Sub">The Sub Name.</param>
		public static void KeyDown(Primitive Shape, Primitive Sub)
		{
			try {
				Window win = ZSWpf.Verify();
				win.Dispatcher.Invoke(() => {
					FrameworkElement fe = GetCtrl(Shape);
					_Subs[fe.Name] = GetSub(Sub);
					fe.KeyDown += KeyDown;
				});
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
			}
		}
		
	}
}