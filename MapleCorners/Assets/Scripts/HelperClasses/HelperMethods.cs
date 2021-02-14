// "Citation: Unity 2D Game Developer Course Farming RPG"

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperMethods
{

    /// <summary>
    /// Returns whether there are any components at the cursor location
    /// </summary>
    public static bool GetComponentsAtBoxLocation<T>(out List<T> listComponentAtBoxPosition, Vector2 point, Vector2 size, float angle)
    {
        bool found = false;

        List<T> componentList = new List<T>();

        // Get an array of the 2D colliders in the designated box
        Collider2D[] collider2DArray = Physics2D.OverlapBoxAll(point, size, angle);

        for (int i = 0; i < collider2DArray.Length; i++)
        {
            T tComponent = collider2DArray[i].gameObject.GetComponentInParent<T>();
            if (tComponent != null)
            {
                found = true;
                componentList.Add(tComponent);
            }
            else
            {
                tComponent = collider2DArray[i].gameObject.GetComponentInChildren<T>();
                if (tComponent != null)
                {
                    found = true;
                    componentList.Add(tComponent);
                }
            }
        }

        listComponentAtBoxPosition = componentList;

        return found;
    }
}
