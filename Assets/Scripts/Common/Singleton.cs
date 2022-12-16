using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> where T : new()
{
    private static T m_instance;
    public static T Instance
    {
        get
        {
            if (m_instance == null)
                m_instance = new T();
            return m_instance;
        }
    }
    protected Singleton() { }
}
