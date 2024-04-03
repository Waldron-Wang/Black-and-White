using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class black_controller : MonoBehaviour
{
    Rigidbody2D rb_black;
    Animator animator_black;
    SpriteRenderer spriteRenderer_balck;

    [SerializeField] float move_speed;
    [SerializeField] float jump_force;
    float move_horizontal;
    int jump_count;
    int jump_count_max;
    bool move_vertical;

    // Start is called before the first frame update
    void Start()
    {
        rb_black = gameObject.GetComponent<Rigidbody2D>();
        animator_black = gameObject.GetComponent<Animator>();
        spriteRenderer_balck = gameObject.GetComponent<SpriteRenderer>();

        jump_count = 0;
        jump_count_max = 1;
    }

    // Update is called once per frame
    void Update()
    {
        move_horizontal = Input.GetAxisRaw("Horizontal");
        //unity的输入系统被设置为与帧更新同步
        if (Input.GetButtonDown("Jump"))
        {
            move_vertical = true;
        }
        //Input.GetButtonDown仅会在按下按键的那一帧为true，并且其更新不与物理系统同步，导致输入容易被遗漏，
        //设置一个变量储存按下按键的信息，在物理系统执行跳跃后重置为false
    }

    private void FixedUpdate()
    {
        movement();

        if (move_vertical)
        {
            jump();
            move_vertical = false;
        }
    }

    void movement()
    {
        if(move_horizontal > 0.1f || move_horizontal < -0.1f)
        {
            rb_black.velocity = new Vector3(move_horizontal * move_speed, rb_black.velocity.y, 0f);
            animator_black.SetBool("is_move_x", true);
            if (move_horizontal > 0.1f)
            {
                spriteRenderer_balck.flipX = false;
            }
            else
            {
                spriteRenderer_balck.flipX = true;
            }
        }
        else
        {
            rb_black.velocity = new Vector3(0f, rb_black.velocity.y, 0f);
            animator_black.SetBool("is_move_x", false);
        }
    }

    void jump()
    {
        if (move_vertical && jump_count < jump_count_max)
        {
            rb_black.AddForce(new Vector3(0f, jump_force, 0f), ForceMode2D.Impulse);
            jump_count ++;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "platforms")
        {
            jump_count = 0;
        }
    }
}
