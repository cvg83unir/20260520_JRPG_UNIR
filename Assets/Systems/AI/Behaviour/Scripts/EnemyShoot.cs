using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [SerializeField] float shotSpeed = 2f;
    [SerializeField] float shotDuration = 10f;//tiempo después del cual destruimos el GameObject (el disparo) muera al cabo de unos segundos para ir liberando la escena de Objetos:
    private Vector2 shotDirection;

    private Rigidbody2D rb2D;
    private CharacterController2D characterController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Cacheamos el rigidbody:
        this.rb2D = GetComponent<Rigidbody2D>();
        this.characterController = GetComponentInParent<CharacterController2D>();
        //if (this.characterController != null)
        //{
        //    this.shotDirection = this.characterController.PreviousRawMove;
        //}
        //else
        //{
        //    this.shotDirection = Vector3.left;
        //}

    }

    public void setShotDirection(Vector2 parentShotDirection)
    {
        this.shotDirection = parentShotDirection;
    }


    // Update is called once per frame
    void Update()
    {
        this.rb2D.linearVelocity = this.shotDirection * this.shotSpeed;
        //this.rb2D.linearVelocity = direccion * shotSpeed * Time.deltaTime;

        //Necesitamos que el GameObject (el disparo) muera al cabo de unos segundos para ir liberando la escena de Objetos:
        Destroy(gameObject, shotDuration);

    }

    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        //Si el disparo del enemigo impacta en el player o en el tilemap de colisiones, debe destruirise:
        if (elOtro.CompareTag("Player") || elOtro.CompareTag("TileMapCollisions"))
        {
            Destroy(gameObject);
        }
    }
}
