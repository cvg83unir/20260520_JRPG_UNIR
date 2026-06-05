using UnityEngine;
using UnityEngine.Events;

public class HurtCollider : MonoBehaviour
{
    public UnityEvent<string> onHitReceive;


    internal void NotifyHit(HitCollider hitCollider)
    {
        onHitReceive.Invoke(hitCollider.tag);
    }


}
