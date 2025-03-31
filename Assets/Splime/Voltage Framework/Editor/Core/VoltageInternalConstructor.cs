using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Voltage;
using System;

public class VoltageInternalConstructor : IConstructor
{
	private VoltageArea m_FirstArea = null;
	private VoltageArea m_CurrentArea = null;

	private bool isStored = false;
	public VoltageInternalConstructor()
	{

	}
	/// <summary>
	/// Do not use this. For internal calls only.
	/// </summary>
	/// <param name="firstArea"></param>
	public void StartWildConstructor(VoltageArea firstArea)
	{
		isStored = false;
		m_CurrentArea = null;
		m_FirstArea = null;
		StartArea(firstArea);
	}
	/// <summary>
	/// Starts constructing the stored layout of the target area. Use EndStoredConstructor once you are done.
	/// </summary>
	/// <param name="target"></param>
	public void StartStoredConstructor(VoltageArea target)
	{
		isStored = true;
		m_CurrentArea = null;
		m_FirstArea = null;
		StartArea(target);
	}
	/// <summary>
	/// Ends the stored layout construction of the target area.
	/// </summary>
	public void EndStoredConstructor()
	{
		m_CurrentArea = null;
		m_FirstArea = null;
	}

	public void EndAllAreas()
	{
		m_CurrentArea = null;
	}

	public float CalcHeight(float width)
	{
		return m_FirstArea != null ? m_FirstArea.CalcHeight(width) : 0f;
	}
	public float CalcWidth()
	{
		return m_FirstArea != null ? m_FirstArea.CalcWidth() : 0f;
	}
	public void EventCall(Rect workingArea)
	{
		var controlId = GUIUtility.GetControlID(FocusType.Passive);
		var controlEvent = Event.current.GetTypeForControl(controlId);

		m_FirstArea.EventCall(controlEvent, workingArea, Event.current.mousePosition);
	}
	public void DrawCall(Rect workingArea)
	{
		Color col = GUI.color;
		GUI.color = Color.white;
		Color bgcol = GUI.backgroundColor;
		GUI.backgroundColor = Color.white;

		m_FirstArea.DrawElement(workingArea);
		m_FirstArea.CleanWild();

		GUI.color = col;
		GUI.backgroundColor = bgcol;
	}



	public void StartHelper(Action buildMethod)
	{
		if (buildMethod.Target is VoltageHelper)
		{
		if(isStored)
			((VoltageHelper)buildMethod.Target).BuildStored(m_CurrentArea, buildMethod);
			else
				((VoltageHelper)buildMethod.Target).BuildWild(m_CurrentArea, buildMethod);

		}
		else
			Debug.LogError("BuilderMethod on StartBuilder does not belong to a VoltageBuilder");
	}


	public void StartArea(VoltageArea newArea)
	{
		if (newArea != null)
		{
			if (m_CurrentArea == null)
			{
				m_FirstArea = newArea;
			}
			else
			{
				if (isStored)
					m_CurrentArea.AddStoredElement(newArea);
				else
					m_CurrentArea.AddWildElement(newArea);

				newArea.PreviousArea = m_CurrentArea;
			}
			m_CurrentArea = newArea;
		}
		else
		{
			Debug.LogError("Trying to start null Area");
		}

	}

	public void EndArea()
	{
		if (m_CurrentArea != null)
		{
			if (m_CurrentArea.PreviousArea != null)
				m_CurrentArea = m_CurrentArea.PreviousArea;
			else
				m_CurrentArea = null;
		}
		else
		{
			Debug.LogError("EndArea(): No open area to end");
		}
	}

	#region Labels
	public void Label(string content)
	{
		Field(new VoltageLabel(content));
	}

	public void Label(string content, GUIStyle style)
	{
		Field(new VoltageLabel(content, style));
	}

	public void Label(string content, ElementSettings elementSettings)
	{
		Field(new VoltageLabel(content, elementSettings));
	}

	public void Label(string content, ElementSettings elementSettings, GUIStyle style)
	{
		Field(new VoltageLabel(content, elementSettings, style));
	}


	public void Paragraph(string content)
	{
		Field(new VoltageParagraph(content));
	}

	public void Paragraph(string content, GUIStyle style)
	{
		Field(new VoltageParagraph(content, style));
	}

	public void Paragraph(string content, ElementSettings elementSettings)
	{
		Field(new VoltageParagraph(content, elementSettings));
	}

	public void Paragraph(string content, ElementSettings elementSettings, GUIStyle style)
	{
		Field(new VoltageParagraph(content, elementSettings, style));
	}


