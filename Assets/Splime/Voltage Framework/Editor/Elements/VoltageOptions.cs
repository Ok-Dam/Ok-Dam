using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace Voltage
{
	public class VoltageOptions : VoltageElement
	{
		private int m_value = 0;
		private string[] m_labels = new string[0];

		private GUIStyle m_onStyle;
		private GUIStyle m_offStyle;

		public int Value
		{
			get
			{
				return m_value;
			}
			set
			{
				m_value = value;
			}
		}

		public string[] Labels
		{
			get
			{
				return m_labels;
			}
			set
			{
				m_labels = value;
			}
		}

		public GUIStyle OnStyle
		{
			get
			{
				return m_onStyle;
			}
			set
			{
				m_onStyle = value;
			}
		}
		public GUIStyle OffStyle
		{
			get
			{
				return m_offStyle;
			}
			set
			{
				m_offStyle = value;
			}
		}

		public VoltageOptions(int value, string[] labels)
		{
			Value = value;
			Labels = labels;

			Style = ValidateStyle("Label", "Label");
			OnStyle = ValidateStyle("OptionOn", "VisibilityToggle");
			OffStyle = ValidateStyle("OptionOff", "Toggle");
		}
		public VoltageOptions(int value, string[] labels, ElementSettings settings) : this(value, labels)
		{
			ElementSettings = settings;
		}

		public VoltageOptions(int value, string[] labels, GUIStyle labelStyle, GUIStyle onStyle, GUIStyle offStyle) : this(value, labels)
		{
			Style = ValidateStyle(labelStyle, "Label", "Label");
			OnStyle = ValidateStyle(onStyle, "OptionOn", "VisibilityToggle");
			OffStyle = ValidateStyle(offStyle, "OptionOff", "Toggle");
		}
		public VoltageOptions(int value, string[] labels, ElementSettings settings, GUIStyle labelStyle, GUIStyle onStyle, GUIStyle offStyle) : this(value, labels, settings)
		{
			Style = ValidateStyle(labelStyle, "Label", "Label");
			OnStyle = ValidateStyle(onStyle, "OptionOn", "VisibilityToggle");
			OffStyle = ValidateStyle(offStyle, "OptionOff", "Toggle");
		}

		public override float CalcWidth()
		{
			if (FixedWidth > 0f)
				return FixedWidth;

			float width = 0f;
			for (int i = 0; i < Labels.Length; i++)
			{
				width = Mathf.Max(width, (i == Value ? OnStyle.CalcSize(new GUIContent("")).x : OffStyle.CalcSize(new GUIContent("")).x) + Style.CalcSize(new GUIContent(Labels[i])).x);
			}
			return width;

		}

		public override float CalcHeight(float width)
		{
			if (FixedHeight > 0f)
				return FixedHeight;

			float height = 0f;
			for (int i = 0; i < Labels.Length; i++)
			{
				height += (i == Value ? OnStyle.CalcSize(new GUIContent("")).y : OffStyle.CalcSize(new GUIContent("")).y);
			}
			return height;

		}
		/// <summary>
		/// Do not use this.
		/// </summary>
		/// <param name="workingArea"></param>
		public override void DrawElement(Rect workingArea)
		{
			base.DrawElement(workingArea);
			Rect currentPos = WorkingArea;

			for (int i = 0; i < Labels.Length; i++)
			{
				currentPos.height = (i == Value ? OnStyle.CalcSize(new GUIContent("")).y : OffStyle.CalcSize(new GUIContent("")).y);
				if (GUI.Button(currentPos, Labels[i], (Value == i ? OnStyle : OffStyle)))
				{
					Value = i;
				}
				currentPos.y += currentPos.height;
			}

		}
	}
}
