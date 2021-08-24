using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct RewindState
{
    public PlayerCodeNumber PlayerNumber; 
    public float Time;
    public int LifeAmount;
    public float PleasingAmount;

    public RewindState(PlayerCodeNumber playerNumber, float time, int lifeAmount, float pleasingAmount)
    {
        PlayerNumber = playerNumber;
        Time = time;
        LifeAmount = lifeAmount;
        PleasingAmount = pleasingAmount;
    }
}


public class Rewinder : MonoBehaviour
{
    [SerializeField] private float recordTime;
    private float startTime;
    private float time;
    private List<RewindState> rewindsPlayer1 = new List<RewindState>();
    private List<RewindState> rewindsPlayer2 = new List<RewindState>();

    [SerializeField] private Player player1;
    [SerializeField] private PlayerLifeController player1Life;
    [SerializeField] private Player player2;
    [SerializeField] private PlayerLifeController player2Life;

    private float rewindPowerAmount = 0;
    [SerializeField] private float rewindPowerMax;

    private bool canRewind = false;

    public Slider reversePowerSlider;

    private void Start()
    {
        startTime = Time.time;
    }

    private void Update()
    {
        switch (GameStateManager.Instance.ActualGameState)
        {
            case GameState.NORMAL:
                startTime += Time.deltaTime;
                time = (float)Math.Round(startTime, 1);

                Record(time);
                break;
            case GameState.REWIND:
                startTime -= Time.deltaTime;
                time = (float)Math.Round(startTime, 1);
                break;
        }
    }

    private void Record(float timeStamp)
    {
        if(rewindsPlayer1.Count > Mathf.Round(recordTime / Time.deltaTime))
        {
            rewindsPlayer1.RemoveAt(rewindsPlayer1.Count - 1);
        }

        if(rewindsPlayer2.Count > Mathf.Round(recordTime / Time.deltaTime))
        {
            rewindsPlayer2.RemoveAt(rewindsPlayer2.Count - 1);
        }

        rewindsPlayer1.Insert(0, new RewindState(PlayerCodeNumber.ONE, timeStamp, player1Life.LifeAmount, player1.PleasingAmount));
        rewindsPlayer2.Insert(0, new RewindState(PlayerCodeNumber.TWO, timeStamp, player2Life.LifeAmount, player2.PleasingAmount));
    }

    public void Normal()
    {
        canRewind = false;

        GameStateManager.Instance.OnNormal();
    }

    public void Rewind()
    {
        if (rewindPowerAmount >= rewindPowerMax)
        {
            canRewind = true;
        }

        if (!canRewind || rewindPowerAmount <= 0)
        {
            Normal();
            return;
        }

        SetPlayerValues();

        GameStateManager.Instance.OnRewind();
        ReduceRewindPower();
    }

    private void SetPlayerValues()
    {
        if(rewindsPlayer1.Count > 0)
        {
            var rewind = rewindsPlayer1[0];
            player1.PleasingAmount = rewind.PleasingAmount;
            player1Life.LifeAmount = rewind.LifeAmount;
            rewindsPlayer1.RemoveAt(0);
        }

        if (rewindsPlayer2.Count > 0)
        {
            var rewind = rewindsPlayer2[0];
            player2.PleasingAmount = rewind.PleasingAmount;
            player2Life.LifeAmount = rewind.LifeAmount;

            rewindsPlayer2.RemoveAt(0);
        }
    }

    private void ReduceRewindPower()
    {
        rewindPowerAmount -= Time.deltaTime;
        UpdateRewindPowerSlider();
    }

    public void IncreaseRewindPower()
    {
        if (rewindPowerAmount >= rewindPowerMax)
        {
            rewindPowerAmount = rewindPowerMax;
            UpdateRewindPowerSlider();
            return;
        }

        rewindPowerAmount++;
        UpdateRewindPowerSlider();
    }

    private void UpdateRewindPowerSlider()
    {
        reversePowerSlider.value = rewindPowerAmount / rewindPowerMax;
    }
}
