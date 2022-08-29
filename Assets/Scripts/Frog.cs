using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rig;

    public float speed;

    public Transform leftCol;
    public Transform rightCol;

    public Transform headPoint;

    private bool colliding;

    public LayerMask layer;

    public CircleCollider2D circleCollider;
    public BoxCollider2D boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rig.velocity = new Vector2(speed, rig.velocity.y);

        colliding = Physics2D.Linecast(rightCol.position, leftCol.position, layer);
        if (colliding)
        {
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            speed *= -1f;
        }
    }

    bool playerDestroyed = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Is player");
            float height = collision.contacts[0].point.y - headPoint.position.y;
            if(height > 0 && !playerDestroyed)
            {
                Debug.Log("Is upside head.");
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 9, ForceMode2D.Impulse);
                speed = 0;
                anim.SetTrigger("die");
                boxCollider.enabled = false;
                circleCollider.enabled = false;
                rig.bodyType = RigidbodyType2D.Kinematic;
                Destroy(gameObject, 0.33f);

            }else
            {
                playerDestroyed = true;
                GameController.ShowGameOver();
                Destroy(collision.gameObject);
            }
        }
    }

}
