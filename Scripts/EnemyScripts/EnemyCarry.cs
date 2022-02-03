using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCarry : MonoBehaviour
{
    [SerializeField] Enemy enemy;
    [SerializeField] AudioClipSO carrySound;

    private void OnEnable()
    {
        EventManager.OnEnemyPickedUpItem += HandleOnEnemyPickedUpItem;
        EventManager.OnEnemyDroppedCarriedObject += HandleOnEnemyDroppedCarriedObject;
    }

    private void OnDisable()
    {
        EventManager.OnEnemyPickedUpItem -= HandleOnEnemyPickedUpItem;
        EventManager.OnEnemyDroppedCarriedObject -= HandleOnEnemyDroppedCarriedObject;
    }

    void HandleOnEnemyPickedUpItem(Transform _enemy, Transform item)
    {
        if (_enemy == transform)
        {
            enemy.carriedItem = item;
            enemy.canCarryItem = false;
            AudioManager.instance.PlayAudioClip(carrySound);
        }
    }

    void HandleOnEnemyDroppedCarriedObject(Transform _enemy)
    {
        if (_enemy == transform)
        {
            enemy.carriedItem = null;
            enemy.canCarryItem = true;
        }
    }
}
