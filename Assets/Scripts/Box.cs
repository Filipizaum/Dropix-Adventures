using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public float throwForce;
    public bool isUp;

    public int health = 5;

    public Animator animator;
    public GameObject destroyEffect;

    private void Update()
    {
        if(health <= 0)
        {
            Instantiate(destroyEffect, transform.position, transform.rotation);
            Destroy(transform.parent.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("colidou com a caixa: " + collision.gameObject.tag);
        if (collision.gameObject.tag == "Player")
        {
            //animator.ForceStateNormalizedTime(0f);
            //animator.CrossFadeInFixedTime()
            animator.SetTrigger("hit");
            health--;
            if(isUp)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, throwForce), ForceMode2D.Impulse);
            }else
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, -throwForce), ForceMode2D.Impulse);
            }
        }
    }
}
