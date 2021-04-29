using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    Vector2 deafultPosition1 = new Vector2(-Board.MAIN_X * 4, Board.MAIN_Y * 2);
    Vector2 deafultPosition2 = new Vector2(-Board.MAIN_X * 4, Board.MAIN_Y * 0);
    Vector2 deafultPosition3 = new Vector2(-Board.MAIN_X * 4, Board.MAIN_Y * -2);
    Deck deck;

    [SerializeField]
    GameObject endGamePanel;

    GameObject cardContainer;


    [SerializeField]
    Text infoText;

    public bool blueTurn;

    public static GameManager instance;


    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }
    public void StartSetup()
    {
        cardContainer = Media.instance.cardContainer;
        endGamePanel.SetActive(false);
        cardContainer.SetActive(true);
        deck = FindObjectOfType<Deck>();
        deck.DrawCard(deafultPosition1, "blue");
        deck.DrawCard(deafultPosition2, "blue");
        deck.DrawCard(deafultPosition3, "blue");
        deck.DrawCard(new Vector2(-deafultPosition1.x, deafultPosition1.y), "red");
        deck.DrawCard(new Vector2(-deafultPosition2.x, deafultPosition2.y), "red");
        deck.DrawCard(new Vector2(-deafultPosition3.x, deafultPosition3.y), "red");
        int first = Random.Range(1, 3);
        if (first == 1) blueTurn = true;
        else blueTurn = false;
        infoText.color = blueTurn ? Color.blue : Color.red;
        infoText.text = blueTurn ? "Blue turn" : "Red Turn";
        for (int i = 0; i < 3; i++)
        {
            CreateGem();
        }
    }

    public void ChangeTurn() 
    {
        blueTurn = !blueTurn;
        infoText.color = blueTurn ? Color.blue : Color.red;
        infoText.text = blueTurn ? "Blue turn" : "Red Turn";
    }

   void CreateGem() 
   {
        int randomX = Random.Range(-1, 2);
        int randomY = Random.Range(-1, 2);
        if(Board.instance.gemsPositions.Contains(randomX * Board.MAIN_X + " " + randomY * Board.MAIN_Y)) 
        {
            CreateGem();
            return;
        }
        Board.instance.gemsPositions.Add(randomX * Board.MAIN_X + " " + randomY * Board.MAIN_Y);
        GameObject gem = new GameObject();
        gem.AddComponent<SpriteRenderer>();
        gem.GetComponent<SpriteRenderer>().sprite = Media.instance.gemImage;
        gem.transform.position = new Vector3(randomX * Board.MAIN_X, randomY * Board.MAIN_Y, -1);
    }

    public void IsGameEnd() 
    {
        bool isGem = false;
        for (float i = -Board.MAIN_Y; i < Board.MAIN_Y * 2; i += Board.MAIN_Y)
        {
            for (float j = -Board.MAIN_X; j < Board.MAIN_X * 2; j += Board.MAIN_X)
            {
                if (Board.instance.IsFieldEmpty(j, i))
                {
                    isGem = false;
                    foreach (string gemPosition in Board.instance.gemsPositions) 
                    {
                        if (gemPosition == j + " " + i) 
                        {
                            isGem = true;
                        }
                    }
                    if (!isGem) return;
                }
            }
        }
        if (isGem) 
        {
            if (blueTurn) 
            {
                if (CanMakeMove(FindObjectOfType<Deck>().bluePlayerHand))return;
            }
            else 
            {
                if (CanMakeMove(FindObjectOfType<Deck>().redPlayerHand))return;
            }
        }

        GameEnd();
       
    }

    void GameEnd() 
    {
        int blueCardsOnGems = 0;
        int redCardsOnGems = 0;
        foreach (string gem in Board.instance.gemsPositions)
        {
            float gemX = float.Parse(gem.Split(' ')[0]);
            float gemY = float.Parse(gem.Split(' ')[1]);
            if (Board.instance.IsFieldEmpty(gemX, gemY)) continue;
            if (Board.instance.GetFieldValue(new Vector2(gemX, gemY)).color == "blue") blueCardsOnGems++;
            else redCardsOnGems++;
        }

        if (blueCardsOnGems > redCardsOnGems)
        {
            infoText.text = "Blue Win";
            infoText.color = Color.blue;
        }
        else if (blueCardsOnGems < redCardsOnGems)
        {
            infoText.text = "Red Win";
            infoText.color = Color.red;
        }
        else
        {
            infoText.text = "Draw";
            infoText.color = Color.black;
        }
        foreach(CardObject card in FindObjectOfType<Deck>().bluePlayerHand) 
        {
            card.enabled = false;
        }

        foreach (CardObject card in FindObjectOfType<Deck>().redPlayerHand)
        {
            card.enabled = false;
        }

        endGamePanel.SetActive(true);
    }

    bool CanMakeMove(List<CardObject> hand) 
    {
        foreach(CardObject card in hand) 
        {
            for(int i = 0; i<4; i++) 
            {
                for (float y = -Board.MAIN_Y; y < Board.MAIN_Y * 2; y += Board.MAIN_Y) 
                {
                    for (float x = -Board.MAIN_X; x < Board.MAIN_X * 2; x += Board.MAIN_X) 
                    {
                        if (Board.instance.IsFieldEmpty(x, y))continue;
                        CardObject checkedCard = Board.instance.GetFieldValue(new Vector2(x, y));
                        if (card.CanPush((Direction)i + 1, checkedCard, card.cardArrows[i])) return true;
                    }
                }
            }
        }
        return false;
    }

    public void RestartGame() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

   
}
