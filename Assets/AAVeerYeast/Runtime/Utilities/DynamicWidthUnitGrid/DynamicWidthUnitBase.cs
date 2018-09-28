using UnityEngine;

public abstract class DynamicWidthUnitBase : MonoBehaviour
{
    public abstract Vector2 GetSizeDelta();

    public abstract void ApplyGridSize(Vector2 sizeDelta);

    public abstract void ApplyLocalPosition(Vector2 localPosition);

    public abstract float GetConstHeight();

    public void ApplyGridSizeAuto()
    {
        ApplyGridSize(GetSizeDelta());
    }
}