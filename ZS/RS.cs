using System;
using Microsoft.SmallBasic.Library;
using Raylib_cs;
using System.Globalization;

namespace ZS
{
	/// <summary>
	/// The Raylib 2D Shapes.
	/// </summary>
	[SmallBasicType]
	public static class ZSRS
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
		/// Draw a pixel.
		/// </summary>
		/// <param name="X">The X Position.</param>
		/// <param name="Y">The Y Position.</param>
		/// <param name="Colour">The Hex Value Of Colour.</param>
		public static void DrawPixel(Primitive X, Primitive Y, Primitive Colour)
		{
			Raylib.DrawPixel(int.Parse(X), int.Parse(Y), HexToRaylibColor(Colour));
		}
		
		/// <summary>
		/// Draw a line.
		/// </summary>
		/// <param name="Start">The X-Y position of start of line</param>
		/// <param name="End">The X-Y position of end of line</param>
		/// <param name="Colour">The Hex Value Of Colour</param>
		public static void DrawLine(Primitive Start, Primitive End, Primitive Colour)
		{
			try {
				string[] sp = Start.ToString().Split('-');
				string[] ep = End.ToString().Split('-');
				Raylib.DrawLine(int.Parse(sp[0]), int.Parse(sp[1]), int.Parse(ep[0]), int.Parse(ep[1]), HexToRaylibColor(Colour));
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
			}
		}
		
		
		/// <summary>
		/// Draw a line with a specified thickness.
		/// </summary>
		/// <param name="Start">The X-Y position of the start of the line.</param>
		/// <param name="End">The X-Y position of the end of the line.</param>
		/// <param name="Thickness">The thickness of the line.</param>
		/// <param name="Colour">The Hex Value Of Colour</param>
		public static void DrawLineEx(Primitive Start, Primitive End, Primitive Thickness, Primitive Colour)
		{
			try {
				string[] startPos = Start.ToString().Split('-');
				string[] endPos = End.ToString().Split('-');
				float thickness = float.Parse(Thickness.ToString());
				Raylib.DrawLineEx(
					new Vector2(int.Parse(startPos[0]), int.Parse(startPos[1])),
					new Vector2(int.Parse(endPos[0]), int.Parse(endPos[1])),
					thickness,
					HexToRaylibColor(Colour)
				);
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
			}
		}
		
		/// <summary>
		/// Draw a line using cubic-bezier curves in-out.
		/// </summary>
		/// <param name="Start">The X-Y position of the start of the line.</param>
		/// <param name="End">The X-Y position of the end of the line.</param>
		/// <param name="Thickness">The thickness of the line.</param>
		/// <param name="Colour">The Hex Value Of Colour</param>
		public static void DrawLineBezier(Primitive Start, Primitive End, Primitive Thickness, Primitive Colour)
		{
			try {
				string[] startPos = Start.ToString().Split('-');
				string[] endPos = End.ToString().Split('-');
				float thickness = float.Parse(Thickness.ToString());
				Raylib.DrawLineBezier(
					new Vector2(int.Parse(startPos[0]), int.Parse(startPos[1])),
					new Vector2(int.Parse(endPos[0]), int.Parse(endPos[1])),
					thickness,
					HexToRaylibColor(Colour)
				);
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
			}
		}

		/// <summary>
		/// Draw a sequence of connected lines.
		/// </summary>
		/// <param name="Points">An array of X-Y positions for the line points.</param>
		/// <param name="NumPoints">The number of points in the sequence.</param>
		/// <param name="Colour">The Hex Value Of Colour</param>
		public static void DrawLineStrip(Primitive Points, Primitive NumPoints, Primitive Colour)
		{
			try {
				string[] pointsArray = Points.ToString().Split(';');
				int numPoints = int.Parse(NumPoints.ToString());
				Vector2[] raylibPoints = new Vector2[numPoints];

				for (int i = 0; i < numPoints; i++) {
					string[] pointCoord = pointsArray[i].Split('=');
					string[] coordinates = pointCoord[1].Split('-');
					raylibPoints[i] = new Vector2(int.Parse(coordinates[0]), int.Parse(coordinates[1]));
				}

				Raylib.DrawLineStrip(raylibPoints, numPoints, HexToRaylibColor(Colour));
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
			}
		}

