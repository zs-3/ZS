using Microsoft.SmallBasic.Library;
using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;

namespace ZS
{
	/// <summary>
	/// Advanced utility for invoking Small Basic subroutines like functions with arguments and return values.
	/// </summary>
	[SmallBasicType]
	public static class ZSCall
	{
		private static Dictionary<string, Primitive> _arguments = new Dictionary<string, Primitive>(StringComparer.OrdinalIgnoreCase);
		private static Primitive _lastReturnValue = "";

		private static Assembly entryAssembly = Assembly.GetEntryAssembly();
		private static Type mainModule = entryAssembly != null && entryAssembly.EntryPoint != null ? entryAssembly.EntryPoint.DeclaringType : null;

		private static void Func(string funcName)
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
				MethodInfo methodInfo = mainModule.GetMethod(funcName, BindingFlags.IgnoreCase | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
				if (methodInfo != null)
				{
					methodInfo.Invoke(null, null);
					return;
				}
			}

			// Fallback search across loaded assemblies if entry assembly fails
			foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
			{
				if (asm.IsDynamic) continue;
				Type[] types = asm.GetTypes();
				foreach (Type t in types)
				{
					MethodInfo m = t.GetMethod(funcName, BindingFlags.IgnoreCase | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
					if (m != null)
					{
						m.Invoke(null, null);
						return;
					}
				}
			}
		}

		private static void SetVal(string Name, string Value)
		{
			if (mainModule != null)
			{
				FieldInfo fieldInfo = mainModule.GetField(Name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase);
				if (fieldInfo != null)
				{
					fieldInfo.SetValue(null, Value);
				}
			}
		}

		private static string GetVal(string Name)
		{
			if (mainModule != null)
			{
				FieldInfo fieldInfo = mainModule.GetField(Name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase);
				if (fieldInfo != null)
				{
					object val = fieldInfo.GetValue(null);
					return val != null ? val.ToString() : "";
				}
			}
			return "";
		}

		/// <summary>
		/// Event triggered when CallSubWithEvent is called.
		/// </summary>
		public static event SmallBasicCallback Function;

		/// <summary>
		/// Sets a named argument before executing a subroutine.
		/// </summary>
		/// <param name="name">The argument name (e.g., "arg1" or variable name).</param>
		/// <param name="value">The argument value.</param>
		public static void SetArg(Primitive name, Primitive value)
		{
			try
			{
				string key = name.ToString();
				_arguments[key] = value;
				SetVal(key, value.ToString());
				Stack.PushValue(key, value);
			}
			catch (Exception ex)
			{
				ZSUtilities.OnError(ex);
			}
		}

		/// <summary>
		/// Gets a passed argument value by name or index inside a subroutine.
		/// </summary>
		/// <param name="name">Argument name or index (e.g. "1", "arg1").</param>
		/// <returns>The argument value or empty string if not found.</returns>
		public static Primitive GetArg(Primitive name)
		{
			try
			{
				string key = name.ToString();
				if (_arguments.ContainsKey(key))
				{
					return _arguments[key];
				}

				Primitive valFromStack = Stack.PopValue(key);
				if (!string.IsNullOrEmpty(valFromStack.ToString()))
				{
					return valFromStack;
				}

				string valFromVar = GetVal(key);
				return new Primitive(valFromVar);
			}
			catch (Exception ex)
			{
				ZSUtilities.OnError(ex);
				return "";
			}
		}

		/// <summary>
		/// Sets the return value from inside a Small Basic subroutine.
		/// </summary>
		/// <param name="value">The return value.</param>
		public static void Return(Primitive value)
		{
			try
			{
				_lastReturnValue = value;
				Stack.PushValue("Return", value);
				SetVal("Return", value.ToString());
			}
			catch (Exception ex)
			{
				ZSUtilities.OnError(ex);
			}
		}

		/// <summary>
		/// Sets the return value for CallSubWithEvent.
		/// </summary>
		/// <param name="value">The return value.</param>
		public static void Return2(Primitive value)
		{
			try
			{
				_lastReturnValue = value;
				Stack.PushValue("ReturnValue-Event", value);
			}
			catch (Exception ex)
			{
				ZSUtilities.OnError(ex);
			}
		}

