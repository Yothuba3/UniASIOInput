using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEditor;

namespace Yothuba.Asio.Editor
{
    //KlakNdiのEditor/Utilityスクリプト引用
    struct AutoProperty
    {
        SerializedProperty _prop;

        public SerializedProperty Target => _prop;

        public AutoProperty(SerializedProperty prop)
            => _prop = prop;

        public static implicit operator
            SerializedProperty(AutoProperty prop) => prop._prop;

        public static void Scan<T>(T target) where T : UnityEditor.Editor
        {
            var so = target.serializedObject;

            var flags = BindingFlags.Public | BindingFlags.NonPublic;
            flags |= BindingFlags.Instance;

            foreach (var f in typeof(T).GetFields(flags))
                if (f.FieldType == typeof(AutoProperty))
                    f.SetValue(target, new AutoProperty(so.FindProperty(f.Name)));
        }
    }
}