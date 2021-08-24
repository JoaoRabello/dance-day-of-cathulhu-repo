using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifeController : MonoBehaviour
{
    private PlayerRenderer playerRenderer;

    [SerializeField] private int lifeMax;

    private int lifeAmount;
    public int LifeAmount
    {
        get => lifeAmount;
        set
        {
            lifeAmount = value;
            playerRenderer.UpdateLife(value);
        }
    }

    private void Start()
    {
        playerRenderer = GetComponent<PlayerRenderer>();
        LifeAmount = lifeMax;
    }

    public void LoseLife()
    {
        if(LifeAmount > 0)
        {
            LifeAmount--;
        }
        else
        {
            GameStateManager.Instance.OnDie(GetComponent<InputHandler>().playerNumber);
        }
    }
}
