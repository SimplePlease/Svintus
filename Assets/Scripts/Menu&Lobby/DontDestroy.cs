using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        GameObject[] objs = GameObject.FindGameObjectsWithTag(this.tag);
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

    }
}
