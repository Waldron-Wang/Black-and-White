using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sword_shield : MonoBehaviour
{
    Animator animator;

    bool attack_input;
    //标示玩家是否有攻击输入
    bool is_attacking;
    //是否处于攻击状态
    
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();

        attack_input = false;
        is_attacking = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerable attack()
    {
        check_attack_input();
        
    }
    
    void check_attack_input()
    {
        if (Input.GetKeyDown(KeyCode.J) && is_attacking == false)
        {
            attack_input = true;
            is_attacking = true;
        }
    }
}
