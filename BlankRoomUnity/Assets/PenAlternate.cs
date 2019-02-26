using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenAlternate : MonoBehaviour
{
    // Start is called before the first frame update
    float distance = 5;
    ParticleSystem myTrail;
    public Transform uvLocation;
   // Vector3 uvWorldPosition;
    public GameObject renderPen;
    ParticleSystem renderPenParticles;
    public Camera myCanvasCam;
    public RenderTexture renderTexture;
    void Start()
    {
        
        
        myTrail = gameObject.GetComponent<ParticleSystem>();
      
        renderPenParticles = renderPen.GetComponent<ParticleSystem>();
       
        canEmit(false);
    }

    // Update is called once per frame
    void Update()
    {
        
        ObjectFollowCursor();
        if (Input.GetMouseButton(0))
        {
            DoAction();
        } else if (Input.GetMouseButtonUp(0))
        {
            canEmit(false);
        }
    }

    private void ObjectFollowCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 point = ray.origin + (ray.direction * distance);
        gameObject.transform.position = point;
    }

 

    void DoAction()
    {
        
        Vector3 uvWorldPosition = Vector3.zero;
        canEmit(true);
        if (HitTestUVPosition(ref uvWorldPosition))
        {
            GameObject brush;
            brush = (GameObject)Instantiate(Resources.Load("PaintBrushSprite"));
            //renderPen.transform.localPosition = new Vector3(-uvWorldPosition.x, -uvWorldPosition.y);
            //brush.transform.parent = brushContainer.transform; //Add the brush to our container to be wiped later
            brush.transform.localPosition = uvWorldPosition; //The position of the brush (in the UVMap)
            brush.transform.localScale = Vector3.one;//The size of the brush


        } else
        {
            Debug.LogError("Hit failed");
        }
    }


    bool HitTestUVPosition(ref Vector3 uvWorldPosition)
    {
        RaycastHit hit;
        Vector3 mousePos = Input.mousePosition;
        Vector3 cursorPos = new Vector3(mousePos.x, mousePos.y, 0.0f);

        Ray cursorRay = Camera.main.ScreenPointToRay(cursorPos);
        if(Physics.Raycast(cursorRay, out hit, 1000))
        {
            MeshCollider meshCollider = hit.collider as MeshCollider;
            if(meshCollider == null || meshCollider.sharedMesh == null)
            {
                return false;
            }
            Vector2 pixelUV = new Vector2(hit.textureCoord.x, hit.textureCoord.y);
            //Debug.Log(pixelUV);
            uvWorldPosition.x = pixelUV.x*10 - myCanvasCam.orthographicSize;
            uvWorldPosition.y = pixelUV.y*10  - myCanvasCam.orthographicSize;
            uvWorldPosition.z = -2.0f;
            
            return true;
        }
        else
        {
            return false;
        }
    }

    void canEmit(bool truefalse)
    {
        //myTrail.enableEmission = truefalse;
       // renderPenParticles.enableEmission = truefalse;
    }


}
