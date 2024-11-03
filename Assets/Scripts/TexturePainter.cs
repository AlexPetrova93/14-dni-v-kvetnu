using System;
using UnityEngine;
using UnityEngine.Events;

public class TexturePainter : MonoBehaviour
{
    public CustomRenderTexture PaintingRenderTexture;
    public Material RenderTextureMaterial;
    public Material SpriteMaterial;
    public Texture2D PaintMask;

    public Texture2D[] PhaseTextures;

    public float BrushMaxSpacing = 5;
    
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

    private Vector4 _brushInfo;
    private Vector2 _brushSize;
    private Vector2 _lastInputPos;
    private Vector2 _lastSpritePos;

    private bool _painting;
    private bool _gameOn;
    
    private static readonly int MaskParam = Shader.PropertyToID("_Mask");
    private static readonly int SecondTexParam = Shader.PropertyToID("_SecondTex");
    private static readonly int BrushInfoParam = Shader.PropertyToID("_BrushInfo");
    //private static readonly int PastBrushInfoParam = Shader.PropertyToID("_PastBrushInfo");

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
        _brushInfo = new Vector4(0, 0, BrushSize, BrushSize);
    }
    
    public void StartGame(int phaseIndex)
    {
        /*PaintMask = new Texture2D(PaintMask.width, PaintMask.height, TextureFormat.Alpha8, true)
        {
            filterMode = FilterMode.Bilinear,
        };
        ResetTexture();*/
        SpriteMaterial.SetTexture(SecondTexParam, PhaseTextures[phaseIndex%PhaseTextures.Length]);
        _gameOn = true;
    }

    public void PauseGame() => _gameOn = false;
    
    public void ResumeGame() => _gameOn = true;
    
    private void Update()
    {
        if (!_gameOn) return;

        //BIG TODO move this to shader - i.e. even brush texture is a texture in the material, not here
        var input = Utility.GetInput();
        if (input.Began)
        {
            _lastInputPos = input.Position;
            var spritePos = GetSpritePos(input.Position);
            if(IsSpritePosOnSprite(spritePos)) _painting = true;
            _lastSize = BrushSize;
        }
        if (_painting && input.Moved)
        {
            // Lerp extra brushes to mask between stroke frames, if spacing too high
            var dist = Vector2.Distance(_lastInputPos, input.Position);
            if (dist > BrushMaxSpacing)
            {
                var numSteps = Mathf.FloorToInt(dist / BrushMaxSpacing);
                for (var i = 0; i < numSteps; i++)
                {
                    var pos = Vector2.Lerp(_lastInputPos, input.Position, i/(float)numSteps);
                    if (UseBrush) PaintBrush(pos, false);
                    else Paint(pos, _lastInputPos);
                }
            }
            
            if (UseBrush) PaintBrush(input.Position, true);
            else Paint(input.Position, _lastInputPos);
            _lastInputPos = input.Position;
        }

        if (input.Ended)
        {
            _painting = false;
            CheckCompleteFactor();
        }
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
        PaintingRenderTexture.initializationTexture = PaintMask;
        PaintingRenderTexture.Initialize();
    }

    private float _lastSize;
    private void Paint(Vector2 pos, Vector2 lastPos)
    {
        var maskSize = new Vector2(PaintMask.width, PaintMask.height);
        var spritePos = GetSpritePos(pos) * maskSize;
        var lastSpritePos = GetSpritePos(lastPos) * maskSize;
        
        
        var center = (spritePos + lastSpritePos)/2;
        var size = BrushSize + Vector2.Distance(spritePos, lastSpritePos);
        var lerpedSize = Mathf.Min(Mathf.Lerp(_lastSize, size, Time.deltaTime*50), BrushSize*2);
        _lastSize = lerpedSize;
        
        _brushInfo.x = spritePos.x;
        _brushInfo.y = spritePos.y;
        _brushInfo.z = lerpedSize;
        _brushInfo.w = lerpedSize;
        RenderTextureMaterial.SetVector(BrushInfoParam, _brushInfo);
        PaintingRenderTexture.Update();
    }

    private void CheckCompleteFactor()
    {
        var pixels = GetRTPixels();
        
        var factor = 0f;
        foreach (var maskPixel in pixels)
        {
            factor += maskPixel.r / 256f;
        }
        factor /= pixels.Length;
        
        if (factor > .6f)
        {
            OnPaintingComplete.Invoke();
        }
    }

    private Color32[] GetRTPixels()
    {
        var temp = new Texture2D(PaintMask.width, PaintMask.height, TextureFormat.RGBAFloat, false);
        var oldRT = RenderTexture.active;
        RenderTexture.active = PaintingRenderTexture;
        temp.ReadPixels(new Rect(0, 0, temp.width, temp.height), 0, 0, false);
        temp.Apply();
        RenderTexture.active = oldRT;
        var pixels = temp.GetPixels32();
        Destroy(temp);
        return pixels;
    }
    
    private void PaintBrush(Vector2 pos, bool updateTexture)
    {
        var spritePos = GetSpritePos(pos);
        if (IsSpritePosOnSprite(spritePos)) 
        {
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

                    if (idx >= _brushPixels.Length || spriteIdx >= _maskPixels.Length) break;  // TODO this shouldn't be necessary
                    
                    var pixel = _maskPixels[spriteIdx];
                    _maskPixels[spriteIdx] = (byte)Math.Clamp(pixel+_brushPixels[idx], 0, 255);
                }
            }

            if (updateTexture)
            {
                PaintMask.SetPixelData(_maskPixels,0);
                PaintMask.Apply();
            }
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
}
