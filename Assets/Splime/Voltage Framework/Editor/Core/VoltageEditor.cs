using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voltage
{

	public abstract class VoltageEditor : Editor
	{
		private VoltageInternalConstructor m_Constructor;
		protected VoltageInternalConstructor Constructor
		{
			get { return m_Constructor; }
		}

		private Rect currenPos = new Rect(0, 0, 0, 0);
		Rect inspectorPos = new Rect(0, 0, 0, 0);

		private void OnEnable()
		{
			
		}
		public override void OnInspectorGUI()
		{

			//Color col = GUI.color;
			//GUI.color = Color.white;
			//Color bgcol = GUI.backgroundColor;
			//GUI.backgroundColor = Color.white;
			if (m_Constructor == null)
			{
				m_Constructor = new VoltageInternalConstructor();

				VoltageInit();
				Constructor.EndAllAreas();
			}

			if (Event.current.type == EventType.Layout)
			{
				Constructor.StartWildConstructor(new StreamArea());
				VoltageGUI();
				Constructor.EndAllAreas();
				
				currenPos = new Rect(0, 0, EditorGUIUtility.currentViewWidth, Constructor.CalcHeight(EditorGUIUtility.currentViewWidth));
				GUILayoutUtility.GetRect(currenPos.width, currenPos.height);
			}

			if (Event.current.type != EventType.Layout)
			{
				inspectorPos = EditorGUILayout.GetControlRect();

				inspectorPos.width = currenPos.width;
				inspectorPos.height = currenPos.height;
				GUI.BeginGroup(inspectorPos);

				Constructor.EventCall(currenPos);

				Constructor.DrawCall(currenPos);
				GUI.EndGroup();

				VoltageSerialization();
			}

			//GUI.color = col;
			//GUI.backgroundColor = bgcol;
		}

		/// <summary>
		/// Use for initializing your elements and making your Stored Layout.
		/// </summary>
		protected abstract void VoltageInit();
		/// <summary>
		/// Use for making your Wild Layout. Called every Repaint.
		/// </summary>
		protected abstract void VoltageGUI();
		/// <summary>
		/// Use for serialization. Called after VoltageGUI.
		/// </summary>
		protected abstract void VoltageSerialization();

	}
}