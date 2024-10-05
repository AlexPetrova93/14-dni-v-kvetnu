using System;
using UnityEngine;
using UnityEngine.Events;

public class TexturePainter : MonoBehaviour
{
    public float DebugCompleteFactor;
    
    public Material SpriteMaterial;
    public Texture2D PaintMask;

    public Texture2D[] PhaseTextures;

    public bool UseBrush;
    public float BrushSize = 50;
    public Texture2D Brush;
    
    public UnityEvent OnPaintingComplete { get; } = new ();
    
    // private
    private Camera _cam;
    private SpriteRenderer _renderer;
    private Sprite _sprite;
    
    private byte[] _maskPixels;
    private byte[] _brushPixels;
    
    private Vector2 _brushSize;
    
    private bool _gameOn;
    
    private static readonly int Mask = Shader.PropertyToID("_Mask");
    private static readonly int SecondTex = Shader.PropertyToID("_SecondTex");

    public void Init(Camera cam)
    {
        _cam = cam; 
        _renderer = GetComponent<SpriteRenderer>();
        _sprite = _renderer.sprite;
        if (UseBrush)
        {
            _brushPixels = new byte[Brush.width*Brush.height];
            _brushPixels = Brush.GetPixelData<byte>(0).ToArray();
            _brushSize = new Vector2(Brush.width, Brush.height);
        }
        _maskPixels = new byte[PaintMask.width*PaintMask.height];
    }
    
    public void StartGame(int phaseIndex)
    {
        PaintMask = new Texture2D(PaintMask.width, PaintMask.height, TextureFormat.Alpha8, true)
        {
            filterMode = FilterMode.Bilinear,
        };
        ResetTexture();
        SpriteMaterial.SetTexture(Mask, PaintMask);
        SpriteMaterial.SetTexture(SecondTex, PhaseTextures[phaseIndex%PhaseTextures.Length]);
        _gameOn = true;
    }

    public void PauseGame() => _gameOn = false;
    
    public void ResumeGame() => _gameOn = true;

