using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FloatingBoat : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer;

    public float HalfRange = 3;
    public float WaveStrength = .1f;
    public float WaveFrequency = 2f;
    public float MoveSpeed = .2f;
    
    [SerializeField] private Vector2 _minMaxX;
    private float _origY;
    
    private void Start()
    {
        var localPosition = transform.localPosition;
        _minMaxX = new Vector2(localPosition.x - HalfRange, localPosition.x + HalfRange);
        _origY = localPosition.y;
        MoveSpeed *= -1;
    }

    private void Update()
    {
        if (!gameObject.activeInHierarchy) return;
        
        var moveX = Time.deltaTime * MoveSpeed;
        if (transform.localPosition.x + moveX < _minMaxX.x)
        {
            MoveSpeed *= -1;
            SpriteRenderer.flipX = !SpriteRenderer.flipX;
        }

        if (transform.localPosition.x + moveX > _minMaxX.y)
        {
            MoveSpeed *= -1;
            SpriteRenderer.flipX = !SpriteRenderer.flipX;
            
        }
        
        // TODO y + sin(time)

        var newPos = transform.localPosition;
        newPos.x += moveX;
        newPos.y = _origY + Mathf.Sin(Time.time * WaveFrequency) * WaveStrength;
        transform.localPosition = newPos;
    }

    private void Reset()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }
}
