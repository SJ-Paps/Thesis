using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class HSMStatesVisualizer : EditorWindow {

    int j = 0;
    List<SJHSMStateAsset> totalStates = new List<SJHSMStateAsset>();
    List<Rect> windows = new List<Rect>();
    List<int> windowsToAttach = new List<int>();
    List<int> attachedChildWindows = new List<int>();
    List<int> attachedParallelChildWindows = new List<int>();
    float jumpInY = 1;
    float distanceBetweenStatesInY = 200;
    float panX = 0;
    float panY = 0;

    [MenuItem("Window/HSM States Visualizer")]
    static void ShowEditor()
    {
        HSMStatesVisualizer editor = EditorWindow.GetWindow<HSMStatesVisualizer>();
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
            windows.Clear();
            totalStates.Clear();
            attachedChildWindows.Clear();
            attachedParallelChildWindows.Clear();
            j = 0;
            jumpInY = 1;
            totalStates.Add(asset);
            windows.Add(new Rect((this.position.width / 2), distanceBetweenStatesInY * jumpInY, 200, 100));
            StateWindowsGenerator(asset);
            Debug.Log(j);
        }
    }

    void StateWindowsGenerator(SJHSMStateAsset SJHSMSasset)
    {
        if(totalStates.Contains(SJHSMSasset))
        {
            j++;

            int counterAux = j - 1;

            if(SJHSMSasset.childs.Length != 0)
            {
                jumpInY++;
                float jumpInYAux = jumpInY - 1;

                for(int i = 0; i < SJHSMSasset.childs.Length; i++)
                {
                    if(!totalStates.Contains(SJHSMSasset.childs[i]))
                    {
                        attachedChildWindows.Add(counterAux);
                        totalStates.Add(SJHSMSasset.childs[i]);
                        windows.Add(new Rect(((this.position.width / (SJHSMSasset.childs.Length + 1)) * i) + 200, distanceBetweenStatesInY * jumpInYAux, 200, 100));
                        attachedChildWindows.Add(totalStates.IndexOf(SJHSMSasset.childs[i]));
                        StateWindowsGenerator(SJHSMSasset.childs[i]);
                    }
                    else
                    {
                        attachedChildWindows.Add(counterAux);
                        attachedChildWindows.Add(totalStates.IndexOf(SJHSMSasset.childs[i]));
                    }
                }
            }

            if(SJHSMSasset.parallelChilds.Length != 0)
            {
                for(int i = 0; i < SJHSMSasset.parallelChilds.Length; i++)
                {
                    if(!totalStates.Contains(SJHSMSasset.parallelChilds[i]))
                    {
                        attachedParallelChildWindows.Add(counterAux);
                        totalStates.Add(SJHSMSasset.parallelChilds[i]);
                        windows.Add(new Rect(((this.position.width / (i + 2))), distanceBetweenStatesInY * jumpInY, 200, 100));
                        attachedParallelChildWindows.Add(totalStates.IndexOf(SJHSMSasset.parallelChilds[i]));
                        StateWindowsGenerator(SJHSMSasset.parallelChilds[i]);
                    }
                    else
                    {
                        attachedParallelChildWindows.Add(counterAux);
                        attachedParallelChildWindows.Add(totalStates.IndexOf(SJHSMSasset.parallelChilds[i]));
                    }
                }
            }
        }
    }

    void DrawNodeWindow(int id)
    {
        if(GUILayout.Button("Attach"))
        {
            Debug.Log(id);
            windowsToAttach.Add(id);
        }

        GUI.DragWindow();
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
        Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.green, null, 1);
    }

    void DrawNodeCurveForParallelChilds(Rect start, Rect end)
    {
        Vector3 startPos = new Vector3(start.x + start.width, start.y + start.height / 2, 0);
        Vector3 endPos = new Vector3(end.x, end.y + end.height / 2, 0);
        Vector3 startTan = startPos + Vector3.right * 50;
        Vector3 endTan = endPos + Vector3.left * 50;
        Color shadowCol = new Color(0, 0, 0, 0.06f);
        for(int i = 0; i < 3; i++)
        {
            Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);
        }
        Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.red, null, 1);
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
