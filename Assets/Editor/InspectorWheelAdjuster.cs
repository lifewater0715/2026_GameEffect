using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[InitializeOnLoad]
public static class InspectorWheelAdjuster
{
    // =========================================================
    // Config
    // =========================================================

    [Serializable]
    private class WheelConfig
    {
        public bool enabled = true;

        // �Ϲ� ��
        public float defaultStep = 1.0f;

        // Ctrl + ��
        public float fineStep = 0.1f;

        // Shift + ��
        public float fastStep = 10.0f;
    }

    private const string ConfigPath =
        "ProjectSettings/InspectorWheelConfig.json";

    private static WheelConfig config;

    private static DateTime lastConfigWriteTime;

    // =========================================================
    // Inspector Hook
    // =========================================================

    private sealed class InspectorHook
    {
        public EditorWindow window;
        public VisualElement root;
    }

    private static readonly Dictionary<int, InspectorHook> hooks = new();

    private static double nextScanTime;

    // =========================================================
    // Initialize 
    // =========================================================

    static InspectorWheelAdjuster()
    {
        LoadConfig();

        EditorApplication.delayCall  += ScanInspectorWindows;
        EditorApplication.update += Update;

        AssemblyReloadEvents.beforeAssemblyReload += Shutdown;
    }

    // =========================================================
    // Update
    // =========================================================

    private static void Update()
    {
        if (EditorApplication.timeSinceStartup < nextScanTime)
            return;

        nextScanTime = EditorApplication.timeSinceStartup + 0.5d;

        ReloadConfigIfChanged();
        ScanInspectorWindows();
    }

    // =========================================================
    // Inspector �˻�
    // =========================================================

    private static void ScanInspectorWindows()
    {
        EditorWindow[] windows =
            Resources.FindObjectsOfTypeAll<EditorWindow>();

        foreach (EditorWindow window in windows)
        {
            if (window == null)
                continue;

            // InspectorWindow�� internal Ŭ������ �̸����� �˻�
            if (window.GetType().FullName !=
                "UnityEditor.InspectorWindow")
                continue;

            int id = window.GetInstanceID();

            VisualElement root = window.rootVisualElement;

            if (root == null)
                continue;

            // �̹� ��ϵǾ� �ִ� ���
            if (hooks.TryGetValue(id, out InspectorHook existingHook))
            {
                // Root�� �ٲ� ��� �ٽ� ����
                if (existingHook.root != root)
                {
                    if (existingHook.root != null)
                    {
                        existingHook.root.UnregisterCallback<WheelEvent>(
                            OnWheel,
                            TrickleDown.TrickleDown
                        );
                    }

                    RegisterWindow(window, root, id);
                }

                continue;
            }

            RegisterWindow(window, root, id);
        }

        CleanupDeadWindows();
    }

    private static void RegisterWindow(
        EditorWindow window,
        VisualElement root,
        int id)
    {
        root.RegisterCallback<WheelEvent>(
            OnWheel,
            TrickleDown.TrickleDown
        );

        hooks[id] = new InspectorHook
        {
            window = window,
            root = root
        };
    }

    // =========================================================
    // Wheel ó��
    // =========================================================

    private static void OnWheel(WheelEvent evt)
    {
        if (config == null)
            return;

        if (!config.enabled)
            return;

        if (Mathf.Approximately(evt.delta.y, 0f))
            return;

        VisualElement hoveredElement =
            evt.target as VisualElement;

        if (hoveredElement == null)
            return;

        // -----------------------------------------------------
        // ���� �ӵ� ����
        // -----------------------------------------------------

        double step;

        if (evt.ctrlKey)
        {
            // Ctrl = �̼� ����
            step = config.fineStep;
        }
        else if (evt.shiftKey)
        {
            // Shift = ���� ����
            step = config.fastStep;
        }
        else
        {
            // �Ϲ�
            step = config.defaultStep;
        }

        if (step <= 0)
            return;

        // Unity WheelEvent
        //
        // ���� ��ũ�� = delta.y < 0
        // �Ʒ��� ��ũ�� = delta.y > 0

        int direction = evt.delta.y < 0f ? 1 : -1;

        double amount = step * direction;

        // -----------------------------------------------------
        // ���� ���� Field ã��
        // -----------------------------------------------------

        if (!TryAdjustValue(
                hoveredElement,
                amount))
        {
            return;
        }

        // ���ڸ� ����������
        // Inspector ��ü ��ũ���� ���´�.
        evt.PreventDefault();
        evt.StopImmediatePropagation();
    }

    // =========================================================
    // Numeric Field ó��
    // =========================================================

