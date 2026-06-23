using System;
using UnityEngine;

public class OrbeScript : MonoBehaviour
{
    OrbeTrileMapScript orbeTrileMapScript;
    HurtCollider hurtCollider;
    Animator animator;

    private void Awake()
    {
        orbeTrileMapScript =GetComponentInParent<OrbeTrileMapScript>();
        hurtCollider = GetComponent<HurtCollider>();
        animator = GetComponent<Animator>();

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hurtCollider.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void sendAnswer()
    {
        if (hurtCollider.enabled)
        {
            orbeTrileMapScript.checkAnswer(this);
        }
        
    }
    public void setCorrect()
    {
        animator.SetTrigger("activation");
        hurtCollider.enabled = false;
    }
    public void setError()
    {
        animator.SetTrigger("error");
        hurtCollider.enabled = true;
    }
    
}
