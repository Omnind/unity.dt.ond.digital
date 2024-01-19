using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HslCommunication;
using HslCommunication.Profinet.Siemens;
using DG.Tweening;



public class PLC_Unity_2 : MonoBehaviour
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
        //上升
        if (siemensTcpNet.ReadBool("I0.2").Content == true)
        {
            //关闭停止
            if (siemensTcpNet.ReadBool("I0.3").Content == false)
            {
                Debug.Log("上升");
                transform.DOLocalMove(new Vector3(0, 0, 0), 3);
            }
        }
        else
        {
            //下降
        if (siemensTcpNet.ReadBool("Q0.5").Content == true)
        {
            //关闭停止
            if (siemensTcpNet.ReadBool("I0.3").Content == false)
            {
                Debug.Log("下降");
                transform.DOLocalMove(new Vector3(0, -0.15f, 0), 3);
            }
        }
        
        }
    }
   public void OpenZZ()
    {
        
        Stop();
        siemensTcpNet.Write("I0.2",true);
        siemensTcpNet.Write("I0.3",false);
    }
    public void Stop()
    {
        siemensTcpNet.Write("Q0.5",false);
        siemensTcpNet.Write("I0.2",false);
        siemensTcpNet.Write("I0.3",true);
    }
    public void OpenFZ()
    {
        
        Stop();
        siemensTcpNet.Write("Q0.5",true);
        siemensTcpNet.Write("I0.3",false);
    }
}