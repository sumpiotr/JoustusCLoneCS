using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{

    [SerializeField]
    DeckScriptableObject blueDeck;
    [SerializeField]
    DeckScriptableObject redDeck;

    List<Card> blueCards = new List<Card>();
    List<Card> redCards = new List<Card>();
    public List<CardObject> bluePlayerHand = new List<CardObject>();
    public List<CardObject> redPlayerHand = new List<CardObject>();
    void Start()
    {
        for (int i = 0; i < 16; i++)
        {
            blueCards.Add(new Card());
            redCards.Add(new Card());
        }
        InitializeCards();
    }

    void InitializeCards() 
    {
        int i = 0;
        foreach(Card card in blueCards) 
        {
            if (i >= blueDeck.cards.Count) break;
            CardScriptableObject cardData = blueDeck.cards[i];
            card.InitializeCard(new int[] {cardData.upArrow, cardData.rightArrow, cardData.bottomArrow, cardData.leftArrow}, cardData.blueImage, "blue");
            i++;
        }
        i = 0;
        foreach (Card card in redCards)
        {
            if (i >= redDeck.cards.Count) break;
            CardScriptableObject cardData = redDeck.cards[i];
            card.InitializeCard(new int[] { cardData.upArrow, cardData.rightArrow, cardData.bottomArrow, cardData.leftArrow }, cardData.redImage, "red");
            i++;
        }
    }

    void Update()
    {
        
    }

    public void DrawCard(Vector2 position, string color) 
    {
        if(color == "blue") 
        {
            int randomIndex = Random.Range(0, blueCards.Count);
            blueCards[randomIndex].CreateCard(position.x, position.y);
            blueCards.RemoveAt(randomIndex);
        }
        else 
        {
            int randomIndex = Random.Range(0, redCards.Count);
            redCards[randomIndex].CreateCard(position.x, position.y);
            redCards.RemoveAt(randomIndex);
        }
      
    }

    public void RemoveFromHand(CardObject card, string color) 
    {
        if(color == "blue") 
        {
            bluePlayerHand.Remove(card);
        }
        else 
        {
            redPlayerHand.Remove(card);
        }
    }
}
