using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteRenderer : MonoBehaviour
{
    public Direction NoteDirection;
    public bool IsActualNote;
    public bool IsLastNote;
    [SerializeField] private Color selectedColor;

    private Transform spawnPosition;
    private Transform destroyPosition;
    
    private SpriteRenderer spriteRenderer;

    [SerializeField] private float speed;
    private float reverseSpeed;
    private float normalSpeed;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        normalSpeed = speed;
        reverseSpeed = -speed;
    }

    private void Update()
    {
        NoteRender();
    }

    private void NoteRender()
    {
        switch (GameStateManager.Instance.ActualGameState)
        {
            case GameState.NORMAL:
                speed = normalSpeed;
                break;
            case GameState.REWIND:
                speed = reverseSpeed;
                break;
            default:
                return;
        }
        
        transform.Translate(Vector2.left * speed * Time.deltaTime);
        
        if (IsActualNote)
            spriteRenderer.color = selectedColor;
        else
            spriteRenderer.color = Color.white;
        
        var oldColor = spriteRenderer.color;

        var alphaBeforeMiddle = (spawnPosition.position.x - transform.position.x) / spawnPosition.position.x;
        var alphaAfterMiddle = Mathf.Abs((destroyPosition.position.x - transform.position.x) / destroyPosition.position.x);
        float alpha = transform.position.x >= 0 ? alphaBeforeMiddle : alphaAfterMiddle;

        if (transform.position.x <= destroyPosition.position.x)
            alpha = 0;

        spriteRenderer.color = new Color(oldColor.r, oldColor.g, oldColor.b, alpha);
    }

    public void SetWaypoints(Transform spawn, Transform destroy)
    {
        spawnPosition = spawn;
        destroyPosition = destroy;
    }
}
