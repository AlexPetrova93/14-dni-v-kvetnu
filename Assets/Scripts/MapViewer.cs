using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class MapViewer : MonoBehaviour
{
    public Vector2 Min;
    public Vector2 Max;

    public GameObject[] Phases;
    [FormerlySerializedAs("MountainsNodeNames")] public MonologueNodeName[] RequiredNodeNames;
    
    public UnityEvent OnRequiredElementFound { get; } = new();

    // PRIVATE

    private MonologueManager _monologueManager;
    private Camera _cam;

    private ClickableMapElement _clickedElement;
    
    private Vector2 _mapLastPos;
    private Vector2 _mapDefaultPos;
    private Vector3 _touchBeganPos;

    private int _currentPhaseIndex;
    
    private float _inputBeginTime;
    
    private bool _dragging;
    private bool _clicking;
    private bool _acceptInput;
    private bool _canRunElementsMonologue;

    public void Init(MonologueManager monologueManager, Camera cam)
    {
        _monologueManager = monologueManager;
        _cam = cam;
        _mapDefaultPos = transform.position;
        _mapLastPos = _mapDefaultPos;
    }

    private void Update()
    {
        if (!_acceptInput) return;

        var input = Utility.GetInput();
        
        var worldPos = _cam.ScreenToWorldPoint(input.Position);
        worldPos.z = 0;

        if (input.Began)
        {
            _clickedElement = GetClickableElement(worldPos);
            if (_clickedElement != null)
            {
                _clickedElement.BeginClick();
                _clicking = true;
            }
            _inputBeginTime = Time.time;
            _dragging = true;
            _touchBeganPos = worldPos;
        }

        if (_dragging && input.Moved)
        {
            var offset = worldPos - _touchBeganPos;
            var mapPos = _mapLastPos + (Vector2)offset;
            mapPos.x = Mathf.Clamp(mapPos.x, Min.x, Max.x);
            mapPos.y = Mathf.Clamp(mapPos.y, Min.y, Max.y);
            transform.position = mapPos;
        }

        if (input.Ended)
        {
            var nodeName = MonologueNodeName.None;
            var clickedOnElement = false;

            if (_clicking && _clickedElement != null) 
            {
                if (Time.time - _inputBeginTime < .2f)
                {
                    var element = GetClickableElement(worldPos);
                    if (_clickedElement == element)
                    {
                        clickedOnElement = true;
                        nodeName = element.MonologueNodeName;
                    }
                }
                _clickedElement.CancelClick(); // cancel click regardless if input ended on it
            }

            if (clickedOnElement)
            {
                if (_canRunElementsMonologue) StartCoroutine(RunElementMonologue(nodeName));
                _clickedElement.EndClick();
            }
            
            _mapLastPos = transform.position;
            _clickedElement = null;
            _clicking = false;
            _dragging = false;
        }
    }

    private ClickableMapElement GetClickableElement(Vector2 worldPos)
    {
        var hitInformation = Physics2D.Raycast(worldPos, _cam.transform.forward);

        if (hitInformation.collider != null) 
        {
            if (hitInformation.collider.TryGetComponent<ClickableMapElement>(out var element))
            {
                return element;
            }
        }

        return null;
    }

    private IEnumerator RunElementMonologue(MonologueNodeName nodeName)
    {
        _canRunElementsMonologue = false;
        yield return _monologueManager.RunMonologue(nodeName);
        if (nodeName == RequiredNodeNames[_currentPhaseIndex]) OnRequiredElementFound.Invoke();
        _canRunElementsMonologue = true;
    }
    
    public void StartPhase(int phaseIndex)
    {
        _currentPhaseIndex = phaseIndex;
        for (var i = 0; i < Phases.Length; i++)
        {
            var phase = Phases[i];
            phase.SetActive(i == phaseIndex);
        }
    }

    public void Activate()
    {
        _mapLastPos = transform.position;
        _acceptInput = true;
    }

    public void ActivateElementsMonologue() => _canRunElementsMonologue = true;

    public void PauseInput()
    {
        _acceptInput = false;
        _canRunElementsMonologue = false;
        _dragging = false;
        _clicking = false;
    }

    public void Deactivate(float time)
    {
        transform.DOMove(_mapDefaultPos, time);
        _acceptInput = false;
        _canRunElementsMonologue = false;
    }
}
