using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voltage
{
	public class VoltageNumericRange : VoltageElement
	{
		private VoltageNumericType type = VoltageNumericType.Int;

		private int m_intValue = 0;
		private int m_intStep = 1;
		private int m_intMin = 0;
		private int m_intMax = 10;

		private float m_floatValue = 0f;
		private float m_floatStep = 0.1f;
		private float m_floatMin = 0f;
		private float m_floatMax = 10f;

		private GUIStyle m_stylePlus;
		private GUIStyle m_styleMinus;
		private GUIStyle m_styleSlider;
		public GUIStyle m_styleThumb;

		#region PROPERTIES
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
				m_intStep = Mathf.Max(1, value);
			}
		}
		public int IntMin
		{
			get
			{
				return m_intMin;
			}
			set
			{
				m_intMin = value;
			}
		}
		public int IntMax
		{
			get
			{
				return m_intMax;
			}
			set
			{
				m_intMax = value;
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
				m_floatStep = Mathf.Max(0f, value);
			}
		}
		public float FloatMin
		{
			get
			{
				return m_floatMin;
			}
			set
			{
				m_floatMin = value;
			}
		}
		public float FloatMax
		{
			get
			{
				return m_floatMax;
			}
			set
			{
				m_floatMax = value;
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
		public GUIStyle StyleSlider
		{
			get
			{
				return m_styleSlider;
			}
			set
			{
				m_styleSlider = ValidateStyle(value, "SliderBar", "PreSlider");
			}
		}
		public GUIStyle StyleThumb
		{
			get
			{
				return m_styleThumb;
			}
			set
			{
				m_styleThumb = ValidateStyle(value, "SliderThumb", "PreSliderThumb");
			}
		}
		#endregion

		private void InitializeStyles()
		{
			Style = ValidateStyle("Textfield", "Textfield");
			StyleSlider = Styles.GetStyle("SliderBar");
			StyleThumb = Styles.GetStyle("SliderThumb");
			StylePlus = Styles.GetStyle("Button Plus Circle");
			StyleMinus = Styles.GetStyle("Button Minus Circle");
		}

		#region CONSTRUCTORS
		public VoltageNumericRange(int value, int min, int max)
		{
			type = VoltageNumericType.Int;
			IntValue = value;
			IntMin = min;
			IntMax = max;

			InitializeStyles();
		}
		public VoltageNumericRange(int value, int min, int max, int step) : this(value,min,max)
		{
			IntStep = step;
		}
		public VoltageNumericRange(int value, int min, int max, int step, ElementSettings settings) : this(value, min, max, step)
		{
			ElementSettings = settings;
		}

		public VoltageNumericRange(float value, float min, float max)
		{
			type = VoltageNumericType.Float;
			FloatValue = value;
			FloatMin = min;
			FloatMax = max;

			InitializeStyles();
		}
		public VoltageNumericRange(float value, float min, float max, float step) : this(value, min, max)
		{
			FloatStep = step;
		}
		public VoltageNumericRange(float value, float min, float max, float step, ElementSettings settings) : this(value, min, max, step)
		{
			ElementSettings = settings;
		}
		#endregion

		/// <summary>
		/// Do not use this.
		/// </summary>
		/// <param name="workingArea"></param>
		public override void DrawElement(Rect workingArea)
		{
			base.DrawElement(workingArea);


			Rect minusRect = new Rect(WorkingArea.x, WorkingArea.y + WorkingArea.height * 0.125f, WorkingArea.height * 0.75f, WorkingArea.height * 0.75f);
			Rect sliderRect = new Rect(minusRect.x + minusRect.width, WorkingArea.y, (WorkingArea.width - WorkingArea.height * 1.5f) * 0.6f, WorkingArea.height);
			Rect plusRect = new Rect(sliderRect.x + sliderRect.width, minusRect.y, minusRect.width, minusRect.height);
			Rect intRect = new Rect(plusRect.x + plusRect.width, WorkingArea.y, (WorkingArea.width - WorkingArea.height * 1.5f) * 0.4f, WorkingArea.height);

			switch (type)
			{
				case VoltageNumericType.Int:
					if (GUI.Button(minusRect, "", StyleMinus))
						IntValue = Mathf.Clamp(IntValue - IntStep, IntMin, IntMax);
					if (GUI.Button(plusRect, "", StylePlus))
						IntValue = Mathf.Clamp(IntValue + IntStep, IntMin, IntMax);

					IntValue = Mathf.RoundToInt(GUI.HorizontalSlider(sliderRect, (float)IntValue, (float)IntMin, (float)IntMax, StyleSlider, StyleThumb));
					IntValue = EditorGUI.IntField(intRect, IntValue, Style);
					break;
				case VoltageNumericType.Float:
					if (GUI.Button(minusRect, "", StyleMinus))
						FloatValue = Mathf.Clamp(FloatValue - FloatStep, FloatMin, FloatMax);
					if (GUI.Button(plusRect, "", StylePlus))
						FloatValue = Mathf.Clamp(FloatValue + FloatStep, FloatMin, FloatMax);

					FloatValue = Mathf.Round(GUI.HorizontalSlider(sliderRect, FloatValue, FloatMin, FloatMax, StyleSlider, StyleThumb) / (FloatStep))*(FloatStep);
					//FloatValue = GUI.HorizontalSlider(sliderRect, FloatValue, FloatMin, FloatMax, StyleSlider, StyleThumb);
					FloatValue = EditorGUI.FloatField(intRect, FloatValue, Style);
					break;
				default:
					break;
			}

			
		}
	}
}
