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

    public event EventHandler OnDrawCard;

    [SerializeField] private Transform cardsOnHandTransform;

    private List<CardObject> cardsOnHand = new List<CardObject>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        DrawnCardUI.Instance.OnPlayerExplode += DrawnCardUI_OnPlayerExplode;
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

    private void GameManager_OnStateChanged(object sender, EventArgs e)
    {
        if (!GameManager.Instance.IsPlayerEndTurn())
        {
            return;
        }

        DrawCardFromDeck();
    }

    private void DrawnCardUI_OnPlayerExplode(object sender, EventArgs e)
    {
        Debug.Log("Player explode!");
    }

    private void DrawCardFromDeck()
    {
        // Reset all selected cards
        foreach (CardObject cardObject in cardsOnHand)
        {
            cardObject.SetCardSelected(false);
        }

        // Add new card to hand
        CardObject newCardObject = Deck.Instance.DrawCard();

        DrawnCardUI.Instance.Show(newCardObject);

        newCardObject.SetCardObjectParent(this);
    }

    public void TriggerDrawCard()
    {
        OnDrawCard?.Invoke(this, EventArgs.Empty);
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
                if (selectedCards[0].GetCardType() == CardObject.CardType.Cat ||
                    selectedCards[0].GetCardType() == CardObject.CardType.Defuse ||
                    selectedCards[0].GetCardType() == CardObject.CardType.Nope)
                {
                    return false;
                }
                return true;
            case 2:
                // Play 2 same type cards
                return CanPlayTwoCards(selectedCards[0], selectedCards[1]);
            case 3:
                // Play 3 same type cards
                return CanPlayThreeCards(selectedCards[0], selectedCards[1], selectedCards[2]);
            case 5:
                // Play 5 different type cards
                return CanPlayFiveCards(selectedCards);
            default:
                return false;
        }
    }

    private bool CanPlayTwoCards(CardObject firstCardObject, CardObject secondCardObject)
    {
        if (firstCardObject.GetCardType() != secondCardObject.GetCardType())
        {
            return false;
        }

        if (firstCardObject.GetCardType() == CardObject.CardType.Cat)
        {
            if (firstCardObject.GetCardObjectSO().cardName != secondCardObject.GetCardObjectSO().cardName)
            {
                return false;
            }
        }

        return true;
    }

    private bool CanPlayThreeCards(CardObject firstCardObject, CardObject secondCardObject, CardObject thirdCardObject)
    {
        return CanPlayTwoCards(firstCardObject, secondCardObject) && CanPlayTwoCards(firstCardObject, thirdCardObject);
    }

    private bool CanPlayFiveCards(List<CardObject> selectedCards)
    {
        if (PlayedDeck.Instance.GetCardObjectList().Count == 0)
        {
            return false;
        }

        HashSet<string> cardTypes = new HashSet<string>();
        foreach (var card in selectedCards)
        {
            if (!cardTypes.Add(card.GetCardObjectSO().cardName))
            {
                return false;
            }
        }
        return true;
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
                CardsListUI.Instance.Show(PlayedDeck.Instance.GetCardObjectListWithoutExplodingKittens());
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

    public CardObject GetDefuseCardOnHand()
    {
        foreach (CardObject cardObject in cardsOnHand)
        {
            if (cardObject.GetCardType() == CardObject.CardType.Defuse)
            {
                return cardObject;
            }
        }

        return null;
    }
}
