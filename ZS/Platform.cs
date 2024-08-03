using Microsoft.SmallBasic.Library;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ZS
{
    /// <summary>
    /// The Platform object provides a way to generically invoke other .NET libraries.
    /// This class has been taken from Small Basic version 0.2.
    /// </summary>
    [SmallBasicType]
    public static class ZSPlatform
    {
        private static Dictionary<string, int> _nameGenerationMap = new Dictionary<string, int>();
        private static Dictionary<string, object> _objectMap = new Dictionary<string, object>();

        /// <summary>
        /// Creates an instance of a specified .NET type.
        /// </summary>
        /// <param name="typeName">The fully qualified name of the type.</param>
        /// <returns>A unique identifier for the created instance or "ERROR" if creation fails.</returns>
        public static Primitive CreateInstance(Primitive typeName)
        {
            try
            {
                Type type = Type.GetType(typeName);
                object value = Activator.CreateInstance(type);
                string text = GenerateNewName("Instance");
                _objectMap[text] = value;
                return text;
            }
            catch (Exception ex)
            {
                return "ERROR" + ex.Message;
            }
        }

        /// <summary>
        /// Invokes a method on an instance of a .NET type.
        /// </summary>
        /// <param name="instanceId">The unique identifier of the instance.</param>
        /// <param name="methodName">The name of the method to invoke.</param>
        /// <param name="argumentsStackName">The name of the stack containing the method arguments.</param>
        /// <returns>The result of the method invocation or "ERROR" if invocation fails.</returns>
        public static Primitive InvokeInstanceMethod(Primitive instanceId, Primitive methodName, Primitive argumentsStackName)
        {
            try
            {
                object obj = _objectMap[instanceId.ToString()];
                MethodInfo method = obj.GetType().GetMethod(methodName.ToString(), BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
                int num = Stack.GetCount(argumentsStackName);
                ParameterInfo[] parameters = method.GetParameters();
                if (parameters.Length == num)
                {
                    object[] list = new object[num];
                    for (int num2 = num - 1; num2 >= 0; num2--)
                    {
                        list[num2] = Stack.PopValue(argumentsStackName);
                    }
                    return new Primitive(method.Invoke(obj, list));
                }
                return "ERROR";
            }
            catch (Exception ex)
            {
                return "ERROR" + ex.Message;
            }
        }

        /// <summary>
        /// Invokes a static method on a .NET type.
        /// </summary>
        /// <param name="typeName">The fully qualified name of the type.</param>
        /// <param name="methodName">The name of the method to invoke.</param>
        /// <param name="argumentsStackName">The name of the stack containing the method arguments.</param>
        /// <returns>The result of the method invocation or "ERROR" if invocation fails.</returns>
        [HideFromIntellisense]
        public static Primitive InvokeStaticMethod(Primitive typeName, Primitive methodName, Primitive argumentsStackName)
        {
            try
            {
                Type type = Type.GetType(typeName.ToString());
                MethodInfo method = type.GetMethod(methodName.ToString(), BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public);
                int num = Stack.GetCount(argumentsStackName);
                ParameterInfo[] parameters = method.GetParameters();
                if (parameters.Length == num)
                {
                    object[] list = new object[num];
                    for (int num2 = num - 1; num2 >= 0; num2--)
                    {
                        list[num2] = Stack.PopValue(argumentsStackName);
                    }
                    return new Primitive(method.Invoke(null, list));
                }
                return "ERROR";
            }
            catch (Exception ex)
            {
                return "ERROR" + ex.Message;
            }
        }

        /// <summary>
        /// Generates a new unique name with a specified prefix.
        /// </summary>
        /// <param name="prefix">The prefix for the generated name.</param>
        /// <returns>A new unique name.</returns>
        internal static string GenerateNewName(string prefix)
        {
            int num = 0;
            _nameGenerationMap.TryGetValue(prefix, out num);
            num++;
            _nameGenerationMap[prefix] = num;
            return prefix + num.ToString();
        }
    }
}
