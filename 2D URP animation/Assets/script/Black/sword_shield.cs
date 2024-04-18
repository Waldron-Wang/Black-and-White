using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sword_shield : MonoBehaviour
{
    Animator ss_animator; //sword_shield的animator组件

    bool attack_input;
    //标示玩家是否有攻击输入
    bool attack_next_input;
    //标示在玩家攻击过程中是否有下一次攻击的输入
    bool is_cheacking_attack_input;
    //现在是否在攻击输入检测时间段内
    bool is_attacking;
    //是否处于攻击状态
    
    // Start is called before the first frame update
    void Start()
    {
        ss_animator = gameObject.GetComponent<Animator>();

        attack_input = false;
        attack_next_input = false;
        is_attacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) && is_attacking == false)
        {
            attack_input = true;
            is_attacking = true;
        }

        if (is_cheacking_attack_input)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                attack_next_input = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (attack_input || attack_next_input)
        {
            ss_animator.SetTrigger("attack_trigger");
        }
    }

    void start_check()
    {
        is_cheacking_attack_input = true;
    }

    void end_check()
    {
        is_cheacking_attack_input = false;
    }
}
