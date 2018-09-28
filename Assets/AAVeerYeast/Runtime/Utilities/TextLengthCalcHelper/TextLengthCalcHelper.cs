using UnityEngine;
using UnityEngine.UI;
using VeerYeast;

public class TextLengthCalcHelper : MonoSingleton<TextLengthCalcHelper>
{
    private bool _bTextInited = false;
    private void InitText()
    {
        _bTextInited = true;
        this.gameObject.SetActive(false);
        RectTransform rect = this.GetAddComponent<RectTransform>();
        rect.sizeDelta = Vector2.zero;
        Text text = this.GetAddComponent<Text>();
        text.enabled = false;
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.horizontalOverflow = HorizontalWrapMode.Overflow;
        text.verticalOverflow = VerticalWrapMode.Overflow;
        _TestText = text;
    }

    private Text _TestText = null;
    private Text TestText
    {
        get
        {
            if (!_bTextInited)
            {
                InitText();
            }
            return _TestText;
        }
    }

    public static void SetTextConfig(Text targetText)
    {
#if UNITY_EDITOR
        if (targetText == null)
        {
            VeerDebug.LogError(" SetTextConfig targetText is null...");
        }
#endif
        Instance.TestText.font = targetText.font;
        Instance.TestText.fontSize = targetText.fontSize;
        Instance.TestText.fontStyle = targetText.fontStyle;
        Instance.TestText.lineSpacing = targetText.lineSpacing;
        Instance.TestText.supportRichText = targetText.supportRichText;
        Instance.TestText.alignment = targetText.alignment;
        Instance.TestText.alignByGeometry = targetText.alignByGeometry;
    }

    public static Vector2 CalculatePreferredSize(string str)
    {
        Instance.TestText.text = str;
        return new Vector2(Instance.TestText.preferredWidth, Instance.TestText.preferredHeight);
    }

    public static Vector2 CalculateLengthAdvance(string str)
    {
        Instance.TestText.text = str;

        int totalLength = 0;
        Font font = Instance.TestText.font;
        font.RequestCharactersInTexture(str, Instance.TestText.fontSize, Instance.TestText.fontStyle);
        CharacterInfo characterInfo = new CharacterInfo();
        char[] arr = str.ToCharArray();
        foreach (char c in arr)
        {
            font.GetCharacterInfo(c, out characterInfo, Instance.TestText.fontSize);
            totalLength += characterInfo.advance;
        }

        return new Vector2(totalLength, Instance.TestText.preferredHeight);
    }

    //待整理，用于汉语换行
    public static string WrapText(string text, Text textComp)
    {
        float maxWidth = textComp.GetComponent<RectTransform>().sizeDelta.x;
        if (maxWidth < textComp.font.fontSize)
        {
            return text;
        }

        string rtext = text;
        string ptext = text;

        float tl = 0f;
        int wt = 0;

        textComp.font.RequestCharactersInTexture(text, textComp.fontSize, textComp.fontStyle);

        for (int i = 0; i < ptext.Length; i++)
        {
            CharacterInfo cinfo;

            if (textComp.font.GetCharacterInfo(ptext[i], out cinfo, textComp.fontSize))
            {
                tl += cinfo.advance;
            }
            if (tl > maxWidth + 2f)
            {
                rtext = rtext.Insert((i + wt - 1), "\n");
                wt++;
                tl = 0f;
            }
        }

        return rtext;
    }
}
