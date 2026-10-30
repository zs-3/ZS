using System;
using Microsoft.SmallBasic.Library;
using System.Reflection;
using System.Windows;
using System.Collections.Generic;

namespace ZS
{
	/// <summary>
	/// Dynamic wrapper class for hooking any .NET or WPF event to Small Basic subroutines.
	/// </summary>
	public class SBEventWrapper
	{
		private readonly string _sub;

		public SBEventWrapper(string sub)
		{
			_sub = sub;
		}

		/// <summary>
		/// Generic event handler method matching standard (object sender, EventArgs e) signature.
		/// </summary>
		public void Handle(object sender, EventArgs e)
		{
			try
			{
				ZSEvent._lastSenderObj = sender;
				ZSEvent._lastEventArgsObj = e;
				ZSEvent._sender = sender != null ? sender.ToString() : "";
				ZSEvent._lasteventarg = e != null ? e.ToString() : "";
				ZSEvent.CallSub(_sub);
			}
			catch (Exception ex)
			{
				ZSUtilities.OnError(ex);
			}
		}

		/// <summary>
		/// Event handler method matching RoutedEventHandler signature in WPF.
		/// </summary>
		public void HandleRouted(object sender, RoutedEventArgs e)
		{
			Handle(sender, e);
		}
	}

	/// <summary>
	/// Powerful event hooking engine allowing Small Basic to subscribe to any WPF or .NET event directly.
	/// </summary>
	[SmallBasicType]
	public static class ZSEvent
	{
		private static Assembly entryAssembly = Assembly.GetEntryAssembly();
		private static Type mainModule = entryAssembly != null && entryAssembly.EntryPoint != null ? entryAssembly.EntryPoint.DeclaringType : null;

		public static string _lasteventarg = "";
		public static string _sender = "";
		public static object _lastSenderObj;
		public static object _lastEventArgsObj;

		/// <summary>
		/// String representation of the last event arguments.
		/// </summary>
		public static Primitive Lasteventargs
		{
			get
			{
				try
				{
					return _lasteventarg ?? "";
				}
				catch (Exception ex)
				{
					ZSUtilities.OnError(ex);
					return "";
				}
			}
		}

		/// <summary>
		/// String representation of the last event sender object.
		/// </summary>
		public static Primitive sender
		{
			get
			{
				try
				{
					return _sender ?? "";
				}
				catch (Exception ex)
				{
					ZSUtilities.OnError(ex);
					return "";
				}
			}
		}

