using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voltage
{
	public class VoltageLabel : VoltageElement
	{
		#region CONSTRUCTORS
		public VoltageLabel(string content)
		{
			Content = new GUIContent(content);
			Style = ValidateStyle("Label","Label");
		}
		public VoltageLabel(string content, GUIStyle style)
		{
			Content = new GUIContent(content);
			Style = ValidateStyle(style,"Label", "Label");
		}
		public VoltageLabel(string content, ElementSettings elementSettings)
		{
			Content = new GUIContent(content);
			ElementSettings = elementSettings;
			Style = ValidateStyle("Label", "Label");
		}
		public VoltageLabel(string content, ElementSettings elementSettings, GUIStyle style)
		{
			Content = new GUIContent(content);
			ElementSettings = elementSettings;
			Style = ValidateStyle(style, "Label", "Label");
		}

		public VoltageLabel(GUIContent content)
		{
			Content = content;
			Style = ValidateStyle("Label", "Label");
		}
		public VoltageLabel(GUIContent content, GUIStyle style)
		{
			Content = content;
			Style = ValidateStyle(style, "Label", "Label");
		}
		public VoltageLabel(GUIContent content, ElementSettings elementSettings)
		{
			Content = content;
			ElementSettings = elementSettings;
			Style = ValidateStyle("Label", "Label");
		}
		public VoltageLabel(GUIContent content, ElementSettings elementSettings, GUIStyle style)
		{
			Content = content;
			ElementSettings = elementSettings;
			Style = ValidateStyle(style, "Label", "Label");
		}
		#endregion

		/// <summary>
		/// Do not use this.
		/// </summary>
		/// <param name="workingArea"></param>
		public override void DrawElement(Rect _workingArea)
		{
			base.DrawElement(_workingArea);

			EditorGUI.LabelField(WorkingArea, Content, Style);
		}
	}
}
