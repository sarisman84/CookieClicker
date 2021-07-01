using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DoTweenManager : MonoBehaviour
{
    public List<DoTweenSettings> settings;
    public bool runOnAwake;
    private Coroutine _coroutine;

    private void Awake()
    {
        if (runOnAwake)
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
            StartCoroutine(Tween());
        }
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
                        yield return transform.DOMove(setting.targetMovement, setting.transitionDuration_Movement)
                            .SetEase(setting.transitionType_Movement).WaitForCompletion();
                    }
                    else
                        transform.DOMove(setting.targetMovement, setting.transitionDuration_Movement)
                            .SetEase(setting.transitionType_Movement);

                    break;
                case DoTweenSettings.TweenType.Rescale:
                    if (setting.playBelowTweenAfterCompletingCurrentTween)
                    {
                        yield return transform.DOScale(setting.targetScale, setting.transitionDuration_Scale)
                            .SetEase(setting.transitionType_Scale).WaitForCompletion();
                    }
                    else
                        transform.DOScale(setting.targetScale, setting.transitionDuration_Scale)
                            .SetEase(setting.transitionType_Scale);

                    break;
                case DoTweenSettings.TweenType.Rotate:
                    if (setting.playBelowTweenAfterCompletingCurrentTween)
                    {
                        yield return transform
                            .DORotateQuaternion(setting.targetRotation, setting.transitionDuration_Rotation)
                            .SetEase(setting.transitionType_Rotation).WaitForCompletion();
                    }
                    else
                        transform.DORotateQuaternion(setting.targetRotation, setting.transitionDuration_Rotation)
                            .SetEase(setting.transitionType_Rotation);

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
        Rotate
    }

    public TweenType tweenType;

    public Vector3 targetMovement;
    public float transitionDuration_Movement;
    public Ease transitionType_Movement;

    public Vector3 targetScale;
    public float transitionDuration_Scale;
    public Ease transitionType_Scale;

    public Quaternion targetRotation;
    public float transitionDuration_Rotation;
    public Ease transitionType_Rotation;

    public bool playBelowTweenAfterCompletingCurrentTween;
}