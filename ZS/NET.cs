using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Collections.Generic;
using Microsoft.SmallBasic.Library;
using ZS;

namespace ZS
{
	/// <summary>
	/// Some Powerful Net Function In Small Basic.
	/// Keywords And Operators.
	/// </summary>
	[SmallBasicType]
	public static class ZSNET
	{
		private static Dictionary<string, Dictionary<string, object>> SwitchStorage = new Dictionary<string, Dictionary<string, object>>();
		
		/// <summary>
		/// Converts True To False And Vice-Versa.
		/// </summary>
		/// <param name="Bool">The Bool True Or False</param>
		/// <returns>Negated Bool as True or False</returns>
		public static Primitive Not(Primitive Bool)
		{
			return !Convert.ToBoolean(Bool.ToString());
		}

		// Logical Operators
		/// <summary>
		/// Performs logical AND on two booleans.
		/// </summary>
		/// <param name="bool1">First boolean</param>
		/// <param name="bool2">Second boolean</param>
		/// <returns>True if both are true, otherwise false</returns>
		public static Primitive And(Primitive bool1, Primitive bool2)
		{
			return Convert.ToBoolean(bool1.ToString()) && Convert.ToBoolean(bool2.ToString());
		}

		/// <summary>
		/// Performs logical OR on two booleans.
		/// </summary>
		/// <param name="bool1">First boolean</param>
		/// <param name="bool2">Second boolean</param>
		/// <returns>True if either or both are true, otherwise false</returns>
		public static Primitive Or(Primitive bool1, Primitive bool2)
		{
			return Convert.ToBoolean(bool1.ToString()) || Convert.ToBoolean(bool2.ToString());
		}

		/// <summary>
		/// Performs logical XOR on two booleans.
		/// </summary>
		/// <param name="bool1">First boolean</param>
		/// <param name="bool2">Second boolean</param>
		/// <returns>True if one is true and the other is false</returns>
		public static Primitive Xor(Primitive bool1, Primitive bool2)
		{
			return Convert.ToBoolean(bool1.ToString()) ^ Convert.ToBoolean(bool2.ToString());
		}

		// Comparison Operators
		/// <summary>
		/// Checks if two values are equal.
		/// </summary>
		/// <param name="val1">First value</param>
		/// <param name="val2">Second value</param>
		/// <returns>True if equal, otherwise false</returns>
		public static Primitive Equal(Primitive val1, Primitive val2)
		{
			return val1 == val2;
		}

		/// <summary>
		/// Checks if two values are not equal.
		/// </summary>
		/// <param name="val1">First value</param>
		/// <param name="val2">Second value</param>
		/// <returns>True if not equal, otherwise false</returns>
		public static Primitive NotEqual(Primitive val1, Primitive val2)
		{
			return val1 != val2;
		}

		/// <summary>
		/// Checks if the first value is greater than the second value.
		/// </summary>
		/// <param name="num1">First number</param>
		/// <param name="num2">Second number</param>
		/// <returns>True if num1 is greater than num2</returns>
		public static Primitive GreaterThan(Primitive num1, Primitive num2)
		{
			return num1 > num2;
		}

		/// <summary>
		/// Checks if the first value is less than the second value.
		/// </summary>
		/// <param name="num1">First number</param>
		/// <param name="num2">Second number</param>
		/// <returns>True if num1 is less than num2</returns>
		public static Primitive LessThan(Primitive num1, Primitive num2)
		{
			return num1 < num2;
		}

		/// <summary>
		/// Checks if the first value is greater than or equal to the second value.
		/// </summary>
		/// <param name="num1">First number</param>
		/// <param name="num2">Second number</param>
		/// <returns>True if num1 is greater than or equal to num2</returns>
		public static Primitive GreaterThanOrEqual(Primitive num1, Primitive num2)
		{
			return num1 >= num2;
		}

		/// <summary>
		/// Checks if the first value is less than or equal to the second value.
		/// </summary>
		/// <param name="num1">First number</param>
		/// <param name="num2">Second number</param>
		/// <returns>True if num1 is less than or equal to num2</returns>
		public static Primitive LessThanOrEqual(Primitive num1, Primitive num2)
		{
			return num1 <= num2;
		}