		/// <summary>
		/// Draw a color-filled circle.
		/// </summary>
		/// <param name="CenterX">The X position of the center of the circle.</param>
		/// <param name="CenterY">The Y position of the center of the circle.</param>
		/// <param name="Radius">The radius of the circle.</param>
		/// <param name="Colour">The Hex Value Of Colour</param>
		public static void DrawCircle(Primitive CenterX, Primitive CenterY, Primitive Radius, Primitive Colour)
		{
			try {
				int centerX = int.Parse(CenterX.ToString());
				int centerY = int.Parse(CenterY.ToString());
				float radius = float.Parse(Radius.ToString());
				Raylib.DrawCircle(centerX, centerY, radius, HexToRaylibColor(Colour));
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
			}
		}

		/// <summary>
		/// Draw a piece of a circle.
		/// </summary>
		/// <param name="Center">The X-Y position of the center of the circle.</param>
		/// <param name="Radius">The radius of the circle sector.</param>
		/// <param name="StartAngle">The starting angle of the sector.</param>
		/// <param name="EndAngle">The ending angle of the sector.</param>
		/// <param name="Segments">The number of segments in the sector.</param>
		/// <param name="Colour">The Hex Value Of Colour</param>
		public static void DrawCircleSector(Primitive Center, Primitive Radius, Primitive StartAngle, Primitive EndAngle, Primitive Segments, Primitive Colour)
		{
			try {
				string[] centerCoords = Center.ToString().Split('-');
				float radius = float.Parse(Radius.ToString());
				int startAngle = int.Parse(StartAngle.ToString());
				int endAngle = int.Parse(EndAngle.ToString());
				int segments = int.Parse(Segments.ToString());

				Raylib.DrawCircleSector(
					new Vector2(int.Parse(centerCoords[0]), int.Parse(centerCoords[1])),
					radius,
					startAngle,
					endAngle,
					segments,
					HexToRaylibColor(Colour)
				);
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
			}
		}

		/// <summary>
		/// Draw circle sector outline.
		/// </summary>
		/// <param name="Center">The X-Y position of the center of the circle.</param>
		/// <param name="Radius">The radius of the circle sector.</param>
		/// <param name="StartAngle">The starting angle of the sector.</param>
		/// <param name="EndAngle">The ending angle of the sector.</param>
		/// <param name="Segments">The number of segments in the sector.</param>
		/// <param name="Colour">The Hex Value Of Colour</param>
		public static void DrawCircleSectorLines(Primitive Center, Primitive Radius, Primitive StartAngle, Primitive EndAngle, Primitive Segments, Primitive Colour)
		{
			try {
				string[] centerCoords = Center.ToString().Split('-');
				float radius = float.Parse(Radius.ToString());
				int startAngle = int.Parse(StartAngle.ToString());
				int endAngle = int.Parse(EndAngle.ToString());
				int segments = int.Parse(Segments.ToString());

				Raylib.DrawCircleSectorLines(
					new Vector2(int.Parse(centerCoords[0]), int.Parse(centerCoords[1])),
					radius,
					startAngle,
					endAngle,
					segments,
					HexToRaylibColor(Colour)
				);
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
			}
		}

		/// <summary>
		/// Draw a gradient-filled circle.
		/// </summary>
		/// <param name="CenterX">The X position of the center of the circle.</param>
		/// <param name="CenterY">The Y position of the center of the circle.</param>
		/// <param name="Radius">The radius of the circle.</param>
		/// <param name="Colour1">The Hex Value Of the first color.</param>
		/// <param name="Colour2">The Hex Value Of the second color.</param>
		public static void DrawCircleGradient(Primitive CenterX, Primitive CenterY, Primitive Radius, Primitive Colour1, Primitive Colour2)
		{
			try {
				int centerX = int.Parse(CenterX.ToString());
				int centerY = int.Parse(CenterY.ToString());
				float radius = float.Parse(Radius.ToString());

				Raylib.DrawCircleGradient(centerX, centerY, radius, HexToRaylibColor(Colour1), HexToRaylibColor(Colour2));
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
			}
		}

		/// <summary>
		/// Draw circle outline.
		/// </summary>
		/// <param name="CenterX">The X position of the center of the circle.</param>
		/// <param name="CenterY">The Y position of the center of the circle.</param>
		/// <param name="Radius">The radius of the circle.</param>
		/// <param name="Colour">The Hex Value Of Colour</param>
		public static void DrawCircleLines(Primitive CenterX, Primitive CenterY, Primitive Radius, Primitive Colour)
		{
			try {
				int centerX = int.Parse(CenterX.ToString());
				int centerY = int.Parse(CenterY.ToString());
				float radius = float.Parse(Radius.ToString());

				Raylib.DrawCircleLines(centerX, centerY, radius, HexToRaylibColor(Colour));
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
			}
		}

