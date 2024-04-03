using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicMono : MonoBehaviour
{
    static PublicMono instance;
    public static PublicMono Instance
    {
        get
        {
            if (instance)
            {
                return instance;
            }
            var addComponent = new GameObject(nameof(PublicMono)).AddComponent<PublicMono>();
            instance = addComponent;
            return instance;
        }
    }

    public Action OnUpdate;
    public Action OnLateUpdate;
    public Action OnFixedUpdate;

    void Update()
    {
        OnUpdate?.Invoke();
    }
    
    void LateUpdate()
    {
        OnLateUpdate?.Invoke();
    }
    
    void FixedUpdate()
    {
        OnFixedUpdate?.Invoke();
    }
}