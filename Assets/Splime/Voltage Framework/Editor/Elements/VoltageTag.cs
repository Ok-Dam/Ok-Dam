using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voltage
{
	public class VoltageTag : VoltageElement
	{
		private string m_tag = "";
		public string Tag
		{
			get
			{
				return m_tag;
			}
			set
			{
				m_tag = value;
			}
		}

		public VoltageTag(string tag)
		{
			Tag = tag;
			Style = ValidateStyle("Minipopup", "Minipopup");
		}
		public VoltageTag(string tag, ElementSettings elementSettings) : this(tag)
		{
			ElementSettings = elementSettings;
		}

		public VoltageTag(string tag, GUIStyle style) : this(tag)
		{
			Style = ValidateStyle(style, "Minipopup", "Minipopup");
		}
		public VoltageTag(string tag, ElementSettings elementSettings, GUIStyle style) : this(tag, elementSettings)
		{
			Style = ValidateStyle(style, "Minipopup", "Minipopup");
		}

		/// <summary>
		/// Do not use this.
		/// </summary>
		/// <param name="workingArea"></param>
		public override void DrawElement(Rect workingArea)
		{
			base.DrawElement(workingArea);
			Tag = EditorGUI.TagField(WorkingArea, Tag, Style);
		}
	}
}
