using Raylib_cs;
using System.Collections.Generic;
using Microsoft.SmallBasic.Library;

namespace ZS
{
	
	public static class CameraManager2D
	{
		private static Dictionary<string, Camera2D> cameras = new Dictionary<string, Camera2D>();

		// Method to create a new 2D camera
		public static void CreateCamera(string name, float offsetX, float offsetY, float targetX, float targetY, float rotation, float zoom)
		{
			Camera2D camera = new Camera2D {
				offset = new Vector2(offsetX, offsetY),
				target = new Vector2(targetX, targetY),
				rotation = rotation,
				zoom = zoom
			};
			cameras[name] = camera;
		}

		
		public static Camera2D GetCamera(string name)
		{
			if (cameras.ContainsKey(name)) {
				return cameras[name];
			}
			// Return an initialized Camera2D with default values
			return new Camera2D {
				offset = new Vector2(0, 0),
				target = new Vector2(0, 0),
				rotation = 0f,
				zoom = 1f
			};
		}

		// Method to set a camera's properties
		public static void SetCameraProperties(string name, float offsetX, float offsetY, float targetX, float targetY, float rotation, float zoom)
		{
			if (cameras.ContainsKey(name)) {
				Camera2D camera = cameras[name];
				camera.offset = new Vector2(offsetX, offsetY);
				camera.target = new Vector2(targetX, targetY);
				camera.rotation = rotation;
				camera.zoom = zoom;
				cameras[name] = camera; // Update the dictionary with the modified camera
			}
		}

		// Methods to get individual properties of a camera
		public static float GetOffsetX(string name)
		{
			if (cameras.ContainsKey(name)) {
				return cameras[name].offset.x; // Assuming x is a field in Vector2
			}
			return 0f;
		}

		public static float GetOffsetY(string name)
		{
			if (cameras.ContainsKey(name)) {
				return cameras[name].offset.y; // Assuming y is a field in Vector2
			}
			return 0f;
		}

		public static float GetTargetX(string name)
		{
			if (cameras.ContainsKey(name)) {
				return cameras[name].target.x; // Assuming x is a field in Vector2
			}
			return 0f;
		}

		public static float GetTargetY(string name)
		{
			if (cameras.ContainsKey(name)) {
				return cameras[name].target.y; // Assuming y is a field in Vector2
			}
			return 0f;
		}

		public static float GetRotation(string name)
		{
			if (cameras.ContainsKey(name)) {
				return cameras[name].rotation;
			}
			return 0f;
		}

		public static float GetZoom(string name)
		{
			if (cameras.ContainsKey(name)) {
				return cameras[name].zoom;
			}
			return 1f;
		}
	}
	
	/// <summary>
	/// The Object For Creating 2D Camera.
	/// </summary>
	[SmallBasicType]
	public static class ZSCamera2D
	{
		/// <summary>
		/// Create A 2D Camera.
		/// </summary>
		/// <param name="Name">The Camera Name.</param>
		/// <param name="Offset">Determines where the camera's target appears on the screen sperated by "-" ex: X-Y</param>
		/// <param name="Target">Target controls which part of the 2D world is visible, allowing for panning across the scene. Seprated by "-" ex: X-Y.</param>
		/// <param name="Rotation">Rotation adjusts the angle of the camera's view, with clockwise and counterclockwise rotation altering the scene's orientation.</param>
		/// <param name="Zoom">Determines the scale of the camera's view</param>
		/// <returns>The Camera Name.</returns>
		public static Primitive CreateCamera(Primitive Name, Primitive Offset, Primitive Target, Primitive Rotation, Primitive Zoom)
		{
			string[] Off = Offset.ToString().Split('-');
			string[] Tar = Target.ToString().Split('-');
			CameraManager2D.CreateCamera(Name.ToString(), float.Parse(Off[0]), float.Parse(Off[1]), float.Parse(Tar[0]), float.Parse(Tar[1]), float.Parse(Rotation.ToString()), float.Parse(Zoom.ToString()));
			return Name;
		}
		
		/// <summary>
		/// Sets The Offset Of Camera. 
		/// </summary>
		/// <param name="Name">The Camera Name.</param>
		/// <param name="Offset">The Offset Value Seprated By "-" X-Y</param>
		public static void SetOffset(Primitive Name, Primitive Offset)
		{
			string[] Off = Offset.ToString().Split('-');
			CameraManager2D.SetCameraProperties(Name.ToString(), float.Parse(Off[0]), float.Parse(Off[1]), CameraManager2D.GetTargetX(Name.ToString()), CameraManager2D.GetTargetY(Name.ToString()), CameraManager2D.GetRotation(Name.ToString()), CameraManager2D.GetZoom(Name.ToString()));
		}
		
