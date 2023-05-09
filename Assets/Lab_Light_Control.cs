using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class Lab_Light_Control : MonoBehaviour
{
    public bool Switch_OnOff;

    [SerializeField] Celling_lamp[] arr;
    private void Start()
    {
        arr = FindObjectsOfType<Celling_lamp>();
    }

    void Update()
    {
        if (Switch_OnOff)
            TurnOn();
        else
            TurnOff();
    }

    // Start is called before the first frame update
    public void TurnOn() 
    {
       
        foreach (Celling_lamp l in arr)
            l.On_Off(true);
    }

    public void TurnOff()
    {
        foreach (Celling_lamp l in arr)
            l.On_Off(false);
    }
}
