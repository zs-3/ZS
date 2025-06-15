using Microsoft.SmallBasic.Library;
using System;

namespace ZS
{

	/// <summary>
	/// The Arguments object provides operations to access the command-line arguments that were passed at the start of this program.
	/// This class has been taken from Small Basic version 0.2.
	/// </summary>
	[SmallBasicType]
	public static class ZSArguments
	{
		private static string[] args = Environment.GetCommandLineArgs();

		/// <summary>
		/// Gets the number of command-line arguments passed to this program.
		/// </summary>
		public static Primitive Count {
			get { return (args.Length != 0) ? (args.Length - 1) : 0; }
		}

		/// <summary>
		/// Returns the specified argument.
		/// </summary>
		/// <param name="index">
		/// Index of the argument.
		/// </param>
		/// <returns>
		/// The command-line argument at the specified index.
		/// </returns>
		public static Primitive GetArgument(Primitive index)
		{
			int num = index;
			if (num >= 1 && num < args.Length) {
				return new Primitive(args[num]);
			}
			return new Primitive("");
		}
	}
}