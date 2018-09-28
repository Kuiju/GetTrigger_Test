using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public abstract class GUITestBase : MonoBehaviour
{
    public bool GUI = false;
    public bool SelfControlGUI = false;

    public RectTransform TestButtonRoot = null;

    public float width = 200;
    public float height = 20;

    protected void CreateTestButton()
    {
        if (TestButtonRoot == null)
            return;

        MethodInfo[] methods = this.GetType().GetMethods(
          BindingFlags.DeclaredOnly |
          BindingFlags.Instance |
          BindingFlags.Public |
          BindingFlags.NonPublic);

        foreach (var func in methods)
        {
            if (func.Name.StartsWith("Test_", StringComparison.Ordinal))
            {
                GameObject go = new GameObject("test_button");

                RectTransform goRect = go.GetAddComponent<RectTransform>();
                goRect.SetParent(TestButtonRoot);
                DynamicUguiUtils.SetAnchorsLeftTop(goRect);
                goRect.position = Vector3.zero;
                goRect.rotation = Quaternion.identity;
                goRect.localScale = Vector3.one;
                goRect.sizeDelta = new Vector2(width, height);

                Image goImage = go.GetAddComponent<Image>();
                goImage.color = new Color(1, 1, 1, 0.5f);

                Button goBtn = go.GetAddComponent<Button>();
                goBtn.onClick.AddListener(() => { func.Invoke(this, null); });
                goBtn.targetGraphic = goImage;

                //改为子级添加text
                GameObject text = new GameObject("text");
                RectTransform textRect = text.GetAddComponent<RectTransform>();
                DynamicUguiUtils.SetAnchorsCenter(textRect);
                textRect.SetParent(go.GetComponent<RectTransform>());
                textRect.position = Vector3.zero;
                textRect.rotation = Quaternion.identity;
                textRect.localScale = Vector3.one;
                textRect.sizeDelta = new Vector2(width, height);
                Text goText = text.GetAddComponent<Text>();
                goText.horizontalOverflow = HorizontalWrapMode.Wrap;
                goText.verticalOverflow = VerticalWrapMode.Truncate;
                goText.resizeTextForBestFit = true;
                goText.color = Color.black;
                goText.alignment = TextAnchor.MiddleLeft;
                goText.font = Resources.Load<Font>("Fonts/AvenirLTStd-Heavy");
                goText.fontSize = 40;
                goText.text = func.Name.Substring(5);
            }
        }

    }

#if UNITY_EDITOR
    private void OnGUI()
    {
        if (!SelfControlGUI)
        {
            return;
        }
        GUIInternal();
    }

    private void GUIInternal()
    {
        MethodInfo[] methods = this.GetType().GetMethods(
          BindingFlags.DeclaredOnly |
          BindingFlags.Instance |
          BindingFlags.Public |
          BindingFlags.NonPublic);

        foreach (var func in methods)
        {
            if (func.Name.StartsWith("Test_", StringComparison.Ordinal))
            {
                if (GUILayout.Button(func.Name.Substring(5)))
                {
                    func.Invoke(this, null);
                }
            }
        }
    }



#endif

}