		// Arithmetic Operators
		/// <summary>
		/// Adds a value to a number.
		/// </summary>
		/// <param name="num">Number</param>
		/// <param name="addValue">Value to add</param>
		/// <returns>Sum of num and addValue</returns>
		public static Primitive Add(Primitive num, Primitive addValue)
		{
			return num + addValue;
		}

		/// <summary>
		/// Subtracts a value from a number.
		/// </summary>
		/// <param name="num">Number</param>
		/// <param name="subValue">Value to subtract</param>
		/// <returns>Difference of num and subValue</returns>
		public static Primitive Subtract(Primitive num, Primitive subValue)
		{
			return num - subValue;
		}

		/// <summary>
		/// Multiplies a number by another value.
		/// </summary>
		/// <param name="num">Number</param>
		/// <param name="mulValue">Value to multiply by</param>
		/// <returns>Product of num and mulValue</returns>
		public static Primitive Multiply(Primitive num, Primitive mulValue)
		{
			return num * mulValue;
		}

		/// <summary>
		/// Divides a number by another value.
		/// </summary>
		/// <param name="num">Number</param>
		/// <param name="divValue">Value to divide by</param>
		/// <returns>Quotient of num and divValue</returns>
		public static Primitive Divide(Primitive num, Primitive divValue)
		{
			return num / divValue;
		}

		// Bitwise Operators
		/// <summary>
		/// Performs bitwise AND on two integers.
		/// </summary>
		/// <param name="int1">First integer</param>
		/// <param name="int2">Second integer</param>
		/// <returns>Result of bitwise AND</returns>
		public static Primitive BitwiseAnd(Primitive int1, Primitive int2)
		{
			return Convert.ToInt32(int1.ToString()) & Convert.ToInt32(int2.ToString());
		}

		/// <summary>
		/// Performs bitwise OR on two integers.
		/// </summary>
		/// <param name="int1">First integer</param>
		/// <param name="int2">Second integer</param>
		/// <returns>Result of bitwise OR</returns>
		public static Primitive BitwiseOr(Primitive int1, Primitive int2)
		{
			return Convert.ToInt32(int1.ToString()) | Convert.ToInt32(int2.ToString());
		}

		/// <summary>
		/// Performs bitwise XOR on two integers.
		/// </summary>
		/// <param name="int1">First integer</param>
		/// <param name="int2">Second integer</param>
		/// <returns>Result of bitwise XOR</returns>
		public static Primitive BitwiseXor(Primitive int1, Primitive int2)
		{
			return Convert.ToInt32(int1.ToString()) ^ Convert.ToInt32(int2.ToString());
		}

		/// <summary>
		/// Performs bitwise NOT on an integer.
		/// </summary>
		/// <param name="intVal">Integer to negate</param>
		/// <returns>Bitwise negation of the integer</returns>
		public static Primitive BitwiseNot(Primitive intVal)
		{
			return ~Convert.ToInt32(intVal.ToString());
		}

		// Shift Operators
		/// <summary>
		/// Shifts the bits of a number to the left by the specified amount.
		/// </summary>
		/// <param name="num">Number</param>
		/// <param name="shiftAmount">Amount to shift</param>
		/// <returns>Number shifted to the left</returns>
		public static Primitive LeftShift(Primitive num, Primitive shiftAmount)
		{
			return Convert.ToInt32(num.ToString()) << Convert.ToInt32(shiftAmount.ToString());
		}

		/// <summary>
		/// Shifts the bits of a number to the right by the specified amount.
		/// </summary>
		/// <param name="num">Number</param>
		/// <param name="shiftAmount">Amount to shift</param>
		/// <returns>Number shifted to the right</returns>
		public static Primitive RightShift(Primitive num, Primitive shiftAmount)
		{
			return Convert.ToInt32(num.ToString()) >> Convert.ToInt32(shiftAmount.ToString());
		}
        
