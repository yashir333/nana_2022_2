﻿using UnityEngine;
using System.Collections;
namespace Puppet3D
{
	[ExecuteInEditMode]
	public class IKFKBlend : MonoBehaviour
	{
		[Range(0f, 1f)]
		public float IKFK = 1f;
		public Transform IK = null;
		public Transform FK = null;
		public Vector3 PositionOffset = Vector3.zero;
		public Quaternion RotationOffset = Quaternion.identity;
		public bool ContrstrainPosition = false;
		public enum IKFKType { ArmL, ArmR, LegL, LegR, Body };
		public IKFKType GroupID = IKFKType.Body;

		public Vector3 Pos;
		public float L;

		public Transform ConstrainedControl;
		[SerializeField]
		private Vector3 _constrainedControlPosOffset =Vector3.zero;
		[SerializeField]
		private Quaternion _constrainedControlRotOffset = Quaternion.identity;
		[SerializeField]
		private Quaternion _constrainedControlRotOffsetDif = Quaternion.identity;
		
		private Transform GlobalControlTrans
		{
			get { return GlobalControl.transform; }
		}

		[HideInInspector]
		public Transform Handle = null;
		[HideInInspector]
		public GlobalControl GlobalControl;
		
		public bool OverrideWithRig = false;
		private Quaternion _initialRotation = Quaternion.identity;

		public void Init()
		{
			if (ConstrainedControl != null)
			{
				_constrainedControlPosOffset = ConstrainedControl.position;
				_constrainedControlRotOffset = ConstrainedControl.rotation;

				_constrainedControlRotOffsetDif = Quaternion.Inverse(FK.rotation) * _constrainedControlRotOffset;

			}

			L = 0f;
			Run();
			if (FK.parent)
			{
				L = Vector3.Distance(FK.position, FK.parent.position);
			}
			_initialRotation = transform.localRotation;
		}
			
		public void Run()
		{			
			if (FK != null)
			{
				if (IK != null)
				{
					Quaternion ikRot = IK.rotation * RotationOffset;
					if (ContrstrainPosition)
						transform.position = Vector3.Lerp(IK.position, FK.position, IKFK);
					transform.rotation = Quaternion.Lerp(ikRot, FK.rotation, IKFK);
				}
				else
				{
					if(OverrideWithRig)
						Quaternion.Lerp(_initialRotation, FK.localRotation, IKFK);
					else
						transform.localRotation = FK.localRotation;
				}
			}
			
		}
		public void RunConstrianedControls()
		{
			if (ConstrainedControl != null)
			{
				if (GlobalControl == null) return;
				
				ConstrainedControl.position = Vector3.Lerp(GlobalControlTrans.TransformPoint(_constrainedControlPosOffset), FK.position, IKFK);
				ConstrainedControl.rotation = Quaternion.Lerp(GlobalControlTrans.rotation*_constrainedControlRotOffset, FK.rotation * _constrainedControlRotOffsetDif, IKFK);
			}
		}
	}
}
