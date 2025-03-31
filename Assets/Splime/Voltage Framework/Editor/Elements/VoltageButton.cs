using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voltage
{
	public class VoltageButton : VoltageElement
	{
		public bool m_pressed = false;
		public delegate void ButtonClickedHandler();
		public event ButtonClickedHandler OnClicked;
		public bool pressed{
			get
			{
				return m_pressed;
			}
		}

		#region Constructors


		public VoltageButton(GUIContent content, ButtonClickedHandler onClicked)
		{
			Content = content;
			OnClicked += onClicked;

			Style = ValidateStyle(Styles.GetStyle("Button"), "button");
		}
		public VoltageButton(GUIContent content, ButtonClickedHandler onClicked, ElementSettings settings) : this(content, onClicked)
		{
			ElementSettings = settings;
		}

		public VoltageButton(GUIContent content, ButtonClickedHandler onClicked, GUIStyle style) : this(content, onClicked)
		{
			Style = ValidateStyle(style, "Button", "button");
		}
		public VoltageButton(GUIContent content, ButtonClickedHandler onClicked, ElementSettings settings, GUIStyle style) : this(content, onClicked, settings)
		{
			Style = ValidateStyle(style, "Button", "button");
		}


		public VoltageButton(string content, ButtonClickedHandler onClicked)
		{
			Content = new GUIContent(content);
			OnClicked += onClicked;

			Style = ValidateStyle(Styles.GetStyle("Button"), "button");
		}
		public VoltageButton(string content, ButtonClickedHandler onClicked, ElementSettings settings) : this(content, onClicked)
		{
			ElementSettings = settings;
		}

		public VoltageButton(string content, ButtonClickedHandler onClicked, GUIStyle style) : this(content, onClicked)
		{
			Style = ValidateStyle(style, "Button", "button");
		}
		public VoltageButton(string content, ButtonClickedHandler onClicked, ElementSettings settings, GUIStyle _style) : this(content, onClicked, settings)
		{
			Style = ValidateStyle(_style, "Button", "button");
		}
		#endregion

		/// <summary>
		/// Do not use this.
		/// </summary>
		/// <param name="workingArea"></param>
		public override void DrawElement(Rect workingArea)
		{
			base.DrawElement(workingArea);

			m_pressed = GUI.Button(WorkingArea, Content, Style);
			if (m_pressed && OnClicked != null)
			{
				OnClicked();
			}
		}
	}
}
