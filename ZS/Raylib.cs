using System;
using Microsoft.SmallBasic.Library;
using Raylib_cs;
using System.Globalization;
using System.Windows;
using System.Collections.Generic;
using ZS;

namespace ZS
{

	/// <summary>
	/// Makes The raylib in SB
	/// </summary>
	[SmallBasicType]
	public static class ZSRaylib
	{
		
		[HideFromIntellisense]
		private static Color HexToRaylibColor(string hex)
		{
			// Remove the '#' if present
			if (hex.StartsWith("#")) {
				hex = hex.Substring(1);
			}

			// Parse the color based on length (6 for RGB, 8 for RGBA)
			byte r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
			byte g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
			byte b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
			byte a = 255; // Default alpha value

			if (hex.Length == 8) { // If there's an alpha value
				a = byte.Parse(hex.Substring(6, 2), NumberStyles.HexNumber);
			}

			// Return a Raylib color
			return new Color(r, g, b, a);
		}
		
		
		/// <summary>
		/// Initialize window and OpenGL context
		/// </summary>
		/// <param name="Width">The Width of window</param>
		/// <param name="Height">The Height of window</param>
		/// <param name="Title">The Title of window</param>
		public static void InitWindow(Primitive Width, Primitive Height, Primitive Title)
		{
			try {
				Raylib.InitWindow(Convert.ToInt32(Width.ToString()), Convert.ToInt32(Height.ToString()), Title.ToString());
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
			}
		}
		
		[HideFromIntellisense]
		public static IntPtr Handle()
		{
			return Raylib.GetWindowHandle();
		}
		
		/// <summary>
		/// The Raylib Window Handle.
		/// </summary>
		/// <returns>The Handle.</returns>
		public static Primitive WindowHandle()
		{
			return Handle().ToString();
		}
		
		
		/// <summary>
		/// Close window and unload OpenGL context
		/// Close window just add at end of code
		/// </summary>
		public static void CloseWindow()
		{
			Raylib.CloseWindow();
		}
		
		/// <summary>
		/// Check if application should close (KEY_ESCAPE pressed or windows close icon clicked)
		/// Does window is closed
		/// </summary>
		public static Primitive WindowShouldClose {
			get {
				return Raylib.WindowShouldClose().ToString();
			}
		}
		
		/// <summary>
		/// Check if window has been initialized successfully
		/// </summary>
		public static Primitive IsWindowReady {
			get {
				return Raylib.IsWindowReady().ToString();
			}
		}
			
		/// <summary>
		/// Check if window is currently fullscreen
		/// </summary>
		public static Primitive IsWindowFullscreen {
			get {
				return Raylib.IsWindowFullscreen().ToString();
			}
		}
		
		/// <summary>
		/// Check if window is currently hidden (only PLATFORM_DESKTOP)
		/// </summary>
		public static Primitive IsWindowHidden {
			get {
				return Raylib.IsWindowHidden().ToString();
			}
		}
			
		/// <summary>
		/// Check if window is currently minimized (only PLATFORM_DESKTOP)
		/// </summary>
		public static Primitive IsWindowMinimized {
			get {
				return Raylib.IsWindowMinimized().ToString();
			}
		}
		
		
		/// <summary>
		/// Check if window has been resized last frame
		/// </summary>
		public static Primitive IsWindowResized {
			get {
				return Raylib.IsWindowResized().ToString();
			}
		}
		
		
		
		/// <summary>
		///  Toggle fullscreen mode (only PLATFORM_DESKTOP)
		/// </summary>
		public static void ToggleFullscreen()
		{
			Raylib.ToggleFullscreen();
		}
		
		/// <summary>
		///  Show the window 
		/// </summary>
		public static void UnhideWindow()
		{
			Raylib.UnhideWindow();
		}
		
		/// <summary>
		/// Hide the window
		/// </summary>
		public static void HideWindow()
		{
			Raylib.HideWindow();
		}
		
		/// <summary>
		/// Set title for window (only PLATFORM_DESKTOP)
		/// </summary>
		/// <param name="Title">The Title</param>
		public static void SetWindowTitle(Primitive Title)
		{
			Raylib.SetWindowTitle(Title.ToString());
		}
		
		/// <summary>
		/// Set window position on screen (only PLATFORM_DESKTOP)
		/// </summary>
		/// <param name="x">x-axis</param>
		/// <param name="y">y-axis</param>
		public static void SetWindowPosition(Primitive x, Primitive y)
		{
			Raylib.SetWindowPosition(Convert.ToInt32(x.ToString()), Convert.ToInt32(y.ToString()));
		}
		
		/// <summary>
		/// Set monitor for the current window (fullscreen mode)
		/// </summary>
		/// <param name="Number">The monitor number 1 for second if available</param>
		public static void SetWindowMonitor(Primitive Number)
		{
			Raylib.SetWindowMonitor(Convert.ToInt32(Number.ToString()));
		}
		
