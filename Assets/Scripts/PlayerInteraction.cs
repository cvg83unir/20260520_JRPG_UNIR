using TMPro;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Detection")]
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private float interactionRadius = 1f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private LayerMask obstacleLayer;

    [Header("Player Control")]
    [SerializeField] private PlayerControl playerControl;

    private IInteractable currentInteractable;

    private void Update()
    {
        CheckInteractable();

        if (currentInteractable != null && playerControl.ConsumeInteract())
        {
            currentInteractable.Interact();
        }
    }

    private void CheckInteractable()
    {
        Collider2D hit = Physics2D.OverlapCircle(
            interactionPoint.position,
            interactionRadius,
            interactableLayer
        );

        if (hit == null)
        {
            ClearCurrentInteractable();
            return;
        }

        if (!CanSeeInteractable(hit))
        {
            ClearCurrentInteractable();
            return;
        }

        if (hit.TryGetComponent(out IInteractable interactable))
        {
            if (currentInteractable != interactable)
            {
                ClearCurrentInteractable();

                currentInteractable = interactable;
                currentInteractable.ShowPrompt();
            }
        }
        else
        {
            ClearCurrentInteractable();
        }
    }

    private void ClearCurrentInteractable()
    {
        if (currentInteractable != null)
        {
            currentInteractable.HidePrompt();
            currentInteractable = null;
        }
    }

    private bool CanSeeInteractable(Collider2D hit)
    {
        Vector2 origin = interactionPoint.position;
        Vector2 destination = hit.bounds.center;
        Vector2 direction = destination - origin;
        float distance = direction.magnitude;

        RaycastHit2D wallHit = Physics2D.Raycast(
            origin,
            direction.normalized,
            distance,
            obstacleLayer
        );

        return wallHit.collider == null;
    }

    private void OnDrawGizmosSelected()
    {
        if (interactionPoint == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionPoint.position, interactionRadius);
    }
}
