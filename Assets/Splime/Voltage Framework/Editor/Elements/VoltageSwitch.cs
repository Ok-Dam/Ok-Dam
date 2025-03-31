using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace Voltage
{
    public class VoltageSwitch : VoltageElement
    {
        private bool m_value = false;
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

        public VoltageSwitch(bool value)
        {
            Value = value;
			OnStyle = ValidateStyle("SwitchOn", "VisibilityToggle");
            OffStyle = ValidateStyle("SwitchOff", "Toggle");
        }
        public VoltageSwitch(bool value, ElementSettings settings) : this(value)
        {
            ElementSettings = settings;
        }

        public VoltageSwitch(bool value, GUIStyle onStyle, GUIStyle offStyle) : this(value)
        {
            OnStyle = ValidateStyle(onStyle, "SwitchOn", "VisibilityToggle");
            OffStyle = ValidateStyle(offStyle, "SwitchOff", "Toggle");
        }
        public VoltageSwitch(bool value, ElementSettings settings, GUIStyle onStyle, GUIStyle offStyle) : this(value, settings)
        {
            OnStyle = ValidateStyle(onStyle, "SwitchOn", "VisibilityToggle");
            OffStyle = ValidateStyle(offStyle, "SwitchOff", "Toggle");
        }


		public override float CalcWidth()
		{
			if (FixedWidth > 0f)
				return FixedWidth;
			else if(Value)
				return OnStyle.CalcSize(Content).x;
				else
				return OffStyle.CalcSize(Content).x;

		}

		public override float CalcHeight(float width)
		{
			if (FixedHeight > 0f)
				return FixedHeight;
			else if (Value)
				return OnStyle.CalcSize(Content).y;
			else
				return OffStyle.CalcSize(Content).y;

		}

		/// <summary>
		/// Do not use this.
		/// </summary>
		/// <param name="workingArea"></param>
		public override void DrawElement(Rect workingArea)
        {
            base.DrawElement(workingArea);
            Rect currentPos = WorkingArea;
            currentPos.width = currentPos.height*2f;

            if (Value)
            {
                if(GUI.Button(currentPos, "", OnStyle))
                {
                    Value = !Value;
                }
            }
            else
            {
                if (GUI.Button(currentPos, "", OffStyle))
                {
                    Value = !Value;
                }
            }
        }
    }
}
