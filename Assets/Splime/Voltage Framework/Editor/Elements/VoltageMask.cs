using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voltage
{
	public class VoltageMask : VoltageElement
	{
		private int m_mask = 0;
		private string[] m_maskLabels = new string[0];

		public int Mask
		{
			get
			{
				return m_mask;
			}
			set
			{
				m_mask = value;
			}
		}
		public string[] MaskLabels
		{
			get
			{
				return m_maskLabels;
			}
			set
			{
				m_maskLabels = value;
			}
		}
		public VoltageMask(int mask, string[] maskLabels)
		{
			Mask = mask;
			MaskLabels = maskLabels;
			Style = ValidateStyle("Miniflag", "Minipopup");
		}
		public VoltageMask(int mask, string[] maskLabels, ElementSettings elementSettings) : this(mask, maskLabels)
		{
			ElementSettings = elementSettings;
		}

		public VoltageMask(int mask, string[] maskLabels, GUIStyle style) : this(mask, maskLabels)
		{
			Style = ValidateStyle(style, "Miniflag", "Minipopup");
		}
		public VoltageMask(int mask, string[] maskLabels, ElementSettings elementSettings, GUIStyle style) : this(mask, maskLabels, elementSettings)
		{
			Style = ValidateStyle(style, "Miniflag", "Minipopup");
		}

		/// <summary>
		/// Do not use this.
		/// </summary>
		/// <param name="workingArea"></param>
		public override void DrawElement(Rect _workingArea)
		{
			base.DrawElement(_workingArea);
			Mask = EditorGUI.MaskField(WorkingArea, Mask,MaskLabels, Style);
		}
	}
}
