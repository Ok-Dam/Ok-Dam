using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Voltage
{

	public abstract class VoltageWindow : EditorWindow
	{
		private VoltageInternalConstructor m_Constructor;
		protected IConstructor Constructor
		{
			get{ return m_Constructor; }
		}

		private void OnEnable()
		{
			titleContent.image = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Splime/Voltage Framework/GUI/Icons/WindowIcon.psd");
		}

		private void OnGUI()
		{
			Rect _currentT = position;
			_currentT.x = 0f;
			_currentT.y = 0f;

			if (m_Constructor == null)
			{
				m_Constructor = new VoltageInternalConstructor();
				
				VoltageInit();
			}

			m_Constructor.StartWildConstructor(new WeightArea());
			VoltageGUI();
			m_Constructor.EndAllAreas();

			m_Constructor.EventCall(_currentT);
			m_Constructor.DrawCall(_currentT);

			VoltageSerialization();
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
