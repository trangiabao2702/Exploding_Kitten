using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardsListUI : MonoBehaviour
{
    public static CardsListUI Instance { get; private set; }

    [SerializeField] private Button closeButton;
    [SerializeField] private Transform playedCardsTransform;

    private void Awake()
    {
        Instance = this;

        closeButton.onClick.AddListener(() =>
        {
            Hide();
        });
    }

    private void Start()
    {
        Hide();
    }

    private void Hide()
    {
        gameObject.SetActive(false);

        foreach (Transform playedCard in playedCardsTransform)
        {
            Destroy(playedCard.gameObject);
        }
    }

    public void Show(List<CardObject> playedCards)
    {
        gameObject.SetActive(true);

        foreach (CardObject playedCard in playedCards)
        {
            Transform cardObjectTransform = Instantiate(playedCard.GetCardObjectSO().prefab);
            cardObjectTransform.transform.SetParent(playedCardsTransform);
        }
    }
}
