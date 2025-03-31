using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voltage
{
	public class VoltageLink : VoltageElement
	{
		private bool m_pressed = false;
		private string m_url = "https://assetstore.unity.com/packages/tools/gui/voltage-editor-ui-framework-110077";

		public bool Pressed
		{
			get
			{
				return m_pressed;
			}
		}
		public string Url
		{
			get
			{
				return m_url;
			}
			set{
				m_url = value;
			}
		}
		#region Constructors


		public VoltageLink(GUIContent content, string link)
		{
			Content = content;
			Url = link;

			Style = ValidateStyle(Styles.GetStyle("Link"), "button");
		}
		public VoltageLink(GUIContent content, string onClicked, ElementSettings settings) : this(content, onClicked)
		{
			ElementSettings = settings;
		}

		public VoltageLink(GUIContent content, string onClicked, GUIStyle style) : this(content, onClicked)
		{
			Style = ValidateStyle(style, "Link", "button");
		}
		public VoltageLink(GUIContent content, string onClicked, ElementSettings settings, GUIStyle style) : this(content, onClicked, settings)
		{
			Style = ValidateStyle(style, "Link", "button");
		}


		public VoltageLink(string content, string url)
		{
			Content = new GUIContent(content);
			Url = url;

			Style = ValidateStyle(Styles.GetStyle("Link"), "button");
		}
		public VoltageLink(string content, string url, ElementSettings settings) : this(content, url)
		{
			ElementSettings = settings;
		}

		public VoltageLink(string content, string url, GUIStyle style) : this(content, url)
		{
			Style = ValidateStyle(style, "Link", "button");
		}
		public VoltageLink(string content, string url, ElementSettings settings, GUIStyle _style) : this(content, url, settings)
		{
			Style = ValidateStyle(_style, "Link", "button");
		}
		#endregion

		/// <summary>
		/// Do not use this.
		/// </summary>
		/// <param name="workingArea"></param>
		public override void DrawElement(Rect workingArea)
		{
			base.DrawElement(workingArea);

			EditorGUIUtility.AddCursorRect(WorkingArea, MouseCursor.Link);

			if(m_pressed = GUI.Button(WorkingArea, Content, Style))
			{
				if (!string.IsNullOrEmpty(Url))
				{
					Application.OpenURL(Url);
				}
			}
			
		}
	}
}
