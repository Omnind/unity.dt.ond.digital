using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using HslCommunication;
using HslCommunication.Profinet.Siemens;
 
public class guanlian: MonoBehaviour
{
     SiemensS7Net siemensTcpNet = new SiemensS7Net(SiemensPLCS.S1500,"192.168.0.17")
    {
         ConnectTimeOut = 5000
    };
    [SerializeField] Transform targetTrans;
 
     void Start()
    {
        ParentConstraint parentConstraint = gameObject.GetComponent<ParentConstraint>();
        
        if (siemensTcpNet.ReadBool("I0.4").Content == true)
        {
            
            if (siemensTcpNet.ReadBool("I0.5").Content == false)
            {
                parentConstraint = gameObject.AddComponent<ParentConstraint>();
 
                //设置组件影响权重
                parentConstraint.weight = 1;
  
                //目标对象权重为0时的参数
                parentConstraint.translationAtRest = Vector3.zero;
                parentConstraint.rotationAtRest = Vector3.zero;
 
                //冻结轴向，自动关联目标对象
                parentConstraint.translationAxis = Axis.X | Axis.Y | Axis.Z;
                parentConstraint.rotationAxis = Axis.X | Axis.Y | Axis.Z;
                //Debug.Log("夹紧");
                //添加目标对象
                ConstraintSource constraintSource = new ConstraintSource() { sourceTransform = targetTrans, weight = 1 };
                parentConstraint.SetSources(new List<ConstraintSource>() { constraintSource });
                //设置相对偏移量
                parentConstraint.SetTranslationOffset(0, Vector3.zero);
                parentConstraint.SetRotationOffset(0, Vector3.zero);
 
                //激活组件
                parentConstraint.constraintActive = true;
            }
        }
        if (siemensTcpNet.ReadBool("Q1.0").Content == true)
        {
            //关闭停止
            if (siemensTcpNet.ReadBool("I0.5").Content == false)
            {
                //Debug.Log("放松");
                 //情况目标
                parentConstraint.SetSources(new List<ConstraintSource>());
                //取消激活
                parentConstraint.constraintActive = false;
            }
        }
 
    
        
    }
 
 
   void Update()
    {   

    }
}
