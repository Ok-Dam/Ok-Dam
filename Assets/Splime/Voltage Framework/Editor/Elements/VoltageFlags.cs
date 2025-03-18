using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace Voltage
{
	public class VoltageFlags : VoltageElement
	{
        private int m_flag = 1;
        private string[] m_labels;

        public int Flag
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

        public VoltageFlags(int flag, string[] labels)
		{
            Flag = flag;
            Labels = labels;
			Style = ValidateStyle("Miniflag", "Minipopup");
		}
		public VoltageFlags(int flag, string[] labels, ElementSettings settings) : this(flag, labels)
		{
			ElementSettings = settings;
		}

		public VoltageFlags(int flag, string[] labels, GUIStyle style) : this(flag, labels)
		{
			Style = ValidateStyle(style, "Miniflag", "Minipopup");
		}
		public VoltageFlags(int flag, string[] labels, ElementSettings settings, GUIStyle style) : this(flag, labels, settings)
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
            Flag = EditorGUI.MaskField(WorkingArea, Flag, Labels, Style);
		}
	}
}
