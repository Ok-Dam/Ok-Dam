using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voltage;

namespace Voltage
{
	public class StreamArea : VoltageArea
	{

		#region CONSTRUCTORS

		public StreamArea()
		{
			Horizontal = false;
		}
		public StreamArea(ElementSettings elementSettings) : this()
		{
			ElementSettings = elementSettings;
		}
		public StreamArea(AreaSettings areaSettings) : this()
		{
			AreaSettings = areaSettings;
		}
		public StreamArea(ElementSettings elementSettings, AreaSettings areaSettings) : this()
		{
			ElementSettings = elementSettings;
			AreaSettings = areaSettings;

		}

		public StreamArea(GUIStyle style) : this()
		{
			Style = ValidateStyle(style, GUIStyle.none);

		}
		public StreamArea(ElementSettings elementSettings, GUIStyle style) : this(style)
		{
			ElementSettings = elementSettings;

		}
		public StreamArea(AreaSettings areaSettings, GUIStyle style) : this(style)
		{
			AreaSettings = areaSettings;

		}
		public StreamArea(ElementSettings elementSettings, AreaSettings areaSettings, GUIStyle style) : this(style)
		{
			ElementSettings = elementSettings;
			AreaSettings = areaSettings;
		}

		#endregion

		#region AREA METHODS

		/// <summary>
		/// Do not use this. Adds an element that will be erased after drawn.
		/// </summary>
		/// <param name="element"></param>
		public override void AddWildElement(VoltageElement element)
		{
			base.AddWildElement(element);
		}
		/// <summary>
		/// Adds a permanent element to this area. Can't be deleted.
		/// </summary>
		/// <param name="element"></param>
		public override void AddStoredElement(VoltageElement element)
		{
			base.AddStoredElement(element);
		}

		protected override void CalcReservedSpace()
		{
			base.CalcReservedSpace();

			foreach (VoltageElement element in m_wildFields)
			{
				if (!element.Flex)
				{
					wildReservedSpace += Horizontal ? element.CalcWidth() : element.CalcHeight(WorkingArea.width);
					wildReservedSpace += Horizontal ? element.Margin.horizontal : element.Margin.vertical;
				}
				else
				{
					wildTotalWeights += element.Weight;
				}
			}
			foreach (VoltageElement element in m_storedFields)
			{
				if (!element.Flex)
				{
					storedReservedSpace += Horizontal ? element.CalcWidth() : element.CalcHeight(WorkingArea.width);
					storedReservedSpace += Horizontal ? element.Margin.horizontal : element.Margin.vertical;
				}
				else
				{
					storedTotalWeights += element.Weight;
				}
			}
		}

		/// <summary>
		/// Calculates the min width of the area with all of its elements
		/// </summary>
		/// <returns></returns>
		public override float CalcWidth()
		{
			float width = base.CalcWidth();
			return width;
		}
		/// <summary>
		/// Calculates the min height of the area with all of its elements.
		/// </summary>
		/// <returns></returns>
		public override float CalcHeight(float width)
		{
			float height = base.CalcHeight(width);
			return height;
		}

		#endregion

		protected Rect GetElementSize(ref Rect currentPos, VoltageElement element)
		{
			Rect r;
			float extra = 0f;

			if (Horizontal)
			{
				if (TotalWeights == 0)
				{
					switch (ElementAlignment)
					{
						case VoltageElementAlignment.TopLeft:
							extra = 0f;
							break;
						case VoltageElementAlignment.Center:
							extra = Mathf.Max(0f,(currentPos.width - ReservedSpace) / 2f);
							break;
						case VoltageElementAlignment.BottomRight:
							extra = Mathf.Max(0f, (currentPos.width - ReservedSpace));
							break;
						default:
							break;
					}
				}

				float width = (element.Flex) ? (currentPos.width - ReservedSpace) * (element.Weight / TotalWeights) - element.Margin.horizontal : element.CalcWidth();

				r = new Rect(currentPos.x + element.Margin.left + extra, currentPos.y + element.Margin.top, width , currentPos.height - element.Margin.vertical);
				

				currentPos.x += r.width + element.Margin.right + ElementMargin;
			}
			else
			{
				if (TotalWeights == 0)
				{
					switch (ElementAlignment)
					{
						case VoltageElementAlignment.TopLeft:
							extra = 0f;
							break;
						case VoltageElementAlignment.Center:
							extra = Mathf.Max(0f, (currentPos.height - ReservedSpace) / 2f);
							break;
						case VoltageElementAlignment.BottomRight:
							extra = Mathf.Max(0f, (currentPos.height - ReservedSpace));
							break;
						default:
							break;
					}
				}
				float height = (element.Flex) ? (currentPos.height - ReservedSpace) * (element.Weight / TotalWeights) - element.Margin.vertical: element.CalcHeight(currentPos.width);

				r = new Rect(currentPos.x + element.Margin.left, currentPos.y + element.Margin.top + extra, currentPos.width - element.Margin.horizontal, height );


				currentPos.y += r.height + element.Margin.bottom + ElementMargin;
			}

			return r;
		}
		/// <summary>
		/// Do not use this. Draws the area and all elements on it.
		/// </summary>
		/// <param name="workingArea">Drawing Rect</param>s
		public override void DrawElement(Rect workingArea)
		{
			base.DrawElement(workingArea);
			GUI.BeginGroup(PaddedArea);
			Rect currentPos = new Rect(0f, 0f, PaddedArea.width, PaddedArea.height);

			//Draw
			foreach (VoltageElement element in m_storedFields)
			{
				element.DrawElement(GetElementSize(ref currentPos, element));
			}
			foreach (VoltageElement element in m_wildFields)
			{
				element.DrawElement(GetElementSize(ref currentPos, element));
			}

			GUI.EndGroup();
		}

		/// <summary>
		/// Do not use this.
		/// </summary>
		/// <param name="controlEvent"></param>
		/// <param name="_eventPos"></param>
		public override void EventCall(EventType controlEvent, Rect eventPos, Vector2 mousePos)
		{
			base.EventCall(controlEvent, eventPos, mousePos);

			Rect currentPos = new Rect(PaddedArea.x, PaddedArea.y, PaddedArea.width, PaddedArea.height);
			
			Rect elementT;
			foreach (VoltageElement element in m_storedFields)
			{
				elementT = GetElementSize(ref currentPos, element);
				if (elementT.Contains(mousePos))
					element.EventCall(controlEvent, elementT, mousePos);
			}
			foreach (VoltageElement element in m_wildFields)
			{
				elementT = GetElementSize(ref currentPos, element);
				if (elementT.Contains(mousePos))
					element.EventCall(controlEvent, elementT, mousePos);
			}
		}
	}
}

