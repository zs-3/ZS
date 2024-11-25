using Microsoft.SmallBasic.Library;
using System.Windows.Forms;
using System;
using System.Threading;
using System.Reflection;
using System.Collections.Generic;
using System.Windows;


namespace ZS
{
	/// <summary>
	/// WinFrom In Small Basic.
	/// </summary>
	[SmallBasicType]
	public static class ZSForm
	{
		
		private static Dictionary<string, Form> forms = new Dictionary<string, Form>();
       
		
		/// <summary>
		/// Creates A New Form.
		/// </summary>
		/// <param name="Name">The Name To Store Form</param>
		/// <param name="Title">The Title Of Form</param>
		/// <param name="Width">The Width</param>
		/// <param name="Height">The Height</param>
		public static void CreateForm(Primitive Name, Primitive Title, Primitive Width, Primitive Height)
		{
			Form form = new Form();
			form.Text = Name;
			form.Width = int.Parse(Width);
			form.Height = int.Parse(Height);
			forms[Name.ToString()] = form;
			
		}
		
		/// <summary>
		/// Make A Form As Child Of A Form.
		/// </summary>
		/// <param name="Parent">The Main Form Name.</param>
		/// <param name="Child">The Child Form Name.</param>
		public static void MakeChild(Primitive Parent, Primitive Child)
		{
			Form form = forms[Parent.ToString()];
			Form child = forms[Child.ToString()];
			child.MdiParent = form;
		}
		
		/// <summary>
		/// Runs the form.
		/// </summary>
		/// <param name="Name">The form name.</param>
		public static void RunForm(Primitive Name)
		{
			Form form = forms[Name.ToString()];
            
			// Create a new thread to run the form
			Thread formThread = new Thread(() => {
				System.Windows.Forms.Application.EnableVisualStyles();
				System.Windows.Forms.Application.Run(form); // This runs the Windows Form, not WPF.
			});
            
			formThread.SetApartmentState(ApartmentState.STA); // Set to Single Threaded Apartment for UI
			formThread.Start(); // Start the thread
			ZSReflection.SetFieldValue(typeof(GraphicsWindow),"_windowVisible",bool.Parse("True"));
		}
		
		/// <summary>
		/// Call this method Once Exiting.
		/// </summary>
		public static void Dispose()
		{
			ZSReflection.SetFieldValue(typeof(GraphicsWindow),"_windowVisible",bool.Parse("False"));			
		}
		
		/// <summary>
		/// Adds a button to a specified form.
		/// </summary>
		/// <param name="FormName">The name of the form to add the button to.</param>
		/// <param name="ButtonText">The text to display on the button.</param>
		/// <param name="X">The X position of the button.</param>
		/// <param name="Y">The Y position of the button.</param>
		public static void AddButton(Primitive FormName, Primitive ButtonText, Primitive X, Primitive Y)
		{
			Form form = forms[FormName.ToString()];

			Button button = new Button();
			button.Text = ButtonText.ToString();
			button.Location = new System.Drawing.Point(int.Parse(X), int.Parse(Y));
			button.Size = new System.Drawing.Size(100, 50); // Set default button size
			
            
			// Add the button to the form
			form.Invoke((Action)(() => {
				form.Controls.Add(button);
			}));
		}
		

		
	}
}