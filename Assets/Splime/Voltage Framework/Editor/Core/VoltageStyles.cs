using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class VoltageStyles : ScriptableObject {
	private const string FILE = "VoltageStyles";

	private const string  MAINPATH = "Assets/Splime/Voltage Framework/Styles/" + FILE + ".asset";
	private static VoltageStyles mainInstance;

	private static Dictionary<string,VoltageStyles> styleBundles = new Dictionary<string, VoltageStyles>(0);
	private static bool folderChecked;

	[SerializeField]
	public List<GUIStyle> styles = new List<GUIStyle>(0);

	protected static VoltageStyles MainInstance
	{
		get
		{
			if (mainInstance == null)
			{
				mainInstance = AssetDatabase.LoadAssetAtPath<VoltageStyles>(MAINPATH);
				if (mainInstance == null)
				{
					mainInstance = ScriptableObject.CreateInstance<VoltageStyles>();
					mainInstance.name = FILE;
					AssetDatabase.CreateAsset(mainInstance, MAINPATH);
					AssetDatabase.SaveAssets();
				}
			}
			//else if(AssetDatabase.LoadAssetAtPath<VoltageStyles>(MAINPATH) == null)
			//{
			//	AssetDatabase.CreateAsset(mainInstance, MAINPATH);
			//	AssetDatabase.SaveAssets();
			//}

			return mainInstance;
		}
		
	}
	protected static Dictionary<string, VoltageStyles> StyleBundles
	{
		get
		{
			
			if (!folderChecked)
			{
				Object[] bundles = AssetDatabase.LoadAllAssetsAtPath(MAINPATH);
				styleBundles.Clear();
				foreach (Object o in bundles)
				{
					if(o is VoltageStyles && o != MainInstance)
					{
						styleBundles.Add(o.name, (VoltageStyles)o);
					}
				}

				folderChecked = true;
			}

			return styleBundles;
		}
	}

	private void Awake()
	{
		folderChecked = false;
	}
	private void OnDestroy()
	{
		folderChecked = false;
	}

	public static VoltageStyles GetFile()
	{
		return MainInstance;
	}
	public static List<GUIStyle> GetStyles()
	{
		return MainInstance.styles;
	}

	public static GUIStyle GetStyle(string styleID)
	{
		GUIStyle style = GUIStyle.none;
		style = MainInstance.GetStyleI(styleID);
		return style;
	}
	//public static VoltageStyle GetVoltageStyle(string styleID)
	//{
	//	VoltageStyle style = null;
	//	style = MainInstance.GetStyleI(styleID);

	//	return style;
	//}
	public static GUIStyle GetStyle(string bundle, string styleID)
	{
		GUIStyle style = null;
		if (styleBundles.ContainsKey(bundle))
		{
			style = styleBundles[bundle].GetStyleI(styleID);

		}
		return style;
	}
	//public static VoltageStyle GetVoltageStyle(string bundle, string styleID)
	//{
	//	VoltageStyle style = null;
	//	if (styleBundles.ContainsKey(bundle))
	//	{
	//		style = styleBundles[bundle].GetStyleI(styleID);

	//	}

	//	return style;
	//}
	//public static bool SetStyle(string key,GUIStyle value)
	//{
	//	return mainInstance.SetStyleI(key, value);
	//}


	//public static void NewStyle()
	//{
	//	int i = 0;
	//	while (MainInstance.ContainsStyle("New Style " + i))
	//	{
	//		i++;
	//	}
	//	AddStyle("New Style " + i);
	//}

	//public static void AddStyle(string _name)
	//{
	//	AddStyle(_name, new GUIStyle());
	//}
	//public static void AddStyle(string _name,GUIStyle _style)
	//{
	//	_style.name = _name;

	//	MainInstance.styles.Add(new (_style);
	//	//AssetDatabase.SaveAssets();

	//}

	public GUIStyle GetStyleI(string key)
	{
		foreach (GUIStyle s in styles)
		{
			if (s.name == key)
				return s;
		}
		//Debug.Log("Null Voltage Style " + key);
		return null;
	}
	//public bool SetStyleI(string key, GUIStyle value)
	//{
	//	value.name = key;
	//	for (int i = 0; i < styles.Count; i++)
	//	{

	//		if (styles[i].style.name == key)
	//		{
	//			styles[i] = value;
	//			return true;
	//		}
	//	}
	//	return false;
	//}
	public bool ContainsStyle(string key)
	{

		foreach (GUIStyle s in styles)
		{
			if (s.name == key)
				return true;
		}

		return false;
	}
}
