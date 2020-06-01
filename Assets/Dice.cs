using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dice : MonoBehaviour {

    private Sprite[] diceSides;
    public SpriteRenderer dice1;
    public SpriteRenderer dice2;
    private bool coroutineAllowed = true;

	// Use this for initialization
	private void Start () {
        diceSides = Resources.LoadAll<Sprite>("DiceSides/");
        dice1.sprite = diceSides[5];
        dice2.sprite = diceSides[5];
	}

    public void ThrowDice()
    {
        if (coroutineAllowed)
            StartCoroutine("RollTheDice");
    }

    private IEnumerator RollTheDice()
    {
        coroutineAllowed = false;
        GameControl.diceButton.SetActive(false);
        int randomDiceSide = 0;
        int randomDiceSide2 = 0;
        for (int i = 0; i <= 20; i++)
        {
            randomDiceSide = Random.Range(0, 6);
            randomDiceSide2 = Random.Range(0, 6);
            dice1.sprite = diceSides[randomDiceSide];
            dice2.sprite = diceSides[randomDiceSide2];
            yield return new WaitForSeconds(0.05f);
        }

        GameControl.diceSideThrown = randomDiceSide + randomDiceSide2 + 2;
        GameControl.activePlayer.lastDiceSideThrown = randomDiceSide + randomDiceSide2 + 2;
        GameControl.activePlayer.canMove = true;
        GameControl.activePlayer.countGoForGameCards += randomDiceSide + randomDiceSide2 + 2;
        coroutineAllowed = true;
    }
}