		/// <summary>
		/// Draw an ellipse.
		/// </summary>
		/// <param name="CenterX">The X position of the center of the ellipse.</param>
		/// <param name="CenterY">The Y position of the center of the ellipse.</param>
		/// <param name="RadiusH">The horizontal radius of the ellipse.</param>
		/// <param name="RadiusV">The vertical radius of the ellipse.</param>
		/// <param name="Colour">The Hex Value Of Colour</param>
		public static void DrawEllipse(Primitive CenterX, Primitive CenterY, Primitive RadiusH, Primitive RadiusV, Primitive Colour)
		{
			try {
				int centerX = int.Parse(CenterX.ToString());
				int centerY = int.Parse(CenterY.ToString());
				float radiusH = float.Parse(RadiusH.ToString());
				float radiusV = float.Parse(RadiusV.ToString());

				Raylib.DrawEllipse(centerX, centerY, radiusH, radiusV, HexToRaylibColor(Colour));
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
			}
		}

		/// <summary>
		/// Draw ellipse outline.
		/// </summary>
		/// <param name="CenterX">The X position of the center of the ellipse.</param>
		/// <param name="CenterY">The Y position of the center of the ellipse.</param>
		/// <param name="RadiusH">The horizontal radius of the ellipse.</param>
		/// <param name="RadiusV">The vertical radius of the ellipse.</param>
		/// <param name="Colour">The Hex Value Of Colour</param>
		public static void DrawEllipseLines(Primitive CenterX, Primitive CenterY, Primitive RadiusH, Primitive RadiusV, Primitive Colour)
		{
			try {
				int centerX = int.Parse(CenterX.ToString());
				int centerY = int.Parse(CenterY.ToString());
				float radiusH = float.Parse(RadiusH.ToString());
				float radiusV = float.Parse(RadiusV.ToString());

				Raylib.DrawEllipseLines(centerX, centerY, radiusH, radiusV, HexToRaylibColor(Colour));
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
			}
		}

		/// <summary>
		/// Draw a ring.
		/// </summary>
		/// <param name="Center">The X-Y position of the center of the ring.</param>
		/// <param name="InnerRadius">The inner radius of the ring.</param>
		/// <param name="OuterRadius">The outer radius of the ring.</param>
		/// <param name="StartAngle">The starting angle of the ring.</param>
		/// <param name="EndAngle">The ending angle of the ring.</param>
		/// <param name="Segments">The number of segments in the ring.</param>
		/// <param name="Colour">The Hex Value Of Colour</param>
		public static void DrawRing(Primitive Center, Primitive InnerRadius, Primitive OuterRadius, Primitive StartAngle, Primitive EndAngle, Primitive Segments, Primitive Colour)
		{
			try {
				string[] centerCoords = Center.ToString().Split('-');
				float innerRadius = float.Parse(InnerRadius.ToString());
				float outerRadius = float.Parse(OuterRadius.ToString());
				int startAngle = int.Parse(StartAngle.ToString());
				int endAngle = int.Parse(EndAngle.ToString());
				int segments = int.Parse(Segments.ToString());

				Raylib.DrawRing(
					new Vector2(int.Parse(centerCoords[0]), int.Parse(centerCoords[1])),
					innerRadius,
					outerRadius,
					startAngle,
					endAngle,
					segments,
					HexToRaylibColor(Colour)
				);
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
			}
		}

		/// <summary>
		/// Draw ring outline.
		/// </summary>
		/// <param name="Center">The X-Y position of the center of the ring.</param>
		/// <param name="InnerRadius">The inner radius of the ring.</param>
		/// <param name="OuterRadius">The outer radius of the ring.</param>
		/// <param name="StartAngle">The starting angle of the ring.</param>
		/// <param name="EndAngle">The ending angle of the ring.</param>
		/// <param name="Segments">The number of segments in the ring.</param>
		/// <param name="Colour">The Hex Value Of Colour</param>
		public static void DrawRingLines(Primitive Center, Primitive InnerRadius, Primitive OuterRadius, Primitive StartAngle, Primitive EndAngle, Primitive Segments, Primitive Colour)
		{
			try {
				string[] centerCoords = Center.ToString().Split('-');
				float innerRadius = float.Parse(InnerRadius.ToString());
				float outerRadius = float.Parse(OuterRadius.ToString());
				int startAngle = int.Parse(StartAngle.ToString());
				int endAngle = int.Parse(EndAngle.ToString());
				int segments = int.Parse(Segments.ToString());

				Raylib.DrawRingLines(
					new Vector2(int.Parse(centerCoords[0]), int.Parse(centerCoords[1])),
					innerRadius,
					outerRadius,
					startAngle,
					endAngle,
					segments,
					HexToRaylibColor(Colour)
				);
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
			}
		}

