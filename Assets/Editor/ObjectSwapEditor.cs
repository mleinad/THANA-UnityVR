using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ObjectSwap))]
public class ObjectSwapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // Draws the default inspector for other fields

        ObjectSwap swap = (ObjectSwap)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Alternative Memory Objects", EditorStyles.boldLabel);

        if (swap.transform.childCount == 0)
        {
            EditorGUILayout.HelpBox("No child objects found.", MessageType.Info);
            return;
        }

        for (int i = 0; i < swap.transform.childCount; i++)
        {
            Transform child = swap.transform.GetChild(i);
            AlternativeMemoryObject alt = child.GetComponent<AlternativeMemoryObject>();
            if (alt == null) continue;

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField($"Variant {i}: {child.name}", EditorStyles.boldLabel);

            // Allow selection toggles to remain enabled
            alt.isDefault = EditorGUILayout.Toggle("Is Default", alt.isDefault);
            alt.isSelected = EditorGUILayout.Toggle("Is Selected", alt.isSelected);

            // Grayed out section
            GUI.enabled = alt.isSelected;

            EditorGUILayout.LabelField("Emotional Values");
            alt.emotions.anger = EditorGUILayout.Slider("Anger", alt.emotions.anger, 0f, 1f);
            alt.emotions.suspicion = EditorGUILayout.Slider("Suspicion", alt.emotions.suspicion, 0f, 1f);
            alt.emotions.happiness = EditorGUILayout.Slider("Happiness", alt.emotions.happiness, 0f, 1f);
            alt.emotions.regret = EditorGUILayout.Slider("Regret", alt.emotions.regret, 0f, 1f);

            GUI.enabled = true; // Reset GUI state
            EditorGUILayout.EndVertical();
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(swap);
        }
    }
}