		/// <summary>
		/// Retrieves a property value from the last event arguments object.
		/// </summary>
		/// <param name="propertyName">Name of property (e.g. "Key", "ClickCount", "NewValue").</param>
		/// <returns>Property value as string.</returns>
		public static Primitive GetEventArgsProperty(Primitive propertyName)
		{
			try
			{
				if (_lastEventArgsObj == null) return "";
				PropertyInfo prop = _lastEventArgsObj.GetType().GetProperty(propertyName.ToString(), BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
				if (prop != null)
				{
					object val = prop.GetValue(_lastEventArgsObj, null);
					return val != null ? val.ToString() : "";
				}
			}
			catch (Exception ex)
			{
				ZSUtilities.OnError(ex);
			}
			return "";
		}

		/// <summary>
		/// Retrieves a property value from the sender object of the last triggered event.
		/// </summary>
		/// <param name="propertyName">Name of property (e.g., "Name", "Text", "Tag").</param>
		/// <returns>Property value as string.</returns>
		public static Primitive GetSenderProperty(Primitive propertyName)
		{
			try
			{
				if (_lastSenderObj == null) return "";
				PropertyInfo prop = _lastSenderObj.GetType().GetProperty(propertyName.ToString(), BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
				if (prop != null)
				{
					object val = prop.GetValue(_lastSenderObj, null);
					return val != null ? val.ToString() : "";
				}
			}
			catch (Exception ex)
			{
				ZSUtilities.OnError(ex);
			}
			return "";
		}

		/// <summary>
		/// Hooks any .NET or WPF event on a target object directly to a Small Basic subroutine.
		/// </summary>
		/// <param name="target">The target control or object instance.</param>
		/// <param name="eventName">The event name (e.g., "Click", "MouseDown", "TextChanged").</param>
		/// <param name="sbSubName">The Small Basic subroutine name to invoke when the event fires.</param>
		public static void HookEventToSBSub(object target, string eventName, string sbSubName)
		{
			try
			{
				if (target == null || string.IsNullOrEmpty(eventName) || string.IsNullOrEmpty(sbSubName))
				{
					TextWindow.WriteLine("Invalid arguments for HookEventToSBSub");
					return;
				}

				EventInfo eventInfo = target.GetType().GetEvent(eventName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.IgnoreCase);
				if (eventInfo == null)
				{
					TextWindow.WriteLine("Event '" + eventName + "' not found on object type '" + target.GetType().Name + "'.");
					return;
				}

				Type handlerType = eventInfo.EventHandlerType;
				SBEventWrapper wrapper = new SBEventWrapper(sbSubName);
				MethodInfo handleMethod = typeof(SBEventWrapper).GetMethod("Handle", BindingFlags.Instance | BindingFlags.Public);

				Delegate handler = Delegate.CreateDelegate(handlerType, wrapper, handleMethod, false);
				if (handler == null)
				{
					MethodInfo handleRoutedMethod = typeof(SBEventWrapper).GetMethod("HandleRouted", BindingFlags.Instance | BindingFlags.Public);
					handler = Delegate.CreateDelegate(handlerType, wrapper, handleRoutedMethod, false);
				}

				if (handler != null)
				{
					eventInfo.AddEventHandler(target, handler);
				}
				else
				{
					TextWindow.WriteLine("Could not create delegate for event '" + eventName + "'.");
				}
			}
			catch (Exception ex)
			{
				ZSUtilities.OnError(ex);
			}
		}

		/// <summary>
		/// Hooks a shape or WPF control by name in GraphicsWindow or ZSUI to a Small Basic subroutine.
		/// </summary>
		/// <param name="controlName">The name of the control or shape.</param>
		/// <param name="eventName">The event name (e.g. "Click", "MouseEnter", "KeyDown").</param>
		/// <param name="sbSubName">The Small Basic subroutine name to execute.</param>
		public static void HookControlEvent(Primitive controlName, Primitive eventName, Primitive sbSubName)
		{
			try
			{
				UIElement ctrl = ZSWpf.GetControl(controlName.ToString());
				if (ctrl != null)
				{
					HookEventToSBSub(ctrl, eventName.ToString(), sbSubName.ToString());
					return;
				}

				ctrl = ZSUI.GetControlByName(controlName.ToString());
				if (ctrl != null)
				{
					HookEventToSBSub(ctrl, eventName.ToString(), sbSubName.ToString());
				}
			}
			catch (Exception ex)
			{
				ZSUtilities.OnError(ex);
			}
		}

		/// <summary>
		/// Invokes a Small Basic subroutine using reflection.
		/// </summary>
		/// <param name="SubName">The subroutine name.</param>
		public static void CallSub(string SubName)
		{
			try
			{
				if (mainModule == null)
				{
					entryAssembly = Assembly.GetEntryAssembly();
					if (entryAssembly != null && entryAssembly.EntryPoint != null)
					{
						mainModule = entryAssembly.EntryPoint.DeclaringType;
					}
				}

				if (mainModule != null)
				{
					MethodInfo methodInfo = mainModule.GetMethod(SubName, BindingFlags.IgnoreCase | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
					if (methodInfo != null)
					{
						methodInfo.Invoke(null, null);
						return;
					}
				}

				foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
				{
					if (asm.IsDynamic) continue;
					Type[] types = asm.GetTypes();
					foreach (Type t in types)
					{
						MethodInfo m = t.GetMethod(SubName, BindingFlags.IgnoreCase | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
						if (m != null)
						{
							m.Invoke(null, null);
							return;
						}
					}
				}
			}
			catch (Exception ex)
			{
				ZSUtilities.OnError(ex);
			}
		}
	}
}
