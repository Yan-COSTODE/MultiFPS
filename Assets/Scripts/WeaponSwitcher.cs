using System;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    [SerializeField] private Animation animation;
    [SerializeField] private AnimationClip draw;
    private int iSelectedWeapon = 0;

    private void Start()
    {
        SelectWeapon();
    }

    private void Update()
    {
        int _previousSelectedWeapon = iSelectedWeapon;

        if (Input.GetKeyDown(KeyCode.Alpha1))
            iSelectedWeapon = 0;
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
            iSelectedWeapon = 1;
        
        if (Input.GetKeyDown(KeyCode.Alpha3))
            iSelectedWeapon = 2;
        
        if (Input.GetKeyDown(KeyCode.Alpha4))
            iSelectedWeapon = 3;
        
        if (Input.GetKeyDown(KeyCode.Alpha5))
            iSelectedWeapon = 4;
        
        if (Input.GetKeyDown(KeyCode.Alpha6))
            iSelectedWeapon = 5;
        
        if (Input.GetKeyDown(KeyCode.Alpha7))
            iSelectedWeapon = 6;
        
        if (Input.GetKeyDown(KeyCode.Alpha8))
            iSelectedWeapon = 7;
        
        if (Input.GetKeyDown(KeyCode.Alpha9))
            iSelectedWeapon = 8;

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (iSelectedWeapon >= transform.childCount - 1)
                iSelectedWeapon = 0;
            else
                ++iSelectedWeapon;
        }
        
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (iSelectedWeapon <= 0)
                iSelectedWeapon = transform.childCount - 1;
            else
                --iSelectedWeapon;
        }
        
        if (_previousSelectedWeapon != iSelectedWeapon)
            SelectWeapon();
    }

    private void SelectWeapon()
    {
        if (iSelectedWeapon >= transform.childCount)
            iSelectedWeapon = transform.childCount - 1;

        animation.Stop();
        animation.Play(draw.name);
        
        int i = 0;

        foreach (Transform _weapon in transform)
        {
            if (i == iSelectedWeapon)
                _weapon.GetComponent<Weapon>()?.UpdateUI();
            
            _weapon.gameObject.SetActive(i == iSelectedWeapon);
            ++i;
        }
    }
}
