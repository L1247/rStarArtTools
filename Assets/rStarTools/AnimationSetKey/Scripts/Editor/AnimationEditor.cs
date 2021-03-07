using System;
using System.Linq;
using System.Reflection;
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
            AddKeyInternal(Random.value , "Breath");
        }

    #endregion

    #region Private Methods

        private static void AddKeyInCurrentTime(float value , string targetCurveName)
        {
            AnimationCurve targetCurve   = null;
            var            targetBinding = GetCurveAndBinding(targetCurveName , ref targetCurve);
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
                Debug.LogError($"Cloud Not Found Curve by curveName : {targetCurveName}");
            }
        }

        private static EditorCurveBinding GetCurveAndBinding(string targetCurveName , ref AnimationCurve targetCurve)
        {
            AnimationClip      clip          = GetActiveAnimationClip();
            var                bindings      = AnimationUtility.GetCurveBindings(clip);
            EditorCurveBinding targetBinding = default;
            foreach (var curveBinding in bindings)
            {
                var propertyName = curveBinding.propertyName;
                if (propertyName == targetCurveName)
                {
                    var curve = AnimationUtility.GetEditorCurve(clip , curveBinding);
                    targetBinding = curveBinding;
                    targetCurve   = curve;
                }
            }

            return targetBinding;
        }

        private static void AddKeyInternal(float value , string targetCurveName)
        {
            if (GetActiveAnimationClip() != null)
                AddKeyInCurrentTime(value , targetCurveName);
        }

        private static AnimationClip GetActiveAnimationClip()
        {
            return GetPropertyValueOfWindowsState<AnimationClip>("activeAnimationClip");
        }

        private static float GetCurrentTime()
        {
            return GetPropertyValueOfWindowsState<float>("currentTime");
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

    #endregion
    }
}