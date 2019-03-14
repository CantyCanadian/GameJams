using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorExtention
{
    public static Vector2 Multiply(this Vector2 v1, params Vector2[] v2)
    {
        for(int i = 0; i < v2.Length; i++)
        {
            v1.x *= v2[i].x;
            v1.y *= v2[i].y;
        }

        return v1;
    }

    public static Vector3 Multiply(this Vector3 v1, params Vector3[] v2)
    {
        for (int i = 0; i < v2.Length; i++)
        {
            v1.x *= v2[i].x;
            v1.y *= v2[i].y;
            v1.z *= v2[i].z;
        }

        return v1;
    }

    public static Vector4 Multiply(this Vector4 v1, params Vector4[] v2)
    {
        for (int i = 0; i < v2.Length; i++)
        {
            v1.w *= v2[i].w;
            v1.x *= v2[1].x;
            v1.y *= v2[i].y;
            v1.z *= v2[i].z;
        }

        return v1;
    }
}