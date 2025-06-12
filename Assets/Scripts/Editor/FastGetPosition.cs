using UnityEngine;
using UnityEditor;
using Doors;

[CustomEditor(typeof(PortalDoorSender))]
public class FastGetPosition : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Label("拖拽传送目标物体到下面区域", EditorStyles.boldLabel);
        Rect dropArea = GUILayoutUtility.GetRect(0, 50, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, "拖拽区域");

        Event evt = Event.current;

        if (dropArea.Contains(evt.mousePosition))
        {
            switch (evt.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:

                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                    if (evt.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();
                        // 遍历所有拖拽过来的对象
                        foreach (Object draggedObject in DragAndDrop.objectReferences)
                        {
                            // Debug.Log("拖拽进来的对象: " + draggedObject.name);
                            GameObject targetGameObjet = draggedObject as GameObject;
                            PortalDoorSender myself = (PortalDoorSender)target;
                            myself.TargetPosition = targetGameObjet.transform.position;
                        }
                    }
                    evt.Use();
                    break;
            }
        }
    }
}
