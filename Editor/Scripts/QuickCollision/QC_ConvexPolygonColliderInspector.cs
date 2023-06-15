using GoodHub.Core.Runtime.QuickCollision;
using UnityEditor;
using UnityEngine;

namespace GoodHub.Core.Editor.QuickCollision
{
    [CustomEditor(typeof(QC_ConvexPolygonCollider))]
    public class QC_ConvexPolygonColliderInspector : UnityEditor.Editor
    {
        private SerializedProperty pointsProperty;
        private QC_ConvexPolygonCollider polygonCollider;

        private const float handleSize = 0.1f;
        private const float selectionRadius = 100f;
        private int hoveredIndex = -1;
        private int selectedIndex = -1;

        private void OnEnable()
        {
            pointsProperty = serializedObject.FindProperty("_points");
            polygonCollider = (QC_ConvexPolygonCollider) target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(pointsProperty, true);

            serializedObject.ApplyModifiedProperties();
        }

        public void OnSceneGUI()
        {
            Event currentEvent = Event.current;

            DrawHandles(currentEvent);
            HandleInput(currentEvent);

            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        }

        private void DrawHandles(Event currentEvent)
        {
            if (currentEvent.type != EventType.Repaint)
                return;

            for (int i = 0; i < pointsProperty.arraySize; i++)
            {
                if (selectedIndex == -1)
                    Handles.color = (i == hoveredIndex) ? Color.cyan : Color.green;
                else
                    Handles.color = (i == selectedIndex) ? Color.red : Color.green;

                Vector2 point = pointsProperty.GetArrayElementAtIndex(i).vector2Value;
                Vector3 transformedPoint = polygonCollider.transform.TransformPoint(point);

                Handles.DrawSolidDisc(transformedPoint, Vector3.back, handleSize);
            }
        }

        private void HandleInput(Event currentEvent)
        {
            Vector2 mousePos = currentEvent.mousePosition;
            Vector2 mouseWorldPos = HandleUtility.GUIPointToWorldRay(mousePos).origin;

            if (currentEvent.button != 0 || currentEvent.modifiers != EventModifiers.None)
                return;

            switch (currentEvent.type)
            {
                case EventType.MouseMove:
                {
                    if (selectedIndex != -1)
                        break;

                    hoveredIndex = -1;
                    float closestDistance = Mathf.Infinity;

                    for (int i = 0; i < pointsProperty.arraySize; i++)
                    {
                        Vector2 point = pointsProperty.GetArrayElementAtIndex(i).vector2Value;
                        Vector3 transformedPoint = polygonCollider.transform.TransformPoint(point);
                        float distance = Vector2.Distance(HandleUtility.WorldToGUIPoint(transformedPoint), mousePos);

                        if (distance >= closestDistance || distance >= selectionRadius)
                            continue;

                        closestDistance = distance;
                        hoveredIndex = i;
                    }

                    currentEvent.Use();

                    break;
                }
                case EventType.MouseDown when currentEvent.button == 0:
                {
                    selectedIndex = -1;
                    float closestDistance = Mathf.Infinity;

                    for (int i = 0; i < pointsProperty.arraySize; i++)
                    {
                        Vector2 point = pointsProperty.GetArrayElementAtIndex(i).vector2Value;
                        Vector3 transformedPoint = polygonCollider.transform.TransformPoint(point);
                        float distance = Vector2.Distance(HandleUtility.WorldToGUIPoint(transformedPoint), mousePos);

                        if (distance >= closestDistance || distance >= selectionRadius)
                            continue;

                        closestDistance = distance;
                        selectedIndex = i;
                    }

                    if (selectedIndex == -1)
                    {
                        Undo.RecordObject(polygonCollider, "Add Position");
                        Vector2 localPos = polygonCollider.transform.InverseTransformPoint(mouseWorldPos);

                        pointsProperty.InsertArrayElementAtIndex(pointsProperty.arraySize);
                        pointsProperty.GetArrayElementAtIndex(pointsProperty.arraySize - 1).vector2Value = localPos;
                        selectedIndex = pointsProperty.arraySize - 1;
                        serializedObject.ApplyModifiedProperties();
                    }

                    currentEvent.Use();
                    break;
                }
                case EventType.MouseDown when currentEvent.button == 1:
                {
                    break;
                }
                case EventType.MouseDrag:
                {
                    if (selectedIndex == -1)
                        break;

                    Vector2 localPos = polygonCollider.transform.InverseTransformPoint(mouseWorldPos);

                    pointsProperty.GetArrayElementAtIndex(selectedIndex).vector2Value = localPos;
                    serializedObject.ApplyModifiedProperties();

                    currentEvent.Use();
                    break;
                }
                case EventType.MouseUp:
                {
                    if (selectedIndex != -1)
                        currentEvent.Use();

                    break;
                }
            }
        }
    }
}