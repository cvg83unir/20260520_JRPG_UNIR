using UnityEngine;

public class GroundButtonScript : MonoBehaviour
{
    Animator animator;
    

    GroundButtonTileMapScript gbTileMap;
    private void Awake()
    {
        gbTileMap = GetComponentInParent<GroundButtonTileMapScript>();
        animator = GetComponent<Animator>();
        animator.SetBool("Push", false);

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("tocar el boton");
        if (collision.gameObject.tag == "Player")
        {
            animator.SetBool("Push", true);
            gbTileMap.checkAnswer();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !gbTileMap.blockDeactivation)
        {
            animator.SetBool("Push", false);
        }
    }

    public bool isPush()
    {
        return animator.GetBool("Push");
    }
}
