using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;


public class CardScript : MonoBehaviour, IBeginDragHandler, IDragHandler,IEndDragHandler
{
    private Vector3 offset;
    public bool isCardWorked = true;
    public CardType type;
    public CardColor color;
    public enum CardType
    {
        Zero,
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Perexryuk,
        Polisvin,
        Hapezh,
        Zaxrapin
    }
    
    public enum CardColor
    {
        Grey,
        Blue,
        Green,
        Red,
        Yellow
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        offset = transform.position - Camera.main.ScreenToWorldPoint(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = new Vector3(Camera.main.ScreenToWorldPoint(eventData.position).x,
            Camera.main.ScreenToWorldPoint(eventData.position).y, 0) + offset;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (transform.parent.name == "Hand")
        {
            transform.parent.GetComponent<HorizontalLayoutGroup>().SetLayoutHorizontal();
            transform.parent.GetComponent<HorizontalLayoutGroup>().SetLayoutVertical();
        }
    }
}
