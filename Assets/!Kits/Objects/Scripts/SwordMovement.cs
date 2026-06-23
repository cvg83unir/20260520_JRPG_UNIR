using UnityEngine;

public class SwordMovement : MonoBehaviour
{
    Transform target;
    Vector3 offset;

    [SerializeField] float yOffset = 0.2f;

    public void SetTarget(Transform player, Vector3 attackOffset)
    {
        target = player;
        offset = attackOffset;
    }

    void Update()
    {
        if (target != null)
        {
            transform.position = target.position + offset + Vector3.up * yOffset;
        }
    }
}