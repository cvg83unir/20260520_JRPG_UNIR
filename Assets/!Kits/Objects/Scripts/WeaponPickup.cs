using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] private WeaponType weaponType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        PlayerWeaponManager weaponManager = collision.GetComponent<PlayerWeaponManager>();

        if (weaponManager == null) return;

        if (weaponType == WeaponType.Sword)
            { weaponManager.UnlockSword();  }
        else if (weaponType == WeaponType.MagicProjectile)
            { weaponManager.UnlockMagicProjectile(); }

        Destroy(gameObject);
    }
}
