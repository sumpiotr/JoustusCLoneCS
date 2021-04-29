using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
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
        simpleBlueDeck();
        SimpleRedDeck();
    }

    void simpleBlueDeck() 
    {
        blueCards[0].InitializeCard("1 0 0 0", "blue");
        blueCards[1].InitializeCard("0 0 0 1", "blue");
        blueCards[2].InitializeCard("0 1 0 0", "blue");
        blueCards[3].InitializeCard("1 1 0 0", "blue");
        blueCards[4].InitializeCard("0 1 1 0", "blue");
        blueCards[5].InitializeCard("0 0 1 1", "blue");
        blueCards[6].InitializeCard("1 0 1 0", "blue");
        blueCards[7].InitializeCard("1 0 1 0", "blue");
        blueCards[8].InitializeCard("0 1 0 1", "blue");
        blueCards[9].InitializeCard("1 0 1 1", "blue");
        blueCards[10].InitializeCard("0 1 1 1", "blue");
        blueCards[11].InitializeCard("1 1 0 0", "blue");
        blueCards[12].InitializeCard("0 1 1 0", "blue");
        blueCards[13].InitializeCard("1 0 1 0", "blue");
        blueCards[14].InitializeCard("0 1 0 1", "blue");
        blueCards[15].InitializeCard("0 1 1 1", "blue");
    }

    void SimpleRedDeck() 
    {
        redCards[0].InitializeCard("0 1 1 0", "red");
        redCards[1].InitializeCard("1 1 0 0", "red");
        redCards[2].InitializeCard("0 1 0 0", "red");
        redCards[3].InitializeCard("1 0 0 0", "red");
        redCards[4].InitializeCard("0 0 0 1", "red");
        redCards[5].InitializeCard("0 1 0 1", "red");
        redCards[6].InitializeCard("0 1 0 1", "red");
        redCards[7].InitializeCard("1 0 1 0", "red");
        redCards[8].InitializeCard("1 0 1 0", "red");
        redCards[9].InitializeCard("1 0 0 0", "red");
        redCards[10].InitializeCard("1 0 0 1", "red");
        redCards[11].InitializeCard("1 0 0 1", "red");
        redCards[12].InitializeCard("0 0 1 1", "red");
        redCards[13].InitializeCard("0 0 1 1", "red");
        redCards[14].InitializeCard("1 1 0 1", "red");
        redCards[15].InitializeCard("1 1 1 0", "red");
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
