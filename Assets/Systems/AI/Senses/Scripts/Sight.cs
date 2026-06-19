using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : MonoBehaviour
{
    //Este script va a identificar qué está viendo y qué no está viendo el enemigo:
    List<IVisible> visiblesInSight = new();

    List<IVisible> visiblesToShoot = new();

    /// <summary>
    /// Property de sólo lectura para poder acceder desde otro objeto a la lista visiblesInSight 
    /// </summary>
    public List<IVisible> VisiblesInSight
    {
        get => this.visiblesInSight;
    }

    public List<IVisible> VisiblesToShoot
    {
        get => this.visiblesToShoot;
    }

    [SerializeField] private float radius = 3f;
    [SerializeField] List<IVisible.Side> attendedSides;



    // Update is called once per frame
    void Update()
    {
        CheckPosiblesInSight();

        CheckPosiblesToShoot();
    }

    private void CheckPosiblesInSight()
    {
        visiblesInSight.Clear();

        Collider2D[] potentialVisibles = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D c in potentialVisibles)
        {
            IVisible visible = c.gameObject.GetComponent<IVisible>();

            if (visible != null && attendedSides.Contains(visible.GetSide()))
            {
                this.visiblesInSight.Add(visible);
            }
        }
    }

    private void CheckPosiblesToShoot()
    {
        visiblesToShoot.Clear();

        Collider2D[] potentialVisibles = Physics2D.OverlapCircleAll(transform.position, radius/2);

        foreach (Collider2D c in potentialVisibles)
        {
            IVisible visible = c.gameObject.GetComponent<IVisible>();

            if (visible != null && attendedSides.Contains(visible.GetSide()))
            {
                this.visiblesToShoot.Add(visible);
            }
        }
    }


}
