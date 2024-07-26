using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [ColorUsage(true, true)]
    [SerializeField] private Color _flashColor = Color.white;
    [SerializeField] private float flashTime = 0.25f;
    [SerializeField] private AnimationCurve _flashSpeedCurve;
    
    private PlayerMovement _movement;
    
    private SpriteRenderer _spriteRenderer;
    private Material _material;

    private Coroutine _damageFlasherCoroutine;

    [SerializeField] private SafeGroundCheckpointSaver safeGroundCheckpointSaver;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _material = _spriteRenderer.material;
        _movement = _spriteRenderer.GetComponentInParent<PlayerMovement>();
    }
    
    public void CallDamageFlash()
    {
        _damageFlasherCoroutine = StartCoroutine(DamageFlasher());
    }

    private IEnumerator DamageFlasher() //Корутина що відповідає за інтенсивність шейдеру спалаху
    {
        SetFlasheColor(); //Встановлення кольору спалаху

        //Зміна статусу фізичного тіла гравця з динамічного на статичне для моментальної
        //зупинки падіння та вірного програвання спалаху перед переміщенням на чекпоінт.
        //Функція викликається в контроллері персонажа
        _movement.StaticRBType(true);

        float currentFlashAmount = 0f;
        float elapsedTime = 0f;

        //Зміна інтенсивності спалалаху відповідно до таймеру
        while (elapsedTime < flashTime)
        {
            elapsedTime += Time.deltaTime;

            currentFlashAmount = Mathf.Lerp(1f, _flashSpeedCurve.Evaluate(elapsedTime), (elapsedTime / flashTime));
            SetFlashAmount(currentFlashAmount);

            yield return null;
        }
        //Повернення до динамічного статусу фізичного тіла 
        _movement.StaticRBType(false);

        //Виклик функції повернення гравця до чекпоінту
        safeGroundCheckpointSaver.WarpPlayerToSafeGround();
    }
    private void SetFlasheColor()
    {
        _material.SetColor("_FlashColor", _flashColor);
    }
    private void SetFlashAmount(float amount)
    {
        _material.SetFloat("_FlashAmount", amount);
    }

}
