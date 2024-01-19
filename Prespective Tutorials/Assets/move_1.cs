using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class move_1 : MonoBehaviour
{
   
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Start",3);
        transform.DOLocalMove(new Vector3(0, -0.15f, 0), 3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
