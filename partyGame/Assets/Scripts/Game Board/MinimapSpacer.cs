using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapSpacer : MonoBehaviour
{
    LayoutGroup myLayout;
    bool isHorizontal = false;

    int groupCount = 0;

    int[] sizes = new int[6] { 0, -6, -15, -23, -32, -40 };

    void Start()
    {
        myLayout = GetComponent<LayoutGroup>();
        if (myLayout as HorizontalLayoutGroup) isHorizontal = true;
        else isHorizontal = false;
    }

    public void AddToLayout(Transform token)
    {
        if (groupCount == 0) groupCount++;
        else if (groupCount > 0)
        {
            if (isHorizontal)
            {
                var layout = myLayout as HorizontalLayoutGroup;
                layout.padding.left = sizes[groupCount];
            }
            else
            {
                var layout = myLayout as VerticalLayoutGroup;
                layout.padding.top = sizes[groupCount];
            }
            groupCount++;
        }
        else Debug.LogError("invalid groupCount: " + groupCount);

        token.SetParent(transform);
    }

    public void RemoveFromLayout()
    {
        if (groupCount == 1) groupCount--;
        else if(groupCount > 1)
        {
            groupCount--;
            if (isHorizontal)
            {
                var layout = myLayout as HorizontalLayoutGroup;
                layout.padding.left = sizes[groupCount - 1];
            }
            else
            {
                var layout = myLayout as VerticalLayoutGroup;
                layout.padding.top = sizes[groupCount - 1];
            }
        }
        else Debug.LogError("invalid groupCount: " + groupCount);
    }
}
