using System;

public class BoolArgs : EventArgs
{
    public readonly bool value;

    public BoolArgs(bool value)
    {
        this.value = value;
    }
}

