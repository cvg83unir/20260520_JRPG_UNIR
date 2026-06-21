using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyPatrol : MonoBehaviour
{
    private CharacterController2D characterController;
    private Life life;

    //[SerializeField] private float maxXValue = 5;
    //[SerializeField] private float maxYValue = 5;
    [SerializeField] float maxSecondsSameDirection = 2f;
    private float directionSecondsCounter = 0f;
    private Vector2 movementDirection = Vector2.zero;
    private bool changeDirectionDueToCollision = false;
    //private bool changeDirectionDueToOutOfBounds = false;



    private void Awake()
    {
        //Cacheamos el componente de character controller y de vida
        this.characterController = GetComponent<CharacterController2D>();
        this.life = GetComponent<Life>();
    }

    // Update is called once per frame
    void Update()
    {

        //if (Math.Abs(this.transform.position.x)> this.maxXValue || Math.Abs(this.transform.position.y) > this.maxYValue)
        //{
        //    Debug.Log("Límites sobrepasados. X actual: " + this.transform.position.x + ". Y actual: " + this.transform.position.y);
        //    this.changeDirectionDueToOutOfBounds = true;
        //}

    }

    private void OnEnable()
    {
        //De momento los enemigos simplemente escucharán al evento OnLifeDepleted (cuando la barra de vida llega a su fin)
        this.life.onLifeDepleted.AddListener(OnLifeDepleted);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Patrol());
    }

    private void OnDisable()
    {
        //De momento los enemigos simplemente escucharán al evento OnLifeDepleted (cuando la barra de vida llega a su fin)
        this.life.onLifeDepleted.RemoveListener(OnLifeDepleted);
    }

    private void OnLifeDepleted(float startLife)
    {
        //Destruimos el gameobject del enemigo
        Destroy(this.gameObject);
    }

    IEnumerator Patrol()
    {

        this.movementDirection = SetNewDestiny();
        this.characterController.SetRawMove(movementDirection);

        while (true)
        {
            this.directionSecondsCounter += Time.deltaTime;

            if (CheckChangeDirection())
            {
                this.movementDirection = SetNewDestiny();
            }

            yield return null;
            this.characterController.SetRawMove(this.movementDirection);

        }

    }

    private Vector2 SetNewDestiny()
    {
        //Hayamos una dirección aleatoria:
        float randomXCoordinate = Random.Range(-1f, 1f);
        float randomYCoordinate = Random.Range(-1f, 1f);

        Vector2 searchDirection = new Vector2(randomXCoordinate, randomYCoordinate).normalized;

        //Reseteamos el contador de segundos de movimeinto en la misma dirección:
        this.directionSecondsCounter = 0;
        this.changeDirectionDueToCollision = false;
        //this.changeDirectionDueToOutOfBounds = false;

        return searchDirection;

    }

    private bool CheckChangeDirection()
    {
        if (this.directionSecondsCounter >= this.maxSecondsSameDirection)
        {
            Debug.Log("Enemigo, cambiar sentido por máximo número de segundos en la misma dirección");
            return true;
        }
        else if(this.changeDirectionDueToCollision == true)
        {
            Debug.Log("Enemigo, cambiar sentido por colisión");
            return true;
        }
        //else if (this.changeDirectionDueToOutOfBounds == true)
        //{
        //    Debug.Log("Enemigo, cambiar sentido por irse fuera de límites establecidos");
        //    return true;
        //}
        else
            return false;
    }

    private void OnCollisionEnter2D(Collision2D elOtro)
    {
        Debug.Log("Enemigo ha colisionado con:" + elOtro.ToString());
        this.changeDirectionDueToCollision = true;
    }


}
