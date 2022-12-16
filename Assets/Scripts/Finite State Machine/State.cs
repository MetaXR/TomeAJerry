using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 蜗牛状态机将用于实现蜗牛NPC的各种行为，这些行为包括巡逻，追逐和攻击。
public class State
{
   //蜗牛的状态 
   public enum STATE
   {
        IDLE,
        PATROL,
        CHASE,
        ATTACK,
        DEATH
   };
   //状态的3个阶段 
   public enum EVENT
   {
       ENTER,
       UPDATE,
       EXIT
   };   
   public STATE name;//当前状态    
   public EVENT stage;//状态的当前阶段  
   protected GameObject npc;//NPC
   protected Animator anim;//动画控制器
   protected NavMeshAgent agent;//代理机器人
   protected Transform player;//游戏主角-玩家控制
   public State nextState;//下一个状态
   
   float visDist=10.0f;//NPC的可视距离
   float visAngle=30.0f;//NPC的可见角度
   float attcakDist=0.8f;//NPC的攻击距离

   public State(GameObject _npc,NavMeshAgent _agent,Animator _anim,Transform _player)
   {
       npc=_npc;
       agent=_agent;
       anim=_anim;
       player=_player;
       stage=EVENT.ENTER;
   }

   public virtual void Enter(){stage=EVENT.UPDATE;}
   public virtual void Update(){stage=EVENT.UPDATE;}
   public virtual void Exit(){stage=EVENT.EXIT;}
   
   //状态机的执行过程
   public State Process()
   {
       if(stage==EVENT.ENTER) Enter();
       if(stage==EVENT.UPDATE) Update();
       if(stage==EVENT.EXIT)
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
       if( direction.magnitude<visDist && angle<visAngle )
       {
           return true;
       }
       return false;
   }
   //NPC是否可以攻击玩家Player的判断函数
   public bool CanAttackPlayer()
   {
       Vector3 direction=player.position-npc.transform.position;

       if(direction.magnitude<attcakDist)
       {
           return true;
       }
       return false;
   }
}

// ===============站立状态Idle====================
public class Idle:State
{
    public Idle(GameObject _npc,NavMeshAgent _agent,Animator _anim,Transform _player)
                 : base(_npc,_agent,_anim,_player)
    {
        name=STATE.IDLE;
    }
    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        if(CanSeePlayer())
        {
            nextState=new Chase(npc,agent,anim,player);
            stage=EVENT.EXIT;
        }
        else if(Random.Range(0,100)<100.0f)
        {
            nextState=new Patrol(npc,agent,anim,player);
            stage=EVENT.EXIT;
        }        
    }
    public override void Exit()
    {
        base.Exit();
    }
}

// ===============巡逻状态Patrol====================
public class Patrol:State
{
    int currentIndex=-1;
    public Patrol(GameObject _npc,NavMeshAgent _agent,Animator _anim,Transform _player)
                 : base(_npc,_agent,_anim,_player)
    {
        name=STATE.PATROL;
        agent.speed=1.0f;
        agent.isStopped=false;
    }

    public override void Enter()
    {
        currentIndex=0;
        anim.SetBool("IsMoving",true);
        base.Enter();
    }

    public override void Update()
    {
        if(agent.remainingDistance<1)
        {
           if(currentIndex >= GameEnviroment.Instance.CheckPoints.Count-1)
           {
               currentIndex=0;
           }
           else
           {
               currentIndex++;
           }
           agent.SetDestination(GameEnviroment.Instance.CheckPoints[currentIndex].transform.position);
        }

        if(CanSeePlayer())
        {
            nextState=new Chase(npc,agent,anim,player);
            stage=EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        anim.SetBool("IsMoving",false);
        base.Exit();
    }
}

//===追逐状态Chase===
public class Chase:State
{
    public Chase(GameObject _npc,NavMeshAgent _agent,Animator _anim,Transform _player)
                 : base(_npc,_agent,_anim,_player)
    {
        name=STATE.CHASE;
        agent.speed=2.0f;
        agent.isStopped=false;
    }

    public override void Enter()
    {
        anim.SetBool("IsMoving",true);
        base.Enter();
    }

    public override void Update()
    {
        //把玩家位置设置为NPC追逐的目标位置
        agent.SetDestination(player.position);
        //判断导航是否计算出路线
        if(agent.hasPath)
        {
            //当游戏主角进入了NPC的可攻击范围，NPC进入攻击状态
            if(CanAttackPlayer())
            {
                nextState=new Attack(npc,agent,anim,player);
                stage=EVENT.EXIT;
            }
            else if(!CanSeePlayer())//当游戏主角逃出了NPC的可见范围，NPC进入巡逻状态。
            {
                nextState=new Patrol(npc,agent,anim,player);
                stage=EVENT.EXIT;
            }
        }
    }

    public override void Exit()
    {
        anim.SetBool("IsMoving",false);
        base.Exit();
    }
}

//===攻击状态Attack===
public class Attack:State
{
    float rotationSpeed=2.0f;
    AudioSource shoot;

    public Attack(GameObject _npc,NavMeshAgent _agent,Animator _anim,Transform _player)
                 : base(_npc,_agent,_anim,_player)
    {
        name=STATE.ATTACK;    
        // shoot=_npc.GetComponent<AudioSource>();    
    }

    public override void Enter()
    {
        agent.isStopped=true;//NPC停止寻路
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
        if(!CanAttackPlayer())
        {
            nextState=new Idle(npc,agent,anim,player);
            stage=EVENT.EXIT;
        }
        //死亡状态的入口点======如果NPC死亡了，则进入死亡状态。
        if (npc.GetComponent<Critter>().IsDead)
        {
            nextState = new Death(npc,agent,anim,player);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        anim.ResetTrigger("Attack");
        base.Exit();
    }
}

//===死亡状态Death===
public class Death : State
{
    public Death(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
                 : base(_npc, _agent, _anim, _player)
    {
        name = STATE.DEATH;
    }

    public override void Enter()
    {
        agent.isStopped = true;
        anim.SetTrigger("IsDead");
        base.Enter();
    }

    public override void Update()
    {
        stage = EVENT.EXIT;
    }

    public override void Exit()
    {
        anim.ResetTrigger("IsDead");
        npc.GetComponent<Critter>().CritterIsDeath();//调用脚本Critter中的CritterIsDeath，销毁NPC。
        base.Exit();
    }
}
