using Microsoft.SmallBasic.Library;
using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;

namespace ZS
{
	/// <summary>
	/// Use For Using Small Basic Sub As A Method.
	/// </summary>
	[SmallBasicType]
	public static class ZSCall
	{


		private static Dictionary<Primitive, Primitive> Vars = new Dictionary<Primitive, Primitive>();
		
		private static Assembly entryAssembly = Assembly.GetEntryAssembly();
		private static Type mainModule = entryAssembly.EntryPoint.DeclaringType;
		private static void Func(string funcName)
		{
			MethodInfo methodInfo = mainModule.GetMethod(funcName, BindingFlags.IgnoreCase | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
			methodInfo.Invoke(null, null);
		}

		private static void SetVal(string Name, string Value)
		{
			FieldInfo fieldInfo = mainModule.GetField(Name, BindingFlags.NonPublic | BindingFlags.Static);
			fieldInfo.SetValue(null, Value);
		}
		
		private static String GetVal(string Name)
		{
			FieldInfo fieldInfo = mainModule.GetField(Name, BindingFlags.NonPublic | BindingFlags.Static);
			return fieldInfo.GetValue(null).ToString();
		}

		private static string GetCallingMethodName()
		{
			// Create a StackTrace object
			StackTrace stackTrace = new StackTrace(true);

			// Get the calling method from the stack trace
			StackFrame frame = stackTrace.GetFrame(2); // 2 represents the method that called the current method
			if (frame != null) {
				MethodBase method = frame.GetMethod();
				return method.Name;
			}
			return "Unknown";
		}
		
		/// <summary>
		/// The Event Of Sub
		/// </summary>
		public static event SmallBasicCallback Function;

		[HideFromIntellisense]
		public static void AddValue(Primitive Name, Primitive Value)
		{
			Stack.PushValue(Name, Value);			
		}

		/// <summary>
		/// Gets The Value Sent To Stack
		/// </summary>
		/// <param name="Name">The name of stack</param>
		/// <returns>Value sent </returns>
		public static Primitive GetValue(Primitive Name)
		{
			return Stack.PopValue(Name);
		}

		/// <summary>
		/// Return The Value to CallSub.
		/// </summary>
		/// <param name="Value">The Value To Return.</param>
		public static void Return(Primitive Value)
		{
			Stack.PushValue("Return", Value);
			
		}
		
		/// <summary>
		/// Return The Value to CallSubWithEvent
		/// </summary>
		/// <param name="Value">The Value To Return</param>
		public static void Return2(Primitive Value)
		{
			Stack.PushValue("ReturnValue-Event", Value);
			
		}

		/// <summary>
		/// Calls The Sub With Args.
		/// </summary>
		/// <param name="Sub">The Sub Name</param>
		/// <param name="Values">The Values Of Args Seperated By ;.</param>
		/// <returns>The Output Send Through Return.</returns>
		public static Primitive CallSub(Primitive Sub, Primitive Values)
		{
			// Convert Primitive to string
			string valuesString = Values.ToString();

			// Split the input string by semicolon to get each stack name-value pair
			string[] pairs = valuesString.Split(';');

			foreach (string pair in pairs) {
				// Split each pair by '=' to get the stack name and value
				string[] keyValue = pair.Split('=');

				if (keyValue.Length == 2) {
					string stackName = keyValue[0].Trim();
					string stackValue = keyValue[1].Trim();

					// Add the value to the stack
					AddValue(stackName, stackValue);
				}
			}
			Func(Sub);
			return GetValue("Return");
		}
		
		/// <summary>
		/// Calls The Sub That Is Suscribed To Function Event With Args.
		/// </summary>
		/// <param name="Values">The Values Of Args Seperated By ;.</param>
		/// <returns>The Output Send Through Return2.</returns>
		public static Primitive CallSubWithEvent(Primitive Values)
		{
			// Convert Primitive to string
			string valuesString = Values.ToString();

			// Split the input string by semicolon to get each stack name-value pair
			string[] pairs = valuesString.Split(';');

			foreach (string pair in pairs) {
				// Split each pair by '=' to get the stack name and value
				string[] keyValue = pair.Split('=');

				if (keyValue.Length == 2) {
					string VarName = keyValue[0].Trim();
					string Value = keyValue[1].Trim();

					// Add the value to the stack
					SetVal(VarName, Value);
				}
			}
			Function.Invoke();
			return GetValue("ReturnValue-Event");
		}
		
		/// <summary>
		/// Calls The Sub With Args.
		/// The Varibles decleared inside sub will be assinged with new arg value.
		/// A Varible by name Return value will be returned.
		/// </summary>
		/// <param name="Values">The Values Of Args Seperated By ;.</param>
		/// <param name="Sub">The Sub Name.</param>
		/// <returns>The Output Send Through Return.</returns>
		public static Primitive Call2(Primitive Sub,Primitive Values)
		{
			// Convert Primitive to string
			string valuesString = Values.ToString();

			// Split the input string by semicolon to get each stack name-value pair
			string[] pairs = valuesString.Split(';');

			foreach (string pair in pairs) {
				// Split each pair by '=' to get the stack name and value
				string[] keyValue = pair.Split('=');

				if (keyValue.Length == 2) {
					string VarName = keyValue[0].Trim();
					string Value = keyValue[1].Trim();

					// Add the value to the stack
					SetVal(VarName, Value);
				}
			}
			Func(Sub);
			return GetVal("Return");
		}
		
		/// <summary>
		/// Calls The Sub With Args.
		/// The Varibles decleared inside sub will be assinged with new arg value.
		/// A Varible by name Return_SubName value will be returned.
		/// </summary>
		/// <param name="Values">The Values Of Args Seperated By ;.</param>
		/// <returns>The Output Send Through Return.</returns>
		public static Primitive Call3(Primitive Sub,Primitive Values)
		{
			// Convert Primitive to string
			string valuesString = Values.ToString();

			// Split the input string by semicolon to get each stack name-value pair
			string[] pairs = valuesString.Split(';');

			foreach (string pair in pairs) {
				// Split each pair by '=' to get the stack name and value
				string[] keyValue = pair.Split('=');

				if (keyValue.Length == 2) {
					string VarName = keyValue[0].Trim();
					string Value = keyValue[1].Trim();

					// Add the value to the stack
					SetVal(VarName, Value);
				}
			}
			Func(Sub);
			return GetVal("Return_" + Sub);
		}
	}
}
