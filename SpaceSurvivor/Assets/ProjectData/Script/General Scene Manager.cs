using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GeneralSceneManager : MonoBehaviour
{
    public String GameState;
    public GameObject loadingBar;
    public Image loadingBarSlider;
    public GameObject[] menuObjects;
    private List<AsyncOperation> sceneToLoad = new List<AsyncOperation>();
    public SceneField Persistant_Scene;

    // Start is called before the first frame update
    void Start()
    {
        loadingBar.SetActive(false);
    }

    private void HideMenu()
    {
        for (int i = 0; i < menuObjects.Length; i++)
        {
            menuObjects[i].SetActive(false);
        }
    }

    public void StartGame()
    {
        HideMenu();

        loadingBar.SetActive(true);

        sceneToLoad.Add(SceneManager.LoadSceneAsync(Persistant_Scene, LoadSceneMode.Additive));

        StartCoroutine(ProgressLoadingBar());

    }

    private IEnumerator ProgressLoadingBar()
    {
        float totalProgress = 0f;

        for (int i = 0; i < sceneToLoad.Count; i++)
        {
            while (!sceneToLoad[i].isDone)
            {
                totalProgress += sceneToLoad[i].progress;
                loadingBarSlider.fillAmount = totalProgress / sceneToLoad.Count;
                yield return null;
            }
        }

        // Décharger la scène Main_Menu une fois que tout est chargé
        SceneManager.UnloadSceneAsync("Main_Menu");

        // Cache la barre de chargement une fois terminé
        loadingBar.SetActive(false);
    }
}
