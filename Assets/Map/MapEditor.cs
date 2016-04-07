using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class MapEditor : EditorWindow
{
    private List<MapInfo> windows = new List<MapInfo>();

    private Vector2 mousePos;
    private MapInfo selectedMap;
    private bool makeTransitionMode = false;

    [MenuItem("Windows/Map Editor")]
    static void ShowEditor()
    {
        MapEditor editor = EditorWindow.GetWindow<MapEditor>();
    }

    void OnGUI()
    {
        Event e = Event.current;
        mousePos = e.mousePosition;

        if (e.button == 1 && !makeTransitionMode)
        {
            if (e.type == EventType.MouseDown)
            {
                bool clickedOnWindow = false;
                int selectedIndex = 1;

                for (int i = 0; i < windows.Count; ++i)
                {
                    if (windows[i].windowRect.Contains(mousePos))
                    {
                        selectedIndex = i;
                        clickedOnWindow = true;
                        break;
                    }
                }

                if (!clickedOnWindow)
                {
                    GenericMenu menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Add Map"), false, ContextCallback, "addMap");
                    menu.ShowAsContext();
                    e.Use();
                }
                else
                {
                    GenericMenu menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Make Transition"), false, ContextCallback, "makeTransition");
                    menu.AddSeparator("");
                    menu.AddItem(new GUIContent("Delete Node"), false, ContextCallback, "deleteNode");

                    menu.ShowAsContext();
                    e.Use();
                }
            }
        }

        else if (e.button == 0 && e.type == EventType.MouseDown && makeTransitionMode)
        {
            bool clickedOnWindow = false;
            int selectedIndex = 1;

            for (int i = 0; i < windows.Count; ++i)
            {
                if (windows[i].windowRect.Contains(mousePos))
                {
                    selectedIndex = i;
                    clickedOnWindow = true;
                    break;
                }
            }

            if (clickedOnWindow && !windows[selectedIndex].Equals(selectedMap))
            {
                windows[selectedIndex].SetInput(selectedMap, mousePos);
                makeTransitionMode = false;
                selectedMap = null;
            }

            if (!clickedOnWindow)
            {
                makeTransitionMode = false;
                selectedMap = null;
            }

            e.Use();
        }
        else if (e.button == 0 && e.type == EventType.MouseDown && !makeTransitionMode)
        {
            bool clickedOnWindow = false;
            int selectedIndex = 1;

            for (int i = 0; i < windows.Count; ++i)
            {
                if (windows[i].windowRect.Contains(mousePos))
                {
                    selectedIndex = i;
                    clickedOnWindow = true;
                    break;
                }
            }

            if (clickedOnWindow)
            {
                MapInfo mapToChange = windows[selectedIndex].ClickedOnInput(mousePos);

                if (mapToChange != null)
                {
                    selectedMap = mapToChange;
                    makeTransitionMode = true;
                }
            }
        }

        if (makeTransitionMode && selectedMap != null)
        {
            Rect mouseRect = new Rect(e.mousePosition.x, e.mousePosition.y, 10, 10);

            DrawNodeCurve(selectedMap.windowRect, mouseRect);

            Repaint();
        }

        foreach (MapInfo n in windows)
        {
            n.DrawCurves();
        }

        BeginWindows();
        for (int i = 0; i < windows.Count; ++i)
        {
            windows[i].windowRect = GUI.Window(i, windows[i].windowRect, DrawNodeWindow, windows[i].windowTitle);
        }

        EndWindows();
    }

    private void DrawNodeWindow(int id)
    {
        windows[id].DrawWindow();
        GUI.DragWindow();
    }

    void ContextCallback(object obj)
    {
        string clb = obj.ToString();

        if (clb.Equals("addMap"))
        {
            MapInfo inputMap = ScriptableObject.CreateInstance<MapInfo>();
            inputMap.windowRect = new Rect(mousePos.x, mousePos.y, 200, 150);

            windows.Add(inputMap);
        }
        else if (clb.Equals("makeTransition"))
        {
            bool clickedOnWindow = false;
            int selectedIndex = 1;

            for (int i = 0; i < windows.Count; ++i)
            {
                if (windows[i].windowRect.Contains(mousePos))
                {
                    selectedIndex = i;
                    clickedOnWindow = true;
                    break;
                }
            }

            if (clickedOnWindow)
            {
                selectedMap = windows[selectedIndex];
                makeTransitionMode = true;
            }
        }
        else if (clb.Equals("deleteNode"))
        {
            bool clickedOnWindow = false;
            int selectedIndex = 1;

            for (int i = 0; i < windows.Count; ++i)
            {
                if (windows[i].windowRect.Contains(mousePos))
                {
                    selectedIndex = i;
                    clickedOnWindow = true;
                    break;
                }
            }

            if (clickedOnWindow)
            {
                MapInfo selNode = windows[selectedIndex];
                windows.RemoveAt(selectedIndex);
                foreach (MapInfo n in windows)
                {
                    n.MapDeleted(selNode);
                }
            }
        }
    }

    public static void DrawNodeCurve(Rect start, Rect end)
    {
        Vector3 startPos = new Vector3(start.x + start.width, start.y + start.height / 2, 0);
        Vector3 endPos = new Vector3(end.x, end.y + end.height / 2, 0);

        Vector3 startTan = startPos + Vector3.right * 50;
        Vector3 endTan = endPos + Vector3.left * 50;

        Color shadowCol = new Color(0, 0, 0, .06f);

        for (int i = 0; i < 3; ++i)
        {
            Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);
        }

        Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.red, null, 1);
    }
}
