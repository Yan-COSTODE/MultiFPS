using UnityEngine;
using Photon.Pun;

public class RoomManager : SingletonTemplatePunCallbacks<RoomManager>
{
    public static RoomManager instance;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject roomCamera;
    [SerializeField] private GameObject nameUI;
    [SerializeField] private GameObject connectingUI;
    private string nickname = "Unnamed";

    public void ChangeNickname(string _name)
    {
        nickname = _name;
    }

    public void JoinRoomButtonPressed()
    {
        Debug.Log("Connecting...");
        PhotonNetwork.ConnectUsingSettings();
        nameUI.SetActive(false);
        connectingUI.SetActive(true);
    }
    
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Connected to Server");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("We're in the lobby");
        PhotonNetwork.JoinOrCreateRoom("test", null, null);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("We're connected and in a room!");
        roomCamera.SetActive(false);
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        GameObject _player = PhotonNetwork.Instantiate(player.name, spawnPoint.position, Quaternion.identity);
        _player.GetComponent<PlayerSetup>().IsLocalPlayer();
        _player.GetComponent<Health>().IsLocalPlayer = true;
        _player.GetComponent<PhotonView>().RPC("SetNickname", RpcTarget.AllBuffered, nickname);
        PhotonNetwork.LocalPlayer.NickName = nickname;
    }
}
