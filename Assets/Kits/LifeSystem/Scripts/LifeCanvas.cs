using System;
using UnityEngine;
using UnityEngine.UI;

public class LifeCanvas : MonoBehaviour
{
    [SerializeField] Life life;
    [SerializeField] Image mask;

    private void OnEnable()
    {
        this.life.onLifeChanged.AddListener(OnLifeChanged);
        //this.life.onLifeDepleted.AddListener(OnLifeDepleted);
    }

    private void OnDisable()
    {
        this.life.onLifeChanged.RemoveListener(OnLifeChanged);
        //this.life.onLifeDepleted.RemoveListener(OnLifeDepleted);    
    }



    private void OnLifeChanged(float currentLife, float startLife)
    {
        mask.fillAmount = currentLife / startLife;
    }

    //private void OnLifeDepleted(float startLife)
    //{
    //    throw new NotImplementedException();
    //}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
