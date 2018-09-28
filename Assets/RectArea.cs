using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectArea 
{
    public List<Vector2> BorderCornerList = new List<Vector2>();
    public Vector2 center = new Vector2();


    //获取目标的所有顶点，得到触发范围
    public RectArea GetTargetAnchorBorder(RectTransform target)
    {

        Vector2 leftbottom = GetInScreenPostion(target, ECorner.LeftBottom);
        Vector2 lefttop = GetInScreenPostion(target, ECorner.LeftTop);
        Vector2 leftmiddle = GetInScreenPostion(target, ECorner.LeftMiddle);
        Vector2 rightbottom = GetInScreenPostion(target, ECorner.RightBottom);
        Vector2 righttop = GetInScreenPostion(target, ECorner.RightTop);
        Vector2 rightmiddle = GetInScreenPostion(target, ECorner.RightMiddle);
        Vector2 topmiddle = GetInScreenPostion(target, ECorner.TopMiddle);
        Vector2 bottommiddle = GetInScreenPostion(target, ECorner.BottomMiddle);
        Vector2 center = GetInScreenPostion(target, ECorner.Center);

        RectArea theAnchorBorder = new RectArea();
        theAnchorBorder.BorderCornerList.Add(leftbottom);
        theAnchorBorder.BorderCornerList.Add(lefttop);
        theAnchorBorder.BorderCornerList.Add(righttop);
        theAnchorBorder.BorderCornerList.Add(rightbottom);
        theAnchorBorder.center = center;
        return theAnchorBorder;
    }





    //获取顶点坐标
    public Vector2 GetInScreenPostion(RectTransform target, ECorner corner)
    {
        GameObject canvas = TargetAnchorConfig.Instance .canvas;
        RectTransform AnchorOriginal = TargetAnchorConfig.Instance .originArchor;
        RectTransform AnchorTarget = TargetAnchorConfig.Instance .targetArchor;
        Vector2 targetsize;
        float canvasX = canvas.GetComponent<RectTransform>().sizeDelta.x;
        float targetwidth = canvasX * (target.anchorMax.x - target.anchorMin.x);
        float offsetX = -target.offsetMax.x + target.offsetMin.x;
        if (targetwidth != 0)
            targetwidth -= offsetX;
        else
            targetwidth = target.sizeDelta.x;


        float canvasY = canvas.GetComponent<RectTransform>().sizeDelta.y;

        float targetheight = canvasY * (target.anchorMax.y - target.anchorMin.y);

        float offsetY = -target.offsetMax.y + target.offsetMin.y;

        if (targetheight != 0)
            targetheight -= offsetY;
        else
            targetheight = target.sizeDelta.y;

        targetsize = new Vector2(targetwidth, targetheight);
        AnchorOriginal.transform.SetParent(canvas.transform);
        AnchorTarget.transform.SetParent(target.transform);
        AnchorOriginal.anchoredPosition = new Vector3(-canvas.GetComponent<RectTransform>().sizeDelta.x / 2, -canvas.GetComponent<RectTransform>().sizeDelta.y / 2, 0);
        switch (corner)
        {
            case ECorner.LeftBottom:
                AnchorTarget.anchoredPosition = new Vector3(-targetwidth / 2, -targetheight / 2, 0);
                break;
            case ECorner.LeftTop:
                AnchorTarget.anchoredPosition = new Vector3(-targetwidth / 2, targetheight / 2, 0);
                break;
            case ECorner.RightBottom:
                AnchorTarget.anchoredPosition = new Vector3(targetwidth / 2, -targetheight / 2, 0);
                break;
            case ECorner.RightTop:
                AnchorTarget.anchoredPosition = new Vector3(targetwidth / 2, targetheight / 2, 0);
                break;
            case ECorner.LeftMiddle:
                AnchorTarget.anchoredPosition = new Vector3(-targetwidth / 2, 0, 0);
                break;
            case ECorner.RightMiddle:
                AnchorTarget.anchoredPosition = new Vector3(targetwidth / 2, 0, 0);
                break;
            case ECorner.TopMiddle:
                AnchorTarget.anchoredPosition = new Vector3(0, targetheight / 2, 0);
                break;
            case ECorner.BottomMiddle:
                AnchorTarget.anchoredPosition = new Vector3(0, -targetheight / 2, 0);
                break;
            case ECorner.Center:
                AnchorTarget.anchoredPosition = new Vector3(0, 0, 0);
                break;
            default:
                Debug.Log("Wrong Corner");
                break;

        }

        AnchorTarget.transform.SetParent(canvas.transform);
        Vector3 coordinateIn3D = AnchorTarget.anchoredPosition - AnchorOriginal.anchoredPosition;
        Vector2 coordinateIn2D = new Vector2(coordinateIn3D.x, coordinateIn3D.y);
        return coordinateIn2D;
    }


}
