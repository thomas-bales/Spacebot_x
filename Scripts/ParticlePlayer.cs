using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePlayer : MonoBehaviour
{
    [SerializeField] ParticleSystem enemyDeathParticles;

    private void OnEnable()
    {
        EventManager.OnEnemyDied += HandleOnEnemyDied;
    }

    private void OnDisable()
    {
        EventManager.OnEnemyDied -= HandleOnEnemyDied;
    }

    void HandleOnEnemyDied(Transform enemy)
    {
        enemyDeathParticles.transform.position = enemy.position;
        enemyDeathParticles.Play();
    }
}
