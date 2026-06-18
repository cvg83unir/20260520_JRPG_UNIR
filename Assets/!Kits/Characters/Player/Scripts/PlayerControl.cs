using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    [Header("CharacterActions")]
    [SerializeField] InputActionReference move;
    [SerializeField] InputActionReference attack;
    [SerializeField] InputActionReference showInventory;
    [SerializeField] GameObject canvasInventoryPanel;
    CharacterController2D characterController;

    [Header("BlinkAfterHit")]
    [SerializeField] private float blinkingSecondsInterval = 0.15f;
    private SpriteRenderer spriteRenderer;
    private Life life;

    private void Awake()
    {
        this.characterController = GetComponent<CharacterController2D>();

        //Cacheamos el componente de vida y el spriterenderer del personaje
        this.life = GetComponent<Life>();
        //Aunque, en este juego, el spriterender del personaje está en uno de sus gamebojects
        //hijos, por ello debemos buscarlo con GetComponentsInChildren:
        this.spriteRenderer = GetComponentsInChildren<SpriteRenderer>()[0];
    }

    private void OnEnable()
    {
        move.action.Enable();
        //Así cogemos las acciones del personaje sólo cuando hay alguna interacción por parte del jugador
        move.action.started += OnMove;
        move.action.performed += OnMove; //cada vez que cambia el valor de una acción
        move.action.canceled += OnMove; //cada vez que finaliza una acción

        attack.action.Enable();
        showInventory.action.Enable();
        showInventory.action.started += OnPressInventoryButton;

        //El jugador escuchará los eventos OnLifeChanged (cuando la barra de vida aumenta o dismunuye)
        //y OnLifeDepleted (cuando la barra de vida llega a su fin)
        this.life.onLifeChanged.AddListener(OnLifeChanged);
        this.life.onLifeDepleted.AddListener(OnLifeDepleted);
    }

    private void OnPressInventoryButton(InputAction.CallbackContext context)
    {
        if(this.canvasInventoryPanel.activeSelf == false)
        {
            this.canvasInventoryPanel.SetActive(true);
        }
        else 
        {
            this.canvasInventoryPanel.SetActive(false);
        }
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

    private void OnLifeChanged(float currentLife, float startLife)
    {
        if (currentLife > 0f)
        {
            //Comenzamos el parpadeo del personaje:
            StartCoroutine(Blink());
        }
    }

    private IEnumerator Blink()
    {

        while (this.life.Recovering)
        {
            //Si estamos en tiempo de recuperación, hacemos que el personaje parpadee, jugando con el cuarto parámetro del color del
            //personaje, que es la transparencia.
            this.spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
            yield return new WaitForSeconds(this.blinkingSecondsInterval);

            //pasado el tiempo designado para el parpadeo, volvemos el color de nuestro personaje a su intensidad habitual.
            this.spriteRenderer.color = new Color(1f, 1f, 1f, 1.0f);
            yield return new WaitForSeconds(this.blinkingSecondsInterval);

        }
    }

    private void OnLifeDepleted(float startLife)
    {
        //Destruimos el gameobject del enemigo
        Destroy(this.gameObject);
    }

    private void OnDisable()
    {
        move.action.Disable();
        //Así cogemos las acciones del personaje sólo cuando hay alguna interacción por parte del jugador
        move.action.started -= OnMove;
        move.action.canceled -= OnMove;
        move.action.performed -= OnMove;

        attack.action.Disable();
        showInventory.action.Disable();
        showInventory.action.started -= OnPressInventoryButton;

        //El jugador escuchará los eventos OnLifeChanged (cuando la barra de vida aumenta o dismunuye)
        //y OnLifeDepleted (cuando la barra de vida llega a su fin)
        this.life.onLifeChanged.RemoveListener(OnLifeChanged);
        this.life.onLifeDepleted.RemoveListener(OnLifeDepleted);

    }
}
