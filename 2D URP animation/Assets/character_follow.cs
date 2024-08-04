using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follow_player : MonoBehaviour
{
    public Transform player; // 定义一个 Transform 类型的变量来存储人物的位置

    // Start is called before the first frame update
    void Start()
    {
        // 初始化 player 变量
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 更新摄像头的位置，使其跟随人物的位置
        if (player != null)
        {
            transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
        }
    }
}