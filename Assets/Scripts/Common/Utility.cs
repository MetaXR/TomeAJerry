using System;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public enum RenderingMode
{
    Opaque,
    Cutout,
    Fade,
    Transparent,
}

public static class Utility
{
    private const float Radius = 5f;
    private const float Factor = 0.7f;


    public static int compareThree(List<float> list)
    {
        int maxIdx = 0;
        for (int i = 0; i < list.Count; i++)
        {
            for (int j = i; j < list.Count; j++)
            {
                if (list[i] > list[j])
                {
                    list[i] = list[j];
                    i = j;
                }               
            }
            maxIdx = i;
            return maxIdx;
        }

        return maxIdx;
    }

    /// <summary>
    /// 查找距离最近的信息点，
    /// </summary>
    /// <param name="list">the sources Dictionary</param>
    /// <returns>the show_Win Dictionary</returns>
    public static Dictionary<string, float> CompareAvage(Dictionary<string, float> list)
    {
        Dictionary<string, float> showDic = new Dictionary<string, float>();
        float sum = 0;
        float min = list.Values.Min();
        foreach (var item in list)
        {
            sum += item.Value;
        }                
        float avage = sum/ list.Count;
       
        foreach (var item in list)
        {
            if (item.Value < avage)
            {
                showDic.Add(item.Key, item.Value);               
            }
        }

        if (showDic.Count > 0)
        {
            return showDic;
        }
        else
        {
            return null;
        }
    }

    public static Vector3 LatLonToWorldPosition(float latitude, float longitude, GameObject sphere)
    {
        var render = sphere.GetComponent<MeshRenderer>();
        if (render == null)
        {
            Debug.LogError("Not Find Sphere MeshRenderer!");
            return Vector3.zero;
        }
        
           
        var radius = 0.5f * render.bounds.size.x * Factor;
        
        var y  = radius * Mathf.Sin(latitude * Mathf.Deg2Rad);
        var xz = radius * Mathf.Cos(latitude * Mathf.Deg2Rad);
        var x  = xz * Mathf.Cos((90 - longitude) * Mathf.Deg2Rad);
        var z  = xz * Mathf.Sin((90 - longitude) * Mathf.Deg2Rad);

        return new Vector3(x, y, z);
    }

    public static Vector2 LocalToWorldRect(Vector2 rect, GameObject sphere)
    {
        var render = sphere.GetComponent<MeshRenderer>();
        if (render == null)
        {
            Debug.LogError("Not Find Sphere MeshRenderer!");
            return Vector2.zero;
        }

        var worldRadius = 0.5f * render.bounds.size.x * Factor;
        var scaleFactor = worldRadius/Radius;

        return scaleFactor * rect / 28;
    }

    /// <summary>
    /// second goto timestr
    /// </summary>
    /// <param name="nSeek">second by /1000</param>
    /// <returns></returns>
    public static string FormatIntToTimeStr(int nSeek)
    {
        int hour = 0;
        int minute = 0;
        int second = 0;
        second = nSeek / 1000;

        if (second > 60)
        {
            minute = second / 60;
            second = second % 60;
        }
        if (minute > 60)
        {
            hour = minute / 60;
            minute = minute % 60;
        }
        string ret = string.Format("{0:D2}:{1:D2}:{2:D2}", hour, minute, second);
        return ret;
    }

    public static bool IsVisibleFrom(RectTransform rectTransform, Camera camera)
    {
        var worldCorners = new Vector3[4];
        rectTransform.GetWorldCorners(worldCorners);

        var rect = new Rect(0, 0, 1, 1);
        for (var i = 0; i < worldCorners.Length; i++)
        {
            var viewportPos = camera.WorldToViewportPoint(worldCorners[i]);
            if (rect.Contains(viewportPos))
                return true;
        }

        return false;
    }

    public static void GetCorners(Camera camera, float distance, ref Vector3[] corners)
    {
        Array.Resize(ref corners, 4);

        // Top left
        corners[0] = camera.ViewportToWorldPoint(new Vector3(0, 1, distance));

        // Top right
        corners[1] = camera.ViewportToWorldPoint(new Vector3(1, 1, distance));

        // Bottom left
        corners[2] = camera.ViewportToWorldPoint(new Vector3(0, 0, distance));

        // Bottom right
        corners[3] = camera.ViewportToWorldPoint(new Vector3(1, 0, distance));
    }

