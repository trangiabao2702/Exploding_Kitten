using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CardsListUI : MonoBehaviour
{
    public static CardsListUI Instance { get; private set; }

    [SerializeField] private Transform playedCardsTransform;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button confirmButton;

    private int numberCardsCanSelect;
    private List<CardObject> selectedCardObjects;

    private void Awake()
    {
        Instance = this;

        closeButton.onClick.AddListener(() =>
        {
            Hide();
        });
        confirmButton.onClick.AddListener(() =>
        {
            // When player selected enough cards
            if (numberCardsCanSelect == 0)
            {
                // Change parent object and remove from cards list
                foreach (CardObject selectedCardObject in selectedCardObjects.ToList())
                {
                    selectedCardObject.SetCardObjectParent(Player.Instance);
                    selectedCardObject.AdjustCardSelected(false);

                    PlayedDeck.Instance.RemoveCardObject(selectedCardObject);
                }

                Hide();
            }
        });
    }

    private void Start()
    {
        Hide();
    }

    private void Hide()
    {
        gameObject.SetActive(false);

        numberCardsCanSelect = 0;
        selectedCardObjects = new List<CardObject>();

        foreach (Transform playedCard in playedCardsTransform)
        {
            Destroy(playedCard.gameObject);
        }
    }

    public void Show(List<CardObject> playedCards)
    {
        gameObject.SetActive(true);
        
        closeButton.gameObject.SetActive(numberCardsCanSelect <= 0);
        confirmButton.gameObject.SetActive(numberCardsCanSelect > 0);

        foreach (CardObject playedCard in playedCards)
        {
            Transform cardObjectTransform = Instantiate(playedCard.GetCardObjectSO().prefab);
            cardObjectTransform.transform.SetParent(playedCardsTransform);
        }
    }

    public int UnlockSelectCard(int count = 1)
    {
        // Unlock the number of cards can be selected
        numberCardsCanSelect = count;
        return numberCardsCanSelect;
    }

    public int LockSelectCard()
    {
        // Lock a card to be selected
        return numberCardsCanSelect--;
    }

    public void AdjustSelectCardObjects(CardObject cardObject, bool isAddingCardObject)
    {
        if (isAddingCardObject)
        {
            selectedCardObjects.Add(cardObject);
        }
        else
        {
            if (selectedCardObjects.Contains(cardObject))
            {
                selectedCardObjects.Remove(cardObject);
            }
        }
    }
}
