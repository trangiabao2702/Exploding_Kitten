using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PutExplodingKittenBackToDeckUI : MonoBehaviour
{
    public static PutExplodingKittenBackToDeckUI Instance { get; private set; }

    public event EventHandler OnPutExplodingKittenIntoDeck;

    [SerializeField] private TMP_InputField positionInputField;
    [SerializeField] private Button topButton;
    [SerializeField] private Button bottomButton;
    [SerializeField] private Button okButton;

    private void Awake()
    {
        Instance = this;

        topButton.onClick.AddListener(() =>
        {
            positionInputField.text = "1";
        });
        bottomButton.onClick.AddListener(() =>
        {
            positionInputField.text = Deck.Instance.GetCardObjectList().Count.ToString();
        });
        okButton.onClick.AddListener(() =>
        {
            int position = int.Parse(positionInputField.text);
            Deck.Instance.PutExplodingKittenIntoDeck(position);
            OnPutExplodingKittenIntoDeck?.Invoke(this, EventArgs.Empty);

            Hide();
        });

        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
    }

    private void Start()
    {
        Hide();
    }

    private void GameManager_OnStateChanged(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsPlayerPlayTurn())
        {
            Hide();
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}
