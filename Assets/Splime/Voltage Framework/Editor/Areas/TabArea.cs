using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voltage
{
	public class TabArea : VoltageArea
	{
		#region PROPERTIES
		private class Tab
		{

			private string m_name;
			public string Name
			{
				get { return m_name; }
				set { m_name = value; }
			}

			Texture2D m_icon = null;
			public Texture2D Icon
			{
				get { return m_icon; }
				set { m_icon = value; }
			}

			private WeightArea m_area = new WeightArea();
			public WeightArea Area
			{
				get { return m_area; }
				set { m_area = value; }
			}

			public Tab(string name, Texture2D icon)
			{
				Name = name;
				Icon = icon;
			}
		}
		private List<Tab> tabs = new List<Tab>(0);

		private int currentGUI = 0;
		private int current = 0;

		private GUIStyle m_styleLeft;
		private GUIStyle m_styleLeftOn;
		private GUIStyle m_styleCenter;
		private GUIStyle m_styleCenterOn;
		private GUIStyle m_styleRight;
		private GUIStyle m_styleRightOn;

		public GUIStyle StyleLeft
		{
			get { return m_styleLeft; }
			set { m_styleLeft = value; }
		}

		public GUIStyle StyleCenter
		{
			get { return m_styleCenter; }
			set { m_styleCenter = value; }
		}

		public GUIStyle StyleRight
		{
			get { return m_styleRight; }
			set { m_styleRight = value; }
		}
		public GUIStyle StyleLeftOn
		{
			get { return m_styleLeftOn; }
			set { m_styleLeftOn = value; }
		}

		public GUIStyle StyleCenterOn
		{
			get { return m_styleCenterOn; }
			set { m_styleCenterOn = value; }
		}

		public GUIStyle StyleRightOn
		{
			get { return m_styleRightOn; }
			set { m_styleRightOn = value; }
		}
		#endregion

		private void ClassicStyles()
		{
			StyleLeft = ValidateStyle("TabLeft", "TL tab left");
			StyleLeftOn = ValidateStyle("TabLeft On", "TL tab left");
			StyleCenter = ValidateStyle("TabCenter", "TL tab mid");
			StyleCenterOn = ValidateStyle("TabCenter On", "TL tab mid");
			StyleRight = ValidateStyle("TabRight", "TL tab right");
			StyleRightOn = ValidateStyle("TabRight On", "TL tab right");
		}

		#region	CONSTRUCTORS
		public TabArea()
		{
			ClassicStyles();
		}
		public TabArea(ElementSettings elementSettings) : this()
		{
			ElementSettings = elementSettings;
		}
		public TabArea(AreaSettings areaSettings) : this()
		{
			AreaSettings = areaSettings;
			Horizontal = true;
		}
		public TabArea(ElementSettings elementSettings, AreaSettings areaSettings) : this()
		{
			ElementSettings = elementSettings;
			AreaSettings = areaSettings;
			Horizontal = true;
		}

		public TabArea(GUIStyle style) : this()
		{
			Style = ValidateStyle(style, GUIStyle.none);
		}
		public TabArea(ElementSettings elementSettings, GUIStyle style) : this(style)
		{
			ElementSettings = elementSettings;
		}
		public TabArea(AreaSettings areaSettings, GUIStyle style) : this(style)
		{
			AreaSettings = areaSettings;
			Horizontal = true;
		}
		public TabArea(ElementSettings elementSettings, AreaSettings areaSettings, GUIStyle style) : this(style)
		{
			ElementSettings = elementSettings;
			AreaSettings = areaSettings;
			Horizontal = true;
		}
		#endregion

		#region AREA METHODS
		/// <summary>
		/// Do not use this. Adds an element that will be erased after drawn.
		/// </summary>
		/// <param name="element"></param>
		public override void AddWildElement(VoltageElement element)
		{
			tabs[currentGUI].Area.AddWildElement(element);

		}
		/// <summary>
		/// Adds a permanent element to this area. Can't be deleted.
		/// </summary>
		/// <param name="element"></param>
		public override void AddStoredElement(VoltageElement element)
		{
			tabs[currentGUI].Area.AddStoredElement(element);
		}
		
		/// <summary>
		/// Calculates the min width of the area with all of its elements.
		/// </summary>
		/// <returns></returns>
		public override float CalcWidth()
		{
			int count = tabs.Count;
			float width = Horizontal ? 18f * count: 18f ;
			float areaWidth = 0f;

			for (int i = 0; i < count; i++)
			{
				if (!Horizontal)
				{
					if (i == 0)
						width = Mathf.Max(width, (current == i ? StyleLeftOn : StyleLeft).CalcSize(new GUIContent("NN")).x);
					else if (i == count - 1)
						width = Mathf.Max(width, (current == i ? StyleRightOn : StyleRight).CalcSize(new GUIContent("NN")).x);
					else
						width = Mathf.Max(width, (current == i ? StyleCenterOn : StyleCenter).CalcSize(new GUIContent("NN")).x);
				}

				areaWidth = Mathf.Max(areaWidth, tabs[i].Area.CalcWidth());
			}

			return !Horizontal ? width + areaWidth + Padding.vertical : Mathf.Max(width, areaWidth) + Padding.vertical;
		}
		/// <summary>
		/// Calculates the min height of the area with all of its elements.
		/// </summary>
		/// <returns></returns>
		public override float CalcHeight(float width)
		{

			int count = tabs.Count;
			float height = Horizontal ? 18f : 18f * count;
			float areaHeight = 0f;

			for (int i = 0; i < count; i++)
			{
				if (Horizontal)
				{
					if (i == 0)
						height = Mathf.Max(height, (current == i ? StyleLeftOn : StyleLeft).CalcSize(new GUIContent(tabs[i].Name)).y);
					else if (i == count - 1)
						height = Mathf.Max(height, (current == i ? StyleRightOn : StyleRight).CalcSize(new GUIContent(tabs[i].Name)).y);
					else
						height = Mathf.Max(height, (current == i ? StyleCenterOn : StyleCenter).CalcSize(new GUIContent(tabs[i].Name)).y);
				}

				areaHeight = Mathf.Max(areaHeight, tabs[i].Area.CalcHeight(width));
			}

			return Horizontal ? height + areaHeight + Padding.vertical : Mathf.Max(height, areaHeight) + Padding.vertical;
		}

		/// <summary>
		/// Do not use this. Wipes all wild elements and resets this area. Called after each Draw call.
		/// </summary>
		public override void CleanWild()
		{
			currentGUI = 0;
			foreach (Tab tab in tabs)
			{
				tab.Area.CleanWild();
			}
		}
		#endregion 

		/// <summary>
		/// VoltageInit: Adds a new tab.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="icon"></param>
		public void AddTab(string name, Texture2D icon)
		{
			Tab tab = new Tab(name, icon);
			tab.Area.Style = Style;
			tabs.Add(tab);
		}
		/// <summary>
		/// VoltageInit: Adds a new tab.
		/// </summary>
		/// <param name="name"></param>
		public void AddTab(string name)
		{
			Tab tab = new Tab(name, null);
			tab.Area.Style = Style;
			tabs.Add(tab);
		}

		/// <summary>
		/// VoltageGUI/Init: Makes the next tab active for adding elements.
		/// </summary>
		public void NextTab()
		{
			currentGUI = Mathf.Min(currentGUI + 1, tabs.Count-1);
		}


		#region DRAW
		protected Rect GetHeaderSize(ref Rect currentPos, out Rect[] tabsHeaders)
		{
			int count = tabs.Count;

			tabsHeaders = new Rect[count];

			float tabWidth = currentPos.width / count;
			float tabHeight = currentPos.height / count;


			//CALC FINAL RECT
			Rect finalRect = new Rect(0,0, 18f, 18f);
			for (int i = 0; i < count; i++)
			{
				if (Horizontal)
				{
					if (i == 0)
						finalRect.height = Mathf.Max(finalRect.height, (current == i ? StyleLeftOn:StyleLeft).CalcSize(new GUIContent(tabs[i].Name)).y);
					else if (i == count - 1)
						finalRect.height = Mathf.Max(finalRect.height, (current == i ? StyleRightOn : StyleRight).CalcSize(new GUIContent(tabs[i].Name)).y);
					else
						finalRect.height = Mathf.Max(finalRect.height, (current == i ? StyleCenterOn : StyleCenter).CalcSize(new GUIContent(tabs[i].Name)).y);
				}
				else
				{
					finalRect.width = 18f;
					break;
				}
			}

			//SET TABS SIZES
			for (int i = 0; i< count; i++)
			{
				if (Horizontal)
				{
					tabsHeaders[i] = new Rect(currentPos.x, currentPos.y, tabWidth, finalRect.height);

					currentPos.x += tabsHeaders[i].width;
				}
				else
				{
					tabsHeaders[i] = new Rect(currentPos.x, currentPos.y, finalRect.width, tabHeight);

					currentPos.y += tabsHeaders[i].height;
				}

			}

			if (Horizontal)
			{
				currentPos.x = 0;
				currentPos.y += finalRect.height;
			}
			else
			{
				currentPos.y = 0;
				currentPos.x += finalRect.width;
			}

			return finalRect;
		}
		protected Rect GetAreaSize(ref Rect currentPos, Rect headerPos)
		{
			Rect r;
			currentPos.x += Padding.left;
			currentPos.y += Padding.top;
			currentPos.height -= Padding.vertical;
			currentPos.width -= Padding.horizontal;

			if (Horizontal)
			{
				r = new Rect(currentPos.x, currentPos.y, currentPos.width , currentPos.height - headerPos.height );

				currentPos.x += r.width;
			}
			else
			{
				r = new Rect(currentPos.x , currentPos.y, currentPos.width - headerPos.width, currentPos.height );

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

			GUI.BeginGroup(WorkingArea);
			Rect currentPos = new Rect(0f, 0f, WorkingArea.width, WorkingArea.height);

			Rect[] tabHeaders;
			Rect headerPos = GetHeaderSize(ref currentPos, out tabHeaders);

			GUIStyle tabStyle = StyleLeft;
			for (int i = 0; i < tabs.Count; i++)
			{
				
				if (Horizontal)
				{
					if (i == 0)
						tabStyle = current == i ? StyleLeftOn : StyleLeft;
					else if(i==tabs.Count-1)
						tabStyle = current == i ? StyleRightOn : StyleRight;
					else
						tabStyle = current == i ? StyleCenterOn : StyleCenter;

					if (GUI.Button(tabHeaders[i], tabs[i].Name, tabStyle))
					{
						current = i;
					}

				}
				else
				{
					

					if (GUI.Button(tabHeaders[i], ""))
					{
						current = i;
					}
				}
				Rect icon;
				if (tabs[i].Icon != null)
				{
					icon = new Rect(tabHeaders[i].x + 1, tabHeaders[i].y + (tabHeaders[i].height - 16f) / 2f, 16f, 16f);
					GUI.Box(icon, tabs[i].Icon);
				}
			}

			Rect areaPos = GetAreaSize(ref currentPos, headerPos);
			tabs[current].Area.DrawElement(areaPos);

			GUI.EndGroup();

		}
		#endregion

		/// <summary>
		/// Do not use this.
		/// </summary>
		/// <param name="controlEvent"></param>
		/// <param name="_eventPos"></param>
		public override void EventCall(EventType controlEvent, Rect eventPos, Vector2 mousePos)
		{
			base.EventCall(controlEvent, eventPos, mousePos);

			Rect currentPos = new Rect(WorkingArea.x, WorkingArea.y, WorkingArea.width, WorkingArea.height);

			Rect[] tabHeaders;
			Rect headerPos = GetHeaderSize(ref currentPos, out tabHeaders);

			Rect areaPos = GetAreaSize(ref currentPos, headerPos);

			if (areaPos.Contains(mousePos))
			{
				tabs[current].Area.EventCall(controlEvent, areaPos, mousePos);
			}
		}
	}
}

