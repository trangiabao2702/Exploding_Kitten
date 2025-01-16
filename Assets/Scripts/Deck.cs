using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    public static Deck Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI countCard;
    [SerializeField] private List<PackObjectSO> packObjectSOList;

    private List<CardObjectSO> cardsList = new List<CardObjectSO>();

    private void Awake()
    {
        Instance = this;
    }

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

    public CardObjectSO DrawCard()
    {
        CardObjectSO drawnCard = cardsList[0];

        cardsList.Remove(drawnCard);

        Debug.Log(drawnCard);

        return drawnCard;
    }
}
