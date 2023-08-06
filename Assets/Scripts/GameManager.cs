using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static constant;

public class GameManager : MonoBehaviour
{

    public static GameManager instance { get; private set; }

    Queue<card> PlayerCardLibrary = new Queue<card>();
    Queue<card> ComputerCardLibrary = new Queue<card>();
    Queue<card> PlayerUsedCardLibrary = new Queue<card>();
    Queue<card> ComputerUsedCardLibrary = new Queue<card>();


    public bool gameover = false;
    public bool computerPlayed = false;
    public bool playerPlayed = false;

    public int computerAttack = 1;
    public int computerGuard = 0;
    public int computerBlood = 20;
    public int computerDeath = 0;
    public bool computerFirstPlay = false;
    public bool computerDrag = false;
    public int playerAttack = 1;
    public int playerGuard = 0;
    public int playerBlood = 20;
    public int playerDeath = 0;
    public bool playerDrag = false;
    public int playerPoint = 0;
    public int playerPowerUp = 1;


    public Player player;
    public Computer computer;
    public card card { get; set; }

    public card playerPlayingCard;
    public CardUI playerPlayingCardUI;
    public card computerPlayingCard;
    public CardUI computerPlayingCardUI;



    void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (computerPlayed == true && playerPlayed == true)
        {
            computerPlayed = false;
            Invoke("nextmove", 0.5f);
        }


    }

    public void StartGame()
    {
        InitComputerCardLibrary();
        InitPlayerCardLibrary();
        Shuffle(ComputerCardLibrary);
        Shuffle(PlayerCardLibrary);
        UImanager.instance.SetComputerAttack(1);
        UImanager.instance.SetComputerGuard(0);
        UImanager.instance.SetComputerBlood(20);
        UImanager.instance.SetPlayerAttack(1);
        UImanager.instance.SetPlayerGuard(0);
        UImanager.instance.SetPlayerBlood(20);
        TurnOn();
        Invoke("computerPlay", 1);

    }


    /// <summary>
    /// 回合开始
    /// </summary>
    public void TurnOn()
    {

        player = gameObject.GetComponent<Player>();
        computer = gameObject.GetComponent<Computer>();
        player.prefab = Resources.Load<GameObject>("card");
        player.characterType = ShowPoint.playerHand;
        computer.prefab = Resources.Load<GameObject>("card");
        computer.characterType = ShowPoint.computerHand;
        if (gameover)
            return;
        else
        {
            while (player.CardList.Count <= 2)
            {
                card = DealCard(ShowPoint.playerHand,PlayerCardLibrary);
                player.AddCard(card, false, ShowPoint.playerHand);
            } 
            while (computer.CardList.Count <= 2)
            {
                card = DealCard(ShowPoint.computerHand, ComputerCardLibrary);
                computer.AddCard(card, false, ShowPoint.computerHand);
            }
        }
    }


    public void computerPlay()
    {
        if (gameover == true)
            return;
        int i = Random.Range(0, computer.CardList.Count);
        computer.tempCard = computer.CardList[i];
        computerPlayingCard = computer.tempCard;
        foreach (Transform child in transform.Find("gamePanel/computer/computerHand"))
        {
            CardUI eachUI = child.GetComponent<CardUI>();
            if (eachUI.card == computerPlayingCard)
            {
                computer.tempUI = eachUI;
                computerPlayingCardUI = eachUI;
            }

        }
 
        computer.DestroySelectCard();
        if (computerFirstPlay)
        {
            computerPlayingCard.Belongto = ShowPoint.computerused;
            computerPlayingCardUI.SetImage();
            computerFirstPlay = false;
        }
        computerPlayed = true;
    }

    public void playerPlay()
    {
        if (playerPlayed == true)
            return;
        if (gameover == true)
            return;

        foreach (Transform child in transform.Find("gamePanel/player/playerHand"))
        {
            CardUI eachUI = child.GetComponent<CardUI>();
            if (eachUI.IsSelected == true)
            {
                player.tempCard = eachUI.card;
                player.tempUI = eachUI;
                playerPlayingCardUI = eachUI;
            }               
        }
        playerPlayingCard = player.tempCard;
        
        player.DestroySelectCard();
        playerPlayed = true;
    }

    public void nextmove()
    {
        
        playerPlayingCardUI.SetPosition(transform.Find("gamePanel/player/playerUsed"), PlayerUsedCardLibrary.Count);
        computerPlayingCardUI.SetPosition(transform.Find("gamePanel/computer/computerUsed"), ComputerUsedCardLibrary.Count);
        computerPlayingCard.Belongto = ShowPoint.computerused;
        computerPlayingCardUI.SetImage();
        PlayerUsedCardLibrary.Enqueue(playerPlayingCard);
        ComputerUsedCardLibrary.Enqueue(computerPlayingCard);
        if (computerPlayingCard.cardNumber == playerPlayingCard.cardNumber)
        {           
            endTurn();
        }
        else if(computerPlayingCard.cardNumber > playerPlayingCard.cardNumber)
        {
            playerEffect(playerPlayingCard.cardNumber);
            if (computerBlood <= 0)
            {
                gameover = true;
                UImanager.instance.gameover();
            }
            if (playerBlood <= 0)
            {
                gameover = true;
                UImanager.instance.gameover();
            }
            computerMove(computerPlayingCard.cardNumber - playerPlayingCard.cardNumber);
            if (computerBlood <= 0)
            {
                gameover = true;
                UImanager.instance.gameover();
            }
            if (playerBlood <= 0)
            {
                gameover = true;
                UImanager.instance.gameover();
            }
        }
        else
        {
            computerEffect(computerPlayingCard.cardNumber);
            if (computerBlood <= 0)
            {
                gameover = true;
                UImanager.instance.gameover();
            }
            if (playerBlood <= 0)
            {
                gameover = true;
                UImanager.instance.gameover();
            }
            playerMove(playerPlayingCard.cardNumber - computerPlayingCard.cardNumber);

        }
    }

    public void endTurn()
    {
        UImanager.instance.panels[2].SetActive(false);
        playerPowerUp = 1;
        if(computerDrag && ComputerUsedCardLibrary.Count != 1)
        {
            int i = Random.Range(0, ComputerUsedCardLibrary.Count);
            foreach (Transform child in transform.Find("gamePanel/computer/computerUsed"))
            {
                CardUI eachUI = child.GetComponent<CardUI>();
                eachUI.SetPosition(transform.Find("gamePanel/computer/computerHand"), computer.cardList.Count + 1);
                eachUI.card.Belongto = ShowPoint.computerHand;
                break;
            }
        }
        if (playerDrag && PlayerUsedCardLibrary.Count != 1)
        {
            foreach (Transform child in transform.Find("gamePanel/player/playerUsed"))
            {
                CardUI eachUI = child.GetComponent<CardUI>();
                eachUI.IsSelected = false;
            }
            UImanager.instance.panels[3].SetActive(true);
        }
        else
        {
            if (player.cardList.Count < 3)
            {
                if (PlayerCardLibrary.Count == 0)
                {
                    WashPlayerCard();
                }
                card = DealCard(ShowPoint.playerHand, PlayerCardLibrary);
                player.AddCard(card, false, ShowPoint.playerHand);
            }
            if (computer.cardList.Count < 3)
            {
                if (ComputerCardLibrary.Count == 0)
                {
                    WashComputerCard();
                }
                card = DealCard(ShowPoint.computerHand, ComputerCardLibrary);
                computer.AddCard(card, false, ShowPoint.computerHand);
            }
            playerBlood -= playerDeath;
            UImanager.instance.SetPlayerBlood(playerBlood);
            computerBlood -= computerDeath;
            UImanager.instance.SetComputerBlood(computerBlood);
            playerPlayed = false;
            playerDrag = false;
            computerDrag = false;
            Invoke("computerPlay", 1);
        }
    }

    public void playerEffect(int point)
    {
        if(gameover == true)
        {
            return;
        }
        switch (point) 
        {
            case 1:
                WashPlayerCard();
                playerGuard++;
                UImanager.instance.SetPlayerGuard(playerGuard);
                break;
            case 2:
                if(playerAttack -computerGuard > 3)
                {
                    computerBlood -= (playerAttack - computerGuard);
                    UImanager.instance.SetComputerBlood(computerBlood);
                }
                else
                {
                    computerBlood -= 3;
                    UImanager.instance.SetComputerBlood(computerBlood);
                }
                break;
            case 3:
                playerDrag = true;
                break;
            case 4:
                computerFirstPlay = true;
                playerBlood += 2;
                UImanager.instance.SetPlayerBlood(playerBlood);
                break;
            case 5:
                computerBlood -= playerAttack;
                UImanager.instance.SetComputerBlood(computerBlood);
                break;
            case 6:
                if(playerAttack > computerGuard)
                {
                    computerBlood -= (playerAttack - computerGuard);
                    UImanager.instance.SetComputerBlood(computerBlood);
                }
                break;
            case 7:
                computerDeath++;
                break;
            case 8:
                for (int i = 0; i < 3; i++)
                {
                    if(PlayerCardLibrary.Count > 0)
                    {
                        card = DealCard(ShowPoint.playerHand, PlayerCardLibrary);
                        player.AddCard(card, false, ShowPoint.playerHand);
                    }
                    else
                    {
                        WashPlayerCard();
                        card = DealCard(ShowPoint.playerHand, PlayerCardLibrary);
                        player.AddCard(card, false, ShowPoint.playerHand);
                    }
                }
                break;
            default:
                break;
        }
            

    }

    public void computerEffect(int point)
    {
        if (gameover == true)
        {
            return;
        }
        switch (point)
        {
            case 1:
                WashComputerCard();
                computerGuard++;
                UImanager.instance.SetComputerGuard(computerGuard);
                break;
            case 2:
                if (computerAttack - playerGuard > 3)
                {
                    playerBlood -= (computerAttack - playerGuard);
                    UImanager.instance.SetPlayerBlood(playerBlood);
                }
                else
                {
                    playerBlood -= 3;
                    UImanager.instance.SetPlayerBlood(playerBlood);
                }
                break;
            case 3:
                computerDrag = true;
                break;
            case 4:
                computerBlood += 2;
                UImanager.instance.SetComputerBlood(computerBlood);
                break;
            case 5:
                playerBlood -= computerAttack;
                UImanager.instance.SetPlayerBlood(playerBlood);
                break;
            case 6:
                if (computerAttack > playerGuard)
                {
                    playerBlood -= (computerAttack - playerGuard);
                    UImanager.instance.SetPlayerBlood(playerBlood);
                }
                break;
            case 7:
                playerDeath++;
                break;
            case 8:
                for (int i = 0; i < 3; i++)
                {
                    if (ComputerCardLibrary.Count > 0)
                    {
                        card = DealCard(ShowPoint.computerHand, ComputerCardLibrary);
                        player.AddCard(card, false, ShowPoint.computerHand);
                    }
                    else
                    {
                        WashComputerCard();
                        card = DealCard(ShowPoint.computerHand, ComputerCardLibrary);
                        player.AddCard(card, false, ShowPoint.computerHand);
                    }
                }
                break;
            default:
                break;
        }
    }
    public void playerMove(int point)
    {
        if (gameover == true)
        {
            return;
        }
        UImanager.instance.panels[2].SetActive(true);
        playerPoint = point;
        UImanager.instance.SetPlayerPoint(playerPoint);
    }

    public void magic()
    {
        playerPoint--;
        computerBlood--;
        UImanager.instance.SetPlayerPoint(playerPoint);
        UImanager.instance.SetComputerBlood(computerBlood);
        if (playerPoint == 0)
        {
            endTurn();
        }
        if (computerBlood <= 0)
        {
            gameover = true;
            UImanager.instance.gameover();
        }
        if (playerBlood <= 0)
        {
            gameover = true;
            UImanager.instance.gameover();
        }
    }

    public void power()
    {
        if (playerPoint < playerPowerUp)
            return;
        playerPoint -= playerPowerUp;
        playerPowerUp++;
        playerAttack++;
        UImanager.instance.SetPlayerPoint(playerPoint);
        UImanager.instance.SetPlayerAttack(playerAttack); 
        if (playerPoint == 0)
        {
            endTurn();
        }
        if (computerBlood <= 0)
        {
            gameover = true;
            UImanager.instance.gameover();
        }
        if (playerBlood <= 0)
        {
            gameover = true;
            UImanager.instance.gameover();
        }
    }

    public void guard()
    {
        string value = UImanager.instance.InputField.text;
        if (value == null)
            return;
        int number = int.Parse(value);
        if (number > playerPoint)
        {
            return;
        }
        playerGuard = number;
        playerPoint -= number;
        UImanager.instance.SetPlayerGuard(playerGuard);
        UImanager.instance.SetPlayerPoint(playerPoint);
        if (playerPoint == 0)
        {
            endTurn();
        }
        if (computerBlood <= 0)
        {
            gameover = true;
            UImanager.instance.gameover();
        }
        if (playerBlood <= 0)
        {
            gameover = true;
            UImanager.instance.gameover();
        }
    }

    public void health()
    {
        playerPoint--;
        playerBlood++;
        UImanager.instance.SetPlayerPoint(playerPoint);
        UImanager.instance.SetPlayerBlood(playerBlood);
        if (playerPoint == 0)
        {
            endTurn();
        }
    }

    public void computerMove(int point)
    {
        if (gameover == true)
        {
            return;
        }
        playerBlood -= point;
        UImanager.instance.SetPlayerBlood(playerBlood);
        endTurn();
    }

    public void drag()
    {
        int temp = 0;
        foreach (Transform child in transform.Find("gamePanel/player/playerUsed"))
        {
            CardUI eachUI = child.GetComponent<CardUI>();
            if (eachUI.IsSelected == true)
            {
                eachUI.SetPosition(transform.Find("gamePanel/player/playerHand"), player.cardList.Count + 1);
                eachUI.card.Belongto = ShowPoint.playerHand;
                temp = 1;
            }
        }
        if(temp == 0)
        {
            return;
        }
        else
        {
            UImanager.instance.panels[3].SetActive(false);
            if (player.cardList.Count < 3)
            {
                if (PlayerCardLibrary.Count == 0)
                {
                    WashPlayerCard();
                }
                card = DealCard(ShowPoint.playerHand, PlayerCardLibrary);
                player.AddCard(card, false, ShowPoint.playerHand);
            }
            if (computer.cardList.Count < 3)
            {
                if (ComputerCardLibrary.Count == 0)
                {
                    WashComputerCard();
                }
                card = DealCard(ShowPoint.computerHand, ComputerCardLibrary);
                computer.AddCard(card, false, ShowPoint.computerHand);
            }
            playerBlood -= playerDeath;
            UImanager.instance.SetPlayerBlood(playerBlood);
            computerBlood -= computerDeath;
            UImanager.instance.SetComputerBlood(computerBlood);
            playerPlayed = false;
            playerDrag = false;
            computerDrag = false;
            Invoke("computerPlay", 1);
        }
    }

    public void playerAgain()
    {
        PlayerUsedCardLibrary.Clear();
        ComputerUsedCardLibrary.Clear();
        player.cardList.Clear();
        computer.cardList.Clear();
        foreach (Transform child in transform.Find("gamePanel/player/playerUsed"))
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in transform.Find("gamePanel/computer/computerUsed"))
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in transform.Find("gamePanel/computer/computerHand"))
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in transform.Find("gamePanel/player/playerHand"))
        {
            Destroy(child.gameObject);
        }
        gameover = false;
        computerPlayed = false;
        playerPlayed = false;

        computerAttack = 1;
        computerGuard = 0;
        computerBlood = 20;
        computerDeath = 0;
        computerFirstPlay = false;
        computerDrag = false;
        playerAttack = 1;
        playerGuard = 0;
        playerBlood = 20;
        playerDeath = 0;
        playerDrag = false;
        playerPoint = 0;
        playerPowerUp = 1;
        UImanager.instance.panels[2].SetActive(false);
        UImanager.instance.panels[3].SetActive(false);
        UImanager.instance.panels[4].SetActive(false);
        StartGame();
    }

    public void quit()
    {
        Application.Quit();
    }
    #region 牌库相关
    public void InitComputerCardLibrary()
    {
        for(int i = 1; i < 10; i++)
        {
            string name = i.ToString();
            card card = new card(name, i, constant.ShowPoint.computerLibrary);
            ComputerCardLibrary.Enqueue(card);
        }
    }

    public void InitPlayerCardLibrary()
    {
        for (int i = 1; i < 10; i++)
        {
            string name = i.ToString();
            card card = new card(name, i, constant.ShowPoint.playerLibrary);
            PlayerCardLibrary.Enqueue(card);
        }
    }

    public void Shuffle(Queue<card> CardLibrary)
    {
        List<card> newList = new List<card>();
        foreach (var card in CardLibrary)
        {
            int index = Random.Range(0, newList.Count + 1);
            newList.Insert(index, card);
        }

        CardLibrary.Clear();
        foreach (var card in newList)
        {
            CardLibrary.Enqueue(card);
        }

        newList.Clear();
    }

    public card DealCard(ShowPoint sendTo, Queue<card> CardLibrary)
    {
        card card = CardLibrary.Dequeue();
        card.Belongto = sendTo;
        return card;
    }

    public void WashPlayerCard()
    {
        foreach (Transform child in transform.Find("gamePanel/player/playerUsed"))
        {
            CardUI eachUI = child.GetComponent<CardUI>();
            eachUI.Destroy();
        }
        int j = PlayerUsedCardLibrary.Count;    
        for (int i = 0; i < j; i++)
        {
            card card = PlayerUsedCardLibrary.Dequeue();
            PlayerCardLibrary.Enqueue(card);
        }

        Shuffle(PlayerCardLibrary);
    }

    public void WashComputerCard()
    {
        foreach (Transform child in transform.Find("gamePanel/computer/computerUsed"))
        {
            CardUI eachUI = child.GetComponent<CardUI>();
            eachUI.Destroy();
        }
        int j = ComputerUsedCardLibrary.Count;
        for (int i = 0; i < j; i++)
        {
            card card = ComputerUsedCardLibrary.Dequeue();
            ComputerCardLibrary.Enqueue(card);
        }

        Shuffle(ComputerCardLibrary);
    }

    #endregion



}
