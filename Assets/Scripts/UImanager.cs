using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    public static UImanager instance { get; private set; }

    private GameManager gameManager;

    public GameObject[] panels;

    public Text Text;

    public Text[] UIs;

    public InputField InputField;


    private void Start()
    {
        instance = this;
        gameManager = GameManager.instance;
        OpenGame();

    }

    public void OpenGame()
    {
        panels[0].SetActive(true);
        panels[1].SetActive(false);
    }

    public void StartGame()
    {
        panels[0].SetActive(false);
        panels[1].SetActive(true);
        gameManager.StartGame();

    }

    public void SetComputerAttack(int number)
    {
        Text = UIs[0];
        Text.text = number.ToString();
    }    
    public void SetComputerGuard(int number)
    {
        Text = UIs[1];
        Text.text = number.ToString();
    }    
    public void SetComputerBlood(int number)
    {
        Text = UIs[2];
        Text.text = number.ToString();
    }    
    public void SetPlayerAttack(int number)
    {
        Text = UIs[3];
        Text.text = number.ToString();
    }    
    public void SetPlayerGuard(int number)
    {
        Text = UIs[4];
        Text.text = number.ToString();
    }    
    public void SetPlayerBlood(int number)
    {
        Text = UIs[5];
        Text.text = number.ToString();
    }

    public void SetPlayerPoint(int point)
    {
        Text = UIs[6];
        Text.text = "你的剩余行动点："+point.ToString();
    }

    public void CardClick() 
    {
        Debug.Log("button clicked");
        CardUI cardUI = EventSystem.current.currentSelectedGameObject.GetComponent<CardUI>();
        foreach(Transform child in cardUI.transform.parent)
        {
            CardUI eachUI = child.GetComponent<CardUI>();
            eachUI.IsSelected = false;
        }
        cardUI.IsSelected = true; 
        
    }

    public void gameover()
    {
        panels[4].SetActive(true);
        if(gameManager.computerBlood<=0)
        {
            Text = UIs[7];
            Text.text = "你赢了！你真牛！";
        }
        else
        {
            Text = UIs[7];
            Text.text = "你输了！你真拉！";
        }
    }
}
