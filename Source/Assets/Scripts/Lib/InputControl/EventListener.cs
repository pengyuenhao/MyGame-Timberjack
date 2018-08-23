using UnityEngine;
using System.Collections;

public class EventListener : MonoBehaviour {
    /*UI监听模式设计*/
    public delegate void VoidEventHandler(GameObject go = null);
    public delegate void BoolEventHandler(GameObject go,bool state);
    //public delegate void ObjectEventHandler(GameObject go, GameObject obj);
    //public delegate void KeyCodeEventHandler(GameObject go, KeyCode key);

    public VoidEventHandler onClick;
    public VoidEventHandler onFree;
    public VoidEventHandler onPress;
    public VoidEventHandler onRightClick;
    public VoidEventHandler onHover;
    public VoidEventHandler onLeave;
    public VoidEventHandler onDrag;
    //public KeyCodeEventHandler onKeyClick;

    #region
    /// <summary>
    /// 判断碰撞器是否启用
    /// </summary>
    bool isColliderEnabled
    {
        get
        {
            Collider c = GetComponent<Collider>();
            if (c != null) return c.enabled;
            Collider2D b = GetComponent<Collider2D>();
            return (b != null && b.enabled);
        }
    }
    public void OnClick() { if (isColliderEnabled && onClick != null) onClick(gameObject); }
    public void OnFree() { if (isColliderEnabled && onFree != null) onFree(gameObject); }
    public void OnPress() { if (isColliderEnabled && onPress != null) onPress(gameObject); }
    public void OnRightClick() { if (isColliderEnabled && onRightClick != null) onRightClick(gameObject); }
    public void OnHover() { if (isColliderEnabled && onHover != null) onHover(gameObject); }
    public void OnLeave() { if (isColliderEnabled && onLeave != null) onLeave(gameObject); }
    public void OnDrag() { if (isColliderEnabled && onDrag != null) onDrag(gameObject); }
    #endregion
    //为监听的对象绑定监听脚本，这样可以使得其能够通过委托事件返回自己所在的对象
    static public EventListener Get(GameObject go)
    {
        //Debug.Log(go);
        EventListener listener = go.GetComponent<EventListener>();
        if (listener == null) listener = go.AddComponent<EventListener>();
        return listener;
    }
}
