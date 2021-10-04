
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoScript : MonoBehaviour
{
    public Text nick;

    public Text cardCount;
    
    public void UpdateInfo(string nick, int cardCount)
    {
        this.nick.text = nick;
        this.cardCount.text = cardCount.ToString();
    }

    public void OutlineActive(bool value)
    {
        gameObject.GetComponent<Outline>().enabled = value;
        nick.GetComponent<Outline>().enabled = value;
        cardCount.GetComponent<Outline>().enabled = value;
    }
    
    
}
