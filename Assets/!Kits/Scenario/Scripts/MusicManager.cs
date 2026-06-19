using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    public void ChangeMusic(AudioClip newMusic)
    {
        if (audioSource.clip == newMusic) return;

        audioSource.clip = newMusic;
        audioSource.Play();
    }
}
