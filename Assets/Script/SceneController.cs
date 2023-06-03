using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class SceneController : MonoBehaviour
{
    private int cardNo = 0;
    private int totalValue = 0;

    public float offset = 0.20f;
    [SerializeField] Card card;

    public int[] cards;
    public Vector3 startPos;
    private void Start()
    {

        cards = Enumerable.Range(1, 53).ToArray();
        cards = shuffle(cards);

        Card firstCard = Instantiate(card) as Card;
        int value = getValue(cards[cardNo]);
        string suit = getSuit(cards[cardNo]);
        firstCard.setCard(value, suit);
        firstCard.transform.position = new Vector3(-7.85f, -2.65f, -10f);

        startPos = firstCard.transform.position;

        cardNo++;

        Card secondCard = Instantiate(card) as Card;
        secondCard.transform.position = new Vector3(startPos.x + 3, startPos.y , startPos.z);
        value = getValue(cards[cardNo]);
        suit = getSuit(cards[cardNo]);
        secondCard.setCard(value, suit);

    }
    private int getValue(int cards)
    {
        int value = cards % 13;
        if (value > 0) 
        {
            return value;
        }
        else
        {
            return 13;
        }
    }
    private string getSuit(int cards)
    {
        if (cards >= 1 && cards <= 13)
        {
            return "hearts";
        }
        else if (cards > 13 && cards <= 26)
        {
            return "clubs";

        }
        else if (cards > 26 && cards <= 39)
        {
            return "diamonds";
        }
        else
        {
            return "spades";
        }
    }


    private int[] shuffle(int[] cards)
    {
        int[] newCards = cards.Clone() as int[];
        for (int i = 0; i < newCards.Length; i++)
        {
            int tmp = cards[i];
            int r  = Random.Range(i, newCards.Length);
            newCards[i] = newCards[r];
            newCards[r] = tmp;
        }
        return newCards;
    }
    public void Reset()
    {
        
    }
    public void Stick()
    {

    }
    public void Hit()
    {
        cardNo++;
        Card secondCard = Instantiate(card) as Card;
        secondCard.transform.position = new Vector3(startPos.x + (3 * cardNo), startPos.y, startPos.z);
        int value = getValue(cards[cardNo]);
        string suit = getSuit(cards[cardNo]);
        secondCard.setCard(value, suit);    
    }
}
