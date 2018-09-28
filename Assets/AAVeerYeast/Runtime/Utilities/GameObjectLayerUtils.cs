public enum EGameObjectLayer
{
    Default = 0,
    TransparentFX = 1,
    IgnoreRaycast = 2,
    Water = 4,
    UI = 5,
    FrontUI = 8,
    Left = 9,
    Right = 10
}

public static class GameObjectLayerUtils
{
    public static int GetLayerId(EGameObjectLayer layer)
    {
        return (int)layer;
    }
}
