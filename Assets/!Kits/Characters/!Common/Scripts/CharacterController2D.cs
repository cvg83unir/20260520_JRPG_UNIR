using System;
using UnityEditor.Experimental.GraphView;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class CharacterController2D : MonoBehaviour, IVisible
{
    [SerializeField] float movementSpeed = 3f;

    Rigidbody2D rb2D;
    Animator animator;

    [SerializeField] IVisible.Side side = IVisible.Side.Neutral;

    private bool walking;
    [SerializeField] GameObject prefabAttack;

    //Dash
    [SerializeField] float dashSpeed = 20f;
    [SerializeField] float dashDuration = 0.15f;
    [SerializeField] float dashCooldown = 2f;

    [SerializeField] private Transform shootAttackPoint;

    bool isDashing = false;
    bool canDash = true;

    [SerializeField] Collider2D playerCollider;
    [SerializeField] HurtCollider hurtCollider;

    // Ataque espada
    [SerializeField] bool useSwordAttack;
    [SerializeField] GameObject swordAttackPrefab;
    [SerializeField] Transform swordAttackPoint;

    private PlayerWeaponManager weaponManager;

    private void Awake()
    {
        this.rb2D = GetComponent<Rigidbody2D>();
        this.animator = GetComponentInChildren<Animator>();
        this.weaponManager = GetComponent<PlayerWeaponManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDashing)
        {
            this.rb2D.linearVelocity = rawMove * movementSpeed;
        }
    }

    private Vector2 rawMove = Vector2.zero;
    private Vector2 previousRawMove = Vector2.zero;

    /// <summary>
    /// Propiedad de sólo lectura para que otros objetos sepan si estamos en tiempo de recuperación
    /// </summary>
    public Vector2 PreviousRawMove
    {
        get => this.previousRawMove;
    }

    public void SetRawMove(Vector2 rawMove)
    {
        this.rawMove = rawMove;

        if (this.rawMove.x != 0f || this.rawMove.y != 0f) 
        {
            this.walking = true;
            this.animator.SetFloat("HorizontalVelocity", this.rawMove.x);
            this.animator.SetFloat("VerticalVelocity", this.rawMove.y);
        }
        else 
        {
            this.walking = false;
            this.animator.SetFloat("HorizontalVelocity", this.previousRawMove.x);
            this.animator.SetFloat("VerticalVelocity", this.previousRawMove.y);

        }

        this.animator.SetBool("Walking", this.walking);

        if (this.rawMove != Vector2.zero)
            { this.previousRawMove = this.rawMove.normalized; }
        this.animator.SetFloat("AnimLastMoveX", this.previousRawMove.x);
        this.animator.SetFloat("AnimLastMoveY", this.previousRawMove.y);

    }

    public IVisible.Side GetSide()
    {
        return this.side;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    internal void Attack()
    {
        //Si el gameobject es el player, se hará el ataque directamente en la función attack
        if(this.gameObject.CompareTag("Player"))
        {
            if (weaponManager.GetCurrentWeapon() == WeaponType.None) return;

            this.animator.SetTrigger("Attack");

            if (weaponManager.GetCurrentWeapon() == WeaponType.Sword)
                { SwordOnAttackAnimation(); }
            else if (weaponManager.GetCurrentWeapon() == WeaponType.MagicProjectile)
                { ShootOnAttackAnimation(); }

            return;
        }

        this.animator.SetTrigger("Attack");
    }


    internal void ShootOnAttackAnimation()
    {
        if (this.gameObject.CompareTag("Player"))
        {
            GameObject ataque = Instantiate(this.prefabAttack, shootAttackPoint.position, Quaternion.identity);

            ProjectileController projectile = ataque.GetComponent<ProjectileController>();
            
            if (projectile != null)
                { projectile.SetDirection(this.previousRawMove); }
        }

        if (this.gameObject.tag.StartsWith("EnemyShooter"))
        {
            Debug.Log("Instanciar Disparo Enemigo");
            GameObject ataque = Instantiate(this.prefabAttack, transform.position, transform.rotation);

            ataque.GetComponent<EnemyShoot>().setShotDirection(previousRawMove);
            
        }
    }

    internal void SwordOnAttackAnimation()
    {
        if (this.gameObject.CompareTag("Player"))
        {
            Vector2 attackDirection = previousRawMove.normalized;

            if (attackDirection == Vector2.zero)
            {
                attackDirection = Vector2.down;
            }

            Vector3 offset = new Vector3(attackDirection.x, attackDirection.y, 0f);

            GameObject sword = Instantiate(
                swordAttackPrefab,
                transform.position + offset,
                Quaternion.identity
            );

            SwordMovement swordMovement = sword.GetComponent<SwordMovement>();

            if (swordMovement != null)
            {
                swordMovement.SetTarget(transform, offset);
            }

            Collider2D swordCollider = sword.GetComponent<Collider2D>();
            Collider2D playerCollider = GetComponent<Collider2D>();

            if (swordCollider != null && playerCollider != null)
            {
                Physics2D.IgnoreCollision(swordCollider, playerCollider, true);
            }

            Animator swordAnimator = sword.GetComponent<Animator>();

            if (swordAnimator != null)
            {
                swordAnimator.SetFloat("HorizontalVelocity", attackDirection.x);
                swordAnimator.SetFloat("VerticalVelocity", attackDirection.y);
                swordAnimator.SetTrigger("SwordAttack");
            }

            Destroy(sword, 1f);
        }
    }


    internal void Dash()
    {
        Debug.Log("DASH ACTIVADO");

        if (!canDash) return;
        if (isDashing) return;

        Vector2 dashDirection = rawMove;

        if (dashDirection == Vector2.zero)
            dashDirection = previousRawMove;

        if (dashDirection == Vector2.zero) return;

        hurtCollider.enabled = false;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            Collider2D enemyCollider = enemy.GetComponent<Collider2D>();

            if (enemyCollider != null)
                Physics2D.IgnoreCollision(playerCollider, enemyCollider, true);
        }

        canDash = false;
        isDashing = true;

        dashDirection = dashDirection.normalized;
        rb2D.linearVelocity = dashDirection * dashSpeed;

        animator.SetFloat("HorizontalVelocity", dashDirection.x);
        animator.SetFloat("VerticalVelocity", dashDirection.y);
        animator.SetTrigger("Dash");

        Invoke(nameof(ResetDash), dashDuration);
        Invoke(nameof(ResetDashCooldown), dashCooldown);
    }

    void ResetDash()
    {
        hurtCollider.enabled = true;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            Collider2D enemyCollider = enemy.GetComponent<Collider2D>();

            if (enemyCollider != null)
                Physics2D.IgnoreCollision(playerCollider, enemyCollider, false);
        }

        isDashing = false;
    }

    void ResetDashCooldown()
    {
        canDash = true;
    }

}