		/// <summary>
		/// Inline if (ternary operator) - evaluates a mathematical or boolean expression and returns one value if true, otherwise another value.
		/// </summary>
		/// <param name="expression">Mathematical or boolean expression to evaluate (e.g., "5 > 2")</param>
		/// <param name="trueValue">Value returned if the expression is true</param>
		/// <param name="falseValue">Value returned if the expression is false</param>
		/// <returns>The trueValue if the expression is true, otherwise falseValue</returns>
		public static Primitive InlineIf(Primitive expression, Primitive trueValue, Primitive falseValue)
		{
			try {
				// Create a DataTable to evaluate the expression
				System.Data.DataTable table = new System.Data.DataTable();

				// Use Compute to evaluate the string expression (compatible with .NET 4.5)
				object result = table.Compute(expression.ToString(), null);

				// Check if the result is a boolean and return trueValue or falseValue accordingly
				if (result is bool) {
					return (bool)result ? trueValue : falseValue;
				}

				// If result is a number, consider non-zero as true and zero as false
				double numericResult;
				if (double.TryParse(result.ToString(), out numericResult)) {
					return numericResult != 0 ? trueValue : falseValue;
				}

				// If none of the above, return falseValue (fallback)
				return falseValue;
			} catch {
				// In case of errors (e.g., invalid expression), return falseValue
				return falseValue;
			}
		}




		/// <summary>
		/// Returns the first non-null value.
		/// </summary>
		/// <param name="value1">First value</param>
		/// <param name="value2">Second value</param>
		/// <returns>First non-null value</returns>
		public static Primitive Coalesce(Primitive value1, Primitive value2)
		{
			return !string.IsNullOrEmpty(value1.ToString()) ? value1 : value2;
		}

		/// <summary>
		/// Negates the number (changes the sign).
		/// </summary>
		/// <param name="num">The number to negate</param>
		/// <returns>The negated number</returns>
		public static Primitive Negate(Primitive num)
		{
			return -num;
		}

		/// <summary>
		/// Returns the remainder of the division between two numbers.
		/// </summary>
		/// <param name="num">The number to divide</param>
		/// <param name="divisor">The number to divide by</param>
		/// <returns>The remainder of the division</returns>
		public static Primitive Modulus(Primitive num, Primitive divisor)
		{
			return num % divisor;
		}

		/// <summary>
		/// Checks if a number is within a specified range (inclusive).
		/// </summary>
		/// <param name="num">The number to check</param>
		/// <param name="min">The lower bound of the range</param>
		/// <param name="max">The upper bound of the range</param>
		/// <returns>True if the number is within the range, otherwise false</returns>
		public static Primitive IsInRange(Primitive num, Primitive min, Primitive max)
		{
			double number = num;
			return number >= min && number <= max;
		}

		/// <summary>
		/// Clamps a number between a minimum and maximum value.
		/// </summary>
		/// <param name="num">The number to clamp</param>
		/// <param name="min">The minimum allowed value</param>
		/// <param name="max">The maximum allowed value</param>
		/// <returns>The clamped value</returns>
		public static Primitive Clamp(Primitive num, Primitive min, Primitive max)
		{
			double number = num;
			return System.Math.Max(min, System.Math.Min(max, number));
		}

		
		/// <summary>
		/// Interpolates values into a string template.
		/// </summary>
		/// <param name="template">The string template with placeholders (e.g., "Hello {0}")</param>
		/// <param name="args">The values to replace the placeholders</param>
		/// <returns>The interpolated string</returns>
		public static Primitive Interpolate(Primitive template, params Primitive[] args)
		{
			string[] argStrings = new string[args.Length];
			for (int i = 0; i < args.Length; i++) {
				argStrings[i] = args[i].ToString();
			}
			return string.Format(template.ToString(), argStrings);
		}

		/// <summary>
		/// Repeats an action for the specified number of iterations.
		/// </summary>
		/// <param name="iterations">Number of times to repeat</param>
		/// <param name="Sub">The action to repeat</param>
		/// <returns>True after completion</returns>
		public static Primitive Repeat(Primitive iterations, Primitive Sub)
		{
			for (int i = 0; i < iterations; i++) {
				ZSTest.FireSub(Sub);
			}
			return true;
		}