		/// <summary>
		/// Draw a color-filled rectangle.
		/// </summary>
		/// <param name="PosX">The X position of the rectangle.</param>
		/// <param name="PosY">The Y position of the rectangle.</param>
		/// <param name="Width">The width of the rectangle.</param>
		/// <param name="Height">The height of the rectangle.</param>
		/// <param name="Colour">The Hex Value Of Colour</param>
		public static void DrawRectangle(Primitive PosX, Primitive PosY, Primitive Width, Primitive Height, Primitive Colour)
		{
			try {
				int posX = int.Parse(PosX.ToString());
				int posY = int.Parse(PosY.ToString());
				int width = int.Parse(Width.ToString());
				int height = int.Parse(Height.ToString());

				Raylib.DrawRectangle(posX, posY, width, height, HexToRaylibColor(Colour));
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
			}
		}

		/// <summary>
		/// Draw a color-filled rectangle with pro parameters.
		/// </summary>
		/// <param name="Rec">The rectangle parameters (X, Y, Width, Height).</param>
		/// <param name="Origin">The origin point of the rectangle.</param>
		/// <param name="Rotation">The rotation of the rectangle.</param>
		/// <param name="Colour">The Hex Value Of Colour</param>
		public static void DrawRectanglePro(Primitive Rec, Primitive Origin, Primitive Rotation, Primitive Colour)
		{
			try {
				string[] recParams = Rec.ToString().Split('-');
				string[] originCoords = Origin.ToString().Split('-');
				float rotation = float.Parse(Rotation.ToString());

				Raylib.DrawRectanglePro(
					new Rectangle(float.Parse(recParams[0]), float.Parse(recParams[1]), float.Parse(recParams[2]), float.Parse(recParams[3])),
					new Vector2(float.Parse(originCoords[0]), float.Parse(originCoords[1])),
					rotation,
					HexToRaylibColor(Colour)
				);
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
			}
		}

		/// <summary>
		/// Draw a vertical-gradient-filled rectangle.
		/// </summary>
		/// <param name="PosX">The X position of the rectangle.</param>
		/// <param name="PosY">The Y position of the rectangle.</param>
		/// <param name="Width">The width of the rectangle.</param>
		/// <param name="Height">The height of the rectangle.</param>
		/// <param name="Colour1">The Hex Value Of the first color.</param>
		/// <param name="Colour2">The Hex Value Of the second color.</param>
		public static void DrawRectangleGradientV(Primitive PosX, Primitive PosY, Primitive Width, Primitive Height, Primitive Colour1, Primitive Colour2)
		{
			try {
				int posX = int.Parse(PosX.ToString());
				int posY = int.Parse(PosY.ToString());
				int width = int.Parse(Width.ToString());
				int height = int.Parse(Height.ToString());

				Raylib.DrawRectangleGradientV(posX, posY, width, height, HexToRaylibColor(Colour1), HexToRaylibColor(Colour2));
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
			}
		}

		/// <summary>
		/// Draw a horizontal-gradient-filled rectangle.
		/// </summary>
		/// <param name="PosX">The X position of the rectangle.</param>
		/// <param name="PosY">The Y position of the rectangle.</param>
		/// <param name="Width">The width of the rectangle.</param>
		/// <param name="Height">The height of the rectangle.</param>
		/// <param name="Colour1">The Hex Value Of the first color.</param>
		/// <param name="Colour2">The Hex Value Of the second color.</param>
		public static void DrawRectangleGradientH(Primitive PosX, Primitive PosY, Primitive Width, Primitive Height, Primitive Colour1, Primitive Colour2)
		{
			try {
				int posX = int.Parse(PosX.ToString());
				int posY = int.Parse(PosY.ToString());
				int width = int.Parse(Width.ToString());
				int height = int.Parse(Height.ToString());

				Raylib.DrawRectangleGradientH(posX, posY, width, height, HexToRaylibColor(Colour1), HexToRaylibColor(Colour2));
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
			}
		}

