using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voltage
{
	public class VoltagePluralNumeric : VoltageElement
	{
		private class VoltageNumber
		{
			private VoltageNumericType type = VoltageNumericType.Int;
			private int m_intValue = 0;
			private float m_floatValue = 0f;

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

			public VoltageNumber(int value)
			{
				type = VoltageNumericType.Int;
				IntValue = value;
			}
			public VoltageNumber(float value)
			{
				type = VoltageNumericType.Float;
				FloatValue = value;
			}
		}

		private GUIContent[] subLabels;

		private VoltageNumericType type = VoltageNumericType.Int;

		private int m_count = 0;
		private int current = 0;
		private float elementSeparation = 3f;

		private GUIStyle labelStyle;
		private GUIStyle fieldStyle;
		private GUIStyle fieldLabelStyle;

		private GUIStyle buttonOff;
		private GUIStyle buttonOn;

		private VoltageNumber[] fields;

		public int Count
		{
			get
			{
				return m_count;
			}
		}

		public GUIStyle FieldStyle
		{
			get { return fieldStyle; }
			set { fieldStyle = value; }
		}
		public GUIStyle FieldLabelStyle
		{
			get { return fieldLabelStyle; }
			set { fieldLabelStyle = value; }
		}

		#region VALUES
		public RectOffset RectOffsetValue
		{
			get
			{
				return new RectOffset(fields[0].IntValue, fields[1].IntValue, fields[2].IntValue, fields[3].IntValue);
			}
			set
			{
				if (type != VoltageNumericType.Int)
					return;

				fields[0].IntValue = value.left;
				fields[1].IntValue = value.right;
				fields[2].IntValue = value.top;
				fields[3].IntValue = value.bottom;
			}
		}

		public Rect RectValue
		{
			get
			{
				return new Rect(fields[0].FloatValue, fields[1].FloatValue, fields[2].FloatValue, fields[3].FloatValue);
			}
			set
			{
				if (type != VoltageNumericType.Float)
					return;

				fields[0].FloatValue = value.x;
				fields[1].FloatValue = value.y;
				fields[2].FloatValue = value.width;
				fields[3].FloatValue = value.height;
			}
		}

		public Vector2 Vector2Value
		{
			get
			{
				return new Vector2(fields[0].FloatValue, fields[1].FloatValue);
			}
			set
			{
				if (type != VoltageNumericType.Float)
					return;

				fields[0].FloatValue = value.x;
				fields[1].FloatValue = value.y;
			}
		}

		public Vector3 Vector3Value
		{
			get
			{
				return new Vector3(fields[0].FloatValue, fields[1].FloatValue, fields[2].FloatValue);
			}
			set
			{
				if (type != VoltageNumericType.Float)
					return;

				fields[0].FloatValue = value.x;
				fields[1].FloatValue = value.y;
				fields[2].FloatValue = value.z;
			}
		}

		public Vector4 Vector4Value
		{
			get
			{
				return new Vector4(fields[0].FloatValue, fields[1].FloatValue, fields[2].FloatValue, fields[3].FloatValue);
			}
			set
			{
				if (type != VoltageNumericType.Float)
					return;

				fields[0].FloatValue = value.x;
				fields[1].FloatValue = value.y;
				fields[2].FloatValue = value.z;
				fields[3].FloatValue = value.w;
			}
		}

		public Quaternion QuaternionValue
		{
			get
			{
				return new Quaternion(fields[0].FloatValue, fields[1].FloatValue, fields[2].FloatValue, fields[3].FloatValue);
			}
			set
			{
				if (type != VoltageNumericType.Float)
					return;

				fields[0].FloatValue = value.x;
				fields[1].FloatValue = value.y;
				fields[2].FloatValue = value.z;
				fields[3].FloatValue = value.w;
			}
		}

		public float[] FloatArrayValue
		{
			get
			{

				float[] r = new float[Count];
				for (int i = 0; i < Count; i++)
				{
					r[i] = fields[i].FloatValue;
				}
				return r;
			}
			set
			{
				if (type != VoltageNumericType.Float)
					return;

				for (int i = 0; i < Count; i++)
				{
					fields[i].FloatValue = value[i];
				}
			}
		}

		public int[] IntArrayValue
		{
			get
			{
				int[] r = new int[Count];
				for (int i = 0; i < Count; i++)
				{
					r[i] = fields[i].IntValue;
				}
				return r;
			}
			set
			{
				if (type != VoltageNumericType.Int)
					return;

				for (int i = 0; i < Count; i++)
				{
					fields[i].IntValue = value[i];
				}
			}
		}
		#endregion

		#region INITIALIZER
		private void InitializeIntFields(int count)
		{
			m_count = count;

			fields = new VoltageNumber[Count];
			for (int i = 0; i < Count; i++)
				fields[i] = new VoltageNumber(0);
		}

		private void InitializeFloatFields(int count)
		{
			m_count = count;

			fields = new VoltageNumber[Count];
			for (int i = 0; i < Count; i++)
				fields[i] = new VoltageNumber(0f);
		}

		private void InitializeStyles()
		{
			Style = ValidateStyle("Multifield HeaderLabel", "Label");
			fieldLabelStyle = ValidateStyle("Multifield FieldLabel", "Label");

			buttonOff = ValidateStyle("Multifield ButtonOff", "ShurikenCheckMark");
			buttonOn = ValidateStyle("Multifield ButtonOn", "ShurikenCheckMarkMixed");

			labelStyle = ValidateStyle(Styles.GetStyle("LabeledText Label"), GUIStyle.none);
			fieldStyle = ValidateStyle(Styles.GetStyle("LabeledText Text"), GUIStyle.none);
		}
		#endregion

		#region CONSTRUCTORS
		public VoltagePluralNumeric(RectOffset rectOffset)
		{
			type = VoltageNumericType.Int;
			InitializeIntFields(4);

			subLabels = new GUIContent[] { new GUIContent("Left"), new GUIContent("Right"), new GUIContent("Top"), new GUIContent("Bottom") };
			RectOffsetValue = rectOffset;

			InitializeStyles();
		}
		public VoltagePluralNumeric(RectOffset rectOffset, ElementSettings settings) : this(rectOffset)
		{
			ElementSettings = settings;
		}

		public VoltagePluralNumeric(Rect rect)
		{
			type = VoltageNumericType.Float;
			InitializeFloatFields(4);

			subLabels = new GUIContent[] { new GUIContent("X"), new GUIContent("Y"), new GUIContent("Width"), new GUIContent("Height") };
			RectValue = rect;

			InitializeStyles();
		}
		public VoltagePluralNumeric(Rect rect, ElementSettings settings) : this(rect)
		{
			ElementSettings = settings;
		}

		public VoltagePluralNumeric(Vector2 vector2)
		{
			type = VoltageNumericType.Float;
			InitializeFloatFields(2);

			subLabels = new GUIContent[] { new GUIContent("X"), new GUIContent("Y")};
			Vector2Value = vector2;

			InitializeStyles();
		}
		public VoltagePluralNumeric(Vector2 vector2, ElementSettings settings) : this(vector2)
		{
			ElementSettings = settings;
		}

		public VoltagePluralNumeric(Vector3 vector3)
		{
			type = VoltageNumericType.Float;
			InitializeFloatFields(3);

			subLabels = new GUIContent[] { new GUIContent("X"), new GUIContent("Y"), new GUIContent("Z")};
			Vector3Value = vector3;

			InitializeStyles();
		}
		public VoltagePluralNumeric(Vector3 vector3, ElementSettings settings) : this(vector3)
		{
			ElementSettings = settings;
		}

		public VoltagePluralNumeric(Vector4 vector4)
		{
			type = VoltageNumericType.Float;
			InitializeFloatFields(4);

			subLabels = new GUIContent[] { new GUIContent("X"), new GUIContent("Y"), new GUIContent("Z"), new GUIContent("W") };
			Vector4Value = vector4;

			InitializeStyles();
		}
		public VoltagePluralNumeric(Vector4 vector4, ElementSettings settings) : this(vector4)
		{
			ElementSettings = settings;
		}

		public VoltagePluralNumeric(Quaternion quaternion)
		{
			type = VoltageNumericType.Float;
			InitializeFloatFields(4);

			subLabels = new GUIContent[] { new GUIContent("X"), new GUIContent("Y"), new GUIContent("Z"), new GUIContent("W") };
			QuaternionValue = quaternion;

			InitializeStyles();
		}
		public VoltagePluralNumeric(Quaternion quaternion, ElementSettings settings) : this(quaternion)
		{
			ElementSettings = settings;
		}

		public VoltagePluralNumeric(int[] intArray)
		{
			type = VoltageNumericType.Int;
			InitializeIntFields(intArray.Length);

			subLabels = new GUIContent[Count];
			for (int i = 0; i < Count; i++)
			{
				subLabels[i] = new GUIContent("F" + (i+1));
			}

			IntArrayValue = intArray;

			InitializeStyles();
		}
		public VoltagePluralNumeric(int[] intArray, ElementSettings settings) : this(intArray)
		{
			ElementSettings = settings;
		}
		public VoltagePluralNumeric(int[] intArray,string[] labels): this(intArray)
		{
			for (int i = 0; i < Count; i++)
			{
				subLabels[i] = new GUIContent(labels[i]);
			}
		}
		public VoltagePluralNumeric(int[] intArray, string[] labels, ElementSettings settings) : this(intArray, labels)
		{
			ElementSettings = settings;
		}

		public VoltagePluralNumeric(float[] floatArray)
		{
			type = VoltageNumericType.Float;
			InitializeFloatFields(floatArray.Length);

			subLabels = new GUIContent[Count];
			for (int i = 0; i < Count; i++)
			{
				subLabels[i] = new GUIContent("F" + (i + 1));
			}

			FloatArrayValue = floatArray;

			InitializeStyles();
		}
		public VoltagePluralNumeric(float[] floatArray, ElementSettings settings) : this(floatArray)
		{
			ElementSettings = settings;
		}
		public VoltagePluralNumeric(float[] floatArray, string[] labels) : this(floatArray)
		{
			for (int i = 0; i < Count; i++)
			{
				subLabels[i] = new GUIContent(labels[i]);
			}
		}
		public VoltagePluralNumeric(float[] floatArray, string[] labels, ElementSettings settings) : this(floatArray, labels)
		{
			ElementSettings = settings;
		}
		#endregion

		
		/// <summary>
		/// Calculates the height of the element.
		/// </summary>
		/// <returns></returns>
		public override float CalcHeight(float width)
		{
			//return base.CalcHeight(width) + CalcFieldHeight(width) + fieldLabelStyle.CalcSize(new GUIContent("1")).y + 4f;
			float fieldHeight = Mathf.Max(labelStyle.CalcSize(new GUIContent("A")).y, fieldStyle.CalcSize(new GUIContent("0")).y);

			return fieldHeight * Mathf.CeilToInt(Count / 3f) + elementSeparation * (Count - 1);
		}

		/// <summary>
		/// Do not use this.
		/// </summary>
		/// <param name="workingArea"></param>
		public override void DrawElement(Rect workingArea)
		{
			base.DrawElement(workingArea);

			float fieldHeight = Mathf.Max(labelStyle.CalcSize(new GUIContent("A")).y, fieldStyle.CalcSize(new GUIContent("0")).y);

			Rect currentPos = new Rect(WorkingArea.x,WorkingArea.y,(WorkingArea.width - elementSeparation * 2f )/ 3f,fieldHeight);

			float a = 0.61803f;
			float widthLabel = (currentPos.width) * (1f- a);
			float widthField = (currentPos.width) * a;

			//float widthLabel = /*Mathf.Min(currentPos.width / 3f, */labelStyle.CalcSize(new GUIContent(subLabels[i])).x;//);
			//float widthField = /*Mathf.Min(currentPos.width * 2f/ 3f, */fieldStyle.CalcSize(new GUIContent("0000.00")).x;//);
			//Debug.Log(Count + " " + Mathf.CeilToInt(Count / 3f).ToString());

			for (int i = 0; i < Mathf.CeilToInt(Count / 3f); i++)
			{
				for (int j = 0; j < Mathf.Min(3,Count - 3*i); j++)
				{
					Rect label = new Rect(currentPos.x, currentPos.y, widthLabel, fieldHeight);
					Rect field = new Rect(currentPos.x + label.width, currentPos.y, widthField, fieldHeight);

					GUI.Label(label, subLabels[i*3 + j], labelStyle);
					switch (type)
					{
						case VoltageNumericType.Int:
							fields[i * 3 + j].IntValue = EditorGUI.IntField(field, fields[i * 3 + j].IntValue, fieldStyle);
							break;
						case VoltageNumericType.Float:
							fields[i * 3 + j].FloatValue = EditorGUI.FloatField(field, fields[i * 3 + j].FloatValue, fieldStyle);
							break;
						default:
							break;
					}
					currentPos.x += widthLabel + widthField + elementSeparation;

				}
				currentPos.x = WorkingArea.x;
				currentPos.y += fieldHeight+ elementSeparation;
			}
		}
	}

}

