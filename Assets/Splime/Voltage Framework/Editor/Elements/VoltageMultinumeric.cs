using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voltage
{
	public class VoltageMultinumeric : VoltageElement
	{

		private GUIContent[] subLabels;

		private VoltageNumericType type = VoltageNumericType.Int;

		private int m_count = 0;
		private int current = 0;

		private GUIStyle headerStyle;
		private GUIStyle fieldStyle;
		private GUIStyle fieldLabelStyle;

		private GUIStyle buttonOff;
		private GUIStyle buttonOn;

		private VoltageNumeric[] fields;

		public int Count
		{
			get
			{
				return m_count;
			}
		}
		public GUIStyle HeaderStyle
		{
			get { return headerStyle; }
			set { headerStyle = value; }
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
		public GUIStyle ButtonOn
		{
			get { return buttonOn; }
			set { buttonOn = value; }
		}
		public GUIStyle ButtonOff
		{
			get { return buttonOff; }
			set { buttonOff = value; }
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

			fields = new VoltageNumeric[Count];
			for (int i = 0; i < Count; i++)
				fields[i] = new VoltageNumeric(0);
		}

		private void InitializeFloatFields(int count)
		{
			m_count = count;

			fields = new VoltageNumeric[Count];
			for (int i = 0; i < Count; i++)
				fields[i] = new VoltageNumeric(0f);
		}

		private void InitializeStyles()
		{
			Style = ValidateStyle("Multifield HeaderLabel", "Label");
			fieldLabelStyle = ValidateStyle("Multifield FieldLabel", "Label");

			buttonOff = ValidateStyle("Multifield ButtonOff", "ShurikenCheckMark");
			buttonOn = ValidateStyle("Multifield ButtonOn", "ShurikenCheckMarkMixed");

			headerStyle = ValidateStyle(Styles.GetStyle("Multifield HeaderBG"), GUIStyle.none);
			fieldStyle = ValidateStyle(Styles.GetStyle("Multifield FieldBG"), GUIStyle.none);
		}
		#endregion

		#region CONSTRUCTORS
		public VoltageMultinumeric(RectOffset rectOffset)
		{
			type = VoltageNumericType.Int;
			InitializeIntFields(4);

			subLabels = new GUIContent[] { new GUIContent("Left"), new GUIContent("Right"), new GUIContent("Top"), new GUIContent("Bottom") };
			RectOffsetValue = rectOffset;

			InitializeStyles();
		}
		public VoltageMultinumeric(RectOffset rectOffset, ElementSettings settings) : this(rectOffset)
		{
			ElementSettings = settings;
		}

		public VoltageMultinumeric(Rect rect)
		{
			type = VoltageNumericType.Float;
			InitializeFloatFields(4);

			subLabels = new GUIContent[] { new GUIContent("X"), new GUIContent("Y"), new GUIContent("Width"), new GUIContent("Height") };
			RectValue = rect;

			InitializeStyles();
		}
		public VoltageMultinumeric(Rect rect, ElementSettings settings) : this(rect)
		{
			ElementSettings = settings;
		}

		public VoltageMultinumeric(Vector2 vector2)
		{
			type = VoltageNumericType.Float;
			InitializeFloatFields(2);

			subLabels = new GUIContent[] { new GUIContent("X"), new GUIContent("Y")};
			Vector2Value = vector2;

			InitializeStyles();
		}
		public VoltageMultinumeric(Vector2 vector2, ElementSettings settings) : this(vector2)
		{
			ElementSettings = settings;
		}

		public VoltageMultinumeric(Vector3 vector3)
		{
			type = VoltageNumericType.Float;
			InitializeFloatFields(3);

			subLabels = new GUIContent[] { new GUIContent("X"), new GUIContent("Y"), new GUIContent("Z")};
			Vector3Value = vector3;

			InitializeStyles();
		}
		public VoltageMultinumeric(Vector3 vector3, ElementSettings settings) : this(vector3)
		{
			ElementSettings = settings;
		}

		public VoltageMultinumeric(Vector4 vector4)
		{
			type = VoltageNumericType.Float;
			InitializeFloatFields(4);

			subLabels = new GUIContent[] { new GUIContent("X"), new GUIContent("Y"), new GUIContent("Z"), new GUIContent("W") };
			Vector4Value = vector4;

			InitializeStyles();
		}
		public VoltageMultinumeric(Vector4 vector4, ElementSettings settings) : this(vector4)
		{
			ElementSettings = settings;
		}

		public VoltageMultinumeric(Quaternion quaternion)
		{
			type = VoltageNumericType.Float;
			InitializeFloatFields(4);

			subLabels = new GUIContent[] { new GUIContent("X"), new GUIContent("Y"), new GUIContent("Z"), new GUIContent("W") };
			QuaternionValue = quaternion;

			InitializeStyles();
		}
		public VoltageMultinumeric(Quaternion quaternion, ElementSettings settings) : this(quaternion)
		{
			ElementSettings = settings;
		}

		public VoltageMultinumeric(int[] intArray)
		{
			type = VoltageNumericType.Int;
			InitializeIntFields(intArray.Length);

			subLabels = new GUIContent[Count];
			for (int i = 0; i < Count; i++)
			{
				subLabels[i] = new GUIContent("Field " + (i+1));
			}

			IntArrayValue = intArray;

			InitializeStyles();
		}
		public VoltageMultinumeric(int[] intArray, ElementSettings settings) : this(intArray)
		{
			ElementSettings = settings;
		}
		public VoltageMultinumeric(int[] intArray,string[] labels): this(intArray)
		{
			for (int i = 0; i < Count; i++)
			{
				subLabels[i] = new GUIContent(labels[i]);
			}
		}
		public VoltageMultinumeric(int[] intArray, string[] labels, ElementSettings settings) : this(intArray, labels)
		{
			ElementSettings = settings;
		}

		public VoltageMultinumeric(float[] floatArray)
		{
			type = VoltageNumericType.Float;
			InitializeFloatFields(floatArray.Length);

			subLabels = new GUIContent[Count];
			for (int i = 0; i < Count; i++)
			{
				subLabels[i] = new GUIContent("Field " + (i + 1));
			}

			FloatArrayValue = floatArray;

			InitializeStyles();
		}
		public VoltageMultinumeric(float[] floatArray, ElementSettings settings) : this(floatArray)
		{
			ElementSettings = settings;
		}
		public VoltageMultinumeric(float[] floatArray, string[] labels) : this(floatArray)
		{
			for (int i = 0; i < Count; i++)
			{
				subLabels[i] = new GUIContent(labels[i]);
			}
		}
		public VoltageMultinumeric(float[] floatArray, string[] labels, ElementSettings settings) : this(floatArray, labels)
		{
			ElementSettings = settings;
		}
		#endregion

		private float CalcFieldHeight(float width)
		{
				return fields[0].CalcHeight(width);
		}
		/// <summary>
		/// Calculates the height of the element.
		/// </summary>
		/// <returns></returns>
		public override float CalcHeight(float width)
		{
			return base.CalcHeight(width) + CalcFieldHeight(width) + fieldLabelStyle.CalcSize(new GUIContent("1")).y + 4f;
		}

		/// <summary>
		/// Do not use this.
		/// </summary>
		/// <param name="workingArea"></param>
		public override void DrawElement(Rect workingArea)
		{
			base.DrawElement(workingArea);

			Rect header = new Rect(WorkingArea.x, WorkingArea.y, WorkingArea.width, Style.CalcSize(new GUIContent("X")).y);
			Rect buttons = new Rect(header.x + header.width - 10 - 2, header.y + (header.height-10f)/2f, 10f, 10f);
			Rect headerLabel = new Rect(header.x, header.y, header.width - buttons.width * Count -2, header.height);


			Rect fieldBG = new Rect(WorkingArea.x, WorkingArea.y+header.height, WorkingArea.width, WorkingArea.height-header.height);
			Rect fieldTotal = new Rect(fieldBG.x+2f, fieldBG.y+2f, fieldBG.width-4f, fieldBG.height-4f);

			Rect field = new Rect(fieldTotal.x, fieldTotal.y, fieldTotal.width, CalcFieldHeight(workingArea.width));

			Rect arrayValues = new Rect(fieldTotal.x, fieldTotal.y + field.height, fieldTotal.width, fieldTotal.height-field.height);

			GUI.Box(header, "", headerStyle);
			GUI.Box(fieldBG, "", fieldStyle);


			for (int i = Count-1; i >= 0; i--)
			{
				if (GUI.Button(buttons, "", current == i ? buttonOn : buttonOff))
					current = i;
				buttons.x -= buttons.width;
			}


			GUI.Label(headerLabel, subLabels[current], Style);

			fields[current].DrawElement(field);

			//VALUE LABEL
			string valuesString = string.Empty;
			switch (type)
			{
				case VoltageNumericType.Int:
					for (int i = 0; i < Count; i++)
					{
						valuesString += fields[i].IntValue + ", ";
					}
					break;
				case VoltageNumericType.Float:
					for (int i = 0; i < Count; i++)
					{
						valuesString += fields[i].FloatValue + ", ";
					}
					break;
			}
			valuesString = valuesString.Substring(0, valuesString.Length - 2);
			

			GUI.Label(arrayValues, "[" + valuesString + "]", fieldLabelStyle);



		}
	}

}