		/// <summary>
		/// Set icon for window (only PLATFORM_DESKTOP)
		/// </summary>
		/// <param name="Path">The path of image</param>
		public static void SetWindowIcon(Primitive Path)
		{
			Image icon = Raylib.LoadImage(Path.ToString());
			Raylib.SetWindowIcon(icon);
		}
		
		/// <summary>
		/// Set window minimum dimensions (for FLAG_WINDOW_RESIZABLE)
		/// </summary>
		/// <param name="Width">The width</param>
		/// <param name="Height">The height</param>
		public static void SetWindowMinSize(Primitive Width, Primitive Height)
		{
			Raylib.SetWindowMinSize(Convert.ToInt32(Width.ToString()), Convert.ToInt32(Height.ToString()));
		}
		
		/// <summary>
		/// Set window dimensions
		/// </summary>
		/// <param name="Width">The width</param>
		/// <param name="Height">The height</param>
		public static void SetWindowSize(Primitive Width, Primitive Height)
		{
			Raylib.SetWindowSize(Convert.ToInt32(Width.ToString()), Convert.ToInt32(Height.ToString()));
		}
		
		/// <summary>
		/// Get current screen width
		/// </summary>
		public static Primitive GetScreenWidth {
			get {
				return Raylib.GetScreenWidth().ToString();
			}
		}
		
		
		/// <summary>
		/// Get current screen height
		/// </summary>
		public static Primitive GetScreenHeight {
			get {
				return Raylib.GetScreenHeight().ToString();
			}
		}
		
		/// <summary>
		/// Get number of connected monitors
		/// </summary>
		public static Primitive GetMonitorCount {
			get {
				return Raylib.GetMonitorCount().ToString();
			}
		}
		
		/// <summary>
		/// Get primary monitor width
		/// </summary>
		/// <param name="Monitor">The monitor number start from 0 as first</param>
		/// <returns>Width</returns>
		public static Primitive GetMonitorWidth(Primitive Monitor)
		{
			return Raylib.GetMonitorWidth(Convert.ToInt32(Monitor.ToString())).ToString();
		}
		
		
		/// <summary>
		/// Get primary monitor height
		/// </summary>
		/// <param name="Monitor">The monitor number start from 0 as first</param>
		/// <returns>Height</returns>
		public static Primitive GetMonitorHeight(Primitive Monitor)
		{
			return Raylib.GetMonitorHeight(Convert.ToInt32(Monitor.ToString())).ToString();
		}
		
		
		/// <summary>
		/// Get primary monitor physical width in millimetres
		/// </summary>
		/// <param name="Monitor">The monitor number start from 0 as first</param>
		/// <returns>Width</returns>
		public static Primitive GetMonitorPhysicalWidth(Primitive Monitor)
		{
			return Raylib.GetMonitorPhysicalWidth(Convert.ToInt32(Monitor.ToString())).ToString();
		}
		
		
		/// <summary>
		/// Get primary monitor physical height in millimetres
		/// </summary>
		/// <param name="Monitor">The monitor number start from 0 as first</param>
		/// <returns>Height</returns>
		public static Primitive GetMonitorPhysicalHeight(Primitive Monitor)
		{
			return Raylib.GetMonitorPhysicalHeight(Convert.ToInt32(Monitor.ToString())).ToString();
		}
		
		/// <summary>
		/// Get window position XY on monitor
		/// </summary>
		/// <returns>XY position</returns>
		public static Primitive GetWindowPosition()
		{
			Vector2 Winpos = Raylib.GetWindowPosition();
			return Winpos.x + "-" + Winpos.y;
		}
		
		/// <summary>
		/// Get the human-readable, UTF-8 encoded name of the primary monitor
		/// </summary>
		/// <param name="Monitor">The monitor number start from 0 as first</param>
		/// <returns>Name</returns>
		public static Primitive GetMonitorName(Primitive Monitor)
		{
			return Raylib.GetMonitorName(Convert.ToInt32(Monitor.ToString()));
		}
		
		/// <summary>
		/// Get clipboard text content
		/// </summary>
		/// <returns>The text</returns>
		public static Primitive GetClipboardText()
		{
			return Raylib.GetClipboardText();
		}
		
		/// <summary>
		/// Set clipboard text content
		/// </summary>
		/// <param name="Text">The text</param>
		public static void SetClipboardText(Primitive Text)
		{
			Raylib.SetClipboardText(Text.ToString());
		}
		
		/// <summary>
		/// Shows cursor
		/// </summary>
		public static void ShowCursor()
		{
			Raylib.ShowCursor();
		}
		
		/// <summary>
		/// Hides cursor
		/// </summary>
		public static void HideCursor()
		{
			Raylib.HideCursor();
		}
		
		/// <summary>
		/// Check if cursor is not visible
		/// </summary>
		public static Primitive IsCursorHidden {
			get {
				return Raylib.IsCursorHidden().ToString();
			}
		}
		
		/// <summary>
		/// Enables cursor (unlock cursor) 
		/// </summary>
		public static void EnableCursor()
		{
			Raylib.EnableCursor();
		}
		
