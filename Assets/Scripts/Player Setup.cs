using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    [SerializeField] private Movement movement;
    [SerializeField] private GameObject camera;
    [SerializeField] private string nickname;
    [SerializeField] private TMP_Text nicknameText;
    
    public void IsLocalPlayer()
    {
        movement.enabled = true;
        camera.SetActive(true);
        nicknameText.enabled = false;
    }

    [PunRPC]
    public void SetNickname(string _name)
    {
        nickname = _name;
        nicknameText.text = nickname;
    }
}
