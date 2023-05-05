using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockTransform : MonoBehaviour
{
    void Update()
    {
        this.transform.eulerAngles = new Vector3(0,this.transform.eulerAngles.y,this.transform.eulerAngles.z);
    }
}
