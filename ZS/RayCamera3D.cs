using Raylib_cs;
using System.Collections.Generic;
using Microsoft.SmallBasic.Library;

namespace ZS
{
	public static class CameraManager3D
	{
		private static Dictionary<string, Camera3D> cameras = new Dictionary<string, Camera3D>();

		// Method to create a new 3D camera
		public static string CreateCamera(string name, Vector3 position, Vector3 target, Vector3 up, float fovy, CameraType type)
		{
			Camera3D camera = new Camera3D {
				position = position,
				target = target,
				up = up,
				fovy = fovy,
				type = type
			};
			cameras[name] = camera;
			return name;
		}

		// Method to get a camera by name
		public static Camera3D GetCamera(string name)
		{
			if (cameras.ContainsKey(name)) {
				return cameras[name];
			}

			// Throw an exception or return a default Camera3D if the camera is not found
			throw new KeyNotFoundException("Camera with name '" + name + "' not found.");
		}


		// Method to set a camera's properties
		public static void SetCameraProperties(string name, Vector3 position, Vector3 target, Vector3 up, float fovy, CameraType type)
		{
			if (cameras.ContainsKey(name)) {
				Camera3D camera = cameras[name];
				camera.position = position;
				camera.target = target;
				camera.up = up;
				camera.fovy = fovy;
				camera.type = type;
				cameras[name] = camera; // Update the dictionary with the modified camera
			}
		}

		// Methods to get individual properties of a camera
		public static Vector3 GetPosition(string name)
		{
			Camera3D? camera = GetCamera(name);
			return camera.HasValue ? camera.Value.position : new Vector3(0, 0, 0);
		}

		public static Vector3 GetTarget(string name)
		{
			Camera3D? camera = GetCamera(name);
			return camera.HasValue ? camera.Value.target : new Vector3(0, 0, 0);
		}

		public static Vector3 GetUp(string name)
		{
			Camera3D? camera = GetCamera(name);
			return camera.HasValue ? camera.Value.up : new Vector3(0, 1, 0);
		}

		public static float GetFovy(string name)
		{
			Camera3D? camera = GetCamera(name);
			return camera.HasValue ? camera.Value.fovy : 45f;
		}

		public static CameraType GetCameraType(string name)
		{
			Camera3D? camera = GetCamera(name);
			return camera.HasValue ? camera.Value.type : CameraType.CAMERA_PERSPECTIVE;
		}
	}
	
	
	/// <summary>
	/// The Object For Creating 2D Camera And Modifing.
	/// </summary>
	[SmallBasicType]
	public static class ZSCamera3D
	{
		
		/// <summary>
		/// Creates a new camera
		/// All paramaters like position , target and up are sperated with "-" for cordinates
		/// like this "X-Y-Z"
		/// 
		/// Camera Type : 
		/// CAMERA_PERSPECTIVE: Objects appear smaller as they are further from the camera, simulating depth.
		/// CAMERA_ORTHOGRAPHIC: Objects maintain the same size regardless of their distance from the camera, commonly used for 2D-style games in a 3D space.
		/// </summary>
		/// <param name="Name">A name for camera that will be used in feature</param>
		/// <param name="Position">defines the position x-y-z of the camera in the 3D world.</param>
		/// <param name="Target">defines the point x-y-z in the 3D world that the camera is looking at.</param>
		/// <param name="Up">defines the "up" direction for the camera. It helps orient the camera and define which way is up.</param>
		/// <param name="Fovy">defines the vertical field of view angle, typically measured in degrees. It determines how wide the camera's view is.</param>
		/// <param name="Type">determines the projection type of the camera, 1 for perspective and 2 for orthographic.</param>
		/// <returns>The Name Of Camera.</returns>
		public static Primitive CreateCamera(Primitive Name, Primitive Position, Primitive Target, Primitive Up, Primitive Fovy, Primitive Type)
		{
			string[] pos = Position.ToString().Split('-');
			string[] tar = Target.ToString().Split('-');
			string[] upa = Up.ToString().Split('-');
			Vector3 position = new Vector3(float.Parse(pos[0]), float.Parse(pos[1]), float.Parse(pos[2]));
			Vector3 target = new Vector3(float.Parse(tar[0]), float.Parse(tar[1]), float.Parse(tar[2]));
			Vector3 up = new Vector3(float.Parse(upa[0]), float.Parse(upa[1]), float.Parse(upa[2]));
			CameraType Camera = CameraType.CAMERA_PERSPECTIVE;
			if (Type.ToString() == "1") {
				Camera = CameraType.CAMERA_PERSPECTIVE;
			}
			if (Type.ToString() == "2") {
				Camera = CameraType.CAMERA_ORTHOGRAPHIC;
			}
			return CameraManager3D.CreateCamera(Name.ToString(), position, target, up, float.Parse(Fovy.ToString()), Camera);
		}
		
