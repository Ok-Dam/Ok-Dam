using UnityEngine;
using Voltage;
using UnityEditor;


public class DemoFields : VoltageWindow
{
	ScrollArea scrollMain;

	VoltageLink guideButton;

	// Text Fields
	VoltageRichTextArea voltageRichTextArea;

	// Label
	VoltageMessageBox classicMessageBox;

	VoltageMessageBox voltageMessageBox;


	// Boolean Fields
	VoltageToggle classicToggle;

	VoltageToggle voltageToggle;
	VoltageSwitch voltageSwitch;
	VoltageLabelSwitch voltageLabelSwitch;
	VoltagePluralSwitch voltagePluralSwitch;

	// Option Fields
	VoltagePopup classicPopup;
	VoltageFlags classicFlags;

	VoltagePopup voltagePopup;
	VoltageOptions voltageOptions;
	VoltageFlags voltageFlags;
	VoltageOptionsSwitch voltageOptionsSwitch;

	// Numeric Fields
	VoltageNumeric classicIntFloat;
	VoltageNumericRange classicIntFloatRange;
	VoltagePluralNumeric classicVector;

	VoltageNumeric voltageIntFloat;
	VoltageNumericRange voltageIntFloatRange;
	VoltagePluralNumeric voltagePluralNumericVector;
	VoltagePluralNumeric voltagePluralNumeric;
	VoltageMultinumeric voltageMultinumeric;

	[MenuItem("Voltage/Demos/Demo Fields")]
	public static void Init()
	{
		DemoFields window = VoltageWindow.GetWindow<DemoFields>("Demo Fields");
		window.Show();
	}

	protected override void VoltageInit()
	{
		scrollMain = new ScrollArea(new ElementSettings(true));
		guideButton = new VoltageLink("Voltage Quickstart Guide", "https://docs.google.com/document/d/1M_0fuK4Fa3a34AShCwT5dQsaEa8_RXXfHBKKeGo7APs/edit#heading=h.cfj271qn228f");


		// Text
		voltageRichTextArea = new VoltageRichTextArea("<b><color=#49bdff>Rich\nText\nField</color></b>");

		// Labels
		classicMessageBox = new VoltageMessageBox("Message Box", MessageType.Info);

		voltageMessageBox = new VoltageMessageBox("Message Box", MessageType.Info);

		// Booleans
		classicToggle = new VoltageToggle(true);

		voltageToggle = new VoltageToggle(true);
		voltageSwitch = new VoltageSwitch(true);
		voltageLabelSwitch = new VoltageLabelSwitch(true,"Follow Target");
		voltagePluralSwitch = new VoltagePluralSwitch(new bool[4] { true, false, false, true }, new string[4] { "x", "y", "z", "Lock Rot" });

		// Options
		classicFlags = new VoltageFlags(0, new string[] { "Flag 1", "Flag 2", "Flag 3" });
		classicPopup = new VoltagePopup(0, new string[] { "Option 1", "Option 2", "Option 3" });

		voltageFlags = new VoltageFlags(0, new string[] { "Flag 1", "Flag 2", "Flag 3" });
		voltageOptions = new VoltageOptions(0, new string[] { "Option 1", "Option 2", "Option 3"});
		voltagePopup = new VoltagePopup(0, new string[] { "Option 1", "Option 2", "Option 3" });
		voltageOptionsSwitch = new VoltageOptionsSwitch(0, new string[] { "Option 1", "Option 2", "Option 3"});

		// Numeric
		classicIntFloat = new VoltageNumeric(3f,0.5f);
		classicIntFloatRange = new VoltageNumericRange(2.3f, 0f, 5f);
		classicVector = new VoltagePluralNumeric(Vector3.one);

		voltageIntFloat = new VoltageNumeric(3f, 0.5f);
		voltageIntFloatRange = new VoltageNumericRange(2.3f, 0f, 5f);
		voltagePluralNumericVector = new VoltagePluralNumeric(Vector3.one);
		voltagePluralNumeric = new VoltagePluralNumeric(new float[] { 1f,1f,1f,1f,1f,1f});
		voltageMultinumeric = new VoltageMultinumeric(new float[] { 0.5f, 2f, 1f, 0.1f, 5f },new string[] { "X","Y","Z", "Step","Max"});
	}

