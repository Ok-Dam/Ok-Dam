using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voltage
{
	public class VoltageMessageBox : VoltageElement
	{
		private string m_message = "";
		private MessageType m_messageType = MessageType.None;
		public string Message
		{
			get{ return m_message; }
			set{ m_message = value; }
		}
		public MessageType MessageType
		{
			get { return m_messageType; }
			set { m_messageType = value; }
		}

		#region CONSTRUCTORS
		public VoltageMessageBox(string message)
		{
			Message = message;
		}

		public VoltageMessageBox(string message, MessageType messageType) : this(message)
		{
			MessageType = messageType;
		}

		public VoltageMessageBox(string message, ElementSettings elementSettings) : this(message)
		{
			ElementSettings = elementSettings;
		}

		public VoltageMessageBox(string message, MessageType messageType, ElementSettings elementSettings) : this(message, messageType)
		{
			ElementSettings = elementSettings;
		}
		#endregion

		public override float CalcHeight(float width)
		{
			return EditorStyles.helpBox.CalcHeight(new GUIContent(Message),width) + 16f;
		}
		/// <summary>
		/// Do not use this.
		/// </summary>
		/// <param name="workingArea"></param>
		public override void DrawElement(Rect _workingArea)
		{
			base.DrawElement(_workingArea);

			EditorGUI.HelpBox(WorkingArea, Message, MessageType);
		}
	}
}
