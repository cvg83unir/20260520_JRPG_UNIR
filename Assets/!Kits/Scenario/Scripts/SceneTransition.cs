using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private string sceneToLoad;

    [Header("Events")]
    [SerializeField] private UnityEvent onBeforeSceneLoad;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        onBeforeSceneLoad?.Invoke();

        SceneManager.LoadScene(sceneToLoad);
    }
}
