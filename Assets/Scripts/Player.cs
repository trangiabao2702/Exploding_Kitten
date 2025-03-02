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
    [SerializeField] private Button drawButton;
    [SerializeField] private Button playButton;
    [SerializeField] private Button arrangeButton;
    [SerializeField] private Button nopeButton;
    [SerializeField] private Button helpButton;

    private List<CardObject> cardsOnHand = new List<CardObject>();

    private void Awake()
    {
        Instance = this;

        drawButton.onClick.AddListener(() =>
        {
            OnDrawCard?.Invoke(this, EventArgs.Empty);
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
        nopeButton.onClick.AddListener(() =>
        {
            
        });
        helpButton.onClick.AddListener(() =>
        {
            TutorialUI.Instance.Show();
        });

        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        DrawnCardUI.Instance.OnPlayerExplode += DrawnCardUI_OnPlayerExplode;
    }

    private void Update()
    {
        nopeButton.interactable = HasNopeCard();
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
                if (CanPlayTwoCards(selectedCards[0], selectedCards[1]))
                {
                    return true;
                }
                return false;
            case 3:
                // Play 3 same type cards
                if (CanPlayThreeCards(selectedCards[0], selectedCards[1], selectedCards[2]))
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
                            // 2 cards are Cat type but different cards
                            if (selectedCards[i].GetCardObjectSO().cardName != selectedCards[j].GetCardObjectSO().cardName)
                            {
                                continue;
                            }

                            return false;
                        }
                    }
                }
                return true;
            default:
                return false;
        }
    }

    public bool CanPlayTwoCards(CardObject firstCardObject, CardObject secondCardObject)
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

    public bool CanPlayThreeCards(CardObject firstCardObject, CardObject secondCardObject, CardObject thirdCardObject)
    {
        return CanPlayTwoCards(firstCardObject, secondCardObject) && CanPlayTwoCards(firstCardObject, thirdCardObject);
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

    private bool HasNopeCard()
    {
        foreach (CardObject cardObject in cardsOnHand)
        {
            if (cardObject.GetCardType() == CardObject.CardType.Nope)
            {
                return true;
            }
        }
        return false;
    }
}
