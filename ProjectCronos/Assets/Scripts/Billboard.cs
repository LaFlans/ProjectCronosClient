using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectCronos
{
    /// <summary>
    /// 常にカメラを向く機能クラス
    /// </summary>
    public class Billboard : MonoBehaviour
    {
        void Update()
        {
            Vector3 p = Camera.main.transform.position;
            p.y = transform.position.y;
            transform.LookAt(p);
        }
    }
}
