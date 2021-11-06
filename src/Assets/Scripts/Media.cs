using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Media : MonoBehaviour
{
 

    [SerializeField]
    GameObject cardPrefab;

    public GameObject cardContainer;

    public Sprite gemImage;

    public static Media instance = null;


    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    public GameObject getCardPrefab() 
    {
        return cardPrefab;
    }
}
