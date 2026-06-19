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

    private void Awake()
    {
        this.rb2D = GetComponent<Rigidbody2D>();
        this.animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        this.rb2D.linearVelocity = rawMove * movementSpeed;
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

        this.previousRawMove = this.rawMove;
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
        this.animator.SetTrigger("Attack");

        if (this.gameObject.tag.StartsWith("EnemyShooter"))
        {
            GameObject ataque = Instantiate(this.prefabAttack, transform.position, transform.rotation);

            ataque.GetComponent<EnemyShoot>().setShotDirection(previousRawMove);
        }
    }
}
