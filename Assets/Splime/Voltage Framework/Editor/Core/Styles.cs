using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class Styles
{
	private const string FILE = "Voltage Styles";

	private const string MAINPATH = "/Splime/Voltage Framework/Style Bundles/";
	private const string MAINBUNDLE = "Assets" + MAINPATH + FILE + ".asset";


	private static StyleBundle mainInstance;
	private static Dictionary<string, StyleBundle> styleBundles = new Dictionary<string, StyleBundle>(0);

	private static StyleBundle MainInstance
	{
		get
		{
			ValidatePath();

			if (mainInstance == null)
			{
				mainInstance = AssetDatabase.LoadAssetAtPath<StyleBundle>(MAINBUNDLE);
				if (mainInstance == null)
				{
					mainInstance = ScriptableObject.CreateInstance<StyleBundle>();
					mainInstance.name = FILE;
					AssetDatabase.CreateAsset(mainInstance, MAINBUNDLE);
					AssetDatabase.SaveAssets();
				}
			}
			else if (AssetDatabase.LoadAssetAtPath<StyleBundle>(MAINBUNDLE) == null)
			{
				AssetDatabase.CreateAsset(mainInstance, MAINBUNDLE);
				AssetDatabase.SaveAssets();
			}

			return mainInstance;
		}

	}
	private static Dictionary<string, StyleBundle> StyleBundles
	{
		get
		{
			return styleBundles;
		}
	}

	public static void ReloadBundles()
	{
		ReloadBundlesInternal();
	}

	private static void ValidatePath()
	{
		if (!System.IO.Directory.Exists(Application.dataPath + MAINPATH))
		{
			string[] directories = (Application.dataPath + MAINPATH).Split('/');
			string currentDirectory = directories[0];
			for(int i=1;i<directories.Length;i++)
			{
				currentDirectory += "/" + directories[i];
				if(!System.IO.Directory.Exists(currentDirectory))
				{
					Debug.Log(currentDirectory);
					System.IO.Directory.CreateDirectory(currentDirectory);
				}

			}
		}
	}
	private static void ReloadBundlesInternal()
	{
		ValidatePath();

		styleBundles.Clear();
		//Debug.Log("Reloading" + styleBundles.Count);
		styleBundles.Add(MainInstance.name,MainInstance);

		string[] aBundleFiles = System.IO.Directory.GetFiles(Application.dataPath + MAINPATH, "*.asset", System.IO.SearchOption.AllDirectories);

	
		foreach (string bundleFile in aBundleFiles)
		{
			string assetPath = "Assets" + bundleFile.Replace(Application.dataPath, "").Replace('\\', '/');

			string[] split = bundleFile.Replace('\\', '/').Split('/');
			string bundleID = split[split.Length - 1].Replace(".asset", "");
			//Debug.Log("Found: " + bundleID);

			LoadBundle(bundleID);

			//StyleBundle bundle = AssetDatabase.LoadAssetAtPath<StyleBundle>(assetPath);

			//if (bundle != null)
			//{
			//	if (bundle != MainInstance)
			//	{
			//		styleBundles.Add(bundle.name, bundle);
			//	}
			//}
		}
	}
	private static StyleBundle LoadBundle(string bundleID)
	{
		string assetPath = "Assets" + MAINPATH + bundleID + ".asset";
		//Debug.Log("Loading: " + assetPath);
		StyleBundle bundle = AssetDatabase.LoadAssetAtPath<StyleBundle>(assetPath);
		//Debug.Log("Asset: " + bundle.name);

		if (bundle != null)
		{
			if (bundle != MainInstance && !ContainsBundle(bundleID))
			{
				//Debug.Log("Loaded: " + bundle.name);
				styleBundles.Add(bundleID, bundle);
			}
		}
		return bundle;
	}


	private static StyleBundle GetBundle(string bundleID)
	{
		if (styleBundles.ContainsKey(bundleID))
		{
			return styleBundles[bundleID];
		}
		else
		{
			return LoadBundle(bundleID);
		}
	}

	public static StyleBundle NewBundle()
	{
		return NewBundle("New Bundle");
	}
	public static StyleBundle NewBundle(string bundleName)
	{
		ValidatePath();
		if(ContainsBundle(bundleName))
		{
			int i = 0;
			while (ContainsBundle(bundleName +" "+ i))
			{
				i++;
			}
			bundleName = bundleName + " " + i;
		}
		
		StyleBundle newBundle = ScriptableObject.CreateInstance<StyleBundle>();
		newBundle.name = bundleName;
		AssetDatabase.CreateAsset(newBundle,"Assets" + MAINPATH + bundleName + ".asset");
		AssetDatabase.SaveAssets();

		ReloadBundlesInternal();

		return newBundle;
	}

	public static void DeleteBundle(string bundleID)
	{
		//Debug.Log("Assets" + MAINPATH + GetBundle(bundleID).name + ".asset");
		AssetDatabase.DeleteAsset( "Assets"+ MAINPATH + GetBundle(bundleID).name + ".asset");
	}

	public static string[] GetBundles()
	{
		ReloadBundlesInternal();
		List<string> bundleIDs = new List<string>(0);
		foreach(KeyValuePair<string,StyleBundle> bundle in StyleBundles)
		{
			bundleIDs.Add(bundle.Key);
			//Debug.Log("Getting Bundles: " + bundle.Key);
		}
		return bundleIDs.ToArray();
	}
	/// <summary>
	/// Does the bundle exist
	/// </summary>
	/// <param name="bundleID"></param>
	/// <returns></returns>
	public static bool ContainsBundle(string bundleID)
	{
		return (styleBundles.ContainsKey(bundleID));
	}
	/// <summary>
	/// Does the bundle exist
	/// </summary>
	/// <param name="bundleID"></param>
	/// <returns></returns>
	public static bool ContainsBundle(StyleBundle bundle)
	{
		return (styleBundles.ContainsValue(bundle));
	}
	#region MainInstance

	/// <summary>
	/// Returns the styles stored on the VoltageStyles Bundle
	/// </summary>
	/// <returns></returns>
	public static List<GUIStyle> GetStyles()
	{
		return MainInstance.GetStyles();
	}

	/// <summary>
	/// Adds a style to the VoltageStyles Bundle, with name = styleID
	/// </summary>
	public static string AddStyle()
	{
		return MainInstance.AddStyle();
	}
	/// <summary>
	/// Adds a style to the VoltageStyles Bundle, with name = styleID
	/// </summary>
	/// <param name="styleID"></param>
	public static void AddStyle(GUIStyle style)
	{
		MainInstance.AddStyle(style);
	}

	/// <summary>
	/// Returns the style matching the styleID stored on the VoltageStyles Bundle
	/// </summary>
	/// <param name="styleID"></param>
	/// <returns></returns>
	public static GUIStyle GetStyle(string styleID)
	{
		return MainInstance.GetStyle(styleID); ;
	}

	/// <summary>
	/// Sets the style matching the styleID stored on the VoltageStyles Bundle
	/// </summary>
	/// <param name="styleID"></param>
	/// <param name="value"></param>
	public static void SetStyle(string styleID, GUIStyle value)
	{
		MainInstance.SetStyle(styleID, value);
	}

	/// <summary>
	/// Deletes the style matching the styleID stored on the VoltageStyles Bundle
	/// </summary>
	/// <param name="styleID"></param>
	public static void DeleteStyle(string styleID)
	{
		MainInstance.DeleteStyle(styleID);
	}

	#endregion

	#region Bundle by Name
	/// <summary>
	/// Returns the styles stored on the Bundle matching the bundleID
	/// </summary>
	/// <param name="bundleID"></param>
	/// <returns></returns>
	public static List<GUIStyle> GetStyles(string bundleID)
	{
		StyleBundle bundle = GetBundle(bundleID);
		if (bundle != null)
		{
			return bundle.GetStyles();
		}
		return null;
	}

	/// <summary>
	/// Adds a style to the the Bundle matching the bundleID, with name = styleID
	/// </summary>
	/// <param name="bundleID"></param>
	/// <param name="styleID"></param>
	public static string AddStyle(string bundleID)
	{
		StyleBundle bundle = GetBundle(bundleID);
		if (bundle != null)
		{
			return bundle.AddStyle(/*styleID*/);
		}
		return "";
	}
	/// <summary>
	/// Adds a style to the the Bundle matching the bundleID, with name = styleID
	/// </summary>
	/// <param name="bundleID"></param>
	/// <param name="styleID"></param>
	public static string AddStyle(string bundleID, GUIStyle style)
	{
		StyleBundle bundle = GetBundle(bundleID);
		if (bundle != null)
		{
			return bundle.AddStyle(style);
		}
		return "";
	}

	/// <summary>
	/// Returns the style matching the styleID stored on the Bundle matching the bundleID
	/// </summary>
	/// <param name="bundle"></param>
	/// <param name="styleID"></param>
	/// <returns></returns>
	public static GUIStyle GetStyle(string bundleID, string styleID)
	{
		GUIStyle style = null;
		StyleBundle bundle = GetBundle(bundleID);
		if (bundle != null)
		{
			style = bundle.GetStyle(styleID);
		}
		return style;
	}
	/// <summary>
	/// Sets the style matching the styleID stored on the Bundle matching the bundleID
	/// </summary>
	/// <param name="bundleID"></param>
	/// <param name="styleID"></param>
	/// <param name="value"></param>
	public static void SetStyle(string bundleID, string styleID, GUIStyle value)
	{
		StyleBundle bundle = GetBundle(bundleID);
		if (bundle != null)
		{
			bundle.SetStyle(styleID, value);
		}
	}

	/// <summary>
	/// Deletes the style matching the styleID stored on the Bundle matching the bundleID
	/// </summary>
	/// <param name="bundleID"></param>
	/// <param name="styleID"></param>
	public static void DeleteStyle(string bundleID, string styleID)
	{
		StyleBundle bundle = GetBundle(bundleID);
		if (bundle != null)
		{
			bundle.DeleteStyle(styleID);
		}
	}

	#endregion

	#region Bundle by Index
	/// <summary>
	/// Returns the styles stored on the Bundle matching the bundleIndex
	/// </summary>
	/// <param name="bundleIndex"></param>
	/// <returns></returns>
	//public static List<GUIStyle> GetStyles(int bundleIndex)
	//{
	//	StyleBundle bundle = styleBundles[bundleIndex];
	//	if (bundle != null)
	//	{
	//		return bundle.GetStyles();
	//	}
	//	return null;
	//}

	/// <summary>
	/// Adds a style to the the Bundle matching the bundleIndex, with name = styleID
	/// </summary>
	/// <param name="bundleIndex"></param>
	/// <param name="styleID"></param>
	//public static string AddStyle(int bundleIndex/*, string styleID*/)
	//{
	//	StyleBundle bundle = styleBundles[bundleIndex];
	//	if (bundle != null)
	//	{
	//		return bundle.AddStyle(/*styleID*/);
	//	}
	//	return "";
	//}

	/// <summary>
	/// Returns the style matching the styleID stored on the Bundle matching the bundleIndex
	/// </summary>
	/// <param name="bundleIndex"></param>
	/// <param name="styleID"></param>
	/// <returns></returns>
	//public static GUIStyle GetStyle(int bundleIndex, string styleID)
	//{
	//	GUIStyle style = null;
	//	StyleBundle bundle = styleBundles[bundleIndex];
	//	if (bundle != null)
	//	{
	//		style = bundle.GetStyle(styleID);
	//	}
	//	return style;
	//}

	/// <summary>
	/// Sets the style matching the styleID stored on the Bundle matching the bundleIndex
	/// </summary>
	/// <param name="bundleIndex"></param>
	/// <param name="styleID"></param>
	/// <param name="value"></param>
	//public static void SetStyle(int bundleIndex, string styleID, GUIStyle value)
	//{
	//	StyleBundle bundle = styleBundles[bundleIndex];
	//	if (bundle != null)
	//	{
	//		bundle.SetStyle(styleID, value);
	//	}
	//}

	/// <summary>
	/// Deletes the style matching the styleID stored on the Bundle matching the bundleIndex
	/// </summary>
	/// <param name="bundleIndex"></param>
	/// <param name="styleID"></param>
	//public static void DeleteStyle(int bundleIndex, string styleID)
	//{
	//	StyleBundle bundle = styleBundles[bundleIndex];
	//	if (bundle != null)
	//	{
	//		bundle.DeleteStyle(styleID);
	//	}
	//}
	#endregion








	
}
