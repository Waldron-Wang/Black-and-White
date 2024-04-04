using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class black_controller : MonoBehaviour
{
    Rigidbody2D rb_black;
    Animator animator_black;
    SpriteRenderer spriteRenderer_balck;

    [SerializeField] float move_speed;   //移动的速度velocit
    [SerializeField] float jump_force_1; //跳跃的力度force
    [SerializeField] float jump_force_2; //二段跳的力度froce
    float move_horizontal;
    //检测玩家是否有水平移动输入
    int jump_count;
    //记录当前跳跃次数，每跳跃一次+1，落地置0
    int jump_state;
    //记录当前跳跃状态，0为未跳跃，1为上升，2为下降，3为即将落地
    int jump_count_max;
    //最大跳跃次数
    bool move_vertical;
    //检测玩家是否有跳跃输入

    RaycastHit2D object_being_hit;
    //这是被射线碰撞的结构体实例，包含有关碰撞的详细信息，如碰撞点的位置、碰撞点的表面法线、碰撞物体的引用
    Vector2 ray_origin;
    //射线的起始点
    Vector2 ray_direction;
    //射线的方向
    [SerializeField] float ray_distance;
    //射线的长度,同时也是人物开始进入jump_state 3时的离地面的距离
    int hit_layer = 1 << 3;
    //使射线只检测30层里的碰撞ti
    // Start is called before the first frame update
    void Start()
    {
        rb_black = gameObject.GetComponent<Rigidbody2D>();
        animator_black = gameObject.GetComponent<Animator>();
        spriteRenderer_balck = gameObject.GetComponent<SpriteRenderer>();

        jump_count = 0;
        jump_count_max = 2;

        ray_direction = Vector2.down;
    }

    // Update is called once per frame
    void Update()
    {
        move_horizontal = Input.GetAxisRaw("Horizontal");
        //unity的输入系统被设置为与帧更新同步

        ray_origin = new Vector2(transform.position.x, transform.position.y);
        if (jump_count == 0) //不用速度作为人物是否在地面的原因是人物的部分空中攻击动作是悬停在空中的
        {
            jump_state = 0;
            animator_black.SetInteger("jump_state", 0);
        }
        else if(rb_black.velocity.y > 0.001f)
        {
            jump_state = 1;
            animator_black.SetInteger("jump_state", 1);
        }
        else if (rb_black.velocity.y < -0.001f)
        {
            object_being_hit = Physics2D.Raycast(ray_origin, ray_direction, ray_distance, hit_layer);
            if(object_being_hit.collider != null)
            {
                Debug.Log("Hit " + object_being_hit.collider.gameObject.name);
                jump_state = 3;
                animator_black.SetInteger("jump_state", 3);
            }
            else
            {
                jump_state = 2;
                animator_black.SetInteger("jump_state", 2);
            }
        }
        Debug.Log("jump_state" + jump_state);
        Debug.Log("jump_count" + jump_count);

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
            animator_black.SetBool("is_running", true);
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
            animator_black.SetBool("is_running", false);
        }
    }

    void jump()
    {
        if (jump_count < jump_count_max)
        {
            if(jump_count == 0)
            {
                if (jump_state == 2 || jump_state == 3)
                {
                    jump_count++;
                    animator_black.SetInteger("jump_count", jump_count);
                }
                //对于并没有进行一段跳跃，因跑出平台而发生坠落的情况：只允许跳一次
                rb_black.AddForce(new Vector3(0f, jump_force_1, 0f), ForceMode2D.Impulse);
            }
            else if (jump_count == 1)
            {
                rb_black.velocity = new Vector3(rb_black.velocity.x, 0f, 0f);
                rb_black.AddForce(new Vector3(0f, jump_force_2, 0f), ForceMode2D.Impulse);
            }
            jump_count ++;
            animator_black.SetInteger("jump_count", jump_count);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "platforms")
        {
            jump_count = 0;
            animator_black.SetInteger("jump_count", 0);
        }
    }
}
