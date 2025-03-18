using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Voltage;

[CustomEditor(typeof(CustomEditorExample))]
public class ExampleEditor : VoltageEditor
{
	FoldoutArea fold;
	FoldoutArea fold2;
	TabArea tab;
	protected override void VoltageInit()
	{
		fold = new FoldoutArea("Sup");
		fold2 = new FoldoutArea("Sup");
		tab = new TabArea();
		tab.AddTab("Tab1");
		tab.AddTab("Tab2");
	}
	protected override void VoltageGUI()
	{

		Constructor.WeightAreaStart();
		Constructor.FoldoutAreaStart(fold);
		Constructor.StreamAreaStart();
		Constructor.Label("VoltageEditor");
		Constructor.Label("VoltageEditor");
		Constructor.Label("VoltageEditor");
		Constructor.Label("VoltageEditor");
		Constructor.Label("VoltageEditor");
		Constructor.EndArea();
		Constructor.EndArea();
		Constructor.FoldoutAreaStart(fold2);
		Constructor.StreamAreaStart();
		Constructor.Label("VoltageEditor");
		Constructor.Label("VoltageEditor");
		Constructor.Label("VoltageEditor");
		Constructor.Label("VoltageEditor");
		Constructor.Label("VoltageEditor");
		Constructor.EndArea();
		Constructor.EndArea();
		Constructor.EndArea();
		Constructor.TabAreaStart(tab);
		Constructor.Label("VoltageEditor");
		Constructor.NextTab();
		Constructor.StreamAreaStart();
		Constructor.Label("VoltageEditor");
		Constructor.Label("VoltageEditor");
		Constructor.Label("VoltageEditor");
		Constructor.EndArea();
		Constructor.EndArea();
		
	}

	protected override void VoltageSerialization()
	{
	}
}