		/// <summary>
		/// Calls a Small Basic subroutine with position-based or semicolon-separated arguments and returns the result.
		/// </summary>
		/// <param name="subName">The name of the Sub in Small Basic.</param>
		/// <param name="args">Semicolon separated key=value string (e.g. "a=5;b=10") or positional values ("5;10").</param>
		/// <returns>The return value set by ZSCall.Return() or Return variable.</returns>
		public static Primitive Call(Primitive subName, Primitive args)
		{
			try
			{
				_arguments.Clear();
				_lastReturnValue = "";

				string argsStr = args.ToString();
				if (!string.IsNullOrEmpty(argsStr))
				{
					string[] pairs = argsStr.Split(';');
					int index = 1;
					foreach (string pair in pairs)
					{
						string[] kv = pair.Split('=');
						if (kv.Length == 2)
						{
							string k = kv[0].Trim();
							string v = kv[1].Trim();
							SetArg(k, v);
						}
						else
						{
							string val = pair.Trim();
							SetArg("arg" + index, val);
							SetArg(index.ToString(), val);
							index++;
						}
					}
				}

				Func(subName.ToString());

				if (!string.IsNullOrEmpty(_lastReturnValue.ToString()))
				{
					return _lastReturnValue;
				}

				Primitive retFromStack = Stack.PopValue("Return");
				if (!string.IsNullOrEmpty(retFromStack.ToString()))
				{
					return retFromStack;
				}

				string retFromVar = GetVal("Return_" + subName.ToString());
				if (string.IsNullOrEmpty(retFromVar))
				{
					retFromVar = GetVal("Return");
				}

				return new Primitive(retFromVar);
			}
			catch (Exception ex)
			{
				ZSUtilities.OnError(ex);
				return "";
			}
		}

		/// <summary>
		/// Calls a Small Basic subroutine without arguments and gets the return value.
		/// </summary>
		/// <param name="subName">The subroutine name.</param>
		/// <returns>The return value.</returns>
		public static Primitive CallSubNoArgs(Primitive subName)
		{
			return Call(subName, "");
		}

		/// <summary>
		/// Calls a subroutine with semicolon-separated key=value pairs or positional arguments.
		/// </summary>
		public static Primitive CallSub(Primitive Sub, Primitive Values)
		{
			return Call(Sub, Values);
		}

		/// <summary>
		/// Calls the Small Basic subroutine subscribed to the Function event with arguments.
		/// </summary>
		public static Primitive CallSubWithEvent(Primitive Values)
		{
			try
			{
				_lastReturnValue = "";
				string valuesString = Values.ToString();
				string[] pairs = valuesString.Split(';');

				foreach (string pair in pairs)
				{
					string[] keyValue = pair.Split('=');
					if (keyValue.Length == 2)
					{
						string VarName = keyValue[0].Trim();
						string Value = keyValue[1].Trim();
						SetVal(VarName, Value);
					}
				}

				if (Function != null)
				{
					Function.Invoke();
				}

				if (!string.IsNullOrEmpty(_lastReturnValue.ToString()))
				{
					return _lastReturnValue;
				}

				return GetValue("ReturnValue-Event");
			}
			catch (Exception ex)
			{
				ZSUtilities.OnError(ex);
				return "";
			}
		}

		/// <summary>
		/// Gets a value from stack by name.
		/// </summary>
		public static Primitive GetValue(Primitive Name)
		{
			return Stack.PopValue(Name);
		}

		/// <summary>
		/// Adds a value to the stack.
		/// </summary>
		[HideFromIntellisense]
		public static void AddValue(Primitive Name, Primitive Value)
		{
			Stack.PushValue(Name, Value);
		}

		/// <summary>
		/// Legacy method for calling subroutine with variable assignment.
		/// </summary>
		public static Primitive Call2(Primitive Sub, Primitive Values)
		{
			return Call(Sub, Values);
		}

		/// <summary>
		/// Legacy method for calling subroutine returning Return_SubName variable.
		/// </summary>
		public static Primitive Call3(Primitive Sub, Primitive Values)
		{
			return Call(Sub, Values);
		}
	}
}
