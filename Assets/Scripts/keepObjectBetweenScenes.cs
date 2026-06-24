using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class keepObjectBetweenScenes : MonoBehaviour
{

    static GameObject[] gameobjectsSaved= new GameObject[4];
    public int id = 0;
    private void Awake()
    {
        if (CompareTag("Player"))
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
            if (gameobjectsSaved[id] == null)
        {
            gameobjectsSaved[id] = gameObject;
        }
        else if (gameobjectsSaved[id] != gameObject)
        {
            
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        

    }
    private void OnDestroy()
    {
        Debug.LogError("Me dessubscribo y soy el player falso");
        SceneManager.sceneLoaded -= OnSceneLoaded;
        gameObject.GetComponent<PlayerControl>()?.UnSubscribe();
        gameobjectsSaved[0].GetComponent<PlayerControl>().Subscribe();
    }

    private void OnSceneLoaded(Scene sc, LoadSceneMode arg1)
    {
        
        if(sc.buildIndex == 0)
        {
            CollectiveSuicide();
            return;
        }
        Debug.Log("escena cargada");
        if (CompareTag("Player") && gameobjectsSaved[id] == gameObject)
        {
            GameObject spawn = GameObject.FindGameObjectWithTag("Spawn");
            if (spawn)
            {
                Debug.Log("spawn detectado, cargando posicion");
                transform.position = spawn.transform.position;
            }
            else
            {
                Debug.Log("spawn no detectado, redirigiendo al centro");
                transform.position = Vector2.zero;
            }
            //Debug.LogError("Me dessubscribo y soy el player verdadero al cargar escena");
            //gameObject.GetComponent<PlayerControl>().UnSubscribe();
            Debug.LogError("Me subscribo y soy el player verdadero al cargar escena");
            gameObject.GetComponent<PlayerControl>().Subscribe();
        }

    }

    private void CollectiveSuicide()
    {
        
        foreach (GameObject go in gameobjectsSaved)
        {
            if (go != gameObject && go) Destroy(go);
        }
        
        
        Destroy(gameObject);
    }

    
}
