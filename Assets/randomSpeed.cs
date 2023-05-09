using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomSpeed : MonoBehaviour
{
    public float min = 0.1f;
    public float max = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        float sp = Random.Range(min, max);

        Animator compo = gameObject.GetComponent<Animator>();
        compo.speed = sp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
