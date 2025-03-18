using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voltage
{
	public class VoltagePassword : VoltageElement
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

		public VoltagePassword(string text)
		{
			Text = text;
			Style = ValidateStyle("Textfield", "Textfield");
		}
		public VoltagePassword(string text, GUIStyle style) : this(text)
		{
			Style = ValidateStyle(style, "Textfield", "Textfield");
		}
		public VoltagePassword(string text, ElementSettings elementSettings) : this(text)
		{
			ElementSettings = elementSettings;
		}
		public VoltagePassword(string text, ElementSettings elementSettings, GUIStyle style) : this(text, elementSettings)
		{
			Style = ValidateStyle(style, "Textfield", "Textfield");
		}

		/// <summary>
		/// Do not use this.
		/// </summary>
		/// <param name="workingArea"></param>
		public override void DrawElement(Rect workingArea)
		{
			base.DrawElement(workingArea);

			Text = EditorGUI.PasswordField(WorkingArea, Text, Style);
		}
	}
}
