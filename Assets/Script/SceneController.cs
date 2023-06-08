using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private int cardNo = 0;

    public float offset = 0.20f;
    [SerializeField] Card card;

    [SerializeField] TMP_Text scoreLabel;
    [SerializeField] TMP_Text roundMessage;
    [SerializeField] TMP_Text livesText;

    public int scoreTotal;
    public int[] cards;
    public Vector3 startPos;
    public int scoreTop = 0;
    public int scoreBottom = 0;
    public int finalScore = 0;

    private bool canHit = true;
    private bool canStick = true;

    private bool compTurn = false;

    private int compScoreTop = 0;
    private int compscoreBottom = 0;

    private int playerCardCount = 2;

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
    public void resetGame()
    {
        SceneManager.LoadScene("game");
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
    public void Start()
    {
        scoreLabel.text = $"Score: {score.scoreValue}";
        livesText.text = $"lives: {score.lives}";
        cards = Enumerable.Range(1, 53).ToArray();
        cards = shuffle(cards);

        Card firstCard = Instantiate(card) as Card;
        int value = getValue(cards[cardNo]);
        string suit = getSuit(cards[cardNo]);
        firstCard.setCard(value, suit);
        firstCard.transform.position = new Vector3(-7.85f, -2.65f, -10f);

        scoreBottom += getBottomScore(value);
        scoreTop += getTopScore(value);

        startPos = firstCard.transform.position;

        cardNo++;

        Card secondCard = Instantiate(card) as Card;
        secondCard.transform.position = new Vector3(startPos.x + 2.3f, startPos.y, startPos.z);
        value = getValue(cards[cardNo]);
        suit = getSuit(cards[cardNo]);
        secondCard.setCard(value, suit);

        scoreBottom += getBottomScore(value);
        scoreTop += getTopScore(value);

        cardNo++;
    }

    public int getTopScore(int value)
    {
        if (value == 1)
        {
            return 11;
        }
        else if(value == 11 || value == 12 || value == 13)
        {
            return 10;
        }
        else
        {
            return value;
        }
    }
    public int getBottomScore(int value)
    {
        if (value == 1)
        {
            return 1;
        }
        else if (value == 11 || value == 12 || value == 13)
        {
            return 10;
        }
        else
        {
            return value;
        }
    }
    public void Stick()
    {
        if (canStick)
        {
            canStick = false;
            canHit = false;
            compTurn = true;
            if (scoreTop <= 21)
            {
                finalScore = scoreTop;
            }
            else
            {
                finalScore = scoreBottom;
            }
            computersTurn();
        }
    }
    public void Hit()
    {
        if (canHit)
        {
            drawCard();
        }
    }
    public void drawCard()
    {
        
        Card secondCard = Instantiate(card) as Card;
        if (compTurn)
        {
            secondCard.transform.position = new Vector3(startPos.x + (2.3f * (cardNo - playerCardCount)), startPos.y, startPos.z);
        }
        else
        {
            secondCard.transform.position = new Vector3(startPos.x + (2.3f * cardNo), startPos.y, startPos.z);
        }

        int value = getValue(cards[cardNo]);
        string suit = getSuit(cards[cardNo]);
        secondCard.setCard(value, suit);

        cardNo++;

        if (compTurn)
        {
            compscoreBottom += getBottomScore(value);
            compScoreTop += getTopScore(value);
            StartCoroutine(checkWin());
        }
        else
        {
            playerCardCount++;
            scoreBottom += getBottomScore(value);
            scoreTop += getTopScore(value);
            StartCoroutine(checkBust(scoreBottom));
        }
    }
    public IEnumerator checkBust(int scoreTotal)
    {
        if (scoreTotal > 21)
        {
            score.lives--;
            canStick = false;
            canHit = false;
            StartCoroutine(roundEnd("bust"));
            yield return 0;

        }
    }
    public IEnumerator checkWin()
    {
        string message="";

        if ((compscoreBottom > finalScore && compscoreBottom <= 21) || (compScoreTop <= 21 && compScoreTop > finalScore))
        {
            score.lives--;
            message = "you lose";
            StartCoroutine(roundEnd(message));
        }
        else if (compscoreBottom == finalScore)
        {
            message = "draw";
            StartCoroutine(roundEnd(message));
        }
        else if (compscoreBottom < finalScore)
        {
            yield return new WaitForSeconds(2f);
            drawCard();
        }
        else if (compscoreBottom > 21)
        {
            score.scoreValue++;
            score.lives++;
            message = "winner";
            StartCoroutine(roundEnd(message));
            //player wins
        }
    }
    public IEnumerator roundEnd(string message)
    {
        scoreLabel.text = $"Score: {score.scoreValue}";
        livesText.text = $"lives: {score.lives}";
        roundMessage.text = message;
        yield return new WaitForSeconds(2f);
        roundMessage.text = "";
        if (score.lives == 0)
        {
            SceneManager.LoadScene("gameOver");
        }
    }
    public void computersTurn()
    {

        Card firstCard = Instantiate(card) as Card;
        int value = getValue(cards[cardNo]);
        string suit = getSuit(cards[cardNo]);
        firstCard.setCard(value, suit);
        firstCard.transform.position = new Vector3(-7.85f, 3.8f, -10f);

        compscoreBottom += getBottomScore(value);
        compScoreTop += getTopScore(value);

        startPos = firstCard.transform.position;

        cardNo++;

        Card secondCard = Instantiate(card) as Card;
        secondCard.transform.position = new Vector3(startPos.x + 2.3f, startPos.y, startPos.z);
        value = getValue(cards[cardNo]);
        suit = getSuit(cards[cardNo]);
        secondCard.setCard(value, suit);

        compscoreBottom += getBottomScore(value);
        compScoreTop += getTopScore(value);

        cardNo++;

        StartCoroutine(checkWin());
    }
}