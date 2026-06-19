using UnityEngine;

public class MusicChanger : MonoBehaviour
{
    [SerializeField] private AudioClip music;

    private void Start()
    {
        MusicManager.Instance.ChangeMusic(music);
    }
}
