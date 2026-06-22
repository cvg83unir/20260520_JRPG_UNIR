using UnityEngine;

public class keepObjectBetweenScenes : MonoBehaviour
{

    static GameObject[] gameobjectsSaved= new GameObject[4];
    public int id = 0;
    private void Awake()
    {
        if(gameobjectsSaved[id] == null)
        {
            gameobjectsSaved[id] = gameObject;
        }
        else if (gameobjectsSaved[id] != gameObject)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        if (gameObject.CompareTag("Player"))
        {
            SpawnScript spawn = FindAnyObjectByType<SpawnScript>();
            if (spawn)
            {
                transform.position = spawn.transform.position;
            }
            else
            {
                transform.position = Vector2.zero;
            }
        }
        

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
