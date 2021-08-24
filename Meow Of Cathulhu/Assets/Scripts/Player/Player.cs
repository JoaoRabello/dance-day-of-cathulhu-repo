using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerRenderer), typeof(PlayerLifeController))]
public class Player : MonoBehaviour
{
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private ScoreManager scoreManager;
    private PlayerRenderer playerRenderer;
    private PlayerLifeController playerLifeController;

    public UnityEvent OnRewindEvent;
    public UnityEvent OnNormalEvent;
    public UnityEvent OnCorrectHitEvent;

    [SerializeField] private int pleasingMax;
    private float pleasingAmount;
    public float PleasingAmount
    {
        get => pleasingAmount;
        set
        {
            pleasingAmount = value;
            playerRenderer.UpdatePleasingValue(value, pleasingMax);
        }
    }

    private void Awake()
    {
        playerRenderer = GetComponent<PlayerRenderer>();
        playerLifeController = GetComponent<PlayerLifeController>();
    }

    void OnEnable()
    {
        inputHandler.OnReverseButton += OnRewind;
        inputHandler.OnReverseButtonUp += OnNormal;

        inputHandler.OnLeftButtonDown += OnButtonDown;
        inputHandler.OnRightButtonDown += OnButtonDown;
        inputHandler.OnUpButtonDown += OnButtonDown;
        inputHandler.OnDownButtonDown += OnButtonDown;
    }

    private void OnDisable()
    {
        inputHandler.OnReverseButton -= OnRewind;
        inputHandler.OnReverseButtonUp -= OnNormal;

        inputHandler.OnLeftButtonDown -= OnButtonDown;
        inputHandler.OnRightButtonDown -= OnButtonDown;
        inputHandler.OnUpButtonDown -= OnButtonDown;
        inputHandler.OnDownButtonDown -= OnButtonDown;
    }

    private void OnRewind()
    {
        OnRewindEvent?.Invoke();
    }

    private void OnNormal()
    {
        OnNormalEvent?.Invoke();
    }

    private void OnButtonDown(Direction direction)
    {
        int score = scoreManager.AddScore(direction);

        if (score == -1 || score == 0)
        {
            playerLifeController.LoseLife();
            DecreasePleasing();
            return;
        }

        playerRenderer.UpdateScoreText(score);
        
        CorrectHit();
    }

    private void CorrectHit()
    {
        OnCorrectHitEvent?.Invoke();
        IncreasePleasing();
    }

    private void IncreasePleasing()
    {
        if(PleasingAmount >= pleasingMax)
        {
            PleasingAmount = pleasingMax;
            return;
        }

        PleasingAmount += 0.5f;
    }

    private void DecreasePleasing()
    {
        if (PleasingAmount <= 0)
        {
            PleasingAmount = 0;
            return;
        }

        PleasingAmount -= 1f;
    }
}
