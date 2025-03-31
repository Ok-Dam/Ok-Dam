using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName ="StyleBundle",menuName = "Voltage/Style Bundle")]
[System.Serializable]
public class StyleBundle : ScriptableObject
{
	public static bool reloadBundles = false;

	[SerializeField]
	private List<GUIStyle> styles = new List<GUIStyle>(0);

	/// <summary>
	/// Gets all the styles stored on this bundle
	/// </summary>
	/// <returns></returns>
	public List<GUIStyle> GetStyles()
	{
		return styles;
	}

	/// <summary>
	/// Adds a new style
	/// </summary>
	/// <param name="styleID"></param>
	public string AddStyle()
	{
		//Get Serialized properties
		SerializedObject styleBundle = new SerializedObject(this);
		SerializedProperty stylesArray = styleBundle.FindProperty("styles");

		//Add a new style and retrieve it
		stylesArray.arraySize++;
		SerializedProperty newStyleObject = stylesArray.GetArrayElementAtIndex(stylesArray.arraySize - 1);

		//Create new blank style
		GUIStyle newStyle = GUIStyle.none;

		//Find available ID
		int i = 0;
		while (ContainsStyle("New Style " + i))
		{
			i++;
		}
		newStyle.name = "New Style " + i;

		//Set style settings
		SetStyle(newStyleObject, newStyle);
		styleBundle.ApplyModifiedProperties();

		//return ID
		return newStyle.name;
	}
	/// <summary>
	/// Adds a new style based on an existing one
	/// </summary>
	/// <param name="style"></param>
	public string AddStyle(GUIStyle style)
	{
		//Get Serialized properties
		SerializedObject styleBundle = new SerializedObject(this);
		SerializedProperty stylesArray = styleBundle.FindProperty("styles");

		//Add a new style and retrieve it
		stylesArray.arraySize++;
		SerializedProperty newStyleObject = stylesArray.GetArrayElementAtIndex(stylesArray.arraySize - 1);

		//Duplicate style to avoid overrides
		GUIStyle newStyle = new GUIStyle(style);

		//Find available ID
		int i = 0;
		while (ContainsStyle(newStyle.name + ((i>0) ? i.ToString():"")))
		{
			i++;
		}
		newStyle.name = newStyle.name + ((i > 0) ? i.ToString() : "");

		//Set style settings
		SetStyle(newStyleObject, newStyle);
		styleBundle.ApplyModifiedProperties();

		//return ID
		return newStyle.name;
	}
	/// <summary>
	/// Gets the style matching the styleID
	/// </summary>
	/// <param name="styleID"></param>
	/// <returns></returns>
	public GUIStyle GetStyle(string styleID)
	{
		GUIStyle style = styles.Find(s => s.name == styleID);
		//if (style == null)
		//{
		//	Debug.Log("[Null] Voltage Style with name [" + styleID + "] does not exist on " + name + " bundle");
		//	style = GUIStyle.none;
		//}
		return style;
	}

	/// <summary>
	/// Sets the style matching the styleID
	/// </summary>
	/// <param name="styleID"></param>
	/// <param name="value"></param>
	public void SetStyle(string styleID, GUIStyle value)
	{
		SerializedObject styleBundle = new SerializedObject(this);
		SerializedProperty stylesArray = styleBundle.FindProperty("styles");
		SerializedProperty style = null;

		for (int i = 0; i < stylesArray.arraySize; i++)
		{
			style = stylesArray.GetArrayElementAtIndex(i);

			if (style.FindPropertyRelative("m_Name").stringValue == styleID)
			{
				SetStyle(style, value);
			}
		}
	}

