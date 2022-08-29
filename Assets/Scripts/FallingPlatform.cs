using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{

    public float fallingTime;

    private TargetJoint2D target;
    private BoxCollider2D boxColl;

    // Start is called before the first frame update
    void Start()
    {
        target = GetComponent<TargetJoint2D>();
        boxColl = GetComponent<BoxCollider2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player") // Se colidiu com o jogador
        {
            Invoke("Falling", fallingTime);
        }

        if (collision.gameObject.layer == 6 || collision.gameObject.tag == "Spike") // Se colidiu com o chão
        {
            boxColl.isTrigger = true;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7) // Se colidiu com o abismo
        {
            Destroy(gameObject);
        }
    }

    void Falling()
    {
        target.enabled = false;
    }
}
