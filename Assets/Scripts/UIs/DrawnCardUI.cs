using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DrawnCardUI : MonoBehaviour
{
    public static DrawnCardUI Instance { get; private set; }

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

            Hide();

            PutExplodingKittenBackToDeckUI.Instance.Show();
        });
        explodeButton.onClick.AddListener(() =>
        {
            Hide();
            Debug.Log("Game over!");
        });
    }

    private void Start()
    {
        Hide();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
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
}
