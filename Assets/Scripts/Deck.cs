using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Deck : NetworkBehaviour, ICardObjectParent
{
    public static Deck Instance { get; private set; }

    [SerializeField] private Transform undrawnCards;
    [SerializeField] private TextMeshProUGUI countCard;
    [SerializeField] private List<PackObjectSO> packObjectSOList;

    private List<CardObject> cardsInDeck;
    private List<CardObject> defuseCardsList = new List<CardObject>();
    private List<CardObject> explodingKittenCardsList = new List<CardObject>();
    private CardObject drawnCardObject;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (!IsServer)
        {
            return;
        }

        countCard.text = cardsInDeck.Count.ToString();
    }
    public override void OnNetworkSpawn()
    {
        InitDeck();
    }

    public void InitDeck()
    {
        if (!IsServer)
        {
            return;
        }

        // Step 1: Add cards to Deck (except Defuse and Exploding Kitten)
        AddCardsWithoutDefuseOrExplodingKitten();

        // Step 2: Shuffle the Deck
        ShuffleDeck();

        // Step 3: Deal the cards to Players
        DealTheCards();

        // Step 4: Add the rest Defuse and Exploding Kitten to the Deck
        AddDefuseAndExplodingKitten();

        // Step 5: Shuffle the Deck again
        ShuffleDeck();
    }

    public void ShuffleDeck()
    {
        RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();

        int n = cardsInDeck.Count;
        while (n > 1)
        {
            byte[] box = new byte[1];
            do provider.GetBytes(box);
            while (!(box[0] < n * (Byte.MaxValue / n)));
            int k = (box[0] % n);
            n--;
            CardObject value = cardsInDeck[k];
            cardsInDeck[k] = cardsInDeck[n];
            cardsInDeck[n] = value;
        }
    }

    public void DrawCard(ICardObjectParent player)
    {
        DrawCardServerRpc(player.GetNetworkObject());
    }

    [ServerRpc(RequireOwnership = false)]
    private void DrawCardServerRpc(NetworkObjectReference playerNetworkObjectReference)
    {
        DrawCardClientRpc(playerNetworkObjectReference);
    }

    [ClientRpc]
    private void DrawCardClientRpc(NetworkObjectReference playerNetworkObjectReference)
    {
        drawnCardObject = cardsInDeck[0];

        cardsInDeck.Remove(drawnCardObject);

        DrawnCardUI.Instance.Show(drawnCardObject);

        playerNetworkObjectReference.TryGet(out NetworkObject playerNetworkObject);
        ICardObjectParent player = playerNetworkObject.GetComponent<ICardObjectParent>();
        drawnCardObject.SetCardObjectParent(player);
    }

    public Transform GetCardObjectFollowTransform()
    {
        return undrawnCards;
    }

    public List<CardObject> GetCardObjectList()
    {
        return cardsInDeck;
    }

    public void AddCardObject(CardObject cardObject)
    {
        // Add to the top of Deck
        cardsInDeck.Insert(0, cardObject);
    }

    public void RemoveCardObject(CardObject cardObject)
    {
        // To remove exactly a card
        cardsInDeck.Remove(cardObject);
    }

    public NetworkObject GetNetworkObject()
    {
        return NetworkObject;
    }

    private void AddCardsWithoutDefuseOrExplodingKitten()
    {
        cardsInDeck = new List<CardObject>();

        foreach (PackObjectSO packObjectSO in packObjectSOList)
        {
            foreach (CardObjectSO cardObjectSO in packObjectSO.cardObjectSOList)
            {
                CardObject cardObject = CardObject.SpawnCardObject(cardObjectSO, this);

                if (cardObject.GetCardType() == CardObject.CardType.Defuse)
                {
                    RemoveCardObject(cardObject);

                    defuseCardsList.Add(cardObject);
                }
                else if (cardObject.GetCardType() == CardObject.CardType.ExplodingKitten)
                {
                    RemoveCardObject(cardObject);

                    explodingKittenCardsList.Add(cardObject);
                }
                else if (cardObject.GetCardType() == CardObject.CardType.Cat) {
                    for (int i = 0; i < 3; i++)
                    {
                        CardObject.SpawnCardObject(cardObjectSO, this);
                    }
                }
            }
        }
    }

    private void AddDefuseAndExplodingKitten()
    {
        List<ICardObjectParent> listPlayers = GameManager.Instance.GetPlayers();

        int numberOfDefuseCards = Mathf.Min(listPlayers.Count, defuseCardsList.Count);
        int numberOfExplodingKittenCards = Mathf.Min(listPlayers.Count - 1, explodingKittenCardsList.Count);

        for (int i = 0; i < numberOfDefuseCards; i++)
        {
            AddCardObject(GetDefuseOrExplodingKittenCardObject(defuseCardsList));
        }

        for (int i = 0; i < numberOfExplodingKittenCards; i++)
        {
            AddCardObject(GetDefuseOrExplodingKittenCardObject(explodingKittenCardsList));
        }
    }

    private void DealTheCards()
    {
        if (!IsServer)
        {
            return;
        }
        
        List<ICardObjectParent> listPlayers = GameManager.Instance.GetPlayers();

        foreach (ICardObjectParent player in listPlayers)
        {
            // Each player will have a Defuse card before starting game
            GetDefuseOrExplodingKittenCardObject(defuseCardsList).SetCardObjectParent(player);

            // Each player will have 4 other cards from the Deck
            for (int i = 0; i < 4; i++)
            {
                DrawCard(player);
            }
        }
    }

    private CardObject GetDefuseOrExplodingKittenCardObject(List<CardObject> cardObjectsList)
    {
        System.Random random = new System.Random();

        int randomIndex = random.Next(cardObjectsList.Count);
        CardObject cardObject = cardObjectsList[randomIndex];

        cardObjectsList.RemoveAt(randomIndex);

        return cardObject;
    }

    public void PutExplodingKittenIntoDeck(int position)
    {
        CardObject explodingKittenCardObject = cardsInDeck[0];

        RemoveCardObject(explodingKittenCardObject);
        cardsInDeck.Insert(position - 1, explodingKittenCardObject);
    }
}
