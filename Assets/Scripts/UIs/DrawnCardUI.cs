using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DrawnCardUI : MonoBehaviour
{
    public static DrawnCardUI Instance { get; private set; }

    public event EventHandler OnPlayerEndTurn;
    public event EventHandler OnPlayerExplode;

    [SerializeField] private Image cardImage;
    [SerializeField] private TextMeshProUGUI cardTypeText;
    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private TextMeshProUGUI cardFeatureText;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button defuseButton;
    [SerializeField] private Button explodeButton;

    private CardObject defuseCardObject;
    private CardObject explodingKittenCardObject;

    private void Awake()
    {
        Instance = this;

        closeButton.onClick.AddListener(() =>
        {
            Hide();
        });
        defuseButton.onClick.AddListener(() =>
        {
            DefuseTheExplodingKitten();

            PutExplodingKittenBackToDeckUI.Instance.Show();

            ResetCardObjectVariables();
        });
        explodeButton.onClick.AddListener(() =>
        {
            ResetCardObjectVariables();

            Hide();
            OnPlayerExplode?.Invoke(this, EventArgs.Empty);
        });

        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        PutExplodingKittenBackToDeckUI.Instance.OnPutExplodingKittenIntoDeck += PutEKBackToDeckUI_OnPutExplodingKittenIntoDeck;
    }

    private void Start()
    {
        Hide();
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (!GameManager.Instance.IsPlayerPlayTurn())
        {
            return;
        }

        // The drawn card is Exploding Kitten but Player not choose Defuse or Explode
        if (explodingKittenCardObject != null)
        {
            if (defuseCardObject != null)
            {
                // Player has Defuse
                DefuseTheExplodingKitten();
                Deck.Instance.PutExplodingKittenIntoDeck();

                ResetCardObjectVariables();
            }
            else
            {
                // Player does not have Defuse
                OnPlayerExplode?.Invoke(this, EventArgs.Empty);
            }
        }

        Hide();
    }

    private void PutEKBackToDeckUI_OnPutExplodingKittenIntoDeck(object sender, EventArgs e)
    {
        Hide();
    }

    private void Hide()
    {
        gameObject.SetActive(false);

        if (!GameManager.Instance.IsPlayerEndTurn())
        {
            return;
        }
        OnPlayerEndTurn?.Invoke(this, EventArgs.Empty);
    }

    public void Show(CardObject cardObject)
    {
        gameObject.SetActive(true);

        cardImage.sprite = cardObject.GetCardObjectSO().sprite;
        cardTypeText.text = cardObject.GetCardType().ToString();
        cardNameText.text = cardObject.GetCardObjectSO().cardName;
        cardFeatureText.text = GetCardFeature(cardObject.GetCardType());

        if (cardObject.GetCardType() == CardObject.CardType.ExplodingKitten)
        {
            defuseButton.gameObject.SetActive(true);
            explodeButton.gameObject.SetActive(true);
            closeButton.gameObject.SetActive(false);

            defuseCardObject = Player.Instance.GetDefuseCardOnHand();
            explodingKittenCardObject = cardObject;

            if (defuseCardObject == null)
            {
                defuseButton.interactable = false;
            }
            else
            {
                defuseButton.interactable = true;
            }
        }
        else
        {
            defuseButton.gameObject.SetActive(false);
            explodeButton.gameObject.SetActive(false);
            closeButton.gameObject.SetActive(true);

            ResetCardObjectVariables();
        }
    }

    private string GetCardFeature(CardObject.CardType cardType)
    {
        switch (cardType)
        {
            case CardObject.CardType.Attack:
                return "End your turn without drawing a card.\nForce the next player to take TWO turns.";
            case CardObject.CardType.Defuse:
                return "Put your last drawn card back into the Deck.";
            case CardObject.CardType.ExplodingKitten:
                return "Show this card immediately.";
            case CardObject.CardType.Favor:
                return "One player must give you a card of their choice.";
            case CardObject.CardType.Nope:
                return "Stop the action of another player.\nYou can play this at any time.\nExcept when player use Defuse.";
            case CardObject.CardType.SeeTheFuture:
                return "Privately view the top THREE cards of the Deck";
            case CardObject.CardType.Shuffle:
                return "Shuffle the draw pile.";
            case CardObject.CardType.Skip:
                return "End your turn without drawing a card.";
            case CardObject.CardType.Cat:
                return "Play two cards to steal a random card from another player.\nPlay three cards to steal exactly a card from another player.\nPlay with 4 other cat cards to take a card from played pile.";
            default:
                return "None...";
        }
    }

    private void DefuseTheExplodingKitten()
    {
        Player.Instance.RemoveCardObject(defuseCardObject);
        defuseCardObject.SetCardObjectParent(PlayedDeck.Instance);

        Player.Instance.RemoveCardObject(explodingKittenCardObject);
        explodingKittenCardObject.SetCardObjectParent(Deck.Instance);
    }

    private void ResetCardObjectVariables()
    {
        defuseCardObject = null;
        explodingKittenCardObject = null;
    }
}
