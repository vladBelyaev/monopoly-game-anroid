using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameCard : MonoBehaviour
{
    public GameObject gameCardCanvas;
    public GameObject buyButtonObject;
    public GameObject returnToBankButtonObject;
    public GameObject returnToPlayerButtonObject;
    public GameObject buyDomButtonObject;
    public GameObject sellDomButtonObject;
    public Transform position;
    public string name;
    public int price;
    public int rent;
    public GameObject ownerSpriteGameObject;
    public GameObject[] houseSpriteGameObjects;
    public int[] rents;
    public int[] domPrice;
    public CardGroup cardGroup;
    [HideInInspector] public int[] domReturnPrice;

    [HideInInspector] public int currentRent;
    [HideInInspector] public Player owner;
    [HideInInspector] public int priceForReturn;
    [HideInInspector] public int priceForReturnToPlayer;
    [HideInInspector] public SpriteRenderer ownerSpriteRender;
    [HideInInspector] public SpriteRenderer[] houseSpriteRenders;
    [HideInInspector] public int returnTurnCount;
    [HideInInspector] public bool couldReturnToPlayer = false;
    [HideInInspector] public int domCount = 0;
     private Text returnToBankText;
     private Text returnToPlayerText;
     private Text sellDomText;
     private Text buyDomText;


    // Start is called before the first frame update
    void Start()
    {
        currentRent = rent;
        ownerSpriteRender = ownerSpriteGameObject.GetComponent<SpriteRenderer>();
        houseSpriteRenders = new SpriteRenderer[houseSpriteGameObjects.Length];
        for (int i = 0; i < houseSpriteRenders.Length; i++)
        {
            houseSpriteRenders[i] = houseSpriteGameObjects[i].GetComponent<SpriteRenderer>();
        }

        priceForReturn = price / 2;
        priceForReturnToPlayer = (int) (priceForReturn * 1.1);
        returnToBankText = returnToBankButtonObject.GetComponentInChildren<Text>();
        returnToPlayerText = returnToPlayerButtonObject.GetComponentInChildren<Text>();
        buyDomText = buyDomButtonObject.GetComponentInChildren<Text>();
        sellDomText = sellDomButtonObject.GetComponentInChildren<Text>();
        buyButtonObject.GetComponentInChildren<Text>().text = "Придбати -$" + price;
        buyButtonObject.GetComponentInChildren<Text>().fontSize = 20;
        returnToBankText.text = "Закласти +$" + priceForReturn;
        returnToBankText.fontSize = 20;
        returnToPlayerText.text = "Викупити -$" + priceForReturnToPlayer;
        returnToPlayerText.fontSize = 20;
        returnToBankButtonObject.SetActive(false);
        returnToPlayerButtonObject.SetActive(false);
        buyDomButtonObject.SetActive(false);
        sellDomButtonObject.SetActive(false);
        domReturnPrice = new int[domPrice.Length];

        for (int i = 0; i < domPrice.Length; ++i)
        {
            domReturnPrice[i] = domPrice[i] / 2;
        }
    }

    public void Appear()
    {
        if (GameControl.isCardAppear)
        {
            return;
        }

        GameControl.isCardAppear = true;
        GameControl.diceButton.SetActive(false);
        GameControl.endTurnButton.SetActive(false);
        GameControl.bankrotButton.SetActive(false);
        GameControl.endGameButton.SetActive(false);
        buyButtonObject.SetActive(false);
        sellDomButtonObject.SetActive(false);
        buyDomButtonObject.SetActive(false);

        if (GameControl.activePlayer.Equals(owner))
        {
            UpdateCardGroupAndButtons();
        }
        else
        {
            returnToBankButtonObject.SetActive(false);
            returnToPlayerButtonObject.SetActive(false);
        }

        gameCardCanvas.SetActive(true);
    }

    public void ClickContinue()
    {
        if (!GameControl.activePlayer.wasMove)
        {
            GameControl.diceButton.SetActive(true);
        }
        else if (GameControl.activePlayer.balance < 0)
        {
            GameControl.bankrotButton.SetActive(true);
        }
        else
        {
            GameControl.endTurnButton.SetActive(true);
        }

        GameControl.endGameButton.SetActive(true);
        gameCardCanvas.SetActive(false);
        GameControl.isCardAppear = false;
    }

    public void PlayerComeAt()
    {
        if (owner == null)
        {
            Appear();
            buyButtonObject.SetActive(true);
        }
        else
        {
            if (couldReturnToPlayer)
            {
                GameControl.ChangeButtonToEndTurn();
                return;
            }

            GameControl.activePlayer.balance -= currentRent;
            GameControl.activePlayer.spendMoney += currentRent;
            GameControl.activePlayer.updateInofBalance();
            owner.balance += currentRent;
            owner.recieveMoney += currentRent;
            owner.updateInofBalance();
            GameControl.ChangeButtonToEndTurn();
        }
    }

    public void Buy()
    {
        if (owner == null)
        {
            GameControl.activePlayer.balance -= price;
            GameControl.activePlayer.spendMoney += price;
            owner = GameControl.activePlayer;
            owner.gameCards.Add(this);
            ownerSpriteRender.sprite = owner.SpriteRenderer.sprite;
            ownerSpriteRender.color = Color.white;
            owner.updateInofBalance();
            buyButtonObject.SetActive(false);
            returnToBankButtonObject.SetActive(true);
            couldReturnToPlayer = false;
            currentRent = rent;
            UpdateCardGroupAndButtons();
        }
    }

    public void StartReturnToBank()
    {
        if (owner != null && domCount == 0)
        {
            GameControl.activePlayer.balance += priceForReturn;
            GameControl.activePlayer.recieveMoney += priceForReturn;
            ownerSpriteRender.color = Color.grey;
            owner.updateInofBalance();
            couldReturnToPlayer = true;
            UpdateCardGroupAndButtons();
        }
    }

    public void ReturnToPlayer()
    {
        if (owner != null)
        {
            GameControl.activePlayer.balance -= priceForReturnToPlayer;
            GameControl.activePlayer.spendMoney += priceForReturnToPlayer;
            ownerSpriteRender.color = Color.white;
            owner.updateInofBalance();
            couldReturnToPlayer = false;
            returnTurnCount = 0;
            UpdateCardGroupAndButtons();
        }
    }

    public void ReturnToBank()
    {
        ownerSpriteRender.sprite = null;
        owner = null;
        couldReturnToPlayer = false;
        UpdateCardGroupAndButtons();
    }

    public void BuyDom()
    {
        currentRent = rents[domCount];
        houseSpriteRenders[domCount].sprite = owner.HouseSpriteRenderer.sprite;
        GameControl.activePlayer.spendMoney += domPrice[domCount];
        GameControl.activePlayer.balance -= domPrice[domCount];
        domCount += 1;
        UpdateCardGroupAndButtons();
        owner.updateInofBalance();
    }

    public void SellDom()
    {
        domCount -= 1;
        houseSpriteRenders[domCount].sprite = null;
        GameControl.activePlayer.recieveMoney += domPrice[domCount];
        GameControl.activePlayer.balance += domPrice[domCount];
        if (domCount == 0)
        {
            currentRent = rent;
        }
        else
        {
            currentRent = rents[domCount - 1];
        }

        UpdateCardGroupAndButtons();
        owner.updateInofBalance();
    }

    private void UpdateCardGroupAndButtons()
    {
        cardGroup.UpdateInfo();
        UpdateDomButtons();
    }

    private void UpdateDomButtons()
    {
        if (cardGroup.couldReturnToBank)
        {
            if (couldReturnToPlayer)
            {
                returnToPlayerButtonObject.SetActive(true);
                returnToBankButtonObject.SetActive(false);
                sellDomButtonObject.SetActive(false);
                buyDomButtonObject.SetActive(false);
            }
            else
            {
                returnToBankButtonObject.SetActive(true);
                returnToPlayerButtonObject.SetActive(false);
            }
        }
        else
        {
            returnToBankButtonObject.SetActive(false);
            returnToPlayerButtonObject.SetActive(false);
        }

        if (cardGroup.couldBuild)
        {
            if (domCount > 0)
            {
                sellDomButtonObject.SetActive(true);
                sellDomText.text = "Продати будинок +$" + domReturnPrice[domCount - 1];
            }
            else
            {
                sellDomButtonObject.SetActive(false);
            }

            if (domCount < domPrice.Length)
            {
                buyDomButtonObject.SetActive(true);
                buyDomText.text = "Придбати будинок -$" + domPrice[domCount];
            }
            else
            {
                buyDomButtonObject.SetActive(false);
            }
        }
    }
}