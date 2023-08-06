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
    /// 手牌
    /// </summary>
    public List<card> CardList
    {
        get
        {
            return cardList;
        }

    }

    /// <summary>
    /// 是否有牌
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
    /// 添加牌
    /// </summary>
    /// <param name="card">添加的牌</param>
    /// <param name="selected">要增高么</param>
    public virtual void AddCard(card card, bool selected,constant.ShowPoint hand)
    {
        cardList.Add(card);
        //****//
        card.Belongto = hand;
        CreateCardUI(card, cardList.Count - 1, selected);
        
    }

    /// <summary>
    /// 排序
    /// </summary>
    /// <param name="asc">升序？</param>
    public void Sort()
    {
        Tool.Sort(cardList);
        //UI
        SortCardUI(cardList);
    }
    /// <summary>
    /// 排序CardUI
    /// </summary>
    /// <param name="cards">有序list</param>
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
    /// 根据数据创建CardUI
    /// </summary>
    /// <param name="card">数据</param>
    /// <param name="index">索引</param>
    /// <param name="isSeleted">上升？</param>
    public void CreateCardUI(card card, int index, bool isSeleted)
    {
        //对象池生成
        GameObject go = LeanPool.Spawn(prefab);
        Button btn = go.GetComponent<Button>();
        btn.onClick.AddListener(UImanager.instance.CardClick);
        go.name = characterType.ToString() + index.ToString();
        //设置位置和是否选中
        CardUI cardUI = go.GetComponent<CardUI>();
        cardUI.Card = card;
        cardUI.IsSelected = isSeleted;
        cardUI.SetPosition(Point, index);
    }

    /// <summary>
    /// 出牌
    /// </summary>
    /// <returns></returns>
    public virtual card DealCard()
    {
        card card = cardList[CardCount - 1];
        cardList.Remove(card);
        return card;
    }


}
