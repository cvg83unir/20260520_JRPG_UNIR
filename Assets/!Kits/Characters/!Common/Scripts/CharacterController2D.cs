using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] float movementSpeed = 3f;

    Rigidbody2D rb2D;
    Animator animator;

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

    Vector2 rawMove = Vector2.zero;
    public void SetRawMove(Vector2 rawMove)
    {
        this.rawMove = rawMove;
        this.animator.SetFloat("HorizontalVelocity", this.rawMove.x);
        this.animator.SetFloat("VerticalVelocity", this.rawMove.y);
    }
}
