using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voltage
{
	public class VoltageEnumPopup : VoltageElement
	{

		public System.Enum value;

		public VoltageEnumPopup(System.Enum enumList)
		{
			value = enumList;
			Style = ValidateStyle("Minipopup", "Minipopup");
		}
		public VoltageEnumPopup(System.Enum enumList, ElementSettings settings) : this(enumList)
		{
			ElementSettings = settings;
		}
		
		public VoltageEnumPopup(System.Enum enumList, GUIStyle style) : this(enumList)
		{
			Style = ValidateStyle(style,"Minipopup", "Minipopup");
		}
		public VoltageEnumPopup(System.Enum enumList, ElementSettings settings, GUIStyle style) : this(enumList,settings)
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

			value = EditorGUI.EnumPopup(WorkingArea, value, Style);
		}
	}
}
