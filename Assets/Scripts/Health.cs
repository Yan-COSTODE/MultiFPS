using Photon.Pun;
using TMPro;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int iHealth;
    [SerializeField] private TMP_Text healthText;

    [PunRPC]
    public void TakeDamage(int _damage)
    {
        iHealth -= _damage;
        healthText.text = iHealth.ToString();
        
        if (iHealth <= 0)
            Destroy(gameObject);
    }
}
