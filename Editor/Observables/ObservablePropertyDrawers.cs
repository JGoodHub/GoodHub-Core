using System;
using System.Globalization;
using GoodHub.Core.Runtime.Observables;
using UnityEditor;
using UnityEngine;

namespace GoodHub.Core.Editor.Observables
{
    [CustomPropertyDrawer(typeof(ObservableBool))]
    public class ObservableBoolPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            ObservableBool observable = (ObservableBool)fieldInfo.GetValue(property.serializedObject.targetObject);

            position = EditorGUI.PrefixLabel(position, label);

            EditorGUI.LabelField(position, Application.isPlaying ? observable.Value.ToString() : string.Empty);

            EditorGUI.EndProperty();
        }
    }

    [CustomPropertyDrawer(typeof(ObservableInt))]
    public class ObservableIntPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            ObservableInt observable = (ObservableInt)fieldInfo.GetValue(property.serializedObject.targetObject);

            position = EditorGUI.PrefixLabel(position, label);

            EditorGUI.LabelField(position, Application.isPlaying ? observable.Value.ToString() : string.Empty);

            EditorGUI.EndProperty();
        }
    }

    [CustomPropertyDrawer(typeof(ObservableFloat))]
    public class ObservableFloatPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            ObservableFloat observable = (ObservableFloat)fieldInfo.GetValue(property.serializedObject.targetObject);

            position = EditorGUI.PrefixLabel(position, label);

            EditorGUI.LabelField(position,
                Application.isPlaying ? observable.Value.ToString(CultureInfo.InvariantCulture) : string.Empty);

            EditorGUI.EndProperty();
        }
    }

    [CustomPropertyDrawer(typeof(ObservableString))]
    public class ObservableStringPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            ObservableString observable = (ObservableString)fieldInfo.GetValue(property.serializedObject.targetObject);

            position = EditorGUI.PrefixLabel(position, label);

            EditorGUI.LabelField(position, Application.isPlaying ? observable.Value : string.Empty);

            EditorGUI.EndProperty();
        }
    }

    [CustomPropertyDrawer(typeof(ObservableEnum<>))]
    public class ObservableEnumDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            IObservableEnum observable = (IObservableEnum)fieldInfo.GetValue(property.serializedObject.targetObject);

            Type fieldType = fieldInfo.FieldType;
            Type[] typeArgs = fieldType.GetGenericArguments();
            Type enumType = typeArgs[0];
            string enumValueName = Enum.ToObject(enumType, observable.IntValue).ToString();

            position = EditorGUI.PrefixLabel(position, label);

            EditorGUI.LabelField(position, Application.isPlaying ? enumValueName : string.Empty);

            EditorGUI.EndProperty();
        }
    }
}