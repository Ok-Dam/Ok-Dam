using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Voltage;

public class VoltageStyleConverter : VoltageWindow
{
	[MenuItem("Voltage/Style Converter")]
	public static void Init()
	{
		VoltageStyleConverter window = EditorWindow.GetWindow<VoltageStyleConverter>("Style Converter");
		window.Show();
	}

	VoltageObject oldFile;
	VoltageButton buttonConvert;
	protected override void VoltageInit()
	{
		oldFile = new VoltageObject(null, typeof(VoltageStyles));
		buttonConvert = new VoltageButton("Upgrade from VoltageStyle to StyleBundle", Convert);
	}

	protected override void VoltageGUI()
	{
		Constructor.StreamAreaStart();
		{
			Constructor.Paragraph("Use this converter to make a StyleBundle out of a deprecated VoltageStyles");
			Constructor.LabeledField("VoltageStyles", oldFile);
			if(oldFile.objectReference!=null)
				Constructor.Field(buttonConvert);
		}
	}

	
	protected override void VoltageSerialization()
	{
	}

	public void Convert()
	{
		VoltageStyles oldStyles = (VoltageStyles) oldFile.objectReference;
		StyleBundle newBundle = Styles.NewBundle(oldStyles.name + " (To StyleBundle)");

		foreach(GUIStyle style in oldStyles.styles)
		{
			newBundle.AddStyle(style.name);
			newBundle.SetStyle(style.name, style);
		}

		AssetDatabase.SaveAssets();

	}
}
