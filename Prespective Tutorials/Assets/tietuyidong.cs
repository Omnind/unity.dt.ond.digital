using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tietuyidong : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    float x;
    float y;
    float scrollX = 1.0f;//用来调节贴图移动的速度,可适当调大或缩小
    // Update is called once per frame
    void Update()
    {
         y = y + Time.deltaTime * scrollX;
        GetComponent<Renderer>().material.mainTextureOffset = new Vector2(x, y);
        
       GetComponent<Renderer>().material.SetTextureOffset("1", new Vector2(x, y)); 
    }
}
