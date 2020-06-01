using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    private Player player1, player2;
    public static Player winplayer, losePlayer;

    public static int diceSideThrown = 0;
    public static Player activePlayer;
    public long addBalanceForStart;
    public long startBalance;
    public Dice dice;

    public static GameObject diceButton;
    public static GameObject endTurnButton;
    public static GameObject bankrotButton;
    public static GameObject endGameButton;
    public static bool isCardAppear = false;
    private int whosTurn = 0;
    private Player[] players;
    private GameCard[] gameCards;
    private Text whoseTurnText;


    // Use this for initialization
    void Start()
    {
        players = GameObject.Find("Players").GetComponentsInChildren<Player>();
        player1 = players[0];
        player2 = players[1];

        player1.balance = startBalance;
        player2.balance = startBalance;
        player1.addSumForGoStart = addBalanceForStart;
        player2.addSumForGoStart = addBalanceForStart;

        player1.canMove = false;
        player2.canMove = false;
        activePlayer = player1;
        diceButton = GameObject.Find("DiceButton");
        endTurnButton = GameObject.Find("EndTurn");
        bankrotButton = GameObject.Find("BankrotButton");
        endGameButton = GameObject.Find("EndGameButton");
        whoseTurnText = GameObject.Find("WhoseTurn").GetComponent<Text>();
        bankrotButton.SetActive(false);
        endTurnButton.SetActive(false);
        activePlayer = players[whosTurn];
        whoseTurnText.text = ("Хід гравця:\n" + activePlayer.name).ToUpperInvariant();
        gameCards = GameObject.Find("GameCards").GetComponentsInChildren<GameCard>();
        foreach (var gameCard in gameCards)
        {
            gameCard.ClickContinue();
        }

        player2.updateInofBalance();
    }

    // Update is called once per frame
    void Update()
    {

        if (activePlayer.pointIndex > activePlayer.startPoint + activePlayer.lastDiceSideThrown)
        {
            activePlayer.canMove = false;
            activePlayer.wasMove = true;
            activePlayer.startPoint = activePlayer.pointIndex - 1;
            if (activePlayer.atStart)
            {
                activePlayer.atStart = false;
                activePlayer.pointIndex -= 1;
            }

            activePlayer.SetCurrentPosition();
            diceButton.SetActive(false);
            GameCard card = findCardForposition(activePlayer);
            if (card != null)
            {
                card.PlayerComeAt();
            }
            else
            {
                ChangeButtonToEndTurn();
            }
        }
    }

    public static void ChangeButtonToEndTurn()
    {
        if (activePlayer.balance < 0)
        {
            bankrotButton.SetActive(true);
            endTurnButton.SetActive(false);
        }
        else
        {
            endTurnButton.SetActive(true);
            bankrotButton.SetActive(false);
        }
    }

    public void ThrowDice()
    {
        dice.ThrowDice();
    }

    public void EndTurn()
    {
        if (whosTurn >= players.Length - 1)
        {
            whosTurn = -1;
        }

        activePlayer.CheckCards();
        activePlayer = players[++whosTurn];
        whoseTurnText.text = ("Хід гравця:\n" + activePlayer.name).ToUpperInvariant();
        activePlayer.wasMove = false;
        endTurnButton.SetActive(false);
        diceButton.SetActive(true);
    }

    public void EndGame()
    {
        losePlayer = activePlayer;
        foreach (var player in players)
        {
            if (!player.Equals(losePlayer))
            {
                winplayer = player;
                break;
            }
        }

        losePlayer.countCards = losePlayer.gameCards.Count;
        winplayer.countCards = winplayer.gameCards.Count;
        SceneManager.LoadScene("Scenes/EndGame");
    }

    private GameCard findCardForposition(Player player)
    {
        foreach (var gameCard in gameCards)
        {
            if (gameCard.position.Equals(player.currentPosition))
            {
                return gameCard;
            }
        }

        return null;
    }

    private int PlayersGardCount(Player player)
    {
        int count = 0;
        foreach (var gameCard in gameCards)
        {
            if (gameCard.owner != null && gameCard.owner.Equals(player))
            {
                count++;
            }
        }

        return count;
    }
}