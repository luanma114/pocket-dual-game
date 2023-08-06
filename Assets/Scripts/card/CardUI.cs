using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CardUI : MonoBehaviour
{
    public card card;
    Image image;
    bool isSelected;
    public bool isComputerSelected;

    public card Card
    {
        get
        {
            return card;
        }

        set
        {
            card = value;
            SetImage();
        }
    }

    public bool IsSelected
    {
        get
        {
            return isSelected;
        }

        set
        {
            if (card.Belongto != constant.ShowPoint.playerHand || isSelected == value)
                return;

            if (value)
                transform.localPosition += Vector3.up * 30;
            else
                transform.localPosition -= Vector3.up * 30;

            isSelected = value;
        }
    }

    public void SetImage()
    {
        image = GetComponent<Image>();
        if (card.Belongto == constant.ShowPoint.computerHand)
        {
            image.sprite = Resources.Load<Sprite>("back");
        }
        else
        {
            image.sprite = Resources.Load<Sprite>(card.cardName);
        }
    }


    public void SetPosition(Transform parent, int index)
    {
        transform.SetParent(parent, false);
        transform.SetSiblingIndex(index);

        if (card.Belongto == constant.ShowPoint.playerHand || card.Belongto == constant.ShowPoint.computerHand)
        {
            transform.localPosition = Vector3.right * index * 200;

            //防止还原
            if (IsSelected)
                transform.localPosition += Vector3.up * 20;

        }
    }

    /// <summary>
    /// 回收
    /// </summary>
    public void Destroy()
    {
        gameObject.SetActive(false);
    }

}
