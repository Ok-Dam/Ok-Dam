using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voltage
{
	public struct VoltageListCallbackArgument
	{
		public int[] id;
		public bool firstSelected;
		public bool folder;
		public GUIContent content;
	}
	public delegate void VoltageListCallback(VoltageListCallbackArgument e);

	public class VoltageList : VoltageArea
	{
		private class VoltListItem
		{
			public VoltListItem parent = null;

			public bool isFolder = false;
			public bool expanded = true;
			public bool selected = false;
			public bool renaming = false;
			public bool alreadyFocus = false;
			public string renaimingText = "";

			public GUIContent content = new GUIContent("Element");
			public List<VoltListItem> children = new List<VoltListItem>(0);
			public int[] id = new int[0];
			public Texture icon = null;

			public VoltListItem this[int i]
			{
				get
				{
					return children[i];
				}
			}
			public int ChildrenCount
			{
				get
				{
					int i = 0;
					foreach(VoltListItem item in children)
					{
						i += item.ChildrenCount + 1;
					}
					return i;
				}
			}
			public VoltListItem(GUIContent _content, bool _folder)
			{
				id = new int[0];
				content = _content;
				isFolder = _folder;

			}
			public VoltListItem(GUIContent _content,bool _folder,int[] _id)
			{
				if (_id == null)
					id = new int[] { 0 };
				content = _content;
				isFolder = _folder;
				
			}
			public VoltListItem(GUIContent _content, bool _folder, Texture _icon, int[] _id = null)
			{
				if (_id == null)
					id = new int[] { 0 };

				content = _content;
				isFolder = _folder;
				icon = _icon;
			}
			public VoltListItem AddChild(GUIContent _content, bool _folder)
			{
				//int l = (id != null) ? id.Length + 1,1;

				int[] _id = new int[id.Length + 1];
				for(int i = 0; i < id.Length; i++)
				{
					_id[i] = id[i];
				}
				_id[_id.Length - 1] = children.Count;
				VoltListItem element = new VoltListItem(_content, _folder, _id);
				return AddChild(element);
			}

			public VoltListItem AddChild(GUIContent _content, bool _folder, Texture _icon)
			{
				int[] _id = new int[id.Length + 1];
				for (int i = 0; i < id.Length; i++)
				{
					_id[i] = id[i];
				}
				_id[_id.Length - 1] = children.Count;
				VoltListItem element = new VoltListItem(_content, _folder, _icon, _id);
				return AddChild(element);
			}

			private VoltListItem AddChild(VoltListItem element)
			{
				int[] _id = new int[id.Length + 1];
				for (int i = 0; i < id.Length; i++)
				{
					_id[i] = id[i];
				}
				_id[_id.Length - 1] = children.Count;

				element.parent = this;
				element.id = _id;
				children.Add(element);
				return element;
			}

			public void DeleteChild(VoltListItem child)
			{
				children.Remove(child);

				int a = 0;
				foreach (VoltListItem item in children)
				{
					int[] _id = new int[id.Length + 1];
					for (int i = 0; i < id.Length; i++)
					{
						_id[i] = id[i];
					}
					_id[_id.Length - 1] = a;

					item.id = _id;
					a++;
				}
			}
		}


		#region EVENTS
		public event VoltageListCallback OnItemSelected;

		public event VoltageListCallback OnDuplicateItem;
		public event VoltageListCallback OnDeleteItem;
		public event VoltageListCallback OnNewItem;
		//public event VoltageListCallback OnRenameItem;

		#endregion

		#region Variables
		public bool expanded = true;
		public bool m_multiSelect = false;

		VoltListItem rootFolder;

		public GUIStyle expandedStyle;
		public GUIStyle compactedStyle;

		public GUIStyle idleElementStyle;
		public GUIStyle selectedElementStyle;
		private List<VoltListItem> selection = new List<VoltListItem>(0);

		public Texture m_folderIcon = null;
		public Texture m_itemIcon = null;

		public float elementHeight = 16f;

		public bool rearange;

		private Vector2 scroll = Vector2.zero;

		private bool expandEvent = false;
		private List<int> eventIndex = null;

		private bool dragging = false;
		private bool shiftMod = false;

		private bool showMenu = false;
		private bool menuFirstPass = false;

		private VoltListItem renamingItem;
		private bool renameFirstPass = false;
		#endregion
		public int Count
		{
			get
			{
				return rootFolder.ChildrenCount + 1;
			}
		}
		#region Constructors
		private void ClasicStyles()
		{
			Style = ValidateStyle("Box", new GUIStyle("box"));
			idleElementStyle = ValidateStyle("ListItemIdle", "Label");
			selectedElementStyle = ValidateStyle("ListItemSelected", "LODSliderRangeSelected");
			expandedStyle = ValidateStyle("Foldout On", "IN foldout act on");
			compactedStyle = ValidateStyle("Foldout Off", "IN foldout act");

			m_folderIcon = EditorGUIUtility.IconContent("Folder Icon").image;
			m_itemIcon = null;
		}
		public VoltageList(bool multiSelect)
		{
			rootFolder = new VoltListItem(new GUIContent(""), true);

			m_multiSelect = multiSelect;

			ClasicStyles();
		}
		public VoltageList(bool multiSelect, ElementSettings settings) : this(multiSelect)
		{
			ElementSettings = settings;
		}

		public VoltageList(bool multiSelect, GUIStyle style) : this(multiSelect)
		{
			Style = ValidateStyle(style, "Box", "box");
		}
		public VoltageList(bool multiSelect, ElementSettings settings, GUIStyle style) : this(multiSelect, settings)
		{
			Style = ValidateStyle(style, "Box", "box");
		}
		public VoltageList(bool multiSelect, GUIStyle style, Texture folderIcon, Texture itemIcon) : this(multiSelect, style)
		{
			m_folderIcon = folderIcon;
			m_itemIcon = itemIcon;
		}
		public VoltageList(bool multiSelect, ElementSettings settings, GUIStyle style, Texture folderIcon, Texture itemIcon) : this(multiSelect, settings, style)
		{
			m_folderIcon = folderIcon;
			m_itemIcon = itemIcon;
		}
		#endregion

		#region Private Controls
		private VoltListItem GetItem(int[] index)
		{
			VoltListItem currentFolder = rootFolder;

			for (int i = 0; i < index.Length; i++)
			{
				if (i == index.Length - 1)
					return currentFolder[index[i]];
				else if (currentFolder[index[i]].isFolder)
					currentFolder = currentFolder[index[i]];
				else
					return null;
			}
			return null;
		}

		private void DeselectItem(VoltListItem item)
		{
			item.selected = false;
			selection.Remove(item);
		}

		private void DeselectAll()
		{
			foreach (VoltListItem element in selection)
			{
				element.selected = false;
			}
			selection.Clear();
		}

		#endregion

		#region Public Controls
		/// <summary>
		/// Adds an item to the folder with index folder. Uses the list itemIcon.
		/// </summary>
		/// <param name="content"></param>
		public int[] AddItem(int[] folder, GUIContent content)
		{
			return AddItem(folder,content, m_itemIcon);
		}

		/// <summary>
		/// Adds an item to the folder with index folder.
		/// </summary>
		/// <param name="folder"></param>
		/// <param name="content"></param>
		/// <param name="icon"></param>
		/// <returns></returns>
		public int[] AddItem(int[] folder, GUIContent content, Texture icon)
		{
			VoltListItem currentFolder = GetItem(folder);
			if (currentFolder.isFolder)
			{
				return currentFolder.AddChild(content, false, icon).id;
			}
			return null;
		}

		/// <summary>
		/// Adds an item to the root folder. Uses the list folderIcon.
		/// </summary>
		/// <param name="content"></param>
		public int[] AddItem(GUIContent content)
		{
			return rootFolder.AddChild(content, false, m_itemIcon).id;
		}

		/// <summary>
		/// Adds an item to the root folder.
		/// </summary>
		/// <param name="content"></param>
		/// <param name="icon"></param>
		public int[] AddItem(GUIContent content, Texture icon)
		{
			VoltListItem currentFolder = rootFolder;

			return currentFolder.AddChild(content, false, icon).id;
		}

		/// <summary>
		/// Adds an item to the folder with index folder. Uses the list itemIcon.
		/// </summary>
		/// <param name="content"></param>
		public void DeleteItem(int[] item)
		{
			VoltListItem target = GetItem(item);
			target.parent.DeleteChild(target);
		}

		public void RenameItem(int[] itemIndex, string name)
		{
			VoltListItem item = GetItem(itemIndex);
			item.content.text = name;
		}

		/// <summary>
		/// Adds a folder to the specified folder. If index is not a valid folder returns false.
		/// </summary>
		/// <param name="folder"></param>
		/// <param name="content"></param>
		/// <returns></returns>
		public int[] AddFolder(int[] folder, GUIContent content)
		{
			return AddFolder(folder, content, m_folderIcon);
		}
		/// <summary>
		/// Adds a folder to the specified folder. If index is not a valid folder returns false.
		/// </summary>
		/// <param name="folder"></param>
		/// <param name="content"></param>
		/// <returns></returns>
		public int[] AddFolder(int[] folder, GUIContent content, Texture icon)
		{
			VoltListItem currentFolder = GetItem(folder);
			if(currentFolder != null)
			{
				if (currentFolder.isFolder)
				{
					return currentFolder.AddChild(content, true, icon).id;
				}
			}
			Debug.LogError("Folder index is invalid");
			return null;
		}

		/// <summary>
		/// Adds a folder on the root folder.
		/// </summary>
		/// <param name="content"></param>
		public int[] AddFolder(GUIContent content)
		{
			return AddFolder(content, m_folderIcon);
		}

		/// <summary>
		/// Adds a folder on the root folder.
		/// </summary>
		/// <param name="content"></param>
		/// <param name="icon"></param>
		public int[] AddFolder(GUIContent content, Texture icon)
		{
			return rootFolder.AddChild(content, true, icon).id;
		}
		#endregion

		#region Base Methods
		public override float CalcHeight(float width)
		{
			float h = CalcElementHeight(rootFolder, width);
			//h += elementHeight;
			return h;
		}
		private float CalcElementHeight(VoltListItem element,float width, bool root = true)
		{
			float h = 0f;
			if (element.isFolder)
			{
				if (!root)
					h += elementHeight;
				if (element.expanded)
					foreach (VoltListItem child in element.children)
					{
						h += CalcElementHeight(child,width, false);
					}
			}
			else
			{
				h = elementHeight;
			}
			return h;
		}
		public override float CalcWidth()
		{
			float w = CalcItemWidth(rootFolder,0f);
			//w += 32f;
			return w;
		}
		private float CalcItemWidth(VoltListItem element, float tab)
		{
			float w = 0f;
			if (element.isFolder && element.expanded)
			{
				foreach (VoltListItem child in element.children)
				{
					float eW = CalcItemWidth(child, tab + elementHeight);
					w = (w < eW) ? eW : w;
				}
			}
			else
			{
				w = idleElementStyle.CalcSize(element.content).x + elementHeight + ((element.icon!=null) ? elementHeight: 0f);
			}
			return w;
		}
		#endregion

		#region OnGUI
		/// <summary>
		/// Do not use this.
		/// </summary>
		/// <param name="workingArea"></param>
		public override void DrawElement(Rect workingArea)
		{
			base.DrawElement(workingArea);

			float barH = 15f;

			float height = CalcHeight(PaddedArea.width);
			float width = CalcWidth();

			Rect currentPos = new Rect();
			


			currentPos = new Rect(0f, 0f, Mathf.Max(PaddedArea.width, width /*- Padding.horizontal*/), Mathf.Max(PaddedArea.height, height /*- Padding.vertical*/));
			
			Rect scrollArea = new Rect(0f, 0f, currentPos.width /*+ Padding.horizontal*/, currentPos.height /*+ Padding.vertical*/);
			
			if (scrollArea.width > WorkingArea.width)
			{
				if (Horizontal)
				{
					currentPos.height = Mathf.Max(currentPos.height - barH, height);// - Padding.vertical);
				}

				if (height < WorkingArea.height - barH)
				{
					scrollArea.height = Mathf.Clamp(scrollArea.height - barH, height, currentPos.height /*+ Padding.vertical*/ - barH);
				}
				else
				{
					scrollArea.height = Mathf.Max(height, currentPos.height /*+ Padding.vertical*/ - barH);
				}
			}


			if (scrollArea.height > WorkingArea.height)
			{
				if (!Horizontal)
				{
					currentPos.width = Mathf.Max(currentPos.width - barH, width);// - Padding.horizontal);
				}

				if (width < WorkingArea.width - barH)
				{
					scrollArea.width = Mathf.Clamp(scrollArea.width - barH, width, currentPos.width /*+ Padding.horizontal*/ - barH);
				}
				else
				{
					scrollArea.width = Mathf.Max(width, currentPos.width /*+ Padding.horizontal*/ - barH);
				}
			}

			scroll = GUI.BeginScrollView(PaddedArea, scroll, scrollArea, false, false);

			if (showMenu)
			{
				if (menuFirstPass)
				{
					GenericMenu menu = new GenericMenu();

					
					if (OnNewItem != null)
						menu.AddItem(new GUIContent("New"), false, NewSelectedItem);
					//if (OnRenameItem != null)
					//	menu.AddItem(new GUIContent("Rename"), false, RenameSelectedItem);
					if (OnDuplicateItem != null)
						menu.AddItem(new GUIContent("Duplicate"), false, DuplicateSelectedItem);
					if (OnDeleteItem != null)
						menu.AddItem(new GUIContent("Delete"), false, DeleteSelectedItem);
					

					if (menu.GetItemCount() > 0)
						menu.ShowAsContext();
					showMenu = false;
					menuFirstPass = false;
				}
				else
				{
					menuFirstPass = true;

				}

			}

			PrintFolder(ref currentPos, rootFolder.children);

			GUI.EndScrollView();
		}

		private void PrintFolder(ref Rect currentPos, List<VoltListItem> list, float tab = 0f)
		{
			//Rect tabPos = new Rect(currentPos.x + tab, currentPos.y, currentPos.width, elementHeight);

			for (int e = 0; e < list.Count; e++)
			{
				
				Rect foldoutRect = new Rect(currentPos.x + tab, currentPos.y, elementHeight, elementHeight);

				Rect iconRect = new Rect(foldoutRect.x + foldoutRect.width, currentPos.y, (list[e].icon!=null) ? elementHeight : 0f, elementHeight);
				float contentPadding = iconRect.x + iconRect.width;
				Rect contentRect = new Rect(currentPos.x, currentPos.y, currentPos.width, elementHeight);


				GUIStyle selElStyle = new GUIStyle(selectedElementStyle);
				GUIStyle idleElStyle = new GUIStyle(idleElementStyle);
				selElStyle.padding.left += (int)contentPadding;
				idleElStyle.padding.left += (int)contentPadding;

				if (list[e].isFolder)
				{

					
					GUI.Label(contentRect, list[e].content, (list[e].selected) ? selElStyle : idleElStyle);
					if (GUI.Button(foldoutRect, "",(list[e].expanded?expandedStyle:compactedStyle)))
					{
						list[e].expanded = !list[e].expanded;
					}
					if (list[e].icon != null)
						GUI.DrawTexture(iconRect, list[e].icon);

					currentPos.y += elementHeight;

					if (list[e].expanded)
						PrintFolder(ref currentPos, list[e].children, tab+elementHeight);
				}
				else
				{


					if (list[e].renaming && !renameFirstPass)
					{
						//string lname = e.ToString();
						Rect area = new Rect(currentPos.x + contentPadding, currentPos.y, currentPos.width - contentPadding, elementHeight);
						//GUI.SetNextControlName(lname);
						list[e].renaimingText = EditorGUI.TextField(area, "adad");
						
						if (!renameFirstPass)
						{
							//if (!list[e].alreadyFocus)
							//{
							//	EditorGUI.FocusTextInControl(lname);
							//	list[e].alreadyFocus = true;
							//}

							//if (GUI.GetNameOfFocusedControl() != lname || (GUI.GetNameOfFocusedControl() == lname && !EditorGUIUtility.editingTextField))
							//{

							//	list[e].alreadyFocus = false;
							//	list[e].renaming = false;
							//	list[e].content.text = list[e].renaimingText;
							//	VoltageListCallbackArgument arguments = new VoltageListCallbackArgument()
							//	{
							//		firstSelected = true,
							//		id = list[e].id,
							//		folder = list[e].folder,
							//		content = new GUIContent((string)list[e].content.text.Clone()),
							//	};
								
							//	OnRenameItem(arguments);
							//}
						}
						else
						{
							//EditorGUI.TextField(area, list[e].renaimingText);
							
							Debug.Log(Event.current.isMouse);
							renameFirstPass = false;
						}
						
					}
					else
					{
						GUI.Label(contentRect, list[e].content, (list[e].selected) ? selElStyle : idleElStyle);
						renameFirstPass = false;
					}

					currentPos.y += elementHeight;
					if (list[e].icon != null)
						GUI.DrawTexture(iconRect, list[e].icon);
				}
			}
		}
		#endregion

		#region Event Handling
		public override void EventCall(EventType controlEvent, Rect eventPos, Vector2 mousePos)
		{
			base.EventCall(controlEvent, eventPos, mousePos);

			float barH = 15f;

			Rect currentPos = new Rect(PaddedArea.x, PaddedArea.y, CalcWidth(), CalcHeight(eventPos.width));
			Rect scrollArea = new Rect(PaddedArea.x, PaddedArea.y, PaddedArea.width, PaddedArea.height);
			if (currentPos.width > scrollArea.width)
			{
				scrollArea.height -= barH;

				if (currentPos.height > scrollArea.height)
				{
					scrollArea.width -= barH;
				}
			}
			else if (currentPos.height > scrollArea.height)
			{
				scrollArea.width -= barH;
				if (currentPos.width > scrollArea.width)
				{
					scrollArea.height -= barH;
				}
			}

			if (scrollArea.Contains(mousePos))
			{
				if (this.EventMap.ContainsKey(controlEvent))
				{
					this.EventMap[controlEvent].Invoke();
				}
			}


		}
		protected override void OnMouseDown(MouseButton button, Vector2 position)
		{
			Rect currentPos = new Rect(WorkingArea.x, WorkingArea.y, WorkingArea.width, WorkingArea.height);

			eventIndex = CheckEvent(ref currentPos, rootFolder.children);
			int eventTarget = eventIndex[eventIndex.Count - 1];

			if (eventTarget != -1)
			{

				VoltListItem target = GetItem(eventIndex.ToArray());
				if (expandEvent)
				{
					//Debug.Log("Expand: " + eventIndex.Count.ToString() + " - " + eventIndex[eventIndex.Count - 1].ToString());
					//target.expanded = !target.expanded;
					//expandEvent = false;
				}
				else
				{
					if (!target.renaming)
					{
						if (m_multiSelect && shiftMod)
						{

						}
						else
						{
							SelectItemEvent(target);
							



							

						}
						
						Event.current.Use();
					}
				}
			}
		}

		
		protected override void OnMouseDrag(MouseButton button, Vector2 position, Vector2 delta)
		{
			//Debug.Log("Dragging");
			//if (m_dragged)
			//{
			//	if (horizontal)
			//		division = (position.x - workingArea.x) / workingArea.width;
			//	else
			//		division = (position.y - workingArea.y) / workingArea.height;
			//}
			//Event.current.Use();
		}

		protected override void OnMouseUp(MouseButton button, Vector2 position)
		{
			if (!dragging)
			{
				dragging = false;

				Rect currentPos = new Rect(WorkingArea.x, WorkingArea.y, WorkingArea.width, WorkingArea.height);

				eventIndex = CheckEvent(ref currentPos, rootFolder.children);
				int eventTarget = eventIndex[eventIndex.Count - 1];

				if (eventTarget != -1)
				{

					if (expandEvent)
					{
						//Debug.Log("Expand: " + eventIndex.Count.ToString() + " - " + eventIndex[eventIndex.Count - 1].ToString());
						//list[eventTarget].expanded = !list[eventTarget].expanded;
					}
					else
					{
						//Debug.Log("Mouse Up: " + eventIndex.Count.ToString() + " - " + eventIndex[eventIndex.Count - 1].ToString());
						if (button == MouseButton.Right)
						{
							showMenu = true;
						}
						else
							Event.current.Use();
					}
					
				}
			}
			
		}
		private List<int> CheckEvent(ref Rect currentPos, List<VoltListItem> list, List<int> index = null)
		{
			//expandEvent = false;


			if (index == null)
				index = new List<int>(0);

			index.Add(-1);

			int indexDepth = index.Count - 1;

			//Rect tabPos = new Rect(currentPos.x + tab, currentPos.y, currentPos.width, elementHeight);
			Vector2 eventPos = Event.current.mousePosition + scroll;
			for (int e = 0; e < list.Count; e++)
			{
				index[indexDepth] = e;
				Rect fullRect = new Rect(currentPos.x, currentPos.y, currentPos.width, elementHeight);

				Rect buttonRect = new Rect(fullRect.x + indexDepth * elementHeight, fullRect.y, (list[e].icon != null) ? elementHeight : 0f, elementHeight);


				if (list[e].isFolder)
				{
					if (buttonRect.Contains(eventPos))
					{
						expandEvent = true;
						//Debug.Log("Expand");
						break;
					}
					else if (fullRect.Contains(eventPos))
					{
						expandEvent = false;
						//Debug.Log("Expand Free");
						break;
					}

					currentPos.y += elementHeight;

					if (list[e].expanded)
					{
						List<int> bufferindex = CheckEvent(ref currentPos, list[e].children, index);
						if (bufferindex[bufferindex.Count - 1] != -1)
						{
							break;
						}
						index.RemoveAt(index.Count - 1);

					}
				}
				else
				{
					if (fullRect.Contains(eventPos))
					{
						expandEvent = false;
						break;
					}
					currentPos.y += elementHeight;
				}


				index[indexDepth] = -1;
			}
			return index;
		}
		#endregion

		

		#region ContextMenu
		public void RenameSelectedItem()
		{
			//if (OnRenameItem != null)
			//{
			//	if (selection.Count > 0)
			//	{
			//		if(renamingItem!=null)
			//		{
			//			renamingItem.renaming = false;
			//		}
			//		VoltListItem item = selection[0];
			//		renamingItem = item;

			//		item.renaimingText = item.content.text;
			//		item.renaming = true;
			//		item.alreadyFocus = false;

			//		renameFirstPass = true;
			//	}

			//}
		}
		public void DuplicateSelectedItem()
		{
			if (OnDuplicateItem!=null)
			{
				List<VoltageListCallbackArgument> arguments = new List<VoltageListCallbackArgument>(0);
				foreach (VoltListItem item in selection)
				{
					VoltageListCallbackArgument e = new VoltageListCallbackArgument()
					{
						firstSelected = true,
						id = item.id,
						folder = item.isFolder,
						content = new GUIContent((string)item.content.text.Clone()),
					};
					arguments.Add(e);
				}
				foreach(VoltageListCallbackArgument argument in arguments)
				{
					OnDuplicateItem(argument);
				}
			}
			
		}
		public void DeleteSelectedItem()
		{
			if (OnDeleteItem != null)
			{
				bool first = true;
				List<VoltageListCallbackArgument> arguments = new List<VoltageListCallbackArgument>(0);

				foreach (VoltListItem item in selection)
				{
					VoltageListCallbackArgument e = new VoltageListCallbackArgument()
					{
						firstSelected = first,
						id = item.id,
						folder = item.isFolder,
						content = new GUIContent((string)item.content.text.Clone()),
					};
					arguments.Add(e);
					first = false;
				}
				foreach (VoltageListCallbackArgument argument in arguments)
				{
					OnDeleteItem(argument);
				}
			}
		}
		public void NewSelectedItem()
		{
			if (OnNewItem != null)
			{
				if (selection.Count > 0)
				{
					VoltListItem item = GetItem(selection[0].id);
					VoltageListCallbackArgument e = new VoltageListCallbackArgument()
					{
						firstSelected = true,
						id = item.id,
						folder = item.isFolder,
						content = new GUIContent((string)item.content.text.Clone()),
					};
					OnNewItem(e);
				}
			}
		}

		#endregion

		private void SelectItemEvent(VoltListItem item, bool multiple = false)
		{
			SelectItem(item, multiple);

			if (OnItemSelected != null)
			{
				VoltageListCallbackArgument e = new VoltageListCallbackArgument()
				{
					firstSelected = selection.Count==1,
					id = item.id,
					folder = item.isFolder,
					content = new GUIContent((string)item.content.text.Clone()),
				};

				OnItemSelected(e);
			}
		}

		private void SelectItem(VoltListItem item, bool multiple = false)
		{
			if (!multiple)
			{
				DeselectAll();
			}
			item.selected = true;
			selection.Add(item);
		}

		public void SelectItem(int[] itemIndex, bool multiple = false)
		{
			SelectItemEvent(GetItem(itemIndex), multiple);
			
		}

		

	}
}