		/// <summary>
		/// Applies an operation or function to each element in a comma-separated list of values.
		/// </summary>
		/// <param name="list">A comma-separated list of values</param>
		/// <param name="operation">The operation to apply (e.g., "+1" to increment all values)</param>
		/// <returns>A new list of results with the operation applied</returns>
		public static Primitive Apply(Primitive list, Primitive operation)
		{
			string[] items = list.ToString().Split(',');
			List<Primitive> result = new List<Primitive>();

			foreach (string item in items) {
				string expression = item + operation.ToString();
				result.Add(Eval(expression));
			}

			return string.Join(",", result);
		}

		/// <summary>
		/// Filters a list of values based on a condition.
		/// </summary>
		/// <param name="list">A comma-separated list of values</param>
		/// <param name="condition">The condition to apply (e.g., ">2" to filter values greater than 2)</param>
		/// <returns>A new list of values that meet the condition</returns>
		public static Primitive Filter(Primitive list, Primitive condition)
		{
			string[] items = list.ToString().Split(',');
			List<Primitive> result = new List<Primitive>();

			foreach (string item in items) {
				string expression = item + condition.ToString();
				if (Convert.ToBoolean(Eval(expression).ToString())) {
					result.Add(item);
				}
			}

			return string.Join(",", result);
		}

		/// <summary>
		/// Reduces a list of values to a single value by applying an operation (e.g., summing all values).
		/// </summary>
		/// <param name="list">A comma-separated list of values</param>
		/// <param name="operation">The operation to apply (e.g., "+" to sum values)</param>
		/// <returns>The reduced result (e.g., sum of the list)</returns>
		public static Primitive Reduce(Primitive list, Primitive operation)
		{
			string[] items = list.ToString().Split(',');
			Primitive result = items[0];

			for (int i = 1; i < items.Length; i++) {
				string expression = result + operation.ToString() + items[i];
				result = Eval(expression);
			}

			return result;
		}

		/// <summary>
		/// Combines two lists element-wise using a specified operation.
		/// </summary>
		/// <param name="list1">First comma-separated list</param>
		/// <param name="list2">Second comma-separated list</param>
		/// <param name="operation">Operation to apply, e.g., "+" to add elements</param>
		/// <returns>A new list with the operation applied to each pair of elements</returns>
		public static Primitive ZipWith(Primitive list1, Primitive list2, Primitive operation)
		{
			string[] items1 = list1.ToString().Split(',');
			string[] items2 = list2.ToString().Split(',');
			List<Primitive> result = new List<Primitive>();

			for (int i = 0; i < System.Math.Min(items1.Length, items2.Length); i++) {
				string expression = items1[i] + operation.ToString() + items2[i];
				result.Add(Eval(expression));
			}

			return string.Join(",", result);
		}

		/// <summary>
		/// Applies an operation to each item in a list (e.g., increment all items by 1).
		/// </summary>
		/// <param name="list">A comma-separated list of values</param>
		/// <param name="operation">The operation to apply to each item (e.g., "+1")</param>
		/// <returns>A new list with the operation applied</returns>
		public static Primitive ForEach(Primitive list, Primitive operation)
		{
			return Apply(list, operation);
		}

		/// <summary>
		/// Chains multiple operations to apply to a single value.
		/// </summary>
		/// <param name="value">The starting value</param>
		/// <param name="operations">A comma-separated list of operations to chain</param>
		/// <returns>The result after all operations are applied</returns>
		public static Primitive Chain(Primitive value, Primitive operations)
		{
			string[] ops = operations.ToString().Split(',');
			Primitive result = value;

			foreach (string operation in ops) {
				string expression = result + operation;
				result = Eval(expression);
			}

			return result;
		}

		/// <summary>
		/// Helper method to evaluate simple arithmetic expressions.
		/// </summary>
		/// <param name="expression">The expression to evaluate</param>
		/// <returns>The result of the evaluation</returns>
		public static Primitive Eval(string expression)
		{
			DataTable table = new DataTable();
			object result = table.Compute(expression, "");
			return Convert.ToDouble(result);
		}
    

		/// <summary>
		/// The Current I Of Loop
		/// </summary>
		public static Primitive I;
		// Public variable to hold the current loop iteration value

