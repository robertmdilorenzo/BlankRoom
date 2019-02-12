using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenTool : MonoBehaviour
{
    // Start is called before the first frame update
    

    private Vector3 offset;
    float penZ;
    void Start()
    {
        //gameObject.transform.position = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        penZ = Camera.main.ScreenToViewportPoint(gameObject.transform.position).z;
        offset = gameObject.transform.position - GetMouseAsWorldPoint();

    }

    // Update is called once per frame
    void Update()
    {
      
        gameObject.transform.position = GetMouseAsWorldPoint() + offset;
         

    }

    private Vector3 GetMouseAsWorldPoint()

    {

        // Pixel coordinates of mouse (x,y)

        Vector3 mousePoint = -Input.mousePosition;



        // z coordinate of game object on screen

        mousePoint.z = penZ;



        // Convert it to world points

        return Camera.main.ScreenToWorldPoint(mousePoint);

    }

    private void OnMouseDown()
    {
        gameObject.GetComponent<TrailRenderer>().enabled = true;
    }

    private void OnMouseUp()
    {
        gameObject.GetComponent<TrailRenderer>().enabled = false;
    }


}
