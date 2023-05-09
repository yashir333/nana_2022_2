


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 //[ExecuteInEditMode]
public class Celling_lamp : MonoBehaviour
{
    public GameObject LightGM;
    public GameObject LightMesh;
    // Start is called before the first frame update
   

    // Update is called once per frame
   


    public void On_Off(bool b)
    {
        if (b)
        {
            LightGM.SetActive(true);
            LightMesh.GetComponent<MeshRenderer>().sharedMaterial.SetFloat("_EmissiveExposureWeight", 0);
         }

        else 
        {
            LightGM.SetActive(false);
            LightMesh.GetComponent<MeshRenderer>().sharedMaterial.SetFloat("_EmissiveExposureWeight", 1);
        }
           
    }

}
