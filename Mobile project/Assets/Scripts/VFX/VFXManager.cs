using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager instance;
    public VFXListSO VFXScriptableList;

    private void Awake()
    {
        if (instance) return;
        instance = this;
    }

    public void PlayVFX(string vfxName, Transform parent)
    {
        StartCoroutine(BeginPlayVFX(vfxName, parent));
    }

    public GameObject PlayPermanentVFX(string vfxName, Transform parent)
    {
        VFXProperties vfx = VFXScriptableList.FindVFX(vfxName);
        GameObject vfxObject = Instantiate(vfx.vfx, parent);

        return vfxObject;
    }

    IEnumerator BeginPlayVFX(string vfxName, Transform parent)
    {
        Debug.Log("Play " + vfxName);
        VFXProperties vfx = VFXScriptableList.FindVFX(vfxName);
        GameObject vfxObject = Instantiate(vfx.vfx, parent);

        yield return new WaitForSeconds(vfx.duration);
        
        Destroy(vfxObject);
    }
}

[System.Serializable]
public class VFXProperties
{
    public string nameVFX;
    public GameObject vfx;
    public float duration;
}
