using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouseCube : MonoBehaviour
{
    
    [HideInInspector] private Vector2 mousePosition;

    // Update is called once per frame
    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePosition;
    }
}
