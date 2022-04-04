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
        private AutoProperty inputChCount;
        private AutoProperty outputChCount;

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

        
        /// <summary>
        /// getting asio driver parameters.
        /// InputChannel count
        /// outputChannel count
        /// Input Channel names
        /// </summary>
        /// <param name="name">selected ASIOdriver name</param>
        void OnSelectName(object name)
        {
            AsioManager asio = target as AsioManager;
            
            serializedObject.Update();
            
            driverName.Target.stringValue = (string) name;
           
            var driver = AsioDriver.GetAsioDriverByName(driverName.Target.stringValue);
            
            driver.GetChannels(out int inNum, out int outNum);
            inputChCount.Target.intValue = inNum;
            outputChCount.Target.intValue = outNum;
            driver.GetBufferSize(out _,out _,out int preferredSize, out _);
            bufferSizePerCh.Target.intValue = preferredSize;
            sampleRate.Target.intValue = (int)driver.GetSampleRate();
            serializedObject.ApplyModifiedProperties();
            
            var names = new string[inputChCount.Target.intValue];
            for (int i = 0; i < inputChCount.Target.intValue; i++)
            { 
                names[i] = driver.GetChannelInfo(i, false).name;
            }
            asio.nameOfChannels = names;
            
            driver.Stop();
            driver.DisposeBuffers();
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
            EditorGUILayout.PropertyField(bufferSizePerCh, new GUIContent("BufferSize/ch"));
            EditorGUILayout.PropertyField(sampleRate);
            serializedObject.ApplyModifiedProperties();
        }
    }
}