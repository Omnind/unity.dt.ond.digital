using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HslCommunication;
using HslCommunication.Profinet.Siemens;
using DG.Tweening;

public class PLC_zuojiazhua : MonoBehaviour
{
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
       //夹紧
        if (siemensTcpNet.ReadBool("I0.4").Content == true)
        {
            //关闭停止
            if (siemensTcpNet.ReadBool("I0.5").Content == false)
            {
                Debug.Log("夹紧");
                transform.DOLocalRotate(new Vector3(0, 10, 0), 3);
            }
        }
        else
        {
            //下降
        if (siemensTcpNet.ReadBool("Q1.0").Content == true)
        {
            //关闭停止
            if (siemensTcpNet.ReadBool("I0.5").Content == false)
            {   
               
                Debug.Log("放松");
                transform.DOLocalRotate(new Vector3(0, 0, 0), 3);
            }
        }
        
        }
        
    }
     public void OpenZZ()
    {
        
        Stop();
        siemensTcpNet.Write("I0.4",true);
        siemensTcpNet.Write("I0.5",false);
    }
    public void Stop()
    {
        siemensTcpNet.Write("Q1.0",false);
        siemensTcpNet.Write("I0.4",false);
        siemensTcpNet.Write("I0.5",true);
    }
    public void OpenFZ()
    {
        
        Stop();
        siemensTcpNet.Write("Q1.0",true);
        siemensTcpNet.Write("I0.5",false);
    }
}

