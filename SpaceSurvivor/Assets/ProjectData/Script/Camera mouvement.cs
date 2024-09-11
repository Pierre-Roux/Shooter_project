using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class Cameramouvement : MonoBehaviour
{

    [SerializeField] public float distanceFactor; 
    [SerializeField] public float smoothTime; 
    [SerializeField] public float snapDistance;

    [HideInInspector] private GameObject player;
    [HideInInspector] private Vector3 velocity = Vector3.zero;
    [HideInInspector] private Vector2 targetPosition;
    [HideInInspector] private bool Snaped;

    // Start is called before the first frame update
    void Start()
    {
        player = Player_controler.Instance.gameObject; 
        targetPosition = player.transform.position;
        distanceFactor = 0.2f; 
        smoothTime  = 0.2f;
        snapDistance = 100f;
        Snaped = true;
    }

    void Update()
    {
        //////////////////////////////////////// En commentaire le système de caméra qui s'avance vers la cible du joeur /////////////////////////////////////////////////////
        /*if (Snaped)
        {
            if (Input.GetMouseButton(0))
            {
                Snaped = false;
            }
            else
            {*/
                transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
        /*    }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                // Calculer le point médian ajusté entre la position du joueur et celle de la souris
                targetPosition = (Vector2)player.transform.position + (mousePosition - (Vector2)player.transform.position) * distanceFactor;
                //transform.position = Vector3.SmoothDamp(transform.position, new Vector3(targetPosition.x, targetPosition.y, -10), ref velocity, smoothTime);
                transform.DOMove(new Vector3(targetPosition.x, targetPosition.y, -10), smoothTime);
            }
            else
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                targetPosition = player.transform.position;
                //transform.position = Vector3.SmoothDamp(transform.position, new Vector3(targetPosition.x, targetPosition.y, -10), ref velocity, smoothTime/2);
                transform.DOMove(new Vector3(targetPosition.x, targetPosition.y, -10), smoothTime);
                if (Vector2.Distance(transform.position, targetPosition) <= snapDistance)
                {
                    Snaped = true;
                }
            }

            //Debug.Log($"Camera Position: {transform.position}, Target Position: {targetPosition}, Velocity: {velocity}");
        }*/
    }
}
