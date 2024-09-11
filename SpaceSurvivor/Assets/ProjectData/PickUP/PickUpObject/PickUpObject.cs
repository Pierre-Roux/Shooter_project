using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickUpObject : MonoBehaviour
{
[Header("Param")] 
    [SerializeField] public GameObject objectToInstanciate;
    [HideInInspector] private Transform target;

    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameObject NewObj = Instantiate(objectToInstanciate,target.position ,target.rotation,target.transform);
        }

        target.GetComponent<Player_controler>().UpdateWeapon();

        Destroy(gameObject);
    }
}
