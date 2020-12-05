using UnityEngine;
using UnityEditor;

public class AnimationFrameTool : EditorWindow
{
    public static  AnimationFrameTool instance;
    private static float              _perFrameSecond = 1f / 30f;
    bool                              groupEnabled;
    bool                              myBool       = true;
    float                             myFloat      = 1.23f;
    string                            myString     = "Hello World";
    private int                       currentFrame = 0;

    void OnGUI()
    {
        var activeGameObject = Selection.activeGameObject;

        var isPlaying = Application.isPlaying;
        if (activeGameObject != null)
        {
            GUILayout.Label($"Current Select {activeGameObject.name}" , EditorStyles.boldLabel);
            var animator = activeGameObject.GetComponent<Animator>();
            if (animator != null && isPlaying)
            {
                var   clipInfo                 = animator.GetCurrentAnimatorClipInfo(0)[0];
                var   currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
                var   clip                     = clipInfo.clip;
                var   clipWeight               = clipInfo.weight;
                var   clipFrameRate            = clip.frameRate;
                var   clipLength               = clip.length;
                var   frameTime                = 1 / clipFrameRate * currentFrame;
                var   normalizedTime           = currentAnimatorStateInfo.normalizedTime;
                float time                     = clipLength * normalizedTime;
                if (time >= frameTime) currentFrame++;
                Debug.Log($"{time} , {normalizedTime} , {currentFrame - 1}");
            }
        }

        if (isPlaying == false)
        {
            currentFrame = 0;
        }
    }

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
}