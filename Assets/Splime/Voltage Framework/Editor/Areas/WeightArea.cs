using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voltage
{
	public class WeightArea : VoltageArea
	{

		#region CONSTRUCTORS

		public WeightArea()
		{

		}
		public WeightArea(ElementSettings elementSettings)
		{
			ElementSettings = elementSettings;
		}
		public WeightArea(AreaSettings areaSettings)
		{
			AreaSettings = areaSettings;
		}
		public WeightArea(ElementSettings elementSettings, AreaSettings areaSettings)
		{
			ElementSettings = elementSettings;
			AreaSettings = areaSettings;
		}

		public WeightArea(GUIStyle style)
		{
			Style = ValidateStyle(style, GUIStyle.none);
		}
		public WeightArea(ElementSettings elementSettings, GUIStyle style) : this(style)
		{
			ElementSettings = elementSettings;
		}
		public WeightArea(AreaSettings areaSettings, GUIStyle style) : this(style)
		{
			AreaSettings = areaSettings;
		}
		public WeightArea(ElementSettings elementSettings, AreaSettings areaSettings, GUIStyle style) : this(style)
		{
			AreaSettings = areaSettings;
			ElementSettings = elementSettings;
		}

		#endregion

		#region AREA METHODS

		/// <summary>
		/// Do not use this. Adds an element that will be erased after the Draw call.
		/// </summary>
		/// <param name="element"></param>s
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
			if(!hasCalculated)
			{
				foreach (VoltageElement element in m_wildFields)
				{
					if (element.FixedHeight > 0f && !Horizontal)
					{
						wildReservedSpace += element.CalcHeight(PaddedArea.width);
					}
					else if (element.FixedWidth > 0f && Horizontal)
					{
						wildReservedSpace += element.CalcWidth();
					}
					else
					{
						wildTotalWeights += element.Weight;
					}

					wildReservedSpace += Horizontal ? element.Margin.horizontal : element.Margin.vertical;
				}
				foreach (VoltageElement element in m_storedFields)
				{
					if (element.FixedHeight > 0f && !Horizontal)
					{
						storedReservedSpace += element.CalcHeight(PaddedArea.width);
					}
					else if (element.FixedWidth > 0f && Horizontal)
					{
						storedReservedSpace += element.CalcWidth();
					}
					else
					{
						storedTotalWeights += element.Weight;
					}

					storedReservedSpace += Horizontal ? element.Margin.horizontal : element.Margin.vertical;

					hasCalculated = true;
				}
			}
			
		}

		public override float CalcHeight(float width)
		{
			if (FixedSize.y > 0f)
				return FixedSize.y;

			CalcReservedSpace();
			float height = Padding.vertical;
			//height += Margin.vertical;
			width -= Padding.horizontal;

			//Debug.Log(PaddedArea.width);

			if (Horizontal)
			{
				float maxH = 0f;
				width -= ElementMargin * (ElementCount - 1);
				foreach (VoltageElement element in m_wildFields)
				{
					maxH = Mathf.Max(maxH, element.CalcHeight((width - ReservedSpace) * (element.Weight / TotalWeights)) + element.Margin.vertical);
				}
				foreach (VoltageElement element in m_storedFields)
				{
					maxH = Mathf.Max(maxH, element.CalcHeight((width - ReservedSpace) * (element.Weight / TotalWeights)) + element.Margin.vertical);
				}
				height += maxH;
			}
			else
			{
				height += ElementMargin * (ElementCount - 1);
				foreach (VoltageElement element in m_wildFields)
				{
					height += element.CalcHeight(width);
					height += element.Margin.vertical;
				}
				foreach (VoltageElement element in m_storedFields)
				{
					height += element.CalcHeight(width);
					height += element.Margin.vertical;
				}
			}


			return height;
		}

		/// <summary>
		/// Do not use this. Wipes all wild elements and resets this area. Called after each Draw call.
		/// </summary>
		public override void CleanWild()
		{
			base.CleanWild();
		}

		#endregion

		
		protected Rect GetElementSize(ref Rect currentPos,VoltageElement element)
		{
			Rect r;

			if (Horizontal)
			{
				currentPos.x += element.Margin.left;

				if (element.FixedWidth > 0f)
				{
					r = new Rect(currentPos.x, currentPos.y + element.Margin.top, 
						element.FixedSize.x , ((element.FixedHeight > 0f) ? element.FixedHeight : currentPos.height) - element.Margin.vertical);
				}
				else
				{
					r = new Rect(currentPos.x, currentPos.y + element.Margin.top, 
						(currentPos.width - ReservedSpace) * (element.Weight / TotalWeights) , ((element.FixedHeight > 0f) ? element.FixedHeight : currentPos.height) - element.Margin.vertical);
				}

				if (currentPos.height != r.height)
				{
					if (ElementAlignment == VoltageElementAlignment.Center)
					{
						r.y += (currentPos.height - r.height) / 2f;
					}
					else if (ElementAlignment == VoltageElementAlignment.BottomRight)
					{
						r.y += (currentPos.height - r.height);
					}
				}

				currentPos.x += r.width + element.Margin.right + ElementMargin;
			}
			else
			{
				currentPos.y += element.Margin.top;

				if (element.FixedHeight > 0f)
				{
					r = new Rect(currentPos.x + element.Margin.left, currentPos.y, ((element.FixedWidth > 0f) ? element.FixedWidth : currentPos.width) - element.Margin.horizontal, element.FixedHeight);
				}
				else
				{
					r = new Rect(currentPos.x + element.Margin.left, currentPos.y, ((element.FixedWidth > 0f) ? element.FixedWidth : currentPos.width) - element.Margin.horizontal, (currentPos.height - ReservedSpace) * (element.Weight / TotalWeights) - element.Margin.vertical);
				}
				if (currentPos.width != r.width)
				{
					if (ElementAlignment == VoltageElementAlignment.Center)
					{
						r.x += (currentPos.width - r.width) / 2f;
					}
					else if (ElementAlignment == VoltageElementAlignment.BottomRight)
					{
						r.x += (currentPos.width - r.width);
					}
				}
				

				currentPos.y += r.height + element.Margin.bottom + ElementMargin;
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

			GUI.BeginGroup(PaddedArea);
			Rect currentPos = new Rect(0f, 0f, PaddedArea.width, PaddedArea.height);
			foreach (VoltageElement element in m_storedFields)
			{
				element.DrawElement(GetElementSize(ref currentPos, element));
			}
			foreach (VoltageElement element in m_wildFields)
			{
				element.DrawElement(GetElementSize(ref currentPos,element));
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
				{
					element.EventCall(controlEvent, elementT, mousePos);
				}
			}
		}
	}
}

