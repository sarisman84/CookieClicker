using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DoTweenManager : MonoBehaviour
{
    public List<DoTweenSettings> settings;
    public bool runOnAwake;
    private Coroutine _coroutine;

    private void Awake()
    {
        if (runOnAwake)
        {
            StartTweening();
        }
    }

    private Vector3 postionSinceTweening;

    public void StartTweening()
    {
        postionSinceTweening = transform.position;
        if (_coroutine != null)
            StopCoroutine(_coroutine);
        StartCoroutine(Tween());
    }

    public IEnumerator Tween()
    {
        foreach (var setting in settings)
        {
            switch (setting.tweenType)
            {
                case DoTweenSettings.TweenType.Move:

                    if (setting.playBelowTweenAfterCompletingCurrentTween)
                    {
                        yield return (setting.useLocalMovement)
                            ? transform
                                .DOLocalMove(setting.targetMovement,
                                    setting.transitionDurationMovement)
                                .SetEase(setting.transitionTypeMovement).WaitForCompletion()
                            : transform
                                .DOMove(setting.targetMovement,
                                    setting.transitionDurationMovement)
                                .SetEase(setting.transitionTypeMovement).WaitForCompletion();
                    }
                    else if (setting.useLocalMovement)
                        transform
                            .DOLocalMove(setting.targetMovement,
                                setting.transitionDurationMovement)
                            .SetEase(setting.transitionTypeMovement);
                    else
                        transform
                            .DOMove(setting.targetMovement,
                                setting.transitionDurationMovement)
                            .SetEase(setting.transitionTypeMovement);

                    break;
                case DoTweenSettings.TweenType.Rescale:
                    if (setting.playBelowTweenAfterCompletingCurrentTween)
                    {
                        yield return transform.DOScale(setting.targetScale, setting.transitionDurationScale)
                            .SetEase(setting.transitionTypeScale).WaitForCompletion();
                    }
                    else
                        transform.DOScale(setting.targetScale, setting.transitionDurationScale)
                            .SetEase(setting.transitionTypeScale);

                    break;
                case DoTweenSettings.TweenType.Rotate:
                    if (setting.playBelowTweenAfterCompletingCurrentTween)
                    {
                        yield return transform
                            .DORotateQuaternion(setting.targetRotation, setting.transitionDurationRotation)
                            .SetEase(setting.transitionTypeRotation).WaitForCompletion();
                    }
                    else
                        transform
                            .DORotateQuaternion(setting.targetRotation, setting.transitionDurationRotation)
                            .SetEase(setting.transitionTypeRotation);

                    break;

                case DoTweenSettings.TweenType.Fade:
                    List<Component> elementsToFade = new List<Component>();
                    elementsToFade.Add(GetComponent<TMP_Text>());
                    elementsToFade.Add(GetComponent<Text>());
                    elementsToFade.Add(GetComponent<Image>());
                    elementsToFade.Add(GetComponent<CanvasGroup>());


                    foreach (var elementToFade in elementsToFade)
                    {
                        if (setting.playBelowTweenAfterCompletingCurrentTween)
                        {
                            switch (elementToFade)
                            {
                                case Text text:
                                    text.color = new Color(text.color.r, text.color.g, text.color.b,
                                        setting.initialFadeAmm);
                                    yield return text.DOFade(setting.targetFadeAmm, setting.transitionDurationFade)
                                        .SetEase(setting.transitionTypeFade).WaitForCompletion();
                                    break;

                                case TMP_Text tmpText:
                                    tmpText.color = new Color(tmpText.color.r, tmpText.color.g, tmpText.color.b,
                                        setting.initialFadeAmm);
                                    yield return tmpText.DOFade(setting.targetFadeAmm, setting.transitionDurationFade)
                                        .SetEase(setting.transitionTypeFade).WaitForCompletion();
                                    break;

                                case Image image:
                                    image.color = new Color(image.color.r, image.color.g, image.color.b,
                                        setting.initialFadeAmm);
                                    yield return image.DOFade(setting.targetFadeAmm, setting.transitionDurationFade)
                                        .SetEase(setting.transitionTypeFade).WaitForCompletion();
                                    break;

                                case CanvasGroup canvasGroup:
                                    if (canvasGroup)
                                    {
                                        canvasGroup.alpha = setting.initialFadeAmm;
                                        yield return canvasGroup.DOFade(setting.targetFadeAmm,
                                                setting.transitionDurationFade)
                                            .SetEase(setting.transitionTypeFade).WaitForCompletion();
                                    }
                                    break;
                            }

                            if (setting.uponFadeCompletionDestroyGameObject)
                            {
                                Destroy(gameObject);
                                yield return null;
                            }

                            if (setting.uponFadeCompletionDisableGameObject)
                            {
                                gameObject.SetActive(false);
                                yield return null;
                            }
                        }
                        else
                            switch (elementToFade)
                            {
                                case Text text:
                                    text.color = new Color(text.color.r, text.color.g, text.color.b,
                                        setting.initialFadeAmm);
                                    text.DOFade(setting.targetFadeAmm, setting.transitionDurationFade)
                                        .SetEase(setting.transitionTypeFade).WaitForCompletion();
                                    break;

                                case TMP_Text tmpText:
                                    tmpText.color = new Color(tmpText.color.r, tmpText.color.g, tmpText.color.b,
                                        setting.initialFadeAmm);
                                    tmpText.DOFade(setting.targetFadeAmm, setting.transitionDurationFade)
                                        .SetEase(setting.transitionTypeFade).WaitForCompletion();
                                    break;

                                case Image image:
                                    image.color = new Color(image.color.r, image.color.g, image.color.b,
                                        setting.initialFadeAmm);
                                    image.DOFade(setting.targetFadeAmm, setting.transitionDurationFade)
                                        .SetEase(setting.transitionTypeFade).WaitForCompletion();
                                    break;

                                case CanvasGroup canvasGroup:
                                    if (canvasGroup)
                                    {
                                        canvasGroup.alpha = setting.initialFadeAmm;
                                        canvasGroup.DOFade(setting.targetFadeAmm, setting.transitionDurationFade)
                                            .SetEase(setting.transitionTypeFade).WaitForCompletion();
                                    }

                                    break;
                            }
                    }


                    break;
            }

            yield return null;
        }
    }
}


[Serializable]
public struct DoTweenSettings
{
    public enum TweenType
    {
        Move,
        Rescale,
        Rotate,
        Fade
    }

    public TweenType tweenType;

    public Vector3 targetMovement;
    public bool useLocalMovement;
    public float transitionDurationMovement;
    public Ease transitionTypeMovement;

    public Vector3 targetScale;
    public float transitionDurationScale;
    public Ease transitionTypeScale;

    public Quaternion targetRotation;
    public float transitionDurationRotation;
    public Ease transitionTypeRotation;

    public float initialFadeAmm;
    public float targetFadeAmm;
    public float transitionDurationFade;
    public Ease transitionTypeFade;
    public bool uponFadeCompletionDisableGameObject;
    public bool uponFadeCompletionDestroyGameObject;

    public bool playBelowTweenAfterCompletingCurrentTween;
}