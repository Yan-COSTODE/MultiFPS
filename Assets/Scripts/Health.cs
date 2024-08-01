using Photon.Pun;
using TMPro;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private bool bIsLocalPlayer;
    [SerializeField] private int iHealth;
    [SerializeField] private TMP_Text healthText;

    public int HP => iHealth;
    
    public bool IsLocalPlayer
    {
        get => bIsLocalPlayer;
        set => bIsLocalPlayer = value;
    }
    
    [PunRPC]
    public void TakeDamage(int _damage)
    {
        iHealth -= _damage;
        healthText.text = iHealth.ToString();

        if (iHealth > 0) 
            return;
        
        if (bIsLocalPlayer)
            RoomManager.instance.SpawnPlayer();
            
        Destroy(gameObject);
    }
}
