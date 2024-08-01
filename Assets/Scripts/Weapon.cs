using System;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private int iDamage = 25;
    [SerializeField] private float fFireRate = 10.0f;
    [SerializeField] private Camera camera;
    [SerializeField] private GameObject hitVFX;
    [Header("Ammo")]
    [SerializeField] private int iMag = 5;
    [SerializeField] private int iAmmo = 30;
    [SerializeField] private int iMagAmmo = 30;
    [Header("UI")] 
    [SerializeField] private TMP_Text magText;
    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private Animation animation;
    [SerializeField] private AnimationClip reload;
    [Header("Recoil")]
    [Range(0, 2), SerializeField] private float fRecoverPercent = 0.7f;
    [SerializeField] private float fRecoilUp = 1.0f;
    [SerializeField] private float fRecoilBack = 0.0f;
    private float fNextFire;
    private Vector3 originalPosition;
    private Vector3 recoilVelocity = Vector3.zero;
    private float fRecoilLength;
    private float fRecoverLength;
    private bool bRecoiling;
    private bool bRecovering;

    private void Start()
    {
        UpdateUI();
        originalPosition = transform.localPosition;
        fRecoilLength = 0;
        fRecoverLength = 1 / fFireRate * fRecoverPercent;
    }

    private void Update()
    {
        if (fNextFire > 0)
            fNextFire -= Time.deltaTime;
        
        if (Input.GetButton("Fire1") && fNextFire <= 0.0f && iAmmo > 0 && !animation.isPlaying)
        {
            fNextFire = 1.0f / fFireRate;
            --iAmmo;
            Fire();
            UpdateUI();
        }
        
        if (Input.GetKeyDown(KeyCode.R) && iMag > 0 && iAmmo < iMagAmmo)
            Reload();
        
        if (bRecoiling)
            Recoil();
        
        if (bRecovering)
            Recover();
    }

    private void Reload()
    {
        animation.Play(reload.name);
        --iMag;
        iAmmo = iMagAmmo;
        UpdateUI();
    }

    public void UpdateUI()
    {
        magText.text = iMag.ToString();
        ammoText.text = iAmmo + "/" + iMagAmmo;
    }
    
    private void Fire()
    {
        bRecoiling = true;
        bRecovering = false;
        
        Ray _ray = new Ray(camera.transform.position, camera.transform.forward);
        RaycastHit _hit;

        if (Physics.Raycast(_ray.origin, _ray.direction, out _hit, 100.0f))
        {
            PhotonNetwork.Instantiate(hitVFX.name, _hit.point, Quaternion.identity);
            PhotonNetwork.LocalPlayer.AddScore(1);
            
            if (_hit.transform.gameObject.GetComponent<Health>())
            {
                if (iDamage >= _hit.transform.gameObject.GetComponent<Health>().HP)
                    PhotonNetwork.LocalPlayer.AddScore(100);
                
                _hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, iDamage);
            }
        }
    }

    private void Recoil()
    {
        Vector3 _finalPosition = new Vector3(originalPosition.x, originalPosition.y + fRecoilUp, originalPosition.z - fRecoilBack);
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, _finalPosition, ref recoilVelocity, fRecoilLength);

        if (transform.localPosition == _finalPosition)
        {
            bRecoiling = false;
            bRecovering = true;
        }
    }
    
    private void Recover()
    {
        Vector3 _finalPosition = originalPosition;
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, _finalPosition, ref recoilVelocity, fRecoverLength);

        if (transform.localPosition == _finalPosition)
        {
            bRecoiling = false;
            bRecovering = false;
        }
    }
}
