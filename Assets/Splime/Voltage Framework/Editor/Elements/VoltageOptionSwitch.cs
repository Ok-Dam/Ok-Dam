using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voltage
{
	public class VoltageOptionsSwitch : VoltageElement
	{

        private int m_index;
        private string[] m_labels;

		private GUIStyle m_onStyleL;
		private GUIStyle m_onStyleC;
		private GUIStyle m_onStyleR;

		private GUIStyle m_offStyleL;
		private GUIStyle m_offStyleC;
		private GUIStyle m_offStyleR;

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

		public VoltageOptionsSwitch(int selected, string[] labels)
		{
			Selected = selected;
			Labels = labels;

			OffStyleLeft = ValidateStyle("MiniTabLeft Off", "TL tab left");
			OnStyleLeft = ValidateStyle("MiniTabLeft On", "TL tab left");
			OffStyleCenter = ValidateStyle("MiniTabCenter Off", "TL tab mid");
			OnStyleCenter = ValidateStyle("MiniTabCenter On", "TL tab mid");
			OffStyleRight = ValidateStyle("MiniTabRight Off", "TL tab right");
			OnStyleRight = ValidateStyle("MiniTabRight On", "TL tab right");
		}
		public VoltageOptionsSwitch(int selected, string[] labels, ElementSettings settings) : this(selected, labels)
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
				for (int i = 0; i < Labels.Length; i++)
				{
					if (Selected == i)
						w = Mathf.Max(w,OnStyleCenter.CalcSize(new GUIContent((i < Labels.Length ? Labels[i] : i.ToString()))).y);
					else
						w = Mathf.Max(w, OffStyleCenter.CalcSize(new GUIContent((i < Labels.Length ? Labels[i] : i.ToString()))).y);
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
				for (int i = 0; i < Labels.Length; i++)
				{
					if (Selected == i)
						w += OnStyleCenter.CalcSize(new GUIContent((i < Labels.Length ? Labels[i] : i.ToString()))).x;
					else
						w += OffStyleCenter.CalcSize(new GUIContent((i < Labels.Length ? Labels[i] : i.ToString()))).x;
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
			for (int i = 0; i < Labels.Length; i++)
			{
				if (i == 0)
					forStyles = (Selected == i ? OnStyleLeft : OffStyleLeft);
				else if (i == Labels.Length - 1)
					forStyles = (Selected == i ? OnStyleRight : OffStyleRight);
				else
					forStyles = (Selected == i ? OnStyleCenter : OffStyleCenter);


				currentPos.width = Mathf.Max(WorkingArea.width/Labels.Length, forStyles.CalcSize(new GUIContent(Labels[i])).x);

				if (GUI.Button(currentPos, Labels[i], forStyles))
				{
					Selected = i;
				}

				currentPos.x += currentPos.width;
			}

		}
	}
}
