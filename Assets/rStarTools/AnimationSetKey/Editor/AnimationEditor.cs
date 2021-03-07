using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Main.Editor
{
    public class AnimationEditor
    {
        [MenuItem("Tools/AnimationEditor/AddKey1 %q")]
        public static void AddKey1()
        {
            Debug.Log($" AddKey1");
            Assembly editorAssembly = typeof(UnityEditor.Editor).Assembly;

            Type windowType = editorAssembly.GetType
                ("UnityEditorInternal.AnimationWindowState");

            // Get a reference to the unbound property we want to access.
            PropertyInfo isRecordingProp = windowType.GetProperty
            ("activeAnimationClip" , BindingFlags.Instance |
                                     BindingFlags.Public);

            // Get all instances of objects of type "AnimationWindowState"
            // (There should be exactly one of these, unless the window is closed)
            Object[] windowInstances = Resources.FindObjectsOfTypeAll(windowType);

            for (int i = 0 ; i < windowInstances.Length ; i++)
            {
                AnimationClip obj = (AnimationClip)isRecordingProp.GetValue
                    (windowInstances[i] , null);
                Debug.Log($" {obj}");
                AddKeyInCurrentTime(obj);
            }
        }

        private static void AddKeyInCurrentTime(AnimationClip clip)
        {
            var                bindings           = AnimationUtility.GetCurveBindings(clip);
            AnimationCurve     breathCurve        = null;
            EditorCurveBinding breathCurveBinding = default;
            foreach (var curveBinding in bindings)
            {
                var propertyName = curveBinding.propertyName;
                if (propertyName == "Breath")
                {
                    var curve = AnimationUtility.GetEditorCurve(clip , curveBinding);
                    breathCurveBinding = curveBinding;
                    breathCurve        = curve;
                }
            }

            if (breathCurve == null) return;
            var breathCurveKeys = breathCurve.keys;
            var currentTime     = GetCurrentTime();
            var value           = Random.value;
            var curveKeys       = breathCurveKeys.ToList();
            var sameKeyIndex    = curveKeys.FindIndex(kf => kf.time == currentTime);
            if (sameKeyIndex >= 0) breathCurve.RemoveKey(sameKeyIndex);
            breathCurve.AddKey(currentTime , value);
            AnimationUtility.SetEditorCurve(clip , breathCurveBinding , breathCurve);
            EditorUtility.SetDirty(clip);
            InternalEditorUtility.RepaintAllViews();
        }

        public static float GetCurrentTime()
        {
            Assembly editorAssembly = typeof(UnityEditor.Editor).Assembly;

            Type windowType = editorAssembly.GetType
                ("UnityEditorInternal.AnimationWindowState");

            // Get a reference to the unbound property we want to access.
            PropertyInfo propertyInfo = windowType.GetProperty
            ("currentTime" , BindingFlags.Instance |
                             BindingFlags.Public);

            // Get all instances of objects of type "AnimationWindowState"
            // (There should be exactly one of these, unless the window is closed)
            Object[] windowInstances = Resources.FindObjectsOfTypeAll(windowType);

            for (int i = 0 ; i < windowInstances.Length ; i++)
            {
                float currentTime = (float)propertyInfo.GetValue
                    (windowInstances[i] , null);


                return currentTime;
            }

            return 0f;
        }
    }
}