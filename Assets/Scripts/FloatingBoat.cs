using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FloatingBoat : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer;

    public float MoveSpeed = .2f;
    
    private Vector2 _minMaxX;
    
    private void Start()
    {
        _minMaxX = new Vector2(transform.localPosition.x - 2, transform.localPosition.x + 2);
        MoveSpeed *= -1;
    }

    private void Update()
    {
        if (!gameObject.activeInHierarchy) return;
        
        var moveX = Time.deltaTime * MoveSpeed;
        if (transform.localPosition.x + moveX < _minMaxX.x || transform.localPosition.x + moveX > _minMaxX.y)
        {
            MoveSpeed *= -1;
            SpriteRenderer.flipX = !SpriteRenderer.flipX;
        }
        
        // TODO y + sin(time)

        transform.localPosition += moveX*Vector3.left;
    }

    private void Reset()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }
}
