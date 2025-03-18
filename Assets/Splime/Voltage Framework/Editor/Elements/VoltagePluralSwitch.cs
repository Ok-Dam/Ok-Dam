using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace Voltage
{
    public class VoltagePluralSwitch : VoltageElement
    {
        private bool[] m_values = new bool[0];
		private string[] m_labels = new string[0];

		private float m_elementMargin = 3f;

        private GUIStyle m_onStyle;
        private GUIStyle m_offStyle;

        public bool[] Values
        {
            get
            {
                return m_values;
            }
            set
            {
				m_values = value;
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

        public GUIStyle OnStyle
        {
            get
            {
                return m_onStyle;
            }
            set
            {
                m_onStyle = value;
            }
        }
        public GUIStyle OffStyle
        {
            get
            {
                return m_offStyle;
            }
            set
            {
                m_offStyle = value;
            }
        }

        public VoltagePluralSwitch(bool[] values, string[] labels)
        {
            Values = values;
			Labels = labels;
			OnStyle = ValidateStyle("LabelSwitchOn", "VisibilityToggle");
            OffStyle = ValidateStyle("LabelSwitchOff", "Toggle");
        }
		public VoltagePluralSwitch(bool[] values, string[] labels, ElementSettings settings) : this(values, labels)
        {
            ElementSettings = settings;
        }

        public VoltagePluralSwitch(bool[] values, string[] labels, GUIStyle onStyle, GUIStyle offStyle) : this(values, labels)
        {
            OnStyle = ValidateStyle(onStyle, "LabelSwitchOn", "VisibilityToggle");
            OffStyle = ValidateStyle(offStyle, "LabelSwitchOff", "Toggle");
        }
        public VoltagePluralSwitch(bool[] values, string[] labels, ElementSettings settings, GUIStyle onStyle, GUIStyle offStyle) : this(values, labels, settings)
        {
            OnStyle = ValidateStyle(onStyle, "LabelSwitchOn", "VisibilityToggle");
            OffStyle = ValidateStyle(offStyle, "LabelSwitchOff", "Toggle");
        }

		public VoltagePluralSwitch(bool[] values)
		{
			Values = values;

			OnStyle = ValidateStyle("LabelSwitchOn", "VisibilityToggle");
			OffStyle = ValidateStyle("LabelSwitchOff", "Toggle");
		}
		public VoltagePluralSwitch(bool[] values, ElementSettings settings) : this(values)
		{
			ElementSettings = settings;
		}

		public VoltagePluralSwitch(bool[] values, GUIStyle onStyle, GUIStyle offStyle) : this(values)
		{
			OnStyle = ValidateStyle(onStyle, "LabelSwitchOn", "VisibilityToggle");
			OffStyle = ValidateStyle(offStyle, "LabelSwitchOff", "Toggle");
		}
		public VoltagePluralSwitch(bool[] values, ElementSettings settings, GUIStyle onStyle, GUIStyle offStyle) : this(values, settings)
		{
			OnStyle = ValidateStyle(onStyle, "LabelSwitchOn", "VisibilityToggle");
			OffStyle = ValidateStyle(offStyle, "LabelSwitchOff", "Toggle");
		}

		public override float CalcWidth()
		{
			float w = 0f;

			if (FixedWidth > 0f)
				return FixedWidth;
			else
			{
				for (int i = 0; i < Values.Length; i++)
				{
					if (Values[i])
						w += OnStyle.CalcSize(new GUIContent((i < Labels.Length ? Labels[i] : i.ToString()))).x;
					else
						w += OffStyle.CalcSize(new GUIContent((i < Labels.Length ? Labels[i] : i.ToString()))).x;
				}
			}
			w += (Values.Length-1) * m_elementMargin;
			return w;
		}

		public override float CalcHeight(float width)
		{
			float h = 0f;
			
			if (FixedHeight > 0f)
				return FixedHeight;
			else
			{
				for (int i = 0; i < Values.Length; i++)
				{
					if (Values[i])
						h = Mathf.Max(h, OnStyle.CalcSize(new GUIContent((i < Labels.Length ? Labels[i] : i.ToString()))).y);
					else
						h = Mathf.Max(h, OffStyle.CalcSize(new GUIContent((i < Labels.Length ? Labels[i] : i.ToString()))).y);
				}
			}
			
			return h;
		}

		/// <summary>
		/// Do not use this.
		/// </summary>
		/// <param name="workingArea"></param>
		public override void DrawElement(Rect workingArea)
        {
            base.DrawElement(workingArea);
            Rect currentPos = WorkingArea;

			for (int i = 0; i < Values.Length; i++)
			{
				GUIStyle forStyles = (Values[i] ? OnStyle : OffStyle);
				currentPos.width = forStyles.CalcSize(new GUIContent((i < Labels.Length ? Labels[i] : i.ToString()))).x;

				if (GUI.Button(currentPos, (i < Labels.Length ? Labels[i] : i.ToString()), forStyles))
				{
					Values[i] = !Values[i];
				}

				currentPos.x += m_elementMargin + currentPos.width;
			}

          
        }
    }
}
