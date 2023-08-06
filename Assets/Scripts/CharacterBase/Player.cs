using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Player : CharacterBase
{
    public override card DealCard()
    {
        card card = base.DealCard();
        return card;
    }


    public card tempCard = null;
    public CardUI tempUI = null;

    /// <summary>
    /// ɾ������/�ɹ�����
    /// </summary>
    public void DestroySelectCard()
    {
        if (tempCard == null || tempUI == null)
            return;
        else
        {


            tempUI.SetPosition(transform.Find("gamePanel/player/playerPlaying"), 0);
            CardList.Remove(tempCard);


            SortCardUI(CardList);


        }
    }
}
