using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Voltage;
using System.Reflection;

public class StyleEditorWindow : VoltageWindow
{
	public enum StylePreviewType{
		Button,
		Textbox,
		Label
	}
	VoltageEnumPopup previewType;

	VoltageLink guideButton;

	SplitArea splitMain;
	GUIStyle currentStyle = null;
	int currentBundle = 0;
	List<string> bundles;

	List<int[]> selected = new List<int[]>(0);

	VoltageObject font;

	VoltageText fontName;
	VoltageNumeric fontSize;
	VoltageEnumPopup fontStyle;
	VoltageEnumPopup alignment;
	VoltageEnumPopup clipping;
	VoltageEnumPopup imagePosition;
	VoltageNumeric fixedWidth;
	VoltageNumeric fixedHeight;

	VoltageMultinumeric padding;
	VoltageMultinumeric imageBorder;
	VoltageMultinumeric overflow;

	VoltageColor fontColorNormal;
	VoltageObject backgroundNormal;

	VoltageColor fontColorHover;
	VoltageObject backgroundHover;

	VoltageColor fontColorActive;
	VoltageObject backgroundActive;

	VoltageColor fontColorFocused;
	VoltageObject backgroundFocused;

	VoltageList list;

	ScrollArea scrollDetails;

	//VoltageButton btnAdd;
	VoltageButton btnSave;
	VoltageButton btnNewBundle;
	VoltageButton btnReload;

	FoldoutArea foldoutGeneral;
	FoldoutArea foldoutImages;
	//List<List<KeyValuePair<string,GUIStyle>>> stylesList = new List<List<KeyValuePair<string, GUIStyle>>>(0);
	StreamArea styleNotNull;
	StreamArea styleNull;

	[MenuItem("Voltage/Style Editor")]
	public static void Init()
	{
		StyleEditorWindow window = EditorWindow.GetWindow<StyleEditorWindow>("Style Editor");
		window.Show();
	}

	public void LoadStyles()
	{
		Styles.ReloadBundles();

		list = new VoltageList(false, new ElementSettings(true));
		list.OnItemSelected += ListSelectedCallback;
		list.OnDuplicateItem += ListDuplicateCallback;
		list.OnNewItem += ListNewCallback;
		list.OnDeleteItem += ListDeleteCallback;

		int[] folder;

		List<GUIStyle> styles;

		bundles = new List<string> (Styles.GetBundles());

		for(int i = 0; i < bundles.Count;i++)
		{
			folder = list.AddFolder(new GUIContent(bundles[i]));
			styles = Styles.GetStyles(bundles[i]);

			foreach (GUIStyle style in styles)
			{
				list.AddItem(folder, new GUIContent(style.name));
			}
		}

		list.SelectItem(new int[] { 0 });
	}
	private void InitFields()
	{
		currentStyle = GUIStyle.none;
		{
			fontName = new VoltageText(currentStyle.name);
			font = new VoltageObject(currentStyle.font, typeof(Font));
			fontSize = new VoltageNumeric(currentStyle.fontSize);
			fontStyle = new VoltageEnumPopup(currentStyle.fontStyle);
			alignment = new VoltageEnumPopup(currentStyle.alignment);
			clipping = new VoltageEnumPopup(currentStyle.clipping);
			imagePosition = new VoltageEnumPopup(currentStyle.imagePosition);

			fixedWidth = new VoltageNumeric(currentStyle.fixedWidth);
			fixedHeight = new VoltageNumeric(currentStyle.fixedHeight);

			imageBorder = new VoltageMultinumeric(currentStyle.border);
			overflow = new VoltageMultinumeric(currentStyle.overflow);
			padding = new VoltageMultinumeric(currentStyle.padding);

			fontColorNormal = new VoltageColor(currentStyle.normal.textColor);
			backgroundNormal = new VoltageObject(currentStyle.normal.background, typeof(Texture2D));

			fontColorActive = new VoltageColor(currentStyle.active.textColor);
			backgroundActive = new VoltageObject(currentStyle.active.background, typeof(Texture2D));

			fontColorHover = new VoltageColor(currentStyle.hover.textColor);
			backgroundHover = new VoltageObject(currentStyle.hover.background, typeof(Texture2D));

			fontColorFocused = new VoltageColor(currentStyle.focused.textColor);
			backgroundFocused = new VoltageObject(currentStyle.focused.background, typeof(Texture2D));
		}
		currentStyle = null;
	}

