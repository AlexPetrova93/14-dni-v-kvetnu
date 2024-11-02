using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class DraggableDoor : MonoBehaviour
{
    private SoundManager _soundManager;
    private Camera _camera;
    public SpriteRenderer DoorHandle;

    public UnityEvent OnDoorOpen { get; } = new();
    
    private Vector3 _inputBeginPos;
    
    private float _lastPositionX;
    private float _defaultPositionX;
    private float _openPositionX;
    private float _halfOpenPosition;
    
    private bool _active;
    private bool _dragging;

    public void Init(SoundManager soundManager, Camera cam)
    {
        _soundManager = soundManager;
        _camera = cam;
        _defaultPositionX = transform.position.x;
        _openPositionX = _defaultPositionX + 3.5f;
        _halfOpenPosition = _defaultPositionX + 1.5f;
    }

    private void Update()
    {
        if (!_active) return;

        var input = Utility.GetInput();

        if (input.Began)
        {
            var worldPos = _camera.ScreenToWorldPoint( input.Position );
            worldPos.z = 0;
            if (DoorHandle.bounds.Contains(worldPos))
            {
                _inputBeginPos = worldPos;
                _lastPositionX = transform.position.x;
                _dragging = true;
                _soundManager.PlaySound(AudioRef.Door);
            }
        }

        if (!_dragging) return;

        if (input.Moved)
        {
            var worldPos = _camera.ScreenToWorldPoint( input.Position );
            var pos = transform.position;
            pos.x = _lastPositionX + worldPos.x - _inputBeginPos.x;
            pos.x = Mathf.Clamp(pos.x, _defaultPositionX, _openPositionX);
            
            if (pos.x > _halfOpenPosition) OnDoorOpen.Invoke();
            
            transform.position = pos;
        }

        if (input.Ended)
        {
            _dragging = false;
        }
    }

    public void OpenFully()
    {
        _dragging = false;
        transform.DOMoveX(_openPositionX, .5f);
    }
    
    public void Activate()
    {
        /*var rot = 0f;
        DOTween.Shake(() => rot * Vector3.one, value => rot = value.x, 1, 30).OnUpdate(() =>
        {
            DoorHandle.transform.localRotation = Quaternion.Euler(0,0,rot);
        });*/
        DoorHandle.transform.DOShakeRotation(1, 30);
        //DoorHandle.DOFade(.5f, .2f);
        _active = true;
    }

    public void Deactivate()
    {
        //DoorHandle.DOFade(0, .2f);
        _active = false;
    }
}
