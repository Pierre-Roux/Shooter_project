using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneAnchor : MonoBehaviour
{
    public float loadDistance;
    public float unloadDistance;
    public SceneField SceneToManage;
    [HideInInspector] public GameObject target;
    AsyncOperation chargementNiveau;

    void Start()
    {
        target = Player_controler.Instance.gameObject; 
    }

    private void OnDrawGizmos()
    {
        // Couleur du gizmo pour la distance de chargement
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, loadDistance);

        // Couleur du gizmo pour la distance de déchargement
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, unloadDistance);
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, target.transform.position) <= loadDistance)
        {
            LoadScene();
        } 
        else if (Vector2.Distance(transform.position, target.transform.position) >= unloadDistance)
        {
            UnLoadScene();
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
            StartCoroutine(UpdateScan());
        }
    }

    private void UnLoadScene()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene loadedScene = SceneManager.GetSceneAt(i);
            if (loadedScene.name == SceneToManage.SceneName)
            {
                SceneManager.UnloadSceneAsync(SceneToManage.SceneName);
            }
        }
    }

    IEnumerator UpdateScan()
    {
        // Attendre que la scène soit complètement chargée
        yield return new WaitUntil(() => chargementNiveau.isDone);

        // Scanner la nouvelle grille
        Debug.Log("Scanning new grid...");
        AstarPath.active.ScanAsync();
    }
}