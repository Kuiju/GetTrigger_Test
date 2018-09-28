using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KuijuTest :MonoBehaviour 
{
    public GameObject Root{ get { return TargetAnchorConfig.Instance.canvas; }} 
    public Detector Detect;
    public string TriggerName;

    public List<GameObject > Triggers = new List<GameObject> ();

    public void Start()
    {
        Detect = new Detector(Root);
        ChangeName(Detect);

    }

    public void Update()
    {
   
        Test_GetTrigger();

    }

    public void Test_GetTrigger()
    {
        TriggerName = null;
        Triggers = Test_GetTriggerOwner();
        if (Triggers.Count != 0)
        {
            TriggerName = Triggers[Triggers.Count - 1].name;
        }
    }

 
    public List <GameObject  > Test_GetTriggerOwner()
    {

        List<GameObject > Triggers = new List<GameObject > ();
        foreach (var child in Detect.ButtonWrapperList)
        {
            if (child.RectAreaCollection.IsPositionInTriggerArea(Input.mousePosition))
            {

                Triggers.Add(child.TargetObj );


            }

        }

        return Triggers;
    }


    public void ChangeName(Detector detector )
    {
        
        foreach  (var btn in detector .ButtonWrapperList  )
        {
            btn.TargetObj.name = "btn"+detector .ButtonWrapperList .IndexOf (btn );


        }

        int i = 0;

        foreach (var child in detector .RaycastTargetWrapperList )
        {
            if(!child .TargetObj .GetComponent <Button >())
            {
                child.TargetObj.name = "image" + i;
                i++;
            }
        }
    }


    








    






}
