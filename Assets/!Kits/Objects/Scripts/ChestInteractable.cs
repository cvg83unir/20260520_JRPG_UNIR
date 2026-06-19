using System.Collections;
using UnityEngine;

public class ChestInteractable : MonoBehaviour, IInteractable
{
    [Header("UI")]
    [SerializeField] private GameObject chestCanvas;

    [Header("Chest")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D chestCollider;

    [Header("Reward")]
    [SerializeField] private GameObject rewardPrefab;
    [SerializeField] private Transform rewardSpawnPoint;

    [Header("Times")]
    [SerializeField] private float timeBeforeReward = 0.5f;
    [SerializeField] private float fadeDuration = 0.8f;

    private bool isOpen = false;

    private void Start()
    {
        HidePrompt();

        if (chestCollider == null)
            { chestCollider = GetComponent<Collider2D>(); }

        if (spriteRenderer == null)
            { spriteRenderer = GetComponentInChildren<SpriteRenderer>(); }
    }

    public void Interact()
    {
        if (isOpen) return;

        isOpen = true;
        HidePrompt();

        if (chestCollider != null)
            { chestCollider.enabled = false; }

        StartCoroutine(OpenChestRoutine());
    }

    public void ShowPrompt()
    {
        if (isOpen) return;

        if (chestCanvas != null)
            { chestCanvas.SetActive(true); }
    }

    public void HidePrompt()
    {
        if (chestCanvas != null)
            { chestCanvas.SetActive(false); }
    }

    private IEnumerator OpenChestRoutine()
    {
        if (animator != null)
            { animator.SetTrigger("Open"); }

        yield return new WaitForSeconds(timeBeforeReward);

        SpawnReward();

        yield return StartCoroutine(FadeAndDestroy());
    }

    private void SpawnReward()
    {
        if (rewardPrefab == null) return;

        Vector3 spawnPosition = transform.position;

        if (rewardSpawnPoint != null)
            { spawnPosition = rewardSpawnPoint.position; }

        Instantiate(rewardPrefab, spawnPosition, Quaternion.identity);
    }

    private IEnumerator FadeAndDestroy()
    {
        if (spriteRenderer == null)
        {
            Destroy(gameObject);
            yield break;
        }

        float timer = 0f;
        Color startColor = spriteRenderer.color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            spriteRenderer.color = new Color(
                startColor.r,
                startColor.g,
                startColor.b,
                alpha
            );

            yield return null;
        }

        Destroy(gameObject);
    }
}
