using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
using static constant;
using UnityEngine.UI;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine.TextCore.Text;

public class CharacterBase : MonoBehaviour
{

    public ShowPoint characterType;

    public List<card> cardList = new List<card>();

    public Transform point;

    public GameObject prefab;
    /// <summary>
    /// ����
    /// </summary>
    public List<card> CardList
    {
        get
        {
            return cardList;
        }

    }

    /// <summary>
    /// �Ƿ�����
    /// </summary>
    public bool HasCard
    {
        get { { return cardList.Count != 0; } }

    }

    public int CardCount { get { return cardList.Count; } }

    virtual public Transform Point
    {
        get 
        { 
            if (point == null)
                point = transform.Find("gamePanel/player/"+characterType.ToString());
            return point; 
        }
    }

    /// <summary>
    /// �����
    /// </summary>
    /// <param name="card">��ӵ���</param>
    /// <param name="selected">Ҫ����ô</param>
    public virtual void AddCard(card card, bool selected,constant.ShowPoint hand)
    {
        cardList.Add(card);
        //****//
        card.Belongto = hand;
        CreateCardUI(card, cardList.Count - 1, selected);
        
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="asc">����</param>
    public void Sort()
    {
        Tool.Sort(cardList);
        //UI
        SortCardUI(cardList);
    }
    /// <summary>
    /// ����CardUI
    /// </summary>
    /// <param name="cards">����list</param>
    public void SortCardUI(List<card> cards)
    {
        CardUI[] cardUIs = Point.GetComponentsInChildren<CardUI>();
        for (int i = 0; i < cards.Count; i++)
        {
            for (int j = 0; j < cardUIs.Length; j++)
            {
                if (cardUIs[j].card == cards[i])
                {
                    cardUIs[j].SetPosition(Point, i);
                }
            }
        }
    }

    /// <summary>
    /// �������ݴ���CardUI
    /// </summary>
    /// <param name="card">����</param>
    /// <param name="index">����</param>
    /// <param name="isSeleted">������</param>
    public void CreateCardUI(card card, int index, bool isSeleted)
    {
        //���������
        GameObject go = LeanPool.Spawn(prefab);
        Button btn = go.GetComponent<Button>();
        btn.onClick.AddListener(UImanager.instance.CardClick);
        go.name = characterType.ToString() + index.ToString();
        //����λ�ú��Ƿ�ѡ��
        CardUI cardUI = go.GetComponent<CardUI>();
        cardUI.Card = card;
        cardUI.IsSelected = isSeleted;
        cardUI.SetPosition(Point, index);
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <returns></returns>
    public virtual card DealCard()
    {
        card card = cardList[CardCount - 1];
        cardList.Remove(card);
        return card;
    }


}