		/// <summary>
		/// Executes a complex 'for' loop with a starting value, condition, and increment or decrement option.
		/// </summary>
		/// <param name="startValue">The starting value for the loop</param>
		/// <param name="condition">The condition to continue the loop (e.g., "I < 10")</param>
		/// <param name="increment">The increment or decrement to apply each iteration (e.g., "+1" or "-1")</param>
		/// <param name="subName">The Small Basic subroutine to call during each iteration</param>
		public static void For(Primitive startValue, Primitive condition, Primitive increment, Primitive subName)
		{
			try {
				// Initialize the loop variable
				I = startValue;
				int currentValue = Convert.ToInt32(I.ToString());
				string loopCondition = condition.ToString();
				string loopIncrement = increment.ToString();
        
				// Define DataTable to evaluate expressions
				System.Data.DataTable table = new System.Data.DataTable();

				// Loop until the condition evaluates to false
				while (Convert.ToBoolean(table.Compute(loopCondition.Replace("I", currentValue.ToString()), null))) {
					// Update the global 'I' variable with the current loop value
					I = currentValue;

					// Fire the Small Basic subroutine
					ZSTest.FireSub(subName);

					// Modify the loop variable based on the increment provided (e.g., "+1", "-1", "*2", "/2")
					string newValueExpression = currentValue.ToString() + loopIncrement;
					currentValue = Convert.ToInt32(table.Compute(newValueExpression, null));

					// Update the condition with the new value of 'I'
					loopCondition = condition.ToString().Replace("I", currentValue.ToString());
				}
			} catch (Exception ex) {
				// Handle any exceptions that occur during loop execution
				TextWindow.WriteLine("Error in complex For loop: " + ex.Message);
			}
		}


		// Dictionary to store switch cases, keyed by name
		private static Dictionary<string, Dictionary<Primitive, Primitive>> switches = new Dictionary<string, Dictionary<Primitive, Primitive>>();

		// Dictionary to store default values for each switch
		private static Dictionary<string, Primitive> defaultValues = new Dictionary<string, Primitive>();

		/// <summary>
		/// Creates a switch case with a specified name and returns the name for later reference.
		/// The cases are provided in the format: "1-one,2-two", where "1" is the case, "one" is the result.
		/// </summary>
		/// <param name="switchName">The name of the switch</param>
		/// <param name="cases">A comma-separated list of cases and results (e.g., "1-one,2-two")</param>
		/// <param name="defaultValue">The default value to return if no match is found</param>
		/// <returns>The switch name for reference</returns>
		public static Primitive CreateSwitch(Primitive switchName, Primitive cases, Primitive defaultValue)
		{
			// Split the cases into a list of case-result pairs
			string[] casePairs = cases.ToString().Split(',');

			// Create a dictionary to hold the case-result pairs
			Dictionary<Primitive, Primitive> caseDict = new Dictionary<Primitive, Primitive>();

			// Add the cases to the dictionary (splitting each case from its result by '-')
			foreach (string casePair in casePairs) {
				string[] parts = casePair.Split('-');
				if (parts.Length == 2) {
					caseDict.Add(parts[0], parts[1]);
				}
			}

			// Store the switch in the global dictionary using the switchName
			switches[switchName.ToString()] = caseDict;

			// Store the default value for this switch
			defaultValues[switchName.ToString()] = defaultValue;

			// Return the name of the switch for future reference
			return switchName;
		}

		/// <summary>
		/// Calls a previously created switch by name with an argument and returns the corresponding result.
		/// If no match is found, the default value is returned.
		/// </summary>
		/// <param name="switchName">The name of the switch to call</param>
		/// <param name="arg">The argument to pass to the switch (e.g., "1" to return the value for case 1)</param>
		/// <returns>The result of the switch case, or the default value if no match is found</returns>
		public static Primitive CallSwitch(Primitive switchName, Primitive arg)
		{
			// Check if the switch exists
			if (switches.ContainsKey(switchName.ToString())) {
				Dictionary<Primitive, Primitive> caseDict = switches[switchName.ToString()];

				// Check if the argument matches a case, and return the corresponding result
				if (caseDict.ContainsKey(arg)) {
					return caseDict[arg];
				}

				// Return the default value if no match is found
				if (defaultValues.ContainsKey(switchName.ToString())) {
					return defaultValues[switchName.ToString()];
				}
			}

			// If the switch doesn't exist or no default value is available, return a fallback default
			return "Default";
		}

