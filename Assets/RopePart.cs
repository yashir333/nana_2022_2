using System.Collections;	      //How to use this Rope : Drag and Drop the Rope Prefab.                   //
using System.Collections.Generic;     // If You want a longer Rope - insert the Prefab into the End of the Rope //
using UnityEngine;		      // and uncheck Root checkbox in the head of the new part of the Rope      //
				      // ---------------------------------------------------------------------- //
public class RopePart : MonoBehaviour // Unity Rope by Dim So Vik youtube.com/channel/UCdA9FJp35MytWLg_gxBgj5A  //
{				      // If You Are Interested - I can make a Rope Creator where You can choice //
	public Transform a;	      // whether You want the rope to be cheap looking or expensive,            //
	public Transform b;           // Line Renderer vs Mesh Renderer. The trick is to make it work without   //
	public bool root;             // Actual mesh. Although, I can make a Real Mesh Rope tutorial. Thanks!   //
	public bool customMaterial;   // ---------------------------------------------------------------------- //

    void Start()
    {
	if(GetComponent<LineRenderer>())
	{       if(!customMaterial) GetComponent<LineRenderer>().material = transform.root.GetComponent<LineRenderer>().material;
		if(!root) {	 Vector3 temp = transform.localPosition; temp.y = -2; transform.localPosition = temp;
				 gameObject.AddComponent<CharacterJoint>();
				 GetComponent<CharacterJoint>().connectedBody = transform.parent.gameObject.GetComponent<Rigidbody>();
				 GetComponent<CharacterJoint>().enableProjection = true;
        			 transform.parent = null;
			  } else GetComponent<Rigidbody>().isKinematic = true;
				 GetComponent<LineRenderer>().useWorldSpace = true;
	}
    }

    void Update()
    {
	if(GetComponent<LineRenderer>())
	{	if(a && b)
			{
				GetComponent<LineRenderer>().SetPosition(0, a.position);
				GetComponent<LineRenderer>().SetPosition(1, b.position);
			}
	}
    }
}