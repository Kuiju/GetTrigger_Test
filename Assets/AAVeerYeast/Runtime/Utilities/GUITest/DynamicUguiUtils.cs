using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// By GuoYu 需要后续整理
public class DynamicUguiUtils
{
    private static GameObject MyRoot = null;
    private static GameObject MyCanvas = null;
    private static GameObject MyEventSyStem = null;

    private static bool isTestCreateButtonInit = false;

    // 用来调整button 间距的值，暂时给定
    private const int Space = 10;

    #region  设置九个方位的锚点 以及四角锚

    public static void SetAnchorsLeftTop(RectTransform rect)
    {
        rect.anchorMin = new Vector2(0, 1);
        rect.anchorMax = new Vector2(0, 1);
    }

    public static void SetAnchorsCenterTop(RectTransform rect)
    {
        rect.anchorMin = new Vector2(0.5f, 1);
        rect.anchorMax = new Vector2(0.5f, 1);
    }

    public static void SetAnchorsRightTop(RectTransform rect)
    {
        rect.anchorMin = new Vector2(1, 1);
        rect.anchorMax = new Vector2(1, 1);
    }

    public static void SetAnchorsLeftMiddle(RectTransform rect)
    {
        rect.anchorMin = new Vector2(0, 0.5f);
        rect.anchorMax = new Vector2(0, 0.5f);
    }

    public static void SetAnchorsCenter(RectTransform rect)
    {
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
    }

    public static void SetAnchorsRightMiddle(RectTransform rect)
    {
        rect.anchorMin = new Vector2(1, 0.5f);
        rect.anchorMax = new Vector2(1, 0.5f);
    }

    public static void SetAnchorsLeftBottom(RectTransform rect)
    {
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(0, 0);
    }

    public static void SetAnchorsCenterBottom(RectTransform rect)
    {
        rect.anchorMin = new Vector2(0.5f, 0);
        rect.anchorMax = new Vector2(0.5f, 0);
    }

    public static void SetAnchorsRightBottom(RectTransform rect)
    {
        rect.anchorMin = new Vector2(1, 0);
        rect.anchorMax = new Vector2(1, 0);
    }

    public static void SetAnchorsVertexs(RectTransform rect)
    {
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(1, 1);
    }

    #endregion

    #region 设置 pivot 四个角以及中心点

    public static void SetPivotCenter(RectTransform rect)
    {
        rect.pivot = new Vector2(0.5f, 0.5f);
    }

    public static void SetPivotLeftTop(RectTransform rect)
    {
        rect.pivot = new Vector2(0, 1);
    }

    public static void SetPivotRightTop(RectTransform rect)
    {
        rect.pivot = new Vector2(1, 1);
    }

    public static void SetPivotRightBottom(RectTransform rect)
    {
        rect.pivot = new Vector2(1, 0);
    }

    public static void SetPivotLeftBotton(RectTransform rect)
    {
        rect.pivot = new Vector2(0, 0);
    }

    #endregion

    public static void CreateTestButtonInit()
    {
        if (isTestCreateButtonInit)
            return;
        if (MyCanvas == null)
        {
            MyCanvas = new GameObject("Canvas");
            MyCanvas.AddComponent<Canvas>();
            MyCanvas.AddComponent<GraphicRaycaster>();
            MyCanvas.AddComponent<CanvasScaler>();
            RectTransform rect = MyCanvas.GetAddComponent<RectTransform>();
            rect.position = Vector3.zero;
            rect.rotation = Quaternion.identity;
            rect.localScale = Vector3.one;
        }
        if (MyRoot == null)
        {
            MyRoot = new GameObject("Root");
            MyRoot.layer = 5;  //UI
            MyRoot.transform.parent = MyCanvas.transform;
            RectTransform rect = MyRoot.GetAddComponent<RectTransform>();
            rect.position = Vector3.zero;
            rect.rotation = Quaternion.identity;
            rect.localScale = Vector3.one;
            SetAnchorsLeftTop(rect);
            SetPivotLeftTop(rect);
            VerticalLayoutGroup tmp = MyRoot.GetAddComponent<VerticalLayoutGroup>();
            tmp.spacing = Space;
            tmp.childControlHeight = false;
            tmp.childControlWidth = false;
        }
        if (MyEventSyStem == null)
        {
            MyEventSyStem = new GameObject("EventSyStem");
            EventSystem tmp = MyEventSyStem.AddComponent<EventSystem>();
            MyEventSyStem.AddComponent<StandaloneInputModule>();
            MyEventSyStem.AddComponent<BaseInput>();
        }
        isTestCreateButtonInit = true;
    }

    public static RectTransform GetTestButtonRootFor2D()
    {
        CreateTestButtonInit();
        // 保证一定是2d
        Canvas canvas = MyCanvas.GetAddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        return MyRoot.GetComponent<RectTransform>();
    }

    public static RectTransform GetTestButtonRootFor3D()
    {
        CreateTestButtonInit();
        // 保证是3d
        Canvas tmp = MyCanvas.GetAddComponent<Canvas>();
        tmp.renderMode = RenderMode.WorldSpace;

        return MyRoot.GetComponent<RectTransform>();
    }

}
