using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voltage
{
	public class VoltageEnumSwitch : VoltageElement
	{
		private System.Enum m_value;

		private int m_index;
        private string[] m_labels;

		private GUIStyle m_onStyleL;
		private GUIStyle m_onStyleC;
		private GUIStyle m_onStyleR;

		private GUIStyle m_offStyleL;
		private GUIStyle m_offStyleC;
		private GUIStyle m_offStyleR;

		public System.Enum Value
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

		public GUIStyle OnStyleLeft
		{
			get
			{
				return m_onStyleL;
			}
			set
			{
				m_onStyleL = value;
			}
		}
		public GUIStyle OnStyleCenter
		{
			get
			{
				return m_onStyleC;
			}
			set
			{
				m_onStyleC = value;
			}
		}
		public GUIStyle OnStyleRight
		{
			get
			{
				return m_onStyleR;
			}
			set
			{
				m_onStyleR = value;
			}
		}

		public GUIStyle OffStyleLeft
		{
			get
			{
				return m_offStyleL;
			}
			set
			{
				m_offStyleL = value;
			}
		}
		public GUIStyle OffStyleCenter
		{
			get
			{
				return m_offStyleC;
			}
			set
			{
				m_offStyleC = value;
			}
		}
		public GUIStyle OffStyleRight
		{
			get
			{
				return m_offStyleR;
			}
			set
			{
				m_offStyleR = value;
			}
		}

		public VoltageEnumSwitch(System.Enum value)
		{
			Value = value;

			OffStyleLeft = ValidateStyle("MiniTabLeft Off", "TL tab left");
			OnStyleLeft = ValidateStyle("MiniTabLeft On", "TL tab left");
			OffStyleCenter = ValidateStyle("MiniTabCenter Off", "TL tab mid");
			OnStyleCenter = ValidateStyle("MiniTabCenter On", "TL tab mid");
			OffStyleRight = ValidateStyle("MiniTabRight Off", "TL tab right");
			OnStyleRight = ValidateStyle("MiniTabRight On", "TL tab right");
		}
		public VoltageEnumSwitch(System.Enum value, ElementSettings settings) : this(value)
		{
			ElementSettings = settings;
		}

		public override float CalcHeight(float width)
		{
			float w = 0f;

			if (FixedWidth > 0f)
				return FixedWidth;
			else
			{
				string[] labels = System.Enum.GetNames(Value.GetType());

				for (int i = 0; i < labels.Length; i++)
				{
					if (Value.ToString() == labels[i])
						w = Mathf.Max(w,OnStyleCenter.CalcSize(new GUIContent((i < labels.Length ? labels[i] : i.ToString()))).y);
					else
						w = Mathf.Max(w, OffStyleCenter.CalcSize(new GUIContent((i < labels.Length ? labels[i] : i.ToString()))).y);
				}
			}
			return w;
		}

		public override float CalcWidth()
		{
			float w = 0f;

			if (FixedWidth > 0f)
				return FixedWidth;
			else
			{
				string[] labels = System.Enum.GetNames(Value.GetType());
				for (int i = 0; i < labels.Length; i++)
				{
					if (Value.ToString() == labels[i])
							w += OnStyleCenter.CalcSize(new GUIContent((i < labels.Length ? labels[i] : i.ToString()))).x;
					else
						w += OffStyleCenter.CalcSize(new GUIContent((i < labels.Length ? labels[i] : i.ToString()))).x;
				}
			}
			return w;
		}
		/// <summary>
		/// Do not use this.
		/// </summary>
		/// <param name="workingArea"></param>
		public override void DrawElement(Rect workingArea)
		{
			base.DrawElement(workingArea);

			Rect currentPos = WorkingArea;
			GUIStyle forStyles;

			string[] labels = System.Enum.GetNames(Value.GetType());

			for (int i = 0; i < labels.Length; i++)
			{
				if (i == 0)
					forStyles = (Value.ToString() == labels[i] ? OnStyleLeft : OffStyleLeft);
				else if (i == labels.Length - 1)
					forStyles = (Value.ToString() == labels[i] ? OnStyleRight : OffStyleRight);
				else
					forStyles = (Value.ToString() == labels[i] ? OnStyleCenter : OffStyleCenter);


				currentPos.width = Mathf.Max(WorkingArea.width/ labels.Length, forStyles.CalcSize(new GUIContent(labels[i])).x);

				if (GUI.Button(currentPos, labels[i], forStyles))
				{
					Value = (System.Enum) System.Enum.Parse(Value.GetType(),labels[i]);
				}

				currentPos.x += currentPos.width;
			}

		}
	}
}