	public void LabeledField(string content, VoltageElement field)
	{
		WeightAreaStart();
		field.Weight = 3;
		Field(new VoltageLabel(content, new ElementSettings(2)));
		Field(field);
		EndArea();
	}

	public void LabeledField(string content, GUIStyle style, VoltageElement field)
	{
		WeightAreaStart();
		field.Weight = 3;
		Field(new VoltageLabel(content, new ElementSettings(2), style));
		Field(field);
		EndArea();
	}
	public void LabeledField(string content, ElementSettings elementSettings, GUIStyle style, VoltageElement field)
	{
		
		WeightAreaStart();
		Field(new VoltageLabel(content, elementSettings, style));
		Field(field);
		EndArea();
	}
	#endregion

	#region ScrollArea
	/// <summary>
	/// Adds a ScrollArea to the current Area.
	/// </summary>
	/// <param name="scrollArea"></param>
	public void ScrollAreaStart(ScrollArea scrollArea)
	{
		StartArea(scrollArea);
	}

	#endregion

	#region SplitArea
	/// <summary>
	/// Adds a SplitArea to the current area
	/// </summary>
	/// <param name="splitArea"></param>
	public void SplitAreaStart(SplitArea splitArea)
	{
		StartArea(splitArea);
	}
	/// <summary>
	/// Makes the second area of the active SplitArea active for adding elements.
	/// </summary>
	public void SplitCurrentArea()
	{
		if (m_CurrentArea is SplitArea)
		{
			(m_CurrentArea as SplitArea).Split();
		}
		else
		{
			Debug.LogError("SplitCurrentArea(): Current area isn't of type SplitArea");
		}
	}
	#endregion

	#region TabArea
	/// <summary>
	/// Adds a TabArea to the current area
	/// </summary>
	/// <param name="tabArea"></param>
	public void TabAreaStart(TabArea tabArea)
	{
		StartArea(tabArea);
	}
	/// <summary>
	/// Makes the next tab of the active TabArea active for adding elements.
	/// </summary>
	public void NextTab()
	{
		if (m_CurrentArea is TabArea)
		{
			(m_CurrentArea as TabArea).NextTab();
		}
		else
		{
			Debug.LogError("NextTab(): Current area isn't of type TabArea");
		}
	}

	#endregion

	#region FoldoutArea
	/// <summary>
	/// Adds a FoldoutArea to the current area
	/// </summary>
	/// <param name="foldoutArea"></param>
	public void FoldoutAreaStart(FoldoutArea foldoutArea)
	{
		StartArea(foldoutArea);
	}
	#endregion

	#region WeightArea
	/// <summary>
	/// Adds a WeightArea to the current area.
	/// </summary>
	/// <param name="weightArea"></param>
	public void WeightAreaStart(WeightArea weightArea)
	{
		StartArea(weightArea);
	}
	/// <summary>
	/// Adds a new WeightArea to the current area.
	/// </summary>
	public void WeightAreaStart()
	{
		StartArea(new WeightArea());
	}
	/// <summary>
	/// Adds a new WeightArea to the current area.
	/// </summary>
	public void WeightAreaStart(GUIStyle style)
	{
		StartArea(new WeightArea(style));
	}
	/// <summary>
	/// Adds a new WeightArea to the current area.
	/// </summary>
	/// <param name="elementSettings"></param>
	public void WeightAreaStart(ElementSettings elementSettings)
	{
		StartArea(new WeightArea(elementSettings));
	}
	/// <summary>
	/// Adds a new WeightArea to the current area.
	/// </summary>
	/// <param name="areaSettings"></param>
	public void WeightAreaStart(AreaSettings areaSettings)
	{
		StartArea(new WeightArea(areaSettings));
	}
	/// <summary>
	/// Adds a new WeightArea to the current area.
	/// </summary>
	/// <param name="areaSettings"></param>
	/// <param name="style"></param>
	public void WeightAreaStart(AreaSettings areaSettings, GUIStyle style)
	{
		StartArea(new WeightArea(areaSettings, style));
	}
	/// <summary>
	/// Adds a new WeightArea to the current area.
	/// </summary>
	/// <param name="elementSettings"></param>
	/// <param name="style"></param>
	public void WeightAreaStart(ElementSettings elementSettings, GUIStyle style)
	{
		StartArea(new WeightArea(elementSettings, style));
	}

	/// <summary>
	/// Adds a new WeightArea to the current area.
	/// </summary>
	/// <param name="elementSettings"></param>
	/// <param name="areaSettings"></param>
	public void WeightAreaStart(ElementSettings elementSettings, AreaSettings areaSettings)
	{
		StartArea(new WeightArea(elementSettings, areaSettings));
	}
	/// <summary>
	/// Adds a new WeightArea to the current area.
	/// </summary>
	/// <param name="elementSettings"></param>
	/// <param name="areaSettings"></param>
	/// <param name="style"></param>
	public void WeightAreaStart(ElementSettings elementSettings, AreaSettings areaSettings, GUIStyle style)
	{
		StartArea(new WeightArea(elementSettings, areaSettings, style));
	}
	#endregion

