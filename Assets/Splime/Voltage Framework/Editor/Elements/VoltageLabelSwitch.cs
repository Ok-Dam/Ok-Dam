using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace Voltage
{
    public class VoltageLabelSwitch : VoltageElement
    {
        private bool m_value = false;
        private string m_label = "switch";
		private GUIStyle m_onStyle;
        private GUIStyle m_offStyle;

        public bool Value
        {
            get
            {
                return m_value;
            }
            set
            {
                m_value = value;
            }
        }
		public string Label
		{
			get
			{
				return m_label;
			}
			set
			{
				m_label = value;
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

        public VoltageLabelSwitch(bool value, string label)
        {
            Value = value;
            Label = label;
			OnStyle = ValidateStyle("LabelSwitchOn", "VisibilityToggle");
            OffStyle = ValidateStyle("LabelSwitchOff", "Toggle");
        }
        public VoltageLabelSwitch(bool value, string label, ElementSettings settings) : this(value, label)
        {
            ElementSettings = settings;
        }

        public VoltageLabelSwitch(bool value, string label, GUIStyle onStyle, GUIStyle offStyle) : this(value, label)
        {
            OnStyle = ValidateStyle(onStyle, "LabelSwitchOn", "VisibilityToggle");
            OffStyle = ValidateStyle(offStyle, "LabelSwitchOff", "Toggle");
        }
        public VoltageLabelSwitch(bool value, string label, ElementSettings settings, GUIStyle onStyle, GUIStyle offStyle) : this(value, label, settings)
        {
            OnStyle = ValidateStyle(onStyle, "LabelSwitchOn", "VisibilityToggle");
            OffStyle = ValidateStyle(offStyle, "LabelSwitchOff", "Toggle");
        }


		public override float CalcWidth()
		{
			if (FixedWidth > 0f)
				return FixedWidth;
			else if(Value)
				return OnStyle.CalcSize(new GUIContent(Label)).x;
			else
				return OffStyle.CalcSize(new GUIContent(Label)).x;

		}

		public override float CalcHeight(float width)
		{
			if (FixedHeight > 0f)
				return FixedHeight;
			else if (Value)
				return OnStyle.CalcSize(new GUIContent(Label)).y;
			else
				return OffStyle.CalcSize(new GUIContent(Label)).y;
		}

		/// <summary>
		/// Do not use this.
		/// </summary>
		/// <param name="workingArea"></param>
		public override void DrawElement(Rect workingArea)
        {
            base.DrawElement(workingArea);
            Rect currentPos = WorkingArea;

			GUIStyle style = (Value ? OnStyle : OffStyle);

			currentPos.width = style.CalcSize(new GUIContent(Label)).x;

			if (GUI.Button(currentPos, Label, style))
			{
				Value = !Value;
			}
        }
    }
}
