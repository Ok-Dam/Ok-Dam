using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voltage;
using UnityEditor;

public class DemoAreas : VoltageWindow
{
	//Declare your areas
	TabArea mainTabs;
	VoltageLink guideButton;

	ScrollArea weightScroll;
	ScrollArea streamScroll;
	ScrollArea splitScroll;
	ScrollArea scrollScroll;
	ScrollArea foldoutScroll;
	ScrollArea tabScroll;

	SplitArea splitDemoHorizontal;
	SplitArea splitDemoVertical;
	SplitArea splitNestedParent;
	SplitArea splitNestedChild;

	ScrollArea scrollDemo;

	FoldoutArea foldoutHorizontal;
	FoldoutArea foldoutVertical;

	TabArea tabDemo;

	[MenuItem("Voltage/Demos/Demo Areas")]
	public static void Init()
	{
		VoltageWindow window = VoltageWindow.GetWindow<DemoAreas>("Demo Areas");
		window.Show();
	}
	protected override void VoltageInit()
	{
		// Initialize the main Tab Area
		mainTabs = new TabArea(new ElementSettings(true,1));

		guideButton = new VoltageLink("Voltage Quickstart Guide", "https://docs.google.com/document/d/1M_0fuK4Fa3a34AShCwT5dQsaEa8_RXXfHBKKeGo7APs/edit#heading=h.cfj271qn228f");

		// Add tabs to the Main Tab Area
		mainTabs.AddTab("Weight Area");
		mainTabs.AddTab("Stream Area");
		mainTabs.AddTab("Split Area");
		mainTabs.AddTab("Scroll Area");
		mainTabs.AddTab("Foldout Area");
		mainTabs.AddTab("Tab Area");

		// Initialize the Body Scroll Area
		weightScroll = new ScrollArea();
		streamScroll = new ScrollArea();
		splitScroll = new ScrollArea();
		scrollScroll = new ScrollArea();
		foldoutScroll = new ScrollArea();
		tabScroll = new ScrollArea();

		// Initialize the Demos
		splitDemoHorizontal = new SplitArea(0.4f, new ElementSettings(new Vector2(0f, 160f)), new AreaSettings(true, new RectOffset(6, 6, 6, 6)), Styles.GetStyle("Simplebox Purple"));
		splitDemoVertical = new SplitArea(0.4f, new ElementSettings(new Vector2(0f, 160f)), new AreaSettings(false, new RectOffset(6,6,6,6)), Styles.GetStyle("Simplebox Purple"));
		
		splitNestedParent = new SplitArea(0.4f, new ElementSettings(new Vector2(0f, 160f)), new AreaSettings(true, new RectOffset(6, 6, 6, 6)), Styles.GetStyle("Simplebox Purple"));
		splitNestedChild = new SplitArea(0.4f, new AreaSettings(false, new RectOffset(6,6,6,6)), Styles.GetStyle("Simplebox Purple"));

		scrollDemo = new ScrollArea(new ElementSettings(new Vector2(200f, 160f)), new AreaSettings(false, new RectOffset(6, 6, 6, 6)), Styles.GetStyle("Simplebox Purple"));

		
		foldoutHorizontal = new FoldoutArea("Foldout", new AreaSettings(true, new RectOffset(6, 6, 6, 6)));
		foldoutVertical = new FoldoutArea("Foldout", new AreaSettings(false, new RectOffset(6, 6, 6, 6)));

		tabDemo = new TabArea(new AreaSettings(true, new RectOffset(6, 6, 6, 6)));
		tabDemo.AddTab("Left");
		tabDemo.AddTab("Middle");
		tabDemo.AddTab("Right");
	}