	#region StreamArea
	/// <summary>
	/// Adds a StreamArea to the current area.
	/// </summary>
	/// <param name="streamArea"></param>
	public void StreamAreaStart(StreamArea streamArea)
	{
		StartArea(streamArea);
	}

	/// <summary>
	/// Adds a new StreamArea to the current area.
	/// </summary>
	public void StreamAreaStart()
	{
		StartArea(new StreamArea());
	}
	/// <summary>
	/// Adds a new StreamArea to the current area.
	/// </summary>
	/// <param name="style"></param>
	public void StreamAreaStart(GUIStyle style)
	{
		StartArea(new StreamArea(style));
	}
	/// <summary>
	/// Adds a new StreamArea to the current area.
	/// </summary>
	/// <param name="elementSettings"></param>
	public void StreamAreaStart(ElementSettings elementSettings)
	{
		StartArea(new StreamArea(elementSettings));
	}
	/// <summary>
	/// Adds a new StreamArea to the current area.
	/// </summary>
	/// <param name="areaSettings"></param>
	public void StreamAreaStart(AreaSettings areaSettings)
	{
		StartArea(new StreamArea(areaSettings));
	}
	/// <summary>
	/// Adds a new StreamArea to the current area.
	/// </summary>
	/// <param name="elementSettings"></param>
	/// <param name="style"></param>
	public void StreamAreaStart(ElementSettings elementSettings, GUIStyle style)
	{
		StartArea(new StreamArea(elementSettings, style));
	}
	/// <summary>
	/// Adds a new StreamArea to the current area.
	/// </summary>
	/// <param name="areaSettings"></param>
	/// <param name="style"></param>
	public void StreamAreaStart(AreaSettings areaSettings, GUIStyle style)
	{
		StartArea(new StreamArea(areaSettings, style));
	}
	/// <summary>
	/// Adds a new StreamArea to the current area.
	/// </summary>
	/// <param name="elementSettings"></param>
	/// <param name="areaSettings"></param>
	public void StreamAreaStart(ElementSettings elementSettings, AreaSettings areaSettings)
	{
		StartArea(new StreamArea(elementSettings, areaSettings));
	}
	/// <summary>
	/// Adds a new StreamArea to the current area.
	/// </summary>
	/// <param name="elementSettings"></param>
	/// <param name="areaSettings"></param>
	/// <param name="style"></param>
	public void StreamAreaStart(ElementSettings elementSettings, AreaSettings areaSettings, GUIStyle style)
	{
		StartArea(new StreamArea(elementSettings, areaSettings, style));
	}
	#endregion
	

	public void Image(VoltageImage image)
	{
		if (m_CurrentArea != null)
		{
			if (image != null)
			{
				StreamAreaStart(new AreaSettings(false, VoltageElementAlignment.Center));
				StreamAreaStart(new AreaSettings(true, VoltageElementAlignment.Center));
				Field(image);
				EndArea();
				EndArea();
			}
			else
				Debug.LogError("Trying to add null element.");

		}
		else
		{
			Debug.LogError("There is no Area active, this shouldn't happen.");
		}
	}

	public void Image(VoltageImage image, VoltageElementAlignment alignment)
	{
		if (m_CurrentArea != null)
		{
			if (image != null)
			{
				StreamAreaStart(new AreaSettings(false, alignment));
				StreamAreaStart(new AreaSettings(true, alignment));
				Field(image);
				EndArea();
				EndArea();
			}
			else
				Debug.LogError("Trying to add null element.");

		}
		else
		{
			Debug.LogError("There is no Area active, this shouldn't happen.");
		}
	}

	public void Field(VoltageElement element)
	{
		if (m_CurrentArea != null)
		{
			if (element != null){
				if (isStored)
					m_CurrentArea.AddStoredElement(element);
				else
					m_CurrentArea.AddWildElement(element);
			}
				
			else
				Debug.LogError("Trying to add null element.");

		}
		else
		{
			Debug.LogError("There is no Area active, this shouldn't happen.");
		}

	}

	public void Field(VoltageElement element, int weight)
	{
		if (m_CurrentArea != null)
		{
			element.Weight = weight;
			if (isStored)
				m_CurrentArea.AddStoredElement(element);
			else
				m_CurrentArea.AddWildElement(element);
		}
	}
}
