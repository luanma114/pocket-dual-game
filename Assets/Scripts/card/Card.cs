using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static constant;

public class card 
{
    public string cardName;
    public int cardNumber;
    public ShowPoint Belongto;

    public card(string cardName, int cardNumber,ShowPoint player)
    {
        this.cardName = cardName;
        this.cardNumber = cardNumber;
        this.Belongto = player;
    }

}