		/// <summary>
		/// Disables cursor (lock cursor)
		/// </summary>
		public static void DisableCursor()
		{
			Raylib.DisableCursor();
		    
		}
		
		/// <summary>
		/// Set background color (framebuffer clear color)
		/// </summary>
		/// <param name="Colour">The Hex Value Of Colour.</param>
		public static void ClearBackground(Primitive Colour)
		{
			Raylib.ClearBackground(HexToRaylibColor(Colour.ToString()));
		}
		
		/// <summary>
		/// Setup canvas (framebuffer) to start drawing.
		/// </summary>
		public static void BeginDrawing()
		{
			Raylib.BeginDrawing();
		}
		
		/// <summary>
		/// End canvas drawing and swap buffers (double buffering).
		/// </summary>
		public static void EndDrawing()
		{
			Raylib.EndDrawing();
		}
		
		/// <summary>
		/// Initialize 2D mode with custom camera (2D).
		/// </summary>
		/// <param name="CameraName">The 2D Camera Name.</param>
		public static void BeginMode2D(Primitive CameraName)
		{
			Raylib.BeginMode2D(CameraManager2D.GetCamera(CameraName.ToString()));
		}
		
		/// <summary>
		/// Ends 2D mode with custom camera.
		/// </summary>
		public static void EndMode2D()
		{
			Raylib.EndMode2D();
		}
		
		/// <summary>
		/// Initializes 3D mode with custom camera (3D).
		/// </summary>
		/// <param name="CameraName">The 3D Camera Name.</param>
		public static void BeginMode3D(Primitive CameraName)
		{
			Raylib.BeginMode3D(CameraManager3D.GetCamera(CameraName.ToString()));
		}
		
		/// <summary>
		/// Ends 3D mode and returns to default 2D orthographic mode.
		/// </summary>
		public static void EndMode3D()
		{
			Raylib.EndMode3D();
		}
		
		/// <summary>
		/// Initializes render texture for drawing.
		/// </summary>
		/// <param name="Name">The Render Texture Name.</param>
		public static void BeginTextureMode(Primitive Name)
		{
			Raylib.BeginTextureMode(RenderTextureManager.GetRender(Name));
		}
		
		/// <summary>
		/// Ends drawing to render texture.
		/// </summary>
		public static void EndTextureMode()
		{
			Raylib.EndTextureMode();
		}
		
		/// <summary>
		/// Begin scissor mode (define screen area for following drawing).
		/// </summary>
		/// <param name="X">The X Pos</param>
		/// <param name="Y">The Y Pos</param>
		/// <param name="Width">The Width.</param>
		/// <param name="Height">The Height.</param>
		public static void BeginScissorMode(Primitive X, Primitive Y, Primitive Width, Primitive Height)
		{
			Raylib.BeginScissorMode(X, Y, Width, Height);
		}
		
		/// <summary>
		/// End scissor mode.
		/// </summary>
		public static void EndScissorMode()
		{
			Raylib.EndScissorMode();
		}
			
		/// <summary>
		/// Gets the ray of mouse position and store it.
		/// if want get details of ray from ZSRS.GetRayProperties
		/// </summary>
		/// <param name="Name">The Name of ray.</param>
		/// <param name="Mouse">the X-Y cordinates of mouse.</param>
		/// <param name="Camera">the 3D Camera Name.</param>
		/// <returns>Ray Name</returns>
		public static Primitive GetMouseRay(Primitive Name, Primitive Mouse, Primitive Camera)
		{
			var mp = Mouse.ToString().Split('-');
			RayManager.AddRay(Name, Raylib.GetMouseRay(new Vector2(float.Parse(mp[0]), float.Parse(mp[1])), CameraManager3D.GetCamera(Camera)));
			return Name;
		}
		
		/// <summary>
		/// Set the target FPS (maximum frames per second).
		/// </summary>
		/// <param name="FPS">Target frames per second (e.g., 60).</param>
		public static void SetTargetFPS(Primitive FPS)
		{
			try {
				int fps = int.Parse(FPS.ToString());
				Raylib.SetTargetFPS(fps);
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
			}
		}

		/// <summary>
		/// Returns the current FPS (frames per second).
		/// </summary>
		/// <returns>The current FPS as an integer.</returns>
		public static Primitive GetFPS()
		{
			try {
				return Raylib.GetFPS();
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
				return 0;
			}
		}

		/// <summary>
		/// Returns the time in seconds for the last frame drawn.
		/// </summary>
		/// <returns>The time in seconds as a float.</returns>
		public static Primitive GetFrameTime()
		{
			try {
				return Raylib.GetFrameTime();
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
				return 0f;
			}
		}

		/// <summary>
		/// Returns the elapsed time in seconds since InitWindow().
		/// </summary>
		/// <returns>The elapsed time in seconds as a double.</returns>
		public static Primitive GetTime()
		{
			try {
				return Raylib.GetTime();
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
				return 0.0;
			}
		}

		
		
			
	}
}