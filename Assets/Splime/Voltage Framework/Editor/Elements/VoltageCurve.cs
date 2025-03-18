using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace Voltage
{
	public class VoltageCurve : VoltageElement
	{
		private AnimationCurve m_curve;
		private Color m_curveColor = Color.white;
		private Rect m_ranges;
		private bool hasValues = false;

		public AnimationCurve Curve
		{
			get
			{
				return m_curve;
			}
			set
			{
				m_curve = value;
			}
		}
		public Color CurveColor
		{
			get
			{
				return m_curveColor;
			}
			set
			{
				m_curveColor = value;
			}
		}
		public Rect Ranges
		{
			get
			{
				return m_ranges;
			}
			set
			{
				m_ranges = value;
			}
		}

		public VoltageCurve(AnimationCurve curve)
		{
			Curve = curve;
		}
		public VoltageCurve(AnimationCurve curve, ElementSettings settings) : this(curve)
		{
			ElementSettings = settings;
		}

		public VoltageCurve(AnimationCurve curve, Color curveColor, Rect ranges) : this(curve)
		{
			hasValues = true;
			CurveColor = curveColor;
			Ranges = ranges;
		}
		public VoltageCurve(AnimationCurve curve, ElementSettings settings, Color curveColor, Rect ranges) : this(curve, curveColor, ranges)
		{
			ElementSettings = settings;
		}

		/// <summary>
		/// Do not use this.
		/// </summary>
		/// <param name="workingArea"></param>
		public override void DrawElement(Rect workingArea)
		{
			base.DrawElement(workingArea);
			if(hasValues)
				Curve = EditorGUI.CurveField(WorkingArea, Curve,CurveColor,Ranges);
			else
				Curve = EditorGUI.CurveField(WorkingArea, Curve);
		}
	}
}
