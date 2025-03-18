using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voltage
{
	public class VoltageText : VoltageElement
	{
		private string m_text = "";
		private bool m_delayed = true;
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

		public VoltageText(string text)
		{
			Text = text;
			Style = ValidateStyle("Textfield", "Textfield");
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		/// <param name="delayed">Will the field change value only after editing is finalized.</param>
		public VoltageText(string text,bool delayed)
		{
			Text = text;
			m_delayed = delayed;
			Style = ValidateStyle("Textfield", "Textfield");
		}
		public VoltageText(string text, ElementSettings elementSettings) : this(text)
		{
			ElementSettings = elementSettings;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		/// <param name="delayed">Will the field change value only after editing is finalized.</param>
		/// <param name="elementSettings"></param>
		public VoltageText(string text, bool delayed, ElementSettings elementSettings) : this(text, delayed)
		{
			ElementSettings = elementSettings;
		}
		public VoltageText(string text, GUIStyle style) : this(text)
		{
			Style = ValidateStyle(style, "Textfield", "Textfield");
		}
		public VoltageText(string text, ElementSettings elementSettings, GUIStyle style) : this(text, elementSettings)
		{
			Style = ValidateStyle(style, "Textfield", "Textfield");
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		/// <param name="delayed">Will the field change value only after editing is finalized.</param>
		/// <param name="elementSettings"></param>
		/// <param name="style"></param>
		public VoltageText(string text, bool delayed, ElementSettings elementSettings, GUIStyle style) : this(text, delayed, elementSettings)
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
			if (m_delayed)
			{
				Text = EditorGUI.DelayedTextField(WorkingArea, Text, Style);
			}
			else
			{
				Text = EditorGUI.TextField(WorkingArea, Text, Style);
			}
		}
	}
}
