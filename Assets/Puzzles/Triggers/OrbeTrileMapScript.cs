using System;
using UnityEngine;
using UnityEngine.Events;

public class OrbeTrileMapScript : MonoBehaviour
{
    public UnityEvent onPuzzleCompleat;

    OrbeScript[] childOrbs;
    int progress = 0;



    private void Awake()
    {
        childOrbs = GetComponentsInChildren<OrbeScript>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void checkAnswer(OrbeScript orb)
    {
        //Debug.Log($"activado el orbe: {Array.IndexOf(childOrbs, orb)}, y se esperaba el numero {progress}, asi que es:{childOrbs[progress] == orb}")
        if (childOrbs[progress] == orb)
        {
           
            orb.setCorrect();
            progress++;
            if (progress >= childOrbs.Length)
            {
                puzzleSolved();
            }
        }
        else
        {
            
            setErrorAll();
            progress=0;
        }
    }

    void setErrorAll()
    {
        foreach(OrbeScript orb in childOrbs)
        {
            orb.setError();
        }
    }
    internal void puzzleSolved()
    {
        onPuzzleCompleat.Invoke();
    }
}