	private void CurrentStyle2Fields()
	{
		bool isNull = currentStyle == null;

		if (isNull)
			currentStyle = GUIStyle.none;

		fontName.Text = currentStyle.name;
		font.objectReference = currentStyle.font;
		fontSize.IntValue = currentStyle.fontSize;
		fontStyle.value = currentStyle.fontStyle;
		alignment.value = currentStyle.alignment;
		clipping.value = currentStyle.clipping;
		imagePosition.value = currentStyle.imagePosition;

		fixedWidth.FloatValue = currentStyle.fixedWidth;
		fixedHeight.FloatValue = currentStyle.fixedHeight;

		imageBorder.RectOffsetValue = currentStyle.border;
		overflow.RectOffsetValue = currentStyle.overflow;
		padding.RectOffsetValue = currentStyle.padding;

		fontColorNormal.Color = currentStyle.normal.textColor;
		backgroundNormal.objectReference = currentStyle.normal.background;

		fontColorActive.Color = currentStyle.active.textColor;
		backgroundActive.objectReference = currentStyle.active.background;

		fontColorHover.Color = currentStyle.hover.textColor;
		backgroundHover.objectReference = currentStyle.hover.background;

		fontColorFocused.Color = currentStyle.focused.textColor;
		backgroundFocused.objectReference = currentStyle.focused.background;

		if (isNull)
			currentStyle = null;
	}
	private void Fields2CurrentStyle()
	{
		if (currentStyle != null)
		{
			currentStyle.font = (Font)font.objectReference;
			currentStyle.fontSize = fontSize.IntValue;

			currentStyle.fontStyle = (FontStyle)fontStyle.value;
			currentStyle.alignment = (TextAnchor)alignment.value;
			currentStyle.clipping = (TextClipping)clipping.value;
			currentStyle.imagePosition = (ImagePosition)imagePosition.value;

			currentStyle.fixedWidth = fixedWidth.FloatValue;
			currentStyle.fixedHeight = fixedHeight.FloatValue;

			currentStyle.border = imageBorder.RectOffsetValue;
			currentStyle.overflow = overflow.RectOffsetValue;
			currentStyle.padding = padding.RectOffsetValue;

			currentStyle.normal.textColor = fontColorNormal.Color;
			currentStyle.active.textColor = fontColorActive.Color;
			currentStyle.hover.textColor = fontColorHover.Color;
			currentStyle.focused.textColor = fontColorFocused.Color;

			currentStyle.normal.background = (Texture2D)backgroundNormal.objectReference;
			currentStyle.active.background = (Texture2D)backgroundActive.objectReference;
			currentStyle.hover.background = (Texture2D)backgroundHover.objectReference;
			currentStyle.focused.background = (Texture2D)backgroundFocused.objectReference;
		}
	}

