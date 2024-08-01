using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using TMPro;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private GameObject playersHolder;
    [Header("Options")]
    [SerializeField] private float fRefreshRate = 1.0f;
    [Header("UI")]
    [SerializeField] private GameObject[] slots;
    [SerializeField] private TMP_Text[] scoreTexts;
    [SerializeField] private TMP_Text[] nameTexts;

    private void Start()
    {
        InvokeRepeating(nameof(Refresh), 1.0f, fRefreshRate);
    }

    private void Update()
    {
        playersHolder.SetActive(Input.GetKey(KeyCode.Tab));
    }

    public void Refresh()
    {
        foreach (GameObject _slot in slots)
        {
            _slot.SetActive(false);
        }

        List<Player> _sortedPlayerList = (from player in PhotonNetwork.PlayerList orderby player.GetScore() descending select player).ToList();
        int i = 0;

        foreach (Player _player in _sortedPlayerList)
        {
            slots[i].SetActive(true);

            if (_player.NickName == "")
                _player.NickName = "Unnamed";

            nameTexts[i].text = _player.NickName;
            scoreTexts[i].text = _player.GetScore().ToString();
            ++i;
        }
    }
}