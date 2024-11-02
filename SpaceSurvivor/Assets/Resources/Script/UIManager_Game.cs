using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
[Header("Other")] 
    [SerializeField] public GameObject endPanel;
    [SerializeField] private TMP_Text timerText;

    [HideInInspector] public float gameTimer;
    [HideInInspector] public GameObject Player;

    void Start()
    {
        Player = Player_controler.Instance.gameObject; 
        gameTimer = 0f; 
    }

    void Update()
    {
        if (!Player.GetComponent<Player_controler>().playerMort)
        {
            gameTimer += Time.deltaTime;
            UpdateTimerUI();
        }
        
        if (Player.GetComponent<Player_controler>().playerMort && !endPanel.activeSelf)
        {
            StartCoroutine(DeathUI());
        }
    }

    void UpdateTimerUI()
    {
        // Convertir le temps en minutes et secondes
        int minutes = Mathf.FloorToInt(gameTimer / 60f);
        int seconds = Mathf.FloorToInt(gameTimer % 60f);

        // Afficher le temps dans le format "MM:SS"
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
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
