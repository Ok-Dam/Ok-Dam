using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voltage
{
	public class VoltageRichTextArea : VoltageElement
	{
		private string m_text = "";
		private GUIStyle m_richStyle;

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
		public GUIStyle RichStyle
		{
			get
			{
				return m_richStyle;
			}
			set
			{
				m_richStyle = value;
			}
		}
		public VoltageRichTextArea(string text)
		{
			Text = text;
			Style = ValidateStyle("Textfield", "Textfield");
			RichStyle = ValidateStyle("Paragraph", "Wrap Label");
		}

		public VoltageRichTextArea(string text, GUIStyle style) : this(text)
		{
			Text = text;
			Style = ValidateStyle(style, "Textfield", "Textfield");
		}

		public VoltageRichTextArea(string text, ElementSettings elementSettings) : this(text)
		{
			Text = text;
			ElementSettings = elementSettings;

		}
		public VoltageRichTextArea(string text, ElementSettings elementSettings, GUIStyle style) : this(text, elementSettings)
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

			width = Mathf.Max(0f, width);
			float height = Style.CalcHeight(new GUIContent(Text), width) + Style.CalcHeight(new GUIContent(Text), width);

			if (FixedSize.y > 0f)
				height = FixedSize.y;

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
			currentPos.height = Style.CalcHeight(new GUIContent(Text), currentPos.width);
			Text = EditorGUI.TextArea(currentPos, Text, Style);

			currentPos.y += currentPos.height;
			currentPos.height = RichStyle.CalcHeight(new GUIContent(Text), currentPos.width);
			EditorGUI.LabelField(currentPos, Text, RichStyle);
		}
	}
}
