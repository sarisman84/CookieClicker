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

            switch (tweenTypeProp.enumValueIndex)
            {
                case 0:
                    DrawElementsContainingPath(pos, property, "Movement");
                    break;

                case 1:
                    DrawElementsContainingPath(pos, property, "Scale");
                    break;

                case 2:
                    DrawElementsContainingPath(pos, property, "Rotation");
                    break;
            }

            property.serializedObject.ApplyModifiedProperties();
        }

        private void DrawElementsContainingPath(Rect pos, SerializedProperty property, string value)
        {
            SerializedProperty iterator = property.serializedObject.GetIterator();
            iterator.Next(true);
            while (iterator.NextVisible(true))
            {
                if (iterator.propertyPath.Contains(property.propertyPath) && iterator.propertyPath.Contains(value) &&
                    DoesNotContainVector3Components(iterator.propertyPath))
                {
                    if (value.Equals("Rotation") && iterator.propertyType == SerializedPropertyType.Quaternion)
                    {
                        Vector3 eulerAngles = iterator.quaternionValue.eulerAngles;
                        Vector3 rot = EditorGUI.Vector3Field(pos, new GUIContent(iterator.displayName),
                            new Vector3(eulerAngles.x, eulerAngles.y,
                                eulerAngles.z));
                        iterator.quaternionValue = Quaternion.Euler(rot);
                    }
                    else
                        EditorGUI.PropertyField(pos, iterator, new GUIContent(iterator.displayName));

                    pos.y += pos.height;
                }
            }
        }

        private bool DoesNotContainVector3Components(string iteratorPropertyPath)
        {
            return !iteratorPropertyPath.Contains(".x") && !iteratorPropertyPath.Contains(".y") &&
                   !iteratorPropertyPath.Contains(".z") && !iteratorPropertyPath.Contains(".w");
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return (base.GetPropertyHeight(property, label) * 4f) + EditorGUIUtility.singleLineHeight;
        }
    }
}