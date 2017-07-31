using UnityEngine;
using System;
using System.Collections.Generic;
using Helpers.Extensions;

namespace Helpers
{
    /// <summary>
    /// Selection controller class that works together with SelectableWorldObjects.
    /// Inherit and implement custom selection logic. Can also be used from outside, if that's preferred.
    /// </summary>
    public class WorldObjectSelector : MonoBehaviour
    {
        public static EventHandler<EventArg<SelectableWorldObject>> ObjectHighlighted = (sender, args) => { };
        public static EventHandler<EventArg<SelectableWorldObject>> ObjectUnhighlighted = (sender, args) => { };
        public static EventHandler<EventArg<SelectableWorldObject>> ObjectSelected = (sender, args) => { };
        public static EventHandler<EventArg<SelectableWorldObject>> ObjectDeselected = (sender, args) => { };

        protected HashSet<SelectableWorldObject> highlightedObjects = new HashSet<SelectableWorldObject>();
        protected HashSet<SelectableWorldObject> selectedObjects = new HashSet<SelectableWorldObject>();

        private static WorldObjectSelector _instance;
        public static WorldObjectSelector Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<WorldObjectSelector>();
                }
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (Instance != this)
            {
                Debug.LogWarning("[WorldObjectSelector] Another instance found. Destroying self.");
                Destroy(this);
            }
        }

        public void ToggleSelect(SelectableWorldObject target)
        {
            if (target != null)
            {
                if (target.IsSelected) { Deselect(target); }
                else { Select(target); }
            }
        }

        public void Select(SelectableWorldObject target)
        {
            target.Select(true);
            selectedObjects.Add(target);
            ObjectSelected(this, new EventArg<SelectableWorldObject>(target));
        }

        public void Deselect(SelectableWorldObject target)
        {
            target.Select(false);
            selectedObjects.Remove(target);
            ObjectDeselected(this, new EventArg<SelectableWorldObject>(target));
        }

        public void Highlight(SelectableWorldObject target)
        {
            if (!target.IsHighlighted)
            {
                highlightedObjects.Add(target);
                target.Highlight(true);
                ObjectHighlighted(this, new EventArg<SelectableWorldObject>(target));
            }
        }

        public void Unhighlight(SelectableWorldObject target)
        {
            if (target.IsHighlighted)
            {
                highlightedObjects.Remove(target);
                target.Highlight(false);
                ObjectUnhighlighted(this, new EventArg<SelectableWorldObject>(target));
            }
        }

        public void DeselectAll()
        {
            Deselect(selectedObjects);
        }

        public void UnhighlightAll()
        {
            Unhighlight(highlightedObjects);
        }

        protected void Select(IEnumerable<SelectableWorldObject> targets)
        {
            targets.ForEachMod(t => Select(t));
        }

        protected void Deselect(IEnumerable<SelectableWorldObject> targets)
        {
            targets.ForEachMod(t => Deselect(t));
        }

        protected void Highlight(IEnumerable<SelectableWorldObject> targets)
        {
            targets.ForEachMod(t => Highlight(t));
        }

        protected void Unhighlight(IEnumerable<SelectableWorldObject> targets)
        {
            targets.ForEachMod(t => Unhighlight(t));
        }

        protected void ToggleSelect(IEnumerable<SelectableWorldObject> targets)
        {
            targets.ForEachMod(t => ToggleSelect(t));
        }
    }
}
