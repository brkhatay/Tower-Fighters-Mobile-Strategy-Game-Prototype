using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public static class Utils
{
    public static string ClearInstanceName(string targetName)
    {
        string clearedName = targetName.Replace(" (Instance)", "");
        return clearedName;
    }

    public static async Task ShakeRandom(this Transform target, float shakePower = .2f, float shakeDuration = .2f)
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        float shakeLerpEndValue = shakeDuration;

        var startTransform = target.localPosition;

        while (shakeLerpEndValue > 0)
        {
            if (!target)
            {
                cancellationTokenSource.Cancel();
                break;
            }

            target.transform.localPosition = target.transform.localPosition + Random.insideUnitSphere * shakePower;
            shakeLerpEndValue -= Time.deltaTime;

            await Task.Yield();
        }

        float lerpDuration = 0f, lerpEndValue = 1f;
        while (lerpDuration < lerpEndValue)
        {
            lerpDuration += Time.deltaTime;
            target.localPosition =
                Vector3.Lerp(target.localPosition, startTransform, (lerpDuration / lerpEndValue));
            await Task.Yield();
        }
    }

    #region Position

    public static async Task LerpPosition_Task(this Transform target, Vector3 end, float duration, float delay = 0,
        UnityAction endAction = null)
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        await Task.Delay((int) (delay * 1000));
        float t = 0f;

        while (t < duration)
        {
            if (!target)
                cancellationTokenSource.Cancel();

            t += Time.deltaTime;
            target.position = Vector3.Lerp(target.position, end, t / duration);
            await Task.Yield();
        }

        endAction?.Invoke();
    }

    public static async Task LerpPosition_Local_Task(this Transform target, Vector3 end, float duration,
        float delay = 0)
    {
        await Task.Delay((int) (delay * 1000));
        float t = 0f;
        Vector3 startPosition = target.localPosition;

        while (t < duration)
        {
            t += Time.deltaTime;
            target.localPosition = Vector3.Lerp(startPosition, end, t / duration);
            await Task.Yield();
        }
    }

    #endregion

    #region UI

    public static async void RecAnchorLerpPosX(this RectTransform target, float vector, float duration = 1f,
        bool deltaMode = false, UnityAction action = null)
    {
        float lerpDuration = 0;
        float startPosX = target.anchoredPosition.x;

        while (lerpDuration <= duration)
        {
            await Task.Yield();
            lerpDuration += Time.deltaTime;

            target.anchoredPosition = new Vector2(
                Mathf.Lerp(startPosX, deltaMode ? startPosX - vector : vector, lerpDuration / duration),
                target.anchoredPosition.y
            );

            if (lerpDuration >= duration/2 )
            {
                action?.Invoke();
                action = null;
            }
        }
    }

    #endregion

    #region Parabbola

    public static async Task LerpParabola_Task(this Transform target, Vector3 end, float duration = 1f,
        float height = 1.5f, float delay = 0,
        UnityAction endAction = null)
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        await Task.Delay((int) (delay * 1000));
        float t = 0f;

        Vector3 startPosition = target.position;

        while (t < duration && !cancellationTokenSource.IsCancellationRequested)
        {
            if (!target)
                cancellationTokenSource.Cancel();

            t += Time.deltaTime;
            target.position = Parabola(startPosition, end, height, t / duration);

            target.position = Vector3.Lerp(target.position, end, t / duration);
            await Task.Yield();
        }

        endAction?.Invoke();
    }

    private static Vector3 Parabola(Vector3 start, Vector3 end, float height, float time)
    {
        Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

        var mid = Vector3.Lerp(start, end, time);

        return new Vector3(mid.x, f(time) + Mathf.Lerp(start.y, end.y, time), mid.z);
    }

    #endregion
}