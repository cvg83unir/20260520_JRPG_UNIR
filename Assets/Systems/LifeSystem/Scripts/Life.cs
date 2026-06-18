using System;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.AdaptivePerformance.Provider.AdaptivePerformanceSubsystemDescriptor;

public class Life : MonoBehaviour
{
    [Header("LifeParametersAndRecovery")]
    [SerializeField] float startLifePercentage = 100f;
    [SerializeField] float damagePerHitPercentage = 30f;
    [SerializeField] private float recoverySecondsLimit = 1.5f;
    private bool recovering = false;
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
    HurtCollider hurtCollider;
    private float currentLifePercentage;

    PlayerCollect playerCollect;
    Inventory inventory;

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

        //Cacheamos el hurtcollider, el playercollect y el inventory
        this.hurtCollider = GetComponent<HurtCollider>();
        this.playerCollect = GetComponent<PlayerCollect>();
        this.inventory = GetComponent<Inventory>(); 

    }

    private void OnEnable()
    {
        this.hurtCollider.onHitReceive.AddListener(OnHitReceive);
        this.playerCollect?.onCollectedObjectDirectUsage.AddListener(OnCollectedObjectDirectUsage);
        this.inventory?.onObjectUsed.AddListener(OnObjectUsed);
    }

    private void OnDisable()
    {
        this.hurtCollider.onHitReceive.RemoveListener(OnHitReceive);
        this.playerCollect?.onCollectedObjectDirectUsage.RemoveListener(OnCollectedObjectDirectUsage);
        this.inventory?.onObjectUsed.RemoveListener(OnObjectUsed);
    }



    private void OnCollectedObjectDirectUsage(CollectableObject collectable)
    {
        InventoryInfo info = collectable.PropInventoryInfo;

        UseInventoryInfo(info);

    }

    private void OnObjectUsed(InventoryInfo info)
    {
        UseInventoryInfo(info);
    }

    private void UseInventoryInfo(InventoryInfo info)
    {
        if (info.type == InventoryInfo.InventoryObjectType.Health)
        {
            //Controlamos que no superemos el 100% de vida
            if (this.currentLifePercentage + info.recovery > 100f)
            {
                this.currentLifePercentage = 100;
            }
            else
            {
                this.currentLifePercentage += info.recovery;
            }

            //Actualizamos la barra de vida:
            onLifeChanged.Invoke(this.currentLifePercentage, this.startLifePercentage);
        }
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


    private void KillGameObject()
    {
        this.currentLifePercentage = 0f;
        onLifeChanged.Invoke(this.currentLifePercentage, this.startLifePercentage);
        onLifeDepleted.Invoke(this.startLifePercentage);
    }
}
