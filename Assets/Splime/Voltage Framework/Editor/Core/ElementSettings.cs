using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voltage
{
	public struct ElementSettings
	{
		private int m_weight;
		private Vector2 m_fixedSize;
		private bool m_flex;
		private RectOffset m_margin;

		/// <summary>
		/// Weight for layout distribution.
		/// </summary>
		public int Weight
		{
			get
			{
				if (m_weight < 1)
					m_weight = 1;

				return m_weight;
			}
			set
			{
				m_weight = Mathf.Max(1, value);
			}
		}

		/// <summary>
		/// Fixed size for layout. If higher than 0 overrides all size calculations.
		/// </summary>
		public Vector2 FixedSize
		{
			get
			{
				return m_fixedSize;
			}
			set
			{
				m_fixedSize.x = Mathf.Max(0f, value.x);
				m_fixedSize.y = Mathf.Max(0f, value.y);
			}
		}

		/// <summary>
		/// When on a streaming area, should this element take all the extra space?
		/// </summary>
		public bool Flex
		{
			get
			{
				return m_flex;
			}
			set
			{
				m_flex = value;
			}
		}
		
		/// <summary>
		/// Margin between this element and others.
		/// </summary>
		public RectOffset Margin
		{
			get
			{
				if (m_margin == null)
					m_margin = new RectOffset(0, 0, 0, 0);

				return m_margin;
			}
			set
			{
				if (value != null)
				{
					m_margin.left = Mathf.Max(0, value.left);
					m_margin.right = Mathf.Max(0, value.right);
					m_margin.top = Mathf.Max(0, value.top);
					m_margin.bottom = Mathf.Max(0, value.bottom);
				}
				else
				{
					m_margin = new RectOffset(0,0,0,0);
				}
			}
		}

		public ElementSettings(int weight)
		{
			m_weight = weight;
			m_fixedSize = Vector2.zero;
			m_flex = false;
			m_margin = new RectOffset(0, 0, 0, 0);
		}
		public ElementSettings(Vector2 fixedSize)
		{
			m_weight = 1;
			m_fixedSize = fixedSize;
			m_flex = false;
			m_margin = new RectOffset(0, 0, 0, 0);
		}
		public ElementSettings(bool flex)
		{
			m_weight = 1;
			m_fixedSize = Vector2.zero;
			m_flex = flex;
			m_margin = new RectOffset(0, 0, 0, 0);
		}
		public ElementSettings(bool flex, int weight)
		{
			m_weight = weight;
			m_fixedSize = Vector2.zero;
			m_flex = flex;
			m_margin = new RectOffset(0, 0, 0, 0);
		}
		public ElementSettings(int weight, RectOffset margin) : this(weight)
		{
			if (margin != null)
			{
				m_margin.left = Mathf.Max(0, margin.left);
				m_margin.right = Mathf.Max(0, margin.right);
				m_margin.top = Mathf.Max(0, margin.top);
				m_margin.bottom = Mathf.Max(0, margin.bottom);
			}

		}
		public ElementSettings(Vector2 fixedSize, RectOffset margin) : this(fixedSize)
		{
			if (margin != null)
			{
				m_margin.left = Mathf.Max(0, margin.left);
				m_margin.right = Mathf.Max(0, margin.right);
				m_margin.top = Mathf.Max(0, margin.top);
				m_margin.bottom = Mathf.Max(0, margin.bottom);
			}
		}
		public ElementSettings(bool flex, RectOffset margin) : this(flex)
		{
			if (margin != null)
			{
				m_margin.left = Mathf.Max(0, margin.left);
				m_margin.right = Mathf.Max(0, margin.right);
				m_margin.top = Mathf.Max(0, margin.top);
				m_margin.bottom = Mathf.Max(0, margin.bottom);
			}
		}
		public ElementSettings(bool flex, int weight, RectOffset margin) : this(flex, weight)
		{
			if (margin != null)
			{
				m_margin.left = Mathf.Max(0, margin.left);
				m_margin.right = Mathf.Max(0, margin.right);
				m_margin.top = Mathf.Max(0, margin.top);
				m_margin.bottom = Mathf.Max(0, margin.bottom);
			}
		}
	}
}
