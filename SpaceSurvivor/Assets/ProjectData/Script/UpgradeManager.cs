using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{

    [HideInInspector] public GameObject Player;

    void Start()
    {
        Player = Player_controler.Instance.gameObject; 
    }

    void Update()
    {
        if (Player.GetComponent<Player_controler>().upgraded)
        {
            foreach (WeaponBase weapon in Player.GetComponent<Player_controler>().weapons)
            {
                weapon.Upgrade();
            }

            Player.GetComponent<Player_controler>().upgraded = false;
        }
    }
}
