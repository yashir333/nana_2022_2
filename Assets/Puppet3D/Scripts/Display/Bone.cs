﻿using UnityEngine;
using System.Collections;
namespace Puppet3D
{
	[ExecuteInEditMode]
	public class Bone : MonoBehaviour {
		private Mesh _mesh;
		public float Radius = .5f;
		public Color Color = Color.white;
        public bool ShowJoint = true;
		// Use this for initialization
		void Start() {
			string path = "bone";
			_mesh = (Mesh)Resources.Load(path, typeof(Mesh));
		}

		// Update is called once per frame
		void Update() {

		}
		
		private void OnDrawGizmos()
		{
			DrawThisBone(Color);
			
		}
		private void DrawThisBone(Color col)
		{
			if (this.enabled)
			{
				Gizmos.color = col;
				if (_mesh == null)
				{
					string path = "bone";
					_mesh = (Mesh)Resources.Load(path, typeof(Mesh));
				}

				Gizmos.DrawWireSphere(transform.position, Radius);
				foreach (Transform child in transform)
				{
                    Bone bone = child.GetComponent<Bone>();
                    if (bone != null && bone.ShowJoint)
                    {
                        Vector3 nudge = (child.position - transform.position).normalized * Radius;

                        float scaler = Vector3.Distance(child.position - nudge, transform.position + nudge);
                        Vector3 look = child.position - transform.position;
                        Quaternion rot = Quaternion.identity;
                        if (look != Vector3.zero)
                            rot = Quaternion.LookRotation(look);
                        Gizmos.DrawWireMesh(_mesh, 0, transform.position + nudge, rot, new Vector3(Radius, Radius, scaler));
                        //Gizmos.DrawLine(transform.position, transform.position + transform.up*.5f );
                    }
				}
			}
		}
		private void OnDrawGizmosSelected()
		{
			DrawThisBone(new Color(1f, .588f, 0f, 1f));
		}
		private void OnHideGizmos()
		{

		}
	}

}
