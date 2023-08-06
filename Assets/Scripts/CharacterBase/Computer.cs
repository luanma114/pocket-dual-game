using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Computer : CharacterBase
{
    public override card DealCard()
    {
        card card = base.DealCard();
        return card;
    }

    public override Transform Point
    {
        get
        {
            if (point == null)
                point = transform.Find("gamePanel/computer/" + characterType.ToString());
            return point;
        }
    }

    public card tempCard;
    public CardUI tempUI = null;

    public void DestroySelectCard()
    {
        if (tempCard == null || tempUI == null)
            return;
        else
        {


            tempUI.SetPosition(transform.Find("gamePanel/computer/computerPlaying"), 0);
            CardList.Remove(tempCard);


            SortCardUI(CardList);


        }
    }
}
