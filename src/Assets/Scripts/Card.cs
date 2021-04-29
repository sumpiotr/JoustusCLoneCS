using UnityEngine;

public class Card 
{

    public Sprite cardImage;
    public int[] cardArrows = new int[4];
    public GameObject card;
    public string color = "";

    void Start()
    {
    }
    void Update()
    {

    }

    public void InitializeCard(string cardArrows, string color)
    {
        this.color = color;
        this.cardImage = Media.instance.GetImage(cardArrows, color);
        int j = 0;
        for (int i = 0; i < 7; i += 2)
        {
            this.cardArrows[j] = int.Parse(cardArrows[i].ToString());
            j++;
        }

    }

    public void CreateCard(float x, float y)
    {
        Deck deck = Object.FindObjectOfType<Deck>();
        card = Object.Instantiate(Media.instance.getCardPrefab());
        card.transform.position = new Vector3(x, y, 0);
        card.transform.SetParent(Media.instance.cardContainer.transform);
        card.GetComponent<SpriteRenderer>().sprite = cardImage;
        card.GetComponent<CardObject>().cardImage = this.cardImage;
        card.GetComponent<CardObject>().cardArrows = this.cardArrows;
        card.GetComponent<CardObject>().color = this.color;

        if(color == "blue")deck.bluePlayerHand.Add(card.GetComponent<CardObject>());
        else deck.redPlayerHand.Add(card.GetComponent<CardObject>());
    }

}
