using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HslCommunication;
using HslCommunication.Profinet.Siemens;
using DG.Tweening;

public class PLC_Unity_1 : MonoBehaviour
{
    //实例化
    SiemensS7Net siemensTcpNet = new SiemensS7Net(SiemensPLCS.S1500,"192.168.0.17")
    {
         ConnectTimeOut = 5000
    };
  
    


    void Start()
    {
              //连接PLC
        OperateResult connect = siemensTcpNet.ConnectServer();
        //判断是否连接成功
        if (connect.IsSuccess)
        {
            Debug.Log("连接成功！");
        }
        else
        {
            Debug.Log("连接失败！请输入正确的IP地址");
        }
    }

     void Update()
    {
        //正向启动
        if (siemensTcpNet.ReadBool("I0.0").Content == true)
        {
            //关闭停止
            if (siemensTcpNet.ReadBool("I0.1").Content == false)
            {
                Debug.Log("右移");
                transform.DOLocalMoveX(0.2f,3); //脚本物体3秒从当前位置本地坐标X轴移动
                
            }
        }
        else
        {
            //反向启动
        if (siemensTcpNet.ReadBool("Q0.2").Content == true)
        {
            //关闭停止
            if (siemensTcpNet.ReadBool("I0.1").Content == false)
            {
                Debug.Log("左移");
                transform.DOLocalMoveX(0,3);
            }
        }
        
        }
    }
    public void OpenZZ()
    {
        
        Stop();
        siemensTcpNet.Write("I0.0",true);
        siemensTcpNet.Write("I0.1",false);
    }
    public void Stop()
    {
        siemensTcpNet.Write("Q0.2",false);
        siemensTcpNet.Write("I0.0",false);
        siemensTcpNet.Write("I0.1",true);
    }
    public void OpenFZ()
    {
        
        Stop();
        siemensTcpNet.Write("Q0.2",true);
        siemensTcpNet.Write("I0.1",false);
    }
}
