using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour, ICardObjectParent
{
    public static Deck Instance { get; private set; }

    [SerializeField] private Transform undrawnCards;
    [SerializeField] private TextMeshProUGUI countCard;
    [SerializeField] private List<PackObjectSO> packObjectSOList;

    private List<CardObject> cardsList;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        cardsList = new List<CardObject>();

        foreach (PackObjectSO packObjectSO in packObjectSOList)
        {
            foreach (CardObjectSO cardObjectSO in packObjectSO.cardObjectSOList)
            {
                CardObject.SpawnCardObject(cardObjectSO, this);
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
            CardObject value = cardsList[k];
            cardsList[k] = cardsList[n];
            cardsList[n] = value;
        }
    }

    public CardObject DrawCard()
    {
        CardObject drawnCard = cardsList[0];

        cardsList.Remove(drawnCard);

        return drawnCard;
    }

    public Transform GetCardObjectFollowTransform()
    {
        return undrawnCards;
    }

    public List<CardObject> GetCardObjectList()
    {
        return cardsList;
    }

    public void AddCardObject(CardObject cardObject)
    {
        // Add to the top of Deck
        cardsList.Insert(0, cardObject);
    }

    public void RemoveCardObject(CardObject cardObject)
    {
        // To remove exactly a card
        cardsList.Remove(cardObject);
    }
}