		/// <summary>
		/// Fires a subroutine for each element in a Small Basic array.
		/// </summary>
		/// <param name="array">The Small Basic array</param>
		/// <param name="subName">The name of the subroutine to fire for each element</param>
		public static void ForEachInArray(Primitive[] array, Primitive subName)
		{
			// Iterate over the array elements
			foreach (var element in array) {
				// Set the current value of ZSCS.I to the current array element
				ZSNET.I = element;

				// Fire the subroutine with the current element
				ZSTest.FireSub(subName);
			}
		}

		/// <summary>
		/// Fires a subroutine for each element in a comma-separated list.
		/// </summary>
		/// <param name="list">The comma-separated list</param>
		/// <param name="subName">The name of the subroutine to fire for each element</param>
		public static void ForEachInList(Primitive list, Primitive subName)
		{
			// Split the comma-separated list into elements
			string[] elements = list.ToString().Split(',');

			// Iterate over each element
			for (int i = 0; i < elements.Length; i++) {
				// Set the current value of ZSCS.I
				ZSNET.I = new Primitive(elements[i].Trim());

				// Fire the subroutine for each element
				ZSTest.FireSub(subName);
			}
		}
		
		/// <summary>
		/// Creates a switch with a specified name, an array of cases, an array of results, and a default result.
		/// </summary>
		/// <param name="switchName">The name of the switch to be created</param>
		/// <param name="cases">Array of case values</param>
		/// <param name="results">Array of result values corresponding to each case</param>
		/// <param name="defaultResult">The default result if no case matches</param>
		/// <returns>The name of the switch created</returns>
		public static Primitive Switch(Primitive switchName, Primitive[] cases, Primitive[] results, Primitive defaultResult)
		{
			// Create a dictionary to store the switch cases and results
			Dictionary<string, object> switchData = new Dictionary<string, object>();
    
			for (int i = 0; i < cases.Length; i++) {
				// Store the case-result pairs
				switchData[cases[i].ToString()] = results[i];
			}

			// Store the default result under a specific key
			switchData["default"] = defaultResult;

			// Store the switch data globally (e.g., in a dictionary of switches)
			if (!SwitchStorage.ContainsKey(switchName.ToString())) {
				SwitchStorage[switchName.ToString()] = switchData;
			}

			// Return the name of the switch created
			return switchName;
		}
		
		/// <summary>
		/// Uses a previously created switch by name and returns the result based on the input value.
		/// </summary>
		/// <param name="switchName">The name of the switch to be used</param>
		/// <param name="value">The value to be checked against the cases in the switch</param>
		/// <returns>The result corresponding to the matched case, or the default result if no match is found</returns>
		public static Primitive UseSwitch(Primitive switchName, Primitive value)
		{
			// Check if the switch with the given name exists
			if (SwitchStorage.ContainsKey(switchName.ToString())) {
				var switchData = SwitchStorage[switchName.ToString()];

				// Check if the value matches any of the cases
				if (switchData.ContainsKey(value.ToString())) {
					// Return the corresponding result for the matched case
					return (Primitive)switchData[value.ToString()];
				} else {
					// Return the default result if no match is found
					return (Primitive)switchData["default"];
				}
			}

			// If the switch doesn't exist, return an empty Primitive or error
			return new Primitive("");
		}

