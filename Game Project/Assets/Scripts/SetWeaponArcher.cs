using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetWeaponArcher : MonoBehaviour
{
    public List<GameObject> Weapons;
    public List<float> speed;
    public List<float> timeBetweenAttacks;
    public List<int> Damage;
    public List<Arrow> Arrow;

    public ArcherController playerController;
    public int currentWeapon;

    void Update()
    {
        if (currentWeapon > Weapons.Count - 1)
        {
            currentWeapon = Weapons.Count - 1;
        }

        if (currentWeapon < 0)
        {
            currentWeapon = 0;
        }
    }

    public void setWeaponIndex(int WeaponIndex)
    {
        currentWeapon = WeaponIndex;
    }

    public void ChangeWeapon(int WeaponIndex)
    {
        Weapons[currentWeapon].SetActive(false);
        setWeaponIndex(WeaponIndex);
        Weapons[currentWeapon].SetActive(true);

        playerController.speed = speed[currentWeapon];
        playerController.timeBetweenAttacks = timeBetweenAttacks[currentWeapon];
        playerController.ArrowPrefab = Arrow[currentWeapon].gameObject;
        Arrow[currentWeapon].GetComponent<Arrow>().damage = Damage[currentWeapon];
    }
}
