using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class maskScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().material.renderQueue = 3002;      
    }

  
}
