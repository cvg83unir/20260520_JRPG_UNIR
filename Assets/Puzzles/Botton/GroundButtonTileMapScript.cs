using UnityEngine;
using UnityEngine.Events;

public class GroundButtonTileMapScript : MonoBehaviour
{
    public UnityEvent onPuzzleCompleat;
    GroundButtonScript[] childButton;
    public bool blockDeactivation;
    bool solved = false;
    private void Awake()
    {
        childButton = GetComponentsInChildren<GroundButtonScript>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void comprobar(bool active)
    {

    }
    public void checkAnswer()
    {
        Debug.Log("Comprobar solucion");
        if (solved)
        {
            return;
        }
        foreach(GroundButtonScript child in childButton)
        {
            if (!child.isPush())
            {
                Debug.Log(child.isPush());
                return;
            }
        }
        solved = true;
        blockDeactivation = true;
        onPuzzleCompleat.Invoke();

    }
}