	protected override void VoltageInit()
	{
		previewType = new VoltageEnumPopup(StylePreviewType.Button);

		//btnAdd = new VoltageButton("Add Style", AddStyleToCurrentBundle);
		btnSave = new VoltageButton("Save", ButtonSave);
		btnNewBundle = new VoltageButton("New Bundle", ButtonNewBundle);
		btnReload = new VoltageButton("Reload", LoadStyles);
		splitMain = new SplitArea(0.4f, new ElementSettings(true));

		scrollDetails = new ScrollArea(new ElementSettings(true));

		foldoutGeneral = new FoldoutArea("General");
		foldoutImages = new FoldoutArea("States");

		InitFields();
		LoadStyles();
		CurrentStyle2Fields();

		guideButton = new VoltageLink("Voltage Quickstart Guide", "https://docs.google.com/document/d/1M_0fuK4Fa3a34AShCwT5dQsaEa8_RXXfHBKKeGo7APs/edit#heading=h.cfj271qn228f");

		styleNotNull = new StreamArea(new AreaSettings(false, new RectOffset(10, 10, 10, 10), 10f));
		Constructor.StartStoredConstructor(styleNotNull);
		{
			Constructor.LabeledField("Name", Styles.GetStyle("Title"), fontName);

			Constructor.FoldoutAreaStart(foldoutGeneral);
			{
				Constructor.StreamAreaStart(new AreaSettings(false, new RectOffset(5, 5, 5, 5), 10f));
				{
					Constructor.StreamAreaStart(new AreaSettings(false, 3f));
					{
						Constructor.LabeledField("Font", font);
						Constructor.LabeledField("Font size", fontSize);
						Constructor.LabeledField("Font style", fontStyle);
						Constructor.LabeledField("Alignment", alignment);
						Constructor.LabeledField("Clipping", clipping);
						Constructor.LabeledField("Image Position", imagePosition);
						Constructor.LabeledField("Fixed Width", fixedWidth);
						Constructor.LabeledField("Fixed Height", fixedHeight);
					}
					Constructor.EndArea();

					Constructor.StreamAreaStart(new AreaSettings(false, 3f));
					{
						Constructor.LabeledField("Padding", padding);
						Constructor.LabeledField("Image Border", imageBorder);
						Constructor.LabeledField("Image Overflow", overflow);
					}
					Constructor.EndArea();
				}
				Constructor.EndArea();
			}
			Constructor.EndArea();

			Constructor.FoldoutAreaStart(foldoutImages);
			{
				Constructor.StreamAreaStart(new AreaSettings(false, new RectOffset(5, 5, 5, 5), 3f));
				{
					Constructor.StreamAreaStart();
					{
						Constructor.Label("Normal", Styles.GetStyle("Title"));
						Constructor.LabeledField("Font Color", fontColorNormal);
						Constructor.LabeledField("Background", backgroundNormal);
					}
					Constructor.EndArea();

					Constructor.StreamAreaStart();
					{
						Constructor.Label("Hover", Styles.GetStyle("Title"));
						Constructor.LabeledField("Font Color", fontColorHover);
						Constructor.LabeledField("Background", backgroundHover);
					}
					Constructor.EndArea();

					Constructor.StreamAreaStart();
					{
						Constructor.Label("Active", Styles.GetStyle("Title"));
						Constructor.LabeledField("Font Color", fontColorActive);
						Constructor.LabeledField("Background", backgroundActive);
					}
					Constructor.EndArea();

					Constructor.StreamAreaStart();
					{
						Constructor.Label("Focused", Styles.GetStyle("Title"));
						Constructor.LabeledField("Font Color", fontColorFocused);
						Constructor.LabeledField("Background", backgroundFocused);
					}
					Constructor.EndArea();
				}
				Constructor.EndArea();

			}
			Constructor.EndArea();
		}
		Constructor.EndStoredConstructor();

		styleNull = new StreamArea(new AreaSettings(false, new RectOffset(10, 10, 10, 10), 10f));
		Constructor.StartStoredConstructor(styleNull);
		{
			Constructor.Field(new VoltageLabel(new GUIContent("No Style Selected")));
		}
		Constructor.EndStoredConstructor();
	}

	protected override void VoltageGUI()
	{
		Constructor.StreamAreaStart();
		{
			Constructor.StreamAreaStart(new AreaSettings(true));
			{
				Constructor.Field(btnSave);
				Constructor.Field(btnNewBundle);
				Constructor.Field(btnReload);
			}
			Constructor.EndArea();

			Constructor.SplitAreaStart(splitMain);
			{
				Constructor.StreamAreaStart();
				{
					Constructor.Field(list);
				}
				Constructor.EndArea();
			}
			Constructor.SplitCurrentArea();
			{

				if (currentStyle != null)
				{
					Constructor.StreamAreaStart();
					{
						Constructor.ScrollAreaStart(scrollDetails);
						{
							Constructor.StartArea(styleNotNull);
							Constructor.EndArea();
						}
						Constructor.EndArea();
						Constructor.StreamAreaStart(new AreaSettings(false, new RectOffset(10, 10, 10, 10), 10f), Styles.GetStyle("Multifield FieldBG"));
						{
							Constructor.LabeledField("Preview Type", previewType);
							switch ((StylePreviewType)previewType.value)
							{
								case StylePreviewType.Textbox:
									Constructor.Field(new VoltageText(currentStyle.name, currentStyle));
									break;
								case StylePreviewType.Label:
									Constructor.Label(currentStyle.name, currentStyle);
									break;
								default:
									Constructor.Field(new VoltageButton(currentStyle.name, null, currentStyle));
									break;
							}
						}
						Constructor.EndArea();
					}
					Constructor.EndArea();
				}
				else
				{
					Constructor.StartArea(styleNull);
					Constructor.EndArea();
				}

			}
			Constructor.EndArea();

			Constructor.StreamAreaStart(new AreaSettings(true, new RectOffset(0, 0, 4, 3), 0f, VoltageElementAlignment.Center), Styles.GetStyle("Section Bottom"));
			{
				Constructor.Label("Be sure to check the ");
				Constructor.Field(guideButton);

			}
			Constructor.EndArea();
		}
	}

