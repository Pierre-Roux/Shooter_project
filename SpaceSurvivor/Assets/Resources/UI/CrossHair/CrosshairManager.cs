using UnityEngine;

public class CrosshairManager : MonoBehaviour
{

    [HideInInspector] private Vector2 mousePosition;

    void Start()
    {
        Cursor.visible = false; // Cache le curseur natif
        Cursor.lockState = CursorLockMode.Confined; // Empêche le curseur de quitter la fenêtre du jeu
    }
    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePosition;
    }
}