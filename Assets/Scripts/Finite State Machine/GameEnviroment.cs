using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;//排序算法所需的引用库

//游戏环境类---单例类
public sealed class GameEnviroment
{
    private static GameEnviroment instance;
    private List<GameObject> checkPoints=new List<GameObject>();//巡逻路线点列表
    public List<GameObject> CheckPoints{ get{return checkPoints;} }

    public static GameEnviroment Instance
    {
        get
        {
            if(instance==null)
            {
                instance=new GameEnviroment();
                instance.checkPoints.AddRange(GameObject.FindGameObjectsWithTag("CheckPoint"));//找到场景中所有的带有CheckPoint标签的点
                instance.checkPoints=instance.checkPoints.OrderBy(waypoint=>waypoint.name).ToList();//排序算法：把路线点按照物体名字排序
            }
            return instance;
        }
    }    
}
