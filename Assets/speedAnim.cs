using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speedAnim : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Animator>().speed = Random.Range(1f, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
