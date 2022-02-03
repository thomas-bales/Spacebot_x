using UnityEngine;
using System;

public static class EventManager
{
    public static Action<Transform> OnPlayerTriedToPickSomethingUp;
    public static void RaiseOnPlayerTriedToPickSomethingUp(Transform player)
    {
        OnPlayerTriedToPickSomethingUp?.Invoke(player);
    }

    public static Action<Transform, Vector2> OnPlayerThrewCarriedObject;
    public static void RaiseOnPlayerThrewCarriedObject(Transform player, Vector2 throwForce)
    {
        OnPlayerThrewCarriedObject?.Invoke(player, throwForce);
    }

    public static Action<Transform> OnPlayerPickedUpItem;
    public static void RaiseOnPlayerPickedUpItem(Transform item)
    {
        OnPlayerPickedUpItem?.Invoke(item);
    }

    public static Action<Transform> OnEnemyDroppedCarriedObject;
    public static void RaiseOnEnemyDroppedCarriedObject(Transform item)
    {
        OnEnemyDroppedCarriedObject?.Invoke(item);
    }

    public static Action<Transform, Transform> OnEnemyPickedUpItem;
    public static void RaiseOnEnemyPickedUpItem(Transform enemy, Transform item)
    {
        OnEnemyPickedUpItem?.Invoke(enemy, item);
    }

    public static Action<Transform> OnDamageEnemy;
    public static void RaiseOnDamageEnemy(Transform enemy)
    {
        OnDamageEnemy?.Invoke(enemy);
    }

    public static Action<Transform, Vector2> OnSpawnObject;
    public static void RaiseOnSpawnObject(Transform spawnedObject, Vector2 direction)
    {
        OnSpawnObject?.Invoke(spawnedObject, direction);
    }

    public static Action OnPlayerGotHighScore;
    public static void RaiseOnPlayerGotHighScore()
    {
        OnPlayerGotHighScore?.Invoke();
    }

    public static Action<string> OnAddNewHighScore;
    public static void RaiseOnAddNewHighScore(string _name)
    {
        OnAddNewHighScore?.Invoke(_name);
    }

    public static Action OnGameOver;
    public static void RaiseOnGameOver()
    {
        OnGameOver?.Invoke();
    }

    public static Action OnOpenHighScores;
    public static void RaiseOnOpenHighScores()
    {
        OnOpenHighScores?.Invoke();
    }

    public static Action<Music> OnTriggerAudio;
    public static void RaiseOnTriggerAudio(Music track)
    {
        OnTriggerAudio?.Invoke(track);
    }

    public static Action<Transform> OnEnemyDied;
    public static void RaiseOnEnemyDied(Transform enemy)
    {
        OnEnemyDied?.Invoke(enemy);
    }
}