		/// <summary>
		/// Sets The Target Of Camera. 
		/// </summary>
		/// <param name="Name">The Camera Name.</param>
		/// <param name="Target">The Target Value Seprated By "-" X-Y</param>
		public static void SetTarget(Primitive Name, Primitive Target)
		{
			string[] Tar = Target.ToString().Split('-');
			CameraManager2D.SetCameraProperties(Name.ToString(), CameraManager2D.GetOffsetX(Name.ToString()), CameraManager2D.GetOffsetY(Name.ToString()), float.Parse(Tar[0]), float.Parse(Tar[1]), CameraManager2D.GetRotation(Name.ToString()), CameraManager2D.GetZoom(Name.ToString()));
		}
		
		
		/// <summary>
		/// Sets The Rotation Of Camera. 
		/// </summary>
		/// <param name="Name">The Camera Name.</param>
		/// <param name="Rotation">The Rotation Value.</param>
		public static void SetRotation(Primitive Name, Primitive Rotation)
		{
			CameraManager2D.SetCameraProperties(Name.ToString(), CameraManager2D.GetOffsetX(Name.ToString()), CameraManager2D.GetOffsetY(Name.ToString()), CameraManager2D.GetTargetX(Name.ToString()), CameraManager2D.GetTargetY(Name.ToString()), float.Parse(Rotation.ToString()), CameraManager2D.GetZoom(Name.ToString()));
		}
		
		/// <summary>
		/// Sets The Zoom Of Camera. 
		/// </summary>
		/// <param name="Name">The Camera Name.</param>
		/// <param name="Zoom">The Zoom Value.</param>
		public static void SetZoom(Primitive Name, Primitive Zoom)
		{
			CameraManager2D.SetCameraProperties(Name.ToString(), CameraManager2D.GetOffsetX(Name.ToString()), CameraManager2D.GetOffsetY(Name.ToString()), CameraManager2D.GetTargetX(Name.ToString()), CameraManager2D.GetTargetY(Name.ToString()), CameraManager2D.GetRotation(Name.ToString()), float.Parse(Zoom.ToString()));
		}
		
		/// <summary>
		/// Gets The Offset X.
		/// </summary>
		/// <param name="Name">The Camera Name.</param>
		/// <returns>The Offset</returns>
		public static Primitive GetOffsetX(Primitive Name)
		{
			return CameraManager2D.GetOffsetX(Name.ToString());
		}
		
		/// <summary>
		/// Gets The Offset Y.
		/// </summary>
		/// <param name="Name">The Camera Name.</param>
		/// <returns>The Offset</returns>
		public static Primitive GetOffsetY(Primitive Name)
		{
			return CameraManager2D.GetOffsetY(Name.ToString());
		}
		
		/// <summary>
		/// Gets The Target X.
		/// </summary>
		/// <param name="Name">The Camera Name.</param>
		/// <returns>The Target</returns>
		public static Primitive GetTargetX(Primitive Name)
		{
			return CameraManager2D.GetTargetX(Name.ToString());
		}
		
		/// <summary>
		/// Gets The Target Y.
		/// </summary>
		/// <param name="Name">The Camera Name.</param>
		/// <returns>The Target</returns>
		public static Primitive GetTargetY(Primitive Name)
		{
			return CameraManager2D.GetTargetY(Name.ToString());
		}
		
		/// <summary>
		/// Gets The Rotation.
		/// </summary>
		/// <param name="Name">The Camera Name.</param>
		/// <returns>The Rotation</returns>
		public static Primitive GetRotation(Primitive Name)
		{
			return CameraManager2D.GetRotation(Name.ToString());
		}
		
		/// <summary>
		/// Gets The Zoom.
		/// </summary>
		/// <param name="Name">The Camera Name.</param>
		/// <returns>The Zoom</returns>
		public static Primitive GetZoom(Primitive Name)
		{
			return CameraManager2D.GetZoom(Name.ToString());
		}
		
		/// <summary>
		/// Provides detailed information on the properties of a Camera2D object in Raylib.
		/// 
		///     offset :
		/// 
        ///  	  This property defines the 2D vector representing the camera's offset from the origin (0,0).
		///       The offset determines the point within the window where the camera's target will be centered.
		///       For example, setting the offset to (0, 0) will place the camera's focus point at the top-left corner of the screen,
		///       whereas (screenWidth/2, screenHeight/2) centers the focus point in the middle of the window.
		///       This property is essential for positioning the camera’s view within the rendering window.
		///     
		///     target :
		///       
		///       The target property specifies the 2D point that the camera is centered on, represented as a Vector2.
		///       It controls what part of the 2D world is visible on the screen. By changing the target, 
		///       you can pan the camera across the scene, effectively moving the view.
		///       The relationship between the offset and the target determines which part of the world is rendered.
		///     
		///     rotation :
		///       
		///       This property, a float, defines the rotation angle of the camera in degrees.
		///       A rotation of 0 means no rotation, while positive values rotate the camera clockwise, and negative values rotate it counterclockwise.
		///       Rotation affects how the entire scene is viewed, making it possible to achieve effects like tilting or spinning the camera’s view.
		///       This property is particularly useful in 2D games where dynamic camera angles are desired.
		///     
		///     zoom :
		///       
		///       The zoom property, also a float, determines the scale of the camera's view.
		///       A zoom value of 1.0 means no zoom, values greater than 1.0 zoom in, and values less than 1.0 zoom out.
		///       Zooming in magnifies the view, making objects appear larger, while zooming out provides a wider field of view.
		///       The zoom property is crucial for creating effects such as magnifying specific parts of the scene or providing an overview of a larger area.
		/// </summary>
		public static void Help()
		{
			// This method serves as documentation and does not perform any operations.
		}

		
		
	}
}