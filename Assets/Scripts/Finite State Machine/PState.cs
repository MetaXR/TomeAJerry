using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 蜗牛状态机将用于实现蜗牛NPC的各种行为，这些行为包括巡逻，追逐和攻击。
public class PState
{
   //蜗牛的状态 
   public enum PSTATE
   {
        IDLE,
        PATROL,
        CHASE,
        ATTACK
   };
   //状态的3个阶段 
   public enum PEVENT
   {
       ENTER,
       UPDATE,
       EXIT
   };   
   public PSTATE name;//当前状态    
   public PEVENT stage;//状态的当前阶段  
   protected GameObject npc;//NPC
   protected Animator anim;//动画控制器
   protected NavMeshAgent agent;//代理机器人
   protected Transform player;//游戏主角-玩家控制
   public PState nextState;//下一个状态
   
   //float visDist=10.0f;//NPC的可视距离
   //float visAngle=30.0f;//NPC的可见角度
   //float attcakDist=0.8f;//NPC的攻击距离

   public PState(GameObject _npc,NavMeshAgent _agent,Animator _anim,Transform _player)
   {
       npc=_npc;
       agent=_agent;
       anim=_anim;
       player=_player;
       stage=PEVENT.ENTER;
   }

   public virtual void Enter(){stage=PEVENT.UPDATE;}
   public virtual void Update(){stage=PEVENT.UPDATE;}
   public virtual void Exit(){stage=PEVENT.EXIT;}
   
   //状态机的执行过程
   public PState Process()
   {
       if(stage==PEVENT.ENTER) Enter();
       if(stage==PEVENT.UPDATE) Update();
       if(stage==PEVENT.EXIT)
       {
           Exit();
           return nextState;
       }
       return this;
   }

   //NPC是否能看见玩家Player的判定函数
   public bool CanSeePlayer()
   {
       Vector3 direction=player.position-npc.transform.position;
       float angle=Vector3.Angle(direction,npc.transform.forward);
       //可视化范围：一个视角60度、可视距离10米的视锥体
       //if( direction.magnitude<visDist && angle<visAngle )
       //{
       //    return true;
       //}
       return false;
   }
   //NPC是否可以攻击玩家Player的判断函数
   public bool CanAttackPlayer()
   {
       Vector3 direction=player.position-npc.transform.position;

       //if(direction.magnitude<attcakDist)
       //{
       //    return true;
       //}
       return false;
   }
}

// ===============站立状态Idle====================
public class PIdle:PState
{
    public PIdle(GameObject _npc,NavMeshAgent _agent,Animator _anim,Transform _player)
                 : base(_npc,_agent,_anim,_player)
    {
        name=PSTATE.IDLE;
    }
    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        if(npc.GetComponent<PlantCritter>().isPlayerInAlertZone)
        {
            nextState=new PChase(npc,agent,anim,player);
            stage=PEVENT.EXIT;
        }
        else if(Random.Range(0,100)<100.0f)
        {
            nextState=new PPatrol(npc,agent,anim,player);
            stage=PEVENT.EXIT;
        }        
    }
    public override void Exit()
    {
        base.Exit();
    }
}

// ===============巡逻状态Patrol====================
public class PPatrol:PState
{
    int currentIndex=-1;
    public PPatrol(GameObject _npc,NavMeshAgent _agent,Animator _anim,Transform _player)
                 : base(_npc,_agent,_anim,_player)
    {
        name=PSTATE.PATROL;
        //agent.speed=1.0f;
        //agent.isStopped=false;
    }

    public override void Enter()
    {
        currentIndex=0;
        base.Enter();
    }

    public override void Update()
    {
        //if(agent.remainingDistance<1)
        //{
        //   if(currentIndex >= GameEnviroment.Instance.CheckPoints.Count-1)
        //   {
        //       currentIndex=0;
        //   }
        //   else
        //   {
        //       currentIndex++;
        //   }
        //   agent.SetDestination(GameEnviroment.Instance.CheckPoints[currentIndex].transform.position);
        //}

        agent.SetDestination(npc.GetComponent<PlantCritter>().roamingPosTarget);



        if (npc.GetComponent<PlantCritter>().isPlayerInAlertZone)
        {
            nextState=new PChase(npc,agent,anim,player);
            stage=PEVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

//===追逐状态Chase===
public class PChase : PState
{
    public PChase(GameObject _npc,NavMeshAgent _agent,Animator _anim,Transform _player)
                 : base(_npc,_agent,_anim,_player)
    {
        name=PSTATE.CHASE;
        //agent.speed=2.0f;
        //agent.isStopped=false;
    }

    public override void Enter()
    {
        anim.SetBool("IsAlert",true);
        base.Enter();
    }

    public override void Update()
    {
        //把玩家位置设置为NPC追逐的目标位置
        agent.SetDestination(player.position);
        //判断导航是否计算出路线
        if (agent.hasPath)
        {
            //当游戏主角进入了NPC的可攻击范围，NPC进入攻击状态
            if (npc.GetComponent<PlantCritter>().isPlayerInAlertZone)
            {
                nextState = new PAttack(npc, agent, anim, player);
                stage = PEVENT.EXIT;
            }
            else if (!npc.GetComponent<PlantCritter>().isPlayerInAlertZone)//当游戏主角逃出了NPC的可见范围，NPC进入巡逻状态。
            {
                nextState = new PPatrol(npc, agent, anim, player);
                stage = PEVENT.EXIT;
            }
        }
    }

    public override void Exit()
    {
        //anim.SetBool("IsAlert", false);
        base.Exit();
    }
}

//===攻击状态Attack===
public class PAttack:PState
{
    float rotationSpeed=2.0f;
    AudioSource shoot;

    public PAttack(GameObject _npc,NavMeshAgent _agent,Animator _anim,Transform _player)
                 : base(_npc,_agent,_anim,_player)
     {
        name=PSTATE.ATTACK;    
        // shoot=_npc.GetComponent<AudioSource>();    
    }

    public override void Enter()
    {
        //agent.isStopped=true;//NPC停止寻路
        anim.SetTrigger("Attack");//播放攻击动作
        // shoot.Play();//播放攻击音效
        base.Enter();
    }

    public override void Update()
    {
        Vector3 direction=player.position-npc.transform.position;
        float angle=Vector3.Angle(npc.transform.forward,direction);
        //保证NPC一直面向游戏主角
        npc.transform.rotation=Quaternion.Slerp(npc.transform.rotation,
                                                Quaternion.LookRotation(direction),
                                                Time.deltaTime * rotationSpeed);
        //如果游戏主角逃出了NPC的可攻击范围，则NPC切换到初始状态：站立
        if(!npc.GetComponent<PlantCritter>().isPlayerInAttackZone)
        {
            nextState=new PIdle(npc,agent,anim,player);
            stage = PEVENT.EXIT;
        }                                    
    }

    public override void Exit()
    {
        anim.ResetTrigger("Attack");
        anim.SetBool("IsAlert", false);
        // shoot.Stop();
        base.Exit();
    }
}
