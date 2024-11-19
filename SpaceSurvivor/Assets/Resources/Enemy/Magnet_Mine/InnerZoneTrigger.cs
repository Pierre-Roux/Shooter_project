using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnerZoneTrigger : MonoBehaviour
{
    [SerializeField] public MagnetMine Mine;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Mine.OnEnterInnerZone(other);
        }
    }
}
