using System;
using SV = Microsoft.SmallVisualBasic.Library;
using SB = Microsoft.SmallBasic.Library;
using WI = Microsoft.SmallVisualBasic.WinForms;
using SBi = Microsoft.SmallBasic.Library.Internal;

namespace ZS
{
	
	/// <summary>
	/// sVB In SB.
	/// just for showing svb can also used in small basic.
	/// Copy SVB dll to program dir
	/// </summary>
	[SB.SmallBasicType]
	public static class ZSSVb
	{
		/// <summary>
		/// Show The GW.
		/// </summary>
		public static void GW()
		{
			Microsoft.SmallVisualBasic.Library.GraphicsWindow.Show();
			SB.GraphicsWindow.Hide();
		}
		
		/// <summary>
		/// Add A Button
		/// </summary>
		/// <param name="Caption">The Caption</param>
		/// <param name="Left">X Axis</param>
		/// <param name="Top">y Axis</param>
		/// <returns>Button Name</returns>
		public static SB.Primitive AddButton(SB.Primitive Caption, SB.Primitive Left, SB.Primitive Top)
		{
			return SV.Controls.AddButton(Caption.ToString(), Left.ToString(), Top.ToString()).ToString();
		}
		
		/// <summary>
		/// Move A Shape.
		/// </summary>
		/// <param name="Control">The Shape Name</param>
		/// <param name="x">x Axis</param>
		/// <param name="y">y Axis</param>
		public static void Move(SB.Primitive Control, SB.Primitive x, SB.Primitive y)
		{
			SV.Controls.Move(Control.ToString(), x.ToString(), y.ToString());
		}
		
	}
	
}