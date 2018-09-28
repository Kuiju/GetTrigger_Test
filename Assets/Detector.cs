using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Detector
{
   
    public List<TriggerArea> RaycastTargetWrapperList = new List<TriggerArea>();
    public List<TriggerArea> ButtonWrapperList = new List<TriggerArea>();
    public TriggerArea RootWrapper;
    public GameObject Root;
    bool GoOn = true;
    public Detector (GameObject root)
    {
        Root = root;
        RootWrapper = GetWrapperInternal(root, GoOn);
        RaycastTargetWrapperList = GetRaycastTargetWrapperList(RootWrapper);
        ButtonWrapperList = GetButtonWrapperList(RootWrapper);
        GetTriggerArea(RaycastTargetWrapperList);
    }




    public List<TriggerArea> GetRaycastTargetWrapperList(TriggerArea RootWrapper)
    {
        GetRaycastTarget(RootWrapper, RaycastTargetWrapperList);
        return RaycastTargetWrapperList;
    }

    public TriggerArea GetWrapperWithButton(TriggerArea CurrentWrapper)
    {
        if (CurrentWrapper.TargetObj.GetComponent<Canvas>() != null)
            return null;
        else if (CurrentWrapper.Btn != null)
            return CurrentWrapper;
        else
            return GetWrapperWithButton(CurrentWrapper.Parentwrapper);

    }

    public void GetTriggerArea(List <TriggerArea >rayCastTargetWrapper)
    {
        foreach (var child in rayCastTargetWrapper )
        {
            if(GetWrapperWithButton (child )!=null )
            {
                GetWrapperWithButton(child).RectAreaCollection.TriggerBorder.Add(child.RectBorder .GetTargetAnchorBorder (child .RectTrans ) );
                child.TriggerOwner = GetWrapperWithButton(child);
            }
        }
    }










    public List<TriggerArea> GetButtonWrapperList(TriggerArea RootWrapper)
    {

        GetButton(RootWrapper, ButtonWrapperList);
       
        return ButtonWrapperList;
    }

    public void GetButton(TriggerArea RootWrapper, List<TriggerArea> Buttons)
    {
        if (RootWrapper.Btn != null)
        {
            Buttons.Add(RootWrapper);
        }
       

        foreach (TriggerArea child in RootWrapper.Pubwrapperlist)
        {
            GetButton(child, Buttons);
        }
    }

    public void GetRaycastTarget(TriggerArea rootWrapper, List<TriggerArea> RaycastTargets)
    {
        if (rootWrapper.bRaycastTargetActive)
        {
            RaycastTargets.Add(rootWrapper);
        }


        foreach (TriggerArea child in rootWrapper.Pubwrapperlist)
        {
            GetRaycastTarget(child, RaycastTargets);
        }
    }







    public TriggerArea GetWrapperInternal(GameObject root, bool goOn)
    {
        if (!goOn)
            return null;
        TriggerArea wrapper = new TriggerArea();
        wrapper.TargetObj = root;

    
        if (root.GetComponent<Graphic>() != null&& !root .GetComponent <Text >() )
            wrapper.bRaycastTargetActive = root.GetComponent<Graphic>().raycastTarget;
        else
            wrapper.bRaycastTargetActive = false;

        foreach (Transform child in root.GetComponent<Transform>())
        {
            TriggerArea subwrapper = new TriggerArea();
            if (child != null)
            {
                subwrapper = GetWrapperInternal(child.gameObject, goOn);
                subwrapper.Parentwrapper = wrapper;
                wrapper.Pubwrapperlist.Add(subwrapper);
            }
            else
                goOn = false;
        }
        return wrapper;
    }



}
