﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenAlternate : MonoBehaviour
{
    // Start is called before the first frame update
    float distance = 3;
    ParticleSystem myTrail;
    void Start()
    {
        myTrail = gameObject.GetComponent<ParticleSystem>();
        myTrail.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        ObjectFollowCursor();
    }

    private void ObjectFollowCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 point = ray.origin + (ray.direction * distance);
        gameObject.transform.position = point;
    }

    private void OnMouseDown()
    {
        myTrail.Play();
    }

    private void OnMouseUp()
    {
        myTrail.Stop();
    }
}
