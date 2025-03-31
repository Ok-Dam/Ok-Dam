using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voltage
{
	public class VoltageThumb : VoltageElement
	{
		public Object m_objectReference = null;
		private Texture2D preview = null;
		public Vector2 m_thumbSize = Vector2.zero;



		public Object ObjectReference
		{
			get { return m_objectReference; }
			set { m_objectReference = value; }
		}
		public Vector2 ThumbSize
		{
			get { return m_thumbSize; }
			set { m_thumbSize = value; }
		}


		public VoltageThumb(Object objectReference)
		{
			ObjectReference = objectReference;
			preview = AssetPreview.GetAssetPreview(ObjectReference);

		}
		public VoltageThumb(Object objectReference, ElementSettings settings) : this(objectReference)
		{
			ElementSettings = settings;
		}
		public VoltageThumb(Object objectReference, float thumbSize) : this(objectReference)
		{
			m_thumbSize.x = thumbSize;
			m_thumbSize.y = thumbSize;
		}
		public VoltageThumb(Object objectReference, float width, float height) : this(objectReference)
		{
			m_thumbSize.x = width;
			m_thumbSize.y = height;
		}
		/// <summary>
		/// For thumbSize to take effect do not use VoltageElementSettings.FixedSize, or set it to Vector2.zero
		/// </summary>
		/// <param name="objectReference"></param>
		/// <param name="thumbSize"></param>
		/// <param name="settings"></param>
		public VoltageThumb(Object objectReference, float thumbSize, ElementSettings settings) : this(objectReference, thumbSize)
		{
			ElementSettings = settings;
		}
		/// <summary>
		/// For width and height to take effect do not use VoltageElementSettings.FixedSize, or set it to Vector2.zero
		/// </summary>
		/// <param name="objectReference"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="settings"></param>
		public VoltageThumb(Object objectReference, float width, float height, ElementSettings settings) : this(objectReference, width, height)
		{
			ElementSettings = settings;
		}
		/// <summary>
		/// Calculates the width of the element.
		/// </summary>
		/// <returns></returns>
		public override float CalcWidth()
		{
			if (FixedWidth > 0f)
				return FixedWidth;
			else if(ThumbSize.x > 0f)
				return ThumbSize.x;
			else if(preview != null)
				return preview.width;
			return 0f;

		}
		/// <summary>
		/// Calculates the height of the element.
		/// </summary>
		/// <returns></returns>
		public override float CalcHeight(float width)
		{
			if (FixedHeight > 0f)
				return FixedHeight;
			else if(ThumbSize.y > 0f)
				return ThumbSize.y;
			else if(preview != null)
				return preview.height;
			return 0f;
		}

		/// <summary>
		/// Do not use this.
		/// </summary>
		/// <param name="workingArea"></param>
		public override void DrawElement(Rect workingArea)
		{
			base.DrawElement(workingArea);

			Rect r;
			if (FixedWidth > 0f && FixedHeight > 0f)
				r = new Rect(WorkingArea.x, WorkingArea.y, FixedWidth, FixedHeight);

			else if(ThumbSize.x > 0f && ThumbSize.y > 0f)
				r = new Rect(WorkingArea.x, WorkingArea.y, ThumbSize.x, ThumbSize.y);
			else if(preview!=null)
				r = new Rect(WorkingArea.x, WorkingArea.y, preview.width, preview.height);
			else
				r = new Rect(WorkingArea.x, WorkingArea.y, WorkingArea.width, WorkingArea.height);


			//Style.normal.background = preview;
			if (preview != null){
				r.width = r.height * preview.width / preview.height;
				//r.x -= (r.width -r.height);
				EditorGUI.DrawTextureTransparent(r, (Texture)preview);
			}
			else
				EditorGUI.DrawTextureTransparent(r, (Texture)(new Texture2D((int)r.width, (int)r.height)));
		}
	}
}
