using System;
using System.Globalization;
using System.Net.Mail;
using System.ServiceProcess;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.SmallBasic.Library;
using System.Windows.Forms;
using System.Web;
using System.IO;
using System.Drawing;
using System.Drawing.Text;
using System.Net.NetworkInformation;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Net;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using Microsoft.Win32;
using Microsoft.VisualBasic;




namespace ZS
{

	
	/// <summary>
	/// Provides various dialog utilities such as message boxes, input dialogs, file dialogs, and color dialogs.
	/// </summary>
	[SmallBasicType]
	public static class ZSDialog
	{
		/// <summary>
		/// Shows a message box with the specified text.
		/// </summary>
		/// <param name="text">The text to display in the message box.</param>
		public static void ShowMessageBox(Primitive text)
		{
			MessageBox.Show(text);
		}

		/// <summary>
		/// Shows a custom input dialog with the specified prompt text.
		/// </summary>
		/// <param name="prompt">The prompt text to display in the input dialog.</param>
		/// <returns>The text entered by the user.</returns>
		public static Primitive ShowInputDialog(Primitive prompt)
		{
			string input = ShowInputDialogInternal(prompt);
			return new Primitive(input);
		}

		/// <summary>
		/// Shows a file open dialog and returns the selected file path.
		/// </summary>
		/// <returns>The path of the selected file.</returns>
		public static Primitive ShowOpenFileDialog()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			Primitive result = "";
			if (openFileDialog.ShowDialog() == DialogResult.OK) {
				result = openFileDialog.FileName;
			}
			return result;
		}

