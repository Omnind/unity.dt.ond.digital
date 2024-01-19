using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HslCommunication;
using HslCommunication.Profinet.Siemens;
public class PLC_Unity : MonoBehaviour
{
    //实例化
    SiemensS7Net siemensTcpNet = new SiemensS7Net(SiemensPLCS.S1500,"192.168.0.17")
    {
         ConnectTimeOut = 5000
    };
    public Vector3 direction=Vector3.back;//传送带传送方向
    public float movespeed=0.1f;//传送速度 速度过快会飞出去
    private Rigidbody _rig;
      // Start is called before the first frame update
      
    
    


    void Start()
    {
          _rig = GetComponent<Rigidbody>();
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
   
     private void FixedUpdate()
    {
        //正向启动
        if (siemensTcpNet.ReadBool("I1.0").Content == true)
        {
            //关闭停止
            if (siemensTcpNet.ReadBool("I1.1").Content == false)
            {
                Debug.Log("电机正转开始");
                //电机正转代码
                 Vector3 pos = _rig.position;
                 Vector3 temp = -direction.normalized * movespeed * Time.fixedDeltaTime;//实际移动方向与direction相反这里取反
                 _rig.position += temp;
                 _rig.MovePosition(pos);
                 
            }
        }
        else
        {
            //反向启动
        if (siemensTcpNet.ReadBool("Q1.6").Content == true)
        {
            //关闭停止
            if (siemensTcpNet.ReadBool("I1.1").Content == false)
            {
                Debug.Log("电机反转开始");
                //电机正转代码
                 Vector3 pos = _rig.position;
                 Vector3 temp = direction.normalized * movespeed * Time.fixedDeltaTime;//实际移动方向与direction相反这里取反
                 _rig.position += temp;
                 _rig.MovePosition(pos);
                 
            }
        }
        
        }
    }
    //电机正转开始
    public void OpenZZ()
    {
        //正转开始之前需要先停止其他程序
        Stop();
        //向PLC传送数据地址为I0.0的程序打开，把地址为I0.1的程序关闭
        siemensTcpNet.Write("I1.0",true);
        siemensTcpNet.Write("I1.1",false);
    }


    //停止
    public void Stop()
    {
        siemensTcpNet.Write("Q1.6",false);
        siemensTcpNet.Write("I1.0",false);
        siemensTcpNet.Write("I1.1",true);
    }

    //电机反转开始
    public void OpenFZ()
    {
        //正转开始之前需要先停止其他程序
        Stop();
        //向PLC传送数据地址为I0.0的程序打开，把地址为I0.1的程序关闭
        siemensTcpNet.Write("Q1.6",true);
        siemensTcpNet.Write("I1.1",false);
    }
}
