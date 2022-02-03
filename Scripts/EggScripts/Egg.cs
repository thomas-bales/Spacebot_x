using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    [SerializeField] float gravity = 1f;
    [SerializeField] float isGroundedOffset = 0.1f;
    [SerializeField] float eggBounceForce = 1;
    [SerializeField] float camShakeIntensity = 5f;
    [SerializeField] float camShakeTime = 0.1f;
    [SerializeField] Carryable carryable;
    [SerializeField] Throwable throwable;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] CircleCollider2D circleCollider;
    [SerializeField] AudioClipSO spawnSound;
    [SerializeField] ParticleSystem spawnParticles;
    [SerializeField] ParticleSystem landParticles;
    bool playerIsCloseEnoughToPickMeUp;
    bool isBeingThrown;
    bool isDroppedFromSpawn;

    private void OnEnable()
    {
        EventManager.OnPlayerTriedToPickSomethingUp += HandleOnPlayerTriedToPickSomethingUp;
        EventManager.OnPlayerThrewCarriedObject += HandleOnPlayerThrewCarriedObject;
        EventManager.OnEnemyDroppedCarriedObject += HandleOnEnemyDroppedCarriedObject;
        EventManager.OnSpawnObject += HandleOnSpawnObject;
    }

    private void OnDisable()
    {
        EventManager.OnPlayerTriedToPickSomethingUp -= HandleOnPlayerTriedToPickSomethingUp;
        EventManager.OnPlayerThrewCarriedObject -= HandleOnPlayerThrewCarriedObject;
        EventManager.OnEnemyDroppedCarriedObject -= HandleOnEnemyDroppedCarriedObject;
        EventManager.OnSpawnObject -= HandleOnSpawnObject;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isBeingThrown)
        {
            if (collision.gameObject.CompareTag("Wall"))
            {
                if (CheckIsGrounded())
                {
                    MakeObjectCarryable();
                }
                else
                {
                    rb.velocity = Vector2.zero;
                }
            }
            else if (collision.gameObject.CompareTag("Enemy") && !isDroppedFromSpawn)
            {
                EventManager.RaiseOnDamageEnemy(collision.transform);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!carryable.IsCarried && !isBeingThrown)
        {
            if (collision.CompareTag("Player"))
            {
                if (collision.GetComponent<Player>().canCarryItem)
                {
                    playerIsCloseEnoughToPickMeUp = true;
                }
            }
            else if (collision.CompareTag("Enemy"))
            {
                if (collision.GetComponent<Enemy>().canCarryItem)
                {
                    carryable.Carry(collision.transform);
                    EventManager.RaiseOnEnemyPickedUpItem(collision.transform, transform);
                }
            }
        }
        else if (collision.CompareTag("Egg") && !carryable.IsCarried)
        {
            Egg other_egg = collision.GetComponent<Egg>();
            if (!other_egg.carryable.IsCarried)
            {
                int _sign = (int)Mathf.Sign(Random.Range(-1, 1));
                collision.GetComponent<Rigidbody2D>().velocity = (eggBounceForce * Vector2.up + Vector2.right * _sign * eggBounceForce);
                other_egg.BounceOffEgg();
            }
        }
        else if (collision.CompareTag("Wall") && isBeingThrown)
        {
            if (CheckIsGrounded())
            {
                MakeObjectCarryable();
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsCloseEnoughToPickMeUp = false;
        }
    }

    private void Start()
    {
        AudioManager.instance.PlayAudioClip(spawnSound);
        spawnParticles.Play();
    }

    void HandleOnPlayerTriedToPickSomethingUp(Transform player)
    {
        if (playerIsCloseEnoughToPickMeUp)
        {
            carryable.Carry(player);
            EventManager.RaiseOnPlayerPickedUpItem(transform);
            playerIsCloseEnoughToPickMeUp = false;
        }
    }

    void HandleOnPlayerThrewCarriedObject(Transform player, Vector2 throwForce)
    {
        if (carryable.Carrier && carryable.Carrier.name == player.name)
        {
            PrepareToThrowObject();
            throwable.ThrowObject(throwForce);
        }
    }

    void HandleOnEnemyDroppedCarriedObject(Transform item)
    {
        if (item == transform)
        {
            PrepareToThrowObject();
        }
    }

    void HandleOnSpawnObject(Transform _object, Vector2 unneeded)
    {
        if (_object == transform)
        {
            DropFromSpawn();
        }
    }

    void PrepareToThrowObject()
    {
        isBeingThrown = true;
        carryable.Drop();
        circleCollider.isTrigger = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = gravity;
    }

    void DropFromSpawn()
    {
        isDroppedFromSpawn = true;
        isBeingThrown = true;
        carryable.Drop();
        circleCollider.isTrigger = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = gravity;
    }

    public void BounceOffEgg()
    {
        isBeingThrown = true;
        carryable.Drop();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = gravity;
    }

    void MakeObjectCarryable()
    {
        CameraShake.Instance.ShakeCamera(camShakeIntensity, camShakeTime);
        landParticles.Play();
        isBeingThrown = false;
        isDroppedFromSpawn = false;
        circleCollider.isTrigger = true;
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

        //swap sprite to carryable sprite
    }

    bool CheckIsGrounded()
    {
        LayerMask groundMask = LayerMask.GetMask("Wall");
        RaycastHit2D isGroundedHit = Physics2D.Raycast(transform.position, Vector2.down, isGroundedOffset, groundMask);

        if (isGroundedHit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
