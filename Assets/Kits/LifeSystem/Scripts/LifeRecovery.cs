using UnityEngine;
using UnityEngine.Events;

public class LifeRecovery : MonoBehaviour
{
    public UnityEvent<string> onLifeRecovery;


    internal void NotifyHealthRecovery(HeartCollider heartCollider)
    {
        onLifeRecovery.Invoke(heartCollider.tag);
    }

}
