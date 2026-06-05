using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class HitCollider : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        HurtCollider hurt = elOtro.GetComponent<HurtCollider>();

        hurt?.NotifyHit(this);
    }

    //private void OnTriggerStay2D(Collider2D elOtro)
    //{
    //    HurtCollider hurt = elOtro.GetComponent<HurtCollider>();

    //    hurt?.NotifyHit(this);
    //}

    //private void OnTriggerExit2D(Collider2D elOtro)
    //{
    //    HurtCollider hurt = elOtro.GetComponent<HurtCollider>();

    //    hurt?.NotifyHit(this);
    //}





}
