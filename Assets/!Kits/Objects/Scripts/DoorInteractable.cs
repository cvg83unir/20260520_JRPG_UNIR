using UnityEngine;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    [Header("UI")]
    [SerializeField] private GameObject doorCanvas;

    [Header("Door")]
    [SerializeField] private Animator animator;
    [SerializeField] private Collider2D closedCollider;
    [SerializeField] private Collider2D open1Collider;
    [SerializeField] private Collider2D open2Collider;
    [SerializeField] bool startOpened;

    [Header("Access")]
    [SerializeField] private InventoryInfo keyInventoryInfo;
    [SerializeField] private bool block = false;

    private bool opened = false;

    private void Start()
    {
        HidePrompt();

        if (animator == null)
            { animator = GetComponent<Animator>(); }

        if (startOpened)
        {
            opened = true;

            animator.SetTrigger("Open");
            animator.SetBool("IsOpen", true);

            open1Collider.enabled = true;
            open2Collider.enabled = true;
            closedCollider.enabled = false;
        }
        else
        {
            open1Collider.enabled = false;
            open2Collider.enabled = false;
            closedCollider.enabled = true;
        }
    }

    public void Interact()
    {
        if (!doorCanvas.activeSelf) return;

        opened = true;

        HidePrompt();

        animator.SetTrigger("Open");

        open1Collider.enabled = true;
        open2Collider.enabled = true;
        closedCollider.enabled = false;
    }

    public void ShowPrompt()
    {
        if (!opened && !block)
        { 
            if(keyInventoryInfo != null)
            {
                if (!CheckKey(keyInventoryInfo)) return;
            }
            doorCanvas.SetActive(true); 
        }
    }

    public void HidePrompt()
    {
        doorCanvas.SetActive(false);
    }

    public void FinishOpening()
    {
        animator.SetBool("IsOpen", true);
    }
    public bool CheckKey(InventoryInfo keyInInf)
    {
        foreach (InventoryInfo ii in FindObjectsByType<InventoryInfo>(FindObjectsSortMode.None))
        {
            if ((ii.spriteImage == keyInInf.spriteImage) && (ii.infoText == keyInInf.infoText))
            {
                return true;
            }
        }
        return false;
    }

    public void UnlockDoor()
    {
        block = false;
    }
}
