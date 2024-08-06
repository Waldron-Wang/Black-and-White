using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseMovement : MonoBehaviour
{
    public float speed = 2f; // �ƶ��ٶ�
    public float distance = 2f; // �����ƶ��ľ���

    private Vector3 startPosition; // ��ʼλ��
    private bool movingRight = true; // ��ǰ�Ƿ������ƶ�

    void Start()
    {
        // ��¼�������ʼλ��
        startPosition = transform.position;
    }

    void Update()
    {
        // �����ƶ���Χ
        float newXPosition = transform.position.x;

        if (movingRight)
        {
            newXPosition += speed * Time.deltaTime;
            if (newXPosition >= startPosition.x + distance)
            {
                movingRight = false; // �����Ҳ�߽磬��ʼ�����ƶ�
            }
        }
        else
        {
            newXPosition -= speed * Time.deltaTime;
            if (newXPosition <= startPosition.x - distance)
            {
                movingRight = true; // �������߽磬��ʼ�����ƶ�
            }
        }

        // ��������λ��
        transform.position = new Vector3(newXPosition, transform.position.y, transform.position.z);
    }
}
