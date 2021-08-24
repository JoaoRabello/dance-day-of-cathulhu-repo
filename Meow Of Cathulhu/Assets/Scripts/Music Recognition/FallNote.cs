using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FallNote : MonoBehaviour
{
    [SerializeField] private float speed;
    
    [SerializeField] private List<Sprite> sprites = new List<Sprite>();
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Count-1)];
    }

    void Update()
    {
        transform.Translate(Vector3.left * (speed * Time.deltaTime));
    }
}
