using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voltage
{
	public struct AreaSettings
	{
		private bool m_horizontal;
		private VoltageElementAlignment m_elementAlignment;
		private float m_elementMargin;
		private RectOffset m_padding;

		/// <summary>
		/// Is the orientation for drawing elements horizontal.
		/// </summary>
		public bool Horizontal
		{
			get
			{
				return m_horizontal;
			}
			set
			{
				m_horizontal = value;
			}
		}

		/// <summary>
		/// How to align elements when there is extra space.
		/// </summary>
		public VoltageElementAlignment ElementAlignment
		{
			get
			{
				return m_elementAlignment;
			}
			set
			{
				m_elementAlignment = value;
			}
		}
		
		/// <summary>
		/// Space between each element.
		/// </summary>
		public float ElementMargin
		{
			get
			{
				return m_elementMargin;
			}
			set
			{
				m_elementMargin = value;
			}
		}
		/// <summary>
		/// Padding to apply when drawing elements inside an area
		/// </summary>
		public RectOffset Padding
		{
			get
			{
				if (m_padding == null)
					m_padding = new RectOffset(0, 0, 0, 0);

				return m_padding;
			}
			set
			{
				if (value != null)
				{
					m_padding.left = Mathf.Max(0, value.left);
					m_padding.right = Mathf.Max(0, value.right);
					m_padding.top = Mathf.Max(0, value.top);
					m_padding.bottom = Mathf.Max(0, value.bottom);
				}
				else
				{
					m_padding = new RectOffset();
				}
			}
		}


		public AreaSettings(bool horizontal)
		{
			m_horizontal = horizontal;
			m_elementAlignment = VoltageElementAlignment.TopLeft;
			m_elementMargin = 0f;
			m_padding = new RectOffset(0, 0, 0, 0);
		}
		public AreaSettings(bool horizontal, RectOffset padding) : this(horizontal)
		{
			if (padding != null)
			{
				m_padding.left = Mathf.Max(0, padding.left);
				m_padding.right = Mathf.Max(0, padding.right);
				m_padding.top = Mathf.Max(0, padding.top);
				m_padding.bottom = Mathf.Max(0, padding.bottom);
			}
		}
		public AreaSettings(bool horizontal, RectOffset padding, float elementMargin) : this(horizontal, padding)
		{
			m_elementMargin = elementMargin;
		}
		public AreaSettings(bool horizontal, RectOffset padding, float elementMargin, VoltageElementAlignment alignment) : this(horizontal, padding, elementMargin)
		{
			m_elementAlignment = alignment;
		}

		public AreaSettings(bool horizontal, float elementMargin) : this(horizontal)
		{
			m_elementMargin = elementMargin;
		}
		public AreaSettings(bool horizontal, float elementMargin, VoltageElementAlignment alignment) : this(horizontal, elementMargin)
		{
			m_elementAlignment = alignment;
		}

		public AreaSettings(bool horizontal, VoltageElementAlignment alignment) : this(horizontal)
		{
			m_elementAlignment = alignment;
		}
		
	}
}
