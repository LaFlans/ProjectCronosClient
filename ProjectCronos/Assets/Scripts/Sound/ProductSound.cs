using UnityEngine;

namespace ProjectCronos
{
    public class ProductSound : MonoBehaviour
    {
        static ProductSound instance;

        public static ProductSound Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject("ProductSound").AddComponent<ProductSound>();
                    DontDestroyOnLoad(instance.gameObject);
                }
                return instance;
            }
        }


        public void Hello()
        {
            Debug.Log("Hello");
        }
    }
}