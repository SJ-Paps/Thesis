using SAM.FSM;
using System;
using UnityEngine;

[Serializable]
public class TurretShootSubstate : TurretAttackSubstate
{
    [SerializeField]
    private float shootDistance;

    private int targetLayers;

    private DeadlyType deadly;

    [SerializeField]
    private Transform gunPoint;

    [SerializeField]
    private GameObject bulletPrefab;

    private GameObject[] bullets;

    private GameObject nextBullet;

    public override void InitializeState(FSM<TurretAttackState.State, TurretAttackState.Trigger> stateMachine, TurretAttackState.State state, Character character, Character.Blackboard blackboard)
    {
        base.InitializeState(stateMachine, state, character, blackboard);

        deadly = DeadlyType.Bullet;
        targetLayers = (1 << Reg.playerLayer) | (1 << Reg.floorLayer) | (1 << Reg.wallLayer) | (1 << Reg.objectLayer);

        bullets = new GameObject[2];

        for(int i = 0; i < bullets.Length; i++)
        {
            bullets[i] = GameObject.Instantiate(bulletPrefab);
            bullets[i].SetActive(false);
            bullets[i].transform.position = gunPoint.position;
        }
    }

    protected override void OnEnter()
    {
        nextBullet = GetNext();

        nextBullet.SetActive(true);
        nextBullet.transform.position = gunPoint.position;

        for(int i = 0; i < bullets.Length; i++)
        {
            if(bullets[i] != nextBullet)
            {
                bullets[i].SetActive(false);
            }
        }
    }

    protected override void OnUpdate()
    {
        Shoot();
        stateMachine.Trigger(TurretAttackState.Trigger.GoNext);
    }

    private void Shoot()
    {
        RaycastHit2D hit = Physics2D.Raycast(gunPoint.position, character.transform.up, shootDistance, targetLayers);

        

        if (hit.collider != null)
        {
            nextBullet.transform.position = hit.point;

            IMortal mortal = hit.collider.GetComponent<IMortal>();

            if (mortal != null)
            {
                mortal.Die(deadly);
            }
        }
        else
        {
            nextBullet.transform.position = gunPoint.position + (character.transform.up * shootDistance);
        }
    }

    private GameObject GetNext()
    {
        foreach(GameObject bullet in bullets)
        {
            if(!bullet.activeSelf)
            {
                return bullet;
            }
        }

        return null;
    }
}
