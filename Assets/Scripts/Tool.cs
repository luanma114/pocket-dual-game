using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool
{
    public static void Sort(List<card> cards)
    {
        cards.Sort(
            (card a, card b) =>
            {
                return a.cardNumber.CompareTo(b.cardNumber);
            });
    }
}
