using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DoTweenManager : MonoBehaviour
{
    public List<DoTweenSettings> settings;
    public bool runOnAwake;

    private void Awake()
    {
        ExcecuteTweening();
    }

    private void ExcecuteTweening()
    {
        foreach (var setting in settings)
        {
            switch (setting.tweenType)
            {
                case DoTweenSettings.TweenType.Move:
                    transform.DOMove(setting.targetMovement, setting.transitionDuration_Movement)
                        .SetEase(setting.transitionType_Movement);
                    break;
                case DoTweenSettings.TweenType.Rescale:
                    transform.DOScale(setting.targetScale, setting.transitionDuration_Scale)
                        .SetEase(setting.transitionType_Scale);
                    break;
                case DoTweenSettings.TweenType.Rotate:
                    transform.DORotateQuaternion(setting.targetRotation, setting.transitionDuration_Rotation)
                        .SetEase(setting.transitionType_Rotation);
                    break;
            }
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
}