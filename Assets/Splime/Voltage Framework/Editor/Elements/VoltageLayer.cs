using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voltage
{
	public class VoltageLayer : VoltageElement
	{
		private int m_layer = 0;
		 
		public int Layer
		{
			get
			{
				return m_layer;
			}
			set
			{
				m_layer = value;
			}
		}

		public VoltageLayer(int layer)
		{
			Layer = layer;
			Style = ValidateStyle("Minipopup", "Minipopup");
		}
		public VoltageLayer(int layer, ElementSettings elementSettings) : this(layer)
		{
			ElementSettings = elementSettings;
			Style = ValidateStyle("Minipopup", "Minipopup");
		}

		public VoltageLayer(int layer, GUIStyle style) : this(layer)
		{
			Style = ValidateStyle(style, "Minipopup", "Minipopup");
		}
		public VoltageLayer(int layer, ElementSettings elementSettings, GUIStyle style) : this(layer, elementSettings)
		{
			Style = ValidateStyle(style, "Minipopup", "Minipopup");
		}

		/// <summary>
		/// Do not use this.
		/// </summary>
		/// <param name="workingArea"></param>
		public override void DrawElement(Rect workingArea)
		{
			base.DrawElement(workingArea);
			Layer = EditorGUI.LayerField(WorkingArea, Layer, Style);
		}
	}
}
