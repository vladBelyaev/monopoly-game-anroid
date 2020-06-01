using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{

    public Text header;
    public Text player1;
    public Text player2;
    // Start is called before the first frame update
    void Start()
    {
            Player winplayer = GameControl.winplayer;
            Player losePlayer = GameControl.losePlayer;
            header.text = "Вийграв ігрок " + winplayer.name;
            player1.text = "Ігрок: " + winplayer.name + "\n Баланс: " + winplayer.balance + "\n Кількість карток: " + winplayer.countCards
                           + "\n Потрачено грошей: " + winplayer.spendMoney + "\n Отримано грошей під час гри: " + winplayer.recieveMoney
                           + "\n Пройдено клітинок: " + winplayer.countGoForGameCards;
            player2.text = "Ігрок: " + losePlayer.name + "\n Баланс: " + losePlayer.balance + "\n Кількість карток: " + losePlayer.countCards
                           + "\n Потрачено грошей: " + losePlayer.spendMoney + "\n Отримано грошей під час гри: " + losePlayer.recieveMoney
                           + "\n Пройдено клітинок: " + losePlayer.countGoForGameCards;
    }

   

    public void GoToMenu()
    {
        SceneManager.LoadScene("Scenes/Menu");
    }
}
