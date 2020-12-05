using UnityEngine;
using UnityEditor;

public class AnimationFrameTool : EditorWindow
{
    string myString = "Hello World";
    bool   groupEnabled;
    bool   myBool  = true;
    float  myFloat = 1.23f;

    public static AnimationFrameTool instance;

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
            var animator = activeGameObject.GetComponent<Animator>();
            if (animator != null)
            {
                myString = EditorGUILayout.TextField("Text Field" , myString);

                groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings" , groupEnabled);
                myBool       = EditorGUILayout.Toggle("Toggle" , myBool);
                myFloat      = EditorGUILayout.Slider("Slider" , myFloat , -3 , 3);
                EditorGUILayout.EndToggleGroup();
            }
        }
    }
}