		/// <summary>
		/// Sets the Position of camera.
		/// </summary>
		/// <param name="Name">The camera name</param>
		/// <param name="Position">The position in x-y-z seperated by "-"</param>
		public static void SetPosition(Primitive Name, Primitive Position)
		{
			string[] arg = Position.ToString().Split('-');
			Vector3 pro = new Vector3(float.Parse(arg[0]), float.Parse(arg[1]), float.Parse(arg[2]));
			CameraManager3D.SetCameraProperties(Name.ToString(), pro, CameraManager3D.GetTarget(Name.ToString()), CameraManager3D.GetUp(Name.ToString()), CameraManager3D.GetFovy(Name.ToString()), CameraManager3D.GetCameraType(Name.ToString()));
		}
		
		/// <summary>
		/// Sets the target of camera.
		/// </summary>
		/// <param name="Name">The camera name</param>
		/// <param name="Target">The target in x-y-z seperated by "-"</param>
		public static void SetTarget(Primitive Name, Primitive Target)
		{
			string[] arg = Target.ToString().Split('-');
			Vector3 pro = new Vector3(float.Parse(arg[0]), float.Parse(arg[1]), float.Parse(arg[2]));
			CameraManager3D.SetCameraProperties(Name.ToString(), CameraManager3D.GetPosition(Name.ToString()), pro, CameraManager3D.GetUp(Name.ToString()), CameraManager3D.GetFovy(Name.ToString()), CameraManager3D.GetCameraType(Name.ToString()));
		}
		
		/// <summary>
		/// Sets the Up of camera.
		/// </summary>
		/// <param name="Name">The camera name</param>
		/// <param name="Up">The Up in x-y-z seperated by "-"</param>
		public static void SetUp(Primitive Name, Primitive Up)
		{
			string[] arg = Up.ToString().Split('-');
			Vector3 pro = new Vector3(float.Parse(arg[0]), float.Parse(arg[1]), float.Parse(arg[2]));
			CameraManager3D.SetCameraProperties(Name.ToString(), CameraManager3D.GetPosition(Name.ToString()), CameraManager3D.GetTarget(Name.ToString()), pro, CameraManager3D.GetFovy(Name.ToString()), CameraManager3D.GetCameraType(Name.ToString()));
		}
		
		/// <summary>
		/// Sets the fovy of camera.
		/// </summary>
		/// <param name="Name">The camera name</param>
		/// <param name="fovy">The fovy </param>
		public static void Setfovy(Primitive Name, Primitive fovy)
		{
			CameraManager3D.SetCameraProperties(Name.ToString(), CameraManager3D.GetPosition(Name.ToString()), CameraManager3D.GetTarget(Name.ToString()), CameraManager3D.GetUp(Name.ToString()), float.Parse(Name.ToString()), CameraManager3D.GetCameraType(Name.ToString()));
		}
		
		/// <summary>
		/// Sets the Type of camera.
		/// </summary>
		/// <param name="Name">The camera name</param>
		/// <param name="Type">The Type of camera 1 for perspective and 2 for orthographic.</param>
		public static void SetType(Primitive Name, Primitive Type)
		{
			CameraType mytype = CameraType.CAMERA_PERSPECTIVE;
			if (Type.ToString() == "1") {
				mytype = CameraType.CAMERA_PERSPECTIVE;
			}
			if (Type.ToString() == "2") {
				mytype = CameraType.CAMERA_ORTHOGRAPHIC;
			}
			CameraManager3D.SetCameraProperties(Name.ToString(), CameraManager3D.GetPosition(Name.ToString()), CameraManager3D.GetTarget(Name.ToString()), CameraManager3D.GetUp(Name.ToString()), CameraManager3D.GetFovy(Name.ToString()), mytype);
		}
		
		/// <summary>
		/// Gets the position of a camera to an array.
		/// array[1] = X-Coordinate.
		/// array[2] = Y-Coordinate.
		/// array[3] = Z-Coordinate.
		/// </summary>
		/// <param name="Name">The name of camera</param>
		/// <returns>The Array</returns>
		public static Primitive GetPosition(Primitive Name)
		{
			Vector3 pos = CameraManager3D.GetPosition(Name.ToString());
			Primitive mypos = null;
			mypos[1] = pos.x.ToString();
			mypos[2] = pos.y.ToString();
			mypos[3] = pos.z.ToString();
			return mypos;
		}
		
