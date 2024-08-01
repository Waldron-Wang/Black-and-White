using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class sword_shield : MonoBehaviour
{
    black_controller black_Controller;
    Animator animator;

    float attack_cooling_time;
    float attack_waiting_time;
    public int attack_count;
    //0:not attacking   1:attack_1   2:attack_2   3:attack_3
    
    public bool is_attacking;
    //是否处于攻击状态,用于与controller脚本通信
    bool attack_input;
    //标示玩家是否有攻击输入
    bool is_checking_attack_input;
    bool can_attack;
    
    // Start is called before the first frame update
    void Start()
    {
        black_Controller = gameObject.GetComponent<black_controller>();
        animator = gameObject.GetComponent<Animator>();

        attack_count = 0;
        attack_input = false;
        is_attacking = false;
        can_attack = true;
        is_checking_attack_input = false;
    }
    
    void Update()
    {
        CheckAttackInput();
        
        AttackStateMachine();
        Debug.Log("is checking attack input"+is_checking_attack_input);
    }

    void AttackStateMachine()
    {
        if (attack_count == 0 && attack_input && !is_checking_attack_input)
        {
            attack_count = 1;
        }
        else if (attack_count == 1 && attack_input && !is_checking_attack_input)
        {
            attack_count = 2;
        }
        else if (attack_count == 2 && attack_input && !is_checking_attack_input)
        {
            attack_count = 3;
        }
        else if (attack_count == 3)
        {

        }

        if (!attack_input && !is_checking_attack_input)
        {
            attack_count = 0;
            is_attacking = false;
        }
    }
    
    void CheckAttackInput()
    {
        if (Input.GetKey(KeyCode.J) && is_checking_attack_input)
        {
            attack_input = true;
            is_checking_attack_input = false;
        }
    }

    public void BeginCheckInput()
    //call this method at the first frame of attack animation
    {
        is_checking_attack_input = true;
        is_attacking = true;
        Debug.Log("begin_check");
    }

    public IEnumerator EndCheckInput()
    //call this method at the last frame of attack animation
    {
        is_attacking = false;
        yield return new WaitForSeconds(attack_waiting_time);
        is_checking_attack_input = false;
    }
}
