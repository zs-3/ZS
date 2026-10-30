using Microsoft.SmallBasic.Library;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ZS
{
	/// <summary>
	/// Comprehensive, independent WPF UI framework for Small Basic.
	/// Enables creation, styling, event-hooking, layout container nesting, and dynamic management of multiple WPF windows and controls.
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
			try
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
			catch (Exception ex)
			{
				ZSUtilities.OnError(ex);
			}
		}

		private static void InvokeOnUI(Action action)
		{
			try
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
						Application.Current.Dispatcher.Invoke(() =>
						{
							try
							{
								action();
							}
							catch (Exception ex)
							{
								ZSUtilities.OnError(ex);
							}
						});
					}
				}
			}
			catch (Exception ex)
			{
				ZSUtilities.OnError(ex);
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

		private static Brush ParseBrush(string colorStr)
		{
			if (string.IsNullOrEmpty(colorStr)) return Brushes.Transparent;
			try
			{
				return (Brush)new BrushConverter().ConvertFromString(colorStr);
			}
			catch
			{
				return Brushes.Black;
			}
		}

		private static void RemoveFromParent(UIElement element)
		{
			if (element == null) return;
			DependencyObject parent = LogicalTreeHelper.GetParent(element);
			if (parent is Panel panel)
			{
				panel.Children.Remove(element);
			}
			else if (parent is ContentControl contentControl)
			{
				if (contentControl.Content == element)
				{
					contentControl.Content = null;
				}
			}
			else if (parent is Border border)
			{
				if (border.Child == element)
				{
					border.Child = null;
				}
			}
			else if (parent is ItemsControl itemsControl)
			{
				itemsControl.Items.Remove(element);
			}
		}

		// =========================================================================
		// WINDOW MANAGEMENT
		// =========================================================================

		/// <summary>
		/// Creates a new WPF window and sets it as the active window.
		/// </summary>
		/// <param name="title">Window title.</param>
		/// <param name="width">Window width.</param>
		/// <param name="height">Window height.</param>
		/// <returns>Unique window identifier name.</returns>
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
				_namedControls[winName] = win;
				_activeWindowName = winName;
			});

			return winName;
		}

		/// <summary>
		/// Sets the active window handle for subsequent operations.
		/// </summary>
		public static void SetActiveWindow(Primitive windowName)
		{
			string name = windowName.ToString();
			if (_windows.ContainsKey(name))
			{
				_activeWindowName = name;
			}
		}

		/// <summary>
		/// Returns the active window identifier name.
		/// </summary>
		public static Primitive GetActiveWindowName()
		{
			return _activeWindowName;
		}

		/// <summary>
		/// Shows the specified window or active window.
		/// </summary>
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
		/// Hides the specified window or active window.
		/// </summary>
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
		/// Sets window title.
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
		/// Sets window width and height.
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
		/// Sets window screen coordinates (left, top).
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
		/// Centers the active window on screen.
		/// </summary>
		public static void CenterWindow()
		{
			InvokeOnUI(() =>
			{
				Window win = GetActiveWindow();
				if (win != null)
				{
					win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
				}
			});
		}

		/// <summary>
		/// Sets WindowStyle (None, SingleBorderWindow, ThreeDBorderWindow, ToolWindow).
		/// </summary>
		public static void SetWindowStyle(Primitive style)
		{
			InvokeOnUI(() =>
			{
				Window win = GetActiveWindow();
				if (win != null && Enum.TryParse(style.ToString(), true, out WindowStyle ws))
				{
					win.WindowStyle = ws;
				}
			});
		}

		/// <summary>
		/// Sets WindowState (Normal, Minimized, Maximized).
		/// </summary>
		public static void SetWindowState(Primitive state)
		{
			InvokeOnUI(() =>
			{
				Window win = GetActiveWindow();
				if (win != null && Enum.TryParse(state.ToString(), true, out WindowState ws))
				{
					win.WindowState = ws;
				}
			});
		}

		/// <summary>
		/// Sets ResizeMode (NoResize, CanMinimize, CanResize, CanResizeWithGrip).
		/// </summary>
		public static void SetWindowResizeMode(Primitive resizeMode)
		{
			InvokeOnUI(() =>
			{
				Window win = GetActiveWindow();
				if (win != null && Enum.TryParse(resizeMode.ToString(), true, out ResizeMode rm))
				{
					win.ResizeMode = rm;
				}
			});
		}

		/// <summary>
		/// Sets Topmost state (True/False).
		/// </summary>
		public static void SetTopmost(Primitive isTopmost)
		{
			InvokeOnUI(() =>
			{
				Window win = GetActiveWindow();
				if (win != null)
				{
					win.Topmost = (bool)isTopmost;
				}
			});
		}

		/// <summary>
		/// Sets window background color.
		/// </summary>
		public static void SetBackground(Primitive color)
		{
			InvokeOnUI(() =>
			{
				Canvas canvas = GetActiveCanvas();
				if (canvas != null)
				{
					canvas.Background = ParseBrush(color.ToString());
				}
			});
		}

		/// <summary>
		/// Sets window icon from file path.
		/// </summary>
		public static void SetIcon(Primitive iconPath)
		{
			InvokeOnUI(() =>
			{
				Window win = GetActiveWindow();
				if (win != null && File.Exists(iconPath.ToString()))
				{
					win.Icon = BitmapFrame.Create(new Uri(iconPath.ToString(), UriKind.RelativeOrAbsolute));
				}
			});
		}

		// =========================================================================
		// CONTROL CREATION
		// =========================================================================

		/// <summary>
		/// Dynamically creates any WPF control type and places it on the active window canvas.
		/// Supported types: Button, TextBox, Label, TextBlock, CheckBox, RadioButton, Slider, ProgressBar, ComboBox, ListBox, Image, PasswordBox, ToggleButton, Border, Canvas, StackPanel, Grid, ScrollViewer, DatePicker, MediaElement, InkCanvas, RichTextBox, TabControl, TreeView, DataGrid, Menu, Expander, GroupBox, Frame.
		/// </summary>
		public static Primitive AddControl(Primitive controlType, Primitive textOrPath, Primitive left, Primitive top, Primitive width, Primitive height)
		{
			string ctrlName = "ZSControl_" + (++_controlCounter);
			InvokeOnUI(() =>
			{
				Canvas canvas = GetActiveCanvas();
				if (canvas == null) return;

				string type = controlType.ToString().ToLower().Trim();
				string val = textOrPath.ToString();
				FrameworkElement elem = null;

				switch (type)
				{
					case "button":
						elem = new Button { Content = val };
						break;
					case "textbox":
						elem = new TextBox { Text = val };
						break;
					case "label":
						elem = new Label { Content = val };
						break;
					case "textblock":
						elem = new TextBlock { Text = val };
						break;
					case "checkbox":
						elem = new CheckBox { Content = val };
						break;
					case "radiobutton":
						elem = new RadioButton { Content = val };
						break;
					case "togglebutton":
						elem = new ToggleButton { Content = val };
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
					case "listbox":
						elem = new ListBox();
						break;
					case "passwordbox":
						elem = new PasswordBox { Password = val };
						break;
					case "image":
						Image img = new Image();
						if (File.Exists(val))
						{
							img.Source = new BitmapImage(new Uri(val, UriKind.RelativeOrAbsolute));
						}
						elem = img;
						break;
					case "border":
						elem = new Border { Background = Brushes.Transparent, BorderBrush = Brushes.Gray, BorderThickness = new Thickness(1) };
						break;
					case "canvas":
						elem = new Canvas { Background = Brushes.Transparent };
						break;
					case "stackpanel":
						elem = new StackPanel();
						break;
					case "grid":
						elem = new Grid();
						break;
					case "scrollviewer":
						elem = new ScrollViewer();
						break;
					case "datepicker":
						elem = new DatePicker();
						break;
					case "mediaelement":
						MediaElement me = new MediaElement { LoadedBehavior = MediaState.Manual };
						if (File.Exists(val)) me.Source = new Uri(val, UriKind.RelativeOrAbsolute);
						elem = me;
						break;
					case "inkcanvas":
						elem = new InkCanvas();
						break;
					case "richtextbox":
						elem = new RichTextBox();
						break;
					case "tabcontrol":
						elem = new TabControl();
						break;
					case "treeview":
						elem = new TreeView();
						break;
					case "datagrid":
						elem = new DataGrid();
						break;
					case "menu":
						elem = new Menu();
						break;
					case "expander":
						elem = new Expander { Header = val };
						break;
					case "groupbox":
						elem = new GroupBox { Header = val };
						break;
					case "frame":
						elem = new Frame();
						break;
					default:
						elem = new Button { Content = val };
						break;
				}

				elem.Name = ctrlName;
				if ((double)width > 0) elem.Width = (double)width;
				if ((double)height > 0) elem.Height = (double)height;

				Canvas.SetLeft(elem, (double)left);
				Canvas.SetTop(elem, (double)top);

				canvas.Children.Add(elem);
				_namedControls[ctrlName] = elem;
			});

			return ctrlName;
		}

		// =========================================================================
		// PROPERTY & STYLING HELPERS
		// =========================================================================

		/// <summary>
		/// Sets control location on parent Canvas.
		/// </summary>
		public static void SetControlPosition(Primitive controlName, Primitive left, Primitive top)
		{
			InvokeOnUI(() =>
			{
				UIElement ctrl = GetControlByName(controlName.ToString());
				if (ctrl != null)
				{
					Canvas.SetLeft(ctrl, (double)left);
					Canvas.SetTop(ctrl, (double)top);
				}
			});
		}

		/// <summary>
		/// Sets control dimensions (width, height).
		/// </summary>
		public static void SetControlSize(Primitive controlName, Primitive width, Primitive height)
		{
			InvokeOnUI(() =>
			{
				if (GetControlByName(controlName.ToString()) is FrameworkElement elem)
				{
					elem.Width = (double)width;
					elem.Height = (double)height;
				}
			});
		}

		/// <summary>
		/// Sets background color or brush on a control.
		/// </summary>
		public static void SetControlBackground(Primitive controlName, Primitive color)
		{
			InvokeOnUI(() =>
			{
				UIElement ctrl = GetControlByName(controlName.ToString());
				Brush brush = ParseBrush(color.ToString());
				if (ctrl is Control c) c.Background = brush;
				else if (ctrl is Panel p) p.Background = brush;
				else if (ctrl is Border b) b.Background = brush;
			});
		}

		/// <summary>
		/// Sets text/foreground color on a control.
		/// </summary>
		public static void SetControlForeground(Primitive controlName, Primitive color)
		{
			InvokeOnUI(() =>
			{
				UIElement ctrl = GetControlByName(controlName.ToString());
				Brush brush = ParseBrush(color.ToString());
				if (ctrl is Control c) c.Foreground = brush;
				else if (ctrl is TextBlock tb) tb.Foreground = brush;
			});
		}

		/// <summary>
		/// Sets font family, font size, bold, and italic attributes on a control.
		/// </summary>
		public static void SetControlFont(Primitive controlName, Primitive fontFamily, Primitive fontSize, Primitive isBold, Primitive isItalic)
		{
			InvokeOnUI(() =>
			{
				UIElement ctrl = GetControlByName(controlName.ToString());
				if (ctrl == null) return;

				FontFamily family = new FontFamily(fontFamily.ToString());
				double size = (double)fontSize > 0 ? (double)fontSize : 12;
				FontWeight weight = (bool)isBold ? FontWeights.Bold : FontWeights.Normal;
				FontStyle style = (bool)isItalic ? FontStyles.Italic : FontStyles.Normal;

				if (ctrl is Control c)
				{
					c.FontFamily = family;
					c.FontSize = size;
					c.FontWeight = weight;
					c.FontStyle = style;
				}
				else if (ctrl is TextBlock tb)
				{
					tb.FontFamily = family;
					tb.FontSize = size;
					tb.FontWeight = weight;
					tb.FontStyle = style;
				}
			});
		}

		/// <summary>
		/// Sets opacity (0.0 to 1.0).
		/// </summary>
		public static void SetControlOpacity(Primitive controlName, Primitive opacity)
		{
			InvokeOnUI(() =>
			{
				UIElement ctrl = GetControlByName(controlName.ToString());
				if (ctrl != null) ctrl.Opacity = (double)opacity;
			});
		}

		/// <summary>
		/// Sets Visibility (Visible, Hidden, Collapsed).
		/// </summary>
		public static void SetControlVisibility(Primitive controlName, Primitive visibility)
		{
			InvokeOnUI(() =>
			{
				UIElement ctrl = GetControlByName(controlName.ToString());
				if (ctrl != null && Enum.TryParse(visibility.ToString(), true, out Visibility v))
				{
					ctrl.Visibility = v;
				}
			});
		}

		/// <summary>
		/// Enables or disables a control.
		/// </summary>
		public static void SetControlIsEnabled(Primitive controlName, Primitive isEnabled)
		{
			InvokeOnUI(() =>
			{
				UIElement ctrl = GetControlByName(controlName.ToString());
				if (ctrl != null) ctrl.IsEnabled = (bool)isEnabled;
			});
		}

		/// <summary>
		/// Sets tooltip text on a control.
		/// </summary>
		public static void SetControlToolTip(Primitive controlName, Primitive toolTipText)
		{
			InvokeOnUI(() =>
			{
				if (GetControlByName(controlName.ToString()) is FrameworkElement elem)
				{
					elem.ToolTip = toolTipText.ToString();
				}
			});
		}

		/// <summary>
		/// Sets reflection property value on any control.
		/// </summary>
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
		/// Reads property value from a control.
		/// </summary>
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

		// =========================================================================
		// CONTAINER & ITEMS MANAGEMENT
		// =========================================================================

		/// <summary>
		/// Adds a child control into a parent container (Panel, Border, ContentControl, Expander, GroupBox, TabControl, etc.).
		/// Safely detaches the child control from its previous parent container first.
		/// </summary>
		public static void AddChild(Primitive parentName, Primitive childName)
		{
			InvokeOnUI(() =>
			{
				UIElement parent = GetControlByName(parentName.ToString());
				UIElement child = GetControlByName(childName.ToString());
				if (parent == null || child == null) return;

				RemoveFromParent(child);

				if (parent is Panel panel)
				{
					panel.Children.Add(child);
				}
				else if (parent is Border border)
				{
					border.Child = child;
				}
				else if (parent is ContentControl contentControl)
				{
					contentControl.Content = child;
				}
				else if (parent is ItemsControl itemsControl)
				{
					itemsControl.Items.Add(child);
				}
			});
		}

		/// <summary>
		/// Adds a text/object item to item containers (ComboBox, ListBox, TabControl, TreeView, DataGrid, Menu).
		/// </summary>
		public static void AddItem(Primitive controlName, Primitive item)
		{
			InvokeOnUI(() =>
			{
				UIElement ctrl = GetControlByName(controlName.ToString());
				if (ctrl is ItemsControl itemsControl)
				{
					itemsControl.Items.Add(item.ToString());
				}
			});
		}

		/// <summary>
		/// Clears all items from item containers.
		/// </summary>
		public static void ClearItems(Primitive controlName)
		{
			InvokeOnUI(() =>
			{
				UIElement ctrl = GetControlByName(controlName.ToString());
				if (ctrl is ItemsControl itemsControl)
				{
					itemsControl.Items.Clear();
				}
			});
		}

		/// <summary>
		/// Gets selected item text from ComboBox or ListBox.
		/// </summary>
		public static Primitive GetSelectedItem(Primitive controlName)
		{
			string res = "";
			InvokeOnUI(() =>
			{
				UIElement ctrl = GetControlByName(controlName.ToString());
				if (ctrl is Selector selector && selector.SelectedItem != null)
				{
					res = selector.SelectedItem.ToString();
				}
			});
			return res;
		}

		/// <summary>
		/// Sets selected index of ComboBox, ListBox, or TabControl.
		/// </summary>
		public static void SetSelectedIndex(Primitive controlName, Primitive index)
		{
			InvokeOnUI(() =>
			{
				UIElement ctrl = GetControlByName(controlName.ToString());
				if (ctrl is Selector selector)
				{
					selector.SelectedIndex = (int)index;
				}
			});
		}

		// =========================================================================
		// EVENT HOOKING & XAML
		// =========================================================================

		/// <summary>
		/// Hooks any WPF event on a ZSUI control to a Small Basic subroutine.
		/// Events: Click, MouseDown, MouseUp, MouseEnter, MouseLeave, KeyDown, KeyUp, TextChanged, SelectionChanged, Checked, Unchecked, ValueChanged, Loaded, Closed, GotFocus, LostFocus, etc.
		/// </summary>
		public static void HookEvent(Primitive controlName, Primitive eventName, Primitive sbSubName)
		{
			InvokeOnUI(() =>
			{
				UIElement ctrl = GetControlByName(controlName.ToString());
				if (ctrl == null && _windows.ContainsKey(controlName.ToString()))
				{
					ctrl = _windows[controlName.ToString()];
				}

				if (ctrl != null)
				{
					ZSEvent.HookEventToSBSub(ctrl, eventName.ToString(), sbSubName.ToString());
				}
			});
		}

		/// <summary>
		/// Loads XAML string or XAML file path into active window content.
		/// </summary>
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
	}
}
