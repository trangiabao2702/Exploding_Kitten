using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PackObjectSO : ScriptableObject
{
    public List<CardObjectSO> cardObjectSOList;
    public Sprite packCover;
    public string packName;
}
