using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public enum GameState
{
    PAUSE,
    NORMAL,
    REWIND,
    DEFEAT,
    VICTORY
}

public enum PlayerState
{
    ALIVE,
    DEAD
}

public struct PlayerHolder
{
    public PlayerCodeNumber playerCode;
    public PlayerState playerState;

    public PlayerHolder(PlayerCodeNumber playerCode, PlayerState playerState)
    {
        this.playerCode = playerCode;
        this.playerState = playerState;
    }
}

public class GameStateManager : MonoBehaviour
{
    public GameState ActualGameState;
    public static GameStateManager Instance;

    [SerializeField] private TextMeshProUGUI announceLabel;
    
    public UnityEvent OnStartGame;
    public UnityEvent OnDefeat;

    private List<PlayerHolder> players = new List<PlayerHolder> 
    { 
        new PlayerHolder(PlayerCodeNumber.ONE, PlayerState.ALIVE),
        new PlayerHolder(PlayerCodeNumber.TWO, PlayerState.ALIVE)
    };

    void Start()
    {
        if(!Instance) Instance = this;

        ActualGameState = GameState.PAUSE;
    }

    public void OnPlay()
    {
        StartCoroutine(StartGame());
    }

    public void OnRewind()
    {
        ActualGameState = GameState.REWIND;
    }

    public void OnNormal()
    {
        ActualGameState = GameState.NORMAL;
    }

    public void OnDie(PlayerCodeNumber playerCode)
    {
        switch (playerCode)
        {
            case PlayerCodeNumber.ONE:
                if(players[1].playerState == PlayerState.DEAD)
                {
                    ActualGameState = GameState.DEFEAT;
                    OnDefeat?.Invoke();
                }
                else
                {
                    var player = players[0];
                    player.playerState = PlayerState.DEAD;
                    players[0] = player;
                }
                break;
            case PlayerCodeNumber.TWO:
                if (players[0].playerState == PlayerState.DEAD)
                {
                    ActualGameState = GameState.DEFEAT;
                    OnDefeat?.Invoke();
                }
                else
                {
                    var player = players[1];
                    player.playerState = PlayerState.DEAD;
                    players[1] = player;
                }
                break;
        }
    }

    public void OnWin()
    {
        ActualGameState = GameState.VICTORY;
    }

    private IEnumerator StartGame()
    {
        announceLabel.SetText("3");
        announceLabel.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        announceLabel.SetText("2");
        yield return new WaitForSeconds(1f);
        announceLabel.SetText("1");
        yield return new WaitForSeconds(1f);
        announceLabel.SetText("GO!");
        yield return new WaitForSeconds(0.3f);
        ActualGameState = GameState.NORMAL;

        OnStartGame?.Invoke();
    }
}
