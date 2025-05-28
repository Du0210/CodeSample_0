using Spine.Unity;
using System;
using UnityEngine;

public class SpineAnimator
{
    [SerializeField] private SkeletonAnimation _skeletonAnimation;

    private string _currentAnim;

    public void SetSkeletonAnimation(SkeletonAnimation skeletonAnimation)
    {
        _skeletonAnimation = skeletonAnimation;
        _currentAnim = string.Empty;
    }

    public void Play<T>(T state, bool loop = true) where T : Enum
    {
        string animName = state.ToString();
        if (_currentAnim == animName) return;

        _currentAnim = animName;

        _skeletonAnimation.AnimationState.SetAnimation(0, animName, loop);
    }

    public void PlayOnce<T>(T state) where T : Enum
    {
        string animName = state.ToString();
        _skeletonAnimation.AnimationState.SetAnimation(0, animName, false);
        _skeletonAnimation.AnimationState.AddAnimation(0, _currentAnim, true, 0f);
    }
}