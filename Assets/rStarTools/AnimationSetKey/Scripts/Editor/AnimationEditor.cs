using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using rStarTools.ScriptableObjects;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Random = UnityEngine.Random;

namespace rStar.Editor
{
    public static class AnimationEditor
    {
    #region Public Methods

        [MenuItem("Tools/AnimationEditor/AddKey1 %q")]
        public static void AddKey1()
        {
            AddKeyInternal("Q");
        }

        private static string GetPropertyName()
        {
            return "Breath";
        }

        [MenuItem("Tools/AnimationEditor/AddKey2 %w")]
        public static void AddKey2()
        {
            AddKeyInternal("W");
        }

        [MenuItem("Tools/AnimationEditor/AddKey3 %e")]
        public static void AddKey3()
        {
            AddKeyInternal("E");
        }

        [MenuItem("Tools/AnimationEditor/AddKey4 %r")]
        public static void AddKey4()
        {
            AddKeyInternal("R");
        }

        [MenuItem("Tools/AnimationEditor/AddKey5 %t")]
        public static void AddKey5()
        {
            AddKeyInternal("T");
        }

        public static AnimationKeyData GetAnimationKeyData(string keyCode)
        {
            var              animationKeyDatas = GetScriptableObjects<AnimationKeyData>();
            AnimationKeyData animationKeyData  = animationKeyDatas.Find(data => data.name.Contains(keyCode));
            return animationKeyData;
        }

        public static T GetScriptableObject<T>() where T : ScriptableObject
        {
            return GetScriptableObjects<T>().First();
        }

        public static List<T> GetScriptableObjects<T>() where T : ScriptableObject
        {
            var ts   = new List<T>();
            var type = typeof(T);
        #if UNITY_EDITOR
            var guids2 = AssetDatabase.FindAssets(string.Format("t:{0}" , type));
            foreach (var guid2 in guids2)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid2);
                ts.Add((T)AssetDatabase.LoadAssetAtPath(assetPath , type));
            }
        #endif
            return ts;
        }

    #endregion

    #region Private Methods

        private static void AddKeyInCurrentTime(float value , string targetPropertName)
        {
            AnimationCurve targetCurve   = null;
            var            targetBinding = GetCurveAndBinding(targetPropertName , ref targetCurve);
            if (targetCurve != null)
            {
                var animationCurveKeys = targetCurve.keys;
                var currentTime        = GetCurrentTime();
                var curveKeys          = animationCurveKeys.ToList();
                var sameKeyIndex       = curveKeys.FindIndex(kf => kf.time == currentTime);
                if (sameKeyIndex >= 0) targetCurve.RemoveKey(sameKeyIndex);
                targetCurve.AddKey(currentTime , value);
                AnimationUtility.SetEditorCurve(GetActiveAnimationClip() , targetBinding , targetCurve);
                EditorUtility.SetDirty(GetActiveAnimationClip());
                InternalEditorUtility.RepaintAllViews();
            }
            else
            {
                Debug.LogError(string.Format("Cloud Not Found Curve by curveName : {0}" , targetPropertName));
            }
        }

        private static void AddKeyInternal(string keyCode)
        {
            if (GetActiveAnimationClip() != null)
            {
                var animationKeyData = GetAnimationKeyData(keyCode);
                var value            = animationKeyData.GetValue();
                var propertyName     = animationKeyData.propertyName;
                AddKeyInCurrentTime(value , propertyName);
            }
        }

        private static AnimationClip GetActiveAnimationClip()
        {
            return GetPropertyValueOfWindowsState<AnimationClip>("activeAnimationClip");
        }

        private static float GetCurrentTime()
        {
            return GetPropertyValueOfWindowsState<float>("currentTime");
        }

        private static EditorCurveBinding GetCurveAndBinding(string targetPropertyName , ref AnimationCurve targetCurve)
        {
            AnimationClip      clip          = GetActiveAnimationClip();
            var                bindings      = AnimationUtility.GetCurveBindings(clip);
            EditorCurveBinding targetBinding = default;
            foreach (var curveBinding in bindings)
            {
                var propertyName = curveBinding.propertyName;
                if (propertyName == targetPropertyName)
                {
                    var curve = AnimationUtility.GetEditorCurve(clip , curveBinding);
                    targetBinding = curveBinding;
                    targetCurve   = curve;
                }
            }

            return targetBinding;
        }

        private static T GetPropertyValueOfWindowsState<T>(string propertyName)
        {
            var type         = GetTypeOfAssembly("UnityEditorInternal.AnimationWindowState");
            var instances    = Resources.FindObjectsOfTypeAll(type);
            var propertyInfo = GetPublicPropertyInfo(type , propertyName);
            var value        = instances.Length > 0 ? (T)propertyInfo.GetValue(instances[0]) : default(T);
            return value;
        }

        private static PropertyInfo GetPublicPropertyInfo(Type type , string propertyName)
        {
            PropertyInfo propertyInfo = type.GetProperty(propertyName , BindingFlags.Instance | BindingFlags.Public);
            return propertyInfo;
        }

        private static Type GetTypeOfAssembly(string typeName)
        {
            Type windowType = GetUnityEditorAssembly().GetType(typeName);
            return windowType;
        }

        private static Assembly GetUnityEditorAssembly()
        {
            Assembly editorAssembly = typeof(UnityEditor.Editor).Assembly;
            return editorAssembly;
        }

        private static float GetValue(string keyCode)
        {
            var value = GetAnimationKeyData(keyCode).GetValue();
            return value;
        }

    #endregion
    }
}