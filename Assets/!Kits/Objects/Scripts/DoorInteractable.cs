using UnityEngine;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    [Header("UI")]
    [SerializeField] private GameObject doorCanvas;

    [Header("Door")]
    [SerializeField] private Animator animator;
    [SerializeField] private Collider2D closedCollider;
    [SerializeField] private Collider2D openCollider;

    [Header("Access")]
    [SerializeField] private InventoryInfo keyInventoryInfo;
    [SerializeField] private bool block = false;

    private bool opened = false;

    private void Start()
    {
        HidePrompt();

        openCollider.enabled = false;
        closedCollider.enabled = true;

        if (animator == null)
            { animator = GetComponent<Animator>(); }
    }

    public void Interact()
    {
        if (!doorCanvas.activeSelf) return;

        opened = true;

        HidePrompt();

        animator.SetTrigger("Open");

        openCollider.enabled = true;
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

    public void unlockDoor()
    {
        block = false;
    }
}
