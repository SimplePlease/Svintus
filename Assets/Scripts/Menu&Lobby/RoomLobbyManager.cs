using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class RoomLobbyManager : MonoBehaviourPunCallbacks
{
    public GameObject PlayersList;
    public Text roomName;
    public GameObject StartButton;

    void Start()
    {
        roomName.text = PhotonNetwork.CurrentRoom.Name;
        SetPlayersList();
        StartButton.SetActive(false);
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void SetPlayersList()
    {
        for (int i = 0; i < (PhotonNetwork.PlayerList.Length); i++)
        {
            PlayersList.transform.GetChild(i).GetComponent<Text>().text = PhotonNetwork.PlayerList[i].NickName;
        }

    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        StartButton.SetActive((PhotonNetwork.LocalPlayer.IsMasterClient) && (PhotonNetwork.PlayerList.Length > 1));
        SetPlayersList();
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        SetPlayersList();
        PlayersList.transform.GetChild(PhotonNetwork.PlayerList.Length).GetComponent<Text>().text = "";

    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel("GameField");
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape)) { Back(); };
    }
   
    public void Back()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
    
}
