using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private GameObject weaponLogic;

    [SerializeField] private GameObject groundSlamWeaponLogic;

    public void EnableWeapon()
    {
        Debug.Log("Enemy Weapon Active");
        weaponLogic.SetActive(true);
    }

    public void EnableGroundSlamWeapon()
    {
        Debug.Log("Enemy Weapon Active");
        groundSlamWeaponLogic.SetActive(true);
    }

    public void DisableWeapon()
    {
        Debug.Log("Enemy Weapon Disabled");
        weaponLogic.SetActive(false);
    }
    public void DisableGroundSlamWeapon()
    {
        Debug.Log("Enemy Weapon Disabled");
        groundSlamWeaponLogic.SetActive(false);
    }
}
