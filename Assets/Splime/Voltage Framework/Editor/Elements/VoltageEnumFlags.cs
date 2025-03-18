using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace Voltage
{
	public class VoltageEnumFlags : VoltageElement
	{
		private Enum m_flag;
		public Enum Flag
		{
			get
			{
				return m_flag;
			}
			set
			{
				m_flag = value;
			}
		}

		public VoltageEnumFlags(Enum flag)
		{
			Flag = flag;
			Style = ValidateStyle("Miniflag", "Minipopup");
		}
		public VoltageEnumFlags(Enum flag, ElementSettings settings) : this(flag)
		{
			ElementSettings = settings;
		}

		public VoltageEnumFlags(Enum flag, GUIStyle style) : this(flag)
		{
			Style = ValidateStyle(style, "Miniflag", "Minipopup");
		}
		public VoltageEnumFlags(Enum flag, ElementSettings settings, GUIStyle style) : this(flag, settings)
		{
			Style = ValidateStyle(style, "Miniflag", "Minipopup");
		}

		/// <summary>
		/// Do not use this.
		/// </summary>
		/// <param name="workingArea"></param>
		public override void DrawElement(Rect workingArea)
		{
			base.DrawElement(workingArea);
			Flag = EditorGUI.EnumMaskField(WorkingArea, Flag, Style);
		}
	}
}
