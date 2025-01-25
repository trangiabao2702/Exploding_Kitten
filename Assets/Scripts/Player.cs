using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, ICardObjectParent
{
    public static Player Instance { get; private set; }

    [SerializeField] private Transform cardsOnHandTransform;
    [SerializeField] private Button drawButton;
    [SerializeField] private Button playButton;
    [SerializeField] private Button arrangeButton;

    private List<CardObject> cardsOnHand = new List<CardObject>();

    private void Awake()
    {
        Instance = this;

        drawButton.onClick.AddListener(() =>
        {
            CardObject newCardObject = Deck.Instance.DrawCard();

            newCardObject.SetCardObjectParent(this);
        });
        playButton.onClick.AddListener(() =>
        {
            List<CardObject> selectedCards = GetSelectedCards();

            if (CanPlayCards(selectedCards))
            {
                PlayCards(selectedCards);
            }
        });
        arrangeButton.onClick.AddListener(() =>
        {
            ArrangeCardsOnHand();
        });
    }

    private void Update()
    {

    }

    public Transform GetCardObjectFollowTransform()
    {
        return cardsOnHandTransform;
    }

    public List<CardObject> GetCardObjectList()
    {
        return cardsOnHand;
    }

    public void AddCardObject(CardObject cardObject)
    {
        cardsOnHand.Add(cardObject);
    }

    public void RemoveCardObject(CardObject cardObject)
    {
        cardsOnHand.Remove(cardObject);
    }

    public bool HasCardOnHand(CardObject cardObject)
    {
        return cardsOnHand.Contains(cardObject);
    }

    public List<CardObject> GetSelectedCards()
    {
        List<CardObject> selectedCards = new List<CardObject>();

        foreach (CardObject cardObject in cardsOnHand)
        {
            if (cardObject.IsSelected())
            {
                selectedCards.Add(cardObject);
            }
        }

        return selectedCards;
    }

    public bool CanPlayCards(List<CardObject> selectedCards)
    {
        switch (selectedCards.Count)
        {
            case 1:
                // Play only 1 card
                return true;
            case 2:
                // Play 2 same type cards
                if (selectedCards[0].GetCardType() == selectedCards[1].GetCardType())
                {
                    return true;
                }
                return false;
            case 3:
                // Play 3 same type cards
                if (selectedCards[0].GetCardType() == selectedCards[1].GetCardType() &&
                    selectedCards[1].GetCardType() == selectedCards[2].GetCardType())
                {
                    return true;
                }
                return false;
            case 5:
                // Play 5 different type cards
                if (PlayedDeck.Instance.GetCardObjectList().Count == 0)
                {
                    return false;
                }

                for (int i = 0; i < selectedCards.Count - 1; i++)
                {
                    for (int j = i + 1; j < selectedCards.Count; j++)
                    {
                        if (selectedCards[i].GetCardType() == selectedCards[j].GetCardType())
                        {
                            return false;
                        }
                    }
                }
                return true;
            default:
                return false;
        }
    }

    public void PlayCards(List<CardObject> selectedCards)
    {
        // Use cards' feature
        switch (selectedCards.Count)
        {
            case 1:
                // Use Card's feature
                selectedCards[0].PlayCard();
                break;
            case 2:
                // Select a player to get a random card from his hand
                break;
            case 3:
                // Select a player to get a card with exactly name from his hand
                break;
            case 5:
                // Get a card from played deck
                CardsListUI.Instance.UnlockSelectCard();
                CardsListUI.Instance.Show(PlayedDeck.Instance.GetCardObjectList());
                break;
            default:
                break;
        }

        // Place cards into Played Deck
        foreach (CardObject selectedCard in selectedCards)
        {
            selectedCard.SetCardObjectParent(PlayedDeck.Instance);

            RemoveCardObject(selectedCard);
        }
    }

    private void ArrangeCardsOnHand()
    {
        for (int i = 0; i < cardsOnHandTransform.childCount - 1; i++)
        {
            for (int j = i + 1; j < cardsOnHandTransform.childCount; j++)
            {
                if (String.Compare(cardsOnHandTransform.GetChild(i).name, cardsOnHandTransform.GetChild(j).name) > 0)
                {
                    cardsOnHandTransform.GetChild(j).SetSiblingIndex(cardsOnHandTransform.GetChild(i).GetSiblingIndex());
                }
            }
        }
    }
}