		/// <summary>
		/// Draw a gradient-filled rectangle with custom vertex colors.
		/// </summary>
		/// <param name="Rec">The rectangle parameters (X, Y, Width, Height).</param>
		/// <param name="Colour1">The Hex Value Of the first vertex color.</param>
		/// <param name="Colour2">The Hex Value Of the second vertex color.</param>
		/// <param name="Colour3">The Hex Value Of the third vertex color.</param>
		/// <param name="Colour4">The Hex Value Of the fourth vertex color.</param>
		public static void DrawRectangleGradientEx(Primitive Rec, Primitive Colour1, Primitive Colour2, Primitive Colour3, Primitive Colour4)
		{
			try {
				string[] recParams = Rec.ToString().Split('-');

				Raylib.DrawRectangleGradientEx(
					new Rectangle(float.Parse(recParams[0]), float.Parse(recParams[1]), float.Parse(recParams[2]), float.Parse(recParams[3])),
					HexToRaylibColor(Colour1),
					HexToRaylibColor(Colour2),
					HexToRaylibColor(Colour3),
					HexToRaylibColor(Colour4)
				);
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
			}
		}

		/// <summary>
		/// Draw rectangle outline.
		/// </summary>
		/// <param name="PosX">The X position of the rectangle.</param>
		/// <param name="PosY">The Y position of the rectangle.</param>
		/// <param name="Width">The width of the rectangle.</param>
		/// <param name="Height">The height of the rectangle.</param>
		/// <param name="Colour">The Hex Value Of Colour</param>
		public static void DrawRectangleLines(Primitive PosX, Primitive PosY, Primitive Width, Primitive Height, Primitive Colour)
		{
			try {
				int posX = int.Parse(PosX.ToString());
				int posY = int.Parse(PosY.ToString());
				int width = int.Parse(Width.ToString());
				int height = int.Parse(Height.ToString());

				Raylib.DrawRectangleLines(posX, posY, width, height, HexToRaylibColor(Colour));
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
			}
		}

		/// <summary>
		/// Draw rectangle outline with extended parameters.
		/// </summary>
		/// <param name="Rec">The rectangle parameters (X, Y, Width, Height).</param>
		/// <param name="LineThick">The thickness of the outline.</param>
		/// <param name="Colour">The Hex Value Of Colour</param>
		public static void DrawRectangleLinesEx(Primitive Rec, Primitive LineThick, Primitive Colour)
		{
			try {
				string[] recParams = Rec.ToString().Split('-');
				int lineThick = int.Parse(LineThick.ToString());

				Raylib.DrawRectangleLinesEx(
					new Rectangle(float.Parse(recParams[0]), float.Parse(recParams[1]), float.Parse(recParams[2]), float.Parse(recParams[3])),
					lineThick,
					HexToRaylibColor(Colour)
				);
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
			}
		}

		/// <summary>
		/// Draw rectangle with rounded edges.
		/// </summary>
		/// <param name="Rec">The rectangle parameters (X, Y, Width, Height).</param>
		/// <param name="Roundness">The roundness of the rectangle edges.</param>
		/// <param name="Segments">The number of segments in the rounded corners.</param>
		/// <param name="Colour">The Hex Value Of Colour</param>
		public static void DrawRectangleRounded(Primitive Rec, Primitive Roundness, Primitive Segments, Primitive Colour)
		{
			try {
				string[] recParams = Rec.ToString().Split('-');
				float roundness = float.Parse(Roundness.ToString());
				int segments = int.Parse(Segments.ToString());

				Raylib.DrawRectangleRounded(
					new Rectangle(float.Parse(recParams[0]), float.Parse(recParams[1]), float.Parse(recParams[2]), float.Parse(recParams[3])),
					roundness,
					segments,
					HexToRaylibColor(Colour)
				);
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
			}
		}

		/// <summary>
		/// Draw rectangle with rounded edges outline.
		/// </summary>
		/// <param name="Rec">The rectangle parameters (X, Y, Width, Height).</param>
		/// <param name="Roundness">The roundness of the rectangle edges.</param>
		/// <param name="Segments">The number of segments in the rounded corners.</param>
		/// <param name="LineThick">The thickness of the outline.</param>
		/// <param name="Colour">The Hex Value Of Colour</param>
		public static void DrawRectangleRoundedLines(Primitive Rec, Primitive Roundness, Primitive Segments, Primitive LineThick, Primitive Colour)
		{
			try {
				string[] recParams = Rec.ToString().Split('-');
				float roundness = float.Parse(Roundness.ToString());
				int segments = int.Parse(Segments.ToString());
				int lineThick = int.Parse(LineThick.ToString());

				Raylib.DrawRectangleRoundedLines(
					new Rectangle(float.Parse(recParams[0]), float.Parse(recParams[1]), float.Parse(recParams[2]), float.Parse(recParams[3])),
					roundness,
					segments,
					lineThick,
					HexToRaylibColor(Colour)
				);
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
			}
		}

