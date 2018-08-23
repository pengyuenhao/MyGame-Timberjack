using UnityEngine;
using System.Collections;

public static class DetectionRay{
    public static LayerMask LayerDefault { get { return 1 << LayerMask.NameToLayer("Default"); } }
    public static LayerMask LayerWater { get { return 1 << LayerMask.NameToLayer("Water"); } }
    public static LayerMask LayerUI { get { return (1 << LayerMask.NameToLayer("UI")); } }
    public static LayerMask LayerGAME { get { return 1 << LayerMask.NameToLayer("Game"); } }
    public static LayerMask LayerAll { get { return -1; } }

    public static GameObject RayCastMask(Camera camera,Vector3 v,int layerMask)
    {
        Ray ray;
        RaycastHit hit;
        //Debug.Log(LayerAll.value);
        if (camera == null)
        {
            ray = Camera.main.ScreenPointToRay(v);
        }
        else
        {
            ray = camera.ScreenPointToRay(v);
        }
        if (Physics.Raycast(ray, out hit,100,layerMask))
        {
            //Debug.Log(hit.collider.name);
            return hit.collider.gameObject;
        }
        else
        {
            return null;
        }
    }
    public static GameObject RayCastUI(Vector3 v)
    {
        return RayCastMask(null,v, LayerUI.value);
    }
    public static GameObject RayCastGame(Vector3 v)
    {
        return RayCastMask(null, v, LayerGAME.value);
    }
    public static GameObject RayCastAll(Vector3 v)
    {
        return RayCastMask(null, v, LayerAll.value);
    }
    public static GameObject RayCast(Vector3 v)
    {
        GameObject tmp = RayCastUI(v);
        if (tmp == null)
        {
            tmp = RayCastGame(v);
            if (tmp == null)
            {
                tmp = RayCastAll(v);
            }
        }
        return tmp;
    }
}
