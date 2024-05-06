using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class sword_shield : MonoBehaviour
{
    black_controller black_Controller;
    Animator animator;

    const string animation_attack_1 = "attack_1";
    const string animation_attack_2 = "attack_2";
    const string animation_attack_3 = "attack_3";


    float attack_cooling_time;
    float attack_waiting_time;
    int attack_count;
    //0:not attacking   1:attack_1   2:attack_2   3:attack_3
    bool attack_input;
    //标示玩家是否有攻击输入
    bool is_attacking;
    //是否处于攻击状态,用于与controller脚本通信
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

    // Update is called once per frame
    void Update()
    {

    }

    void AttackStateMachine()
    {
        if (attack_count == 0 && attack_input && !is_checking_attack_input)
        {
            attack_count = 1;
            is_attacking = true;
        }
        else if (attack_count == 1 && attack_input && !is_checking_attack_input)
        {
            attack_count = 2;
        }
        else if (attack_count == 2 && attack_input && !is_checking_attack_input)
        {
            attack_count = 3;
        }
        else if (attack_count == 3){

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

    void BeginCheckInput()
    {
        is_checking_attack_input = true;
    }

    IEnumerator EndCheckInput()
    {
        yield return new WaitForSeconds(attack_waiting_time);
        is_checking_attack_input = false;
    }
}
