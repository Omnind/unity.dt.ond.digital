using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class pengzhuangjiance : MonoBehaviour
{   
    public GameObject Gojiazhua;
    public GameObject GoCylinder;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.Equals(GoCylinder))
        {
            Debug.Log("夹取成功");
            other.transform.SetParent(Gojiazhua.transform,true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.Equals(GoCylinder))
        {
            Debug.Log("放置成功");
            other.transform.parent = null;
        }
    }
   
    void Start()
    {
  
    }

    
    void Update()
    {   

    }
    
}
