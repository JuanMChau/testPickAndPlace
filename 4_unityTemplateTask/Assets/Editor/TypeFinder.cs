using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;

public class TypeFinder
{
    [MenuItem("Tools/Find ConnectionHandler Type")]
    static void FindType()
    {
        string typeName = "ConnectionHandler";
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        Debug.Log($"Assemblies loaded: {assemblies.Length}");
        foreach (var asm in assemblies.OrderBy(a=>a.FullName))
        {
            try
            {
                var type = asm.GetType(typeName);
                if (type != null)
                {
                    Debug.Log($"Found {typeName} in assembly: {asm.GetName().Name} (FullName: {asm.FullName})");
                    Debug.Log($"Type namespace: '{type.Namespace ?? "<global>"}'  IsSubclassOf(MonoBehaviour): {type.IsSubclassOf(typeof(MonoBehaviour))}");
                    return;
                }
            }
            catch(Exception e)
            {
                Debug.Log($"Error inspecting assembly {asm?.GetName().Name}: {e.Message}");
            }
        }
        Debug.Log($"Type '{typeName}' not found in any loaded assembly.");
    }
}
