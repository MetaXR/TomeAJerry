using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 猫控制脚本
/// </summary>
public class MaoController : MonoBehaviour
{
    public Rigidbody rBody;
    public float speed;
    Vector3 controlSignal = Vector3.zero;

    public Transform Target0;//一级老鼠
    public Text textCoin;//金币文本
    public int coinNum;//金币数量

    public Transform Target1;//二级老鼠
    public Text textBlue;//蓝钻文本
    public int blueNum;//蓝钻数量

    public Transform Target2;//三级老鼠
    public Text textRed;//红钻文本
    public int redNum;//红钻数量

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// 重置猫咪的状态
    /// </summary>
    public void GameReset()
    {
        if (this.transform.position.y < 0)
        {
            // If the Agent fell, zero its momentum
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.position = new Vector3(0, 0, 0);
        } 
    }

    /// <summary>
    /// 猫捕捉不同级别老鼠，不同的奖励各自增加
    /// 捕捉1级老鼠，金币增加；
    /// 捕捉2级老鼠，蓝钻增加；
    /// 捕捉3级老鼠，红钻增加；
    /// </summary>
    // Update is called once per frame
    void FixedUpdate()
    {     
        
        controlSignal.x = Input.GetAxis("Horizontal");
        controlSignal.z = Input.GetAxis("Vertical");
        rBody.AddForce(controlSignal * speed);

        // 捉到一级老鼠，金币加1
        float distanceToTarget0 = Vector3.Distance(this.transform.position,
                                                  Target0.position);
        // Reached target
        if (distanceToTarget0 < 1.42f)
        {
            GameReset();
            // 1级老鼠被捉到后，随机一个位置，逃跑到这个位置
            Target0.position = new Vector3(Random.value * 8 - 4,
                                          0.5f,
                                          Random.value * 8 - 4);
            coinNum++;
            textCoin.text = coinNum.ToString();
        }
        // 捉到二级老鼠，蓝钻加1
        float distanceToTarget1 = Vector3.Distance(this.transform.position,
                                                  Target1.position);
        // Reached target
        if (distanceToTarget1 < 1.42f)
        {
            GameReset();
            // 2级老鼠被捉到后，随机一个位置，逃跑到这个位置
            Target1.position = new Vector3(Random.value * 8 - 4,
                                          0.5f,
                                          Random.value * 8 - 4);
            blueNum++;
            textBlue.text = coinNum.ToString();
        }
        // 捉到三级老鼠，红钻加1
        float distanceToTarget2 = Vector3.Distance(this.transform.position,
                                                  Target2.position);
        // Reached target
        if (distanceToTarget2 < 1.42f)
        {
            GameReset();
            // 3级老鼠被捉到后，随机一个位置，逃跑到这个位置
            Target2.position = new Vector3(Random.value * 8 - 4,
                                          0.5f,
                                          Random.value * 8 - 4);
            redNum++;
            textRed.text = coinNum.ToString();
        }

        // Fell off platform
        if (this.transform.position.y < 0)
        {
            GameReset();
        }
    }


}
