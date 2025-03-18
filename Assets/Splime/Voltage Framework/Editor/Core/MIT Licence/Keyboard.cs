using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MouseButton
{
	Left = 0,
	Right = 1,
	Middle = 2
}
public class Keyboard
{

	public Keyboard()
	{
	}

	public Keyboard(Event evt)
	{
		this.Code = evt.keyCode;
		this.IsAlt = evt.alt;
		this.IsCapsLock = evt.capsLock;
		this.IsControl = evt.control;
		this.IsFunctionKey = evt.functionKey;
		this.IsNumeric = evt.numeric;
		this.IsShift = evt.shift;
		this.Modifiers = evt.modifiers;
	}

	public KeyCode Code { get; set; }

	public bool IsAlt { get; set; }

	public bool IsCapsLock { get; set; }

	public bool IsControl { get; set; }

	public bool IsFunctionKey { get; set; }

	public bool IsNumeric { get; set; }

	public bool IsShift { get; set; }

	public EventModifiers Modifiers { get; set; }
}
