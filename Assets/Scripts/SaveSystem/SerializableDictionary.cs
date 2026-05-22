using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[Serializable]
public class SerializableDictionary<Tkey, TValue> : Dictionary<Tkey, TValue>, ISerializationCallbackReceiver//该自定义类继承自字典类
{
    [SerializeField] private List<Tkey> keys = new List<Tkey>();//将原字典数据结构的键值拆解成两个列表
    [SerializeField] private List<TValue> values = new List<TValue>();
    
    public void OnAfterDeserialize()//读取序列化的数据并解除序列化之后,即读取到的是两个列表类型的数据结构，需要将其还原回字典
    {
        this.Clear();

        if (keys.Count != values.Count)
        {
            Debug.Log("键值对不相等");
        }

        for (int i = 0; i < keys.Count; i++)
        {
            this.Add(keys[i], values[i]);
        }
    }

    public void OnBeforeSerialize()//在数据序列化之前,把存储在父类(字典)数据结构中的数据提取出来转化为列表形式存储
    {
        keys.Clear();
        values.Clear();

        foreach (KeyValuePair<Tkey, TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }
}
