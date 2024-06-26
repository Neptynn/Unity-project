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

    private void Awake()
    {
        _playerVisual = _playerTransform.gameObject.GetComponent<PlayerVisual>();

    }
    private void Start()
    {
        _isFacingRight = Player.Instance.GetIsFacingRight();
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = _playerTransform.position;
    }

    public void CallTurn()
    {
        //_turnCoroutine = StartCoroutine(FlipYLerp());
        LeanTween.rotateY(gameObject, DetermineEndRotation(), _flipYRotationTime).setEaseInOutSine();
    }
    //private IEnumerator FlipYLerp()
    //{
    //    float startRotation = transform.localEulerAngles.y;
    //    float endRotationAmount = DetermineEndRotation();
    //    float yRotation = 0f;
    //    float elapsedTime = 0f;
    //    while(elapsedTime < _flipYRotationTime)
    //    {
    //        elapsedTime += Time.deltaTime;

    //        yRotation = Mathf.Lerp(startRotation, endRotationAmount, (elapsedTime / _flipYRotationTime));
    //        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
    //        yield return null;
    //    }
    //}
    private float DetermineEndRotation()
    {
        _isFacingRight = !_isFacingRight;
        if(_isFacingRight )
        {
            return 0f;
        }
        else
        {
            return 180f;
        }
    }
}
