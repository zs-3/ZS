using Microsoft.SmallBasic.Library;
using System;
using ZS;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;

namespace ZS
{
	/// <summary>
	/// Manage Multiple Canvas And Functions.
	/// Name For Small Basic Default Canvas Is "GWCanvas"
	/// </summary>
	[SmallBasicType]
	public static class ZSCanvas
	{
		
		private static Dictionary<string, Canvas> _Canvas = new Dictionary<string, Canvas>();
		
		static ZSCanvas()
		{
			Canvas GWCanvas = ZSWpf.GetGWCanvas();
			_Canvas.Add("GWCanvas", GWCanvas);
		}
		
		/// <summary>
		/// Create A New Canvas.
		/// </summary>
		/// <param name="Name">The New Canvas Name.</param>
		public static void CreateCanvas(Primitive Name)
		{
			Window win = ZSWpf.Verify();
			win.Dispatcher.Invoke(() => {
				ZSReflection.InvokeMethod(typeof(GraphicsWindow), "SetWindowContent", null);
				_Canvas.Add(Name, ZSWpf.GetGWCanvas());
			});
		}
		
		/// <summary>
		/// Set A Canvas to GW.
		/// </summary>
		/// <param name="Name"></param>
		public static void SetCanvas(Primitive Name)
		{
			Canvas can = _Canvas[Name];
			ZSWpf.SetGWCanvas(can);
		}
		
		/// <summary>
		/// Hides a Canvas on the GraphicsWindow.
		/// </summary>
		/// <param name="Name"></param>
		public static void HideCanvas(Primitive Name)
		{
			string canvasName = Name.ToString(); // Convert Primitive to string

			// Check if the _Canvas dictionary is initialized
			if (_Canvas == null) {
				throw new Exception("_Canvas dictionary is not initialized.");
			}

			// Check if the canvas exists in the dictionary
			if (_Canvas.ContainsKey(canvasName)) {
				Canvas can = _Canvas[canvasName];
        
				// Check if the canvas itself is null
				if (can != null) {
					can.Dispatcher.Invoke(() => {
						can.Visibility = Visibility.Hidden;
					});
				} else {
					throw new Exception("Canvas with name " + canvasName + " is null.");
				}
			} else {
				throw new Exception("Canvas with name " + canvasName + " not found in _Canvas dictionary.");
			}
		}


		
		/// <summary>
		/// Shows A Canvas On GW.
		/// </summary>
		/// <param name="Name"></param>
		public static void ShowCanvas(Primitive Name)
		{
			Canvas can = _Canvas[Name];
			can.Visibility = Visibility.Visible;
		}
	}
}
	