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
		
		
		
		
		
		
		
	}
	
}