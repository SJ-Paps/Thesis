using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SAM.Timers;

public class TurretShootingState : TurretHSMState
{
    private GameObject[] bulletPool;

    private GameObject nextBullet;

    private SyncTimer timerToFinish;

    private bool isFirstUpdate;

    //utilizo un bool para saber cuando es el segundo para dar tiempo a posicionar la bala en la punta del cañon en el OnEnter
    //y luego la posiciono en el punto final del LineCast en el segundo update (en el Shoot)
    private bool isSecondUpdate; 

    public TurretShootingState(byte stateId, string debugName = null) : base(stateId, debugName)
    {
        int poolSize = 5;
        bulletPool = new GameObject[poolSize];

        timerToFinish = new SyncTimer();
        timerToFinish.Interval = 0.5f;
        timerToFinish.onTick += OnTimerTick;

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
            GameObject bullet = GameObject.Instantiate(Owner.BulletPrefab);

            bulletPool[i] = bullet;
            bullet.SetActive(false);
        }
    }

    protected override void OnEnter()
    {
        base.OnEnter();
        
        nextBullet = GetNext();
        nextBullet.SetActive(true);
        nextBullet.transform.position = Owner.GunPoint.position;

        isFirstUpdate = true;

        timerToFinish.Start();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if(isFirstUpdate)
        {
            isFirstUpdate = false;
            isSecondUpdate = true;
        }
        else if(isSecondUpdate)
        {
            Shoot();
            isSecondUpdate = false;
        }

        timerToFinish.Update(Time.deltaTime);
    }

    protected override void OnExit()
    {
        base.OnExit();

        if (nextBullet != null)
        {
            nextBullet.SetActive(false);
        }

        nextBullet = null;

        timerToFinish.Stop();
    }

    private void OnTimerTick(SyncTimer timer)
    {
        SendEvent(Character.Order.StopAttacking);
    }

    private void Shoot()
    {
        if(nextBullet != null)
        {
            Vector2 gunPointPosition = Owner.GunPoint.position;

            Vector2 endPoint = gunPointPosition + (Vector2)(Owner.GunPoint.up * Owner.ShootDistance);

            RaycastHit2D hit = Physics2D.Linecast(gunPointPosition, endPoint, Physics2D.GetLayerCollisionMask(nextBullet.layer));

            if (hit)
            {
                nextBullet.transform.position = hit.point;

                IDamagable damagable = hit.collider.GetComponent<IDamagable>();

                if (damagable != null)
                {
                    damagable.TakeDamage(Owner.ShootDamage.CurrentValueFloat, DamageType.Bullet);
                }

                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForceAtPosition(Owner.GunPoint.up * 3, hit.point, ForceMode2D.Impulse);
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