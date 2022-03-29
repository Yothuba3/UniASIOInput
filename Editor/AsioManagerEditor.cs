using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NAudio.Wave.Asio;
using UnityEditor;
using UnityEngine;
using Yothuba.Asio.Runtime;

namespace Yothuba.Asio.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(AsioManager))]
    public class AsioManagerEditor :UnityEditor.Editor
    {
        private AutoProperty driverName;
        private AutoProperty bufferSizePerCh;
        private AutoProperty sampleRate;
        private AutoProperty chOffset;
        private AutoProperty _inputChCount;
        private AutoProperty _outputChCount;

        void OnEnable() => AutoProperty.Scan(this);
        
        void ShowDriverNameDropdown(Rect rect)
        {
            var menu = new GenericMenu();
            var drivers = AsioDriver.GetAsioDriverNames();

            if (drivers.Any())
            {
                foreach (var name in drivers)
                {
                    menu.AddItem(new GUIContent(name), false, OnSelectName,name);
                }
            }
            else
            {
                menu.AddItem(new GUIContent("Cant Find ASIO Drivers"), false, null);
            }
            menu.DropDown(rect);
        }

        void OnSelectName(object name)
        {
            AsioManager asio = target as AsioManager;
            
            serializedObject.Update();
            
            driverName.Target.stringValue = (string) name;
           
            var driver = AsioDriver.GetAsioDriverByName((driverName.Target.stringValue));
            
            int inNum, outNum;
            driver.GetChannels(out inNum, out outNum);
            _inputChCount.Target.intValue = inNum;
            _outputChCount.Target.intValue = outNum;
            serializedObject.ApplyModifiedProperties();
            var names = new string[_inputChCount.Target.intValue];
            
            for (int i = 0; i < _inputChCount.Target.intValue; i++)
            { 
                names[i] = driver.GetChannelInfo(i, false).name;
            }
            asio.nameOfChannels = names;
            driver.ReleaseComAsioDriver();
        }
        

        public override void OnInspectorGUI()
        {
            AsioManager asioSample = target as AsioManager;
            serializedObject.Update();
            using(new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.DelayedTextField(driverName, new GUIContent("DriverName"));
                var rect = EditorGUILayout.GetControlRect(false, GUILayout.Width(60));
                if (EditorGUI.DropdownButton(rect, new GUIContent(), FocusType.Keyboard))
                {
                    if (!EditorApplication.isPlaying)
                    {
                        ShowDriverNameDropdown(rect);
                    }
                }
            }
            
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.HelpBox(new GUIContent("InChCount: " + asioSample.InputChCount));
                EditorGUILayout.HelpBox(new GUIContent("OutChCount: " + asioSample.OuputChCount));
            }
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(bufferSizePerCh);
            EditorGUILayout.PropertyField(chOffset);
            EditorGUILayout.PropertyField(sampleRate);
            serializedObject.ApplyModifiedProperties();
        }
    }
}