		/// <summary>
		/// Shows a file save dialog and returns the selected file path.
		/// </summary>
		/// <returns>The path of the file to save.</returns>
		public static Primitive ShowSaveFileDialog()
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			Primitive result = "";
			if (saveFileDialog.ShowDialog() == DialogResult.OK) {
				result = saveFileDialog.FileName;
			}
			return result;
		}

		/// <summary>
		/// Shows a color dialog and returns the selected color as a string.
		/// </summary>
		/// <returns>The selected color in hexadecimal format (e.g., #FF0000 for red).</returns>
		public static Primitive ShowColorDialog()
		{
			ColorDialog colorDialog = new ColorDialog();
			if (colorDialog.ShowDialog() == DialogResult.OK) {
				return new Primitive(ColorTranslator.ToHtml(colorDialog.Color));
			}
			return new Primitive(string.Empty);
		}

		private static string ShowInputDialogInternal(string prompt)
		{
			Form form = new Form();
			Label label = new Label();
			TextBox textBox = new TextBox();
			Button buttonOk = new Button();
			Button buttonCancel = new Button();

			form.Text = "Input Dialog";
			label.Text = prompt;
			textBox.Text = string.Empty;

			buttonOk.Text = "OK";
			buttonCancel.Text = "Cancel";
			buttonOk.DialogResult = DialogResult.OK;
			buttonCancel.DialogResult = DialogResult.Cancel;

			label.SetBounds(9, 20, 372, 13);
			textBox.SetBounds(12, 36, 372, 20);
			buttonOk.SetBounds(228, 72, 75, 23);
			buttonCancel.SetBounds(309, 72, 75, 23);

			label.AutoSize = true;
			textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
			buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

			form.ClientSize = new System.Drawing.Size(396, 107);
			form.Controls.AddRange(new Control[] {
				label,
				textBox,
				buttonOk,
				buttonCancel
			});
			form.ClientSize = new System.Drawing.Size(System.Math.Max(300, label.Right + 10), form.ClientSize.Height);
			form.FormBorderStyle = FormBorderStyle.FixedDialog;
			form.StartPosition = FormStartPosition.CenterScreen;
			form.MinimizeBox = false;
			form.MaximizeBox = false;
			form.AcceptButton = buttonOk;
			form.CancelButton = buttonCancel;

			DialogResult dialogResult = form.ShowDialog();
			return textBox.Text;
		}
    
    
		/// <summary>
		/// Shows a Yes/No dialog with the specified question text.
		/// </summary>
		/// <param name="question">The question text to display in the dialog.</param>
		/// <returns>Returns "Yes" if Yes is clicked, otherwise returns "No".</returns>
		public static Primitive ShowYesNoDialog(Primitive question)
		{
			DialogResult result = MessageBox.Show(question, "Yes/No Dialog", MessageBoxButtons.YesNo);
			return new Primitive(result == DialogResult.Yes ? "Yes" : "No");
		}

		/// <summary>
		/// Shows a folder browser dialog and returns the selected folder path.
		/// </summary>
		/// <returns>The path of the selected folder.</returns>
		public static Primitive ShowFolderBrowserDialog()
		{
			FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
			Primitive result = "";
			if (folderBrowserDialog.ShowDialog() == DialogResult.OK) {
				result = folderBrowserDialog.SelectedPath;
			}
			return result;
		}

		/// <summary>
		/// Shows an error dialog with the specified error message.
		/// </summary>
		/// <param name="errorMessage">The error message to display in the dialog.</param>
		public static void ShowErrorDialog(Primitive errorMessage)
		{
			MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		/// <summary>
		/// Shows a warning dialog with the specified warning message.
		/// </summary>
		/// <param name="warningMessage">The warning message to display in the dialog.</param>
		public static void ShowWarningDialog(Primitive warningMessage)
		{
			MessageBox.Show(warningMessage, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}

		/// <summary>
		/// Shows an information dialog with the specified information message.
		/// </summary>
		/// <param name="informationMessage">The information message to display in the dialog.</param>
		public static void ShowInformationDialog(Primitive informationMessage)
		{
			MessageBox.Show(informationMessage, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		/// <summary>
		/// Shows a custom message box with specified text, title, and buttons.
		/// </summary>
		/// <param name="text">The text to display in the message box.</param>
		/// <param name="title">The title of the message box.</param>
		/// <param name="buttons">The buttons to display in the message box (OK, OKCancel, YesNo).</param>
		/// <returns>The text of the button that was clicked.</returns>
		public static Primitive ShowCustomMessageBox(Primitive text, Primitive title, Primitive buttons)
		{
			MessageBoxButtons messageBoxButtons = MessageBoxButtons.OK;
			if (buttons == "OKCancel")
				messageBoxButtons = MessageBoxButtons.OKCancel;
			else if (buttons == "YesNo")
				messageBoxButtons = MessageBoxButtons.YesNo;

			DialogResult result = MessageBox.Show(text, title, messageBoxButtons);
			return new Primitive(result.ToString());
		}

		/// <summary>
		/// Shows a progress dialog with a specified message and duration.
		/// </summary>
		/// <param name="message">The message to display in the progress dialog.</param>
		/// <param name="durationInSeconds">The duration in seconds for which the progress dialog should be displayed.</param>
		public static void ShowProgressDialog(Primitive message, Primitive durationInSeconds)
		{
			Form progressDialog = new Form();
			Label label = new Label();
			ProgressBar progressBar = new ProgressBar();
			System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

			progressDialog.Text = "Progress";
			label.Text = message;
			progressBar.Style = ProgressBarStyle.Marquee;

			label.SetBounds(9, 20, 372, 13);
			progressBar.SetBounds(12, 36, 372, 20);

			label.AutoSize = true;
			progressBar.Anchor = progressBar.Anchor | AnchorStyles.Right;

			progressDialog.ClientSize = new System.Drawing.Size(396, 75);
			progressDialog.Controls.AddRange(new Control[] { label, progressBar });
			progressDialog.ClientSize = new System.Drawing.Size(System.Math.Max(300, label.Right + 10), progressDialog.ClientSize.Height);
			progressDialog.FormBorderStyle = FormBorderStyle.FixedDialog;
			progressDialog.StartPosition = FormStartPosition.CenterScreen;
			progressDialog.MinimizeBox = false;
			progressDialog.MaximizeBox = false;

			timer.Interval = (int)durationInSeconds * 1000;
			timer.Tick += (sender, e) => {
				timer.Stop();
				progressDialog.Close();
			};

			timer.Start();
			progressDialog.ShowDialog();
		}

		/// <summary>
		/// Shows a font dialog and returns the selected font as a string.
		/// </summary>
		/// <returns>The selected font in the format "FontName, Size, Style".</returns>
		public static Primitive ShowFontDialog()
		{
			FontDialog fontDialog = new FontDialog();
			if (fontDialog.ShowDialog() == DialogResult.OK) {
				Font selectedFont = fontDialog.Font;
				return new Primitive(string.Format("{0}, {1}, {2}", selectedFont.Name, selectedFont.Size, selectedFont.Style));
			}
			return new Primitive(string.Empty);
		}


		/// <summary>
		/// Shows a time picker dialog and returns the selected time.
		/// </summary>
		/// <returns>The selected time in the format "HH:mm:ss".</returns>
		public static Primitive ShowTimePickerDialog()
		{
			DateTimePicker timePicker = new DateTimePicker();
			timePicker.Format = DateTimePickerFormat.Time;
			timePicker.ShowUpDown = true;

			Form form = new Form();
			Button buttonOk = new Button();
			Button buttonCancel = new Button();

			form.Text = "Time Picker";
			timePicker.Value = DateTime.Now;

			buttonOk.Text = "OK";
			buttonCancel.Text = "Cancel";
			buttonOk.DialogResult = DialogResult.OK;
			buttonCancel.DialogResult = DialogResult.Cancel;

			timePicker.SetBounds(12, 12, 200, 20);
			buttonOk.SetBounds(228, 12, 75, 23);
			buttonCancel.SetBounds(309, 12, 75, 23);

			buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

			form.ClientSize = new System.Drawing.Size(396, 45);
			form.Controls.AddRange(new Control[] { timePicker, buttonOk, buttonCancel });
			form.FormBorderStyle = FormBorderStyle.FixedDialog;
			form.StartPosition = FormStartPosition.CenterScreen;
			form.MinimizeBox = false;
			form.MaximizeBox = false;
			form.AcceptButton = buttonOk;
			form.CancelButton = buttonCancel;

			DialogResult dialogResult = form.ShowDialog();
			if (dialogResult == DialogResult.OK) {
				return new Primitive(timePicker.Value.ToString("HH:mm:ss"));
			}
			return new Primitive(string.Empty);
		}

		/// <summary>
		/// Shows a date picker dialog and returns the selected date.
		/// </summary>
		/// <returns>The selected date in the format "yyyy-MM-dd".</returns>
		public static Primitive ShowDatePickerDialog()
		{
			DateTimePicker datePicker = new DateTimePicker();
			datePicker.Format = DateTimePickerFormat.Short;

			Form form = new Form();
			Button buttonOk = new Button();
			Button buttonCancel = new Button();

			form.Text = "Date Picker";
			datePicker.Value = DateTime.Now;

			buttonOk.Text = "OK";
			buttonCancel.Text = "Cancel";
			buttonOk.DialogResult = DialogResult.OK;
			buttonCancel.DialogResult = DialogResult.Cancel;

			datePicker.SetBounds(12, 12, 200, 20);
			buttonOk.SetBounds(228, 12, 75, 23);
			buttonCancel.SetBounds(309, 12, 75, 23);

			buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

			form.ClientSize = new System.Drawing.Size(396, 45);
			form.Controls.AddRange(new Control[] { datePicker, buttonOk, buttonCancel });
			form.FormBorderStyle = FormBorderStyle.FixedDialog;
			form.StartPosition = FormStartPosition.CenterScreen;
			form.MinimizeBox = false;
			form.MaximizeBox = false;
			form.AcceptButton = buttonOk;
			form.CancelButton = buttonCancel;

			DialogResult dialogResult = form.ShowDialog();
			if (dialogResult == DialogResult.OK) {
				return new Primitive(datePicker.Value.ToString("yyyy-MM-dd"));
			}
			return new Primitive(string.Empty);
		}

		/// <summary>
		/// Shows a confirmation dialog with specified text and title.
		/// </summary>
		/// <param name="text">The text to display in the confirmation dialog.</param>
		/// <param name="title">The title of the confirmation dialog.</param>
		/// <returns>Returns true if Yes is clicked, otherwise returns false.</returns>
		public static Primitive ShowConfirmationDialog(Primitive text, Primitive title)
		{
			DialogResult result = MessageBox.Show(text, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			return new Primitive(result == DialogResult.Yes);
		}


		/// <summary>
		/// Shows a color dialog and returns the selected color as a string in the format "R,G,B".
		/// </summary>
		/// <returns>The selected color or an empty string if canceled.</returns>
		public static Primitive ShowCustomColorDialog()
		{
			ColorDialog colorDialog = new ColorDialog();
			if (colorDialog.ShowDialog() == DialogResult.OK) {
				Color color = colorDialog.Color;
				return new Primitive(string.Format("{0},{1},{2}", color.R, color.G, color.B));
			}
			return new Primitive(string.Empty);
		}

		/// <summary>
		/// Shows an open file dialog with multi-select enabled and returns the selected file paths.
		/// </summary>
		/// <param name="filter">The file types filter in the format "Display Name1|Pattern1|Display Name2|Pattern2|...".</param>
		/// <returns>A semicolon-separated list of selected file paths or an empty string if canceled.</returns>
		/// <remarks>
		/// Filters should be specified in pairs where:
		/// - Display Name: The name shown in the dialog's filter dropdown.
		/// - Pattern: The file pattern to filter files by extension (e.g., "*.txt", "*.jpg").
		/// Multiple filters can be separated by vertical bars ('|'). For example:
		/// "Text Files|*.txt|All Files|*.*"
		/// </remarks>
		public static Primitive ShowMultiSelectOpenFileDialog(Primitive filter)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog {
				Filter = filter,
				Multiselect = true
			};

			if (openFileDialog.ShowDialog() == DialogResult.OK) {
				return new Primitive(string.Join(";", openFileDialog.FileNames));
			}

			return new Primitive(string.Empty);
		}

		/// <summary>
		/// Shows an input box with a multi-line text box for larger input.
		/// </summary>
		/// <param name="prompt">The prompt to display in the input box.</param>
		/// <param name="title">The title of the input box.</param>
		/// <returns>The user input or an empty string if canceled.</returns>
		public static Primitive ShowMultiLineInputBox(Primitive prompt, Primitive title)
		{
			Form form = new Form();
			Label label = new Label();
			TextBox textBox = new TextBox();
			Button buttonOk = new Button();
			Button buttonCancel = new Button();

			form.Text = title;
			label.Text = prompt;
			textBox.Multiline = true;

			buttonOk.Text = "OK";
			buttonCancel.Text = "Cancel";
			buttonOk.DialogResult = DialogResult.OK;
			buttonCancel.DialogResult = DialogResult.Cancel;

			label.SetBounds(9, 20, 372, 13);
			textBox.SetBounds(12, 36, 372, 200);
			buttonOk.SetBounds(228, 250, 75, 23);
			buttonCancel.SetBounds(309, 250, 75, 23);

			label.AutoSize = true;
			textBox.Anchor = textBox.Anchor | AnchorStyles.Bottom | AnchorStyles.Right;
			buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

			form.ClientSize = new System.Drawing.Size(396, 285);
			form.Controls.AddRange(new Control[] {
				label,
				textBox,
				buttonOk,
				buttonCancel
			});
			form.ClientSize = new System.Drawing.Size(System.Math.Max(300, label.Right + 10), form.ClientSize.Height);
			form.FormBorderStyle = FormBorderStyle.FixedDialog;
			form.StartPosition = FormStartPosition.CenterScreen;
			form.MinimizeBox = false;
			form.MaximizeBox = false;
			form.AcceptButton = buttonOk;
			form.CancelButton = buttonCancel;

			DialogResult dialogResult = form.ShowDialog();
			if (dialogResult == DialogResult.OK) {
				return new Primitive(textBox.Text);
			}
			return new Primitive(string.Empty);
		}

		/// <summary>
		/// Shows a password input dialog and returns the entered password.
		/// </summary>
		/// <param name="prompt">The prompt to display in the input dialog.</param>
		/// <param name="title">The title of the input dialog.</param>
		/// <returns>The entered password or an empty string if canceled.</returns>
		public static Primitive ShowPasswordInputDialog(Primitive prompt, Primitive title)
		{
			Form form = new Form();
			Label label = new Label();
			TextBox textBox = new TextBox();
			Button buttonOk = new Button();
			Button buttonCancel = new Button();

			form.Text = title;
			label.Text = prompt;
			textBox.PasswordChar = '*';

			buttonOk.Text = "OK";
			buttonCancel.Text = "Cancel";
			buttonOk.DialogResult = DialogResult.OK;
			buttonCancel.DialogResult = DialogResult.Cancel;

			label.SetBounds(9, 20, 372, 13);
			textBox.SetBounds(12, 36, 372, 20);
			buttonOk.SetBounds(228, 72, 75, 23);
			buttonCancel.SetBounds(309, 72, 75, 23);

			label.AutoSize = true;
			textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
			buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

			form.ClientSize = new System.Drawing.Size(396, 107);
			form.Controls.AddRange(new Control[] {
				label,
				textBox,
				buttonOk,
				buttonCancel
			});
			form.ClientSize = new System.Drawing.Size(System.Math.Max(300, label.Right + 10), form.ClientSize.Height);
			form.FormBorderStyle = FormBorderStyle.FixedDialog;
			form.StartPosition = FormStartPosition.CenterScreen;
			form.MinimizeBox = false;
			form.MaximizeBox = false;
			form.AcceptButton = buttonOk;
			form.CancelButton = buttonCancel;

			DialogResult dialogResult = form.ShowDialog();
			if (dialogResult == DialogResult.OK) {
				return new Primitive(textBox.Text);
			}
			return new Primitive(string.Empty);
		}

		/// <summary>
		/// Shows an input dialog with options and returns the selected option.
		/// </summary>
		/// <param name="prompt">The prompt to display in the input dialog.</param>
		/// <param name="title">The title of the input dialog.</param>
		/// <param name="options">The list of options to choose from.</param>
		/// <returns>The selected option or an empty string if canceled.</returns>
		public static Primitive ShowInputDialogWithOptions(Primitive prompt, Primitive title, Primitive[] options)
		{
			Form form = new Form();
			Label label = new Label();
			ComboBox comboBox = new ComboBox();
			Button buttonOk = new Button();
			Button buttonCancel = new Button();

			form.Text = title;
			label.Text = prompt;

			// Convert Primitive[] to object[]
			object[] optionsObjects = new object[options.Length];
			for (int i = 0; i < options.Length; i++) {
				optionsObjects[i] = options[i].ToString();  // Adjust this line to convert Primitive to appropriate object type
			}

			comboBox.Items.AddRange(optionsObjects);
			comboBox.DropDownStyle = ComboBoxStyle.DropDownList;

			buttonOk.Text = "OK";
			buttonCancel.Text = "Cancel";
			buttonOk.DialogResult = DialogResult.OK;
			buttonCancel.DialogResult = DialogResult.Cancel;

			label.SetBounds(9, 20, 372, 13);
			comboBox.SetBounds(12, 36, 372, 21);
			buttonOk.SetBounds(228, 72, 75, 23);
			buttonCancel.SetBounds(309, 72, 75, 23);

			label.AutoSize = true;
			comboBox.Anchor = comboBox.Anchor | AnchorStyles.Right;
			buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

			form.ClientSize = new System.Drawing.Size(396, 107);
			form.Controls.AddRange(new Control[] {
				label,
				comboBox,
				buttonOk,
				buttonCancel
			});
			form.ClientSize = new System.Drawing.Size(System.Math.Max(300, label.Right + 10), form.ClientSize.Height);
			form.FormBorderStyle = FormBorderStyle.FixedDialog;
			form.StartPosition = FormStartPosition.CenterScreen;
			form.MinimizeBox = false;
			form.MaximizeBox = false;
			form.AcceptButton = buttonOk;
			form.CancelButton = buttonCancel;

			DialogResult dialogResult = form.ShowDialog();
			if (dialogResult == DialogResult.OK) {
				return new Primitive(comboBox.SelectedItem != null ? comboBox.SelectedItem.ToString() : string.Empty);
			}
			return new Primitive(string.Empty);
		}


		/// <summary>
		/// Shows a file dialog to select an image file and returns the selected file path.
		/// </summary>
		/// <returns>The path of the selected image file or an empty string if canceled.</returns>
		public static Primitive ShowImageFileDialog()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Image Files (*.bmp;*.jpg;*.jpeg;*.png)|*.BMP;*.JPG;*.JPEG;*.PNG";

			if (openFileDialog.ShowDialog() == DialogResult.OK) {
				return new Primitive(openFileDialog.FileName);
			}
			return new Primitive(string.Empty);
		}
	}
}