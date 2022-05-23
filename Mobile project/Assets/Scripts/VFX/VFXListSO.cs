using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VFXList", menuName = "Scriptables/VFXList", order = 0)]
public class VFXListSO : ScriptableObject
{
    public VFXProperties[] list;
    
    public VFXProperties FindVFX(string name)
    {
        foreach (VFXProperties vfx in list)
        {
            if (vfx.nameVFX == name) return vfx;
        }
        Debug.Log("VFX " + name + " doesn't exist");
        return null;
    }
}
