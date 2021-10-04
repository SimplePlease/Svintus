using Photon.Pun;
using UnityEngine;

public class DeckScript : MonoBehaviourPunCallbacks
{
    public GameObject cardPrefab;
    public GameObject hand;
    public GameObject dropPanel;
    public GameObject gameManager;
    public GameObject storage;

    [PunRPC]
    public void DeleteCard(string cardName)
    {
        if (transform.Find(cardName) != null)
        {
            GameObject card = transform.Find(cardName).gameObject;
            card.transform.SetParent(storage.transform);
        }
    }

    [PunRPC]
    public void GiveCardToHand(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (transform.childCount == 0)
            {
                while (dropPanel.transform.childCount > 9)
                {
                   
                    dropPanel.transform.GetChild(0).transform.SetParent(transform);
                }
            }
            int k = Random.Range(0, transform.childCount);
            GameObject card = transform.GetChild(k).gameObject;
            card.transform.SetParent(hand.transform);
            card.SetActive(true);
            photonView.RPC("DeleteCard", RpcTarget.Others, card.name);
        }
    }

    [PunRPC]
    public void GiveCardToDropPanel(string cardName)
    {
        GameObject card = storage.transform.Find(cardName).gameObject;
        card.transform.SetParent(dropPanel.transform);
        card.transform.position = dropPanel.transform.position;
        card.SetActive(true);
        card.GetComponent<CardScript>().isCardWorked = (card.GetComponent<CardScript>().type < CardScript.CardType.Hapezh); 
        card.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void ShuffleDeck()
    {
        
    }
}