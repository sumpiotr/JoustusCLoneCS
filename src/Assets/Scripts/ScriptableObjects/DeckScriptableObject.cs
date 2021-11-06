using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeckScriptableObject", menuName = "Deck")]
public class DeckScriptableObject : ScriptableObject
{
    public List<CardScriptableObject> cards;
}