	protected override void VoltageGUI()
	{
		Constructor.StreamAreaStart();

		{
			// Start your mainTabs
			Constructor.TabAreaStart(mainTabs);
			{
				Constructor.ScrollAreaStart(weightScroll);
				{
					Constructor.StreamAreaStart(new AreaSettings(false, new RectOffset(4, 4, 4, 4), 4f));
					{
						Constructor.Label("Description", Styles.GetStyle("Title"));


						Constructor.Paragraph("Weight areas work by distributing their available space between all their elements according to their weight. For example, an element with a weight of 2 will be twice as large as an element with a weight of 1.");

						Constructor.Label("Layout", Styles.GetStyle("Title"));
						Constructor.Label("Horizontal", Styles.GetStyle("Subtitle"));
						Constructor.WeightAreaStart(new ElementSettings(new Vector2(0f, 160f)), new AreaSettings(true, new RectOffset(6, 6, 6, 6), 4f), Styles.GetStyle("Simplebox Purple"));
						{
							Constructor.Label("Weight 2", new ElementSettings(2), Styles.GetStyle("Simplefill Blue"));
							Constructor.Label("Weight 2", new ElementSettings(2), Styles.GetStyle("Simplefill Blue"));
							Constructor.Label("Weight 4", new ElementSettings(4), Styles.GetStyle("Simplefill Blue Large"));
							Constructor.Label("Weight 1", new ElementSettings(1), Styles.GetStyle("Simplefill Blue Small"));
						}
						Constructor.EndArea();
						Constructor.Label("Vertical", Styles.GetStyle("Subtitle"));
						Constructor.WeightAreaStart(new ElementSettings(new Vector2(0f, 160f)), new AreaSettings(false, new RectOffset(6, 6, 6, 6), 4f), Styles.GetStyle("Simplebox Purple"));
						{
							Constructor.Label("Weight 2", new ElementSettings(2), Styles.GetStyle("Simplefill Blue"));
							Constructor.Label("Weight 2", new ElementSettings(2), Styles.GetStyle("Simplefill Blue"));
							Constructor.Label("Weight 4", new ElementSettings(4), Styles.GetStyle("Simplefill Blue Large"));
							Constructor.Label("Weight 1", new ElementSettings(1), Styles.GetStyle("Simplefill Blue Small"));
						}
						Constructor.EndArea();
					}
					Constructor.EndArea();
				}
				Constructor.EndArea();
			}// Weight Areas Demo Tab (TabAreas always start with the first tab ready for use)


			Constructor.NextTab();
			// Stream Areas Demo Tab
			{
				Constructor.ScrollAreaStart(streamScroll);
				{
					Constructor.StreamAreaStart(new AreaSettings(false, new RectOffset(4, 4, 4, 4), 4f));
					{


						Constructor.Label("Description", Styles.GetStyle("Title"));

						Constructor.Paragraph("Stream areas organize their elements by fitting them one next to the other with each element’s minimum size. Areas will be fitted too, so there is no need to keep adding stream areas. Stream areas may end up with free space, and they can also overflow out of their assigned area (the overflow won’t be visible and out of bounds). To show the overflowed content wrap the stream area inside a scroll area.");

						Constructor.Label("Layout", Styles.GetStyle("Title"));
						Constructor.Label("Horizontal", Styles.GetStyle("Subtitle"));
						Constructor.StreamAreaStart(new ElementSettings(new Vector2(0f, 160f)), new AreaSettings(true, new RectOffset(6, 6, 6, 6), 4f), Styles.GetStyle("Simplebox Purple"));
						{
							Constructor.Label("Element 1", Styles.GetStyle("Simplefill Blue"));
							Constructor.Label("Element 2", Styles.GetStyle("Simplefill Blue"));
							Constructor.Label("Element 3", Styles.GetStyle("Simplefill Blue Large"));
							Constructor.Label("Element 4", Styles.GetStyle("Simplefill Blue Small"));
						}
						Constructor.EndArea();
						Constructor.Label("Vertical", Styles.GetStyle("Subtitle"));
						Constructor.StreamAreaStart(new ElementSettings(new Vector2(0f, 160f)), new AreaSettings(false, new RectOffset(6, 6, 6, 6), 4f), Styles.GetStyle("Simplebox Purple"));
						{
							Constructor.Label("Element 1", Styles.GetStyle("Simplefill Blue"));
							Constructor.Label("Element 2", Styles.GetStyle("Simplefill Blue"));
							Constructor.Label("Element 3", Styles.GetStyle("Simplefill Blue Large"));
							Constructor.Label("Element 4", Styles.GetStyle("Simplefill Blue Small"));
						}
						Constructor.EndArea();

						Constructor.Label("Flex", Styles.GetStyle("Title"));
						Constructor.Paragraph("The free space on a stream area can be used. If you add fields or elements with their flex propertie set to true, they will share that space in much the same way they would share a weight area.\nYou can find an elements flex propertie on it's element settings");

						Constructor.Label("Horizontal Flex", Styles.GetStyle("Subtitle"));
						Constructor.StreamAreaStart(new ElementSettings(new Vector2(0f, 160f)), new AreaSettings(true, new RectOffset(6, 6, 6, 6), 4f), Styles.GetStyle("Simplebox Purple"));
						{
							Constructor.Label("Element 1", Styles.GetStyle("Simplefill Blue"));
							Constructor.Label("Element 2", Styles.GetStyle("Simplefill Blue"));
							Constructor.Label("Element 3", Styles.GetStyle("Simplefill Blue Large"));
							Constructor.Label("Element 4", Styles.GetStyle("Simplefill Blue Small"));

							// The element itself must have it's flex set to true, in this example it's assigned on instantiation by passing 
							// new ElementSettings(flex, weight)
							// to it's constructor
							Constructor.Label("Flex Space", new ElementSettings(true, 1), Styles.GetStyle("Simplefill Green Large"));
						}
						Constructor.EndArea();


						Constructor.Label("Vertical Flex", Styles.GetStyle("Subtitle"));
						Constructor.StreamAreaStart(new ElementSettings(new Vector2(0f, 160f)), new AreaSettings(false, new RectOffset(6, 6, 6, 6), 4f), Styles.GetStyle("Simplebox Purple"));
						{
							Constructor.Label("Element 1", Styles.GetStyle("Simplefill Blue"));
							Constructor.Label("Element 2", Styles.GetStyle("Simplefill Blue"));
							Constructor.Label("Element 3", Styles.GetStyle("Simplefill Blue Large"));
							Constructor.Label("Element 4", Styles.GetStyle("Simplefill Blue Small"));
							Constructor.Label("Flex Space", new ElementSettings(true, 1), Styles.GetStyle("Simplefill Green Large"));
						}
						Constructor.EndArea();

						Constructor.Label("Advanced Flex", Styles.GetStyle("Title"));
						Constructor.Paragraph("Flex space does not need to be used all on the same place. You might find usefull to know you can have flex elements mixed in between regular elements.");

						Constructor.Label("Advanced Horizontal Flex", Styles.GetStyle("Subtitle"));
						Constructor.StreamAreaStart(new ElementSettings(new Vector2(0f, 160f)), new AreaSettings(true, new RectOffset(6, 6, 6, 6), 4f), Styles.GetStyle("Simplebox Purple"));
						{
							Constructor.Label("Element 1", Styles.GetStyle("Simplefill Blue"));

							// By setting this elements flex to true and it's weight to 2, it will take twice the free space the other flex element will take
							Constructor.Label("Flex 2", new ElementSettings(true, 2), Styles.GetStyle("Simplefill Green Large"));
							Constructor.Label("Element 2", Styles.GetStyle("Simplefill Blue"));
							Constructor.Label("Element 3", Styles.GetStyle("Simplefill Blue Large"));

							// By setting this elements flex to true and it's weight to 1, it will take half the free space the other flex element will take
							Constructor.Label("Flex 1", new ElementSettings(true, 1), Styles.GetStyle("Simplefill Green Large"));

							Constructor.Label("Element 4", Styles.GetStyle("Simplefill Blue Small"));
						}
						Constructor.EndArea();

						Constructor.Label("Advanced Vertical Flex", Styles.GetStyle("Subtitle"));
						Constructor.StreamAreaStart(new ElementSettings(new Vector2(0f, 160f)), new AreaSettings(false, new RectOffset(6, 6, 6, 6), 4f), Styles.GetStyle("Simplebox Purple"));
						{
							Constructor.Label("Element 1", Styles.GetStyle("Simplefill Blue"));
							// By setting this elements flex to true and it's weight to 2, it will take twice the free space the other flex element will take
							Constructor.Label("Flex 2", new ElementSettings(true, 2), Styles.GetStyle("Simplefill Green Large"));
							Constructor.Label("Element 2", Styles.GetStyle("Simplefill Blue"));
							Constructor.Label("Element 3", Styles.GetStyle("Simplefill Blue Large"));
							// By setting this elements flex to true and it's weight to 1, it will take half the free space the other flex element will take
							Constructor.Label("Flex 1", new ElementSettings(true, 1), Styles.GetStyle("Simplefill Green Large"));
							Constructor.Label("Element 4", Styles.GetStyle("Simplefill Blue Small"));
						}
						Constructor.EndArea();
					}
					Constructor.EndArea();
				}
				Constructor.EndArea();
			}

			Constructor.NextTab();
			// Split Areas Demo Tab
			{

				Constructor.ScrollAreaStart(splitScroll);
				{


					Constructor.StreamAreaStart(new AreaSettings(false, new RectOffset(4, 4, 4, 4), 4f));
					{

						Constructor.Label("Description", Styles.GetStyle("Title"));

						Constructor.Paragraph("Split area lets you define an area divided in two, which division can be resized. Both sides of the split will start as horizontal weight areas, to override them just add another area inside of them before you start adding elements.");

						Constructor.Label("Layout", Styles.GetStyle("Title"));

						Constructor.Label("Horizontal", Styles.GetStyle("Subtitle"));
						Constructor.SplitAreaStart(splitDemoHorizontal);
						{
							Constructor.Label("Section 1", Styles.GetStyle("Simplefill Blue Large"));
						}
						// Go to second section
						Constructor.SplitCurrentArea();
						{
							Constructor.Label("Section 2", Styles.GetStyle("Simplefill Blue Large"));
						}
						Constructor.EndArea();


						Constructor.Label("Vertical", Styles.GetStyle("Subtitle"));
						Constructor.SplitAreaStart(splitDemoVertical);
						{
							Constructor.Label("Section 1", Styles.GetStyle("Simplefill Blue Large"));
						}
						// Go to second section
						Constructor.SplitCurrentArea();
						{
							Constructor.Label("Section 2", Styles.GetStyle("Simplefill Blue Large"));
						}
						Constructor.EndArea();

						Constructor.Label("Nested", Styles.GetStyle("Subtitle"));
						Constructor.SplitAreaStart(splitNestedParent);
						{
							Constructor.Label("Section 1", Styles.GetStyle("Simplefill Blue Large"));
						}
						// Go to second section
						Constructor.SplitCurrentArea();
						{
							Constructor.SplitAreaStart(splitNestedChild);
							{
								Constructor.Label("Section 1", Styles.GetStyle("Simplefill Green Large"));
							}
							Constructor.SplitCurrentArea();
							{
								Constructor.Label("Section 2", Styles.GetStyle("Simplefill Green Large"));
							}
							Constructor.EndArea();
						}
						Constructor.EndArea();
					}
					Constructor.EndArea();
				}
				Constructor.EndArea();
			}

			Constructor.NextTab();
			// Scroll Area Demo Tab
			{
				Constructor.ScrollAreaStart(scrollScroll);
				{
					Constructor.StreamAreaStart(new AreaSettings(false, new RectOffset(4, 4, 4, 4), 4f));
					{
						Constructor.Label("Description", Styles.GetStyle("Title"));


						Constructor.Paragraph("Scroll areas are used to wrap other areas or elements and provide scrollbars in the case that the size of it’s content is larger than the size the scroll area is assigned to.", Styles.GetStyle("Paragraph"));

						Constructor.Label("Layout", Styles.GetStyle("Title"));
						Constructor.Paragraph("Scroll areas don't have horizontal/vertical modes.");
						Constructor.ScrollAreaStart(scrollDemo);
						{
							Constructor.WeightAreaStart(new ElementSettings(new Vector2(600f, 600f)), new AreaSettings(false, new RectOffset(6, 6, 6, 6), 4f));
							{
								Constructor.Label("Content", Styles.GetStyle("Simplefill Green Large"));

							}
							Constructor.EndArea();
						}
						Constructor.EndArea();

					}
					Constructor.EndArea();
				}
				Constructor.EndArea();
			}
			Constructor.NextTab();
			// Foldout Area Demo Tab
			{
				Constructor.ScrollAreaStart(foldoutScroll);
				{
					Constructor.StreamAreaStart(new AreaSettings(false, new RectOffset(4, 4, 4, 4), 4f));
					{
						Constructor.Label("Description", Styles.GetStyle("Title"));


						Constructor.Paragraph("Foldout areas are collapsible sections. They allow you to separate your elements into discrete sections, and allow the user to show or hide them.");

						Constructor.Label("Layout", Styles.GetStyle("Title"));
						Constructor.Label("Horizontal", Styles.GetStyle("Subtitle"));
						Constructor.FoldoutAreaStart(foldoutHorizontal);
						{
							Constructor.StreamAreaStart(new AreaSettings(false, new RectOffset(4, 4, 4, 4), 4f));
							{
								Constructor.Label("Content", new ElementSettings(new Vector2(0f, 160f)), Styles.GetStyle("Simplefill Blue Large"));
							}
							Constructor.EndArea();
						}
						Constructor.EndArea();

						Constructor.Label("Vertical", Styles.GetStyle("Subtitle"));
						Constructor.FoldoutAreaStart(foldoutVertical);
						{
							Constructor.StreamAreaStart(new AreaSettings(false, new RectOffset(4, 4, 4, 4), 4f));
							{
								Constructor.Label("Content", new ElementSettings(new Vector2(0f, 160f)), Styles.GetStyle("Simplefill Blue Large"));
							}
							Constructor.EndArea();
						}
						Constructor.EndArea();
					}
					Constructor.EndArea();
				}
				Constructor.EndArea();
			}
			Constructor.NextTab();
			// Tab Area Demo Tab
			{
				Constructor.ScrollAreaStart(tabScroll);
				{
					Constructor.StreamAreaStart(new AreaSettings(false, new RectOffset(4, 4, 4, 4), 4f));
					{
						Constructor.Label("Description", Styles.GetStyle("Title"));


						Constructor.Paragraph("Foldout areas are collapsible sections. They allow you to separate your elements into discrete sections, and allow the user to show or hide them.");

						Constructor.Label("Layout", Styles.GetStyle("Title"));
						Constructor.Paragraph("Tab areas don't have horizontal/vertical modes.");
						Constructor.TabAreaStart(tabDemo);
						{
							Constructor.Label("Content Left tab", new ElementSettings(new Vector2(0f, 160f)), Styles.GetStyle("Simplefill Green Large"));
						}
						Constructor.NextTab();
						{
							Constructor.Label("Content Middle tab", new ElementSettings(new Vector2(0f, 160f)), Styles.GetStyle("Simplefill Blue Large"));
						}
						Constructor.NextTab();
						{
							Constructor.Label("Content Right tab", new ElementSettings(new Vector2(0f, 160f)), Styles.GetStyle("Simplefill Purple Large"));
						}
						Constructor.EndArea();
					}
					Constructor.EndArea();
				}
				Constructor.EndArea();
			}
			Constructor.EndArea();

			Constructor.StreamAreaStart(new AreaSettings(true,new RectOffset(0,0,4,3),0f,VoltageElementAlignment.Center),Styles.GetStyle("Section Bottom"));
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
