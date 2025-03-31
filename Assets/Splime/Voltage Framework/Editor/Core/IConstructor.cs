using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Voltage
{
	public interface IConstructor
	{

		/// <summary>
		/// Starts constructing the stored layout of the target area. Use EndStoredConstructor once you are done.
		/// </summary>
		/// <param name="target"></param>
		void StartStoredConstructor(VoltageArea target);
		/// <summary>
		/// Ends the stored layout construction of the target area.
		/// </summary>
		void EndStoredConstructor();

		/// <summary>
		/// Starts a VoltageHelper method that will add a new area (already filled inside the method provided) to the current area.
		/// </summary>
		/// <param name="helperMethod"></param>
		void StartHelper(Action helperMethod);

		/// <summary>
		/// Adds a VoltageArea (any) to the current area
		/// </summary>
		/// <param name="newArea"></param>
		void StartArea(VoltageArea area);

		#region Labels
		/// <summary>
		/// Adds a new Label to the current Area.
		/// </summary>
		/// <param name="content"></param>
		 void Label(string content);
		/// <summary>
		/// Adds a new Label to the current Area.
		/// </summary>
		/// <param name="content"></param>
		/// <param name="style">Label style</param>
		 void Label(string content, GUIStyle style);
		/// <summary>
		/// Adds a new Label to the current Area.
		/// </summary>
		/// <param name="content"></param>
		/// <param name="elementSettings"></param>
		 void Label(string content, ElementSettings elementSettings);
		/// <summary>
		/// Adds a new Label to the current Area.
		/// </summary>
		/// <param name="content"></param>
		/// <param name="elementSettings"></param>
		/// <param name="style"></param>
		 void Label(string content, ElementSettings elementSettings, GUIStyle style);

		/// <summary>
		/// Adds a new Paragraph to the current Area.
		/// </summary>
		/// <param name="content"></param>
		void Paragraph(string content);
		/// <summary>
		/// Adds a new Paragraph to the current Area.
		/// </summary>
		/// <param name="content"></param>
		/// <param name="style">Label style</param>
		void Paragraph(string content, GUIStyle style);
		/// <summary>
		/// Adds a new Paragraph to the current Area.
		/// </summary>
		/// <param name="content"></param>
		/// <param name="elementSettings"></param>
		void Paragraph(string content, ElementSettings elementSettings);
		/// <summary>
		/// Adds a new Paragraph to the current Area.
		/// </summary>
		/// <param name="content"></param>
		/// <param name="elementSettings"></param>
		/// <param name="style"></param>
		void Paragraph(string content, ElementSettings elementSettings, GUIStyle style);
		



		/// <summary>
		/// Adds a new Labeled Field to the current Area.
		/// </summary>
		/// <param name="content">Label content</param>
		/// <param name="field"></param>
		/// 
		void LabeledField(string content, VoltageElement field);
		/// <summary>
		/// Adds a new Labeled Field to the current Area.
		/// </summary>
		/// <param name="content">Label content</param>
		/// <param name="style">Label style</param>
		/// <param name="field"></param>
		 void LabeledField(string content, GUIStyle style, VoltageElement field);
		#endregion

		#region ScrollArea
		/// <summary>
		/// Adds a ScrollArea to the current Area.
		/// </summary>
		/// <param name="scrollArea"></param>
		 void ScrollAreaStart(ScrollArea scrollArea);

		#endregion

		#region SplitArea
		/// <summary>
		/// Adds a SplitArea to the current area
		/// </summary>
		/// <param name="splitArea"></param>
		 void SplitAreaStart(SplitArea splitArea);
		/// <summary>
		/// Makes the second area of the active SplitArea active for adding elements.
		/// </summary>
		 void SplitCurrentArea();
		#endregion

		#region TabArea
		/// <summary>
		/// Adds a TabArea to the current area
		/// </summary>
		/// <param name="tabArea"></param>
		 void TabAreaStart(TabArea tabArea);
		/// <summary>
		/// Makes the next tab of the active TabArea active for adding elements.
		/// </summary>
		 void NextTab();

		#endregion

		#region FoldoutArea
		/// <summary>
		/// Adds a FoldoutArea to the current area
		/// </summary>
		/// <param name="foldoutArea"></param>
		 void FoldoutAreaStart(FoldoutArea foldoutArea);
		#endregion

		#region WeightArea
		/// <summary>
		/// Adds a WeightArea to the current area.
		/// </summary>
		/// <param name="weightArea"></param>
		 void WeightAreaStart(WeightArea weightArea);
		/// <summary>
		/// Adds a new WeightArea to the current area.
		/// </summary>
		void WeightAreaStart();
		/// <summary>
		/// Adds a new WeightArea to the current area.
		/// </summary>
		void WeightAreaStart(GUIStyle style);
		/// <summary>
		/// Adds a new WeightArea to the current area.
		/// </summary>
		/// <param name="elementSettings"></param>
		void WeightAreaStart(ElementSettings elementSettings);
		/// <summary>
		/// Adds a new WeightArea to the current area.
		/// </summary>
		/// <param name="areaSettings"></param>
		void WeightAreaStart(AreaSettings areaSettings);
		/// <summary>
		/// Adds a new WeightArea to the current area.
		/// </summary>
		/// <param name="areaSettings"></param>
		/// <param name="style"></param>
		void WeightAreaStart(AreaSettings areaSettings, GUIStyle style);
		/// <summary>
		/// Adds a new WeightArea to the current area.
		/// </summary>
		/// <param name="elementSettings"></param>
		/// <param name="style"></param>
		void WeightAreaStart(ElementSettings elementSettings, GUIStyle style);

		/// <summary>
		/// Adds a new WeightArea to the current area.
		/// </summary>
		/// <param name="elementSettings"></param>
		/// <param name="areaSettings"></param>
		void WeightAreaStart(ElementSettings elementSettings, AreaSettings areaSettings);
		/// <summary>
		/// Adds a new WeightArea to the current area.
		/// </summary>
		/// <param name="elementSettings"></param>
		/// <param name="areaSettings"></param>
		/// <param name="style"></param>
		void WeightAreaStart(ElementSettings elementSettings, AreaSettings areaSettings, GUIStyle style);
		#endregion

		#region StreamArea
		/// <summary>
		/// Adds a StreamArea to the current area.
		/// </summary>
		/// <param name="streamArea"></param>
		void StreamAreaStart(StreamArea streamArea);

		/// <summary>
		/// Adds a new StreamArea to the current area.
		/// </summary>
		void StreamAreaStart();
		/// <summary>
		/// Adds a new StreamArea to the current area.
		/// </summary>
		/// <param name="style"></param>
		void StreamAreaStart(GUIStyle style);
		/// <summary>
		/// Adds a new StreamArea to the current area.
		/// </summary>
		/// <param name="elementSettings"></param>
		void StreamAreaStart(ElementSettings elementSettings);
		/// <summary>
		/// Adds a new StreamArea to the current area.
		/// </summary>
		/// <param name="areaSettings"></param>
		void StreamAreaStart(AreaSettings areaSettings);
		/// <summary>
		/// Adds a new StreamArea to the current area.
		/// </summary>
		/// <param name="elementSettings"></param>
		/// <param name="style"></param>
		void StreamAreaStart(ElementSettings elementSettings, GUIStyle style);
		/// <summary>
		/// Adds a new StreamArea to the current area.
		/// </summary>
		/// <param name="areaSettings"></param>
		/// <param name="style"></param>
		void StreamAreaStart(AreaSettings areaSettings, GUIStyle style);
		/// <summary>
		/// Adds a new StreamArea to the current area.
		/// </summary>
		/// <param name="elementSettings"></param>
		/// <param name="areaSettings"></param>
		void StreamAreaStart(ElementSettings elementSettings, AreaSettings areaSettings);
		/// <summary>
		/// Adds a new StreamArea to the current area.
		/// </summary>
		/// <param name="elementSettings"></param>
		/// <param name="areaSettings"></param>
		/// <param name="style"></param>
		void StreamAreaStart(ElementSettings elementSettings, AreaSettings areaSettings, GUIStyle style);
		#endregion
		/// <summary>
		/// Closes the current area and makes the last area active for adding elements.
		/// </summary>
		void EndArea();

		void Image(VoltageImage image);
		void Image(VoltageImage image, VoltageElementAlignment alignment);
		/// <summary>
		/// Adds an existing Field to the current area.
		/// </summary>
		/// <param name="element"></param>
		void Field(VoltageElement element);
		/// <summary>
		/// Adds an existing Field to the current area.
		/// </summary>
		/// <param name="element"></param>
		/// <param name="weight"></param>
		void Field(VoltageElement element, int weight);

	}
}

