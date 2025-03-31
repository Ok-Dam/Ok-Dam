using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voltage
{
	public abstract class VoltageElement
	{
		protected ElementSettings m_Settings = new ElementSettings(1);

		private Vector2 m_mousePos;

		private GUIContent m_content = new GUIContent();
		private GUIStyle m_style = GUIStyle.none;
		private Rect m_workingArea = new Rect();

		protected Dictionary<EventType, Action> EventMap { get; set; }


		#region PROPERTIES
		/// <summary>
		/// Weight | FixedSize | Flex | Padding | Margin
		/// </summary>
		public ElementSettings ElementSettings
		{
			get
			{
				return m_Settings;
			}
			set
			{
				m_Settings = value;
			}
		}
		
		public int Weight
		{
			get
			{
				return m_Settings.Weight;
			}
			set
			{
				m_Settings.Weight = value;
			}
		}
		
		public Vector2 FixedSize
		{
			get
			{
				return m_Settings.FixedSize;
			}
			set
			{
				m_Settings.FixedSize = value;
			}
		}
		public float FixedWidth
		{
			get
			{
				return m_Settings.FixedSize.x;
			}
			set
			{
				Vector2 fw = m_Settings.FixedSize;
				fw.x = value;
				m_Settings.FixedSize = fw;
			}
		}
		public float FixedHeight
		{
			get
			{
				return m_Settings.FixedSize.y;
			}
			set
			{
				Vector2 fh = m_Settings.FixedSize;
				fh.y = value;
				m_Settings.FixedSize = fh;
			}
		}

		public bool Flex
		{
			get
			{
				return m_Settings.Flex;
			}
			set
			{
				m_Settings.Flex = value;
			}
		}
		public RectOffset Margin
		{
			get
			{
				return m_Settings.Margin;
			}
			set
			{
				m_Settings.Margin = value;
			}
		}

		public GUIContent Content
		{
			get
			{
				return m_content;
			}
			set
			{
				if (value != null)
					m_content = value;
				else
					m_content = new GUIContent();
			}
		}
		public GUIStyle Style
		{
			get
			{
				return m_style;
			}
			set
			{
				m_style = ValidateStyle(value, "Label", GUIStyle.none);
			}
		}

		protected Rect WorkingArea
		{
			get
			{
				return m_workingArea;
			}
			set
			{
					m_workingArea.x = Mathf.Max(0f, value.x);
					m_workingArea.y = Mathf.Max(0f, value.y);
					m_workingArea.width = Mathf.Max(0f, value.width);
					m_workingArea.height = Mathf.Max(0f, value.height);
			}
		}
		#endregion

		#region CONSTRUCTORS
		public VoltageElement()
		{
			EventMapInit();
		}
		#endregion

		#region METHODS
		/// <summary>
		/// Returns the first style that's valid or GUIStyle.none if none is.
		/// </summary>
		/// <param name="voltageStyle"></param>
		/// <param name="fallbackClassicStyle"></param>
		/// <returns></returns>
		protected GUIStyle ValidateStyle(string voltageStyle, string fallbackClassicStyle)
		{
			return ValidateStyle(Styles.GetStyle(voltageStyle),fallbackClassicStyle);
		}
		/// <summary>
		/// Returns the first style that's valid or GUIStyle.none if none is.
		/// </summary>
		/// <param name="_style"></param>
		/// <param name="fallbackClassicStyle"></param>
		/// <returns></returns>
		protected GUIStyle ValidateStyle(GUIStyle _style, string fallbackClassicStyle)
		{
			
			if (_style == null)
			{
				//Debug.Log("Null Style");
				_style = new GUIStyle(fallbackClassicStyle);
			}

			if (_style == null)
			{
				//Debug.Log("Null Classic Style");
				_style = GUIStyle.none;
			}

			return _style;
		}
		/// <summary>
		/// Returns the first style that's valid or GUIStyle.none if none is.
		/// </summary>
		/// <param name="_style"></param>
		/// <param name="fallbackStyle"></param>
		/// <returns></returns>
		protected GUIStyle ValidateStyle(GUIStyle _style, GUIStyle fallbackStyle)
		{

			if (_style == null)
				_style = fallbackStyle;

			if (_style == null)
				_style = GUIStyle.none;

			return _style;
		}
		/// <summary>
		/// Returns the first style that's valid or GUIStyle.none if none is.
		/// </summary>
		/// <param name="_style"></param>
		/// <param name="fallbackVoltageStyle"></param>
		/// <param name="fallbackClassicStyle"></param>
		/// <returns></returns>
		protected GUIStyle ValidateStyle(GUIStyle _style, string fallbackVoltageStyle, string fallbackClassicStyle)
		{
			if (_style != null)
			{
				return _style;
			}
			else
			{
				return ValidateStyle(Styles.GetStyle(fallbackVoltageStyle),fallbackClassicStyle);
			}
		}
		/// <summary>
		/// Returns the first style that's valid or GUIStyle.none if none is.
		/// </summary>
		/// <param name="_style"></param>
		/// <param name="fallbackVoltageStyle"></param>
		/// <param name="fallbackStyle"></param>
		/// <returns></returns>
		protected GUIStyle ValidateStyle(GUIStyle _style, string fallbackVoltageStyle, GUIStyle fallbackStyle)
		{
			if (_style != null)
			{
				return _style;
			}
			else
			{
				return ValidateStyle(Styles.GetStyle(fallbackVoltageStyle), fallbackStyle);
			}
		}
		/// <summary>
		/// Calculates the width of the element.
		/// </summary>
		/// <returns></returns>
		public virtual float CalcWidth()
		{
			float width = Style.CalcSize(Content).x;
			//width += Margin.horizontal;
			if (FixedSize.x > 0f)
				width = FixedSize.x;

			return width;
		}
		/// <summary>
		/// Calculates the height of the element.
		/// </summary>
		/// <returns></returns>
		public virtual float CalcHeight(float width)
		{
			float height = Style.CalcHeight(Content, width);
			//height += Margin.vertical;
			if (FixedSize.y > 0f)
				height = FixedSize.y;

			return height;
		}
		/// <summary>
		/// Do not use this.
		/// </summary>
		/// <param name="workingArea"></param>
		public virtual void DrawElement(Rect workingArea)
		{
			WorkingArea = new Rect(workingArea.x, workingArea.y, workingArea.width /*- Margin.horizontal*/, workingArea.height/* - Margin.vertical*/);
		}
		#endregion


		#region EventHandling
		/// <summary>
		/// Do not use this.
		/// </summary>
		/// <param name="controlEvent"></param>
		/// <param name="eventPos"></param>
		public virtual void EventCall(EventType controlEvent, Rect eventPos, Vector2 mousePos)
		{
			WorkingArea = new Rect(eventPos.x, eventPos.y, eventPos.width, eventPos.height);
			m_mousePos = mousePos;
			//if (this.EventMap.ContainsKey(controlEvent))
			//{
			//	//Debug.Log(controlEvent.ToString());
			//	this.EventMap[controlEvent].Invoke();
			//}
		}

		protected void EventMapInit()
		{
			this.EventMap = new Dictionary<EventType, Action>
			{
				{ EventType.ContextClick, this.OnContext },
				{ EventType.Layout, this.OnLayout },
				{ EventType.Repaint, this.OnRepaint },

				{ EventType.KeyDown, () => {
					this.OnKeyDown(new Keyboard(Event.current));
				}},

				{ EventType.KeyUp, () => {
					this.OnKeyUp(new Keyboard(Event.current));
				}},

				{ EventType.MouseDown, () => {
					this.OnMouseDown((MouseButton)Event.current.button, m_mousePos);
				}},

				{ EventType.MouseUp, () => {
					this.OnMouseUp((MouseButton)Event.current.button, m_mousePos);
				}},

				{ EventType.MouseDrag, () => {
					this.OnMouseDrag((MouseButton)Event.current.button, m_mousePos,
						Event.current.delta);
				}},

				{ EventType.MouseMove, () => {
					this.OnMouseMove(m_mousePos, Event.current.delta);
				}},

				{ EventType.ScrollWheel, () => {
					this.OnScrollWheel(Event.current.delta, m_mousePos);
				}}
			};
		}
		
		protected virtual void OnKeyDown(Keyboard keyboard)
		{
		}

		protected virtual void OnKeyUp(Keyboard keyboard)
		{
		}

		protected virtual void OnMouseDown(MouseButton button, Vector2 position)
		{
			//Debug.Log("MouseDown " + position.ToString());
		}
		protected virtual void OnMouseUp(MouseButton button, Vector2 position)
		{
			//Debug.Log("MouseDown " + position.ToString());
		}
		protected virtual void OnMouseDrag(MouseButton button, Vector2 position, Vector2 delta)
		{
		}

		protected virtual void OnMouseMove(Vector2 position, Vector2 delta)
		{
		}

		protected virtual void OnContext()
		{
		}

		protected virtual void OnLayout()
		{
		}

		protected virtual void OnRepaint()
		{
		}

		protected virtual void OnScrollWheel(Vector2 delta, Vector2 position)
		{

		}
		#endregion
	}
}
