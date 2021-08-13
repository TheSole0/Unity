using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingLine : MonoBehaviour
{
    public Transform Target;
    public GameObject viewModel = null;

    // Update is called once per frame
    void Update()
    {
        float distance = 100.0f;

        Debug.DrawLine(this.transform.position, this.transform.position
                       + this.transform.forward * distance, Color.red, 0);
        viewModel.transform.LookAt(Target);

        
    }
    
}