    public static bool AlmostEqual(this Vector3 a, Vector3 b, double accuracy = 0.01)
    {
        return Vector3.SqrMagnitude(a - b) < accuracy;
    }

    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }


    public static float Remap(this int value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;

    }

    public static double Remap(this double value, double from1, double to1, double from2, double to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }



    public static float Clamp(this float value, float min, float max)
    {
        return Mathf.Clamp(value, min, max);
    }

    public static float Clamp(this int value, int min, int max)
    {
        return Mathf.Clamp(value, min, max);
    }

    public static Vector3 CanvasToSphere(Vector3 pos, Vector2 savedRectSize, float savedRadius, float verticalAngle)
    {
        float radius = savedRadius;
        float vAngle = verticalAngle;

        vAngle = vAngle * (savedRectSize.y / savedRectSize.x);
        radius += vAngle > 0 ? -pos.z : pos.z;

        //convert planar coordinates to spherical coordinates
        float theta = (pos.x / savedRectSize.x).Remap(-0.5f, 0.5f, (180 - vAngle) / 2.0f - 90, 180 - (180 - vAngle) / 2.0f - 90);
        theta *= Mathf.Deg2Rad;
        float gamma = (pos.y / savedRectSize.y).Remap(-0.5f, 0.5f, (180 - vAngle) / 2.0f, 180 - (180 - vAngle) / 2.0f);
        gamma *= Mathf.Deg2Rad;

        pos.z = Mathf.Sin(gamma) * Mathf.Cos(theta) * radius;
        pos.y = -radius * Mathf.Cos(gamma);
        pos.x = Mathf.Sin(gamma) * Mathf.Sin(theta) * radius;

        pos.z -= radius;

        return pos;
    }

    public static GameObject FindGameChildObject(GameObject parent, string targetName)
    {
        if (parent == null || targetName == null) return null;
        if (parent.name == targetName)
        {
            return parent;
        }
        for (var i = 0; i < parent.transform.childCount; i++)
        {
            var obj = FindGameChildObject(parent.transform.GetChild(i).gameObject, targetName);
            if (obj)
                return obj;
        }
        return null;
    }

    public static string FormatFromMinutes(float time)
    {
        return TimeSpan.FromMinutes(time).ToString().Substring(0, 5);
    }

    public static string FromMilliseconds(float time)
    {
        return TimeSpan.FromMilliseconds(time).ToString().Substring(0, 8);
    }

    public static void SetButtonInteractable(GameObject button, bool interactable)
    {
        button.layer = LayerMask.NameToLayer(interactable ? "UI" : "Ignore Raycast");
    }

    public static void ExecuteMoveHandler(float x, float y)
    {
        var axisEventData = GetAxisEventData(x, y, 0.6f);
        ExecuteEvents.Execute(EventSystem.current.currentSelectedGameObject, axisEventData, ExecuteEvents.moveHandler);
        ExecuteEvents.Execute(EventSystem.current.currentSelectedGameObject, axisEventData,
            ExecuteEvents.updateSelectedHandler);
    }

    public static void ExecutePointerClickHandler()
    {
        ExecuteEvents.Execute(EventSystem.current.currentSelectedGameObject, new PointerEventData(EventSystem.current), 
            ExecuteEvents.pointerClickHandler);
        
    }

    private static AxisEventData GetAxisEventData(float x, float y, float moveDeadZone)
    {
        var mAxisEventData = new AxisEventData(EventSystem.current)
        {
            moveVector = new Vector2(x, y),
            moveDir = DetermineMoveDirection(x, y, moveDeadZone)
        };
        return mAxisEventData;
    }

    private static MoveDirection DetermineMoveDirection(float x, float y, float deadZone)
    {
        // if vector is too small... just return
        if (new Vector2(x, y).sqrMagnitude < deadZone * deadZone)
            return MoveDirection.None;

        if (Mathf.Abs(x) > Mathf.Abs(y))
        {
            if (x > 0)
                return MoveDirection.Right;
            return MoveDirection.Left;
        }
        else
        {
            if (y > 0)
                return MoveDirection.Up;
            return MoveDirection.Down;
        }
    }

    public static void LogInfomation(object obj)
    {
        
    }

    public static void SetMaterialRenderingMode(Material material, RenderingMode renderingMode)
    {
        switch (renderingMode)
        {
            case RenderingMode.Opaque:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = -1;
                break;
            case RenderingMode.Cutout:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.EnableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 2450;
                break;
            case RenderingMode.Fade:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
            case RenderingMode.Transparent:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
        }
    }

    public static float ClampEquip(this float value, float min, float max)
    {
        if (value < min)
        {
            value = min;
        }       
        else if(value>max)
        {
            value = max;
        }

        return value;
    }

    public  delegate void AddClickCallBack();

    public static void AddClickBtnListener(GameObject obj , AddClickCallBack callBack)
    {
        if(obj == null)
        {
            Debug.LogError("AddClickBtnListener -- obj is null");
            return;
        }
        obj.GetComponent<Button>().onClick.AddListener(()=> {
            if (callBack != null)
                callBack();
        });
    }

    public static void SetText(GameObject obj,string str = "")
    {
        if (obj == null)
        {
            Debug.LogError("SetText -- obj is null");
            return;
        }
        Text text = obj.GetComponent<Text>();
        if (text == null)
        {
            Debug.LogError("SetText -- text component is null");
            return;
        }
        text.text = str;
    }
    public static void SetText(Text text, string str = "")
    {
        if (text == null)
        {
            Debug.LogError("SetText -- text component is null");
            return;
        }
        text.text = str;
    }
}
