using System;
using Microsoft.SmallBasic.Library;
using System.Reflection;

namespace ZS
{
	/// <summary>
	/// Just For Testing.
	/// </summary>
	[SmallBasicType]
	public static class ZSTest
	{
		
		
		private static string _subname = null;
		private static Assembly entryAssembly = Assembly.GetEntryAssembly();
		private static Type mainModule = entryAssembly.EntryPoint.DeclaringType;
		
		/// <summary>
		/// This Event Can Be Fired From Method ZSTest.Fire
		/// </summary>
		public static event SmallBasicCallback FireEvent;

		/// <summary>
		/// Fires The Event.
		/// </summary>
		public static void Fire()
		{
			FireEvent.Invoke();
		}
		
	
		
		
		/// <summary>
		/// Fires A Sub.
		/// </summary>
		/// <param name="SubName">The Sub Name</param>
		public  static void FireSub(Primitive SubName)
		{
			MethodInfo methodInfo = mainModule.GetMethod(SubName.ToString(), BindingFlags.IgnoreCase | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
			methodInfo.Invoke(null, null);
            
		}
		
		/// <summary>
		/// Converts True To False And Vice-Versa.
		/// </summary>
		/// <param name="Bool">The Bool True Or False</param>
		/// <returns>Ops String</returns>
		public static Primitive Ops(Primitive Bool)
		{
			return !Convert.ToBoolean(Bool.ToString());
		}
		
		/// <summary>
		/// Return A Array Of All Variables In This App.
		/// </summary>
		public static Primitive Variables()
		{
			Primitive res = null;
			Primitive i = 1;
			foreach (var field in mainModule.GetFields(BindingFlags.NonPublic | BindingFlags.Static)) {
				res[i] = "Variable Name : " + field.Name + "Variable Value" + field.GetValue(null);
			}
			return res;
		}
		
		
	}
	
}