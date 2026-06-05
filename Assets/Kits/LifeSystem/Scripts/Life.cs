using System;
using UnityEngine;
using UnityEngine.Events;

public class Life : MonoBehaviour
{
    [Header("LifeParametersAndRecovery")]
    [SerializeField] float startLifePercentage = 100f;
    [SerializeField] float damagePerHitPercentage = 30f;
    [SerializeField] float mediumHeartPercentage = 50f;
    [SerializeField] float fullHeartPercentage = 100;
    [SerializeField] private int lifesNumber = 3;
    public int LifesNumber
    {
        get => this.lifesNumber;
    }
    [SerializeField] private float recoverySecondsLimit = 1.5f;
    public bool recovering = false;
    private float actualRecoveringSeconds = 0f;

    /// <summary>
    /// Propiedad de sólo lectura para que otros objetos sepan si estamos en tiempo de recuperación
    /// </summary>
    public bool Recovering
    {
        get => this.recovering;
    }

    public UnityEvent<float, float> onLifeChanged;
    public UnityEvent<float> onLifeDepleted; //vida agotada
    public UnityEvent onTotallyDead; //Número de vidas a 0
    public UnityEvent onHalfLifeRecoved; //Recuperamos la mitad de vida
    public UnityEvent onFullLifeRecoved; //Recuperamos toda la vida
    HurtCollider hurtCollider;
    CharacterController2D characterController2D;
    private LifeRecovery lifeRecovery;

    private float currentLifePercentage;

    [SerializeField] bool debugReceiveDamage;

    /// <summary>
    /// Sólo se llama desde el inspector, al cargar el script o al cambiar una variable en el editor
    /// </summary>
    private void OnValidate()
    {
        if (this.debugReceiveDamage)
        {
            this.debugReceiveDamage = false;
            OnHitReceive("Enemy");
        }
    }

    private void Awake()
    {
        this.currentLifePercentage = startLifePercentage;

        //Cacheamos el hurtcollider y el liferecovery
        this.hurtCollider = GetComponent<HurtCollider>();
        this.lifeRecovery = GetComponent<LifeRecovery>();

        //Cacheamos el character controller 2D
        this.characterController2D = GetComponent<CharacterController2D>();

    }

    private void OnEnable()
    {
        this.hurtCollider.onHitReceive.AddListener(OnHitReceive);
        this.lifeRecovery.onLifeRecovery.AddListener(OnLifeRecovery);
    }

    private void OnDisable()
    {
        this.hurtCollider.onHitReceive.RemoveListener(OnHitReceive);
        this.lifeRecovery.onLifeRecovery.RemoveListener(OnLifeRecovery);
    }

    private void Update()
    {
        //Temporizador del tiempo del recuperación del jugador:
        if (this.recovering)
        {
            this.actualRecoveringSeconds += Time.deltaTime;
            if (this.actualRecoveringSeconds > this.recoverySecondsLimit)
            {
                this.recovering = false;
                //Debug.Log("Fin RECOVERY");
                this.actualRecoveringSeconds = 0;
            }
        }
        else
        {
            this.actualRecoveringSeconds = 0;
        }
    }

    private void OnHitReceive(string tagName)
    {
        //Si el carácter está en fase de recuperación, no hay que hacerle daño, salvo que sea una caída al vacío
        if (tagName.Equals("InstaKillFall"))
        {
            KillGameObject();
        }
        else
        {
            if (this.recovering == false)
            {
                //Si no estamos en fase de recuperación, distinguimos entre los golpes normales que tienen que quitar una cantidad de vida normal
                //y los golpes de las trampas normales que deben quitar toda la vida directamente:
                if (tagName.Equals("InstaKillTrap"))
                {
                    //Trampa mortal, matamos al personaje directamente:
                    KillGameObject();
                }
                else
                {
                    //Golpe normal, quitamos la cantidad de daño que corresponde:
                    this.currentLifePercentage -= damagePerHitPercentage;

                    if (this.currentLifePercentage > 0)
                    {
                        this.recovering = true;
                        onLifeChanged.Invoke(this.currentLifePercentage, this.startLifePercentage);
                    }
                    else
                    {
                        this.recovering = true;
                        KillGameObject();
                    }
                }
            }
        }
    }

    private void OnLifeRecovery(string tagName)
    {
        //Si el carácter está en fase de recuperación, no hay que hacerle daño, salvo que sea una caída al vacío
        if (tagName.Equals("MediumHeart"))
        {
            this.currentLifePercentage += this.mediumHeartPercentage;
            
        }
        else if (tagName.Equals("FullHeart"))
        {
            this.currentLifePercentage += this.fullHeartPercentage;
            
        }

        //Controlamos que no hayamos superado el 100%
        if (this.currentLifePercentage > 100f)
        {
            this.currentLifePercentage = 100;
        }

        onLifeChanged.Invoke(this.currentLifePercentage, this.startLifePercentage);
    }


    private void KillGameObject()
    {
        this.currentLifePercentage = 0f;
        onLifeChanged.Invoke(this.currentLifePercentage, this.startLifePercentage);
        this.lifesNumber -= 1;

        if (this.lifesNumber <= 0)
        {
            onTotallyDead.Invoke();
        }
        else
        {
            onLifeDepleted.Invoke(this.startLifePercentage);
        }
    }

    internal void Restart()
    {
        this.currentLifePercentage = this.startLifePercentage;
        onLifeChanged.Invoke(this.currentLifePercentage, this.startLifePercentage);
    }
}
