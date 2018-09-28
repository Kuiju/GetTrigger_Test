using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class SpriteResolutionChanger : EditorWindow
{
    List<Texture2D> textures = new List<Texture2D>();
    int selected = -1;
    Texture2D currentTexture = null;
    bool stretch = false;
    bool pixel = false;
    Vector2 pos = Vector2.zero;
    float rateWidth = 100f;
    float rateHeight = 100f;
    int pixelWidth = 0;
    int pixelHeight = 0;
    Texture2D curTempTexture;
    [MenuItem("Assets/Edit Image ...", false, 0)]
    static private void EditImage()
    {
        var window = GetWindow<SpriteResolutionChanger>();
        window.Reset();
        var list = Selection.objects;
        for (int i = 0; list != null && i < list.Length; ++i)
        {
            if (list[i] is Texture2D)
                window.textures.Add(list[i] as Texture2D);
            else if (list[i] is Sprite)
                window.textures.Add((list[i] as Sprite).texture);
        }
    }

    void Reset()
    {
        currentTexture = null;
        textures.Clear();
        selected = -1;
        pos = Vector2.zero;
        stretch = true;
        pixel = false;
    }

    void ResetSize()
    {
        rateWidth = rateHeight = 100f;

        pixelWidth = currentTexture.width;
        pixelHeight = currentTexture.height;
    }

    GUIStyle bgStyle = null;
    void OnGUI()
    {
        if (curTempTexture == null)
            curTempTexture = new Texture2D(1, 1, TextureFormat.RGBA32, false);

        var boxRect = EditorGUILayout.GetControlRect(GUILayout.ExpandHeight(true));
        var rect = boxRect;

        if (bgStyle == null)
            bgStyle = GUI.skin.box;

        GUI.Box(rect, "", bgStyle);

        if (0 <= selected && selected < textures.Count)
        {
            if (currentTexture != textures[selected])
            {
                currentTexture = textures[selected];
                ResetSize();
            }
        }
        else
        {
            currentTexture = null;
        }

        if (rect.width != 0 && rect.height != 0 && currentTexture != null && currentTexture.width != 0 && currentTexture.height != 0)
        {
            float rectRatio = (float)rect.width / rect.height;
            float textureRatio = (float)currentTexture.width / currentTexture.height;
            if (rectRatio > textureRatio)
            {
                rect.width = rect.height * textureRatio;
                rect.x += boxRect.width * 0.5f - rect.width * 0.5f;
            }
            else
            {
                rect.height = rect.width / textureRatio;
                rect.y += boxRect.height * 0.5f - rect.height * 0.5f;
            }
            GUI.DrawTexture(rect, currentTexture);
        }

        GUILayout.BeginScrollView(pos, "box");
        if (textures.Count == 1)
        {
            selected = 0;
        }
        else
        {
            for (int i = 0; i < textures.Count; ++i)
            {
                if (EditorGUILayout.Toggle(textures[i].name, i == selected))
                {
                    selected = i;
                }
            }
        }
        var s = EditorGUILayout.Toggle("Stretch", stretch);
        if (stretch != s)
        {
            stretch = s;
            ResetSize();
        }
        pixel = EditorGUILayout.Toggle("Pixel", pixel);
        EditorGUILayout.BeginHorizontal();
        if (pixel)
        {
            pixelWidth = Mathf.Max(1, EditorGUILayout.IntField(pixelWidth));

            pixelHeight = Mathf.Max(1, EditorGUILayout.IntField(pixelHeight));
        }
        else
        {

            rateWidth = Mathf.Max(1f, EditorGUILayout.FloatField(rateWidth));
            EditorGUILayout.LabelField("%");
            rateHeight = Mathf.Max(1f, EditorGUILayout.FloatField(rateHeight));
            EditorGUILayout.LabelField("%");
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndScrollView();
        if (GUILayout.Button("Update"))
        {
            if (currentTexture != null && stretch)
            {
                if (!pixel)
                    ChangeResolution(currentTexture, curTempTexture, rateWidth / 100f, rateHeight / 100f);
                else
                    ChangeResolution(currentTexture, curTempTexture, (float)pixelWidth / currentTexture.width, (float)pixelHeight / currentTexture.height);
            }
            if (currentTexture != null && !stretch)
            {
                if (!pixel)
                {
                    Clip(currentTexture, (int)(rateWidth * currentTexture.width / 100f), (int)(rateHeight * currentTexture.height / 100f));
                }
                else
                    Clip(currentTexture, pixelWidth, pixelHeight);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        if (GUILayout.Button("Expand For 3D"))
        {
            if (currentTexture != null)
            {
                ExpandFor3D(currentTexture);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

    }
    [MenuItem("Assets/Edit Image ...", true)]
    static private bool CanExpand()
    {

        var list = Selection.objects;
        for (int i = 0; list != null && i < list.Length; ++i)
        {
            if (list[i] is Texture2D || list[i] is Sprite)
                return true;
        }
        return false;
    }
    public static void ExpandFor3D()
    {
        var list = Selection.objects;
        for (int i = 0; list != null && i < list.Length; ++i)
        {
            ExpandFor3D(((list[i] is Sprite) ? (list[i] as Sprite).texture : (list[i] as Texture2D)));
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    static void ExpandFor3D(Texture2D tex)
    {
        if (tex == null)
            return;
        int expandSize = 2;
        var width = tex.width;
        var height = tex.height;
        var newWidth = width + expandSize * 2;
        var newHeight = height + expandSize * 2;
        var newColor = new Color[newWidth * newHeight];//, tex.format, false);
        RenderTexture texture = RenderTexture.GetTemporary(tex.width, tex.height, 0, RenderTextureFormat.ARGB32);
        texture.DiscardContents();
        Graphics.Blit(tex, texture);
        Texture2D oldTexture = new Texture2D(tex.width, tex.height, tex.format, false);
        RenderTexture.active = texture;
        oldTexture.Resize(texture.width, texture.height);
        oldTexture.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
        oldTexture.Apply();

        var oldColor = oldTexture.GetPixels();
        for (int i = -expandSize; i < width + expandSize; ++i)
        {
            for (int j = -expandSize; j < height + expandSize; ++j)
            {
                bool iInside = (0 <= i && i < width);
                bool jInside = (0 <= j && j < height);
                int index = i + expandSize + (j + expandSize) * newWidth;
                Color c = Color.clear;
                if (i <= 0 && jInside)
                {
                    c = oldColor[j * width];
                    c.a = c.a * (i + expandSize) * 1f / (expandSize + 1);
                }
                else if (i >= width - 1 && jInside)
                {
                    c = oldColor[j * width + width - 1];
                    c.a = c.a * (width - 1 + expandSize - i) * 1f / (expandSize + 1);
                }
                else if (j <= 0 && iInside)
                {
                    c = oldColor[i];
                    c.a = c.a * (j + expandSize) * 1f / (expandSize + 1);
                }
                else if (j >= height - 1 && iInside)
                {
                    c = oldColor[width * height - width + i];
                    c.a = c.a * (height - 1 + expandSize - j) * 1f / (expandSize + 1);
                }

                else if (iInside && jInside)
                {
                    c = oldColor[i + j * width];
                }
                newColor[index] = c;
            }
        }
        oldTexture.Resize(newWidth, newHeight);

        oldTexture.SetPixels(newColor);
        oldTexture.Apply();
        RenderTexture.ReleaseTemporary(texture);
        var path = AssetDatabase.GetAssetPath(tex);
        File.WriteAllBytes(path, oldTexture.EncodeToPNG());
    }
    static void Clip(Texture2D tex, int newWidth, int newHeight)
    {
        if (tex == null)
            return;
        var width = tex.width;
        var height = tex.height;
        newWidth = Mathf.Min(width, newWidth);
        newHeight = Mathf.Min(height, newHeight);
        var expandX = (newWidth - width) / 2;
        var expandY = (newHeight - height) / 2;
        var newColor = new Color[newWidth * newHeight];//, tex.format, false);
        RenderTexture texture = RenderTexture.GetTemporary(tex.width, tex.height, 0, RenderTextureFormat.ARGB32);
        texture.DiscardContents();
        Graphics.Blit(tex, texture);
        Texture2D oldTexture = new Texture2D(tex.width, tex.height, tex.format, false);
        RenderTexture.active = texture;
        oldTexture.Resize(texture.width, texture.height);
        oldTexture.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
        oldTexture.Apply();

        var oldColor = oldTexture.GetPixels();
        for (int i = -expandX; i < width + expandX; ++i)
        {
            for (int j = -expandY; j < height + expandY; ++j)
            {
                bool iInside = (0 <= i && i < width);
                bool jInside = (0 <= j && j < height);
                int index = i + expandX + (j + expandY) * newWidth;
                Color c = Color.clear;
                if (i <= 0 && jInside)
                {
                    c = oldColor[j * width];
                    c.a = c.a * (i + expandX) * 1f / (expandY + 1);
                }
                else if (i >= width - 1 && jInside)
                {
                    c = oldColor[j * width + width - 1];
                    c.a = c.a * (width - 1 + expandX - i) * 1f / (expandY + 1);
                }
                else if (j <= 0 && iInside)
                {
                    c = oldColor[i];
                    c.a = c.a * (j + expandX) * 1f / (expandY + 1);
                }
                else if (j >= height - 1 && iInside)
                {
                    c = oldColor[width * height - width + i];
                    c.a = c.a * (height - 1 + expandX - j) * 1f / (expandY + 1);
                }

                else if (iInside && jInside)
                {
                    c = oldColor[i + j * width];
                }

                newColor[index] = c;
            }
        }
        oldTexture.Resize(newWidth, newHeight);

        oldTexture.SetPixels(newColor);
        oldTexture.Apply();
        RenderTexture.ReleaseTemporary(texture);
        var path = AssetDatabase.GetAssetPath(tex);
        File.WriteAllBytes(path, oldTexture.EncodeToPNG());
    }
    public static void ChangeResolution(float scaleX, float scaleY)
    {
        var list = Selection.objects;
        Texture2D tempTexture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
        for (int i = 0; list != null && i < list.Length; ++i)
        {
            ChangeResolution(((list[i] is Sprite) ? (list[i] as Sprite).texture : (list[i] as Texture2D)), tempTexture, scaleX, scaleY);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    public static void ChangeResolution(Texture texture, Texture2D tempTexture, float scaleX, float scaleY)
    {
        var path = AssetDatabase.GetAssetPath(texture);
        RenderTexture rtt = RenderTexture.GetTemporary((int)(texture.width * scaleX), (int)(texture.height * scaleY), 0, RenderTextureFormat.ARGB32);
        rtt.DiscardContents();
        Graphics.Blit(texture, rtt);
        RenderTexture.active = rtt;
        tempTexture.Resize(rtt.width, rtt.height);
        tempTexture.ReadPixels(new Rect(0, 0, rtt.width, rtt.height), 0, 0);
        tempTexture.Apply();

        File.WriteAllBytes(path, tempTexture.EncodeToPNG());
        RenderTexture.ReleaseTemporary(rtt);
        EditorUtility.ClearProgressBar();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}