		/// <summary>
		/// Draw a color-filled triangle (vertex in counter-clockwise order!).
		/// </summary>
		/// <param name="V1">The first vertex of the triangle.</param>
		/// <param name="V2">The second vertex of the triangle.</param>
		/// <param name="V3">The third vertex of the triangle.</param>
		/// <param name="Colour">The Hex Value Of Colour</param>
		public static void DrawTriangle(Primitive V1, Primitive V2, Primitive V3, Primitive Colour)
		{
			try {
				string[] v1Coords = V1.ToString().Split('-');
				string[] v2Coords = V2.ToString().Split('-');
				string[] v3Coords = V3.ToString().Split('-');

				Raylib.DrawTriangle(
					new Vector2(float.Parse(v1Coords[0]), float.Parse(v1Coords[1])),
					new Vector2(float.Parse(v2Coords[0]), float.Parse(v2Coords[1])),
					new Vector2(float.Parse(v3Coords[0]), float.Parse(v3Coords[1])),
					HexToRaylibColor(Colour)
				);
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
			}
		}

		/// <summary>
		/// Draw a triangle fan defined by points (first vertex is the center).
		/// </summary>
		/// <param name="Points">The array of triangle fan points.</param>
		/// <param name="NumPoints">The number of points.</param>
		/// <param name="Colour">The Hex Value Of Colour</param>
		public static void DrawTriangleFan(Primitive Points, Primitive NumPoints, Primitive Colour)
		{
			try {
				string[] pointArray = Points.ToString().Split(';');
				int numPoints = int.Parse(NumPoints.ToString());
				Vector2[] vectors = new Vector2[numPoints];

				for (int i = 0; i < numPoints; i++) {
					string[] coords = pointArray[i].Split('-');
					vectors[i] = new Vector2(float.Parse(coords[0].Split('=')[1]), float.Parse(coords[1]));
				}

				Raylib.DrawTriangleFan(vectors, numPoints, HexToRaylibColor(Colour));
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
			}
		}

		/// <summary>
		/// Draw a triangle strip defined by points.
		/// </summary>
		/// <param name="Points">The array of triangle strip points.</param>
		/// <param name="PointsCount">The number of points.</param>
		/// <param name="Colour">The Hex Value Of Colour</param>
		public static void DrawTriangleStrip(Primitive Points, Primitive PointsCount, Primitive Colour)
		{
			try {
				string[] pointArray = Points.ToString().Split(';');
				int pointsCount = int.Parse(PointsCount.ToString());
				Vector2[] vectors = new Vector2[pointsCount];

				for (int i = 0; i < pointsCount; i++) {
					string[] coords = pointArray[i].Split('-');
					vectors[i] = new Vector2(float.Parse(coords[0].Split('=')[1]), float.Parse(coords[1]));
				}

				// Pass the first element of the array as ref
				Raylib.DrawTriangleStrip(ref vectors[0], pointsCount, HexToRaylibColor(Colour));
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
			}
		}


		/// <summary>
		/// Draw a regular polygon (Vector version).
		/// </summary>
		/// <param name="Center">The center point of the polygon.</param>
		/// <param name="Sides">The number of sides.</param>
		/// <param name="Radius">The radius of the polygon.</param>
		/// <param name="Rotation">The rotation of the polygon.</param>
		/// <param name="Colour">The Hex Value Of Colour</param>
		public static void DrawPoly(Primitive Center, Primitive Sides, Primitive Radius, Primitive Rotation, Primitive Colour)
		{
			try {
				string[] centerCoords = Center.ToString().Split('-');
				int sides = int.Parse(Sides.ToString());
				float radius = float.Parse(Radius.ToString());
				float rotation = float.Parse(Rotation.ToString());

				Raylib.DrawPoly(
					new Vector2(float.Parse(centerCoords[0]), float.Parse(centerCoords[1])),
					sides,
					radius,
					rotation,
					HexToRaylibColor(Colour)
				);
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
			}
		}

		/// <summary>
		/// Draw a polygon outline of n sides.
		/// </summary>
		/// <param name="Center">The center point of the polygon.</param>
		/// <param name="Sides">The number of sides.</param>
		/// <param name="Radius">The radius of the polygon.</param>
		/// <param name="Rotation">The rotation of the polygon.</param>
		/// <param name="Colour">The Hex Value Of Colour</param>
		public static void DrawPolyLines(Primitive Center, Primitive Sides, Primitive Radius, Primitive Rotation, Primitive Colour)
		{
			try {
				string[] centerCoords = Center.ToString().Split('-');
				int sides = int.Parse(Sides.ToString());
				float radius = float.Parse(Radius.ToString());
				float rotation = float.Parse(Rotation.ToString());

				Raylib.DrawPolyLines(
					new Vector2(float.Parse(centerCoords[0]), float.Parse(centerCoords[1])),
					sides,
					radius,
					rotation,
					HexToRaylibColor(Colour)
				);
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
			}
		}

