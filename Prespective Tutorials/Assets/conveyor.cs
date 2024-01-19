using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class conveyor : MonoBehaviour
{
    
   private Vector3 offset;
    public float speed = 6;
    void Start () {
        offset = GameObject.Find("Conveyer/target").transform.position.normalized;
    }
    
    public void OnCollisionStay(Collision other)
    {
          other.transform.Translate(speed * Time.deltaTime*offset,Space.World);
        //    other.transform.Translate (new Vector3(speed * Time.deltaTime * offset.x, 0, speed * Time.deltaTime * offset.z));//默认沿着物体的z轴移动，即为前后方向
        //  other.transform.Translate(other.gameObject.transform.localPosition*offset*speed * Time.deltaTime);
        //    Debug.Log("触发");
        //    Debug.Log(other.gameObject.name);
        //移向的方法（可用），存在卡顿的情况
        //   other.gameObject.transform.position= Vector3.MoveTowards(other.gameObject.transform.position, offset, speed * Time.deltaTime);//移向
    }
    float x;
    float y;
    float scrollX = 1f;//用来调节贴图移动的速度,可适当调大或缩小
 void Update ()
    {
        x = x + Time.deltaTime * scrollX;
        GetComponent<Renderer>().material.mainTextureOffset = new Vector2(x, y);
        
        GetComponent<Renderer>().material.SetTextureOffset("wallhaven", new Vector2(x, y));
    }
}
