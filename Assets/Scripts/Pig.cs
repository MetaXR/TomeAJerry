using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//游戏主角小猪佩奇的控制脚本
public class Pig : MonoBehaviour
{    
    public int lifeNum = 2000;//小猪佩奇的生命值   
    public int damageNum = 200; //小猪佩奇的攻击值

    //下面字段，由脚本的输入控制层使用
    public float moveSpeed = 1.5f;
    public float rotateSpeed = 3f;
    public Vector3 moveAmount;//角色的帧移动量 
    Animator anim;//角色的动画组件
    public Transform mainCamera;    
    public VariableJoystick variableJoystick;//摇杆物体

    //下面字段，由状态机读取(These fields are read and manipulated by the StateMachine actions)
    public bool attackInput;//攻击状态变量
    public Vector3 movementInput; //移动向量
    public bool IsWalking;//行走状态变量
    public PigState currenState;//角色当前状态
    //玩家的金币数据
    public MoneySO pigMoney=default;
    void Awake()
    {
        //代码启动时，开始监听物品使用事件，事件发生时执行响应函数ReceiveAnItem()
        EventManager.Instance.useItemEvent += ReceiveAnItem;
        //代码启动时，开始监听敌人死亡事件，并且敌人死亡时执行响应函数AddMoney()
        EventManager.Instance.critterDeadEvent += AddMoney;
    }
    void OnDestroy()
    {
        //代码销毁后，不再监听物品使用事件
        EventManager.Instance.useItemEvent -= ReceiveAnItem;
        //代码销毁后，不再监听敌人死亡事件
        EventManager.Instance.critterDeadEvent -= AddMoney;
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        //初始化状态机
        currenState = new PigIdle(anim,this.GetComponent<Pig>(),this.transform);
    }
    // Update is called once per frame
    void Update()
    {
        
        //选取摄像机作为角色移动的参考系
        movementInput = mainCamera.forward * variableJoystick.Vertical + mainCamera.right * variableJoystick.Horizontal;
        movementInput.y = 0;
        //每帧的移动量 = 速度方向的单位向量(移动方向) * 速度大小(移动的快慢) * 时间平滑参数
        //moveAmount = movementInput * Time.deltaTime;
        //对角色的移动向量进行大小设置，让其最大不超过1.
        movementInput = Vector3.ClampMagnitude(movementInput,1);
        moveAmount = movementInput * Time.deltaTime;

        //目标朝向计算 = 从 猫的Z轴 转向 移动方向 产生的旋转值 * 猫的旋转
        Quaternion targetRot = Quaternion.FromToRotation(transform.forward, movementInput) * transform.rotation;
        //猫的旋转 = 由 当前朝向 转向 目标朝向，转向过程进行中间线性插值 
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);

        //判定条件，
        //当移动量不等于零时，IsWalking为真；
        //当移动量等于零时，IsWalking为假。
        IsWalking = moveAmount != Vector3.zero;

        //给动画的切换参数IsWalking赋值
        //IsWalking为真时，角色在行走，播放行走动画。
        //IsWalking为假时，角色静止，播放站立动画。
        //anim.SetBool("IsWalking", IsWalking);
        //---状态机--执行函数
        currenState = currenState.Process();
    }

    private void FixedUpdate()
    {
        //每帧，游戏角色移动一个次，movementVector是移动向量
        transform.position = transform.position + moveAmount;
    }
    /// <summary>
    /// 小猪开始攻击
    /// </summary>
    /// <param name="CaneHit"></param>
    public void Attack( )
    {
        attackInput = true;
    }
    /// <summary>
    /// 使用物品之后的效果函数
    /// </summary>
    /// <param name="dropItem"></param>
    public void ReceiveAnItem(DropItem dropItem)
    {
        //吃颗苹果，满血复活
        lifeNum = dropItem.ItemType == BagItemType.food ? 2000 : lifeNum;    
    }
    /// <summary>
    /// 增加金币
    /// </summary>
    /// <param name="type">类型</param>
    public void AddMoney(int type)
    {
        //如果是蜗牛，金币加1
        pigMoney.Coin += type == 1 ? 1 : 0;
        //如果是食人花，蓝钻加1
        pigMoney.Diamond += type == 1 ? 0 : 1;
    }
}