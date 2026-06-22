using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    [Header("Weapons Unlocked")]
    [SerializeField] private bool hasSword = false;
    [SerializeField] private bool hasMagicProjectile = false;

    [Header("Current Weapon")]
    [SerializeField] private WeaponType currentWeapon = WeaponType.None;

    [Header("Sword UI")]
    [SerializeField] private GameObject swordDarkIcon;
    [SerializeField] private GameObject swordColorIcon;

    [Header("Magic UI")]
    [SerializeField] private GameObject magicDarkIcon;
    [SerializeField] private GameObject magicColorIcon;

    private void Start()
    {
        UpdateWeaponUI();
    }

    public WeaponType GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public void UnlockSword()
    {
        hasSword = true;
        currentWeapon = WeaponType.Sword;
        UpdateWeaponUI();
    }

    public void UnlockMagicProjectile()
    {
        hasMagicProjectile = true;
        currentWeapon = WeaponType.MagicProjectile;
        UpdateWeaponUI();
    }

    public void ChangeWeapon()
    {
        if (!hasSword || !hasMagicProjectile)
            return;

        if (currentWeapon == WeaponType.Sword)
            currentWeapon = WeaponType.MagicProjectile;
        else
            currentWeapon = WeaponType.Sword;

        UpdateWeaponUI();
    }

    private void UpdateWeaponUI()
    {
        swordDarkIcon.SetActive(hasSword);
        swordColorIcon.SetActive(hasSword && currentWeapon == WeaponType.Sword);

        magicDarkIcon.SetActive(hasMagicProjectile);
        magicColorIcon.SetActive(hasMagicProjectile && currentWeapon == WeaponType.MagicProjectile);
    }
}
