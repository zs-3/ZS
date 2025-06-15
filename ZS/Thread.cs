using Microsoft.SmallBasic.Library;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace ZS
{
	/// <summary>
	/// Provides methods to use multiple threads in Small Basic.
	/// </summary>
	[SmallBasicType]
	public static class ZSThread
	{
		// Dictionary to store threads by their ID
		private static Dictionary<string, Thread> Threads = new Dictionary<string, Thread>();
        
		/// <summary>
		/// Runs a subroutine on a new thread.
		/// </summary>
		/// <param name="SubName">The name of the subroutine to run.</param>
		/// <returns>The ID of the created thread.</returns>
		public static Primitive RunSub(Primitive SubName)
		{
			string subroutineName = SubName.ToString();

			// Create and start the thread
			Thread thread = new Thread(() => {
				try {
					// Use reflection to invoke the Small Basic subroutine
					Func(subroutineName);
				} catch (Exception ex) {
					TextWindow.WriteLine("Exception in thread: " + ex.Message);
				}
			});

			thread.Start();
			Threads[thread.ManagedThreadId.ToString()] = thread;
			return thread.ManagedThreadId.ToString();
		}

		/// <summary>
		/// Terminates a thread by its ID.
		/// </summary>
		/// <param name="ThreadId">The ID of the thread to terminate.</param>
		public static void TerminateThread(Primitive ThreadId)
		{
			Thread thread = Threads[ThreadId.ToString()];
			thread.Abort();
		}

		/// <summary>
		/// Waits for a thread to complete.
		/// </summary>
		/// <param name="ThreadId">The ID of the thread to wait for.</param>
		public static void WaitForThread(Primitive ThreadId)
		{
			Thread thread = Threads[ThreadId.ToString()];
			thread.Join();
		}
        
		/// <summary>
		/// Gets a value indicating the execution status of the current thread.
		/// </summary>
		/// <param name="Threadid">The Thread ID</param>
		/// <returns>true if this thread has been started and has not terminated normally or aborted; otherwise, false.</returns>
		public static Primitive IsAlive(Primitive Threadid)
		{
			Thread thread = Threads[Threadid.ToString()];
			return thread.IsAlive.ToString();
		}

		/// <summary>
		/// Gets a value containing the states of the current thread.
		/// </summary>
		/// <param name="Threadid">The Thread ID</param>
		/// <returns>ne of the ThreadState values indicating the state of the current thread. The initial value is Unstarted.</returns>
		public static Primitive ThreadState(Primitive Threadid)
		{
			Thread thread = Threads[Threadid.ToString()];
			return thread.ThreadState.ToString();
		}
        
		private static Assembly entryAssembly = Assembly.GetEntryAssembly();
		private static Type mainModule = entryAssembly.EntryPoint.DeclaringType;
		private static void Func(string funcName)
		{
			MethodInfo methodInfo = mainModule.GetMethod(funcName, BindingFlags.IgnoreCase | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
			methodInfo.Invoke(null, null);
            
		}
	}
}
