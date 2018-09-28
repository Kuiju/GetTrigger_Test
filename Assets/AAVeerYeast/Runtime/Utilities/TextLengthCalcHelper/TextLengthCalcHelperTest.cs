using UnityEngine;
using UnityEngine.UI;

namespace Test
{
    public class TextLengthCalcHelperTest : GUITestBase
    {
        public Text ExampleText = null;
        public TextAsset TestTextAsset = null;
        public string TestString = "卑鄙是卑鄙者的通行证 高尚是高尚者的墓志铭 Baseness is the secret knock of the base Integrity the epitaph of the noble";

        public void Test_TestStringSize()
        {
            ExampleText.text = TestString;
            TextLengthCalcHelper.SetTextConfig(ExampleText);
            VeerDebug.Log("Test_TestStringSize : " + TextLengthCalcHelper.CalculatePreferredSize(TestString));
        }

        public void Test_TestStringSizeAdvanceCalc()
        {
            ExampleText.text = TestString;
            TextLengthCalcHelper.SetTextConfig(ExampleText);
            VeerDebug.Log("Test_TestStringSizeAdvanceCalc : " + TextLengthCalcHelper.CalculateLengthAdvance(TestString));
        }

        public void Test_TestTextAssetSize()
        {
            ExampleText.text = TestTextAsset.text;
            TextLengthCalcHelper.SetTextConfig(ExampleText);
            VeerDebug.Log("Test_TestTextAssetSize : " + TextLengthCalcHelper.CalculatePreferredSize(TestTextAsset.text));
        }

        public void Test_TestTextAssetSizeAdvanceCalc()
        {
            ExampleText.text = TestTextAsset.text;
            TextLengthCalcHelper.SetTextConfig(ExampleText);
            VeerDebug.Log("Test_TestTextAssetSizeAdvanceCalc : " + TextLengthCalcHelper.CalculateLengthAdvance(TestTextAsset.text));
        }

        //bool filledTag = false;
        //void FillTagLine(TagInfos tagInfos)
        //{
        //    tagCG.blocksRaycasts = false;
        //    tagCG.interactable = false;
        //    Debug.Log("taginfos getted, count is " + tagInfos.hot_keywords.Count);
        //    int tagWidth = 480;
        //    for (int i = 0; i < tagline1.transform.childCount; ++i)
        //    {
        //        if (tagInfos.hot_keywords.Count == 0)
        //            break;

        //        Transform tagTrans = tagline1.transform.GetChild(i);
        //        Text text = tagTrans.GetChild(0).GetComponent<Text>();
        //        int charatorWidth = CalculateLengthOfText(tagInfos.hot_keywords[0], text);
        //        int imageWidth = charatorWidth + 50;
        //        if (tagWidth < imageWidth)
        //            break;

        //        tagWidth -= imageWidth;
        //        text.text = tagInfos.hot_keywords[0];
        //        RectTransform rect = text.GetComponent<RectTransform>();
        //        rect.sizeDelta = new Vector2(charatorWidth, rect.sizeDelta.y);
        //        tagTrans.GetComponent<LayoutElement>().minWidth = imageWidth;
        //        tagTrans.gameObject.SetActive(true);
        //        tagInfos.hot_keywords.RemoveAt(0);
        //        tagline1.SetActive(true);
        //    }

        //    tagWidth = 480;
        //    for (int i = 0; i < tagline2.transform.childCount; ++i)
        //    {
        //        if (tagInfos.hot_keywords.Count == 0)
        //            break;

        //        Transform tagTrans = tagline2.transform.GetChild(i);
        //        Text text = tagTrans.GetChild(0).GetComponent<Text>();
        //        int charatorWidth = CalculateLengthOfText(tagInfos.hot_keywords[0], text);
        //        int imageWidth = charatorWidth + 50;
        //        if (tagWidth < imageWidth)
        //            break;

        //        tagWidth -= imageWidth;
        //        text.text = tagInfos.hot_keywords[0];
        //        RectTransform rect = text.GetComponent<RectTransform>();
        //        rect.sizeDelta = new Vector2(charatorWidth, rect.sizeDelta.y);
        //        tagTrans.GetComponent<LayoutElement>().minWidth = imageWidth;
        //        tagInfos.hot_keywords.RemoveAt(0);
        //        tagTrans.gameObject.SetActive(true);
        //        tagline2.SetActive(true);
        //    }

        //    Manager.instance.StartCoroutine(SetStartPoistion());
        //    filledTag = true;
        //}

        //IEnumerator SetStartPoistion()
        //{
        //    yield return new WaitForSeconds(0.2f);
        //    foreach (TweenBtnOffset tbo in tagline1.GetComponentsInChildren<TweenBtnOffset>())
        //        if (tbo.gameObject.activeInHierarchy)
        //            tbo.SetStartPosition();

        //    foreach (TweenBtnOffset tbo in tagline2.GetComponentsInChildren<TweenBtnOffset>())
        //        if (tbo.gameObject.activeInHierarchy)
        //            tbo.SetStartPosition();

        //    tagCG.blocksRaycasts = true;
        //    tagCG.interactable = true;
        //}

        //int CalculateLengthOfText(string message, Text text)
        //{
        //    int totalLength = 0;
        //    Font myFont = text.font;  //chatText is my Text component
        //    myFont.RequestCharactersInTexture(message, text.fontSize, text.fontStyle);
        //    CharacterInfo characterInfo = new CharacterInfo();
        //    char[] arr = message.ToCharArray();
        //    foreach (char c in arr)
        //    {
        //        myFont.GetCharacterInfo(c, out characterInfo, text.fontSize);
        //        totalLength += characterInfo.advance;
        //    }

        //    return totalLength;
        //}
    }
}