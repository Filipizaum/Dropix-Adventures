using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed;
    public float jumpForce;

    public bool isJumping;
    public int extraJumps;

    public bool ableToMove = true;

    private Rigidbody2D rig;
    private Animator anim;

    public const int MAX_EXTRA_JUMPS = 2;

    bool overAntiJump = false;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
    }

    void Move()
    {
        if (!ableToMove) return;

        float haxis = Input.GetAxis("Horizontal");

        // Move usando transform.position
        //Vector3 movement = new Vector3(haxis, 0f, 0f);
        //transform.position += movement * Time.deltaTime * speed;

        rig.velocity = new Vector2 (speed * haxis, rig.velocity.y);

        if(haxis > 0f)
        {
            anim.SetBool("walk", true);
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
        else if(haxis < 0f)
        {
            anim.SetBool("walk", true);
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
        else if(haxis == 0f)
        {
            anim.SetBool("walk", false);
        }
    }

    void Jump()
    {
        if (!ableToMove) return;
        if (Input.GetButtonDown("Jump") && !overAntiJump)
        {
            bool shouldJump = false;
            if (!isJumping)
            {
                shouldJump = true;
                anim.SetBool("jump", true);
            }
            else if(extraJumps < MAX_EXTRA_JUMPS)
            {
                extraJumps += 1;
                shouldJump = true;
            }
            if(shouldJump)
            {
                rig.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.layer == 6) // Se colidiu com o chão
        {
            isJumping = false;
            extraJumps = 0;
            anim.SetBool("jump", false);
        }
        if(collision.gameObject.tag == "Spike")
        {
            GameController.ShowGameOver();
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "Saw")
        {
            GameController.ShowGameOver();
            Destroy(gameObject);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6) // Se saiu do chão
        {
            isJumping = true;
            extraJumps = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 9)
        {
            overAntiJump = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 9)
        {
            overAntiJump = false;
        }
    }

}
