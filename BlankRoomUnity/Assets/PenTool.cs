using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenTool : MonoBehaviour
{
    // Start is called before the first frame update
    
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = Camera.main.ScreenToViewportPoint(Input.mousePosition);
         

    }
}
