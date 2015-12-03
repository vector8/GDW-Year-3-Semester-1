using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.Serialization;

public class MyNewWindow : EditorWindow
{

    float weightFocusLowestHealth = 50f;
    float weightFocusStrongestUnit = 50f;
    float weightFocusQuickestUnit = 50f;
    float weightDealMostDamage = 50f;
    
    bool toggleOffensive = false;
    bool toggleFocusSingle = false;
    bool toggleKillingBlow = false;

    [MenuItem("Window/AI Editor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(MyNewWindow));
    }

    void OnGUI()
    {
        GUILayout.Label("Priority Settings", EditorStyles.boldLabel);

        GUILayout.Label("Focus Lowest Health Units");
        weightFocusLowestHealth = EditorGUILayout.Slider(weightFocusLowestHealth, -0f, 100f);
        GUILayout.Label("Focus Strongest Units");
        weightFocusStrongestUnit = EditorGUILayout.Slider(weightFocusStrongestUnit, -0f, 100f);
        GUILayout.Label("Focus Quickest Units");
        weightFocusQuickestUnit = EditorGUILayout.Slider(weightFocusQuickestUnit, -0f, 100f);
        GUILayout.Label("Deal The Most Damage");
        weightDealMostDamage = EditorGUILayout.Slider(weightDealMostDamage, -0f, 100f);

        GUILayout.Space(10f);

        GUILayout.Label("Always", EditorStyles.boldLabel);
        GUIContent checkBox1 = new GUIContent("Offensive Actions", "Shows/hides the text");
        toggleOffensive = EditorGUILayout.Toggle(checkBox1, toggleOffensive);
        GUIContent checkBox2 = new GUIContent("Focus One Enemy", "Shows/hides the text");
        toggleFocusSingle = EditorGUILayout.Toggle(checkBox2, toggleFocusSingle);
        GUIContent checkBox3 = new GUIContent("Go For Killing Blow", "Shows/hides the text");
        toggleKillingBlow = EditorGUILayout.Toggle(checkBox3, toggleKillingBlow);
    }

    void UpdateManager()
    {
        //To be implemented when AI Manager is implemented
    }
}
