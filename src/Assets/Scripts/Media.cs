using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Media : MonoBehaviour
{
 
    [SerializeField]
    List<string> blueKey = new List<string>();
    
    [SerializeField]
    List<Sprite> blueValue = new List<Sprite>();

    [SerializeField]
    List<string> redKey = new List<string>();

    [SerializeField]
    List<Sprite> redValue = new List<Sprite>();

    [SerializeField]
    GameObject cardPrefab;

    public GameObject cardContainer;

    public Sprite gemImage;

    Dictionary<string, Sprite> blueCardsImages = new Dictionary<string, Sprite>();

    Dictionary<string, Sprite> redCardsImages = new Dictionary<string, Sprite>();

    public static Media instance = null;


    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
        Initialize();
    }

   

    void Initialize() 
    {
        for (int i = 0; i < blueKey.Capacity; i++)
        {
            blueCardsImages.Add(blueKey[i], blueValue[i]);
        }

        for(int i = 0; i < redKey.Capacity; i++) 
        {
            redCardsImages.Add(redKey[i], redValue[i]);
        }
    }
    public Sprite GetImage(string cardArrows, string color) 
    {
        if (color == "blue") return blueCardsImages[cardArrows];
        else return redCardsImages[cardArrows];
    }

    public GameObject getCardPrefab() 
    {
        return cardPrefab;
    }
}
