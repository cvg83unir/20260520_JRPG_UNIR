using System;
using UnityEngine;

public class InstantiateAttack : MonoBehaviour
{
    private CharacterController2D characterController;
    private Enemy enemyIA;
    private EnemyShooterPatrol enemyIAShooterPatrol;
    private float timeSinceLastShot = 0f;
    private float timeBetweenAttacks = 0f;
    private bool bFirstShoot = true;

    private void Awake()
    {
        this.characterController = GetComponentInParent<CharacterController2D>();
        this.enemyIA = GetComponentInParent<Enemy>();
        this.enemyIAShooterPatrol = GetComponentInParent<EnemyShooterPatrol>();
    }

    private void Start()
    {
        if (this.enemyIA != null)
        {
            this.timeBetweenAttacks = this.enemyIA.PropTimeBetweenAttacks;
        }
        else if (this.enemyIAShooterPatrol != null)
        {
            this.timeBetweenAttacks = this.enemyIAShooterPatrol.PropTimeBetweenAttacks;
        }
    }

    private void Update()
    {
        this.timeSinceLastShot += Time.deltaTime;

        //Para que no se desborde la variable this.timeSinceLastShot, la igualamos a timeBetweenAttacks si ya vale más
        //que el tiempo entre disparos
        if (this.timeSinceLastShot >= this.timeBetweenAttacks)
        {
            this.timeSinceLastShot = this.timeBetweenAttacks;
        }

    }

    internal void Shoot()
    {

        //Debug.Log("Instantiate Attact. timeSinceLastShot " + this.timeSinceLastShot + " timeBetweenAttacks: " + this.timeBetweenAttacks + " enemyIA.CanInstantiateShoot : " + this.enemyIA.CanInstantiateShoot + " bFirstShoot: " + this.bFirstShoot);
        bool bIACaninstantiate = true;

        if (this.enemyIA!=null)
        {
            bIACaninstantiate = this.enemyIA.CanInstantiateShoot;
        }

        if ((bIACaninstantiate && this.timeSinceLastShot >= this.timeBetweenAttacks) || this.bFirstShoot == true)
        {
            this.bFirstShoot = false;
            this.timeSinceLastShot = 0;
            //Debug.Log("LLAMADA A CharacterController?.ShootOnAttackAnimation(); : " + DateTime.Now.ToString("yyyyMMddHHmmss"));
            this.characterController?.ShootOnAttackAnimation();
        }
    }

}
