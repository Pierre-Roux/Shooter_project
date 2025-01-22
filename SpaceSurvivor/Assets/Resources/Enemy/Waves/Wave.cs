using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    [SerializeField] public int power;
    [SerializeField] public List<GameObject> dispoEnemy;
    [SerializeField] public GameObject spawnPoint1;
    [SerializeField] public GameObject spawnPoint2;
    [SerializeField] public GameObject spawnPoint3;
    [SerializeField] public GameObject spawnPoint4;
    
    [HideInInspector] private List<GameObject> EnemyToSpawn = new List<GameObject>();


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            SelectUnitToSpawn();
            spawnUnit();
            Destroy(gameObject);
        }
    }

    void SelectUnitToSpawn()
    {
        while (power > 0)
        {
            int randomSelection = UnityEngine.Random.Range(0, dispoEnemy.Count);
            
            if (power - dispoEnemy[randomSelection].GetComponent<EnemyBase>().unitPowerMesure >= 0)
            {
                power -= dispoEnemy[randomSelection].GetComponent<EnemyBase>().unitPowerMesure;
                EnemyToSpawn.Add(dispoEnemy[randomSelection]);
            }
            else
            {
                break; // Si aucun ennemi ne peut être ajouté, sortir de la boucle
            }
        }

        for (int i = 0; i < dispoEnemy.Count; i++)
        {
            if (power - dispoEnemy[i].GetComponent<EnemyBase>().unitPowerMesure >= 0)
            {
                power -= dispoEnemy[i].GetComponent<EnemyBase>().unitPowerMesure;
                EnemyToSpawn.Add(dispoEnemy[i]);
            }         
        }

        for (int i = 0; i < dispoEnemy.Count; i++)
        {
            if (power - dispoEnemy[i].GetComponent<EnemyBase>().unitPowerMesure >= 0)
            {
                power -= dispoEnemy[i].GetComponent<EnemyBase>().unitPowerMesure;
                EnemyToSpawn.Add(dispoEnemy[i]);
            }         
        }

        for (int i = 0; i < dispoEnemy.Count; i++)
        {
            if (power - dispoEnemy[i].GetComponent<EnemyBase>().unitPowerMesure >= 0)
            {
                power -= dispoEnemy[i].GetComponent<EnemyBase>().unitPowerMesure;
                EnemyToSpawn.Add(dispoEnemy[i]);
            }         
        }
    }

    void spawnUnit()
    {
        while (EnemyToSpawn.Count > 0)
        {
            GameObject Instance;

            int randomSelection = UnityEngine.Random.Range(1, 5);
            switch (randomSelection)
            {
                case 1:
                    Instance = Instantiate(EnemyToSpawn[0], spawnPoint1.transform.position, spawnPoint1.transform.rotation);
                break;
                case 2:
                    Instance = Instantiate(EnemyToSpawn[0], spawnPoint2.transform.position, spawnPoint2.transform.rotation);
                break;
                case 3:
                    Instance = Instantiate(EnemyToSpawn[0], spawnPoint3.transform.position, spawnPoint3.transform.rotation);
                break;
                case 4:
                    Instance = Instantiate(EnemyToSpawn[0], spawnPoint4.transform.position, spawnPoint4.transform.rotation);
                break;
                default:
                    Instance = Instantiate(EnemyToSpawn[0], spawnPoint1.transform.position, spawnPoint1.transform.rotation);
                    break;
            }

            // Retirer l'ennemi instancié de la liste
            EnemyToSpawn.RemoveAt(0);
        }
    }

    void OnDrawGizmos()
    {
        // Configure la couleur du Gizmo
        Gizmos.color = Color.cyan;
        // Dessine un cercle pour représenter la distance de spawn
        Gizmos.DrawWireSphere(transform.position, GetComponent<CircleCollider2D>().radius);
    }

}
