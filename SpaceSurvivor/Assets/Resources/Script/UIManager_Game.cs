using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
[Header("Other")] 
    [SerializeField] public GameObject endPanel;

    [HideInInspector] public GameObject Player;

    void Start()
    {
        Player = Player_controler.Instance.gameObject; 
    }

    void Update()
    {
        if (Player.GetComponent<Player_controler>().playerMort && !endPanel.activeSelf)
        {
            StartCoroutine(DeathUI());
        }
    }

    public void MainMenu()
    {
        Time.timeScale = 1f; // Unpause the game
        SceneManager.LoadScene("Main_Menu"); // Replace with your main menu scene name
    }

    public void Retry()
    {
        Time.timeScale = 1f; // Unpause the game
        SceneManager.LoadScene("Persitant_Scene"); // Reload the current scene
    }

    IEnumerator DeathUI()
    {
        yield return new WaitForSeconds(3f);
        endPanel.SetActive(true);
        Time.timeScale = 0f;
    }
}
