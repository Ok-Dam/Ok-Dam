using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voltage
{
	public class FoldoutArea : VoltageArea
	{
		protected VoltageArea area = new WeightArea();
		bool isOpen = true;

		GUIStyle header;
		GUIStyle body;
		GUIStyle iconOn;
		GUIStyle iconOff;

		#region CONSTRUCTORS
		private void ClassicStyles()
		{

			Style = ValidateStyle(Styles.GetStyle("Foldout Title"), GUIStyle.none);
			if(Horizontal)
				header = ValidateStyle(Styles.GetStyle("Foldout Header Horizontal"), "Box");
			else
				header = ValidateStyle(Styles.GetStyle("Foldout Header Vertical"), "Box");
			if (Horizontal)
				body = ValidateStyle(Styles.GetStyle("Foldout Body Horizontal"), "Box");
			else
				body = ValidateStyle(Styles.GetStyle("Foldout Body Vertical"), "Box");
			iconOn = ValidateStyle(Styles.GetStyle("Foldout On"), "Foldout On");
			iconOff = ValidateStyle(Styles.GetStyle("Foldout Off"), "Foldout Off");

			area.Style = body;
		}

		public FoldoutArea(string label)
		{
			Horizontal = false;
			Content = new GUIContent(label);

			ClassicStyles();
		}
		public FoldoutArea(string label, ElementSettings elementSettings) : this(label)
		{
			ElementSettings = elementSettings;
		}
		public FoldoutArea(string label, AreaSettings areaSettings) : this(label)
		{
			AreaSettings = areaSettings;
			ClassicStyles();
		}
		public FoldoutArea(string label, ElementSettings elementSettings, AreaSettings areaSettings) : this(label)
		{
			ElementSettings = elementSettings;
			AreaSettings = areaSettings;
			ClassicStyles();
		}

		public FoldoutArea(string label, GUIStyle titleStyle, GUIStyle headerStyle, GUIStyle bodyStyle) : this(label)
		{
			Style = ValidateStyle(titleStyle, GUIStyle.none);
			header = ValidateStyle(headerStyle, GUIStyle.none);
			body = ValidateStyle(bodyStyle, GUIStyle.none);
		}
		public FoldoutArea(string label, ElementSettings elementSettings, GUIStyle titleStyle, GUIStyle headerStyle, GUIStyle bodyStyle) : this(label, elementSettings)
		{
			Style = ValidateStyle(titleStyle, GUIStyle.none);
			header = ValidateStyle(headerStyle, GUIStyle.none);
			body = ValidateStyle(bodyStyle, GUIStyle.none);
		}
		public FoldoutArea(string label, AreaSettings areaSettings, GUIStyle titleStyle, GUIStyle headerStyle, GUIStyle bodyStyle) : this(label, areaSettings)
		{
			Style = ValidateStyle(titleStyle, GUIStyle.none);
			header = ValidateStyle(headerStyle, GUIStyle.none);
			body = ValidateStyle(bodyStyle, GUIStyle.none);
		}
		public FoldoutArea(string label, ElementSettings elementSettings, AreaSettings areaSettings, GUIStyle titleStyle, GUIStyle headerStyle, GUIStyle bodyStyle) : this(label, elementSettings, areaSettings)
		{
			Style = ValidateStyle(titleStyle, GUIStyle.none);
			header = ValidateStyle(headerStyle, GUIStyle.none);
			body = ValidateStyle(bodyStyle, GUIStyle.none);
		}
		#endregion

		#region AREA METHODS
		/// <summary>
		/// Do not use this. Adds an element that will be erased after drawn.
		/// </summary>
		/// <param name="element"></param>
		public override void AddWildElement(VoltageElement element)
		{
			area.AddWildElement(element);
		}
		/// <summary>
		/// Adds a permanent element to this area. Can't be deleted.
		/// </summary>
		/// <param name="element"></param>
		public override void AddStoredElement(VoltageElement element)
		{
			area.AddStoredElement(element);
		}
		/// <summary>
		/// Do not use this. Wipes all wild elements and resets this area. Called after each Draw call.
		/// </summary>
		public override void CleanWild()
		{
			base.CleanWild();
			area.CleanWild();
		}
		
		/// <summary>
		/// Calculates the min width of the area with all of its elements
		/// </summary>
		/// <returns></returns>
		public override float CalcWidth()
		{
			float width = 0f;
			if (Horizontal)
			{
				width += 18f;
				if (isOpen)
					width += area.CalcWidth();
			}
			else
			{
				float a = area.CalcWidth();
				width += a;
			}
			//Debug.Log("W " + width);
			return width;
		}
		/// <summary>
		/// Calculates the min height of the area with all of its elements.
		/// </summary>
		/// <returns></returns>
		public override float CalcHeight(float width)
		{
			float height = 0f;

			if (!Horizontal)
			{
				height += Style.CalcSize(new GUIContent("Title")).y;
				if (isOpen)
					height += area.CalcHeight(width);
			}
			else
			{
				float a = area.CalcHeight(width);
				//Debug.Log("A " + a);
				height += a;
			}
			//Debug.Log("H " + height);
			return height;
		}
		#endregion

		protected Rect GetHeaderSize(ref Rect currentPos)
		{
			//float x = Style.CalcSize(new GUIContent("AA")).x;
			float y = Mathf.Max(Style.CalcSize(new GUIContent("Title")).y, 18f);
			Rect r;
			if (Horizontal)
			{
				r = new Rect(currentPos.x /*+ Padding.left*/, currentPos.y /*+ Padding.top*/, 18f, currentPos.height/* - Padding.vertical*/);
				
				currentPos.x += r.width;
			}
			else
			{
				r = new Rect(currentPos.x /*+ Padding.left*/, currentPos.y /*+ Padding.top*/, currentPos.width /*- Padding.horizontal*/, y);
				
				currentPos.y += r.height;
			}
			r.width = Mathf.Max(0f, r.width);
			r.height = Mathf.Max(0f, r.height);

			return r;
		}
		protected Rect GetAreaSize(ref Rect currentPos, Rect headerPos)
		{
			Rect r;
			if (Horizontal)
			{
				r = new Rect(currentPos.x, currentPos.y /*+ Padding.top*/, currentPos.width /*- Padding.right*/ - (headerPos.width), currentPos.height /*- Padding.vertical*/);

				currentPos.x += r.width;
			}
			else
			{
				r = new Rect(currentPos.x /*+ Padding.left*/, currentPos.y, currentPos.width /*- Padding.horizontal*/, currentPos.height /*- Padding.bottom*/ - headerPos.height);

				currentPos.y += r.height;
			}
			r.width = Mathf.Max(0f, r.width);
			r.height = Mathf.Max(0f, r.height);

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

			Rect headerPos = GetHeaderSize(ref currentPos);
			Rect iconPos;
			if (Horizontal)
			{
				iconPos = new Rect(headerPos.x+1f, headerPos.height/2f-8f, 16f, 16f);
			}
			else
			{
				iconPos = new Rect(headerPos.x, headerPos.y, 16f, 16f);
			}


			GUI.Box(headerPos, "", header);
			
			GUI.Box(iconPos, "", isOpen ? iconOn : iconOff);
			if (!Horizontal)
			{
				Rect titlePos = new Rect(headerPos.x + iconPos.width, headerPos.y, headerPos.width - iconPos.width, headerPos.height);
				GUI.Label(titlePos, Content,  Style);
			}
			
			if(isOpen)
				area.DrawElement(GetAreaSize(ref currentPos, headerPos));

			GUI.EndGroup();

			CleanWild();
		}

		/// <summary>
		/// Do not use this.
		/// </summary>
		/// <param name="controlEvent"></param>
		/// <param name="_eventPos"></param>
		public override void EventCall(EventType controlEvent, Rect eventPos, Vector2 mousePos)
		{
			base.EventCall(controlEvent, eventPos, mousePos);

			Rect currentPos = new Rect(WorkingArea.x, WorkingArea.y, WorkingArea.width, WorkingArea.height);

			//Vector2 pos = Event.current.mousePosition;
			Rect headerPos = GetHeaderSize(ref currentPos);
			Rect areaPos = GetAreaSize(ref currentPos, headerPos);

			if (headerPos.Contains(mousePos))
			{
				if (this.EventMap.ContainsKey(controlEvent))
				{
					this.EventMap[controlEvent].Invoke();
				}
			}
			else if (areaPos.Contains(mousePos))
			{
				area.EventCall(controlEvent, areaPos, mousePos);
			}
		}
		protected override void OnMouseDown(MouseButton button, Vector2 position)
		{
			isOpen = !isOpen;
			Event.current.Use();
		}
	}
}

