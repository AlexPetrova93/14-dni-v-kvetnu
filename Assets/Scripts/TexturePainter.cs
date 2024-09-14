using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class TexturePainter : MonoBehaviour
{
    public Material SpriteMaterial;
    public Texture2D PaintMask;

    public float BrushSize = 50;
    
    public bool UseBrush;
    public Texture2D Brush;
    
    // private
    private Camera _cam;
    private SpriteRenderer _renderer;
    private Sprite _sprite;
    private byte[] _maskPixels;
    private static readonly int Mask = Shader.PropertyToID("_Mask");
    private byte[] _brushPixels;
    private Vector2 _brushSize;
    
    private void Start()
    {
        _cam = Camera.main; 
        _renderer = GetComponent<SpriteRenderer>();
        _sprite = _renderer.sprite;
        _maskPixels = new byte[PaintMask.width*PaintMask.height];
        if (UseBrush)
        {
            _brushPixels = new byte[Brush.width*Brush.height];
            _brushPixels = Brush.GetPixelData<byte>(0).ToArray();
            _brushSize = new Vector2(Brush.width, Brush.height);
        }
        PaintMask = new Texture2D(PaintMask.width, PaintMask.height, TextureFormat.Alpha8, true);
        PaintMask.filterMode = FilterMode.Bilinear;
        SpriteMaterial.SetTexture(Mask, PaintMask);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) ResetTexture();

        if (Input.GetMouseButton(0))
        {
            if (UseBrush) PaintBrush(Input.mousePosition);
            else Paint(Input.mousePosition);
        }
    }

    private void ResetTexture()
    {
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
                        _maskPixels[idx] = (byte)Math.Clamp(pixel+255, 0, 255);
                    }
                }
                
                PaintMask.SetPixelData(_maskPixels,0);
                PaintMask.Apply();
                SpriteMaterial.SetTexture(Mask, PaintMask);
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
