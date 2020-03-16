using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Global
{
    public static bool LessEqual(this Vector2 _left, Vector2 _right)
    {
        return (_left.x <= _right.x && _left.y <= _right.y);
    }
    public static bool MoreEqual(this Vector2 _left, Vector2 _right)
    {
        return (_left.x >= _right.x && _left.y >= _right.y);
    }
}
