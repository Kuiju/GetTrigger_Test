using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicWidthUnitGrid : MonoBehaviour
{
    public RectTransform UnitRoot;

    public float LeftPad = 10f;
    public float RightPad = 10f;
    public float UpPad = 10f;
    public float DownPad = 10f;

    private List<DynamicWidthUnitBase> _UnitList = new List<DynamicWidthUnitBase>();

    public void RegisterDynamicWidthUnit(DynamicWidthUnitBase unit)
    {
#if UNITY_EDITOR
        if (_UnitList.Contains(unit))
        {
            VeerDebug.LogError(" try to register a existing DynamicWidthUnitBase...");
            return;
        }
#endif
        _UnitList.Add(unit);
        unit.transform.SetParent(UnitRoot);
    }

    public Vector2 ApplyDynamicWidthUnitGridLayout()
    {
        Vector2 size = this.Rect().sizeDelta - new Vector2(LeftPad + RightPad, UpPad + DownPad);
        float currentLineWidth = 0;
        int currentLine = 0;

        _UnitList.ForEach(unit =>
        {
            if (!unit.gameObject.activeSelf)
            {
                return;
            }

            Vector2 unitSize = unit.GetSizeDelta();
            unit.ApplyGridSize(unitSize);

            float locationX = currentLineWidth;
            if (locationX + unitSize.x > size.x)
            {
                currentLine++;
                locationX = 0;
            }

            unit.ApplyLocalPosition(new Vector2(LeftPad + locationX, -UpPad - currentLine * unit.GetConstHeight()));
            currentLineWidth = locationX + unitSize.x;
        });

        float finalHeight = 0;
        if (_UnitList.Count != 0)
        {
            finalHeight = (currentLine + 1) * _UnitList[0].GetConstHeight() + UpPad;
        }

        Vector2 finalApplySize = new Vector2(this.Rect().sizeDelta.x, finalHeight);
        UnitRoot.sizeDelta = finalApplySize;
        return finalApplySize;
    }
}
