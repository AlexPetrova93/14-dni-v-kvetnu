using System;
using DG.Tweening;
using UnityEngine;

public class AnimatedSprite : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer;
    public Sprite[] Frames;

    public bool StartActive;
    
    public int StartFrame;
    
    public float FrameRate = 3f;

    private bool _active;
    private bool _animated;
    private float _lastFrameTime;
    private int _lastFrameIdx;
    private static readonly int Empty = Shader.PropertyToID("_Empty");

    private void Start()
    {
        if (StartActive) Activate(0);
        else Deactivate(0);
        _animated = Frames.Length > 0;
        if (_animated) _lastFrameIdx = StartFrame % Frames.Length;
    }

    public void Activate(float time = .4f)
    {
        _active = true;

        if (time > 0) SpriteRenderer.material.DOFloat(0, Empty, time);
        else SpriteRenderer.material.SetFloat(Empty, 0);
    }
    
    public void Deactivate(float time = .4f)
    {
        _active = false;

        if (time > 0) SpriteRenderer.material.DOFloat(1, Empty, time);
        else SpriteRenderer.material.SetFloat(Empty, 1);
    }

    public void PlayFromStart()
    {
        gameObject.SetActive(true);
        _active = true;
        _lastFrameIdx = 0;
        _lastFrameTime = -555;
    }
    
    private void Update()
    {
        if (_animated && _active && gameObject.activeSelf && SpriteRenderer.color.a > 0)
        {
            if (Time.time - _lastFrameTime > 1 / FrameRate)
            {
                _lastFrameTime = Time.time;
                SpriteRenderer.sprite = Frames[_lastFrameIdx];
                ++_lastFrameIdx;
                _lastFrameIdx %= Frames.Length;
            }
        }
    }

    private void Reset()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }
}
