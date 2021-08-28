using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetWeapon : MonoBehaviour
{
    public List<GameObject> Weapons;
    public List<float> speed;
    public List<float> timeBetweenAttacks;
    public List<int> Damage;

    public PlayerController playerController;

    public int currentWeapon;

    void Start()
    {
    }

    void Update()
    {
        if (currentWeapon > Weapons.Count - 1)
        {
            currentWeapon = Weapons.Count - 1;
            playerController.currentWeapon = currentWeapon;
        }

        if (currentWeapon < 0)
        {
            currentWeapon = 0;
            playerController.currentWeapon = currentWeapon;
        }

        ChangeWeapon(currentWeapon);

        playerController.speed = speed[currentWeapon];
        playerController.timeBetweenAttacks = timeBetweenAttacks[currentWeapon];
        playerController.weaponDamage = Damage[currentWeapon];
    }

    public void setWeaponIndex(int WeaponIndex)
    {
        currentWeapon = WeaponIndex;
        playerController.currentWeapon = currentWeapon;
    }

    public void ChangeWeapon(int WeaponIndex)
    {
            for (int i = 0; i < Weapons.Count; i++)
            {
                if(WeaponIndex != i)
                {
                    Weapons[i].SetActive(false);
                }
                else
                {
                    Weapons[i].SetActive(true);
                }
        }
    }
}
