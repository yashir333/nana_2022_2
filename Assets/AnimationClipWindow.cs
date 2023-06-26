using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class AnimationClipWindow : EditorWindow
{
    private Vector2 scrollPosition;
    private AnimationClip[] animationClips;
    private string searchQuery = string.Empty;

    [MenuItem("Window/Animation Clip Window")]
    public static void ShowWindow()
    {
        AnimationClipWindow window = GetWindow<AnimationClipWindow>();
        window.titleContent = new GUIContent("Animation Clips");
        window.Show();
    }

    private void OnEnable()
    {
        // Get all animation clips in the project and sort them alphabetically
        animationClips = GetAllAnimationClips().OrderBy(clip => clip.name).ToArray();
    }

    private void OnGUI()
    {
        GUILayout.Label("Animation Clips:");

        // Search bar
        GUILayout.BeginHorizontal();
        GUILayout.Label("Search:", GUILayout.Width(50));
        searchQuery = GUILayout.TextField(searchQuery);
        GUILayout.EndHorizontal();

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        foreach (AnimationClip clip in animationClips)
        {
            // Filter animation clips based on search query
            if (!string.IsNullOrEmpty(searchQuery) &&
                !clip.name.ToLower().Contains(searchQuery.ToLower()) &&
                !GetParentFBXName(clip).ToLower().Contains(searchQuery.ToLower()))
                continue;

            GUILayout.BeginHorizontal();

            // Display animation clip name with parent FBX name if it exists
            string displayName = GetDisplayName(clip);

            // Select animation clip in the project by clicking on its name
            if (GUILayout.Button(displayName, EditorStyles.label))
            {
                Selection.activeObject = clip;
                EditorGUIUtility.PingObject(clip);
            }

            GUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();
    }

    private AnimationClip[] GetAllAnimationClips()
    {
        string[] guids = AssetDatabase.FindAssets("t:AnimationClip");
        AnimationClip[] clips = new AnimationClip[guids.Length];
        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            clips[i] = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
        }
        return clips;
    }

    private string GetDisplayName(AnimationClip clip)
    {
        string parentFBXName = GetParentFBXName(clip);
        if (!string.IsNullOrEmpty(parentFBXName))
            return $"{parentFBXName} > {clip.name}";

        return clip.name;
    }

    private string GetParentFBXName(AnimationClip clip)
    {
        Object parentObject = AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.GetAssetPath(clip));

        if (parentObject != null)
        {
            GameObject parentGameObject = PrefabUtility.GetCorrespondingObjectFromOriginalSource(parentObject) as GameObject;

            if (parentGameObject != null)
            {
                string parentFBXPath = AssetDatabase.GetAssetPath(parentGameObject);

                if (!string.IsNullOrEmpty(parentFBXPath))
                {
                    int startIndex = parentFBXPath.LastIndexOf('/') + 1;
                    int endIndex = parentFBXPath.LastIndexOf('.');
                    if (endIndex > startIndex)
                    {
                        return parentFBXPath.Substring(startIndex, endIndex - startIndex);
                    }
                }
            }
        }

        return string.Empty;
    }
}
