using UnityEngine;

public enum PlayerCodeNumber { ONE, TWO, THREE, FOUR }

public enum Direction 
{ 
    LEFT,
    RIGHT,
    UP,
    DOWN 
}

public class InputHandler : MonoBehaviour
{
    public PlayerCodeNumber playerNumber;

    public delegate void OnReverseButtonDelegate();
    public delegate void OnReverseButtonUpDelegate();
    public delegate void OnLeftButtonDownDelegate(Direction direction);
    public delegate void OnRightButtonDownDelegate(Direction direction);
    public delegate void OnUpButtonDownDelegate(Direction direction);
    public delegate void OnDownButtonDownDelegate(Direction direction);

    public event OnReverseButtonDelegate OnReverseButton;
    public event OnReverseButtonUpDelegate OnReverseButtonUp;
    public event OnLeftButtonDownDelegate OnLeftButtonDown;
    public event OnRightButtonDownDelegate OnRightButtonDown;
    public event OnUpButtonDownDelegate OnUpButtonDown;
    public event OnDownButtonDownDelegate OnDownButtonDown;

    void Update()
    {
        if (GameStateManager.Instance.ActualGameState != GameState.NORMAL &&
           GameStateManager.Instance.ActualGameState != GameState.REWIND) return;

        switch (playerNumber)
        {
            case PlayerCodeNumber.ONE:
                CheckPlayerOneInput();
                break;
            case PlayerCodeNumber.TWO:
                CheckPlayerTwoInput();
                break;
            case PlayerCodeNumber.THREE:

                break;
            case PlayerCodeNumber.FOUR:

                break;
        }
    }

    private void CheckPlayerOneInput()
    {
        if(Input.GetKeyDown(KeyCode.D))
        {
            RightButtonDown();
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.A))
            {
                LeftButtonDown();
            }
            else
            {
                if(Input.GetKeyDown(KeyCode.W))
                {
                    UpButtonDown();
                }
                else
                {
                    if(Input.GetKeyDown(KeyCode.S))
                    {
                        DownButtonDown();
                    }
                }
            }
        }

        if (Input.GetKey(KeyCode.Space))
        {
            ReverseButton();
        }
        else
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                ReverseButtonUp();
            }
        }
    }

    private void CheckPlayerTwoInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            RightButtonDown();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                LeftButtonDown();
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    UpButtonDown();
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        DownButtonDown();
                    }
                }
            }
        }

        if (Input.GetKey(KeyCode.RightControl))
        {
            ReverseButton();
        }
        else
        {
            if (Input.GetKeyUp(KeyCode.RightControl))
            {
                ReverseButtonUp();
            }
        }
    }

    private void ReverseButton()
    {
        OnReverseButton?.Invoke();
    }

    private void ReverseButtonUp()
    {
        OnReverseButtonUp?.Invoke();
    }

    private void LeftButtonDown()
    {
        OnLeftButtonDown?.Invoke(Direction.LEFT);
    }

    private void RightButtonDown()
    {
        OnRightButtonDown?.Invoke(Direction.RIGHT);
    }

    private void UpButtonDown()
    {
        OnUpButtonDown?.Invoke(Direction.UP);
    }

    private void DownButtonDown()
    {
        OnDownButtonDown?.Invoke(Direction.DOWN);
    }
}
