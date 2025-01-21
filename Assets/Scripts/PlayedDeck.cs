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
            PlayedCardsUI.Instance.Show(playedCards);
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
        UpdateVisual();
    }

    public void RemoveCardObject(CardObject cardObject)
    {
        playedCards.Remove(cardObject);
        UpdateVisual();
    }
}
