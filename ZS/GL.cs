using Microsoft.SmallBasic.Library;
using System;
/*
using SharpGL;
using SharpGL.WPF;
using SharpGL.SceneGraph;
*/
using System.Windows.Controls;
using System.Windows;
using System.Windows.Shapes;
using ZS;

namespace ZS
{
	/// <summary>
	/// OpenGL in small basic Using SharpGL.
	/// </summary>
	[HideFromIntellisense]
	public static class ZSGL
	{
	    /*
		private static OpenGLControl openGLControl;

		public static Primitive Init(Primitive X, Primitive Y, Primitive Width, Primitive Height)
		{
			try {
				Window _win = ZSWpf.Verify();
				if (_win == null) {
					TextWindow.WriteLine("Error: Could not find the Small Basic Graphics Window.");
					return "Error: No Window";
				}

				_win.Dispatcher.Invoke(() => {
					try {
						// Instantiate OpenGLControl if not already done
						if (openGLControl == null) {
							openGLControl = new OpenGLControl();
							TextWindow.WriteLine("OpenGL Control Initialized.");
						}

						// Attach event handler for initialization
						openGLControl.OpenGLInitialized += GlControl_OpenGLInitialized;

						// Set the position and size of the OpenGL control
						openGLControl.Margin = new Thickness(0, 0, 0, 0); // Reset margin for full placement
						openGLControl.Width = Width;  // Set the control width
						openGLControl.Height = Height; // Set the control height

						// Add the OpenGL control to the WPF window
						Grid grid = _win.Content as Grid;
						if (grid != null) {
							// Clear any previous children and add the OpenGL control
							grid.Children.Clear();
							grid.Children.Add(openGLControl);

							// Force the OpenGL control to refresh (render) the scene
							openGLControl.InvalidateVisual();

							TextWindow.WriteLine("OpenGL Control successfully added to the window.");
						} else {
							TextWindow.WriteLine("Error: Could not find the Grid in the WPF window.");
						}
					} catch (Exception ex) {
						TextWindow.WriteLine("Error initializing OpenGL control: " + ex.Message);
					}
				});

				return "Success";
			} catch (Exception ex) {
				TextWindow.WriteLine("Error in Init method: " + ex.Message);
				return "Error: " + ex.Message;
			}
		}

		private static void GlControl_OpenGLInitialized(object sender, OpenGLRoutedEventArgs e)
		{
			try {
				OpenGL gl = e.OpenGL;

				// Set up OpenGL initialization
				gl.ClearColor(0, 0, 0, 0); // Black background
				TextWindow.WriteLine("OpenGL Initialized");

				// Set up the viewport
				int width = (int)openGLControl.Width;
				int height = (int)openGLControl.Height;
				gl.Viewport(0, 0, width, height);

				// Set up projection matrix (orthographic projection)
				gl.MatrixMode(OpenGL.GL_PROJECTION);
				gl.LoadIdentity();
				gl.Ortho2D(-width / 2, width / 2, -height / 2, height / 2); // Adjust for your coordinate system

				// Switch to model view matrix
				gl.MatrixMode(OpenGL.GL_MODELVIEW);
				gl.LoadIdentity();
			} catch (Exception ex) {
				TextWindow.WriteLine("Error during OpenGL initialization: " + ex.Message);
			}
		}


		private static void DrawText(OpenGL gl, string text)
		{
			try {
				// Clear the color and depth buffer
				gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
				gl.LoadIdentity();
				
				// Set the color for drawing (white circle)
				gl.Color(1.0f, 1.0f, 1.0f);
				gl.DrawText(10, 10, 1.0f, 1.0f, 1.0f, "Arial", 24, text);

				
				// Swap buffers (if needed, depending on OpenGLControl settings)
				gl.Flush();
				TextWindow.WriteLine("Text drawn.");
			} catch (Exception ex) {
				TextWindow.WriteLine("Error drawing Text: " + ex.Message);
			}
		}


		public static void Draw(Primitive Text)
		{
			try {
				Window _win = ZSWpf.Verify();
				_win.Dispatcher.Invoke(() => {
					if (openGLControl == null) {
						TextWindow.WriteLine("Error: OpenGLControl is not initialized.");
						return;
					}
					TextWindow.WriteLine("Starting to draw a Text: " + Text);
					OpenGL Gl = openGLControl.OpenGL;
					string glVersion = Gl.GetString(OpenGL.GL_VERSION);
					string error = Gl.GetString(Gl.GetError());
					TextWindow.WriteLine("OpenGL Context Active. Version: " + glVersion);
					TextWindow.WriteLine(error);
					DrawText(Gl, Text.ToString());

					// Request the control to redraw the content
					openGLControl.InvalidateVisual();
					TextWindow.WriteLine("Finished drawing the Text.");
				});
			} catch (Exception ex) {
				TextWindow.WriteLine("Error in Draw method: " + ex.Message);
			}
		}
		*/
	}
}
