using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class move : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
         transform.DOLocalMoveX(0.2f,3); //脚本物体3秒从当前位置本地坐标X轴移动
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
