using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
    public static float upGrade(int level)
    {
        return (level / 2 - 0.5f) * 0.5f; ;
    }
}
