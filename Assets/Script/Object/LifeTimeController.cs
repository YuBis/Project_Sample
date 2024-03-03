using Com.LuisPedroFonseca.ProCamera2D;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class LifeTimeController : BaseObject
{
    [SerializeField]
    float m_lifeTime = 1;

    float m_leftTime = 0;
    private void OnEnable()
    {
        m_leftTime = m_lifeTime;
        StartCoroutine(BeginCountdown());
    }

    IEnumerator BeginCountdown()
    {
        while(m_leftTime > 0)
        {
            m_leftTime -= Time.deltaTime;

            yield return null;
        }

        if(GAMEOBJECT)
            GAMEOBJECT.SetActive(false);
    }
}
