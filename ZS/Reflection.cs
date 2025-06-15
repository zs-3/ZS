using Microsoft.SmallBasic.Library;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ZS
{
	/// <summary>
	/// The Reflection For Small Basic.
	/// Used by many extension classes.
	/// </summary>
	[SmallBasicType]
	public static class ZSReflection
	{
		private static Assembly entryAssembly = Assembly.GetEntryAssembly();
		private static Type mainModule = entryAssembly.EntryPoint.DeclaringType;
		private static Dictionary<string, Assembly> _assembly = new Dictionary<string, Assembly>();
		private static Dictionary<string, Type> _type = new Dictionary<string, Type>();
		
		/// <summary>
		/// Invokes A Static Method.
		/// </summary>
		/// <param name="Path">The Path to dll.</param>
		/// <param name="Class">The Namespace.Class</param>
		/// <param name="Method">The Method Name</param>
		/// <param name="parameters">The array of Parameters</param>
		/// <returns>The Result</returns>
		[HideFromIntellisense]
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
		/// Get Some Predefined Types And Assembly.
		/// </summary>
		/// <param name="Option">
		/// 1 - Current Program.
		/// 2 - Current Class of program.
		/// 3 - Assembly Key Of SmallBasicLibaray.dll
		/// 4 - Assembly Key Of ZS.dll
		/// </param>
		/// <returns>The Result</returns>
		public static Primitive GetPre(Primitive Option)
		{
			if (Option == "1") {
				Primitive Key = ZSWpf.GenerateNewName("Assembly");
				_assembly.Add(Key, entryAssembly);
				return Key;
			}
			if (Option == "2") {
				Primitive typeKey = ZSWpf.GenerateNewName("Type");
				_type.Add(typeKey, mainModule);
				return typeKey;
			}
			if (Option == "3") {
				Primitive typeKey = ZSWpf.GenerateNewName("Assembly");
				_assembly.Add(typeKey, Assembly.LoadFrom("SmallBasicLibrary.dll"));
			}
			if (Option == "4") {
				Primitive typeKey = ZSWpf.GenerateNewName("Assembly");
				_assembly.Add(typeKey, Assembly.LoadFrom("ZS.dll"));
			}
			return "Wrong Option!!";
		}
		
		/// <summary>
		/// Loads A Assembly.
		/// A exe or dll is assembly.
		/// </summary>
		/// <param name="Path">The Path Of Assembly.</param>
		/// <returns>The Assembly Key.</returns>
		public static Primitive LoadAssembly(Primitive Path)
		{
			try {
				Assembly assm = Assembly.LoadFrom(Path);
				Primitive Key = ZSWpf.GenerateNewName("Assembly");
				_assembly.Add(Key, assm);
				return Key;
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
			}
			return "";
		}
		
		/// <summary>
		/// Get All Types in a assembly.
		/// </summary>
		/// <param name="Key">The Key</param>
		/// <returns>Array of name of types.</returns>
		public static Primitive GetTypes(Primitive Key)
		{
			try {
				Assembly asm = _assembly[Key];
				Type[] type = asm.GetTypes();
				Primitive returntype = null;
				int i = 0;
				foreach (Type ty in type) {
					++i;
					returntype[i] = ty.Name;
				}
				return returntype;
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
			}
			return "";
		}
		
		/// <summary>
		/// Loads A Type.
		/// </summary>
		/// <param name="Key">The Assembly Key.</param>
		/// <param name="Type">The Type Name (e.g., 'Namespace.ClassName').</param>
		/// <returns>The Type Key</returns>
		public static Primitive LoadType(Primitive Key, Primitive Type)
		{
			try {
				Assembly asm = _assembly[Key];
				Primitive typeKey = ZSWpf.GenerateNewName("Type");
				_type.Add(typeKey, asm.GetType(Type));
				return typeKey;
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
			}
			return "";
		}
		
		/// <summary>
		/// Gets A Array of methods in a type.
		/// </summary>
		/// <param name="Type">The Type Key.</param>
		/// <returns>Array of methods.</returns>
		public static Primitive GetMethods(Primitive Type)
		{
			try {
				Type tp = _type[Type];
				MethodInfo[] mi = tp.GetMethods(BindingFlags.Public |
				                  BindingFlags.NonPublic |
				                  BindingFlags.Instance |
				                  BindingFlags.Static);
				Primitive returntype = null;
				int i = 0;
				foreach (MethodInfo cmi in mi) {
					++i;
					returntype[i] = cmi.Name;
				}
				return returntype;
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
			}
			return "";
		}
		
		/// <summary>
		/// Gets A Array of Fields in a type.
		/// </summary>
		/// <param name="Type">The Type Key.</param>
		/// <returns>Array of Fields.</returns>
		public static Primitive GetFields(Primitive Type)
		{
			try {
				Type tp = _type[Type];
				FieldInfo[] mi = tp.GetFields(BindingFlags.Public |
				                 BindingFlags.NonPublic |
				                 BindingFlags.Instance |
				                 BindingFlags.Static);
				Primitive returntype = null;
				int i = 0;
				foreach (FieldInfo cmi in mi) {
					++i;
					returntype[i] = cmi.Name;
				}
				return returntype;
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
			}
			return "";
		}
		
		/// <summary>
		/// Gets A Array of Events in a type.
		/// </summary>
		/// <param name="Type">The Type Key.</param>
		/// <returns>Array of Events.</returns>
		public static Primitive GetEvents(Primitive Type)
		{
			Type tp = _type[Type];
			EventInfo[] mi = tp.GetEvents(BindingFlags.Public |
			                 BindingFlags.NonPublic |
			                 BindingFlags.Instance |
			                 BindingFlags.Static);
			Primitive returntype = null;
			int i = 0;
			foreach (EventInfo cmi in mi) {
				++i;
				returntype[i] = cmi.Name;
			}
			return returntype;
		}
		
		/// <summary>
		/// Gets A Array of Properties:(getters and setters). in a type.
		/// </summary>
		/// <param name="Type">The Type Key.</param>
		/// <returns>Array of Properties.</returns>
		public static Primitive GetProperties(Primitive Type)
		{
			Type tp = _type[Type];
			PropertyInfo[] mi = tp.GetProperties(BindingFlags.Public |
			                    BindingFlags.NonPublic |
			                    BindingFlags.Instance |
			                    BindingFlags.Static);
			Primitive returntype = null;
			int i = 0;
			foreach (PropertyInfo cmi in mi) {
				++i;
				returntype[i] = cmi.Name;
			}
			return returntype;
		}
		
		/// <summary>
		/// Invokes a method from type.
		/// </summary>
		/// <param name="Type">The Type Key.</param>
		/// <param name="Method">The Method Name.</param>
		/// <param name="Args">The Array Of Parameters.</param>
		/// <returns>The Result.</returns>
		public static Primitive InvokeMethod(Primitive Type, Primitive Method, Primitive[] Args)
		{
			Type tp = _type[Type];
			MethodInfo met = tp.GetMethod(Method, BindingFlags.Public |
			                 BindingFlags.NonPublic |
			                 BindingFlags.Instance |
			                 BindingFlags.Static);
			ParameterInfo[] paramInfos = met.GetParameters();
			object[] convertedParams = new object[paramInfos.Length];

			// Convert parameters to the appropriate types
			for (int i = 0; i < paramInfos.Length; i++) {
				// Get the target type for the parameter
				Type paramType = paramInfos[i].ParameterType;
				convertedParams[i] = ConvertToType(Args[i], paramType);
			}
			
			
			// Check if the method is static
			bool isStatic = met.IsStatic;

			// If it's a non-static method, automatically create an instance of the class
			object targetInstance = isStatic ? null : Activator.CreateInstance(tp);

			// Invoke the method with the provided arguments
			return met.Invoke(targetInstance, convertedParams).ToString();
			
			
		}
		
		/// <summary>
		/// Sets a field's value with type safety.
		/// </summary>
		/// <param name="Key">The Type Key.</param>
		/// <param name="FieldName">The Field Name.</param>
		/// <param name="Value">The Value To Set.</param>
		public static void SetField(Primitive Key, Primitive FieldName, Primitive Value)
		{
			Type targetType = _type[Key];
			FieldInfo field = GetFieldInfo(targetType, FieldName);

			if (field != null) {
				// Convert and set the value based on the field's type
				object typedValue = Convert.ChangeType(Value.ToString(), field.FieldType);
				field.SetValue(field.IsStatic ? null : Activator.CreateInstance(targetType), typedValue);
			}
		}

		/// <summary>
		/// Gets a field's value with type safety.
		/// </summary>
		/// <param name="Key">The Type Key</param>
		/// <param name="FieldName">The Field Name.</param>
		/// <returns>The Result.</returns>
		public static Primitive GetField(Primitive Key, Primitive FieldName)
		{
			Type targetType = _type[Key];
			FieldInfo field = GetFieldInfo(targetType, FieldName);

			if (field != null) {
				object fieldValue = field.GetValue(field.IsStatic ? null : Activator.CreateInstance(targetType));
				return fieldValue != null ? fieldValue.ToString() : null;
			}
			return null;
		}

		/// <summary>
		/// Retrieves the FieldInfo of a specified field.
		/// </summary>
		/// <param name="targetType">The type containing the field.</param>
		/// <param name="fieldName">The name of the field to retrieve.</param>
		/// <returns>The FieldInfo if found, otherwise null.</returns>
		private static FieldInfo GetFieldInfo(Type targetType, string fieldName)
		{
			return targetType.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
		}

		/// <summary>
		/// Gets the property value with type safety.
		/// </summary>
		/// <param name="Key">The Type Key.</param>
		/// <param name="PropertyName">The Property Name.</param>
		/// <returns>The Result.</returns>
		public static Primitive GetProperty(Primitive Key, Primitive PropertyName)
		{
			Type targetType = _type[Key];
			PropertyInfo property = GetPropertyInfo(targetType, PropertyName);

			if (property != null) {
				object propertyValue = property.GetValue(property.CanRead ? Activator.CreateInstance(targetType) : null);
				return propertyValue != null ? propertyValue.ToString() : null;
			}
			return null;
		}

		/// <summary>
		/// Sets a property value with type safety.
		/// </summary>
		/// <param name="Key">The Type Key.</param>
		/// <param name="PropertyName">The Property Name.</param>
		/// <param name="Value">The Value To Set.</param>
		public static void SetProperty(Primitive Key, Primitive PropertyName, Primitive Value)
		{
			Type targetType = _type[Key];
			PropertyInfo property = GetPropertyInfo(targetType, PropertyName);

			if (property != null && property.CanWrite) {
				// Convert and set the value based on the property type
				object typedValue = Convert.ChangeType(Value.ToString(), property.PropertyType);
				property.SetValue(property.CanWrite ? Activator.CreateInstance(targetType) : null, typedValue);
			}
		}

		/// <summary>
		/// Retrieves the PropertyInfo of a specified property.
		/// </summary>
		/// <param name="targetType">The type containing the property.</param>
		/// <param name="propertyName">The name of the property to retrieve.</param>
		/// <returns>The PropertyInfo if found, otherwise null.</returns>
		private static PropertyInfo GetPropertyInfo(Type targetType, string propertyName)
		{
			return targetType.GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
		}

		/// <summary>
		/// Retrieves details about a type based on a specified key and option.
		/// </summary>
		/// <param name="Key">The key used to identify the type from a collection.</param>
		/// <param name="Option">
		/// 1 - Assembly qualified name: The full name of the type including its namespace and assembly.
		/// 
		/// 2 - Attributes: The attributes associated with the type, represented as a string.
		/// 
		/// 3 - Full name: The fully qualified name of the type, including its namespace.
		/// 
		/// 4 - GUID: The unique identifier (GUID) associated with the type.
		/// 
		/// 5 - Is enum: Indicates whether the type is an enumeration (true/false).
		/// 
		/// 6 - Is visible: Determines if the type is visible outside its assembly (true/false).
		/// 
		/// 7 - Name: The name of the type without its namespace.
		/// 
		/// 8 - Namespace: The namespace the type belongs to.
		/// 
		/// 9 - Type handle: The runtime handle for the type, useful for low-level type operations.
		/// </param>
		/// <returns>Returns the requested detail about the specified type based on the provided option.</returns>
		public static Primitive TypeDetails(Primitive Key, Primitive Option)
		{
			Type tp = _type[Key];
			if (Option == "1") {
				return  tp.AssemblyQualifiedName;
			}
			if (Option == "2") {
				return  tp.Attributes.ToString();
			}
			if (Option == "3") {
				return  tp.FullName;
			}
			if (Option == "4") {
				return  tp.GUID.ToString();
			}
			if (Option == "5") {
				return  tp.IsEnum;
			}
			if (Option == "6") {
				return  tp.IsVisible;
				;
			}
			if (Option == "7") {
				return  tp.Name;
			}	
			if (Option == "8") {
				return  tp.Namespace;
			}
			if (Option == "9") {
				return  tp.TypeHandle.ToString();
			}
			return "Wrong Option!!";
		}
		
		/// <summary>
		/// Retrieves details about a specified method from a given type.
		/// </summary>
		/// <param name="Key">The key representing the type from which to retrieve the method.</param>
		/// <param name="Method">The name of the method to retrieve details for.</param>
		/// <param name="Option">
		/// 1 - Method Name.
		/// 2 - Method Description.
		/// 3 - Method Attributes.
		/// 4 - Parameter Types.
		/// 5 - Return Type.
		/// 6 - Method Handle.
		/// 7 - IL Code as a byte array.
		/// 8 - Maximum Stack Size.
		/// 9 - Local Variables (index and type).
		/// </param>
		/// <returns>The requested detail about the method.</returns>
		public static Primitive MethodDetail(Primitive Key, Primitive Method, Primitive Option)
		{
			MethodInfo mi = _type[Key].GetMethod(Method, BindingFlags.Public |
			                BindingFlags.NonPublic |
			                BindingFlags.Instance |
			                BindingFlags.Static);
			MethodBase mba = _type[Key].GetMethod(Method, BindingFlags.Public |
			                 BindingFlags.NonPublic |
			                 BindingFlags.Instance |
			                 BindingFlags.Static);
			MethodBody mb = mba.GetMethodBody();
			if (Option == "1") {
				return mi.Name;
			}
			if (Option == "2") {
				return mi.ToString();
			}
			if (Option == "3") {
				return mi.Attributes.ToString();
			}
			if (Option == "4") {
				ParameterInfo[] pi = mi.GetParameters();
				Primitive res = null;
				for (int i = 1; i < pi.Length; i++) {
					// Get the target type for the parameter
					Type paramType = pi[i].ParameterType;
					res[i] = paramType.ToString();
				}
				return res;
			}
			if (Option == "5") {
				return mi.ReturnType.ToString();
			}
			if (Option == "6") {
				return mba.MethodHandle.ToString();
			}
			if (Option == "7") {
				byte[] ilbytes = mb.GetILAsByteArray();
				return BitConverter.ToString(ilbytes);
			}
			if (Option == "8") {
				return mb.MaxStackSize;
			}
			if (Option == "9") {
				Primitive res = null;
				Primitive i = 1;
				foreach (var localVariable in mb.LocalVariables) {
					res[i] = localVariable.ToString();
				}
				return res;
			}
			return "Invalid Option!!";
		}
		
	

		/// <summary>
		/// Retrieves the value of a field from the specified type.
		/// </summary>
		/// <param name="targetType">The type containing the field.</param>
		/// <param name="fieldName">The name of the field to retrieve.</param>
		/// <returns>The value of the specified field, or null if the field is not found.</returns>
		[HideFromIntellisense]
		public static object GetFieldValue(Type targetType, string fieldName)
		{
			// Retrieve the field with all possible binding flags (static, non-static, public, non-public)
			FieldInfo field = targetType.GetField(fieldName, BindingFlags.NonPublic |
			                  BindingFlags.Public |
			                  BindingFlags.Instance |
			                  BindingFlags.Static);

			if (field != null) {
				if (field.IsStatic) {
					// For static fields, return the value with no instance needed
					return field.GetValue(null);
				} else {
					// Create an instance if it's a non-static field and instance isn't provided
					object instance = Activator.CreateInstance(targetType);
					return field.GetValue(instance);
				}
			}

			return null; // Return null if the field is not found
		}

		/// <summary>
		/// Sets the value of a field in the specified type.
		/// </summary>
		/// <param name="targetType">The type containing the field.</param>
		/// <param name="fieldName">The name of the field to set.</param>
		/// <param name="value">The new value for the field.</param>
		[HideFromIntellisense]
		public static void SetFieldValue(Type targetType, string fieldName, object value)
		{
			// Retrieve the field with all possible binding flags (static, non-static, public, non-public)
			FieldInfo field = targetType.GetField(fieldName, BindingFlags.NonPublic |
			                  BindingFlags.Public |
			                  BindingFlags.Instance |
			                  BindingFlags.Static);

			if (field != null) {
				if (field.IsStatic) {
					// For static fields, set the value with no instance needed
					field.SetValue(null, value);
				} else {
					// Create an instance if it's a non-static field and instance isn't provided
					object instance = Activator.CreateInstance(targetType);
					field.SetValue(instance, value);
				}
			}
		}
		
		/// <summary>
		/// Invokes a method from the specified type.
		/// </summary>
		/// <param name="targetType">The type containing the method.</param>
		/// <param name="methodName">The name of the method to invoke.</param>
		/// <param name="methodArgs">The arguments to pass to the method.</param>
		/// <returns>The result of the method invocation, or null if the method is void or not found.</returns>
		[HideFromIntellisense]
		public static object InvokeMethod(Type targetType, string methodName, params object[] methodArgs)
		{
			// Get the method info (handles public, private, internal, static, and instance methods)
			MethodInfo method = targetType.GetMethod(methodName, BindingFlags.Public |
			                    BindingFlags.NonPublic |
			                    BindingFlags.Instance |
			                    BindingFlags.Static);

			if (method != null) {
				// Check if the method is static
				bool isStatic = method.IsStatic;

				// If it's a non-static method, automatically create an instance of the class
				object targetInstance = isStatic ? null : Activator.CreateInstance(targetType);

				// Invoke the method with the provided arguments
				return method.Invoke(targetInstance, methodArgs);
			}

			// Throw an exception if the method is not found
			throw new MissingMethodException("Method " + methodName + "not found in type " + targetType.FullName);
		}
	}
}
