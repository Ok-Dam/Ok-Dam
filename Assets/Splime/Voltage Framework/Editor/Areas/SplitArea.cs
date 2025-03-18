using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace Voltage
{
	public class SplitArea : VoltageArea
	{
		#region PROPERTIES
		protected VoltageArea area1 = new WeightArea();
		protected VoltageArea area2 = new WeightArea();
		protected VoltageArea current;
		protected GUIStyle lineStyle;
		protected float lineWidth;
		private bool m_dragged = false;
		private float m_division = 0.5f;
		public float Division
		{
			get
			{
				return m_division;
			}
			set
			{
				m_division = Mathf.Clamp(value, 0.1f, 0.9f);
			}
		}
		#endregion


		private void LineSettings()
		{
			lineStyle = new GUIStyle();
			Color col = EditorGUIUtility.isProSkin
			 ? (Color)new Color32(56, 56, 56, 255)
			 : (Color)new Color32(142, 142, 142, 255);

			Texture2D t = new Texture2D(2, 2);
			t.SetPixels(new Color[] { col, col, col, col });
			t.Apply();
			lineStyle.normal.background = t;
			lineWidth = 4f;
		}

		#region CONTRUCTORS
		public SplitArea(float division)
		{
			Division = division;

			current = area1;

			LineSettings();
		}
		public SplitArea(float division, ElementSettings elementSettings) : this(division)
		{
			ElementSettings = elementSettings;
		}
		public SplitArea(float division, AreaSettings areaSettings) : this(division)
		{
			AreaSettings = areaSettings;
		}
		public SplitArea(float division, ElementSettings elementSettings, AreaSettings areaSettings) : this(division)
		{
			ElementSettings = elementSettings;
			AreaSettings = areaSettings;
		}

		public SplitArea(float division, GUIStyle style) : this(division)
		{
			Style = ValidateStyle(style, GUIStyle.none);
		}
		public SplitArea(float division, ElementSettings elementSettings, GUIStyle style) : this(division, elementSettings)
		{
			Style = ValidateStyle(style, GUIStyle.none);
		}
		public SplitArea(float division, AreaSettings areaSettings, GUIStyle style) : this(division, areaSettings)
		{
			Style = ValidateStyle(style, GUIStyle.none);
		}
		public SplitArea(float division, ElementSettings elementSettings, AreaSettings areaSettings, GUIStyle style) : this(division, elementSettings, areaSettings)
		{
			Style = ValidateStyle(style, GUIStyle.none);
		}
		#endregion

		/// <summary>
		/// Calculates the min width of the area with all of its elements
		/// </summary>
		/// <returns></returns>
		public override float CalcWidth()
		{
			if (FixedSize.x > 0f)
				return FixedSize.x;

			float width = 0f;
			//width += Margin.horizontal;
			if (Horizontal)
			{
				width = Padding.horizontal * 2f;

				width += area1.CalcWidth() + area1.Margin.horizontal;
				width += area2.CalcWidth() + area2.Margin.horizontal;

				width += ElementMargin * (ElementCount - 1);
			}
			else
			{
				width = Padding.horizontal;

				float maxW = Mathf.Max(area1.CalcWidth() + area1.Margin.horizontal, area2.CalcWidth() + area2.Margin.horizontal);

				width += maxW;
			}
			return width;
		}
		/// <summary>
		/// Calculates the min height of the area with all of its elements.
		/// </summary>
		/// <returns></returns>
		public override float CalcHeight(float width)
		{
			if (FixedSize.y > 0f)
				return FixedSize.y;

			float height = 0f;
			//height += Margin.vertical;

			if (Horizontal)
			{
				height = Padding.vertical;

				float maxW = Mathf.Max(area1.CalcHeight(width) + area1.Margin.horizontal, area2.CalcHeight(width) + area2.Margin.horizontal);

				height += maxW;
			}
			else
			{
				height = Padding.vertical * 2f;

				height += area1.CalcHeight(width) + area1.Margin.vertical;
				height += area2.CalcHeight(width) + area2.Margin.vertical;

				height += ElementMargin * (ElementCount - 1);
			}


			return height;
		}

		/// <summary>
		/// Do not use this. Adds an element that will be erased after drawn.
		/// </summary>
		/// <param name="element"></param>
		public override void AddWildElement(VoltageElement element)
		{
			current.AddWildElement(element);
		}
		/// <summary>
		/// VoltageInit: Adds a permanent element to this area. Can't be deleted.
		/// </summary>
		/// <param name="element"></param>
		public override void AddStoredElement(VoltageElement element)
		{
			current.AddStoredElement(element);
		}
		/// <summary>
		/// Do not use this. Wipes all wild elements and resets this area. Called after each Draw call. 
		/// </summary>
		public override void CleanWild()
		{
			base.CleanWild();
			area1.CleanWild();
			area2.CleanWild();
		}

		/// <summary>
		/// VoltageGUI/Init: Makes the second area active to add elements.
		/// </summary>
		public void Split()
		{
			current = area2;
		}


		#region DRAW
		private Rect GetElementSize(float percentage,ref Rect currentPos)
		{
			Rect r;
			if (Horizontal)
			{
				r = new Rect(currentPos.x + Padding.left, currentPos.y + Padding.top, (currentPos.width - lineWidth) * percentage - Padding.horizontal, currentPos.height - Padding.vertical);
				
				currentPos.x += r.width + Padding.horizontal;
			}
			else
			{
				r = new Rect(currentPos.x + Padding.left, currentPos.y + Padding.top, currentPos.width - Padding.horizontal, (currentPos.height - lineWidth) * percentage - Padding.vertical);
				
				currentPos.y += r.height + Padding.vertical;
			}
			r.width = Mathf.Max(0f, r.width);
			r.height = Mathf.Max(0f, r.height);

			return r;
		}
		private Rect GetSeparationSize(ref Rect currentPos,bool cursorChange = false)
		{

			Rect r;

			
			if (Horizontal)
			{
				r = new Rect(currentPos.x, currentPos.y, lineWidth, currentPos.height);
				currentPos.x += r.width;
				if (cursorChange)
					EditorGUIUtility.AddCursorRect(r, MouseCursor.ResizeHorizontal);
			}
			else
			{
				r = new Rect(currentPos.x, currentPos.y, currentPos.width, lineWidth);
				currentPos.y += r.height;
				if (cursorChange)
					EditorGUIUtility.AddCursorRect(r, MouseCursor.ResizeVertical);
			}
			

			return r;
		}
		/// <summary>
		/// Do not use this. Draws the area and all elements on it.
		/// </summary>
		/// <param name="workingArea">Drawing Rect</param>
		public override void DrawElement(Rect workingArea)
		{
			base.DrawElement(workingArea);
			 
			//DRAW
			GUI.BeginGroup(WorkingArea);
			Rect currentPos = new Rect(0f, 0f, WorkingArea.width, WorkingArea.height);

				area1.DrawElement(GetElementSize(Division, ref currentPos));

				GUI.Box(GetSeparationSize(ref currentPos, true), "", lineStyle);

				area2.DrawElement(GetElementSize((1f-Division), ref currentPos));

			GUI.EndGroup();
			current = area1;
		}
		#endregion

		#region EVENT HANDLING
		/// <summary>
		/// Do not use this.
		/// </summary>
		/// <param name="controlEvent"></param>
		/// <param name="_eventPos"></param>
		public override void EventCall(EventType controlEvent, Rect eventPos, Vector2 mousePos)
		{
			base.EventCall(controlEvent, eventPos, mousePos);


			Rect currentPos = new Rect(WorkingArea.x, WorkingArea.y, WorkingArea.width, WorkingArea.height);

			if (!m_dragged)
			{
				Rect r1 = GetElementSize(Division, ref currentPos);
				Rect rS = GetSeparationSize(ref currentPos);
				Rect r2 = GetElementSize((1f - Division), ref currentPos);

				if (r1.Contains(mousePos))
				{
					area1.EventCall(controlEvent, r1, mousePos);
				}
				else if (rS.Contains(mousePos))
				{
					if (this.EventMap.ContainsKey(controlEvent))
					{
						this.EventMap[controlEvent].Invoke();
					}
				}
				else if (r2.Contains(mousePos))
				{
					area2.EventCall(controlEvent, r2, mousePos);
				}
			}
			else if (this.EventMap.ContainsKey(controlEvent))
			{
				this.EventMap[controlEvent].Invoke();
			}
		}
		protected override void OnMouseDown(MouseButton button, Vector2 position)
		{
			m_dragged = true;
			Event.current.Use();
		}
		protected override void OnMouseDrag(MouseButton button, Vector2 position, Vector2 delta)
		{
			if (m_dragged)
			{
				if (Horizontal)
					Division = (position.x - WorkingArea.x) / WorkingArea.width;
				else
					Division = (position.y - WorkingArea.y) / WorkingArea.height;
				Event.current.Use();
			}
		}
		protected override void OnMouseUp(MouseButton button, Vector2 position)
		{
			m_dragged = false;
			Event.current.Use();
		}
		#endregion
	}
}

