using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class BasePanel : MonoBehaviour
{
    protected CanvasGroup canvasGroup;

    protected virtual void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        ShowUI(false);
    }

    public virtual void ShowUI(bool isShow)
    {
        canvasGroup.alpha = isShow? 1.0f : 0f;
        canvasGroup.interactable = isShow;
        canvasGroup.blocksRaycasts = isShow;
    }
}
