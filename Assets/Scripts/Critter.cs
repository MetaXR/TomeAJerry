using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//脚本：蜗牛的控制类
public class Critter : MonoBehaviour
{
    private Animator anim;
    private NavMeshAgent agent;
    public Transform player;
    private State currentState;
    public CritterSO critterSO;//蜗牛的数据文件    
    public float currentHealth=1000.0f;//蜗牛的生命值1000
    public float damage=100.0f;//蜗牛的伤害值100
    public DropItem critterItem = default;//蜗牛死亡后的掉落物品
    public bool IsDead = default;
    // Start is called before the first frame update
    void Start()
    {
        anim=this.GetComponent<Animator>();
        agent=this.GetComponent<NavMeshAgent>();
        //新建一个站立状态，让角色默认进入站立状态
        currentState=new Idle(this.gameObject,agent,anim,player);
    }

    // Update is called once per frame
    void Update()
    {
        //在角色控制脚本中，调用蜗牛的当前状态的执行函数Procress()
        //让角色在当前状态中运行起来，并返回下一个状态
        currentState=currentState.Process();
    }
    /// <summary>
    /// 触发器函数：当碰到蜗牛的碰撞体other是武器时，则判定蜗牛被攻击了
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        Weapon playerWeapon = other.GetComponent<Weapon>();
        if (playerWeapon != null)
        {
            ReceiveAnAttack(playerWeapon.AttackStrength);
        }
    }
    /// <summary>
    /// 被攻击函数：当蜗牛被攻击1次时，蜗牛的生命值减少damage
    /// </summary>
    /// <param name="damage">伤害值</param>
    void ReceiveAnAttack(int damage)
    {
        //1.使用数据文件中蜗牛的生命值
        critterSO.MaxHealth -= damage;
        //角色的生命值小于0，即死亡时，调用角色死亡事件函数OnCritterDead()
        if (critterSO.MaxHealth < 0)
        {
            IsDead = true;
            EventManager.Instance.OnCritterDead(1);
            CritterIsDeath();
        }
    }
    /// <summary>
    /// 死亡函数：当蜗牛的生命值小于零时，在蜗牛的右前方实例化一个掉落物品，同时销毁蜗牛
    /// </summary>
    public void CritterIsDeath()
    {
        float randPosRight = Random.value * 2 - 1.0f;
        float randPosForward = Random.value * 2 - 1.0f;
        GameObject prefab = critterSO.GetRandomItem().Prefab;
        //2.使用数据文件中的随机函数随机一个掉落物品
        GameObject collectableItem = GameObject.Instantiate(prefab,
                                                transform.position+2*(randPosRight*transform.right+randPosForward*transform.forward),
                                                transform.rotation);
        collectableItem.GetComponent<DropItemController>().CurrentItem = critterItem;
        GameObject.Destroy(this.gameObject);
    }
}
