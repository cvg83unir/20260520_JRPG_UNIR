using UnityEngine;

public class BattleTrigger : MonoBehaviour
{
    [Header("BattleZone")]
    [SerializeField] private BattleZone battleZone;

    private bool activated;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (activated) return;

        if (collision.CompareTag("Player"))
        {
            activated = true;

            if (battleZone != null)
                { battleZone.StartBattle(); }

            gameObject.SetActive(false);
        }
    }
}
