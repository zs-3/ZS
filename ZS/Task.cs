using Microsoft.SmallBasic.Library;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace ZS
{
    /// <summary>
    /// Provides methods to use tasks in Small Basic.
    /// </summary>
    [SmallBasicType]
    public static class ZSTask
    {
        // Dictionary to store tasks by their ID
        private static Dictionary<string, Task> Tasks = new Dictionary<string, Task>();

        /// <summary>
        /// Runs a subroutine as a new task.
        /// </summary>
        /// <param name="SubName">The name of the subroutine to run.</param>
        /// <returns>The ID of the created task.</returns>
        public static Primitive RunSub(Primitive SubName)
        {
            string subroutineName = SubName.ToString();

            // Create and start the task
            Task task = new Task(() =>
            {
                try
                {
                    // Use reflection to invoke the Small Basic subroutine
                    Func(subroutineName);
                }
                catch (Exception ex)
                {
                    TextWindow.WriteLine("Exception in task: " + ex.Message);
                }
            });

            task.Start();
            string taskId = task.Id.ToString();
            Tasks[taskId] = task;
            return taskId;
        }
        
        /// <summary>
        /// Get the status of a task
        /// </summary>
        /// <param name="TaskID">The ID of the task.</param>
        /// <returns>The status of task.</returns>
        public static Primitive Status(Primitive TaskID)
        {
        	return Tasks[TaskID.ToString()].Status.ToString();
        }

        /// <summary>
        /// Waits for a task to complete.
        /// </summary>
        /// <param name="TaskID">The ID of the task to wait for.</param>
        public static void WaitForTask(Primitive TaskID)
        {
            string taskId = TaskID.ToString();
            if (Tasks.ContainsKey(taskId))
            {
                Tasks[taskId].Wait();
            }
            else
            {
                TextWindow.WriteLine("Task not found: " + taskId);
            }
        }

        /// <summary>
        /// Waits for a task to complete.
        /// </summary>
        /// <param name="TaskID">The ID of the task to wait for.</param>
        /// <param name="Time">The time in milisecond to wait for.</param>
        public static void WaitForTask(Primitive TaskID,Primitive Time)
        {
            string taskId = TaskID.ToString();
            if (Tasks.ContainsKey(taskId))
            {
            	Tasks[taskId].Wait(int.Parse(Time.ToString()));
            }
            else
            {
                TextWindow.WriteLine("Task not found: " + taskId);
            }
        }
        
        
        
        /// <summary>
        /// Checks if a task is still running.
        /// </summary>
        /// <param name="TaskID">The ID of the task to check.</param>
        /// <returns>True if the task is running, false otherwise.</returns>
        public static Primitive IsRunning(Primitive TaskID)
        {
            string taskId = TaskID.ToString();
            if (Tasks.ContainsKey(taskId))
            {
                return (!Tasks[taskId].IsCompleted).ToString();
            }
            else
            {
                TextWindow.WriteLine("Task not found: " + taskId);
                return "False";
            }
        }

        // Reflection-related methods for invoking Small Basic subroutines
        private static Assembly entryAssembly = Assembly.GetEntryAssembly();
        private static Type mainModule = entryAssembly.EntryPoint.DeclaringType;

        private static void Func(string funcName)
        {
            MethodInfo methodInfo = mainModule.GetMethod(funcName, BindingFlags.IgnoreCase | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            if (methodInfo != null)
            {
                methodInfo.Invoke(null, null);
            }
            else
            {
                TextWindow.WriteLine("Subroutine not found: " + funcName);
            }
        }
    }
}
