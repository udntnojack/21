using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    private string _suit;
    private int _value;
    [SerializeField] TMP_Text topText;
    [SerializeField] TMP_Text bottomText;
    [SerializeField] Sprite[] suits;


    public string suit
    {
        get { return _suit; }
    }
    public int value
    {
        get { return _value; }
    }

    public void setCard(int num, string shape)
    {
        _suit = shape;
        if (num == 1) 
        {
            _value = num;
            topText.text = "A";
            bottomText.text = "A";
        }
        else if (num > 1 && num < 11)
        { 
            _value = num;
            topText.text = value.ToString();
            bottomText.text = value.ToString();
        }
        else if(num == 11)
        {
            _value = 10;
            topText.text = "J";
            bottomText.text = "J";
        }
        else if(num == 12) 
        {
            _value = 10;
            topText.text = "Q";
            bottomText.text = "Q";
        }
        else if(num == 13)
        {
            _value = 10;
            topText.text = "K";
            bottomText.text = "K";
        }
        Debug.Log(topText.text);



        var gameObject = new GameObject();
        gameObject.transform.SetParent(this.transform);
        gameObject.transform.localPosition = new Vector3(0f, 0f, -1f);


        var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();


        if (shape == "diamonds")
        {
            spriteRenderer.sprite = suits[0];
        }
        else if (shape == "clubs")
        {
            spriteRenderer.sprite = suits[3];
        }
        else if (shape == "spades")
        {
            spriteRenderer.sprite = suits[2];
        }
        else
        {
            spriteRenderer.sprite = suits[1];
        }
    }
}
