using System;
using UnityEngine;

public interface IToggleable
{
    /// <summary>
    /// The desired state provided as the event argument
    /// Note that the class that implements this interface has to implement the event explicitly, like so:
    /// private event EventHandler<EventArg<bool>> ToggleInternal = (sender, args) => { };
    /// event EventHandler<EventArg<bool>> IToggleable.ToggleInitiated
    /// {
    ///    add { ToggleInternal += value; }
    ///    remove { ToggleInternal -= value; }
    /// }
    /// </summary>
    event EventHandler<EventArg<bool>> ToggleInitiated;
}
