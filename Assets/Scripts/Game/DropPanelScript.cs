using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;


public class DropPanelScript : MonoBehaviourPunCallbacks, IDropHandler
{
    public GameObject gameManager;
    public GameObject deck;


    public void OnDrop(PointerEventData eventData)
    {
        CardScript card = eventData.pointerDrag.GetComponent<CardScript>();
        print(CheckPlacedCard(card));
        if (CheckPlacedCard(card))
        {
            card.transform.SetParent(transform);
            card.GetComponent<CanvasGroup>().blocksRaycasts = false;
            deck.GetComponent<PhotonView>().RPC("GiveCardToDropPanel", RpcTarget.Others, card.name);
            if (!CheckTurn())
            {
                gameManager.GetComponent<PhotonView>().RPC("SetTurn", RpcTarget.All,
                    gameManager.GetComponent<GameManagerScript>().LocalPlayerNumber());
            }
            if ((card.type == CardScript.CardType.Perexryuk) || (card.type == CardScript.CardType.Polisvin))
            {
                gameManager.GetComponent<GameManagerScript>().CardEvent(card);
            }

            if (card.type != CardScript.CardType.Polisvin)
            {
                gameManager.GetComponent<PhotonView>().RPC("NextTurn", RpcTarget.All);
            }
            gameManager.GetComponent<PhotonView>().RPC("SetLogText", RpcTarget.All,
                PhotonNetwork.NickName + " положил карту  " + card.color + " " + card.type);
        }
    }

    private bool CheckPlacedCard(CardScript card)
    {
        CardScript lastCard = transform.GetChild(transform.childCount - 1).GetComponent<CardScript>();
        return ((CheckTurn() && ((lastCard.type == card.type) || lastCard.isCardWorked) &&
                 (card.type == lastCard.type || card.color == lastCard.color ||
                  card.color == CardScript.CardColor.Grey)) ||
                (card.type == lastCard.type && card.color == lastCard.color ||
                 card.color == CardScript.CardColor.Grey));
    }

    private bool CheckTurn()
    {
        return PhotonNetwork.PlayerList[gameManager.GetComponent<GameManagerScript>().turn]
            .Equals(PhotonNetwork.LocalPlayer);
    }

    public int HapezhCount()
    {
        int i = transform.childCount - 1;
        while ((i > 0) && (transform.GetChild(i).GetComponent<CardScript>().type == CardScript.CardType.Hapezh) && (!transform.GetChild(i).GetComponent<CardScript>().isCardWorked))
            i--;
        return transform.childCount - 1 - i;
    }

    [PunRPC]
    public void MarkLastCardAsWorked()
    {
        transform.GetChild(transform.childCount - 1).GetComponent<CardScript>().isCardWorked = true;
    }

    
}