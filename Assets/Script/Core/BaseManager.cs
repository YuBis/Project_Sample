using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager<MANAGER_TYPE> : DataLoader where MANAGER_TYPE : DataLoader, new()
{
    static MANAGER_TYPE s_instance = null;
    public static MANAGER_TYPE Instance
    {
        get
        {
            if(s_instance == null)
            {
                s_instance = new MANAGER_TYPE();
                s_instance.InitData();
            }

            return s_instance;
        }
    }
}
