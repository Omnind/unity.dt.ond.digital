using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public Vector3 direction=Vector3.back;//传送带传送方向
    public float movespeed=0.1f;//传送速度 速度过快会飞出去
    private Rigidbody _rig;
    void Start()
    {
        _rig = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector3 pos = _rig.position;
        Vector3 temp = -direction.normalized * movespeed * Time.fixedDeltaTime;//实际移动方向与direction相反这里取反
        _rig.position += temp;
        _rig.MovePosition(pos);
    }

}
