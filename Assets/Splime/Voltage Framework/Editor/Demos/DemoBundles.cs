using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Voltage;


public class DemoBundles : VoltageWindow {

	VoltageLink guideButton;


	

	VoltageText volText;
	VoltageNumeric volInt;
	VoltageNumericRange volFloatRange;


	VoltageText pixText;
	VoltageNumeric pixInt;
	VoltageNumericRange pixFloatRange;

	VoltageText flaText;
	VoltageNumeric flaInt;
	VoltageNumericRange flaFloatRange;

	[MenuItem("Voltage/Demos/Demo Bundles")]
	public static void Init()
	{
		DemoBundles window = DemoBundles.GetWindow<DemoBundles>("Demo Bundles");
		window.Show();
	}

	protected override void VoltageInit()
	{

		guideButton = new VoltageLink("Voltage Quickstart Guide", "https://docs.google.com/document/d/1M_0fuK4Fa3a34AShCwT5dQsaEa8_RXXfHBKKeGo7APs/edit#heading=h.cfj271qn228f");

		volText = new VoltageText("Textfield");
		volInt = new VoltageNumeric(0);
		volFloatRange = new VoltageNumericRange(0f, 0f, 10f);

		pixText = new VoltageText("Textfield",Styles.GetStyle("MyPixelBundle", "Textfield"));
		pixInt = new VoltageNumeric(0, Styles.GetStyle("MyPixelBundle", "Textfield"), Styles.GetStyle("MyPixelBundle", "Minus"), Styles.GetStyle("MyPixelBundle", "Plus"));
		pixFloatRange = new VoltageNumericRange(0f, 0f, 10f);
		pixFloatRange.Style = Styles.GetStyle("MyPixelBundle", "Textfield");
		pixFloatRange.StyleMinus = Styles.GetStyle("MyPixelBundle", "Minus");
		pixFloatRange.StylePlus = Styles.GetStyle("MyPixelBundle", "Plus");
		pixFloatRange.StyleSlider = Styles.GetStyle("MyPixelBundle", "SliderBar");
		pixFloatRange.StyleThumb = Styles.GetStyle("MyPixelBundle", "SliderThumb");

		flaText = new VoltageText("Textfield", Styles.GetStyle("MyFlatBundle", "Textfield"));
		flaInt = new VoltageNumeric(0, Styles.GetStyle("MyFlatBundle", "Textfield"), Styles.GetStyle("MyFlatBundle", "Minus"), Styles.GetStyle("MyFlatBundle", "Plus"));
		flaFloatRange = new VoltageNumericRange(0f, 0f, 10f);
		flaFloatRange.Style = Styles.GetStyle("MyFlatBundle", "Textfield");
		flaFloatRange.StyleMinus = Styles.GetStyle("MyFlatBundle", "Minus");
		flaFloatRange.StylePlus = Styles.GetStyle("MyFlatBundle", "Plus");
		flaFloatRange.StyleSlider = Styles.GetStyle("MyFlatBundle", "SliderBar");
		flaFloatRange.StyleThumb = Styles.GetStyle("MyFlatBundle", "SliderThumb");
	}

	protected override void VoltageGUI()
	{
		Constructor.StreamAreaStart();
		{
			Constructor.WeightAreaStart(new ElementSettings(true, 1), new AreaSettings(true, new RectOffset(10, 10, 5, 10), 10f));
			{
				Constructor.StreamAreaStart(new AreaSettings(false, 5f));
				{
					Constructor.Label("Voltage Styles", Styles.GetStyle("Title"));
					Constructor.Field(new VoltageButton("Button", null));
					Constructor.Field(volText);
					Constructor.Field(volInt);
					Constructor.Field(volFloatRange);
				}
				Constructor.EndArea();
				Constructor.StreamAreaStart(new AreaSettings(false, 5f));
				{
					Constructor.Label("My Pixel Bundle", Styles.GetStyle("MyPixelBundle", "Title"));
					Constructor.Field(new VoltageButton("BUTTON", null, Styles.GetStyle("MyPixelBundle", "Button")));
					Constructor.Field(pixText);
					Constructor.Field(pixInt);
					Constructor.Field(pixFloatRange);
				}
				Constructor.EndArea();
				Constructor.StreamAreaStart(new AreaSettings(false, 5f));
				{
					Constructor.Label("My Flat Bundle", Styles.GetStyle("MyFlatBundle", "Title"));
					Constructor.Field(new VoltageButton("BUTTON", null, Styles.GetStyle("MyFlatBundle", "Button")));
					Constructor.Field(flaText);
					Constructor.Field(flaInt);
					Constructor.Field(flaFloatRange);
				}
				Constructor.EndArea();
			}
			Constructor.EndArea();

			Constructor.StreamAreaStart(new AreaSettings(true, new RectOffset(0, 0, 4, 3), 0f, VoltageElementAlignment.Center), Styles.GetStyle("Section Bottom"));
			{
				Constructor.Label("Be sure to check the ");
				Constructor.Field(guideButton);

			}
			Constructor.EndArea();
		}
		Constructor.EndArea();

	}

	protected override void VoltageSerialization()
	{

	}
}
