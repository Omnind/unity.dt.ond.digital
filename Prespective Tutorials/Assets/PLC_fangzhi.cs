using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HslCommunication;
using HslCommunication.Profinet.Siemens;
using DG.Tweening;


public class PLC_fangzhi : MonoBehaviour
{
     //实例化
    SiemensS7Net siemensTcpNet = new SiemensS7Net(SiemensPLCS.S1500,"192.168.0.17")
    {
         ConnectTimeOut = 5000
    };
    // Start is called before the first frame update
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

    // Update is called once per frame
    void Update()
    {
        //伸出
        if (siemensTcpNet.ReadBool("I0.6").Content == true)
        {
            //关闭停止
            if (siemensTcpNet.ReadBool("I0.7").Content == false)
            {
                Debug.Log("伸出");
                transform.DOLocalMove(new Vector3(0.415f, 0.351f, 0.352f),3); 
            }
        }
        else
        {
            //收回
        if (siemensTcpNet.ReadBool("Q1.3").Content == true)
        {
            //关闭停止
            if (siemensTcpNet.ReadBool("I0.7").Content == false)
            {
                Debug.Log("收回");
                transform.DOLocalMove(new Vector3(0.415f, 0.351f, 0.27f),3);
            }
        }
        
        }
    }
     public void OpenZZ()
    {
        
        Stop();
        siemensTcpNet.Write("I0.6",true);
        siemensTcpNet.Write("I0.7",false);
    }
    public void Stop()
    {
        siemensTcpNet.Write("Q1.3",false);
        siemensTcpNet.Write("I0.6",false);
        siemensTcpNet.Write("I0.7",true);
    }
    public void OpenFZ()
    {
        
        Stop();
        siemensTcpNet.Write("Q1.3",true);
        siemensTcpNet.Write("I0.7",false);
    }
}
