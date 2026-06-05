using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] InputActionReference move;
    [SerializeField] InputActionReference attack;
    CharacterController2D characterController;

    private void Awake()
    {
        this.characterController = GetComponent<CharacterController2D>();
    }

    private void OnEnable()
    {
        move.action.Enable();
        //Así cogemos las acciones del personaje sólo cuando hay alguna interacción por parte del jugador
        move.action.started += OnMove;
        move.action.performed += OnMove; //cada vez que cambia el valor de una acción
        move.action.canceled += OnMove; //cada vez que finaliza una acción


        attack.action.Enable();
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector2 rawMove = Vector2.zero;
    private void OnMove(InputAction.CallbackContext obj)
    {
        this.rawMove = obj.action.ReadValue<Vector2>();
        this.characterController.SetRawMove(this.rawMove);
    }

    private void OnDisable()
    {
        move.action.Disable();
        //Así cogemos las acciones del personaje sólo cuando hay alguna interacción por parte del jugador
        move.action.started -= OnMove;
        move.action.canceled -= OnMove;
        move.action.performed -= OnMove;

        attack.action.Disable();



    }
}