	private void SetStyle(SerializedProperty style, GUIStyle value)
	{
		style.FindPropertyRelative("m_Name").stringValue = value.name;

		style.FindPropertyRelative("m_Font").objectReferenceValue = value.font;
		style.FindPropertyRelative("m_FontSize").intValue = value.fontSize;
		style.FindPropertyRelative("m_FontStyle").enumValueIndex = (int)value.fontStyle;
		style.FindPropertyRelative("m_Alignment").enumValueIndex = (int)value.alignment;
		style.FindPropertyRelative("m_TextClipping").enumValueIndex = (int)value.clipping;
		style.FindPropertyRelative("m_ImagePosition").enumValueIndex = (int)value.imagePosition;

		style.FindPropertyRelative("m_FixedWidth").floatValue = value.fixedWidth;
		style.FindPropertyRelative("m_FixedHeight").floatValue = value.fixedHeight;


		style.FindPropertyRelative("m_Normal").FindPropertyRelative("m_TextColor").colorValue = value.normal.textColor;
		style.FindPropertyRelative("m_Normal").FindPropertyRelative("m_Background").objectReferenceValue = value.normal.background;

		style.FindPropertyRelative("m_Focused").FindPropertyRelative("m_TextColor").colorValue = value.focused.textColor;
		style.FindPropertyRelative("m_Focused").FindPropertyRelative("m_Background").objectReferenceValue = value.focused.background;

		style.FindPropertyRelative("m_Hover").FindPropertyRelative("m_TextColor").colorValue = value.hover.textColor;
		style.FindPropertyRelative("m_Hover").FindPropertyRelative("m_Background").objectReferenceValue = value.hover.background;

		style.FindPropertyRelative("m_Active").FindPropertyRelative("m_TextColor").colorValue = value.active.textColor;
		style.FindPropertyRelative("m_Active").FindPropertyRelative("m_Background").objectReferenceValue = value.active.background;


		style.FindPropertyRelative("m_Padding").FindPropertyRelative("m_Left").intValue = value.padding.left;
		style.FindPropertyRelative("m_Padding").FindPropertyRelative("m_Right").intValue = value.padding.right;
		style.FindPropertyRelative("m_Padding").FindPropertyRelative("m_Bottom").intValue = value.padding.bottom;
		style.FindPropertyRelative("m_Padding").FindPropertyRelative("m_Top").intValue = value.padding.top;

		style.FindPropertyRelative("m_Border").FindPropertyRelative("m_Left").intValue = value.border.left;
		style.FindPropertyRelative("m_Border").FindPropertyRelative("m_Right").intValue = value.border.right;
		style.FindPropertyRelative("m_Border").FindPropertyRelative("m_Bottom").intValue = value.border.bottom;
		style.FindPropertyRelative("m_Border").FindPropertyRelative("m_Top").intValue = value.border.top;

		style.FindPropertyRelative("m_Overflow").FindPropertyRelative("m_Left").intValue = value.overflow.left;
		style.FindPropertyRelative("m_Overflow").FindPropertyRelative("m_Right").intValue = value.overflow.right;
		style.FindPropertyRelative("m_Overflow").FindPropertyRelative("m_Bottom").intValue = value.overflow.bottom;
		style.FindPropertyRelative("m_Overflow").FindPropertyRelative("m_Top").intValue = value.overflow.top;

		style.serializedObject.ApplyModifiedProperties();

	}

	/// <summary>
	/// Deletes the style matching the styleID
	/// </summary>
	/// <param name="styleID"></param>
	public void DeleteStyle(string styleID)
	{
		SerializedObject styleBundle = new SerializedObject(this);
		SerializedProperty stylesArray = styleBundle.FindProperty("styles");
		SerializedProperty style = null;

		for (int i = 0; i < stylesArray.arraySize; i++)
		{
			style = stylesArray.GetArrayElementAtIndex(i);

			if (style.FindPropertyRelative("m_Name").stringValue == styleID)
			{
				stylesArray.MoveArrayElement(i, stylesArray.arraySize - 1);
				stylesArray.arraySize--;
				styleBundle.ApplyModifiedProperties();
				return;
			}
		}
	}

	/// <summary>
	/// Does this bundle contains the style matching the styleID
	/// </summary>
	/// <param name="styleID"></param>
	/// <returns></returns>
	public bool ContainsStyle(string styleID)
	{
		return (styles.Find(s => s.name == styleID) != null);
	}

}
