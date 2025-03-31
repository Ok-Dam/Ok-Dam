using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voltage
{
	public class VoltageImage : VoltageElement
	{
		private Texture m_texture = null;
		private Rect m_coordinates = new Rect(0, 0, 0, 0);
		private bool hasCoordinates = false;

		public Texture Image
		{
			get { return m_texture; }
			set { m_texture = value; }
		}
		/// <summary>
		/// Use normalized coordinates for the texture (0f = min, 1f = max, 2f = 2*max)
		/// </summary>
		public Rect Coordinates
		{
			get { return m_coordinates; }
			set { m_coordinates = value; hasCoordinates = Mathf.Abs(m_coordinates.x + m_coordinates.y + m_coordinates.width + m_coordinates.height) > 0;}
		}

		public VoltageImage(Texture image)
		{
			Image = image;
			hasCoordinates = false;
		}
		/// <summary>
		/// Use normalized coordinates for the texture (0f = min, 1f = max, 2f = 2*max)
		/// </summary>
		/// <param name="image"></param>
		/// <param name="imageCoordinates"></param>
		public VoltageImage(Texture image, Rect imageCoordinates)
		{
			Image = image;
			Coordinates = imageCoordinates;
		}
		public VoltageImage(Texture image, ElementSettings settings) : this(image)
		{
			ElementSettings = settings;
		}
		/// <summary>
		/// Use normalized coordinates for the texture (0f = min, 1f = max, 2f = 2*max)
		/// </summary>
		/// <param name="image"></param>
		/// <param name="imageCoordinates"></param>
		/// <param name="settings"></param>
		public VoltageImage(Texture image, Rect imageCoordinates, ElementSettings settings) : this(image, imageCoordinates)
		{
			ElementSettings = settings;
		}
		/// <summary>
		/// Calculates the width of the element.
		/// </summary>
		/// <returns></returns>
		public override float CalcWidth()
		{
			if (FixedWidth > 0f)
				return FixedWidth;
			else if (hasCoordinates && Image != null)
				return Coordinates.width * Image.width;
			else if (Image != null)
				return Image.width;
			return 0f;
		}
		/// <summary>
		/// Calculates the height of the element.
		/// </summary>
		/// <returns></returns>
		public override float CalcHeight(float width)
		{
			if (FixedHeight > 0f)
				return FixedHeight;
			else if (hasCoordinates && Image != null)
				return Coordinates.height * Image.height;
			else if(Image != null)
				return Image.height;
			return 0f;
		}

		/// <summary>
		/// Do not use this.
		/// </summary>
		/// <param name="workingArea"></param>
		public override void DrawElement(Rect workingArea)
		{
			base.DrawElement(workingArea);

			
			if (Image != null)
			{
				if (!hasCoordinates)
					GUI.DrawTexture(WorkingArea, Image);
				else
					GUI.DrawTextureWithTexCoords(WorkingArea, Image, Coordinates);
			}
			else
			{
				GUI.DrawTexture(WorkingArea, (Texture)(new Texture2D((int)WorkingArea.width, (int)WorkingArea.height)));
			}
		}
	}
}
