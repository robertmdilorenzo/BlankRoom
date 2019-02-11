using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenTool : MonoBehaviour
{
    // Start is called before the first frame update
    public float mouseSensitivity;

    private Vector3 offset;
    float penZ;
    void Start()
    {
        penZ = Camera.main.ScreenToViewportPoint(gameObject.transform.position).z;
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, penZ));

    }

    // Update is called once per frame
    void Update()
    {
        offset = gameObject.transform.position - GetMouseAsWorldPoint();
        gameObject.transform.position = GetMouseAsWorldPoint() + offset;
         

    }

    private Vector3 GetMouseAsWorldPoint()

    {

        // Pixel coordinates of mouse (x,y)

        Vector3 mousePoint = Input.mousePosition;



        // z coordinate of game object on screen

        mousePoint.z = penZ;



        // Convert it to world points

        return Camera.main.ScreenToWorldPoint(mousePoint);

    }


}
