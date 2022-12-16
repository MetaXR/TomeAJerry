using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///野猪NPC的状态机的简单实现
public class NPC : MonoBehaviour
{
    //NPC的默认状态-巡逻
    string State="PATROL";
    Animator anim;
    public Transform player;
    public float distance;
    void Start() 
    {
        anim=this.GetComponent<Animator>();
    }
    
    void Update()
    {
        //NPC与玩家的距离
        distance=-(player.position-this.transform.position).magnitude;
        //切换NPC的状态
        if(distance>20.0f)
        {
            State="PATROL";
        }
        if((distance<20.0f && distance>15.0f) || (distance<10.0f && distance>5.0f))//新增逃跑状态而导致的更改
        {
            State="CHASE";
        }
         if(distance<5.0f)//新增攻击状态
        {
            State="ATTACK";
        }
         if(distance>10.0f && distance <15.0f)//新增逃跑状态
        {
            State="RUNAWAY";
        }
        //判断NPC的当前状态
        if(State=="PATROL")
        {
            anim.SetTrigger("IsWalking");   
            transform.Translate(0,0,1.0f); //向前移动
            anim.speed=1.0f;        
        }
        else if(State=="CHASE")
        {
            anim.SetTrigger("IsRunninging");
            transform.Translate(0,0,1.0f);//向前移动
            anim.speed=2.0f;
        }
        else if(State=="ATTACK")//攻击状态的执行-播放攻击动画
        {
            anim.SetTrigger("InAttacking");
            anim.speed=1.0f;
        }
        else if(State=="RUNAWay")//攻击状态的执行-播放逃跑动画
        {
            anim.SetTrigger("IsRunning");
            transform.Translate(0,0,-1.0f);//向后移动
            anim.speed=1.0f;
        }
    }
}