		/// <summary>
		/// Check collision between two rectangles.
		/// </summary>
		/// <param name="Rec1">First rectangle, provide X, Y, Width, and Height (e.g., "x-y-width-height").</param>
		/// <param name="Rec2">Second rectangle, provide X, Y, Width, and Height (e.g., "x-y-width-height").</param>
		/// <returns>True if the rectangles collide, false otherwise.</returns>
		public static Primitive CheckCollisionRecs(Primitive Rec1, Primitive Rec2)
		{
			try {
				string[] rec1Coords = Rec1.ToString().Split('-');
				string[] rec2Coords = Rec2.ToString().Split('-');

				Rectangle rectangle1 = new Rectangle(
					                       float.Parse(rec1Coords[0]), float.Parse(rec1Coords[1]),
					                       float.Parse(rec1Coords[2]), float.Parse(rec1Coords[3])
				                       );

				Rectangle rectangle2 = new Rectangle(
					                       float.Parse(rec2Coords[0]), float.Parse(rec2Coords[1]),
					                       float.Parse(rec2Coords[2]), float.Parse(rec2Coords[3])
				                       );

				return Raylib.CheckCollisionRecs(rectangle1, rectangle2);
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
				return false;
			}
		}

		/// <summary>
		/// Check collision between two circles.
		/// </summary>
		/// <param name="Center1">Center of the first circle, provide X and Y coordinates (e.g., "x-y").</param>
		/// <param name="Radius1">Radius of the first circle.</param>
		/// <param name="Center2">Center of the second circle, provide X and Y coordinates (e.g., "x-y").</param>
		/// <param name="Radius2">Radius of the second circle.</param>
		/// <returns>True if the circles collide, false otherwise.</returns>
		public static Primitive CheckCollisionCircles(Primitive Center1, Primitive Radius1, Primitive Center2, Primitive Radius2)
		{
			try {
				string[] center1Coords = Center1.ToString().Split('-');
				string[] center2Coords = Center2.ToString().Split('-');

				Vector2 center1 = new Vector2(float.Parse(center1Coords[0]), float.Parse(center1Coords[1]));
				Vector2 center2 = new Vector2(float.Parse(center2Coords[0]), float.Parse(center2Coords[1]));
				float radius1 = float.Parse(Radius1.ToString());
				float radius2 = float.Parse(Radius2.ToString());

				return Raylib.CheckCollisionCircles(center1, radius1, center2, radius2);
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
				return false;
			}
		}

		/// <summary>
		/// Check collision between a circle and a rectangle.
		/// </summary>
		/// <param name="Center">Center of the circle, provide X and Y coordinates (e.g., "x-y").</param>
		/// <param name="Radius">Radius of the circle.</param>
		/// <param name="Rec">The rectangle, provide X, Y, Width, and Height (e.g., "x-y-width-height").</param>
		/// <returns>True if the circle collides with the rectangle, false otherwise.</returns>
		public static Primitive CheckCollisionCircleRec(Primitive Center, Primitive Radius, Primitive Rec)
		{
			try {
				string[] centerCoords = Center.ToString().Split('-');
				string[] recCoords = Rec.ToString().Split('-');

				Vector2 center = new Vector2(float.Parse(centerCoords[0]), float.Parse(centerCoords[1]));
				float radius = float.Parse(Radius.ToString());
				Rectangle rectangle = new Rectangle(
					                      float.Parse(recCoords[0]), float.Parse(recCoords[1]),
					                      float.Parse(recCoords[2]), float.Parse(recCoords[3])
				                      );

				return Raylib.CheckCollisionCircleRec(center, radius, rectangle);
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
				return false;
			}
		}

