using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    [Header("CharacterActions")]
    [SerializeField] InputActionReference move;
    [SerializeField] InputActionReference attack;
    [SerializeField] InputActionReference dash;
    [SerializeField] InputActionReference showInventory;
    [SerializeField] InputActionReference interact;
    [SerializeField] InputActionReference changeWeapon;
    [SerializeField] GameObject canvasInventoryPanel;
    CharacterController2D characterController;

    [Header("BlinkAfterHit")]
    [SerializeField] private float blinkingSecondsInterval = 0.15f;
    private SpriteRenderer spriteRenderer;
    private Life life;

    [SerializeField] float timeBetweenAttacks = 1f;

    private bool interactPressed = false;
    private bool canAttack = true;

    private PlayerWeaponManager weaponManager;

    private void Awake()
    {
        this.characterController = GetComponent<CharacterController2D>();
        this.weaponManager = GetComponent<PlayerWeaponManager>();

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
        attack.action.started += OnAttack;

        dash.action.Enable();
        dash.action.started += OnDash;

        showInventory.action.Enable();
        showInventory.action.started += OnPressInventoryButton;

        interact.action.Enable();
        interact.action.started += OnInteract;

        changeWeapon.action.Enable();
        changeWeapon.action.started += OnChangeWeapon;

        //El jugador escuchará los eventos OnLifeChanged (cuando la barra de vida aumenta o dismunuye)
        //y OnLifeDepleted (cuando la barra de vida llega a su fin)
        this.life.onLifeChanged.AddListener(OnLifeChanged);
        this.life.onLifeDepleted.AddListener(OnLifeDepleted);
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        if (!canAttack) return;

        this.characterController.Attack();

        StartCoroutine(AttackCooldown());
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        characterController.Dash();
    }

    private void OnChangeWeapon(InputAction.CallbackContext context)
    {
        weaponManager.ChangeWeapon();
    }

    private void OnPressInventoryButton(InputAction.CallbackContext context)
    {
        if (this.canvasInventoryPanel.activeSelf == false)
        {
            this.canvasInventoryPanel.SetActive(true);
        }
        else
        {
            this.canvasInventoryPanel.SetActive(false);
        }
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        interactPressed = true;
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

    private IEnumerator AttackCooldown()
    {
        canAttack = false;

        yield return new WaitForSeconds(timeBetweenAttacks);

        canAttack = true;
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
        attack.action.started -= OnAttack;

        dash.action.Disable();
        dash.action.started -= OnDash;

        showInventory.action.Disable();
        showInventory.action.started -= OnPressInventoryButton;

        interact.action.Disable();
        interact.action.started -= OnInteract;

        changeWeapon.action.Disable();
        changeWeapon.action.started -= OnChangeWeapon;

        //El jugador escuchará los eventos OnLifeChanged (cuando la barra de vida aumenta o dismunuye)
        //y OnLifeDepleted (cuando la barra de vida llega a su fin)
        this.life.onLifeChanged.RemoveListener(OnLifeChanged);
        this.life.onLifeDepleted.RemoveListener(OnLifeDepleted);

    }

    public bool ConsumeInteract()
    {
        if (interactPressed == false)
        {
            return false;
        }

        interactPressed = false;
        return true;
    }
}
