/**
 * Copyright (c) 2023-present Burak Hatay. All rights reserved.
 */

using UnityEngine;
using System.Threading.Tasks;

public delegate void ActionEvent();

[SerializeField]
public class ObjectAnimator : MonoBehaviour
{
    #region Parametars

    public event ActionEvent ActionEvent;

    [SerializeField] public bool isAnimated = false;


    #region Rotating

    [Header("Rotating")] public bool isRotating = false;
    public Vector3 rotationAngle;
    public float rotationSpeed;

    public enum SpeedSelect
    {
        Random,
        Specific
    }

    public SpeedSelect speedSelect;
    public float randomRotationSpeedOne, randomRotationSpeedTwo;

    #endregion

    #region Shake

    public bool isShake = false;
    [SerializeField] private float shakeDuration = 1.0f;
    [SerializeField] private float resetShakeDuration;

    [SerializeField] private AnimationCurve shakeCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);
    [SerializeField] private Vector3 nextPos, nextRot, lastRot, lastPos;

    [SerializeField] private Vector3 shakePosition = new Vector3(1f, 1f, 1f);
    [SerializeField] private Vector3 shakeRotation = new Vector3(1f, 1f, 1f);

    [SerializeField] private float shakeSpeed = 2f;

    #endregion

    #region Ping-Pong

    [Header("Ping-Pong")] public bool isPingPongPosition;
    public bool active;
    [SerializeField] private float pingPongSpeed = 2f;
    [HideInInspector] [SerializeField] private Vector3 pingPongStartPos;
    [HideInInspector] [SerializeField] private Vector3 pingPongEndPos;

    #endregion

    #endregion

    private void Start()
    {
        pingPongStartPos = transform.localPosition;
        StartActionInits();
        StartParametersInit();
    }


    #region Public Functions

    public void StopAnimating()
    {
        isAnimated = false;
    }

    public void ActivateAnimating()
    {
        isAnimated = true;
    }

    public void KillAnimating()
    {
        isAnimated = false;
        ActionEvent = null;
    }

    #endregion

    #region Initiations

    private void StartActionInits()
    {
        InitRotation(isRotating);
        InitPingPongPosition(isPingPongPosition);
        InitShake(isShake);
    }

    private void StartParametersInit()
    {
        if (isRotating)
        {
            if (speedSelect == SpeedSelect.Random)
                rotationSpeed = Random.Range(randomRotationSpeedOne, randomRotationSpeedTwo);
        }
    }

    public void InitAnimated(bool value)
    {
        enabled = value;
        isAnimated = value;
    }

    #endregion

    #region Rotating

    public void InitRotation(bool value)
    {
        if (value)
            ActionEvent += Rotating;
        else
            ActionEvent -= Rotating;

        isRotating = value;
    }

    private void Rotating()
    {
        transform.Rotate(rotationAngle * (rotationSpeed * Time.deltaTime));
    }

    #endregion

    #region PingPongPosition

    private void PingPongPosition()
    {
        transform.localPosition = new Vector3(
            (Mathf.Lerp(
                pingPongStartPos.x,
                pingPongEndPos.x,
                Mathf.PingPong(Time.time * pingPongSpeed, 1))),
            (Mathf.Lerp(
                pingPongStartPos.y,
                pingPongEndPos.y,
                Mathf.PingPong(Time.time * pingPongSpeed, 1))),
            (Mathf.Lerp(
                pingPongStartPos.z,
                pingPongEndPos.z,
                Mathf.PingPong(Time.time * pingPongSpeed, 1))));
    }

    public void InitPingPongPosition(bool value)
    {
        if (value)
            ActionEvent += PingPongPosition;
        else
            ActionEvent -= PingPongPosition;

        isPingPongPosition = value;
    }

    public void SetPingPongStartPos() => pingPongStartPos = transform.localPosition;
    public void SetPingPongEndPos() => pingPongEndPos = transform.localPosition;

    #endregion

    #region Shake

    public async Task Shake()
    {
        float shakeLerpEndValue = shakeDuration;

        while (shakeLerpEndValue > 0)
        {
            nextPos = (Mathf.PerlinNoise(shakeLerpEndValue * shakeSpeed, shakeLerpEndValue * shakeSpeed * 2) - 0.5f) *
                      shakePosition.x * transform.right *
                      shakeCurve.Evaluate(1f - shakeLerpEndValue) +
                      (Mathf.PerlinNoise(shakeLerpEndValue * shakeSpeed * 2, shakeLerpEndValue * shakeSpeed) - 0.5f) *
                      shakePosition.y * transform.up *
                      shakeCurve.Evaluate(1f - shakeLerpEndValue) +
                      (Mathf.PerlinNoise(shakeLerpEndValue * shakeSpeed * 2, shakeLerpEndValue * shakeSpeed) - 0.5f) *
                      shakePosition.z * transform.forward *
                      shakeCurve.Evaluate(1f - shakeLerpEndValue);

            nextRot = (Mathf.PerlinNoise(shakeLerpEndValue * shakeSpeed, shakeLerpEndValue * shakeSpeed * 2) - 0.5f) *
                      shakeRotation.x * transform.right *
                      shakeCurve.Evaluate(1f - shakeLerpEndValue) +
                      (Mathf.PerlinNoise(shakeLerpEndValue * shakeSpeed * 2, shakeLerpEndValue * shakeSpeed) - 0.5f) *
                      shakeRotation.y * transform.up *
                      shakeCurve.Evaluate(1f - shakeLerpEndValue) +
                      (Mathf.PerlinNoise(shakeLerpEndValue * shakeSpeed * 2, shakeLerpEndValue * shakeSpeed) - 0.5f) *
                      shakeRotation.z * transform.forward *
                      shakeCurve.Evaluate(1f - shakeLerpEndValue);


            transform.Rotate(true ? (nextRot - lastRot) : nextPos);

            transform.Translate(true ? (nextPos - lastPos) : nextPos);

            lastPos = nextPos;
            lastRot = nextRot;

            shakeLerpEndValue -= Time.deltaTime;

            await Task.Yield();
        }

        shakeLerpEndValue = shakeDuration;
    }

    public void ShakeNow()
    {
        Shake();
    }

    public void InitShake(bool value)
    {
        // if (value)
        //     ActionEvent += Shake;
        // else
        //     ActionEvent -= Shake;

        isShake = value;
    }

    #endregion

    private void Update() => ActionEvent?.Invoke();
}