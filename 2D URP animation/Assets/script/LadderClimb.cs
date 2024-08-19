using UnityEngine;

public class LadderClimb : MonoBehaviour
{
    public float climbSpeed = 2f; // 攀爬速度
    private bool isClimbing = false; // 是否正在攀爬
    private Transform ladder; // 当前所在的梯子对象
    private float verticalInput = 0f; // 垂直输入值

    void Start()
    {
        // 初始化
        isClimbing = false;
        ladder = null;
        verticalInput = 0f;
    }
    void Update()
    {
        Debug.Log("Update" + isClimbing);
        // 检测玩家是否在梯子区域
        if (isClimbing)
        {
            // 获取垂直输入值
            verticalInput = Input.GetAxis("Vertical");

            // 根据输入方向移动玩家
            if (verticalInput > 0)
            {
                // 向上攀爬
                transform.position += Vector3.up * climbSpeed * Time.deltaTime;
            }
            else if (verticalInput < 0)
            {
                // 向下攀爬
                transform.position += Vector3.down * climbSpeed * Time.deltaTime;
            }
            else
            {
                // 停在当前位置
                // 不做任何移动操作
            }
        }
    }

    // void OnTriggerEnter(Collider other)
    // {
    //     // 检测玩家是否进入梯子区域
    //     Debug.Log("OnTriggerEnter");
    //     if (other.CompareTag("Ladder"))
    //     {
    //         isClimbing = true;
    //         ladder = other.transform;
    //     }
    // }
    void OnTriggerStay2D(Collider2D other)
    {   
        Debug.Log("OnTriggerStay");
        // 检测玩家是否进入梯子区域
        if (other.CompareTag("Ladder"))
        {
            isClimbing = true;
            ladder = other.transform;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("OnTriggerExit");
        // 检测玩家是否离开梯子区域
        if (other.CompareTag("Ladder"))
        {
            isClimbing = false;
            ladder = null;
            verticalInput = 0f; // 重置垂直输入值
        }
    }
}