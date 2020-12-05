using UnityEngine;
using UnityEditor;

public class AnimationFrameTool : EditorWindow
{
    string myString = "Hello World";
    bool   groupEnabled;
    bool   myBool  = true;
    float  myFloat = 1.23f;

    public static  AnimationFrameTool instance;
    private static float              _perFrameSecond = 1f / 30f;

    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/AnimationFrameTool _F1")]
    static void Init()
    {
        if (instance == null)
        {
            // Get existing open window or if none, make a new one:
            AnimationFrameTool window = (AnimationFrameTool)EditorWindow.GetWindow(typeof(AnimationFrameTool));
            window.Show();
            instance = window;
        }
        else instance.Focus();
    }

    void OnGUI()
    {
        var activeGameObject = Selection.activeGameObject;

        if (activeGameObject != null)
        {
            GUILayout.Label($"Current Select {activeGameObject.name}" , EditorStyles.boldLabel);
            var animator  = activeGameObject.GetComponent<Animator>();
            var isPlaying = Application.isPlaying;
            if (animator != null && isPlaying)
            {
                var clipInfo                 = animator.GetCurrentAnimatorClipInfo(0)[0];
                var currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
                var clipWeight               = clipInfo.weight;
                var clip                     = clipInfo.clip;
                var clipFrameRate            = clip.frameRate;
                var clipLength               = clip.length;
                var length                   = currentAnimatorStateInfo.length;
                var normalizedTime           = currentAnimatorStateInfo.normalizedTime;
                var currentFrame             = /*(int)*/(clipWeight * (normalizedTime * 30));
                Debug.Log($"{clipFrameRate} , {clipWeight} , {normalizedTime} , {currentFrame}");
            }
        }
    }
}