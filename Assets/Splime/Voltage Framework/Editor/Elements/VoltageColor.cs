using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voltage
{
	public class VoltageColor : VoltageElement
	{
		private Color m_color = Color.white;

		public Color Color
		{
			get
			{
				return m_color;
			}
			set
			{
				m_color = value;
			}
		}

		public VoltageColor(Color color)
		{
			Color = color;
		}
		public VoltageColor(Color color, ElementSettings settings) : this(color)
		{
			ElementSettings = settings;
		}

		/// <summary>
		/// Do not use this.
		/// </summary>
		/// <param name="workingArea"></param>
		public override void DrawElement(Rect workingArea)
		{
			base.DrawElement(workingArea);
			Color = EditorGUI.ColorField(WorkingArea, Color);
		}
	}
}
