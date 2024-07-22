using Photon.Pun;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private int iDamage = 25;
    [SerializeField] private float fFireRate = 10.0f;
    [SerializeField] private Camera camera;
    [SerializeField] private GameObject hitVFX;
    private float fNextFire;

    private void Update()
    {
        if (fNextFire > 0)
            fNextFire -= Time.deltaTime;
        
        if (Input.GetButton("Fire1") && fNextFire <= 0.0f)
        {
            fNextFire = 1.0f / fFireRate;
            Fire();
        }
    }

    private void Fire()
    {
        Ray _ray = new Ray(camera.transform.position, camera.transform.forward);
        RaycastHit _hit;

        if (Physics.Raycast(_ray.origin, _ray.direction, out _hit, 100.0f))
        {
            PhotonNetwork.Instantiate(hitVFX.name, _hit.point, Quaternion.identity);
            
            if (_hit.transform.gameObject.GetComponent<Health>())
                _hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, iDamage);
        }
    }
}
