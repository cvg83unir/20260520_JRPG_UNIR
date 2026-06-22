using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyShooterPatrol : MonoBehaviour
{
    private CharacterController2D characterController;
    [Header("BlinkAfterHit")]
    [SerializeField] private float blinkingSecondsInterval = 0.15f;
    private SpriteRenderer spriteRenderer;
    private Life life;

    //variables para controlar el cambio de sentido:
    [SerializeField] float maxSecondsSameDirection = 2f;
    private float directionSecondsCounter = 0f;
    private Vector2 movementDirection = Vector2.zero;
    private bool changeDirectionDueToCollision = false;

    //Variables para controlar que los enemigos disparen el número adecuado de veces de acuedo a la variable timeBetweenAttacks 
    [SerializeField] float timeBetweenAttacks = 1f;
    public float PropTimeBetweenAttacks
    {
        get => this.timeBetweenAttacks;
    }
    private float timeSinceLastShotCorroutine = 0f;
    private bool bIsInCoroutineShot = false;
    private bool bTimeBetweenLastCoroutineShootCorrect = false;
    private bool bIsShooting = false;
    public bool CanInstantiateShoot
    {
        get => (this.bTimeBetweenLastCoroutineShootCorrect);
    }

    //Variables para controlar si algún player lo suficientemente cerca como para atacar:
    [SerializeField] private float radiusToShoot = 3f;
    List<IVisible> visiblesToShoot = new();
    [SerializeField] List<IVisible.Side> attendedSides;


    private void Awake()
    {
        //Cacheamos el componente de character controller y de vida
        this.characterController = GetComponent<CharacterController2D>();
        this.life = GetComponent<Life>();

        //Aunque, en este juego, el spriterender del personaje está en uno de sus gamebojects
        //hijos, por ello debemos buscarlo con GetComponentsInChildren:
        this.spriteRenderer = GetComponentsInChildren<SpriteRenderer>()[0];

        this.timeSinceLastShotCorroutine = timeBetweenAttacks;
    }

    // Update is called once per frame
    void Update()
    {
        

        if (bIsInCoroutineShot == false)
        {
            this.timeSinceLastShotCorroutine += Time.deltaTime;
            if (this.timeSinceLastShotCorroutine >= timeBetweenAttacks)
            {
                this.bTimeBetweenLastCoroutineShootCorrect = true;
                this.timeSinceLastShotCorroutine = timeBetweenAttacks;

                if(this.bTimeBetweenLastCoroutineShootCorrect == true)
                {
                    CheckPosiblesToShoot();

                    if (this.visiblesToShoot.Count > 0)
                    {
                        StartCoroutine(Shoot());
                    }

                }
            }
        }
    }

    IEnumerator Shoot()
    {
        this.bIsInCoroutineShot = true;
        this.timeSinceLastShotCorroutine = 0;
        Debug.Log("Nueva Corutina: " + DateTime.Now.ToString("yyyyMMddHHmmss"));

        while (this.visiblesToShoot.Count > 0)
        {

            //Para que no todos los enemigos disparen exactamente a la vez, añadimos número
            //aleatorio de menos de 1 segundo
            Debug.Log("Attack: " + DateTime.Now.ToString("yyyyMMddHHmmss"));
            
            this.bIsShooting = true;
            yield return new WaitForSeconds(0.25f);
            this.characterController.Attack();
            this.bIsShooting = false;
            //Debug.Log("Antes del yield: " + DateTime.Now.ToString("yyyyMMddHHmmss"));
            yield return new WaitForSeconds(this.timeBetweenAttacks);

            CheckPosiblesToShoot();
            //Debug.Log("Superado tiempo de espera en coroutina: " + DateTime.Now.ToString("yyyyMMddHHmmss"));
            //yield return new WaitForSeconds(this.timeBetweenAttacks + Random.Range(0f, 1f));
        }

        this.bIsInCoroutineShot = false;
    }

    private void OnEnable()
    {
        //Estos enemigos escucharán el onLifeDepleted y onLifeChanged
        this.life.onLifeDepleted.AddListener(OnLifeDepleted);
        this.life.onLifeChanged.AddListener(OnLifeChanged);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Patrol());
    }

    private void OnDisable()
    {
        //Estos enemigos escucharán el onLifeDepleted y onLifeChanged
        this.life.onLifeDepleted.RemoveListener(OnLifeDepleted);
        this.life.onLifeChanged.AddListener(OnLifeChanged);
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

            if (this.bIsInCoroutineShot == true && this.bIsShooting == true)
            {
                Debug.Log("Enemigo. Cambio de sentido para disparar hacia el player");
                Vector2 searchDirection = (this.visiblesToShoot[0].GetTransform().position - transform.position).normalized;

                this.movementDirection = searchDirection;
            }
            else if (CheckChangeDirection())
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
        else if (this.changeDirectionDueToCollision == true)
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
        //Debug.Log("Enemigo ha colisionado con:" + elOtro.ToString());
        this.changeDirectionDueToCollision = true;
    }

    private void CheckPosiblesToShoot()
    {
        visiblesToShoot.Clear();

        Collider2D[] potentialVisibles = Physics2D.OverlapCircleAll(transform.position, this.radiusToShoot);

        foreach (Collider2D c in potentialVisibles)
        {
            IVisible visible = c.gameObject.GetComponent<IVisible>();

            if (visible != null && attendedSides.Contains(visible.GetSide()))
            {
                this.visiblesToShoot.Add(visible);
            }
        }
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
}
