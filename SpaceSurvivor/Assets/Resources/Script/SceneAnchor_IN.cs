using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneAnchor_IN : MonoBehaviour
{
    [SerializeField] public SceneField SceneToManage;

    [HideInInspector] public GameObject target;
    [HideInInspector] private AsyncOperation chargementNiveau;

    void Start()
    {
        target = Player_controler.Instance.gameObject; 
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            LoadScene(); 
        }
    }


    private void LoadScene()
    {
        bool isSceneLoaded = false;
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene loadedScene = SceneManager.GetSceneAt(i);
            if (loadedScene.name == SceneToManage.SceneName)
            {
                isSceneLoaded = true;
                break;
            }
        }

        if (!isSceneLoaded)
        {
            chargementNiveau = SceneManager.LoadSceneAsync(SceneToManage.SceneName,LoadSceneMode.Additive);
            //StartCoroutine(UpdateScan());
        }
    }


    /*IEnumerator UpdateScan()
    {
        // Attendre la fin du chargement du niveau avant de scanner
        while (!chargementNiveau.isDone)
        {
            yield return null;
        }

        // Lancer le scan asynchrone
        var scanTask = AstarPath.active.ScanAsync();

        // S'assurer que le scan est bien terminé
        yield return scanTask;

        Debug.Log("Scan completed");
    }*/
}
