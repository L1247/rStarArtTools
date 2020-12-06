using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class AnimationFrameTool : EditorWindow
{
    public static  AnimationFrameTool instance;
    private static float              _perFrameSecond = 1f / 30f;
    bool                              groupEnabled;
    bool                              myBool   = true;
    float                             myFloat  = 1.23f;
    string                            myString = "Hello World";
    private float                     _frameTime;

    void OnGUI()
    {
        var activeGameObject = Selection.activeGameObject;

        var isPlaying = Application.isPlaying;
        if (activeGameObject != null)
        {
            var animator = activeGameObject.GetComponent<Animator>();
            if (animator != null && isPlaying)
            {
                var clipInfo                 = animator.GetCurrentAnimatorClipInfo(0)[0];
                var currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
                var clip                     = clipInfo.clip;
                var clipWeight               = clipInfo.weight;
                var clipFrameRate            = clip.frameRate;
                var clipLength               = clip.length;
                var frameTime                = 1 / clipFrameRate;
                var normalizedTime           = currentAnimatorStateInfo.normalizedTime;
                var time                     = clipLength * normalizedTime;
                // https://gamedev.stackexchange.com/questions/165289/how-to-fetch-a-frame-number-from-animation-clip
                var currentFrame   = (int)(clip.length * (normalizedTime % 1) * clipFrameRate) + 1;
                var controllerPath = AssetDatabase.GetAssetPath(animator.runtimeAnimatorController);
                var controller =
                    AssetDatabase.LoadAssetAtPath<AnimatorController>(
                        controllerPath);
                GUILayout.Label($"Current Select {activeGameObject.name}" ,              EditorStyles.boldLabel);
                GUILayout.Label($"Animator Name : {controller.name}\n{controllerPath}" , EditorStyles.boldLabel);
                GUILayout.Label($"Current Frame : {currentFrame}");
            }
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
}