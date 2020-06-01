using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public Transform[] points;
    public GameObject OwnerSpriteGameObject;
    public GameObject houseSpriteGameObject;
    [HideInInspector] public SpriteRenderer SpriteRenderer;
    [HideInInspector] public SpriteRenderer HouseSpriteRenderer;

    [SerializeField]
    private float moveSpeed = 1f;

    [HideInInspector]
    public int pointIndex = 0;

    public bool canMove = false;
    [HideInInspector] public bool wasMove = false;
    [HideInInspector] public bool atStart = false;
    [HideInInspector] public long lastDiceSideThrown = 0;
    public GameObject playerInfo;

    public String name;

    [HideInInspector]
    public int startPoint = 0;

    [HideInInspector] public long balance = 0;

    [HideInInspector]
    public long addSumForGoStart = 0;
    
    [HideInInspector]
    public Transform currentPosition;
    [HideInInspector]
    public Text playerInfoText;
    [HideInInspector]
    public long countCards = 0;
    [HideInInspector]
    public long countGoForGameCards = 0;
    [HideInInspector]
    public long spendMoney = 0;
    [HideInInspector]
    public long recieveMoney = 0;
    [HideInInspector]
    public List<GameCard> gameCards = new List<GameCard>();
    
    // Start is called before the first frame update
    void Start()
    {
        transform.position = points[pointIndex].transform.position;
        playerInfoText = playerInfo.GetComponent<Text>();
        
        SpriteRenderer = OwnerSpriteGameObject.GetComponent<SpriteRenderer>();
        HouseSpriteRenderer = houseSpriteGameObject.GetComponent<SpriteRenderer>();
        updateInofBalance();
        SetCurrentPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            Move();
        }
    }

    public void updateInofBalance()
    {
        playerInfoText.text = name + "\n Balance: " + balance;
    }

    public void SetCurrentPosition()
    {
        currentPosition = points[startPoint];
    }

    public void CheckCards()
    {
        foreach (var gameCard in gameCards)
        {
            if (gameCard.couldReturnToPlayer)
            {
                gameCard.returnTurnCount++;
            }

            if (gameCard.returnTurnCount > 3)
            {
                gameCard.ReturnToBank();
            }
        }

        gameCards.RemoveAll(card => card.returnTurnCount > 3);
    }

    private void Move()
    {
        if (pointIndex <= points.Length - 1)
        {
            transform.position = Vector2.MoveTowards(transform.position, points[pointIndex].transform.position, moveSpeed * Time.deltaTime);

            if (transform.position == points[pointIndex].transform.position)
            {
                pointIndex += 1;
            }
        }
        else
        {
            pointIndex = 0;
            if (startPoint + GameControl.diceSideThrown == points.Length)
            {
                atStart = true;
            }
            GameControl.diceSideThrown = GameControl.diceSideThrown - (points.Length - startPoint);
            lastDiceSideThrown = GameControl.diceSideThrown;
            
            startPoint = 0;
            atStart = true;
            balance = balance + addSumForGoStart;
            recieveMoney += addSumForGoStart;
            updateInofBalance();
            
            transform.position = Vector2.MoveTowards(transform.position, points[pointIndex].transform.position, moveSpeed * Time.deltaTime);

            if (transform.position == points[pointIndex].transform.position)
            {
                pointIndex += 1;
            }
        }
    }

    protected bool Equals(Player other)
    {
        return base.Equals(other) && name.Equals(other.name);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Player) obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (base.GetHashCode() * 397) ^ (name != null ? name.GetHashCode() : 0);
        }
    }
}