		/// <summary>
		/// Gets the target of a camera to an array.
		/// array[1] = X-Coordinate.
		/// array[2] = Y-Coordinate.
		/// array[3] = Z-Coordinate.
		/// </summary>
		/// <param name="Name">The name of camera</param>
		/// <returns>The Array</returns>
		public static Primitive GetTarget(Primitive Name)
		{
			Vector3 pos = CameraManager3D.GetTarget(Name.ToString());
			Primitive mypos = null;
			mypos[1] = pos.x.ToString();
			mypos[2] = pos.y.ToString();
			mypos[3] = pos.z.ToString();
			return mypos;
		}
		
		/// <summary>
		/// Gets the up of a camera to an array.
		/// array[1] = X-Coordinate.
		/// array[2] = Y-Coordinate.
		/// array[3] = Z-Coordinate.
		/// </summary>
		/// <param name="Name">The name of camera</param>
		/// <returns>The Array</returns>
		public static Primitive GetUp(Primitive Name)
		{
			Vector3 pos = CameraManager3D.GetUp(Name.ToString());
			Primitive mypos = null;
			mypos[1] = pos.x.ToString();
			mypos[2] = pos.y.ToString();
			mypos[3] = pos.z.ToString();
			return mypos;
		}
		
		/// <summary>
		/// Gets The fovy of camera.
		/// </summary>
		/// <param name="Name">The camera Name.</param>
		/// <returns>The fovy</returns>
		public static Primitive Getfovy(Primitive Name)
		{
			return CameraManager3D.GetFovy(Name.ToString()).ToString();
		}
		
		/// <summary>
		/// Gets the type of camera.
		/// </summary>
		/// <param name="Name">The camera name.</param>
		/// <returns>Type as string.</returns>
		public static Primitive GetType(Primitive Name)
		{
			return CameraManager3D.GetCameraType(Name.ToString()).ToString();
		}
		
		/// <summary>
		/// Provides detailed information on the properties of a Camera3D object in Raylib.
		/// 
		///     position :
		///       
		///       This property represents the position of the camera in 3D space, defined as a Vector3.
		///       The position determines where the camera is located within the world. 
		///       For example, setting the position to (0, 10, 10) would place the camera 10 units 
		///       above the origin along the Y-axis and 10 units in front of the origin along the Z-axis.
		///       The position is essential for determining the viewpoint from which the scene is rendered.
		///     
		///     target :
		///       
		///       This property defines the point in 3D space that the camera is aimed at,
		///       specified as a Vector3. The target controls the direction in which the camera is looking.
		///       If the target is set to (0, 0, 0), the camera will point directly at the origin. 
		///       Changing the target allows you to rotate the camera to view different parts of the scene.
		///       The relationship between the position and the target determines the camera's viewing angle.
		///     
		///     up :
		///       
		///       The up vector of the camera, also represented as a Vector3, defines the "up" direction
		///       relative to the camera's orientation. This vector is typically set to (0, 1, 0), 
		///       which corresponds to the positive Y-axis, ensuring that the camera remains upright. 
		///       Modifying this vector can tilt the camera, affecting the roll of the camera's view.
		///       The up vector plays a crucial role in maintaining the camera's orientation in the 3D world.
		///     
		///     fovy :
		///       
		///       The fovy property is a float that determines the field of view (FOV) angle in the Y-axis,
		///       measured in degrees. A typical FOV for a perspective camera is between 45 and 60 degrees.
		///       A larger FOV results in a wider view, allowing more of the scene to be visible, 
		///       but can also introduce perspective distortion. Conversely, a smaller FOV gives a zoomed-in effect.
		///       The fovy property is particularly important for perspective cameras, as it influences 
		///       the depth perception of the scene.
		///     
		///     type :
		///       
		///       The type property specifies the projection type of the camera, using the CameraType enum.
		///       This can be either:
		///      
        /// 		
		///           CAMERA_PERSPECTIVE :
		///             In this mode, the camera simulates how the human eye perceives the world, 
		///             with objects appearing smaller as they are farther away. It is commonly used for 
		///             rendering 3D scenes where depth and perspective are important.
		///           
		///           CAMERA_ORTHOGRAPHIC :
		///             In this mode, the camera renders objects with parallel projection lines, 
		///             meaning that objects do not get smaller with distance. This is useful for 2D games, 
		///             technical drawings, or any scenario where depth perception is not required.
		///           
		///       The type property controls how the 3D scene is projected onto the 2D screen, 
		///       significantly affecting the visual output.
		/// </summary>
		public static void Help()
		{
			// This method serves as documentation and does not perform any operations.
		}

		
		
	}
}