	protected override void VoltageSerialization()
	{
		if (currentStyle != null)
		{
			Fields2CurrentStyle();

			GUIStyle style = Styles.GetStyle(bundles[currentBundle],currentStyle.name);

			if (style != null)
			{
				if (currentStyle.name != fontName.Text)
				{
					if (Styles.GetStyle(bundles[currentBundle], fontName.Text) == null)
					{
						currentStyle.name = (fontName.Text);
						list.RenameItem(selected[0], fontName.Text);

						Styles.SetStyle(bundles[currentBundle], style.name, currentStyle);
						AssetDatabase.SaveAssets();
						return;
					}
					else
					{
						fontName.Text = currentStyle.name;
						EditorApplication.Beep();
					}
				}

				Styles.SetStyle(bundles[selected[0][0]], style.name, currentStyle);
			}
		}
	}

	#region List Callbacks
	void ListSelectedCallback(VoltageListCallbackArgument e)
	{
		if (!e.folder)
		{
			currentStyle = new GUIStyle(Styles.GetStyle(bundles[e.id[0]], e.content.text));
			currentBundle = e.id[0];
		}
		else
		{
				//Debug.Log("Selected: "+e.id[0]);
			currentStyle = null;
			currentBundle = e.id[0];
		}

		CurrentStyle2Fields();

		if (e.firstSelected)
			selected.Clear();


		selected.Add(e.id);

	}

	void ListDeleteCallback(VoltageListCallbackArgument e)
	{
		if (!e.folder)
		{
			Styles.DeleteStyle(bundles[e.id[0]],e.content.text);
			currentStyle = null;
			list.DeleteItem(e.id);
			//LoadStyles();
		}
		else
		{
			if(!(e.id[0] == 0 && e.id.Length==1))
			{
				currentStyle = null;
				list.DeleteItem(e.id);
				//Debug.Log(bundles[e.id[0]]);
				Styles.DeleteBundle(bundles[e.id[0]]);
				bundles.RemoveAt(e.id[0]);
			}
			else
			{
				EditorApplication.Beep();
			}

		}
	}
	void ListDuplicateCallback(VoltageListCallbackArgument e)
	{
		if (!e.folder)
		{
			string styleName = Styles.AddStyle(bundles[e.id[0]], Styles.GetStyle(bundles[e.id[0]], e.content.text));
			AssetDatabase.SaveAssets();

			//list.SelectItem(list.AddItem(new int[] { selected[0][0] }, new GUIContent(styleName)));
			//int[] i = list.AddItem(new int[] { e.id[0] }, new GUIContent(styleName));

			//string s = "";
			//foreach (int a in i)
			//	s += a + ", ";
			//Debug.Log(s);

			list.SelectItem(list.AddItem(new int[] { e.id[0] }, new GUIContent(styleName)));
		}
		else
		{
			EditorApplication.Beep();
		}
	}
	void ListNewCallback(VoltageListCallbackArgument e)
	{ 
		string styleName = Styles.AddStyle(bundles[e.id[0]]);
		AssetDatabase.SaveAssets();

		list.SelectItem(list.AddItem(new int[] { e.id[0] }, new GUIContent(styleName)));
	}
	#endregion

	#region Buttons

	void ButtonSave()
	{
		AssetDatabase.SaveAssets();
	}

	void ButtonNewBundle()
	{
		Styles.NewBundle();
		LoadStyles();
	}

	//void AddStyleToCurrentBundle()
	//{

	//	string styleName = Styles.AddStyle(bundles[currentBundle]);
	//	AssetDatabase.SaveAssets();

	//	list.SelectItem(list.AddItem(new int[] { selected[0][0] }, new GUIContent(styleName)));
	//}
	#endregion
}
