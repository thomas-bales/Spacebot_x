using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] AudioClipSO enemyWinSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (!enemy.canCarryItem)
            {
                EventManager.RaiseOnGameOver();
                AudioManager.instance.PlayAudioClip(enemyWinSound);
            }
        }
    }
}
