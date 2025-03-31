using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voltage
{
	public enum VoltageNumericType
	{
		Int,
		Float
	}



	public class VoltageNumeric : VoltageElement
	{
		private VoltageNumericType type = VoltageNumericType.Int;

		private int m_intValue = 0;
		private int m_intStep = 1;

		private float m_floatValue = 0f;
		private float m_floatStep = 1f;

		private GUIStyle m_stylePlus;
		private GUIStyle m_styleMinus;

		public int IntValue
		{
			get
			{
				return m_intValue;
			}
			set
			{
				m_intValue = value;
			}
		}
		public int IntStep
		{
			get
			{
				return m_intStep;
			}
			set
			{
				m_intStep = Mathf.Max(1,value);
			}
		}
		public float FloatValue
		{
			get
			{
				return m_floatValue;
			}
			set
			{
				m_floatValue = value;
			}
		}
		public float FloatStep
		{
			get
			{
				return m_floatStep;
			}
			set
			{
				m_floatStep = Mathf.Max(0f,value);
			}
		}

		public GUIStyle StylePlus
		{
			get
			{
				return m_stylePlus;
			}
			set
			{
				m_stylePlus = ValidateStyle(value, "Button Plus Circle", "OL Plus");
			}
		}
		public GUIStyle StyleMinus
		{
			get
			{
				return m_styleMinus;
			}
			set
			{
				m_styleMinus = ValidateStyle(value, "Button Minus Circle", "OL Minus");
			}
		}


		private void InitializeStyles()
		{
			Style = ValidateStyle("Textfield","Textfield");
			StylePlus = Styles.GetStyle("Button Plus Circle");
			StyleMinus = Styles.GetStyle("Button Minus Circle");
		}

		public VoltageNumeric(int value)
		{
			type = VoltageNumericType.Int;
			IntValue = value;

			InitializeStyles();
		}
		public VoltageNumeric(int value,int step): this(value)
		{
			IntStep = step;

		}
		public VoltageNumeric(int value, GUIStyle style) : this(value)
		{
			Style = ValidateStyle(style,"Textfield", "Textfield");
		}
		public VoltageNumeric(int value, int step, GUIStyle style) : this(value, step)
		{
			Style = ValidateStyle(style,"Textfield", "Textfield");
		}
		public VoltageNumeric(int value, GUIStyle style, GUIStyle minus, GUIStyle plus) : this(value, style)
		{
			StyleMinus = minus;
			StylePlus = plus;
		}
		public VoltageNumeric(int value, int step, GUIStyle style, GUIStyle minus, GUIStyle plus) : this(value, step, style)
		{
			StyleMinus = minus;
			StylePlus = plus;
		}
		public VoltageNumeric(float value)
		{
			type = VoltageNumericType.Float;
			FloatValue = value;

			InitializeStyles();
		}
		public VoltageNumeric(float value, float step) : this(value)
		{
			FloatStep = step;
		}
		public VoltageNumeric(float value, float step,GUIStyle style) : this(value, step)
		{
			Style = ValidateStyle(style,"Textfield", "Textfield");
		}
		public VoltageNumeric(float value,GUIStyle style) : this(value)
		{
			Style = ValidateStyle(style,"Textfield", "Textfield");
		}
		public VoltageNumeric(float value, float step, GUIStyle style, GUIStyle minus, GUIStyle plus) : this(value, step, style)
		{
			StyleMinus = minus;
			StylePlus = plus;
		}
		public VoltageNumeric(float value, GUIStyle style, GUIStyle minus, GUIStyle plus) : this(value, style)
		{
			StyleMinus = minus;
			StylePlus = plus;
		}
		/// <summary>
		/// Do not use this.
		/// </summary>
		/// <param name="workingArea"></param>
		public override void DrawElement(Rect workingArea)
		{
			base.DrawElement(workingArea);


			Rect minusRect = new Rect(WorkingArea.x, WorkingArea.y + WorkingArea.height * 0.125f, WorkingArea.height * 0.75f, WorkingArea.height * 0.75f);
			Rect plusRect = new Rect(minusRect.x + minusRect.width, minusRect.y, minusRect.width, minusRect.height);
			Rect intRect = new Rect(plusRect.x + plusRect.width, WorkingArea.y, WorkingArea.width - WorkingArea.height * 1.5f, WorkingArea.height);
		

			

			switch (type)
			{
				case VoltageNumericType.Int:
					if (GUI.Button(minusRect, "", StyleMinus))
						IntValue -= IntStep;
					if (GUI.Button(plusRect, "", StylePlus))
						IntValue += IntStep;
					IntValue = EditorGUI.IntField(intRect, IntValue, Style);
					break;
				case VoltageNumericType.Float:
					if (GUI.Button(minusRect, "", StyleMinus))
						FloatValue -= FloatStep;
					if (GUI.Button(plusRect, "", StylePlus))
						FloatValue += FloatStep;
					FloatValue = EditorGUI.FloatField(intRect, FloatValue, Style);
					break;
				default:
					Debug.LogError("VoltageNumeric: Type not implemented.");
					break;
			}
		}
	}
}
