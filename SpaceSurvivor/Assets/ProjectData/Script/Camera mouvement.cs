using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cameramouvement : MonoBehaviour
{
    GameObject player;
    public float distanceFactor; 
    public float smoothTime; 
    public float snapDistance;

    private Vector3 velocity = Vector3.zero;
    private Vector2 targetPosition;
    private bool Snaped;

    // Start is called before the first frame update
    void Start()
    {
        player = Player_controler.Instance.gameObject; 
        targetPosition = player.transform.position;
        distanceFactor = 0.2f; 
        smoothTime  = 0.3f;
        snapDistance = 0.5f;
        Snaped = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Snaped)
        {
            if (Input.GetMouseButton(0))
            {
                Snaped = false;
            }
            else
            {
                transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                // Calculer le point médian ajusté entre la position du joueur et celle de la souris
                targetPosition = (Vector2)player.transform.position + (mousePosition - (Vector2)player.transform.position) * distanceFactor;
                transform.position = Vector3.SmoothDamp(transform.position, new Vector3(targetPosition.x, targetPosition.y, -10), ref velocity, smoothTime);
            }
            else
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                targetPosition = player.transform.position;
                transform.position = Vector3.SmoothDamp(transform.position, new Vector3(targetPosition.x, targetPosition.y, -10), ref velocity, smoothTime/2);
                if (Vector2.Distance(transform.position, targetPosition) <= snapDistance)
                {
                    Snaped = true;
                }
            }

            Debug.Log($"Camera Position: {transform.position}, Target Position: {targetPosition}, Velocity: {velocity}");
        }
    }
}
