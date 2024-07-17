using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollowCam : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _playerTransform;

    [Header("Flip Ratation Stats")]
    [SerializeField] private float _flipYRotationTime = 0.5f;

    private Coroutine _turnCoroutine;
    private PlayerVisual _playerVisual;
    private bool _isFacingRight;
    [SerializeField] private PlayerMovement _playerMovement;

    private void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        _playerMovement.GetIsFacingRight();
        transform.position = _playerTransform.position;
    }


    public void CallTurn()
    {
        //_turnCoroutine = StartCoroutine(FlipYLerp());
        transform.DORotate(new Vector3(0f, DetermineEndRotation(), 0f), _flipYRotationTime).SetEase(Ease.InOutSine);

    }
    void OnComplete()
    {

        Debug.Log("Анімація завершена!");
        // Додаткові дії після завершення анімації
    }
    private IEnumerator FlipYLerp()
    {
        float startRotation = transform.localEulerAngles.y;
        float endRotationAmount = DetermineEndRotation();
        float yRotation = 0f;
        float elapsedTime = 0f;
        while (elapsedTime < _flipYRotationTime)
        {
            elapsedTime += Time.deltaTime;

            yRotation = Mathf.Lerp(startRotation, endRotationAmount, (elapsedTime / _flipYRotationTime));
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
            yield return null;
        }
        Debug.Log("Yes");
    }
    private float DetermineEndRotation()
    {
        _isFacingRight = !_isFacingRight;
        if (_isFacingRight)
        {
            return 180f;
        }
        else
        {
            return 0f;
        }
    }
}