		/// <summary>
		/// Check if a point is inside a rectangle.
		/// </summary>
		/// <param name="Point">The point, provide X and Y coordinates (e.g., "x-y").</param>
		/// <param name="Rec">The rectangle, provide X, Y, Width, and Height (e.g., "x-y-width-height").</param>
		/// <returns>True if the point is inside the rectangle, false otherwise.</returns>
		public static Primitive CheckCollisionPointRec(Primitive Point, Primitive Rec)
		{
			try {
				string[] pointCoords = Point.ToString().Split('-');
				string[] recCoords = Rec.ToString().Split('-');

				Vector2 point = new Vector2(float.Parse(pointCoords[0]), float.Parse(pointCoords[1]));
				Rectangle rectangle = new Rectangle(
					                      float.Parse(recCoords[0]), float.Parse(recCoords[1]),
					                      float.Parse(recCoords[2]), float.Parse(recCoords[3])
				                      );

				return Raylib.CheckCollisionPointRec(point, rectangle);
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
				return false;
			}
		}

		/// <summary>
		/// Check if a point is inside a circle.
		/// </summary>
		/// <param name="Point">The point, provide X and Y coordinates (e.g., "x-y").</param>
		/// <param name="Center">The center of the circle, provide X and Y coordinates (e.g., "x-y").</param>
		/// <param name="Radius">Radius of the circle.</param>
		/// <returns>True if the point is inside the circle, false otherwise.</returns>
		public static Primitive CheckCollisionPointCircle(Primitive Point, Primitive Center, Primitive Radius)
		{
			try {
				string[] pointCoords = Point.ToString().Split('-');
				string[] centerCoords = Center.ToString().Split('-');

				Vector2 point = new Vector2(float.Parse(pointCoords[0]), float.Parse(pointCoords[1]));
				Vector2 center = new Vector2(float.Parse(centerCoords[0]), float.Parse(centerCoords[1]));
				float radius = float.Parse(Radius.ToString());

				return Raylib.CheckCollisionPointCircle(point, center, radius);
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
				return false;
			}
		}

		/// <summary>
		/// Check if a point is inside a triangle.
		/// </summary>
		/// <param name="Point">The point, provide X and Y coordinates (e.g., "x-y").</param>
		/// <param name="P1">The first vertex of the triangle, provide X and Y coordinates (e.g., "x-y").</param>
		/// <param name="P2">The second vertex of the triangle, provide X and Y coordinates (e.g., "x-y").</param>
		/// <param name="P3">The third vertex of the triangle, provide X and Y coordinates (e.g., "x-y").</param>
		/// <returns>True if the point is inside the triangle, false otherwise.</returns>
		public static Primitive CheckCollisionPointTriangle(Primitive Point, Primitive P1, Primitive P2, Primitive P3)
		{
			try {
				string[] pointCoords = Point.ToString().Split('-');
				string[] p1Coords = P1.ToString().Split('-');
				string[] p2Coords = P2.ToString().Split('-');
				string[] p3Coords = P3.ToString().Split('-');

				Vector2 point = new Vector2(float.Parse(pointCoords[0]), float.Parse(pointCoords[1]));
				Vector2 p1 = new Vector2(float.Parse(p1Coords[0]), float.Parse(p1Coords[1]));
				Vector2 p2 = new Vector2(float.Parse(p2Coords[0]), float.Parse(p2Coords[1]));
				Vector2 p3 = new Vector2(float.Parse(p3Coords[0]), float.Parse(p3Coords[1]));

				return Raylib.CheckCollisionPointTriangle(point, p1, p2, p3);
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
				return false;
			}
		}

		/// <summary>
		/// Get the collision rectangle for two rectangles.
		/// </summary>
		/// <param name="Rec1">First rectangle, provide X, Y, Width, and Height (e.g., "x-y-width-height").</param>
		/// <param name="Rec2">Second rectangle, provide X, Y, Width, and Height (e.g., "x-y-width-height").</param>
		/// <returns>The collision rectangle as "x-y-width-height".</returns>
		public static Primitive GetCollisionRec(Primitive Rec1, Primitive Rec2)
		{
			try {
				string[] rec1Coords = Rec1.ToString().Split('-');
				string[] rec2Coords = Rec2.ToString().Split('-');

				Rectangle rectangle1 = new Rectangle(
					                           float.Parse(rec1Coords[0]), float.Parse(rec1Coords[1]),
					                           float.Parse(rec1Coords[2]), float.Parse(rec1Coords[3])
				                           );

				Rectangle rectangle2 = new Rectangle(
					                           float.Parse(rec2Coords[0]), float.Parse(rec2Coords[1]),
					                           float.Parse(rec2Coords[2]), float.Parse(rec2Coords[3])
				                           );

				Rectangle collisionRec = Raylib.GetCollisionRec(rectangle1, rectangle2);
				return rec1Coords[0] + "-" + rec1Coords[1] + "-" + rec1Coords[2] + "-" + rec1Coords[3];
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
				return "";
			}
		}
	}
}