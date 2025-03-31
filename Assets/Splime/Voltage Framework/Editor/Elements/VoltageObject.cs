using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voltage
{
	public class VoltageObject : VoltageElement
	{
		public Object objectReference = null;
		public System.Type objectType = null;
		public VoltageObject(Object _objectReference, System.Type _objectType)
		{
			objectReference = _objectReference;
			objectType = _objectType;
			Content = new GUIContent("");
			Style = EditorStyles.objectField;
		}

		public VoltageObject(Object _objectReference, System.Type _objectType, int _weight)
		{
			objectReference = _objectReference;
			objectType = _objectType;
			Weight = _weight;
			Content = new GUIContent("");
			Style = EditorStyles.objectField;
		}


		public override void DrawElement(Rect _workingArea)
		{
			base.DrawElement(_workingArea);

			Style.clipping = TextClipping.Clip;

			objectReference = EditorGUI.ObjectField(WorkingArea, objectReference, objectType, false);
		}
	}
}
