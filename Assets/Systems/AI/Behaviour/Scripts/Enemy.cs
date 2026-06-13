using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    enum State
    {
        Guarding,
        Wandering,
        Patrolling,
        Seeking,
        Attacking,
        BeingHit,
        Dead
    };

    //El enemigo tendrá como estado por defecto Guarding
    State currentState = State.Guarding;
    CharacterController2D characterController;

    Sight sight;

    private Life life;

    private void Awake()
    {
        this.sight = GetComponent<Sight>();
        this.characterController = GetComponent<CharacterController2D>();

        //Cacheamos el componente de vida
        this.life = GetComponent<Life>();
    }

    private void OnEnable()
    {
        //De momento los enemigos simplemente escucharán al evento OnLifeDepleted (cuando la barra de vida llega a su fin)
        this.life.onLifeDepleted.AddListener(OnLifeDepleted);
    }

    private void OnDisable()
    {
        //De momento los enemigos simplemente escucharán al evento OnLifeDepleted (cuando la barra de vida llega a su fin)
        this.life.onLifeDepleted.RemoveListener(OnLifeDepleted);
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (this.currentState)
        {
            case State.Guarding:
                //Si veo a alguien, paso al estado de Seeking:
                if (sight.VisiblesInSight.Count > 0)
                {
                    this.currentState = State.Seeking;
                }
                else
                {
                    //Si no veo a alguien, que se deje de mover:
                    this.characterController.SetRawMove(Vector2.zero);
                }
                break;
            case State.Wandering:
                break;
            case State.Patrolling:
                break;
            case State.Seeking:
                //Si no veo a nadie, paso al estado de Guardering o de Wandering
                if (sight.VisiblesInSight.Count <= 0)
                {
                    this.currentState = State.Guarding;
                }
                else
                {
                    //Acercarme:
                    //Obtenemos el vector de la dirección entre la posición donde está el personaje al que va a perseguir el enemigo y el enemigo en sí:
                    //Normalizamos el vector resultante para que no mida más de 1, pues sólo necesitamos la dirección del movimiento:
                    try
                    {
                        Vector2 searchDirection = (sight.VisiblesInSight[0].GetTransform().position - transform.position).normalized;
                        this.characterController.SetRawMove(searchDirection);
                    }
                    catch(Exception)
                    {
                        //Si no pudimos obtener la dirección a seguir (puede que el gameobject ya se haya destruido), volvemos
                        //al estado Guarding:
                        this.currentState = State.Guarding;
                    }
                }
                break;
            case State.Attacking:
                break;
            case State.BeingHit:
                break;
            case State.Dead:
                break;
        }

    }

    private void OnLifeDepleted(float startLife)
    {
        //Destruimos el gameobject del enemigo
        Destroy(this.gameObject);
    }
}
