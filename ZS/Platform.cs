using Microsoft.SmallBasic.Library;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ZS
{
	/// <summary>
	/// The Platform object provides a way to generically invoke other .NET libraries.
	/// </summary>
	[SmallBasicType]
	public static class ZSPlatform
	{
        
		/// <summary>
		/// Invokes A Static Method.
		/// </summary>
		/// <param name="Path">The Path to dll.</param>
		/// <param name="Class">The Namespace.Class</param>
		/// <param name="Method">The Method Name</param>
		/// <param name="parameters">The array of Parameters</param>
		/// <returns>The Result</returns>
		public static Primitive InvokeStaticMethod(Primitive Path, Primitive Class, Primitive Method, Primitive[] parameters)
		{
			Assembly assembly = Assembly.LoadFrom(Path.ToString());
			Type myclass = assembly.GetType(Class.ToString());
			MethodInfo method = myclass.GetMethod(Method.ToString(), BindingFlags.Static | BindingFlags.Public);
    		
			ParameterInfo[] paramInfos = method.GetParameters();
			object[] convertedParams = new object[paramInfos.Length];

			// Convert parameters to the appropriate types
			for (int i = 0; i < paramInfos.Length; i++) {
				// Get the target type for the parameter
				Type paramType = paramInfos[i].ParameterType;
				convertedParams[i] = ConvertToType(parameters[i], paramType);
			}
			object result = method.Invoke(null, convertedParams);
			return result != null ? result.ToString() : "";

		}
		
		private static object ConvertToType(Primitive param, Type targetType)
		{
			string paramValue = param.ToString();

			if (targetType == typeof(int)) {
				return int.Parse(paramValue); // Convert to int
			} else if (targetType == typeof(float) || targetType == typeof(double)) {
				return double.Parse(paramValue); // Convert to float/double
			} else if (targetType == typeof(bool)) {
				return paramValue.ToLower() == "true" || paramValue == "1"; // Convert to bool
			} else {
				return paramValue; // Default to string for unsupported types
			}
		}
		
		/// <summary>
		/// Invoke A Nonstatic Method.
		/// </summary>
		/// <param name="Path">The dll Path.</param>
		/// <param name="Class">The Namespace.Class</param>
		/// <param name="Method">The Method Name.</param>
		/// <param name="parameters">The Array of Parameters</param>
		/// <returns>The Result</returns>
		public static Primitive InvokeMethod(Primitive Path, Primitive Class, Primitive Method, Primitive[] parameters)
		{
			Assembly assembly = Assembly.LoadFrom(Path.ToString());
			Type myclass = assembly.GetType(Class.ToString());
			object instance = Activator.CreateInstance(myclass);
			MethodInfo method = myclass.GetMethod(Method.ToString(), BindingFlags.Static | BindingFlags.Public);
    		
			ParameterInfo[] paramInfos = method.GetParameters();
			object[] convertedParams = new object[paramInfos.Length];

			// Convert parameters to the appropriate types
			for (int i = 0; i < paramInfos.Length; i++) {
				// Get the target type for the parameter
				Type paramType = paramInfos[i].ParameterType;
				convertedParams[i] = ConvertToType(parameters[i], paramType);
			}
			object result = method.Invoke(instance, convertedParams);
			return result != null ? result.ToString() : "";

		}
    	
    	
	}
}
