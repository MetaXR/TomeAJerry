using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    /// <summary>
    /// 登录页面
    /// </summary>
    public GameObject Panel_Login;
    /// <summary>
    /// 背包页面
    /// </summary>
    public GameObject Panel_Bag;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// 游戏开始
    /// </summary>
    public void StartGame()
    {
        Panel_Login.SetActive(false);
    }
    /// <summary>
    /// 点击背包按钮,打开背包页面。
    /// </summary>
    public void OnBagBtnClick()
    {
        Panel_Bag.SetActive(true);
    }
}
