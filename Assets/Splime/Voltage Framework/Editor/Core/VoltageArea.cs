using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voltage
{
	public enum VoltageElementAlignment
	{
		Inherit,
		TopLeft,
		Center,
		BottomRight
	}

	public abstract class VoltageArea : VoltageElement
	{
		protected VoltageArea m_previousArea;
		protected List<VoltageElement> m_wildFields = new List<VoltageElement>(0);
		protected List<VoltageElement> m_storedFields = new List<VoltageElement>(0);
		private AreaSettings m_areaSettings = new AreaSettings(true);

		protected float wildTotalWeights = 0f;
		protected float wildReservedSpace = 0f;

		protected float storedTotalWeights = 0f;
		protected float storedReservedSpace = 0f;

		protected bool hasCalculated = false;

		protected int ElementCount
		{
			get { return Mathf.Max(0, m_wildFields.Count + m_storedFields.Count); }
		}
		protected float TotalWeights
		{
			get
			{
				return wildTotalWeights + storedTotalWeights;
			}
		}
		protected float ReservedSpace
		{
			get
			{
				return wildReservedSpace + storedReservedSpace + ((ElementCount - 1) * ElementMargin);
			}
		}

		#region PROPERTIES
		/// <summary>
		/// Used for OnGUI. Do not use this.
		/// </summary>
		public VoltageArea PreviousArea
		{
			get
			{
				return m_previousArea;
			}
			set
			{
				m_previousArea = value;
			}
		}
		/// <summary>
		/// Horizontal | ElementAlignment | ElementMargin
		/// </summary>
		public AreaSettings AreaSettings
		{
			get
			{
				return m_areaSettings;
			}
			set
			{
				m_areaSettings = value;
			}
		}
		public bool Horizontal
		{
			get
			{
				return m_areaSettings.Horizontal;
			}
			set
			{
				m_areaSettings.Horizontal = value;
			}
		}
		public RectOffset Padding
		{
			get
			{
				return m_areaSettings.Padding;
			}
			set
			{
				m_areaSettings.Padding = value;
			}
		}
		protected Rect PaddedArea
		{
			get
			{
				return new Rect(WorkingArea.x + AreaSettings.Padding.left, WorkingArea.y + AreaSettings.Padding.top,
					WorkingArea.width - AreaSettings.Padding.horizontal, WorkingArea.height - AreaSettings.Padding.vertical);
			}
		}
		public float ElementMargin
		{
			get
			{
				return m_areaSettings.ElementMargin;
			}
			set
			{
				m_areaSettings.ElementMargin = Mathf.Max(0f, value);
			}
		}
		public VoltageElementAlignment ElementAlignment
		{
			get
			{
				return m_areaSettings.ElementAlignment;
			}
			set
			{
				m_areaSettings.ElementAlignment = value;
			}
		}
		#endregion

		#region CONSTRUCTORS
		protected VoltageArea(){ }
		#endregion

		public virtual void AddWildElement(VoltageElement element)
		{
			m_wildFields.Add(element);
		}
		public virtual void AddStoredElement(VoltageElement element)
		{
			m_storedFields.Add(element);
		}
		protected virtual void CalcReservedSpace()
		{
			if(!hasCalculated)
			{
				wildReservedSpace = 0f;
				wildTotalWeights = 0;

				storedReservedSpace = 0f;
				storedTotalWeights = 0;
			}

		}

		/// <summary>
		/// Do not use this. Wipes all wild elements and resets this area. Called after each Draw call.
		/// </summary>
		public virtual void CleanWild()
		{
			foreach(VoltageElement area in m_wildFields)
			{
				if(area is VoltageArea)
				{
					((VoltageArea) area).CleanWild();
				}
			}
			m_wildFields.Clear();
			wildTotalWeights = 0f;
			wildReservedSpace = 0f;

			hasCalculated = false;

		}
		/// <summary>
		/// Draws the element. Do not use this.
		/// </summary>
		/// <param name="workingArea"></param>
		public override void DrawElement(Rect workingArea)
		{
			base.DrawElement(workingArea);
			GUI.Box(WorkingArea, "", Style);
			CalcReservedSpace();
		}

		/// <summary>
		/// Calculates the min width of the area with all of its elements
		/// </summary>
		/// <returns></returns>
		public override float CalcWidth()
		{
			if (FixedSize.x > 0f)
				return FixedSize.x;

			float width = Padding.horizontal;
			//width += Margin.horizontal;
			if (Horizontal)
			{
				width += ElementMargin * (ElementCount - 1);
				foreach (VoltageElement element in m_wildFields)
				{
					width += element.CalcWidth();
					width += element.Margin.horizontal;
				}
				foreach (VoltageElement element in m_storedFields)
				{
					width += element.CalcWidth();
					width += element.Margin.horizontal;
				}
			}
			else
			{
				float maxW = 0f;
				foreach (VoltageElement element in m_wildFields)
				{
					maxW = Mathf.Max(maxW, element.CalcWidth() + element.Margin.horizontal);
				}
				foreach (VoltageElement element in m_storedFields)
				{
					maxW = Mathf.Max(maxW, element.CalcWidth() + element.Margin.horizontal);
				}
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

			float height = Padding.vertical;
			//height += Margin.vertical;
			width -= Padding.horizontal;

			if (Horizontal)
			{
				float maxH = 0f;
				width -= ElementMargin * (ElementCount - 1);

				foreach (VoltageElement element in m_wildFields)
				{
					maxH = Mathf.Max(maxH, element.CalcHeight(width) + element.Margin.vertical);
				}
				foreach (VoltageElement element in m_storedFields)
				{
					maxH = Mathf.Max(maxH, element.CalcHeight(width) + element.Margin.vertical);
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
		public override void EventCall(EventType controlEvent, Rect eventPos, Vector2 mousePos)
		{
			base.EventCall(controlEvent, eventPos, mousePos);
			CalcReservedSpace();
		}
	}
}

