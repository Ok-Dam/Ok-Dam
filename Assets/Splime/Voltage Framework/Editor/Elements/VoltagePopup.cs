using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voltage
{
	public class VoltagePopup : VoltageElement
	{

        private int m_index;
        private string[] m_labels;

        public int Selected
        {
            get
            {
                return m_index;
            }
            set
            {
                m_index = value;
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

		public VoltagePopup(int index, string[] labels)
		{
            Labels = labels;
			Style = ValidateStyle("Minipopup", "Minipopup");
		}
		public VoltagePopup(int index, string[] labels, ElementSettings settings) : this(index, labels)
		{
			ElementSettings = settings;
		}
		
		public VoltagePopup(int index, string[] labels, GUIStyle style) : this(index, labels)
		{
			Style = ValidateStyle(style,"Minipopup", "Minipopup");
		}
		public VoltagePopup(int index, string[] labels, ElementSettings settings, GUIStyle style) : this(index, labels, settings)
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

			Selected = EditorGUI.Popup(WorkingArea, Selected, Labels, Style);
		}
	}
}
