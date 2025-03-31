using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voltage
{
	public class VoltageTextArea : VoltageElement
	{
		private string m_text = "";
		public string Text
		{
			get
			{
				return m_text;
			}
			set
			{
				m_text = value;
			}
		}

		public VoltageTextArea(string text)
		{
			Text = text;
			Style = ValidateStyle("Textfield", "Textfield");
		}

		public VoltageTextArea(string text, GUIStyle style) : this(text)
		{
			Text = text;
			Style = ValidateStyle(style, "Textfield", "Textfield");
		}

		public VoltageTextArea(string text, ElementSettings elementSettings) : this(text)
		{
			Text = text;
			ElementSettings = elementSettings;
			Style = ValidateStyle("Textfield", "Textfield");
		}
		public VoltageTextArea(string text, ElementSettings elementSettings, GUIStyle style) : this(text, elementSettings)
		{
			Text = text;
			ElementSettings = elementSettings;
			Style = ValidateStyle(style, "Textfield", "Textfield");
		}
		/// <summary>
		/// Calculates the height of the element.
		/// </summary>
		/// <returns></returns>
		public override float CalcHeight(float width)
		{
			return Style.CalcHeight(new GUIContent(Text),width);
		}
		
		/// <summary>
		/// Do not use this.
		/// </summary>
		/// <param name="workingArea"></param>
		public override void DrawElement(Rect workingArea)
		{
			base.DrawElement(workingArea);
			Text = EditorGUI.TextArea(WorkingArea, Text, Style);
		}
	}
}
