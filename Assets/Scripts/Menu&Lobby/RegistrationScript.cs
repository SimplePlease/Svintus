using System;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class RegistrationScript : MonoBehaviourPunCallbacks
{
    public GameObject mainPanel;
    public InputField inputNickName;
    public InputField inputRoomName;

    void Start()
    {
        inputNickName.text = PlayerPrefs.GetString("NickName");
    }

    public void CreateRoom()
    {
        if ((inputNickName.text != String.Empty) && (inputRoomName.text != string.Empty))
        {
            PhotonNetwork.NickName = inputNickName.text;
            PlayerPrefs.SetString("NickName", inputNickName.text);
            PhotonNetwork.CreateRoom(inputRoomName.text, new RoomOptions() {MaxPlayers = 10}, null);
        }
    }

    public void JoinRoom()
    {
        if ((inputNickName.text != String.Empty) && (inputRoomName.text != String.Empty))
        {
            PhotonNetwork.NickName = inputNickName.text;
            PlayerPrefs.SetString("NickName", inputNickName.text);

            PhotonNetwork.JoinRoom(inputRoomName.text);
        }
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("RoomLobby");
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape)) { Back(); };
    }
    
    public void Back()
    {
        mainPanel.SetActive(true);
        gameObject.SetActive(false);
    }
}
