using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretShootingState : TurretHSMState
{
    private GameObject[] bulletPool;

    private GameObject nextBullet;

    public TurretShootingState(Character.State stateId, string debugName = null) : base(stateId, debugName)
    {
        int poolSize = 5;
        bulletPool = new GameObject[poolSize];

        activeDebug = true;
    }

    protected override void OnOwnerReferencePropagated()
    {
        base.OnOwnerReferencePropagated();

        InitializePool();
    }

    private void InitializePool()
    {
        for(int i = 0; i < bulletPool.Length; i++)
        {
            GameObject bullet = GameObject.Instantiate(character.BulletPrefab);

            bulletPool[i] = bullet;
            bullet.SetActive(false);
        }
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        if(nextBullet != null)
        {
            nextBullet.SetActive(false);
        }
        
        nextBullet = GetNext();
        nextBullet.SetActive(true);
        nextBullet.transform.position = character.GunPoint.position;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        Shoot();
        SendEvent(Character.Trigger.StopAttacking);
    }

    protected override void OnExit()
    {
        base.OnExit();
    }

    private void Shoot()
    {
        if(nextBullet != null)
        {
            Vector2 gunPointPosition = character.GunPoint.position;

            Vector2 endPoint = gunPointPosition + (Vector2)(character.GunPoint.up * character.ShootDistance);

            RaycastHit2D hit = Physics2D.Linecast(gunPointPosition, endPoint, Physics2D.GetLayerCollisionMask(nextBullet.layer));

            if (hit)
            {
                nextBullet.transform.position = hit.point;

                IDamagable damagable = hit.collider.GetComponent<IDamagable>();

                if (damagable != null)
                {
                    damagable.TakeDamage(character.ShootDamage.CurrentValueFloat, DamageType.Bullet);
                }

                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForceAtPosition(character.GunPoint.up * 3, hit.point, ForceMode2D.Impulse);
                }
            }
            else
            {
                nextBullet.transform.position = endPoint;
            }
        }
    }

    private GameObject GetNext()
    {
        for(int i = 0; i < bulletPool.Length; i++)
        {
            GameObject current = bulletPool[i];

            if(current.activeSelf == false)
            {
                return current;
            }
        }

        return null;
    }
}