    private void Update()
    {
        if (!_gameOn) return;
        
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.R)) ResetTexture();
        #endif
        
        var input = Utility.GetInput();
        // TODO better input checks
        if (input.Moved)
        {
            // TODO better painting - lerp extra brush to mask between stroke frames
            // ex. - when dist in mousepos btw this and last fram > threshold -> lerp extra frames (call Paint()) acc. to distance
            if (UseBrush) PaintBrush(Input.mousePosition);
            else Paint(Input.mousePosition);
        }
        if (input.Ended) CheckCompleteFactor();
    }
    
    public void ResetTexture()
    {
        PaintMask = new Texture2D(PaintMask.width, PaintMask.height, TextureFormat.Alpha8, true)
        {
            filterMode = FilterMode.Bilinear
        };
        for (var i = 0; i < PaintMask.width; i++)
        {
            for (var j = 0; j < PaintMask.height; j++)
            {
                _maskPixels[j * PaintMask.width + i] = 0;
            }
        }

        PaintMask.SetPixelData(_maskPixels, 0);
        PaintMask.Apply();
        SpriteMaterial.SetTexture(Mask, PaintMask);
    }
    
    private void Paint(Vector2 pos)
    {
        var spritePos = GetSpritePos(pos);
        //Debug.Log("spritePos = " + spritePos);
        if(IsSpritePosOnSprite(spritePos)) {
                //Debug.Log("painting at " + spritePos);
                var brushPos = spritePos;
                brushPos.x *= PaintMask.width;
                brushPos.y *= PaintMask.height;
                var min = brushPos - Vector2.one * BrushSize / 2;
                var max = brushPos + Vector2.one * BrushSize / 2;
                var minX = Mathf.FloorToInt(Mathf.Clamp(min.x, 0, PaintMask.width));
                var maxX = Mathf.FloorToInt(Mathf.Clamp(max.x, 0, PaintMask.width));
                var minY = Mathf.FloorToInt(Mathf.Clamp(min.y, 0, PaintMask.height));
                var maxY = Mathf.FloorToInt(Mathf.Clamp(max.y, 0, PaintMask.height));
        
                for (var i = minX; i < maxX; i++)
                {
                    for (var j = minY; j < maxY; j++)
                    {
                        var idx = j * PaintMask.width + i;
                        var pixel = _maskPixels[idx];
                        var pixelByte = Math.Clamp(pixel + 255, 0, 255);
                        _maskPixels[idx] = (byte)pixelByte;
                    }
                }
                
                PaintMask.SetPixelData(_maskPixels,0);
                PaintMask.Apply();
                SpriteMaterial.SetTexture(Mask, PaintMask);
        }
    }

    private void CheckCompleteFactor()
    {
        var factor = 0f;
        foreach (var maskPixel in _maskPixels)
        {
            factor += maskPixel / 256f;
        }

        factor /= _maskPixels.Length;

        DebugCompleteFactor = factor;
        
        if (factor > .6f)
        {
            OnPaintingComplete.Invoke();
        }
    }
    
    private void PaintBrush(Vector2 pos)
    {
        var spritePos = GetSpritePos(pos);
        //Debug.Log("spritePos = " + spritePos);
        if(IsSpritePosOnSprite(spritePos)) {
            //Debug.Log("painting at " + spritePos);
            var brushPos = spritePos;
            brushPos.x *= PaintMask.width;
            brushPos.y *= PaintMask.height;
            var min = brushPos - _brushSize / 2;
            var startX = Mathf.FloorToInt(Mathf.Clamp(min.x, 0, PaintMask.width));
            var startY = Mathf.FloorToInt(Mathf.Clamp(min.y, 0, PaintMask.height));
        
            for (var i = 0; i < Brush.width; i++)
            {
                if (i+startX >= PaintMask.width) break;
                for (var j = 0; j < Brush.height; j++)
                {
                    if (j+startY >= PaintMask.height) break;
                    
                    var idx = j * Brush.width + i;
                    var spriteIdx = (j+startY) * PaintMask.width + i+startX;
                    var pixel = _maskPixels[spriteIdx];
                    _maskPixels[spriteIdx] = (byte)Math.Clamp(pixel+_brushPixels[idx], 0, 255);
                }
            }
                
            PaintMask.SetPixelData(_maskPixels,0);
            PaintMask.Apply();
            SpriteMaterial.SetTexture(Mask, PaintMask);
        }
    }
    
    private Vector2 GetSpritePos(Vector2 inputPos)
    {
        var worldPos = _cam.ScreenToWorldPoint(inputPos);
        var objPoint = transform.InverseTransformPoint(worldPos);
        objPoint += _sprite.bounds.extents;
        objPoint.x /= _sprite.bounds.size.x;
        objPoint.y /= _sprite.bounds.size.y;
        return objPoint;
    }

    private bool IsSpritePosOnSprite(Vector2 pos)
    {
        return pos.x >= 0 && pos.y >= 0 && pos.x <= 1 && pos.y <= 1;
    }
    
    public static Vector3[] GetSpriteCorners(SpriteRenderer renderer)
    {
        Vector3 topRight = renderer.transform.TransformPoint(renderer.sprite.bounds.max);
        Vector3 topLeft = renderer.transform.TransformPoint(new Vector3(renderer.sprite.bounds.max.x, renderer.sprite.bounds.min.y, 0));
        Vector3 botLeft = renderer.transform.TransformPoint(renderer.sprite.bounds.min);
        Vector3 botRight = renderer.transform.TransformPoint(new Vector3(renderer.sprite.bounds.min.x, renderer.sprite.bounds.max.y, 0));
        return new Vector3[] { topRight, topLeft, botLeft, botRight };
    }
}
