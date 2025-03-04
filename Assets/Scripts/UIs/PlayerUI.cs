using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public static PlayerUI Instance { get; private set; }

    [SerializeField] private Button drawButton;
    [SerializeField] private Button playButton;
    [SerializeField] private Button arrangeButton;
    [SerializeField] private Button nopeButton;
    [SerializeField] private Button helpButton;

    private void Awake()
    {
        drawButton.onClick.AddListener(() =>
        {
            Player.Instance.TriggerDrawCard();
        });
        playButton.onClick.AddListener(() =>
        {
            List<CardObject> selectedCards = Player.Instance.GetSelectedCards();

            if (Player.Instance.CanPlayCards(selectedCards))
            {
                Player.Instance.PlayCards(selectedCards);
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
    }

    private void Update()
    {
        nopeButton.interactable = HasNopeCard();
    }

    private void ArrangeCardsOnHand()
    {
        Transform cardsOnHandTransform = Player.Instance.GetCardObjectFollowTransform();

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

    private bool HasNopeCard()
    {
        foreach (CardObject cardObject in Player.Instance.GetCardObjectList())
        {
            if (cardObject.GetCardType() == CardObject.CardType.Nope)
            {
                return true;
            }
        }
        return false;
    }
}