	protected override void VoltageGUI()
	{
		Constructor.StreamAreaStart();
		{
			// Main Scroll
			Constructor.ScrollAreaStart(scrollMain);
			{
				// General Stream
				Constructor.StreamAreaStart(new AreaSettings(false, new RectOffset(4, 4, 4, 4), 2f));
				{
					// Text Elements
					Constructor.Label("Text Fields", Styles.GetStyle("Title Centered"));

					// Subtitles
					Constructor.WeightAreaStart(new AreaSettings(true, new RectOffset(2, 2, 0, 0), 8f));
					{
						// Classic
						Constructor.Label("Classic", new ElementSettings(5), Styles.GetStyle("Subtitle Centered"));

						// Voltage
						Constructor.Label("Voltage", new ElementSettings(6), Styles.GetStyle("Subtitle Centered"));
					}
					Constructor.EndArea();

					Constructor.WeightAreaStart(new AreaSettings(true, new RectOffset(2, 2, 2, 2), 8f));
					{
						// Classic
						Constructor.StreamAreaStart(new ElementSettings(5), new AreaSettings(false, new RectOffset(8, 8, 8, 8), 6f), Styles.GetStyle("Simplebox Dark"));
						{
							Constructor.LabeledField("Text Field", new VoltageText("Text Field"));
							Constructor.LabeledField("Text Area Field", new VoltageTextArea("Text\nArea\nField"));
							Constructor.LabeledField("Password Field", new VoltagePassword("Password"));
						}
						Constructor.EndArea();

						// Voltage
						Constructor.StreamAreaStart(new ElementSettings(6), new AreaSettings(false, new RectOffset(8, 8, 8, 8), 6f), Styles.GetStyle("Simplebox Dark Blue"));
						{
							Constructor.LabeledField("Text Field", new VoltageText("Text Field"));
							Constructor.LabeledField("Text Area Field", new VoltageTextArea("Text\nArea\nField"));
							Constructor.LabeledField("Password Field", new VoltagePassword("Password"));
							Constructor.LabeledField("Rich Text Field", voltageRichTextArea);
						}
						Constructor.EndArea();
					}
					Constructor.EndArea();

					// Text Elements
					Constructor.Label("Label Fields", Styles.GetStyle("Title Centered"));

					// Subtitles
					Constructor.WeightAreaStart(new AreaSettings(true, new RectOffset(2, 2, 0, 0), 8f));
					{
						// Classic
						Constructor.Label("Classic", new ElementSettings(5), Styles.GetStyle("Subtitle Centered"));

						// Voltage
						Constructor.Label("Voltage", new ElementSettings(6), Styles.GetStyle("Subtitle Centered"));
					}
					Constructor.EndArea();

					Constructor.WeightAreaStart(new AreaSettings(true, new RectOffset(2, 2, 2, 2), 8f));
					{
						// Classic
						Constructor.StreamAreaStart(new ElementSettings(5), new AreaSettings(false, new RectOffset(8, 8, 8, 8), 6f), Styles.GetStyle("Simplebox Dark"));
						{
							Constructor.LabeledField("Label", new VoltageLabel("Label"));
							Constructor.LabeledField("Message Box", classicMessageBox);
						}
						Constructor.EndArea();

						// Voltage
						Constructor.StreamAreaStart(new ElementSettings(6), new AreaSettings(false, new RectOffset(8, 8, 8, 8), 6f), Styles.GetStyle("Simplebox Dark Blue"));
						{
							Constructor.LabeledField("Label", new VoltageLabel("Label"));
							Constructor.LabeledField("Message Box", voltageMessageBox);
							Constructor.LabeledField("Paragraph", new VoltageParagraph("<b>Bold</b> <i>Italic</i> <color=#fff>Color</color>\n<color=#9fcd48><b><i>Lorem ipsum dolor sit amet consectetur adipiscing elit ad placerat faucibus, ante mi luctus est quis phasellus volutpat nullam feugiat viverra accumsan, cursus congue auctor pellentesque magna turpis consequat leo ac.</i></b></color>"));
							//Constructor.LabeledField("Code Snippet", new VoltageParagraph("<b>Bold</b> <i>Italic</i> <color=#fff>Color</color>\n<color=#9fcd48><b><i>Lorem ipsum dolor sit amet consectetur adipiscing elit ad placerat faucibus, ante mi luctus est quis phasellus volutpat nullam feugiat viverra accumsan, cursus congue auctor pellentesque magna turpis consequat leo ac.</i></b></color>"));
						}
						Constructor.EndArea();
					}
					Constructor.EndArea();

					// Boolean Elements
					Constructor.Label("Boolean Fields", Styles.GetStyle("Title Centered"));

					// Subtitles
					Constructor.WeightAreaStart(new AreaSettings(true, new RectOffset(2, 2, 0, 0), 8f));
					{
						// Classic
						Constructor.Label("Classic", new ElementSettings(5), Styles.GetStyle("Subtitle Centered"));

						// Voltage
						Constructor.Label("Voltage", new ElementSettings(6), Styles.GetStyle("Subtitle Centered"));
					}
					Constructor.EndArea();

					Constructor.WeightAreaStart(new AreaSettings(true, new RectOffset(2, 2, 2, 2), 8f));
					{
						// Classic
						Constructor.StreamAreaStart(new ElementSettings(5), new AreaSettings(false, new RectOffset(8, 8, 8, 8), 6f), Styles.GetStyle("Simplebox Dark"));
						{
							Constructor.LabeledField("Toggle Field", Styles.GetStyle("Label"), classicToggle);
						}
						Constructor.EndArea();

						// Voltage
						Constructor.StreamAreaStart(new ElementSettings(6), new AreaSettings(false, new RectOffset(8, 8, 8, 8), 6f), Styles.GetStyle("Simplebox Dark Blue"));
						{
							Constructor.LabeledField("Toggle Field", voltageToggle);
							Constructor.LabeledField("Switch Field", voltageSwitch); 
							Constructor.LabeledField("Label Switch Field", voltageLabelSwitch); 
							Constructor.LabeledField("PluralSwitch Field", voltagePluralSwitch);
						}
						Constructor.EndArea();
					}
					Constructor.EndArea();

					// Option Elements
					Constructor.Label("Option Fields", Styles.GetStyle("Title Centered"));

					// Subtitles
					Constructor.WeightAreaStart(new AreaSettings(true, new RectOffset(2, 2, 0, 0), 8f));
					{
						// Classic
						Constructor.Label("Classic", new ElementSettings(5), Styles.GetStyle("Subtitle Centered"));

						// Voltage
						Constructor.Label("Voltage", new ElementSettings(6), Styles.GetStyle("Subtitle Centered"));
					}
					Constructor.EndArea();

					Constructor.WeightAreaStart(new AreaSettings(true, new RectOffset(2, 2, 2, 2), 8f));
					{
						// Classic
						Constructor.StreamAreaStart(new ElementSettings(5), new AreaSettings(false, new RectOffset(8, 8, 8, 8), 6f), Styles.GetStyle("Simplebox Dark"));
						{
							Constructor.LabeledField("Popup Field", classicPopup);
							Constructor.LabeledField("Flags Field", classicFlags);

						}
						Constructor.EndArea();

						// Voltage
						Constructor.StreamAreaStart(new ElementSettings(6), new AreaSettings(false, new RectOffset(8, 8, 8, 8), 6f), Styles.GetStyle("Simplebox Dark Blue"));
						{
							Constructor.LabeledField("Popup Field", voltagePopup);
							Constructor.LabeledField("Flags Field", voltageFlags);
							Constructor.LabeledField("Options Field", voltageOptions);
							Constructor.LabeledField("Options Switch Field", voltageOptionsSwitch);
						}
						Constructor.EndArea();
					}
					Constructor.EndArea();

					#region Numeric Elements
					Constructor.Label("Numeric Fields", Styles.GetStyle("Title Centered"));

					// Subtitles
					Constructor.WeightAreaStart(new AreaSettings(true, new RectOffset(2, 2, 0, 0), 8f));
					{
						// Classic
						Constructor.Label("Classic", new ElementSettings(5), Styles.GetStyle("Subtitle Centered"));

						// Voltage
						Constructor.Label("Voltage", new ElementSettings(6), Styles.GetStyle("Subtitle Centered"));
					}
					Constructor.EndArea();

					Constructor.WeightAreaStart(new AreaSettings(true, new RectOffset(2, 2, 2, 2), 8f));
					{
						// Classic
						Constructor.StreamAreaStart(new ElementSettings(5), new AreaSettings(false, new RectOffset(8, 8, 8, 8), 6f), Styles.GetStyle("Simplebox Dark"));
						{
							Constructor.LabeledField("Numeric Field", Styles.GetStyle("Label"), classicIntFloat);
							Constructor.LabeledField("Range Field", Styles.GetStyle("Label"), classicIntFloatRange); 
							Constructor.LabeledField("Vector Field", Styles.GetStyle("Label"), classicVector); 
						}
						Constructor.EndArea();

						// Voltage
						Constructor.StreamAreaStart(new ElementSettings(6), new AreaSettings(false, new RectOffset(8, 8, 8, 8), 6f), Styles.GetStyle("Simplebox Dark Blue"));
						{
							Constructor.LabeledField("Numeric Field", voltageIntFloat);
							Constructor.LabeledField("Range Field", voltageIntFloatRange); 
							Constructor.LabeledField("Plural Numeric Vector", voltagePluralNumericVector);
							Constructor.LabeledField("Plural Numeric Array", voltagePluralNumeric);
							Constructor.LabeledField("Multinumeric Field", voltageMultinumeric);
						}
						Constructor.EndArea();
					}
					Constructor.EndArea();
					#endregion
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
