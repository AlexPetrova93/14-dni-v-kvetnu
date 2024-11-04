using DG.Tweening;
using UnityEngine;

public class TutorialArrow : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer;

    public float MinStretch = .18f;
    public float MaxStretch = .39f;

    private void Start()
    {
        SpriteRenderer.color = new Color(1, 1, 1, 0);
    }

    public void Show()
    {
        SpriteRenderer.DOFade(1, .5f);
        transform.DOScaleX(MaxStretch, 1f).SetEase(Ease.OutBack);
    }

    public void Hide()
    {
        SpriteRenderer.DOFade(0, .3f);
        
    }
}
