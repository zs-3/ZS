using Microsoft.SmallBasic.Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace ZS
{
	/// <summary>
	/// Independent multi-window WPF UI manager for Small Basic.
	/// Enables creating, styling, and managing multiple standalone WPF windows seamlessly.
	/// </summary>
	[SmallBasicType]
	public static class ZSUI
	{
		private static Dictionary<string, Window> _windows = new Dictionary<string, Window>(StringComparer.OrdinalIgnoreCase);
		private static Dictionary<string, Canvas> _canvases = new Dictionary<string, Canvas>(StringComparer.OrdinalIgnoreCase);
		private static Dictionary<string, UIElement> _namedControls = new Dictionary<string, UIElement>(StringComparer.OrdinalIgnoreCase);
		private static string _activeWindowName = "";
		private static int _windowCounter = 0;
		private static int _controlCounter = 0;

		static ZSUI()
		{
			EnsureApplication();
		}

		private static void EnsureApplication()
		{
			if (Application.Current == null)
			{
				Thread appThread = new Thread(() =>
				{
					if (Application.Current == null)
					{
						new Application { ShutdownMode = ShutdownMode.OnExplicitShutdown };
					}
					System.Windows.Threading.Dispatcher.Run();
				});
				appThread.SetApartmentState(ApartmentState.STA);
				appThread.IsBackground = true;
				appThread.Start();

				while (Application.Current == null)
				{
					Thread.Sleep(20);
				}
			}
		}

		private static void InvokeOnUI(Action action)
		{
			EnsureApplication();
			if (Application.Current != null && Application.Current.Dispatcher != null)
			{
				if (Application.Current.Dispatcher.CheckAccess())
				{
					action();
				}
				else
				{
					Application.Current.Dispatcher.Invoke(action);
				}
			}
		}

		private static Window GetActiveWindow()
		{
			if (!string.IsNullOrEmpty(_activeWindowName) && _windows.ContainsKey(_activeWindowName))
			{
				return _windows[_activeWindowName];
			}
			return null;
		}

		private static Canvas GetActiveCanvas()
		{
			if (!string.IsNullOrEmpty(_activeWindowName) && _canvases.ContainsKey(_activeWindowName))
			{
				return _canvases[_activeWindowName];
			}
			return null;
		}

		[HideFromIntellisense]
		public static UIElement GetControlByName(string name)
		{
			if (_namedControls.ContainsKey(name))
			{
				return _namedControls[name];
			}
			return null;
		}

		/// <summary>
		/// Creates a new WPF window and sets it as the active window.
		/// </summary>
		/// <param name="title">Title of the window.</param>
		/// <param name="width">Width of the window.</param>
		/// <param name="height">Height of the window.</param>
		/// <returns>Unique window handle/identifier name.</returns>
		public static Primitive CreateWindow(Primitive title, Primitive width, Primitive height)
		{
			string winName = "ZSWindow_" + (++_windowCounter);
			InvokeOnUI(() =>
			{
				Window win = new Window
				{
					Title = title.ToString(),
					Width = (double)width > 0 ? (double)width : 800,
					Height = (double)height > 0 ? (double)height : 600,
					WindowStartupLocation = WindowStartupLocation.CenterScreen
				};

				Canvas mainCanvas = new Canvas
				{
					Background = Brushes.White,
					ClipToBounds = true
				};

				win.Content = mainCanvas;

				win.Closed += (s, e) =>
				{
					_windows.Remove(winName);
					_canvases.Remove(winName);
					if (_activeWindowName == winName)
					{
						_activeWindowName = "";
					}
				};

				_windows[winName] = win;
				_canvases[winName] = mainCanvas;
				_activeWindowName = winName;
			});

			return winName;
		}

		/// <summary>
		/// Sets the target window to perform subsequent UI operations on.
		/// </summary>
		/// <param name="windowName">The window handle/identifier returned by CreateWindow.</param>
		public static void SetActiveWindow(Primitive windowName)
		{
			string name = windowName.ToString();
			if (_windows.ContainsKey(name))
			{
				_activeWindowName = name;
			}
		}

		/// <summary>
		/// Returns the identifier of the currently active window.
		/// </summary>
		/// <returns>Active window identifier.</returns>
		public static Primitive GetActiveWindowName()
		{
			return _activeWindowName;
		}

		/// <summary>
		/// Shows the active window (or specified window name).
		/// </summary>
		/// <param name="windowName">Window name or empty to show active window.</param>
		public static void Show(Primitive windowName)
		{
			InvokeOnUI(() =>
			{
				string name = string.IsNullOrEmpty(windowName.ToString()) ? _activeWindowName : windowName.ToString();
				if (_windows.ContainsKey(name))
				{
					_windows[name].Show();
					_windows[name].Activate();
				}
			});
		}

		/// <summary>
		/// Hides the active window (or specified window name).
		/// </summary>
		/// <param name="windowName">Window name or empty for active window.</param>
		public static void Hide(Primitive windowName)
		{
			InvokeOnUI(() =>
			{
				string name = string.IsNullOrEmpty(windowName.ToString()) ? _activeWindowName : windowName.ToString();
				if (_windows.ContainsKey(name))
				{
					_windows[name].Hide();
				}
			});
		}

		/// <summary>
		/// Closes the specified window or active window.
		/// </summary>
		/// <param name="windowName">Window name or empty for active window.</param>
		public static void Close(Primitive windowName)
		{
			InvokeOnUI(() =>
			{
				string name = string.IsNullOrEmpty(windowName.ToString()) ? _activeWindowName : windowName.ToString();
				if (_windows.ContainsKey(name))
				{
					_windows[name].Close();
				}
			});
		}

		/// <summary>
		/// Sets the title of the active window.
		/// </summary>
		public static void SetTitle(Primitive title)
		{
			InvokeOnUI(() =>
			{
				Window win = GetActiveWindow();
				if (win != null) win.Title = title.ToString();
			});
		}

		/// <summary>
		/// Sets width and height of the active window.
		/// </summary>
		public static void SetSize(Primitive width, Primitive height)
		{
			InvokeOnUI(() =>
			{
				Window win = GetActiveWindow();
				if (win != null)
				{
					win.Width = (double)width;
					win.Height = (double)height;
				}
			});
		}

		/// <summary>
		/// Sets screen position (left, top) of active window.
		/// </summary>
		public static void SetPosition(Primitive left, Primitive top)
		{
			InvokeOnUI(() =>
			{
				Window win = GetActiveWindow();
				if (win != null)
				{
					win.WindowStartupLocation = WindowStartupLocation.Manual;
					win.Left = (double)left;
					win.Top = (double)top;
				}
			});
		}

		/// <summary>
		/// Sets background color or brush of active window canvas.
		/// </summary>
		/// <param name="color">Color string (e.g. "Red", "#FF3366", "LightBlue").</param>
		public static void SetBackground(Primitive color)
		{
			InvokeOnUI(() =>
			{
				Canvas canvas = GetActiveCanvas();
				if (canvas != null)
				{
					try
					{
						BrushConverter bc = new BrushConverter();
						Brush brush = (Brush)bc.ConvertFromString(color.ToString());
						canvas.Background = brush;
					}
					catch
					{
						canvas.Background = Brushes.White;
					}
				}
			});
		}

		/// <summary>
		/// Parses a XAML string or file path and loads it as content into the active window.
		/// </summary>
		/// <param name="xamlSource">Raw XAML code string or file path to .xaml file.</param>
		public static void LoadXaml(Primitive xamlSource)
		{
			InvokeOnUI(() =>
			{
				Window win = GetActiveWindow();
				if (win == null) return;

				try
				{
					string src = xamlSource.ToString().Trim();
					object loadedObj = null;

					if (File.Exists(src))
					{
						using (FileStream fs = new FileStream(src, FileMode.Open, FileAccess.Read))
						{
							loadedObj = XamlReader.Load(fs);
						}
					}
					else if (src.StartsWith("<"))
					{
						loadedObj = XamlReader.Parse(src);
					}

					if (loadedObj is UIElement element)
					{
						if (element is Window childWin)
						{
							win.Title = childWin.Title;
							win.Width = childWin.Width;
							win.Height = childWin.Height;
							win.Content = childWin.Content;
						}
						else
						{
							win.Content = element;
						}
					}
				}
				catch (Exception ex)
				{
					ZSUtilities.OnError(ex);
				}
			});
		}

		/// <summary>
		/// Dynamically adds a WPF control type (Button, TextBox, Label, Slider, CheckBox, ComboBox, etc.) to the active window.
		/// </summary>
		/// <param name="controlType">WPF control type name (e.g., "Button", "TextBox", "Slider", "Label").</param>
		/// <param name="text">Text/Content of control.</param>
		/// <param name="left">X coordinate.</param>
		/// <param name="top">Y coordinate.</param>
		/// <param name="width">Width of control.</param>
		/// <param name="height">Height of control.</param>
		/// <returns>Control name identifier.</returns>
		public static Primitive AddControl(Primitive controlType, Primitive text, Primitive left, Primitive top, Primitive width, Primitive height)
		{
			string ctrlName = "ZSControl_" + (++_controlCounter);
			InvokeOnUI(() =>
			{
				Canvas canvas = GetActiveCanvas();
				if (canvas == null) return;

				string typeName = controlType.ToString().ToLower();
				FrameworkElement elem = null;

				switch (typeName)
				{
					case "button":
						elem = new Button { Content = text.ToString() };
						break;
					case "textbox":
						elem = new TextBox { Text = text.ToString() };
						break;
					case "label":
					case "textblock":
						elem = new Label { Content = text.ToString() };
						break;
					case "checkbox":
						elem = new CheckBox { Content = text.ToString() };
						break;
					case "slider":
						elem = new Slider { Minimum = 0, Maximum = 100, Value = 50 };
						break;
					case "progressbar":
						elem = new ProgressBar { Minimum = 0, Maximum = 100, Value = 50 };
						break;
					case "combobox":
						elem = new ComboBox();
						break;
					default:
						elem = new Button { Content = text.ToString() };
						break;
				}

				elem.Name = ctrlName;
				elem.Width = (double)width > 0 ? (double)width : 120;
				elem.Height = (double)height > 0 ? (double)height : 30;

				Canvas.SetLeft(elem, (double)left);
				Canvas.SetTop(elem, (double)top);

				canvas.Children.Add(elem);
				_namedControls[ctrlName] = elem;
			});

			return ctrlName;
		}

		/// <summary>
		/// Sets a property on any WPF control by control name using reflection.
		/// </summary>
		/// <param name="controlName">Name of control returned by AddControl.</param>
		/// <param name="propertyName">Property name (e.g., "Content", "Text", "FontSize", "IsEnabled", "Visibility").</param>
		/// <param name="value">Property value.</param>
		public static void SetControlProperty(Primitive controlName, Primitive propertyName, Primitive value)
		{
			InvokeOnUI(() =>
			{
				UIElement ctrl = GetControlByName(controlName.ToString());
				if (ctrl == null) return;

				try
				{
					PropertyInfo prop = ctrl.GetType().GetProperty(propertyName.ToString(), BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
					if (prop != null && prop.CanWrite)
					{
						object convertedVal;
						string valStr = value.ToString();
						if (prop.PropertyType.IsEnum)
						{
							convertedVal = Enum.Parse(prop.PropertyType, valStr, true);
						}
						else
						{
							convertedVal = Convert.ChangeType(valStr, prop.PropertyType);
						}
						prop.SetValue(ctrl, convertedVal, null);
					}
				}
				catch (Exception ex)
				{
					ZSUtilities.OnError(ex);
				}
			});
		}

		/// <summary>
		/// Gets a property value from a WPF control by control name.
		/// </summary>
		/// <param name="controlName">Control identifier name.</param>
		/// <param name="propertyName">Property name (e.g. "Text", "Content", "Value").</param>
		/// <returns>Property value as string.</returns>
		public static Primitive GetControlProperty(Primitive controlName, Primitive propertyName)
		{
			string result = "";
			InvokeOnUI(() =>
			{
				UIElement ctrl = GetControlByName(controlName.ToString());
				if (ctrl == null) return;

				try
				{
					PropertyInfo prop = ctrl.GetType().GetProperty(propertyName.ToString(), BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
					if (prop != null && prop.CanRead)
					{
						object val = prop.GetValue(ctrl, null);
						result = val != null ? val.ToString() : "";
					}
				}
				catch (Exception ex)
				{
					ZSUtilities.OnError(ex);
				}
			});
			return result;
		}
	}
}
