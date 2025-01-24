using GoodHub.Core.Runtime.Observables;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GoodHub.Core.Editor
{
    [CustomPropertyDrawer(typeof(ObservableBool))]
    public class ObservableBoolUIToolkitDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // Create a container for the property (VisualElement)
            VisualElement container = new VisualElement();

            // Get the value property
            SerializedProperty valueProperty = property.FindPropertyRelative("_value");

            // Create a Toggle that includes the label in its text
            Toggle toggle = new Toggle
            {
                value = valueProperty.boolValue,
                label = property.displayName
            };

            toggle.AddToClassList("unity-base-field__aligned");
            toggle.labelElement.AddToClassList("unity-property-field__label");

            // Add the toggle (with label) to the container
            container.Add(toggle);

            // Register a callback when the toggle value changes
            toggle.RegisterValueChangedCallback(evt =>
            {
                if (fieldInfo.GetValue(property.serializedObject.targetObject) is ObservableBool observable)
                    observable.Value = evt.newValue;

                valueProperty.boolValue = evt.newValue;
                property.serializedObject.ApplyModifiedProperties();
            });

            return container;
        }
    }
}