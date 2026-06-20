using UnityEngine;
using UnityEngine.Events;

public class HurtCollider : MonoBehaviour
{
    public UnityEvent<string> onHitReceive;

    internal void NotifyHit(HitCollider hitCollider)
    {
        if (!enabled) return;

        onHitReceive.Invoke(hitCollider.tag);
    }
}
