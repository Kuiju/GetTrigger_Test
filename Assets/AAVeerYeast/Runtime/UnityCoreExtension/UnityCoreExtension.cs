using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class UnityCoreExtension
{
    #region Int Float Bool

    public static bool EqualZero(this float f)
    {
        return f < float.Epsilon && f > -float.Epsilon;
    }

    public static bool FloatEqual(this float f1, float f2, float threshold = 0.000001f)
    {
        return Math.Abs(f1 - f2) < threshold;
    }

    #endregion

    #region Component GameObject Transform MonoBehaviour

    /// <summary>
    /// Get component. If target type component is not exist, then add it.
    /// </summary>
    /// <returns>Target component.</returns>
    /// <param name="comp">Target component.</param>
    /// <typeparam name="T">Component type.</typeparam>
    public static T GetAddComponent<T>(this Component comp) where T : Component
    {
        return comp.gameObject.GetAddComponent<T>();
    }

    /// <summary>
    /// Get component. If target type component is not exist, then add it.
    /// </summary>
    /// <returns>Target component.</returns>
    /// <param name="gameObject">Target game object.</param>
    /// <typeparam name="T">Component type.</typeparam>
    public static T GetAddComponent<T>(this GameObject gameObject) where T : Component
    {
        T comp = gameObject.GetComponent<T>();
        if (comp == null)
        {
            comp = gameObject.AddComponent<T>();
        }
        return comp;
    }

    public static GameObject GetChildGoByName(this GameObject root, string name, bool bRecursive = true)
    {
        if (root == null)
            return null;

        for (int i = 0; i < root.transform.childCount; i++)
        {
            GameObject sub = root.transform.GetChild(i).gameObject;
            if (sub.name == name)
            {
                return sub;
            }

            if (bRecursive)
            {
                GameObject recursiveInSub = GetChildGoByName(sub, name, true);
                if (recursiveInSub != null)
                    return recursiveInSub;
            }
        }

        return null;
    }

    public static RectTransform Rect(this GameObject gameObject)
    {
        return gameObject.GetAddComponent<RectTransform>();
    }

    public static RectTransform Rect(this MonoBehaviour monoBehaviour)
    {
        return monoBehaviour.GetAddComponent<RectTransform>();
    }

    public static CanvasGroup CanvasGroup(this RectTransform rect)
    {
        return rect.GetAddComponent<CanvasGroup>();
    }

    public static float GetAlpha(this RectTransform rect)
    {
        return rect.GetAddComponent<CanvasGroup>().alpha;
    }

    public static void SetAlpha(this RectTransform rect, float val)
    {
        rect.GetAddComponent<CanvasGroup>().alpha = val;
    }

    public static bool GetInteractable(this RectTransform rect)
    {
        return rect.GetAddComponent<CanvasGroup>().interactable;
    }

    public static void SetInteractable(this RectTransform rect, bool val)
    {
        rect.GetAddComponent<CanvasGroup>().interactable = val;
    }

    public static void SetWorldTransAsHolder(this Transform trans, TransHolder holder)
    {
        holder.SetAsWorld(trans);
    }

    public static void SetLocalTransAsHolder(this Transform trans, TransHolder holder)
    {
        holder.SetAsLocal(trans);
    }

    #endregion

    #region Rotation

    public static Quaternion AddEularAngle(this Quaternion quaternion, Vector3 eularAngle)
    {
        return Quaternion.Euler(quaternion.eulerAngles + eularAngle);
    }

    public static Quaternion MinusEularAngle(this Quaternion quaternion, Vector3 eularAngle)
    {
        return Quaternion.Euler(quaternion.eulerAngles - eularAngle);
    }

    #endregion

    #region Array

    public static string ArrayToString<T>(this T[] array, string joinStr = ", ")
    {
        int count = array.Length;
        string rst = "";
        for (int i = 0; i < count; i++)
        {
            rst += array[i].ToString();
            if (i != count - 1)
            {
                rst += joinStr;
            }
        }
        return rst;
    }

    #endregion

    #region List

    /// <summary>
    /// Add value to list if not contain.
    /// </summary>
    public static void AddIfNotContain<T>(this List<T> list, T value)
    {
        if (!list.Contains(value))
        {
            list.Add(value);
        }
    }

    public static string ListToString<T>(this List<T> list, string joinStr = ", ")
    {
        int count = list.Count;
        string rst = "";
        for (int i = 0; i < count; i++)
        {
            rst += list[i].ToString();
            if (i != count - 1)
            {
                rst += joinStr;
            }
        }
        return rst;
    }

    #endregion

    #region Dictionary

    /// <summary>
    /// Set value. If key is not exist then add, else just set.
    /// </summary>
    public static void SetAddValue<T, V>(this Dictionary<T, V> dic, T key, V value)
    {
        if (dic.ContainsKey(key))
        {
            dic[key] = value;
        }
        else
        {
            dic.Add(key, value);
        }
    }

    #endregion

    #region String

    public static Uri ToUrl(this string str)
    {
        Uri url = null;
        try
        {
            url = new Uri(str);
        }
        catch
        {
            Debug.LogError("can not create url by string : " + str);
        }
        return url;
    }

    #endregion

    #region Texture2D Sprite

    public static Sprite ToSprite(this Texture2D texture2d)
    {
        return Sprite.Create(texture2d, new Rect(0, 0, texture2d.width, texture2d.height), Vector2.zero);
    }

    #endregion

    #region MaskableGraphic

    public static void SetUIActiveRecursively(this GameObject gameObject, bool active)
    {
        MaskableGraphic[] maskableGraphics = gameObject.GetComponentsInChildren<MaskableGraphic>(true);
        int count = maskableGraphics.Length;
        for (int i = 0; i < count; i++)
        {
            maskableGraphics[i].SetUIActive(active);
        }
    }

    public static void SetUIActive(this MaskableGraphic maskableGraphic, bool active)
    {
        maskableGraphic.canvasRenderer.cull = !active;
    }

    #endregion
}
