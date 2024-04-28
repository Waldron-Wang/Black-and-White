using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class black_controller : MonoBehaviour
{
    Rigidbody2D rb_black;
    Animator animator_black;

    string current_state;
    const string animation_idle = "idle";
    const string animation_run = "run";
    const string animation_walk = "walk";
    const string animation_run_end = "run_end";
    const string animation_jump_up_1 = "jump_up_1";
    const string animation_jump_roll = "jump_roll";
    const string animation_fall = "fall";
    const string animation_jump_end = "jump_end";
    const string animation_run_jump_up_1 = "run_jump_up_1";
    const string animation_run_jump_end = "run_jump_end";
    const string animation_dodge = "dodge";


    [SerializeField] float run_speed;    //移动的速度velocity
    [SerializeField] float walk_speed;   //移动的速度velocity
    [SerializeField] float jump_force_1; //跳跃的力度force
    [SerializeField] float jump_force_2; //二段跳的力度froce
    [SerializeField] float dodge_time;       //闪避持续时间
    [SerializeField] float dodge_cool_time;  //闪避冷却时间
    [SerializeField] float dodge_power;
    [SerializeField] float dodge_gravity;
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
    bool is_face_right;
    bool is_moving_horizontal;
    bool is_ground;
    bool is_dodging;
    bool can_dodge;
    bool dodge_input;

    RaycastHit2D object_being_hit;
    //这是被射线碰撞的结构体实例，包含有关碰撞的详细信息，如碰撞点的位置、碰撞点的表面法线、碰撞物体的引用
    Vector2 ray_origin;
    //射线的起始点
    Vector2 ray_direction;
    //射线的方向
    [SerializeField] float ray_distance;
    //射线的长度,同时也是人物开始进入jump_state 3时的离地面的距离
    [SerializeField] LayerMask ray_layer;
    //使射线只检测30层里的碰撞ti

    // Start is called before the first frame update
    void Start()
    {
        rb_black = gameObject.GetComponent<Rigidbody2D>();
        animator_black = gameObject.GetComponent<Animator>();

        is_face_right = true;
        is_ground = true;
        is_dodging = false;
        can_dodge = true;
        dodge_input = false;

        jump_count = 0;
        jump_count_max = 2;

        ray_direction = Vector2.down;
    }

    // Update is called once per frame
    void Update()
    {
        //人物移动按键输入
        move_horizontal = Input.GetAxisRaw("Horizontal");
        //unity的输入系统被设置为与帧更新同步

        //监测并调整人物转向
        if (move_horizontal > 0.01f && is_face_right == false)
        {
            Flip();
        }
        else if (move_horizontal < -0.01f && is_face_right == true)
        {
            Flip();
        }


        //监测人物目前跳跃状态 */
        ray_origin = new Vector2(transform.position.x, transform.position.y);
        if (jump_count == 0  && (rb_black.velocity.y < 0.001f && rb_black.velocity.y > -0.001f)) 
        {
            jump_state = 0;
            is_ground = true;
        }
        else if (rb_black.velocity.y > 0.001f)
        {
            jump_state = 1;
            is_ground = false;
        }
        else if (rb_black.velocity.y < -0.001f)
        {
            object_being_hit = Physics2D.Raycast(ray_origin, ray_direction, ray_distance, ray_layer);
            if (object_being_hit.collider != null)
            {
                if(jump_state != 3)
                //使land_trigger仅被触发一次，防止land动画后land_trigger依然处于触发状态
                {
                    jump_state = 3;
                }
            }
            else
            {
                if (jump_state != 2)
                {
                    jump_state = 2;
                }
                
            }
        }

        if (Input.GetButtonDown("Jump"))
        {
            move_vertical = true;
        }
        //Input.GetButtonDown仅会在按下按键的那一帧为true，并且其更新不与物理系统同步，导致输入容易被遗漏，
        //设置一个变量储存按下按键的信息，在物理系统执行跳跃后重置为false

        //闪避按键输入
        if (Input.GetKeyDown(KeyCode.LeftShift) && can_dodge)
        //此处并上can_dodge条件，使其在冷却时间内无法触发input
        {
            dodge_input = true;
        }

        AnimationController();
    }

    private void FixedUpdate()
    {
        if (is_dodging)
        {
            return;
        }

        if (Input.GetKey(KeyCode.Tab) == true)
        {
            Walk();
        }
        else
        {
            Run();
        }

        if (move_vertical)
        {
            Jump();
            move_vertical = false;
        }

        if (dodge_input && can_dodge)
        {
            dodge_input = false;
            StartCoroutine(Dodge());
        }
    }

    void AnimationController()
    {
        /* idle animation */
        if (!is_moving_horizontal && is_ground && !is_dodging)
        {
            if (!IsAnimationPlaying(animation_jump_end))
            {
                ChangeAnimationState(animation_idle);
            }
        }

        /* run animation */
        if (is_moving_horizontal && is_ground && !is_dodging)
        {
            if (!IsAnimationPlaying(animation_run_jump_end))
            {
                if (current_state == animation_jump_end){
                    ChangeAnimationState(animation_run, 0.30f);
                }
                else
                {
                    ChangeAnimationState(animation_run);
                }
            }
        }

        /* jump animation */
        if (!is_ground && !is_dodging)
        {
            /* idle jump */
            if (!is_moving_horizontal)
            {
                if(jump_count == 1 && jump_state == 1
                    && !animator_black.GetCurrentAnimatorStateInfo(0).IsName(animation_run_jump_up_1))
                {
                    ChangeAnimationState(animation_jump_up_1);
                }
                if (jump_state == 3
                    && !animator_black.GetCurrentAnimatorStateInfo(0).IsName(animation_run_jump_end))
                {
                    ChangeAnimationState(animation_jump_end);
                }
            }
            /* run jump */
            else
            {
                if(jump_count == 1 && jump_state == 1 && !animator_black.GetCurrentAnimatorStateInfo(0).IsName(animation_jump_up_1))
                {
                    ChangeAnimationState(animation_run_jump_up_1);
                }
                if (jump_state == 3 && !animator_black.GetCurrentAnimatorStateInfo(0).IsName(animation_run_end))
                {
                    ChangeAnimationState(animation_run_jump_end);
                }
            }

            if (jump_count == 2 && jump_state == 1)
            {
                ChangeAnimationState(animation_jump_roll);
            }
            if (jump_state == 2)
            {
                if (!IsAnimationPlaying(animation_jump_roll)){
                    ChangeAnimationState(animation_fall);
                }
            }
        }

        if (is_dodging)
        {
            ChangeAnimationState(animation_dodge);
        }
    }

    void Run()
    {
        if (move_horizontal > 0.1f || move_horizontal < -0.1f)
        {
            rb_black.velocity = new Vector3(move_horizontal * run_speed, rb_black.velocity.y, 0f);
            is_moving_horizontal = true;
        }
        else
        {
            rb_black.velocity = new Vector3(0f, rb_black.velocity.y, 0f);
            is_moving_horizontal = false;
        }
    }

    void Walk()
    {
        if (move_horizontal > 0.1f || move_horizontal < -0.1f)
        {
            rb_black.velocity = new Vector3(move_horizontal * walk_speed, rb_black.velocity.y, 0f);
            is_moving_horizontal = true;
        }
        else
        {
            rb_black.velocity = new Vector3(0f, rb_black.velocity.y, 0f);
            is_moving_horizontal = false;
        }
    }

    void Jump()
    {
        if (jump_count < jump_count_max)
        {

            if (jump_count == 0)
            {
                if (jump_state == 2 || jump_state == 3)
                {
                    jump_count++;
                }
                //对于并没有进行一段跳跃，因跑出平台而发生坠落的情况：只允许跳一次
                rb_black.AddForce(new Vector3(0f, jump_force_1, 0f), ForceMode2D.Impulse);
            }
            else if (jump_count == 1)
            {
                rb_black.velocity = new Vector3(rb_black.velocity.x, 0f, 0f);
                rb_black.AddForce(new Vector3(0f, jump_force_2, 0f), ForceMode2D.Impulse);
            }
            jump_count++;
        }
    }

    void Flip()
    {
        is_face_right = !is_face_right;
        transform.Rotate(0, 180, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "platforms")
        {
            jump_count = 0;
        }
    }

    IEnumerator Dodge()
    {
        can_dodge = false;
        is_dodging = true;
        float original_gravity = rb_black.gravityScale;
        rb_black.gravityScale = dodge_gravity;
        rb_black.velocity = new Vector3(transform.right.x * dodge_power, 0f, 0f);
        //unity中transform.right(人物x轴在世界坐标中的朝向) forward(z axis)

        yield return new WaitForSeconds(dodge_time);

        is_dodging = false;
        rb_black.velocity = new Vector3(0f, 0f, 0f);
        rb_black.gravityScale = original_gravity;

        yield return new WaitForSeconds(dodge_cool_time);

        can_dodge = true;
    }

    void ChangeAnimationState(string new_state, float start_time = 0f)
    {
        if (new_state == current_state)
        {
            return;
        }

        animator_black.Play(new_state, 0, start_time);
        current_state = new_state;
    }

    bool IsAnimationPlaying(string state_name, float exit_time = 1.0f)
    {
        if (animator_black.GetCurrentAnimatorStateInfo(0).IsName(state_name) && animator_black.GetCurrentAnimatorStateInfo(0).normalizedTime < exit_time)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}