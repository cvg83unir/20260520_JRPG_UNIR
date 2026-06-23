using System;
using UnityEngine;

public class SpikesTileMapScript : MonoBehaviour
{
    [SerializeField] float timeInterval = 2f; //tiempo de intervalo entre activaciones o reset de loop
    [SerializeField] bool loopActivation = false; //true->sigue un loop segun la jerarquia, false -> se activan y desactivan todos a la vez


    SpikesScript[] childSpikes;
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    void Start()
    {
        childSpikes = GetComponentsInChildren<SpikesScript>();
        for(int i=0; i<childSpikes.Length; i++)
        {
            float delay = 0;
            if (loopActivation)
            {
                delay = timeInterval-(1f/(childSpikes.Length - 1f) * i*timeInterval);
            }
            childSpikes[i].setValuesActivate(timeInterval, delay);
        }
        
    }

    // Update is called once per frame
    
    void Update()
    {
       
    }
}
