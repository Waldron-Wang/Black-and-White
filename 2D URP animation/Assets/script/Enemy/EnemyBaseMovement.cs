using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseMovement : MonoBehaviour
{
    public float speed = 2f; // 移动速度
    public float distance = 2f; // 来回移动的距离

    private Vector3 startPosition; // 起始位置
    private bool movingRight = true; // 当前是否向右移动

    void Start()
    {
        // 记录物体的起始位置
        startPosition = transform.position;
    }

    void Update()
    {
        // 计算移动范围
        float newXPosition = transform.position.x;

        if (movingRight)
        {
            newXPosition += speed * Time.deltaTime;
            if (newXPosition >= startPosition.x + distance)
            {
                movingRight = false; // 到达右侧边界，开始向左移动
            }
        }
        else
        {
            newXPosition -= speed * Time.deltaTime;
            if (newXPosition <= startPosition.x - distance)
            {
                movingRight = true; // 到达左侧边界，开始向右移动
            }
        }

        // 更新物体位置
        transform.position = new Vector3(newXPosition, transform.position.y, transform.position.z);
    }
}
