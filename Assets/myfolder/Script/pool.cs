using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//对象池
public class pool : MonoBehaviour
{
    public static pool instance;
    private pool() { }

    //用字典构造多个池子，每个类型的池子的名字为string类型，一个池子本质上是一个栈
    private static Dictionary<string, Stack<GameObject>> m_pool;
   
    void Awake()
    {
        m_pool = new Dictionary<string, Stack<GameObject>>();
        instance = this;//单例模式
    }

    //从池子得到物体
    public static GameObject GetPrefab(GameObject prefab, Vector3 position,Quaternion rotation)
    {
        //池子的名字，这样子命名是因为预制体实例化后会名字会加一个(Clone)
        string key = prefab.name + "(Clone)";

        if(!m_pool.ContainsKey(key))//没有这个栈
        {
            GameObject one = Instantiate(prefab, position, rotation);
            m_pool.Add(key, new Stack<GameObject>());
            one.SetActive(true);
            return one;
        }

        else if ( m_pool[key].Count > 0)//如果栈存在且有东西
        {
            GameObject one = m_pool[key].Pop();
            one.transform.position = position;
            one.transform.rotation = rotation;
            one.SetActive(true);
            return one;
        }
        else //栈存在，栈里没东西
        {
            GameObject one = Instantiate(prefab,position,rotation);
            one.SetActive(true);
            return one;
        }
     
    }

    //将已经实例化出的gameobject放入对应的池子
    public static GameObject PrefabReturn(GameObject one)
    {
        m_pool[one.name].Push(one);
        one.SetActive(false);
        return one;
    }
}

