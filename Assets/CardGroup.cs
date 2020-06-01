using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGroup : MonoBehaviour
{
    public GameCard[] cards;

    [HideInInspector] public bool couldBuild = false;

    [HideInInspector] public bool couldReturnToBank = true;
    [HideInInspector] public Player owner;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateInfo()
    {
        GameCard card = cards[0];
        bool oneOwner = true;
        for (int i = 1; i < cards.Length; i++)
        {
            if (card.owner == null || !card.owner.Equals(cards[i].owner))
            {
                oneOwner = false;
                break;
            }
        }
        

        if (oneOwner)
        {
            owner = card.owner;
            couldBuild = true;
            couldReturnToBank = true;
            for (int i = 0; i < cards.Length; i++)
            {
                if (cards[i].domCount > 0)
                {
                    couldReturnToBank = false;
                }

                if (cards[i].couldReturnToPlayer)
                {
                    couldBuild = false;
                }
            }
        }
        else
        {
            owner = null;
            couldBuild = false;
            couldReturnToBank = true;
        }
    }
}
