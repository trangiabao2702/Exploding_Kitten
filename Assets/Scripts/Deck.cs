using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countCard;
    [SerializeField] private List<PackObjectSO> packObjectSOList;

    private List<CardObjectSO> cardsList = new List<CardObjectSO>();

    private void Start()
    {
        foreach (PackObjectSO packObjectSO in packObjectSOList)
        {
            foreach (CardObjectSO cardObjectSO in packObjectSO.cardObjectSOList)
            {
                cardsList.Add(cardObjectSO);
            }
        }

        ShuffleDeck();
    }

    private void Update()
    {
        countCard.text = cardsList.Count.ToString();
    }

    public void ShuffleDeck()
    {
        RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();

        int n = cardsList.Count;
        while (n > 1)
        {
            byte[] box = new byte[1];
            do provider.GetBytes(box);
            while (!(box[0] < n * (Byte.MaxValue / n)));
            int k = (box[0] % n);
            n--;
            CardObjectSO value = cardsList[k];
            cardsList[k] = cardsList[n];
            cardsList[n] = value;
        }
    }
}
