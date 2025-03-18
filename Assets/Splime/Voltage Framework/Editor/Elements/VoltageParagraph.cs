using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voltage
{
	public class VoltageParagraph : VoltageElement
	{
		#region CONSTRUCTORS
		public VoltageParagraph(string content)
		{
			Content = new GUIContent(content);
			Style = ValidateStyle("Paragraph", "WordWrapLabel");
		}
		public VoltageParagraph(string content, GUIStyle style)
		{
			Content = new GUIContent(content);
			Style = ValidateStyle(style, "Paragraph", "WordWrapLabel");
		}
		public VoltageParagraph(string content, ElementSettings elementSettings)
		{
			Content = new GUIContent(content);
			ElementSettings = elementSettings;
			Style = ValidateStyle("Paragraph", "WordWrapLabel");
		}
		public VoltageParagraph(string content, ElementSettings elementSettings, GUIStyle style)
		{
			Content = new GUIContent(content);
			ElementSettings = elementSettings;
			Style = ValidateStyle(style, "Paragraph", "WordWrapLabel");
		}

		public VoltageParagraph(GUIContent content)
		{
			Content = content;
			Style = ValidateStyle("Paragraph", "WordWrapLabel");
		}
		public VoltageParagraph(GUIContent content, GUIStyle style)
		{
			Content = content;
			Style = ValidateStyle(style, "Paragraph", "WordWrapLabel");
		}
		public VoltageParagraph(GUIContent content, ElementSettings elementSettings)
		{
			Content = content;
			ElementSettings = elementSettings;
			Style = ValidateStyle("Paragraph", "WordWrapLabel");
		}
		public VoltageParagraph(GUIContent content, ElementSettings elementSettings, GUIStyle style)
		{
			Content = content;
			ElementSettings = elementSettings;
			Style = ValidateStyle(style, "Paragraph", "WordWrapLabel");
		}
		#endregion
		public override float CalcWidth()
		{
			return 260f;
		}
		public override float CalcHeight(float width)
		{
			width = Mathf.Max(0f, width);
			float height = Style.CalcHeight(Content, width);

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

			EditorGUI.LabelField(WorkingArea, Content, Style);
		}
	}
}
