using Raylib_cs;
using System.Collections.Generic;
using Microsoft.SmallBasic.Library;
using System;

namespace ZS
{
	[HideFromIntellisense]
	public static class RenderTextureManager
	{
		private static Dictionary<string, RenderTexture2D> renderTextures = new Dictionary<string, RenderTexture2D>();

		public static void CreateRenderTexture(string name, int width, int height)
		{
			if (!renderTextures.ContainsKey(name)) {
				RenderTexture2D renderTexture = Raylib.LoadRenderTexture(width, height);
				renderTextures.Add(name, renderTexture);
			}
		}
		
		public static RenderTexture2D GetRender(string Name)
		{
			return renderTextures[Name];
		}

		public static uint GetTextureId(string name)
		{
			if (renderTextures.ContainsKey(name)) {
				return renderTextures[name].id;
			}
			return 0;
		}

		public static Texture2D GetTexture(string name)
		{
			if (renderTextures.ContainsKey(name)) {
				return renderTextures[name].texture;
			}
			return new Texture2D();
		}

		public static Texture2D GetDepthTexture(string name)
		{
			if (renderTextures.ContainsKey(name)) {
				return renderTextures[name].depth;
			}
			return new Texture2D();
		}

		public static bool HasDepthTexture(string name)
		{
			if (renderTextures.ContainsKey(name)) {
				return renderTextures[name].depthTexture;
			}
			return false;
		}
	}
	
	[HideFromIntellisense]
	public static class Texture2DManager
	{
		private static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
		
		public static void LoadTexture(string name, string fileName)
		{
			Texture2D texture = Raylib.LoadTexture(fileName);
			textures.Add(name, texture);
		}
		
		public static void AddTexture(string Name, Texture2D Texture)
		{
			textures.Add(Name, Texture);
		}

		public static Texture2D GetTexture(string name)
		{
			return textures[name];
		}

		public static uint GetTextureId(string name)
		{
			return textures[name].id;
		}

		public static int GetTextureWidth(string name)
		{
			return textures[name].width;
		}

		public static int GetTextureHeight(string name)
		{
			return textures[name].height;
		}

		public static int GetMipmaps(string name)
		{
			return textures[name].mipmaps;
		}

		public static int GetFormat(string name)
		{
			return textures[name].format;			
		}
	}
	
	public static class RayManager
	{
		private static Dictionary<string, Ray> _rays = new Dictionary<string, Ray>();
		public static void AddRay(Primitive Name,Ray ray)
		{
			_rays.Add(Name,ray);
		}
		public static Primitive GetRayDetail(string Name,string Option)
		{
			Primitive res = null;
			if (Option == "1") {
				Ray ray = _rays[Name];
				res[1] = ray.position.x;
				res[2] = ray.position.y;
				res[3] = ray.position.z;
			}
			if (Option == "2") {
				Ray ray = _rays[Name];
				res[1] = ray.direction.x;
				res[2] = ray.direction.y;
				res[3] = ray.direction.z;
			}
			return res;
		}
	}
	
	
	/// <summary>
	/// some raylib struct added for more functionality not used so much.
	/// you have to view raylib doucumenatation to learn how to use them.
	/// not so much good summary provided here.
	/// </summary>
	[SmallBasicType]
	public static class ZSRay
	{
		/// <summary>
		/// Creates A Render Texture.
		/// </summary>
		/// <param name="Name">The Name For Render Texture.</param>
		/// <param name="Width">Yhe width of render texture.</param>
		/// <param name="Height">the height of render texture.</param>
		/// <returns>the render texture key.</returns>
		public static Primitive CreateRenderTexture(Primitive Name, Primitive Width, Primitive Height)
		{
			RenderTextureManager.CreateRenderTexture(Name, int.Parse(Width), int.Parse(Height));
			return Name;
		}
		
		/// <summary>
		/// gets the id of render texture.
		/// </summary>
		/// <param name="Name"></param>
		/// <returns></returns>
		public static Primitive GetTextureId(Primitive Name)
		{
			return RenderTextureManager.GetTextureId(Name).ToString();
		}
		
		/// <summary>
		/// adds the render texture to ZSTexture2d
		/// </summary>
		/// <param name="Name">the name of render texture.</param>
		/// <param name="TextureName">the name of texture2d.</param>
		/// <returns>the texture 2d key.</returns>
		public static Primitive GetTexture(Primitive Name, Primitive TextureName)
		{
			Texture2DManager.AddTexture(TextureName, RenderTextureManager.GetTexture(Name));
			return TextureName;
		}
		
		/// <summary>
		/// gets the depth texture of render texture.
		/// </summary>
		/// <param name="Name">the name of render texture.</param>
		/// <param name="TextureName">the ZSTexture2d name.</param>
		/// <returns>the texture2d key.</returns>
		public static Primitive GetDepthTexture(Primitive Name, Primitive TextureName)
		{
			Texture2DManager.AddTexture(TextureName, RenderTextureManager.GetDepthTexture(Name));
			return TextureName;
		}
		
		/// <summary>
		/// does render texture has depthtexture.
		/// </summary>
		/// <param name="Name">the name of render texture 2d.</param>
		/// <returns>true or false.</returns>
		public static Primitive HasDepthTexture(Primitive Name)
		{
			return RenderTextureManager.HasDepthTexture(Name).ToString();
		}
		
		/// <summary>
		/// Gets a Texture2D Details.
		/// Use Raylib Documentation to learn.
		/// </summary>
		/// <param name="Name">The Name Of Texture2D.</param>
		/// <param name="Option">
		/// 1 - Texture id.
		/// 2 - Texture Width.
		/// 3 - Texture Height.
		/// 4 - Mipmaps.
		/// 5 - Format.
		/// </param>
		/// <returns>Result.</returns>
		public static Primitive GetTexture2D_Detail(Primitive Name, Primitive Option)
		{
			if (Option == "1") {
				return Texture2DManager.GetTextureId(Name).ToString();
			}
			if (Option == "2") {
				return Texture2DManager.GetTextureWidth(Name).ToString();
			}
			if (Option == "3") {
				return Texture2DManager.GetTextureHeight(Name).ToString();
			}
			if (Option == "4") {
				return Texture2DManager.GetMipmaps(Name).ToString();
			}
			if (Option == "5") {
				return Texture2DManager.GetFormat(Name).ToString();
			}			
			return "invalid option";
		}
		
		/// <summary>
		/// Gets The Properties of A Ray.
		/// </summary>
		/// <param name="Name">The Name Of Ray.</param>
		/// <param name="Option">
		/// 1 - position
		/// 2 - direction
		/// </param>
		/// <returns>Result</returns>
		public static Primitive GetRayProperties(Primitive Name,Primitive Option)
		{
			return RayManager.GetRayDetail(Name,Option);
		}
		
	}
}