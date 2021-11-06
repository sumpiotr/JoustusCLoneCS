using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardScriptableObject", menuName ="Card")]
public class CardScriptableObject : ScriptableObject
{
    public int upArrow;
    public int rightArrow;
    public int bottomArrow;
    public int leftArrow;

    public Sprite blueImage;
    public Sprite redImage;
}
