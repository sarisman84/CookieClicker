using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DOTweenManager.Editor
{
    [CustomPropertyDrawer(typeof(DoTweenSettings))]
    public class DoTweenSettingsDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            Rect pos = position;
            pos.height = EditorGUIUtility.singleLineHeight * 1.15f;
            SerializedProperty tweenTypeProp = property.FindPropertyRelative("tweenType");
            EditorGUI.PropertyField(pos, tweenTypeProp, new GUIContent(tweenTypeProp.displayName));
            pos.y += pos.height;
            DrawElements(ref pos, property, tweenTypeProp);
            property.serializedObject.ApplyModifiedProperties();
        }

        private void DrawElements(ref Rect pos, SerializedProperty property, SerializedProperty tweenTypeProp)
        {
            string value = GetPropertyPathFromTargetTweenType(property);
            SerializedProperty iterator = property.serializedObject.GetIterator();
            iterator.Next(true);
            while (iterator.NextVisible(true))
            {
                if (iterator.propertyPath.ToLower().Contains(property.propertyPath.ToLower()) &&
                    iterator.propertyPath.ToLower().Contains(value) &&
                    DoesNotContainVector3Components(iterator.propertyPath))
                {
                    if (value.Equals("rotation") && iterator.propertyType == SerializedPropertyType.Quaternion)
                    {
                        Vector3 eulerAngles = iterator.quaternionValue.eulerAngles;
                        Vector3 rot = EditorGUI.Vector3Field(pos, new GUIContent(iterator.displayName),
                            new Vector3(eulerAngles.x, eulerAngles.y,
                                eulerAngles.z));
                        iterator.quaternionValue = Quaternion.Euler(rot);
                    }
                    else if (value.Equals("fade") && (
                        iterator.propertyPath.Contains("uponFadeCompletionDisableGameObject") ||
                        iterator.propertyPath.Contains("uponFadeCompletionDestroyGameObject")))
                    {
                        if (tweenTypeProp.boolValue)
                        {
                            EditorGUI.PropertyField(pos, iterator, new GUIContent(iterator.displayName));
                        }
                    }
                    else
                        EditorGUI.PropertyField(pos, iterator, new GUIContent(iterator.displayName));

                    pos.y += pos.height;
                }
            }

            SerializedProperty p = property.FindPropertyRelative("playBelowTweenAfterCompletingCurrentTween");
            EditorGUI.PropertyField(pos, p, new GUIContent(p.displayName));
        }

        private bool DoesNotContainVector3Components(string iteratorPropertyPath)
        {
            return !iteratorPropertyPath.ToLower().Contains(".x") && !iteratorPropertyPath.ToLower().Contains(".y") &&
                   !iteratorPropertyPath.ToLower().Contains(".z") && !iteratorPropertyPath.ToLower().Contains(".w");
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int totalElements = 2;
            SerializedProperty iterator = property.serializedObject.GetIterator();
            iterator.Next(true);
            while (iterator.NextVisible(true))
            {
                if (iterator.propertyPath.ToLower().Contains(property.propertyPath.ToLower()) &&
                    iterator.propertyPath.ToLower().Contains(GetPropertyPathFromTargetTweenType(property)) &&
                    DoesNotContainVector3Components(iterator.propertyPath))
                {
                    totalElements++;
                }
            }

            return (base.GetPropertyHeight(property, label) * totalElements) + EditorGUIUtility.singleLineHeight;
        }

        private string GetPropertyPathFromTargetTweenType(SerializedProperty property)
        {
            SerializedProperty tweenType = property.FindPropertyRelative("tweenType");
            switch (tweenType.enumValueIndex)
            {
                case 0:
                    return "movement";

                case 1:
                    return "scale";

                case 2:
                    return "rotation";

                case 3:
                    return "fade";

                default:
                    return "";
            }
        }
    }
}