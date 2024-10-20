using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactivationUnit : MonoBehaviour
{
    public EnemyBase enemyToReactivate;  // Référence vers l'ennemi à réactiver

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ReactivationCollider"))  // Le joueur possède un trigger spécifique
        {
            enemyToReactivate.gameObject.SetActive(true);  // Réactiver l'ennemi
            Destroy(gameObject);  // Détruire la ReactivationUnit après réactivation
        }
    }
}