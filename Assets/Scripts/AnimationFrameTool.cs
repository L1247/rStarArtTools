using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class AnimationFrameTool : EditorWindow
{
    public static  AnimationFrameTool instance;
    private static float              _perFrameSecond = 1f / 30f;
    bool                              groupEnabled;
    bool                              myBool       = true;
    float                             myFloat      = 1.23f;
    string                            myString     = "Hello World";
    private int                       currentFrame = 1;
    private float                     _frameTime;

    void OnGUI()
    {
        if (currentFrame == 0) currentFrame = 1;
        var activeGameObject                = Selection.activeGameObject;

        var isPlaying = Application.isPlaying;
        if (activeGameObject != null)
        {
            var animator = activeGameObject.GetComponent<Animator>();
            if (animator != null && isPlaying)
            {
                var   clipInfo                 = animator.GetCurrentAnimatorClipInfo(0)[0];
                var   currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
                var   clip                     = clipInfo.clip;
                var   clipWeight               = clipInfo.weight;
                var   clipFrameRate            = clip.frameRate;
                var   clipLength               = clip.length;
                var   frameTime                = 1 / clipFrameRate;
                var   normalizedTime           = currentAnimatorStateInfo.normalizedTime;
                float time                     = clipLength * normalizedTime;
                // if (time >= frameTime) currentFrame++;
                var calculateFrame = (time / (frameTime * currentFrame));
                if (calculateFrame >= 1) currentFrame++;
                // currentFrame = (int)Mathf.Max(currentFrame , calculateFrame);
                Debug.Log($"{time} , {frameTime} , {currentFrame} , {calculateFrame}");
            }

            var controller =
                AssetDatabase.LoadAssetAtPath<AnimatorController>(
                    AssetDatabase.GetAssetPath(animator.runtimeAnimatorController));
            GUILayout.Label($"Current Select {activeGameObject.name}" , EditorStyles.boldLabel);
            GUILayout.Label($"Animator Name : {controller.name}" ,           EditorStyles.boldLabel);
            GUILayout.Label($"Current Frame : {currentFrame}");
        }
    }

    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/AnimationFrameTool _F1")]
    static void Init()
    {
        if (instance == null)
        {
            // Get existing open window or if none, make a new one:
            AnimationFrameTool window = (AnimationFrameTool)GetWindow(typeof(AnimationFrameTool));
            window.Show();
            instance = window;
        }
        else instance.Focus();
    }

    private void OnEnable()
    {
        EditorApplication.playModeStateChanged += change => { OnChangeAction(change); };
    }

    private void OnChangeAction(PlayModeStateChange change)
    {
        if (change == PlayModeStateChange.EnteredPlayMode) currentFrame = 1;
        if (change == PlayModeStateChange.ExitingPlayMode) currentFrame = 1;
    }
}