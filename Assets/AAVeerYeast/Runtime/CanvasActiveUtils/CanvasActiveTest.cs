using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Profiling;

public class CanvasActiveTest : GUITestBase
{
    public GameObject CanvasRoot;
    public GameObject prefab;
    public List<Canvas> list = new List<Canvas>();
    public Material uiDefaultMaterial;
    public Image A;
    public Image B;

    //MaterialPropertyBlock MaterialPropertyBlock = new MaterialPropertyBlock();
    // Use this for initialization
    void Start()
    {

    }

    void Test_CanvasRenderCull()
    {
        Profiler.BeginSample("Test_CanvasRenderCull");
        A.canvasRenderer.cull = !A.canvasRenderer.cull;
        Profiler.EndSample();
    }

    void Test_ImageSetAcitve()
    {
        Profiler.BeginSample("Test_ImageSetAcitve");
        B.gameObject.SetActive(!B.gameObject.activeInHierarchy);
        Profiler.EndSample();
    }

    void Test_SetABMaterials()
    {
        A.material = new Material(Shader.Find("UI/Default")); A.material.SetColor("_Color", Color.red);
        B.material = new Material(Shader.Find("UI/Default")); B.material.SetColor("_Color", Color.white);
        //Material m = new Material(Shader.Find("UI/Default"));
        //A
    }

    void Test_GetMaterials()
    {
        for (int i = 0; i < this.transform.childCount; ++i)
        {
            this.transform.GetChild(i).GetComponent<Graphic>().material = uiDefaultMaterial;
            Material m = this.transform.GetChild(i).GetComponent<Graphic>().material;
            m.SetColor("_Color", Color.red);
            Debug.Log(m.name);
        }
    }

    void Test_CreateCanvas()
    {
        for (int i = 0; i < 1000; i++)
        {
            Canvas go = Instantiate(prefab).GetComponent<Canvas>();
            go.transform.SetParent(CanvasRoot.transform, false);
            list.Add(go);
        }
    }

    void Test_SetActiveFalse()
    {
        Profiler.BeginSample("Test_SetActiveFalse");
        foreach (var canvas in list)
            canvas.gameObject.SetActive(false);
        Profiler.EndSample();
    }

    void Test_SetActiveTrue()
    {
        Profiler.BeginSample("Test_SetActiveTrue");
        foreach (var canvas in list)
            canvas.gameObject.SetActive(true);
        Profiler.EndSample();
    }


    void Test_EnableCanvas()
    {
        Profiler.BeginSample("Test_EnableCanvas");
        foreach (var canvas in list)
            canvas.enabled = true;
        Profiler.EndSample();
    }

    void Test_DisableCanvas()
    {
        Profiler.BeginSample("Test_DisableCanvas");
        foreach (var canvas in list)
            canvas.enabled = false;
        Profiler.EndSample();
    }


}
