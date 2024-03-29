// ReflectionUtil.cs can be used as is and in T4 template
// <#+ /*

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
// */
public static class ReflectionUtil
{
    public static string GetTypeName(Type t) => t.Name;

    public static string[] GetPublicFieldsNames(Type t)
    {
        var bindingFlags = System.Reflection.BindingFlags.Instance |
                            System.Reflection.BindingFlags.Public;
        return t.GetFields(bindingFlags).Select(field => field.Name).ToArray(); ;
    }
    public static string[] GetFieldsNames(Type t)
    {
        var bindingFlags = System.Reflection.BindingFlags.Instance |
                            System.Reflection.BindingFlags.NonPublic |
                            System.Reflection.BindingFlags.Public;
        return t.GetFields(bindingFlags).Select(field => field.Name).ToArray(); ;
    }
}
// #>