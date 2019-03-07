﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class HSMStatesVisualizer : EditorWindow {

    int j = 0;
    List<SJHSMStateAsset> totalStates = new List<SJHSMStateAsset>();
    List<Rect> windows = new List<Rect>();
    List<int> windowsToAttach = new List<int>();
    List<int> attacheChildWindows = new List<int>();
    List<int> attachedParallelChildWindows = new List<int>();
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

        if(attacheChildWindows.Count >= 2)
        {
            for(int i = 0; i < attacheChildWindows.Count; i += 2)
            {
                DrawNodeCurveForChilds(windows[attacheChildWindows[i]], windows[attacheChildWindows[i + 1]]);
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

    void OnSelectionChange()
    {
        if(Selection.objects.Length == 1 && Selection.activeObject is SJHSMStateAsset asset)
        {
            windows.Clear();
            totalStates.Clear();
            attacheChildWindows.Clear();
            attachedParallelChildWindows.Clear();
            j = 0;
            totalStates.Add(asset);
            StateWindowsGenerator(asset);
            Debug.Log(j);
        }
    }

    void StateWindowsGenerator(SJHSMStateAsset SJHSMSasset)
    {
        if(totalStates.Contains(SJHSMSasset))
        {
            j++;

            int aux = j - 1;

            windows.Add(new Rect(10, 10, 100, 100));

            for(int i = 0; i < SJHSMSasset.childs.Length; i++)
            {
                if(!totalStates.Contains(SJHSMSasset.childs[i]))
                {
                    attacheChildWindows.Add(aux);
                    totalStates.Add(SJHSMSasset.childs[i]);
                    attacheChildWindows.Add(totalStates.IndexOf(SJHSMSasset.childs[i]));
                    StateWindowsGenerator(SJHSMSasset.childs[i]);
                }
                else
                {
                    attacheChildWindows.Add(aux);
                    attacheChildWindows.Add(totalStates.IndexOf(SJHSMSasset.childs[i]));
                }
            }

            for(int i = 0; i < SJHSMSasset.parallelChilds.Length; i++)
            {
                if(!totalStates.Contains(SJHSMSasset.parallelChilds[i]))
                {
                    attachedParallelChildWindows.Add(aux);
                    totalStates.Add(SJHSMSasset.parallelChilds[i]);
                    attachedParallelChildWindows.Add(totalStates.IndexOf(SJHSMSasset.parallelChilds[i]));
                    StateWindowsGenerator(SJHSMSasset.parallelChilds[i]);
                }
                else
                {
                    attachedParallelChildWindows.Add(aux);
                    attachedParallelChildWindows.Add(totalStates.IndexOf(SJHSMSasset.parallelChilds[i]));
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
}
