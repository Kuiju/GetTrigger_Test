using System;

public class DefaultResetAttribute : Attribute
{
    public object DefaultResetValue = null;

    public DefaultResetAttribute() { }

    public DefaultResetAttribute(object defaultResetValue)
    {
        this.DefaultResetValue = defaultResetValue;
    }
}
