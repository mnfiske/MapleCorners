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

    /// <summary>
    /// Returns true if a component is found, out varible holds the found component(s)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="componentsAtPositionList"></param>
    /// <param name="positionToCheck"></param>
    /// <returns></returns>
    public static bool GetComponentsAtCursorLocation<T>(out List<T> componentsAtPositionList, Vector3 positionToCheck)
    {
        bool found = false;

        List<T> componentList = new List<T>();

        // Get all colliders that overlap the position that was passed in
        Collider2D[] collider2DArray = Physics2D.OverlapPointAll(positionToCheck);

        T tComponent = default(T);
        // Loop through all the colliders found and check if they are of the specified type. If found, set found to true and add the component
        // to the componentList
        for (int i = 0; i < collider2DArray.Length; i++)
        {
            tComponent = collider2DArray[i].gameObject.GetComponentInParent<T>();
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

        componentsAtPositionList = componentList;

        return found;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="numberOfCollidersToTest"></param>
    /// <param name="point"></param>
    /// <param name="size"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static T[] GetComponentsAtBoxLocationNonAlloc<T>(int numberOfCollidersToTest, Vector2 point, Vector2 size, float angle)
    {
        Collider2D[] collider2DArray = new Collider2D[numberOfCollidersToTest];

        // Store all colliders within the box area in the collider2DArray
        Physics2D.OverlapBoxNonAlloc(point, size, angle, collider2DArray);

        T tComponent = default(T);

        T[] componentArray = new T[collider2DArray.Length];

        // Loop through the found colliders
        for (int i = collider2DArray.Length - 1; i >= 0; i--)
        {
            if (collider2DArray[i] != null)
            {
                // Get the component of the type from the game object
                tComponent = collider2DArray[i].gameObject.GetComponent<T>();

                // Check if such a component exists
                if (tComponent != null)
                {
                    // If so, add it to the componentArray
                    componentArray[i] = tComponent;
                }
            }
        }

        return componentArray;
    }
}
