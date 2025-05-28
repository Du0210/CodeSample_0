using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

public class HPBar : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _hpBarFilled;

    private float _curHpRatio = 1f;
    private float _duration = 0.5f;
    private float _oriHeight = 0.2f;
    private CancellationTokenSource _cts;

    public void SetData(float ratio)
    {
        _curHpRatio = ratio;
        _hpBarFilled.size = new Vector2(_curHpRatio, _oriHeight);
        _cts = new CancellationTokenSource();
    }

    public void DrawHpbar(float targetRatio)
    {
        _cts?.Cancel();
        _cts = new CancellationTokenSource();

        if (targetRatio < 0)
            targetRatio = 0;

        DoAnimation(_curHpRatio, targetRatio, _cts.Token).Forget();
        _curHpRatio = targetRatio;
    }

    private async UniTask DoAnimation(float from, float to, CancellationToken cts)
    {
        float timer = 0f;

        while (timer < _duration)
        {
            if (cts.IsCancellationRequested) return;
            
            timer += Time.deltaTime;
            float ratio = Mathf.Clamp01(timer / _duration);
            float currentRatio = Mathf.Lerp(from, to, ratio);
            _hpBarFilled.size = new Vector2(currentRatio, _oriHeight);

            await UniTask.Yield(PlayerLoopTiming.Update, cts);
        }

        _hpBarFilled.size = new Vector2(to, _oriHeight);
    }

    private void OnDestroy()
    {
        _cts?.Cancel();
    }
}
