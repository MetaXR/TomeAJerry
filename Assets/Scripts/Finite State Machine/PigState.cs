using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PigState
{
    //游戏主角的状态 
    public enum STATE
    {
        IDLE,
        WALK,
        ATTACK
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
    protected Animator anim;//动画控制器
    protected Pig pigScript;//主角的控制脚本
    protected Transform player;//游戏主角
    public PigState nextState;//下一个状态

    protected float walkVelocity = 0.1f;//开始行走

    public PigState(Animator _anim, Pig _pigScript, Transform _player)
    {
        anim = _anim;
        pigScript = _pigScript;
        player = _player;
        stage = EVENT.ENTER;
    }

    public virtual void Enter() { stage = EVENT.UPDATE; }
    public virtual void Update() { stage = EVENT.UPDATE; }
    public virtual void Exit() { stage = EVENT.EXIT; }

    //状态机的执行过程
    public PigState Process()
    {
        if (stage == EVENT.ENTER) Enter();
        if (stage == EVENT.UPDATE) Update();
        if (stage == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }
        return this;
    }
}

// ===============站立状态Idle====================
public class PigIdle : PigState
{
    public PigIdle(Animator _anim, Pig _pigScript, Transform _player)
                 : base(_anim, _pigScript, _player)
    {
        name = STATE.IDLE;
        anim = _anim;
        pigScript = _pigScript;
        player = _player;
    }
    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        if (pigScript.IsWalking)
        {
            nextState = new PigWalk(anim, pigScript, player);
            stage = EVENT.EXIT;
        }
        else if (pigScript.attackInput)
        {
            nextState = new PigAttack(anim, pigScript, player);
            stage = EVENT.EXIT;
        }
    }
    public override void Exit()
    {
        base.Exit();
    }
}


//======行走状态PigWalk======
public class PigWalk : PigState
{
    public PigWalk(Animator _anim, Pig _pigScript, Transform _player)
                 : base(_anim, _pigScript, _player)
    {
        name = STATE.WALK;
        anim = _anim;
        pigScript = _pigScript;
        player = _player;
    }
    public override void Enter()
    {
        anim.speed = 1.0f;
        anim.SetBool("IsWalking", true);
        base.Enter();
    }
    public override void Update()
    {
        //行走动画的速度，由角色的移动向量的大小决定。
        anim.speed = pigScript.movementInput.magnitude;

        if (!pigScript.IsWalking)
        {
            nextState = new PigIdle(anim, pigScript, player);
            stage = EVENT.EXIT;
        }
        else if (pigScript.attackInput)
        {
            nextState = new PigAttack(anim, pigScript, player);
            stage = EVENT.EXIT;
        }
    }
    public override void Exit()
    {
        anim.speed = 1.0f;
        anim.SetBool("IsWalking", false);
        base.Exit();
    }
}

//======攻击状态PigAttack======
public class PigAttack : PigState
{
    public PigAttack(Animator _anim, Pig _pigScript, Transform _player)
                 : base(_anim, _pigScript, _player)
    {
        name = STATE.ATTACK;
        anim = _anim;
        pigScript = _pigScript;
        player = _player;
        //攻击动画整体提高至原来的1.6倍。
        anim.speed = 1.6f;
    }
    public override void Enter()
    {
        anim.SetTrigger("CaneHit");
        base.Enter();
    }
    public override void Update()
    {
        if (!pigScript.IsWalking)
        {
            nextState = new PigIdle(anim, pigScript, player);
            stage = EVENT.EXIT;
        }
        else
        {
            nextState = new PigWalk(anim, pigScript, player);
            stage = EVENT.EXIT;
        }
    }
    public override void Exit()
    {
        pigScript.attackInput = false;
        base.Exit();
    }
}