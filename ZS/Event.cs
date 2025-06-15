using System;
using Microsoft.SmallBasic.Library;
using ZS;
using System.Reflection;
using System.Windows;

namespace ZS
{
	public class SBEventWrapper
	{
		private readonly string _sub;

		public SBEventWrapper(string sub)
		{
			_sub = sub;
		}

		// Works for all events that match (object, EventArgs or derived)
		public void Handle(object sender, EventArgs e)
		{
			try
			{
				ZSEvent.CallSub(_sub);
				ZSEvent._lasteventarg = e.ToString();
				ZSEvent._sender = sender.ToString();
			}
			catch (Exception ex)
			{
				ZSUtilities.OnError(ex);
			}
		}
	}

	/// <summary>
	/// some function related to events.
	/// </summary>
	[SmallBasicType]
	public static class ZSEvent
	{
		private static Assembly entryAssembly = Assembly.GetEntryAssembly();
		private static Type mainModule = entryAssembly.EntryPoint.DeclaringType;

		public static string _lasteventarg;
		public static string _sender;

		/// <summary>
		/// Last event args to string.
		/// </summary>
		public static Primitive Lasteventargs
		{
			get
			{
				try
				{
					return _lasteventarg.ToString() ?? "";
				}
				catch (Exception ex)
				{
					ZSUtilities.OnError(ex);
					return "";
				}
			}
		}

		/// <summary>
		/// Last event sender to string.
		/// </summary>
		public static Primitive sender
		{
			get
			{
				try
				{
					return _sender.ToString() ?? "";
				}
				catch (Exception ex)
				{
					ZSUtilities.OnError(ex);
					return "";
				}
			}
		}

		/// <summary>
		/// Hooks a .NET event to a Small Basic subroutine.
		/// </summary>
		public static void HookEventToSBSub(object target, string eventName, string sbSubName)
		{
			try
			{
				if (target == null || string.IsNullOrEmpty(eventName) || string.IsNullOrEmpty(sbSubName))
				{
					TextWindow.WriteLine("Invalid arguments");
					return;
				}

				var eventInfo = target.GetType().GetEvent(eventName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
				if (eventInfo == null)
				{
					TextWindow.WriteLine("Event " + eventName + " not found on " + target.GetType().Name + ".");
					return;
				}

				var handlerType = eventInfo.EventHandlerType;
				var methodInfo = typeof(SBEventWrapper).GetMethod("Handle", BindingFlags.Instance | BindingFlags.Public);
				var wrapper = new SBEventWrapper(sbSubName);

				var handler = Delegate.CreateDelegate(handlerType, wrapper, methodInfo);
				eventInfo.AddEventHandler(target, handler);
			}
			catch (Exception ex)
			{
				ZSUtilities.OnError(ex);
			}
		}

		/// <summary>
		/// Invokes a Small Basic subroutine using reflection.
		/// </summary>
		public static void CallSub(string SubName)
		{
			try
			{
				MethodInfo methodInfo = mainModule.GetMethod(SubName, BindingFlags.IgnoreCase | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
				if (methodInfo != null)
				{
					methodInfo.Invoke(null, null);
				}
				else
				{
					TextWindow.WriteLine("Sub '" + SubName + "' not found.");
				}
			}
			catch (Exception ex)
			{
				ZSUtilities.OnError(ex);
			}
		}
	}
}
