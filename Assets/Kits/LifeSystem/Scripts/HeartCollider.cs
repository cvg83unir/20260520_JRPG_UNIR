using UnityEngine;

public class HeartCollider : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        if (elOtro.CompareTag("Player"))
        {
            LifeRecovery lr = elOtro.GetComponent<LifeRecovery>();

            lr?.NotifyHealthRecovery(this);
            Destroy(this.gameObject);
        }
    }
}
