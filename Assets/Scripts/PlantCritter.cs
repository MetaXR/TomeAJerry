using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlantCritter : MonoBehaviour
{
    public CritterSO critterSO;

    private int currentHealth = default;
    private float currentWaitTime = default;
    private Vector3 startPosition = default;
    public Vector3 roamingPosTarget = default;
    private NavMeshAgent agent = default;
    private bool agentActiveOnNavMesh = default;
    private Animator anim = default;

    public Transform Player;
    public bool isPlayerInAlertZone { get; set; }
    public bool isPlayerInAttackZone { get; set; }
    public bool getHit { get; set; }
    public bool isDead { get; set; }
    public bool isRoaming { get; set; }

    private PState currentState;

    void Awake()
    {
        currentHealth = critterSO.MaxHealth;
        currentWaitTime = critterSO.WaitTime;
        startPosition = transform.position;
        roamingPosTarget = GetRoamingPosition();
        agent = GetComponent<NavMeshAgent>();
        agentActiveOnNavMesh = agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh;
        anim = GetComponent<Animator>();

        currentState = new PIdle(this.gameObject, agent, anim, Player);
    }
   
    // Update is called once per frame
    void Update()
    {
        currentState = currentState.Process();

        if (agentActiveOnNavMesh)
        {
            isRoaming = false;

            if (isDead)
            {
                agent.isStopped = true;
            }
            else if (isPlayerInAlertZone)
            {
                agent.speed = critterSO.ChasingSpeed;
                agent.SetDestination(Player.position);

                if (isPlayerInAttackZone)
                {
                    agent.isStopped = true;
                }
                else
                {
                    agent.isStopped = false;
                }

            }
            else
            {
                agent.isStopped = false;
                agent.speed = critterSO.RoamingSpeed;
                agent.SetDestination(roamingPosTarget);
                if (!agent.hasPath)
                {
                    currentWaitTime -= Time.deltaTime;
                    if (currentWaitTime < 0)
                    {
                        roamingPosTarget = GetRoamingPosition();
                        currentWaitTime = critterSO.WaitTime;
                    }
                }
                else
                {
                    isRoaming = true;
                }
            }
        }
    }
    
    private Vector3 GetRoamingPosition()
    {
        return startPosition + new Vector3(Random.Range(-1, 1), 0.0f, Random.Range(-1, 1)).normalized * Random.Range(critterSO.RoamingDistance/2,critterSO.RoamingDistance);
    }

    private void OnTriggerEnter(Collider other)
    {
        Weapon playerWeapon = other.GetComponent<Weapon>();
        if (!getHit && playerWeapon!=null && playerWeapon.Enable)
        {
            ReceiveAnAttack(playerWeapon.AttackStrength);
        }
    }
    /// <summary>
    /// 被攻击函数：当食人花被攻击1次时，食人花的生命值减少damage
    /// </summary>
    /// <param name="damage">伤害值</param>
    private void ReceiveAnAttack(int damge)
    {
        currentHealth -= damge;
        //角色生命值currentHealth小于0，即角色死亡时，调用角色死亡事件函数OnCritterDead()
        if (currentHealth < 0)
        {
            isDead = true;
            EventManager.Instance.OnCritterDead(2);
            CritterIsDeath();
        }
    }

    public void CritterIsDeath()
    {       
        DropItem ditem = critterSO.GetRandomItem();

        float randPosRight = Random.value * 2 - 1.0f;
        float randPosForward = Random.value * 2 - 1.0f;

        GameObject collectableItem = GameObject.Instantiate(ditem.Prefab, 
                                     transform.position + 2 * (randPosRight * Vector3.right + randPosForward * Vector3.forward), 
                                     transform.rotation);

        GameObject.Destroy(this.gameObject);
    }
}
