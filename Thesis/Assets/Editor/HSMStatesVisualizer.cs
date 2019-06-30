using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class HSMStatesVisualizer : EditorWindow {

    int k = 0;
    List<SJHSMStateAsset> totalStates = new List<SJHSMStateAsset>();
    List<Rect> windows = new List<Rect>();
    List<int> windowsToAttach = new List<int>();
    List<int> attachedChildWindows = new List<int>();
    List<int> attachedParallelChildWindows = new List<int>();
    HSMStateNode[,] HSMStatesNodesGrid;

    int rowsOfHSMStatesNodesGrid = 100;
    int columnsOfHSMStatesNodesGrid = 100;

    int initialJumpInY = 1;
    float distanceBetweenStatesInY = 300;
    float distanceBetweenStatesInX = 200;

    float widthOfTheStateBox = 250;
    float heightOfTheStateBox = 150;

    float panX = 0;
    float panY = 0;

    [MenuItem("Window/HSM States Visualizer")]
    static void ShowEditor()
    {
        HSMStatesVisualizer editor = EditorWindow.GetWindow<HSMStatesVisualizer>();
    }

    public void Awake()
    {
        GenHSHMStatesNodesGrid();
    }

    void OnGUI()
    {
        GUI.BeginGroup(new Rect(panX, panY, 100000, 100000));

        /*if(windowsToAttach.Count == 2)
        {
            attachedWindows.Add(windowsToAttach[0]);
            attachedWindows.Add(windowsToAttach[1]);
            windowsToAttach.Clear();
        }*/

        //Debug.Log(windowsToAttach.Count);

        if(attachedChildWindows.Count >= 2)
        {
            for(int i = 0; i < attachedChildWindows.Count; i += 2)
            {
                DrawNodeCurveForChilds(windows[attachedChildWindows[i]], windows[attachedChildWindows[i + 1]]);
            }
        }

        if(attachedParallelChildWindows.Count >= 2)
        {
            for(int i = 0; i < attachedParallelChildWindows.Count; i += 2)
            {
                DrawNodeCurveForParallelChilds(windows[attachedParallelChildWindows[i]], windows[attachedParallelChildWindows[i + 1]]);
            }
        }

        BeginWindows();

        /*if(GUILayout.Button("Create Node"))
        {
            windows.Add(new Rect(10, 10, 100, 100));
        }*/

        for(int i = 0; i < windows.Count; i++)
        {
            windows[i] = GUI.Window(i, windows[i], DrawNodeWindow, totalStates[i].name);
        }

        EndWindows();

        GUI.EndGroup();

        ScrollingFunction();
    }

    void OnSelectionChange()
    {
        if(Selection.objects.Length == 1 && Selection.activeObject is SJHSMStateAsset asset)
        {
            //Debug.Log(HSMStatesNodesGrid.Length);
            windows.Clear();
            totalStates.Clear();
            attachedChildWindows.Clear();
            attachedParallelChildWindows.Clear();
            k = 0;
            ResetHSMStatesNodesValues();
            initialJumpInY = 0;
            totalStates.Add(asset);
            windows.Add(new Rect(SearchAvailableColumnNode(initialJumpInY), new Vector2(widthOfTheStateBox, heightOfTheStateBox)));
            StateWindowsGenerator(asset, initialJumpInY);
            //Debug.Log(k);
        }
    }

    void StateWindowsGenerator(SJHSMStateAsset SJHSMSasset, int jumpToDoInY)
    {
        if(totalStates.Contains(SJHSMSasset))
        {
            k++;

            int counterAux = k - 1;
            int jumpInYAux = jumpToDoInY;
            int timesThatEntersIntoTheContainsChildChecker = 0;
            int timesThatEntersIntoTheContainsParallelChildChecker = 0;

            if(SJHSMSasset.Childs.Length !=0)
            {
                for(int i = 0; i < SJHSMSasset.Childs.Length; i++)
                {
                    if(!totalStates.Contains(SJHSMSasset.Childs[i]))
                    {
                        if(timesThatEntersIntoTheContainsChildChecker == 0)
                        {
                            timesThatEntersIntoTheContainsChildChecker++;
                        }
                        if(timesThatEntersIntoTheContainsChildChecker == 1)
                        {
                            jumpInYAux = jumpToDoInY+ 1;
                        }
                        attachedChildWindows.Add(counterAux);
                        totalStates.Add(SJHSMSasset.Childs[i]);
                        windows.Add(new Rect(SearchAvailableColumnNode(jumpInYAux), new Vector2(widthOfTheStateBox, heightOfTheStateBox)));
                        attachedChildWindows.Add(totalStates.IndexOf(SJHSMSasset.Childs[i]));
                        StateWindowsGenerator(SJHSMSasset.Childs[i], jumpInYAux);
                    }
                    else
                    {
                        attachedChildWindows.Add(counterAux);
                        attachedChildWindows.Add(totalStates.IndexOf(SJHSMSasset.Childs[i]));
                    }
                }
            }

            if(SJHSMSasset.ParallelChilds.Length != 0)
            {
                for(int i = 0; i < SJHSMSasset.ParallelChilds.Length; i++)
                {
                    if(!totalStates.Contains(SJHSMSasset.ParallelChilds[i]))
                    {
                        if(timesThatEntersIntoTheContainsParallelChildChecker == 0)
                        {
                            timesThatEntersIntoTheContainsParallelChildChecker++;
                        }
                        if(timesThatEntersIntoTheContainsParallelChildChecker == 1)
                        {
                            jumpInYAux = jumpToDoInY + 1;
                        }
                        attachedParallelChildWindows.Add(counterAux);
                        totalStates.Add(SJHSMSasset.ParallelChilds[i]);
                        windows.Add(new Rect(SearchAvailableColumnNode(jumpInYAux), new Vector2(widthOfTheStateBox, heightOfTheStateBox)));
                        attachedParallelChildWindows.Add(totalStates.IndexOf(SJHSMSasset.ParallelChilds[i]));
                        StateWindowsGenerator(SJHSMSasset.ParallelChilds[i], jumpInYAux);
                    }
                    else
                    {
                        attachedParallelChildWindows.Add(counterAux);
                        attachedParallelChildWindows.Add(totalStates.IndexOf(SJHSMSasset.ParallelChilds[i]));
                    }
                }
            }
        }
    }

    void DrawNodeWindow(int id)
    {
        if(totalStates[id].Childs.Length != 0)
        {
            for(int i = 0; i < totalStates[id].Childs.Length; i++)
            {
                GUILayout.Label("Child " + i + ": " + totalStates[id].Childs[i].name);
            }
        }

        if(totalStates[id].ParallelChilds.Length != 0)
        {
            for(int i = 0; i < totalStates[id].ParallelChilds.Length; i++)
            {
                GUILayout.Label("Parallel Child " + i + ": " + totalStates[id].ParallelChilds[i].name);
            }
        }

        GUI.DragWindow();
    }

    void GenHSHMStatesNodesGrid() {
        HSMStatesNodesGrid = new HSMStateNode[rowsOfHSMStatesNodesGrid, columnsOfHSMStatesNodesGrid];

        for(int i = 0; i < rowsOfHSMStatesNodesGrid; i++)
        {
            for(int j = 0; j < columnsOfHSMStatesNodesGrid; j++)
            {
                HSMStatesNodesGrid[i, j] = new HSMStateNode();

                HSMStatesNodesGrid[i, j].position = new Vector2(distanceBetweenStatesInY * j, distanceBetweenStatesInX * i);
            }
        }
    }

    Vector2 SearchAvailableColumnNode(int row)
    {
        Vector2 positionOfWindow = new Vector2();

        for(int i = 0; i < columnsOfHSMStatesNodesGrid; i++)
        {
            if(HSMStatesNodesGrid[row, i].available)
            {
                HSMStatesNodesGrid[row, i].available = false;
                positionOfWindow = HSMStatesNodesGrid[row, i].position;
                break;
            }
        }

        return positionOfWindow;
    }

    void ResetHSMStatesNodesValues() {
        for(int i = 0; i < rowsOfHSMStatesNodesGrid; i++)
        {
            for(int j = 0; j < columnsOfHSMStatesNodesGrid; j++)
            {
                HSMStatesNodesGrid[i, j].resetValues();
            }
        }
    }

    void DrawNodeCurveForChilds(Rect start, Rect end)
    {
        Vector3 startPos = new Vector3(start.x + start.width / 2, start.y + start.height, 0);
        Vector3 endPos = new Vector3(end.x + end.width / 2, end.y, 0);
        Vector3 startTan = startPos + Vector3.down * 50;
        Vector3 endTan = endPos + Vector3.up * 50;
        Color shadowCol = new Color(0, 0, 0, 0.06f);
        for(int i = 0; i < 3; i++)
        {
            Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);
        }
        Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.green, null, 2.5f);
    }

    void DrawNodeCurveForParallelChilds(Rect start, Rect end)
    {
        Vector3 startPos = new Vector3(start.x + start.width / 2, start.y + start.height, 0);
        Vector3 endPos = new Vector3(end.x + end.width / 2, end.y, 0);
        Vector3 startTan = startPos + Vector3.down * 50;
        Vector3 endTan = endPos + Vector3.up * 50;
        Color shadowCol = new Color(0, 0, 0, 0.06f);
        for(int i = 0; i < 3; i++)
        {
            Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);
        }
        Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.red, null, 2.5f);
    }

    void ScrollingFunction()
    {
        if(GUI.RepeatButton(new Rect(15, 5, 20, 20), "^"))
        {
            panY += 1;
            Repaint();
        }

        if(GUI.RepeatButton(new Rect(5, 25, 20, 20), "<"))
        {
            panX += 1;
            Repaint();
        }

        if(GUI.RepeatButton(new Rect(25, 25, 20, 20), ">"))
        {
            panX -= 1;
            Repaint();
        }

        if(GUI.RepeatButton(new Rect(15, 45, 20, 20), "v"))
        {
            panY -= 1;
            Repaint();
        }
    }
}
