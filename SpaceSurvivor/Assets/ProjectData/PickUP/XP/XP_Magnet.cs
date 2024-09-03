using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XP_Magnet : MonoBehaviour
{
    public float attractionRadius; 
    public float attractionSpeed;   
    public GameObject player;

    void OnTriggerStay2D(Collider2D other)
    {
        // Vérifier si l'objet touché est un objet XP
        if (other.CompareTag("XP"))
        {
            // Attraction de l'objet XP vers le joueur
            other.transform.position = Vector3.Lerp(other.transform.position, player.transform.position, Time.deltaTime * attractionSpeed);

            float distance = Vector3.Distance(other.transform.position, player.transform.position);
            if (distance < 0.9f)  // Distance d'absorption
            {
                CollectXP(other.gameObject);
            }
        }
    }

    void CollectXP(GameObject xpObject)
    {
        if (xpObject.GetComponent<XP>().type == "Large")
        {
            player.GetComponent<Player_controler>().GainXP(10);
        }
        else if (xpObject.GetComponent<XP>().type == "Medium")
        {
            player.GetComponent<Player_controler>().GainXP(3);
        }
        else if (xpObject.GetComponent<XP>().type == "Small")
        {
            player.GetComponent<Player_controler>().GainXP(1);
        }


        Destroy(xpObject);
    }
}
