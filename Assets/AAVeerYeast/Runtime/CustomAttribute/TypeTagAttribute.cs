using System;

/// <summary>
/// 用于将某些 变量 与 类型 保持关联，辅助代码追踪
/// </summary>
public class TypeTagAttribute : Attribute
{
    public Type TypeTag { get; private set; }

    public TypeTagAttribute(Type type)
    {
        TypeTag = type;
    }
}
