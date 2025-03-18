using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voltage
{

	public abstract class VoltageHelper
	{
		private VoltageInternalConstructor m_Constructor;
		protected IConstructor Constructor
		{
			get { return m_Constructor; }
		}

		public VoltageHelper()
		{
			m_Constructor = new VoltageInternalConstructor();
			VoltageInit();
		}
		public void BuildWild(VoltageArea targetArea, Action helperMethod)
		{
			m_Constructor.StartWildConstructor(targetArea);
			helperMethod();
			m_Constructor.EndAllAreas();
		}
		public void BuildStored(VoltageArea targetArea, Action helperMethod){
			Constructor.StartStoredConstructor(targetArea);
			helperMethod();
			Constructor.EndStoredConstructor();
		}
		protected abstract void VoltageInit();
	}
}
