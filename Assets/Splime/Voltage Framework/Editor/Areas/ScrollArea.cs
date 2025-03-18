using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voltage;

namespace Voltage
{
	public class ScrollArea : VoltageArea
	{
		protected VoltageArea area = new StreamArea();
		Vector2 scroll = Vector2.zero;

		#region CONSTRUCTORS
		public ScrollArea()
		{
			Horizontal = false;
		}
		public ScrollArea(ElementSettings elementSettings) : this()
		{
			ElementSettings = elementSettings;
		}
		public ScrollArea(AreaSettings areaSettings) : this()
		{
			AreaSettings = areaSettings;
		}
		public ScrollArea(ElementSettings elementSettings, AreaSettings areaSettings) : this()
		{
			ElementSettings = elementSettings;
			AreaSettings = areaSettings;
		}

		public ScrollArea(GUIStyle style) : this()
		{
			Style = style;
		}
		public ScrollArea(ElementSettings elementSettings, GUIStyle style) : this(style)
		{
			ElementSettings = elementSettings;
		}
		public ScrollArea(AreaSettings areaSettings, GUIStyle style) : this(style)
		{
			AreaSettings = areaSettings;
		}
		public ScrollArea(ElementSettings elementSettings, AreaSettings areaSettings, GUIStyle style) : this(style)
		{
			ElementSettings = elementSettings;
			AreaSettings = areaSettings;
		}
		#endregion

		#region AREA METHODS
		/// <summary>
		/// Adds an element that will be erased after drawn. Do not use this.
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
			area.CleanWild();
		}

		/// <summary>
		/// Calculates the min width of the area with all of its elements
		/// </summary>
		/// <returns></returns>
		public override float CalcWidth()
		{
			if (FixedSize.x > 0f)
				return FixedSize.x;

			return area.CalcWidth();
		}
		/// <summary>
		/// Calculates the min height of the area with all of its elements.
		/// </summary>
		/// <returns></returns>
		public override float CalcHeight(float width)
		{
			if (FixedSize.y > 0f)
				return FixedSize.y;

			float areaWidth = area.CalcWidth();// + Padding.horizontal;
			return area.CalcHeight(Mathf.Max(width - Padding.horizontal, areaWidth));
		}
		#endregion

		/// <summary>
		/// Do not use this. Draws the area and all elements on it.
		/// </summary>
		/// <param name="workingArea">Drawing Rect</param>
		public override void DrawElement(Rect workingArea)
		{
			base.DrawElement(workingArea);

			float barH = 15f;

			float areaWidth = area.CalcWidth();// + Padding.horizontal;
			float areaHeight = area.CalcHeight(Mathf.Max(PaddedArea.width, areaWidth));// + Padding.vertical;

			Rect currentPos = new Rect();

			

			currentPos = new Rect(0f, 0f, Mathf.Max(PaddedArea.width, areaWidth), Mathf.Max(PaddedArea.height, areaHeight));

			Rect scrollArea = new Rect(0f, 0f, currentPos.width /*+ Padding.horizontal*/, currentPos.height /*+ Padding.vertical*/);
			
			if (scrollArea.width > WorkingArea.width)
			{
				if (Horizontal)
				{
					currentPos.height = Mathf.Max(PaddedArea.height - barH, (areaHeight));// - Padding.vertical));
				}

				if (areaHeight < WorkingArea.height - barH)
				{
					scrollArea.height = Mathf.Clamp(scrollArea.height - barH, areaHeight, currentPos.height /*+ Padding.vertical*/ - barH);
				}
				else
				{
					scrollArea.height = Mathf.Max(areaHeight, currentPos.height/* + Padding.vertical*/ - barH);
				}
			}


			if (scrollArea.height > WorkingArea.height)
			{
				if (!Horizontal)
				{
					currentPos.width = Mathf.Max(PaddedArea.width - barH, (areaWidth));// - Padding.horizontal));
				}

				if (areaWidth < WorkingArea.width - barH)
				{
					scrollArea.width = Mathf.Clamp(scrollArea.width - barH, areaWidth, currentPos.width /*+ Padding.horizontal*/ - barH);
				}
				else
				{
					scrollArea.width = Mathf.Max(areaWidth, currentPos.width /*+ Padding.horizontal */- barH);
				}
			}

			scroll = GUI.BeginScrollView(PaddedArea, scroll, scrollArea, false, false);


			//Draw
			area.DrawElement(currentPos);

			GUI.EndScrollView();

		}

		/// <summary>
		/// Do not use this.
		/// </summary>
		/// <param name="controlEvent"></param>
		/// <param name="_eventPos"></param>
		public override void EventCall(EventType controlEvent, Rect eventPos, Vector2 mousePos)
		{
			base.EventCall(controlEvent, eventPos, mousePos);

			float barH = 15f;
			float areaWidth = area.CalcWidth();
			float areaHeight = area.CalcHeight(Mathf.Max(PaddedArea.width, areaWidth));

			Rect currentPos = new Rect(PaddedArea.x, PaddedArea.y, areaWidth, areaHeight);
			Rect eventArea = new Rect(PaddedArea.x, PaddedArea.y, PaddedArea.width, PaddedArea.height);

			if (currentPos.width > eventArea.width)
			{
				eventArea.height -= barH;

				if (currentPos.height > eventArea.height)
				{
					eventArea.width -= barH;
				}
			}
			else if (currentPos.height > eventArea.height)
			{
				eventArea.width -= barH;
				if (currentPos.width > eventArea.width)
				{
					eventArea.height -= barH;
				}
			}

			
			//currentPos.x += scroll.x;
			//currentPos.y += scroll.y;
			if (eventArea.Contains(mousePos))
			{
				mousePos += scroll;
				//eventArea.width = currentPos.width;
				//eventArea.height = currentPos.height;

				area.EventCall(controlEvent, eventArea, mousePos);
			}
		}
	}
}

