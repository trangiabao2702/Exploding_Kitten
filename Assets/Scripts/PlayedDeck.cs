using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayedDeck : MonoBehaviour, ICardObjectParent
{
    public static PlayedDeck Instance { get; private set; }

    [SerializeField] private Transform playedCardsTransform;
    [SerializeField] private Button showButton;

    private List<CardObject> playedCards;

    private void Awake()
    {
        Instance = this;

        showButton.onClick.AddListener(() =>
        {
            CardsListUI.Instance.Show(playedCards);
        });
    }

    private void Start()
    {
        playedCards = new List<CardObject>();

        Hide();
    }

    private void UpdateVisual()
    {
        if (playedCards.Count == 0)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public Transform GetCardObjectFollowTransform()
    {
        return playedCardsTransform;
    }

    public List<CardObject> GetCardObjectList()
    {
        return playedCards;
    }

    public void AddCardObject(CardObject cardObject)
    {
        playedCards.Add(cardObject);
        
        cardObject.transform.rotation = Quaternion.Euler(0, 0, Random.Range(-15f, 15f));

        UpdateVisual();
    }

    public void RemoveCardObject(CardObject cardObject)
    {
        foreach (CardObject playedCard in playedCards)
        {
            if (playedCard.GetCardObjectSO().name == cardObject.GetCardObjectSO().name)
            {
                playedCards.Remove(playedCard);
                UpdateVisual();
                break;
            }
        }
    }

    public List<CardObject> GetCardObjectListWithoutExplodingKittens()
    {
        List<CardObject> availableCards = new List<CardObject>();
        foreach (CardObject playedCard in playedCards)
        {
            if (playedCard.GetCardType() != CardObject.CardType.ExplodingKitten)
            {
                availableCards.Add(playedCard);
            }
        }

        return availableCards;
    }
}
