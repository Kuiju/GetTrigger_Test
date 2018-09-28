using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerArea
{
    public GameObject TargetObj = null;
    public RectTransform RectTrans { get { return TargetObj.GetComponent<RectTransform>(); } }
    public Button Btn { get { return TargetObj.GetComponent<Button>(); } }

    public bool bRaycastTargetActive = false;
    public TriggerArea Parentwrapper = null;
    public TriggerArea TriggerOwner = null;
    public List<TriggerArea> Pubwrapperlist = new List<TriggerArea>();
    public RectArea RectBorder= new RectArea () ;
    public RectAreaCollection RectAreaCollection = new RectAreaCollection () ;

  
}
