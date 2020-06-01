using UnityEngine;

public class Card : MonoBehaviour
{
    
    public GameCard gameCard;

    public void OnMouseDown()
    {
        gameCard.Appear();
    }
}
