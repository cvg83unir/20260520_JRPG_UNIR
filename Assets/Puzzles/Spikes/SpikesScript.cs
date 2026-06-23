using UnityEngine;

public class SpikesScript : MonoBehaviour
{
    BoxCollider2D boxCollider2D;

    Animator animator;


    
    #region variables de tiempo y funcionamiento
    float timeInterval = 0; //para cada cuanto tiempo se tiene que activar de nuevo
    public float timeDelay = 0;    //para llevar la cuenta de los segundos
    public bool activated=false;   //para que no empiece hasta que se hayan seteado todos los valores y por si se tiene que detener
    #endregion






    
    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        setDamageActive(0);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            
            if (timeDelay>=timeInterval)
            {
                
                timeDelay -= timeInterval;
                animator.SetTrigger("activate");
            }
            timeDelay += Time.deltaTime;
        }
    }

    public void setValuesActivate(float interval, float sequenceDelay)
    {
        timeInterval = interval;
        timeDelay = sequenceDelay;
        setValuesActivate();
    }

    public void setValuesActivate()
    {
        if (timeInterval > 0)
        {
            activated = true;
            
        }
        
    }
    public void setDamageActive(int activarHit)
    {

        boxCollider2D.enabled = activarHit!=0;
    }
    


}