		/// <summary>
		/// Evaluates a complex mathematical or logical expression, supporting additional operators like ++, --, <<, >>, ? : (ternary), ?? (null-coalescing).
		/// </summary>
		/// <param name="expression">The expression to evaluate</param>
		/// <returns>The result of the evaluated expression</returns>
		public static Primitive EvaluateExpression(Primitive expression)
		{
			try {
				string expr = expression.ToString();

				// Handle Increment (++)
				expr = Regex.Replace(expr, @"(\w+)\s*\+\+", match => {
					string variable = match.Groups[1].Value;
					// Simulate ++ by increasing the value by 1
					return string.Format("{0} = {0} + 1", variable);
				});

				// Handle Decrement (--)
				expr = Regex.Replace(expr, @"(\w+)\s*--", match => {
					string variable = match.Groups[1].Value;
					// Simulate -- by decreasing the value by 1
					return string.Format("{0} = {0} - 1", variable);
				});

				// Handle Left Shift (<<) and Right Shift (>>)
				expr = Regex.Replace(expr, @"(\d+)\s*<<\s*(\d+)", match => {
					int left = int.Parse(match.Groups[1].Value);
					int right = int.Parse(match.Groups[2].Value);
					return (left << right).ToString();
				});

				expr = Regex.Replace(expr, @"(\d+)\s*>>\s*(\d+)", match => {
					int left = int.Parse(match.Groups[1].Value);
					int right = int.Parse(match.Groups[2].Value);
					return (left >> right).ToString();
				});

				// Handle Ternary (condition ? trueValue : falseValue)
				expr = Regex.Replace(expr, @"(.+?)\s*\?\s*(.+?)\s*:\s*(.+)", match => {
					string condition = match.Groups[1].Value;
					string trueValue = match.Groups[2].Value;
					string falseValue = match.Groups[3].Value;

					// Evaluate the condition
					bool condResult = Convert.ToBoolean(new DataTable().Compute(condition, ""));
					return condResult ? trueValue : falseValue;
				});

				// Handle Null-Coalescing (??)
				expr = Regex.Replace(expr, @"(\w+)\s*\?\?\s*(\w+)", match => {
					string left = match.Groups[1].Value;
					string right = match.Groups[2].Value;

					return !string.IsNullOrEmpty(left) ? left : right;
				});

				// Use DataTable to compute the final expression after modifications
				object result = new DataTable().Compute(expr, "");

				return new Primitive(result.ToString());
			} catch (Exception ex) {
				// Handle errors gracefully and return the error message
				return new Primitive("Error: " + ex.Message);
			}
		}
		
		/// <summary>
		/// Converts an integer value to its binary representation as a string.
		/// </summary>
		/// <param name="value">The decimal number to convert to binary.</param>
		/// <returns>A binary string representation of the decimal number.</returns>
		public static Primitive ToBinary(Primitive value)
		{
			long decimalNumber = (long)value;
			return Convert.ToString(decimalNumber, 2); // Convert to binary
		}

		/// <summary>
		/// Converts a binary string to its decimal representation.
		/// </summary>
		/// <param name="binary">A binary string to convert to decimal.</param>
		/// <returns>The decimal value of the given binary string.</returns>
		public static Primitive BinaryToDecimal(Primitive binary)
		{
			string binaryString = binary;
			return Convert.ToInt64(binaryString, 2).ToString(); // Convert binary string to integer
		}

		/// <summary>
		/// Converts a text string to its binary representation using ASCII encoding.
		/// Each character is represented by 8 bits.
		/// </summary>
		/// <param name="text">The text string to convert to binary.</param>
		/// <returns>A binary string representation of the text, using ASCII encoding.</returns>
		public static Primitive TextToBinary(Primitive text)
		{
			string input = text;
			StringBuilder binaryString = new StringBuilder();
			foreach (char c in input) {
				binaryString.Append(Convert.ToString(c, 2).PadLeft(8, '0')); // Convert char to binary (ASCII)
			}
			return binaryString.ToString();
		}

		/// <summary>
		/// Converts a binary string (ASCII encoded) back to a text string.
		/// </summary>
		/// <param name="binary">The binary string (ASCII encoded) to convert back to text.</param>
		/// <returns>The original text string represented by the binary string.</returns>
		public static Primitive BinaryToText(Primitive binary)
		{
			string binaryString = binary;
			List<byte> byteList = new List<byte>();

			for (int i = 0; i < binaryString.Length; i += 8) {
				byteList.Add(Convert.ToByte(binaryString.Substring(i, 8), 2)); // Convert binary to byte
			}

			return Encoding.ASCII.GetString(byteList.ToArray()); // Convert byte array to text
		}

	}
}