    private static bool TryAdjustValue(
        VisualElement hoveredElement,
        double amount)
    {
        VisualElement current = hoveredElement;

        while (current != null)
        {
            if (current is FloatField floatField)
            {
                if (!CanEdit(
                        hoveredElement,
                        floatField,
                        floatField.labelElement))
                {
                    return false;
                }

                floatField.value += (float)amount;

                return true;
            }

            if (current is DoubleField doubleField)
            {
                if (!CanEdit(
                        hoveredElement,
                        doubleField,
                        doubleField.labelElement))
                {
                    return false;
                }

                doubleField.value += amount;

                return true;
            }

            if (current is IntegerField integerField)
            {
                if (!CanEdit(
                        hoveredElement,
                        integerField,
                        integerField.labelElement))
                {
                    return false;
                }

                int step = GetIntegerStep(amount);

                long nextValue =
                    (long)integerField.value + step;

                nextValue = Math.Max(
                    int.MinValue,
                    Math.Min(int.MaxValue, nextValue)
                );

                integerField.value = (int)nextValue;

                return true;
            }

            if (current is LongField longField)
            {
                if (!CanEdit(
                        hoveredElement,
                        longField,
                        longField.labelElement))
                {
                    return false;
                }

                long step = GetLongStep(amount);

                decimal nextValue =
                    (decimal)longField.value + step;

                if (nextValue > long.MaxValue)
                    nextValue = long.MaxValue;

                if (nextValue < long.MinValue)
                    nextValue = long.MinValue;

                longField.value = (long)nextValue;

                return true;
            }

            current = current.parent;
        }

        return false;
    }

    // =========================================================
    // Field �˻�
    // =========================================================

    private static bool CanEdit(
        VisualElement hovered,
        VisualElement field,
        VisualElement label)
    {
        if (!field.enabledInHierarchy)
            return false;

        // ���� �Է�â�� �ƴ϶�
        // ���� �̸� Label�� ���콺�� �ø� ���� ����
        VisualElement current = hovered;

        while (current != null &&
               current != field)
        {
            if (current == label)
                return false;

            current = current.parent;
        }

        return true;
    }

    // =========================================================
    // Integer ó��
    // =========================================================

    private static int GetIntegerStep(double amount)
    {
        int direction =
            amount >= 0 ? 1 : -1;

        double absolute =
            Math.Abs(amount);

        int step = (int)Math.Round(
            absolute,
            MidpointRounding.AwayFromZero
        );

        // int�� 0.1 �̵� ���� �� �Ұ����ϹǷ�
        // �ּ� �������� 1
        if (step < 1)
            step = 1;

        return step * direction;
    }

    private static long GetLongStep(double amount)
    {
        long direction =
            amount >= 0 ? 1 : -1;

        double absolute =
            Math.Abs(amount);

        long step = (long)Math.Round(
            absolute,
            MidpointRounding.AwayFromZero
        );

        if (step < 1)
            step = 1;

        return step * direction;
    }

    // =========================================================
    // Config
    // =========================================================

    private static void LoadConfig()
    {
        try
        {
            if (!File.Exists(ConfigPath))
            {
                config = new WheelConfig();

                SaveConfig();

                return;
            }

            string json =
                File.ReadAllText(ConfigPath);

            config =
                JsonUtility.FromJson<WheelConfig>(json);

            if (config == null)
                config = new WheelConfig();

            lastConfigWriteTime =
                File.GetLastWriteTimeUtc(ConfigPath);
        }
        catch (Exception exception)
        {
            Debug.LogError(
                "[Inspector Wheel] Config �ε� ����\n" +
                exception
            );

            config = new WheelConfig();
        }
    }

    private static void SaveConfig()
    {
        try
        {
            string json =
                JsonUtility.ToJson(
                    config,
                    true
                );

            File.WriteAllText(
                ConfigPath,
                json
            );

            lastConfigWriteTime =
                File.GetLastWriteTimeUtc(ConfigPath);
        }
        catch (Exception exception)
        {
            Debug.LogError(
                "[Inspector Wheel] Config ���� ����\n" +
                exception
            );
        }
    }

    private static void ReloadConfigIfChanged()
    {
        if (!File.Exists(ConfigPath))
        {
            config = new WheelConfig();

            SaveConfig();

            return;
        }

        DateTime writeTime =
            File.GetLastWriteTimeUtc(ConfigPath);

        if (writeTime ==
            lastConfigWriteTime)
        {
            return;
        }

        LoadConfig();
    }

    // =========================================================
    // Cleanup
    // =========================================================

    private static void CleanupDeadWindows()
    {
        List<int> removeList = new();

        foreach (KeyValuePair<int, InspectorHook> pair in hooks)
        {
            if (pair.Value.window != null)
                continue;

            if (pair.Value.root != null)
            {
                pair.Value.root.UnregisterCallback<WheelEvent>(
                    OnWheel,
                    TrickleDown.TrickleDown
                );
            }

            removeList.Add(pair.Key);
        }

        foreach (int id in removeList)
        {
            hooks.Remove(id);
        }
    }

    private static void Shutdown()
    {
        EditorApplication.update -= Update;

        foreach (InspectorHook hook in hooks.Values)
        {
            if (hook.root == null)
                continue;

            hook.root.UnregisterCallback<WheelEvent>(
                OnWheel,
                TrickleDown.TrickleDown
            );
        }

        hooks.Clear();
    }

    // =========================================================
    // Menu
    // =========================================================

    [MenuItem(
        "Tools/Inspector Wheel/Open Config"
    )]
    private static void OpenConfig()
    {
        if (!File.Exists(ConfigPath))
        {
            config ??= new WheelConfig();

            SaveConfig();
        }

        string fullPath =
            Path.GetFullPath(ConfigPath);

        EditorUtility.OpenWithDefaultApp(
            fullPath
        );
    }

    [MenuItem(
        "Tools/Inspector Wheel/Reload Config"
    )]
    private static void ReloadConfig()
    {
        LoadConfig();

        Debug.Log(
            "[Inspector Wheel] Config Reloaded"
        );
    }
}