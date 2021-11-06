using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public const float BOARD_MAX_X = 2.8f;
    public const float BOARD_MAX_Y = 2.8f;
    public const float MIN_X_TO_MOVE = 0.9f;
    public const float MIN_Y_TO_MOVE = 0.9f;
    public const float MAIN_X = 1.6f;
    public const float MAIN_Y = 1.6f;
    public const float CARD_OFFSET = 1.5f;
    const int BOARD_WIDTH = 3;
    const int BOARD_HEIGHT = 3;
    const int BOARD_FIELD = BOARD_WIDTH * BOARD_HEIGHT;
    public static Board instance = null;
    public List<string> gemsPositions = new List<string>();
    Dictionary<string, GameObject> fieldsContent = new Dictionary<string, GameObject>();

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    void Start()
    {
        fieldsContent.Add( "0 0", null);
        fieldsContent.Add(MAIN_X + " 0" , null);
        fieldsContent.Add( -MAIN_X + " 0" , null);
        fieldsContent.Add( "0 " + MAIN_Y , null);
        fieldsContent.Add( MAIN_X + " " + MAIN_Y, null);
        fieldsContent.Add(-MAIN_X + " " + MAIN_Y, null);
        fieldsContent.Add( "0 " + -MAIN_Y , null);
        fieldsContent.Add(MAIN_X + " " + -MAIN_Y, null);
        fieldsContent.Add(-MAIN_X + " " + -MAIN_Y, null);
        fieldsContent.Add("0 " + MAIN_Y * 2, null);
        fieldsContent.Add(MAIN_X + " " + MAIN_Y*2, null);
        fieldsContent.Add(-MAIN_X + " " + MAIN_Y*2, null);
        fieldsContent.Add("0 " + -MAIN_Y * 2, null);
        fieldsContent.Add(MAIN_X + " " + -MAIN_Y * 2, null);
        fieldsContent.Add(-MAIN_X + " " + -MAIN_Y * 2, null);
        fieldsContent.Add(MAIN_X*2 + " 0", null);
        fieldsContent.Add(MAIN_X*2 + " " + MAIN_Y, null);
        fieldsContent.Add(MAIN_X*2 + " " + -MAIN_Y, null);
        fieldsContent.Add(-MAIN_X * 2 + " 0", null);
        fieldsContent.Add(-MAIN_X * 2 + " " + MAIN_Y, null);
        fieldsContent.Add(-MAIN_X * 2 + " " + -MAIN_Y, null);
        GameManager.instance.StartSetup();
    }


    public void SetCardPosition(CardObject card, string fieldPosition)
    {
        float fieldPositionX = float.Parse(fieldPosition.Split(' ')[0]);
        float fieldPositionY = float.Parse(fieldPosition.Split(' ')[1]);
        if (fieldsContent[fieldPosition] == null)
        {
            foreach(string gem in gemsPositions) 
            {
                if (gem == fieldPosition)
                {
                    card.GoToDeafultPosition();
                    return;
                }
            }
            GameManager.instance.ChangeTurn();
            FindObjectOfType<Deck>().RemoveFromHand(card, card.color);
            FindObjectOfType<Deck>().DrawCard(card.GetDeafultPosition(), card.color);
            setFieldValue(card, new Vector2(fieldPositionX, fieldPositionY));
            GameManager.instance.IsGameEnd();
        }
        else {
            CardObject objectInField = fieldsContent[fieldPosition].GetComponent<CardObject>();
            card.CheckPushCard(objectInField);
        }
      
    }

    public void setFieldValue(CardObject value, Vector2 key) 
    {
        fieldsContent[key.x + " " + key.y] = value.gameObject;
        value.SetDeafultPosition(key.x, key.y);
        if (!value.isPlaced) 
        {
            value.GoToDeafultPosition();
            value.isPlaced = true;
            value.GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }

    public CardObject GetFieldValue(Vector2 keyPosition) 
    {
        string key = keyPosition.x + " " + keyPosition.y;
        if (IsFieldEmpty(keyPosition.x, keyPosition.y)) return null;
        return fieldsContent[key].GetComponent<CardObject>();
    }

    public bool IsFieldEmpty(float x, float y) 
    {
        string fieldKey = x + " " + y;
        if (fieldsContent[fieldKey] == null) return true;
        else return false;
    }

   

}
