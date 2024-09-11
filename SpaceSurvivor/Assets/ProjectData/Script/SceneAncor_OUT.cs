using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneAncor_OUT : MonoBehaviour
{
    [SerializeField] public SceneField SceneToManage;
    
    [HideInInspector] public GameObject target;
    
    void Start()
    {
        target = Player_controler.Instance.gameObject; 
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            UnLoadScene();
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
}
