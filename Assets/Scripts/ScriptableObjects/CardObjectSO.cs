using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class CardObjectSO : ScriptableObject
{
    public Transform prefab;
    public Sprite sprite;
    public string cardName;
}
