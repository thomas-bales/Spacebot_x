using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] float animationTime = 0.1f;
    [SerializeField] float camShakeIntensity = 5f;
    [SerializeField] float camShakeTime = 0.1f;
    [SerializeField] Collider2D _collider;
    [SerializeField] Enemy enemy;
    [SerializeField] AudioClipSO deathSound;

    private void OnEnable()
    {
        EventManager.OnDamageEnemy += HandleOnDamageEnemy;
    }

    private void OnDisable()
    {
        EventManager.OnDamageEnemy -= HandleOnDamageEnemy;
    }

    void HandleOnDamageEnemy(Transform _enemy)
    {
        if (_enemy == transform)
        {
            if (!enemy.canCarryItem)
            {
                EventManager.RaiseOnEnemyDroppedCarriedObject(enemy.carriedItem);
            }

            DestroyEnemy();
        }
    }

    void DestroyEnemy()
    {
        EventManager.RaiseOnEnemyDied(transform);
        CameraShake.Instance.ShakeCamera(camShakeIntensity, camShakeTime);
        StartCoroutine(co_DestroyEnemy());
    }

    IEnumerator co_DestroyEnemy()
    {
        _collider.enabled = false;

        //play animation

        AudioManager.instance.PlayAudioClip(deathSound);

        yield return new WaitForSeconds(animationTime);

        Destroy(gameObject);
    }
}
