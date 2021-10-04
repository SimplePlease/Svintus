using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;


public class GameManagerScript : MonoBehaviourPunCallbacks
{
    public GameObject playersTable;
    public GameObject settingsPanel;
    public GameObject nextTurnButton;
    public GameObject colorSelectPanel;
    public GameObject winerPanel;
    public GameObject logTextPanel;
    public GameObject settingButton;
    public GameObject hand;
    public GameObject deck;
    public GameObject dropPanel;
    public GameObject storage;

    public int turn;
    private bool turnDirection;


    void Start()
    {
        turn = 0;
        turnDirection = true;
        for (int i = PhotonNetwork.PlayerList.Length; i < 8; i++)
        {
            playersTable.transform.GetChild(i).gameObject.SetActive(false);
        }

        if (PhotonNetwork.IsMasterClient)
        {
            DropFirstCard();
            GiveFirstHands();
            nextTurnButton.SetActive(true);
        }

        playersTable.transform.GetChild(0).GetComponent<PlayerInfoScript>().OutlineActive(true);
    }

    private void GiveFirstHands()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            print("Give cards to " + PhotonNetwork.PlayerList[i].NickName);
            deck.GetComponent<PhotonView>().RPC("GiveCardToHand", PhotonNetwork.PlayerList[i], 8);
            photonView.RPC("UpdatePlayerListElement", RpcTarget.All, i, 8);
        }
    }

    private void DropFirstCard()
    {
        deck.GetComponent<DeckScript>().GiveCardToHand(1);
        GameObject firstCard = hand.transform.GetChild(0).gameObject;
        firstCard.transform.SetParent(storage.transform);
        deck.GetComponent<PhotonView>().RPC("GiveCardToDropPanel", RpcTarget.All, firstCard.name);
        //firstCard.GetComponent<CardScript>().isCardWorked = false;
        if ((firstCard.GetComponent<CardScript>().type == CardScript.CardType.Perexryuk) ||
            (firstCard.GetComponent<CardScript>().type == CardScript.CardType.Polisvin))
        {
            CardEvent(firstCard.GetComponent<CardScript>());
        }
    }

    public int LocalPlayerNumber()
    {
        int k = 0;
        for (int j = 0; j < PhotonNetwork.PlayerList.Length; j++)
        {
            if (PhotonNetwork.LocalPlayer.Equals(PhotonNetwork.PlayerList[j]))
            {
                k = j;
            }
        }

        return k;
    }


    [PunRPC]
    void UpdatePlayerListElement(int index, int cardCount)
    {
        //print(index.ToString() + " " + PhotonNetwork.PlayerList[index].NickName);
        playersTable.transform.GetChild(index).GetComponent<PlayerInfoScript>()
            .UpdateInfo(PhotonNetwork.PlayerList[index].NickName, cardCount);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        photonView.RPC("UpdatePlayerListElement", RpcTarget.All, LocalPlayerNumber(), hand.transform.childCount);
        playersTable.transform.GetChild(PhotonNetwork.PlayerList.Length).gameObject.SetActive(false);
    }

    public void Leave()
    {
        if (turn == LocalPlayerNumber())
            photonView.RPC("NextTurn", RpcTarget.All);
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void SettingsButton()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }

    public void NextTurnButton()
    {
        CardScript lastCard =
            dropPanel.transform.GetChild(dropPanel.transform.childCount - 1).GetComponent<CardScript>();
        if ((lastCard.isCardWorked) || (lastCard.type < CardScript.CardType.Hapezh))
        {
            deck.GetComponent<DeckScript>().GiveCardToHand(1);
            photonView.RPC("SetLogText", RpcTarget.All,
                PhotonNetwork.NickName + " пропустил ход и взял карту");
        }
        else if (lastCard.type == CardScript.CardType.Hapezh)
        {
            deck.GetComponent<DeckScript>().GiveCardToHand(dropPanel.GetComponent<DropPanelScript>().HapezhCount() * 3);
            photonView.RPC("SetLogText", RpcTarget.All,
                PhotonNetwork.NickName + " пропустил ход и взял  " +
                dropPanel.GetComponent<DropPanelScript>().HapezhCount() * 3 + " карт");
        }
        lastCard.isCardWorked = true;
        dropPanel.GetComponent<PhotonView>().RPC("MarkLastCardAsWorked", RpcTarget.Others);
        photonView.RPC("NextTurn", RpcTarget.All);
    }

    [PunRPC]
    public void NextTurn()
    {
        photonView.RPC("UpdatePlayerListElement", RpcTarget.All,
            LocalPlayerNumber(), hand.transform.childCount);
        if (hand.transform.childCount == 0)
        {
            photonView.RPC("FinishGame", RpcTarget.All, PhotonNetwork.NickName);
        }
        else
        {
            playersTable.transform.GetChild(turn).GetComponent<PlayerInfoScript>().OutlineActive(false);
            if (turnDirection)
                turn = (turn >= PhotonNetwork.PlayerList.Length - 1) ? 0 : turn + 1;
            else
                turn = (turn <= 0) ? PhotonNetwork.PlayerList.Length - 1 : turn - 1;
            playersTable.transform.GetChild(turn).GetComponent<PlayerInfoScript>().OutlineActive(true);
            nextTurnButton.SetActive(turn == LocalPlayerNumber());
        }
    }

    [PunRPC]
    public void SetTurn(int value)
    {
        turn = value;
    }

    public void CardEvent(CardScript card)
    {
        dropPanel.GetComponent<PhotonView>().RPC("MarkLastCardAsWorked", RpcTarget.Others);
        if (card.type == CardScript.CardType.Perexryuk)
        {
            photonView.RPC("ReverseTurnDirection", RpcTarget.All);
            photonView.RPC("SetLogText", RpcTarget.All,
                PhotonNetwork.NickName + " положил  Perexryuk и сменил направление игры");
        }
        else if (card.type == CardScript.CardType.Polisvin)
        {
            colorSelectPanel.SetActive(true);
            nextTurnButton.SetActive(false);
        }
    }

    [PunRPC]
    public void ReverseTurnDirection()
    {
        turnDirection = !turnDirection; 
    }

    public void SelectColorButton(int newColor)
    {
        photonView.RPC("SelectColor", RpcTarget.All, newColor);
        colorSelectPanel.SetActive(false);
        photonView.RPC("NextTurn", RpcTarget.All);
        photonView.RPC("SetLogText", RpcTarget.All,
            PhotonNetwork.NickName + " положил Polisvin " + (CardScript.CardColor) newColor + " цвета.");
    }

    [PunRPC]
    public void SelectColor(int newColor)
    {
        dropPanel.transform.GetChild(dropPanel.transform.childCount - 1).GetComponent<CardScript>().color =
            (CardScript.CardColor) newColor;
    }

    [PunRPC]
    public void FinishGame(string winerNickName)
    {
        nextTurnButton.SetActive(false);
        settingButton.SetActive(false);
        dropPanel.SetActive(false);
        hand.transform.parent.gameObject.SetActive(false);
        colorSelectPanel.SetActive(false);
        winerPanel.SetActive(true);
        winerPanel.transform.GetChild(0).GetComponent<Text>().text = winerNickName + " is a winner!!!";
    }

    [PunRPC]
    public void SetLogText(string logText)
    {
        logTextPanel.transform.GetChild(0).GetComponent<Text>().text = logText;
    }
}