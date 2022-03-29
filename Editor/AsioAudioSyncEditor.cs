using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NAudio.Wave;
using NAudio.Wave.Asio;
using UnityEditor;
using UnityEngine;
using Yothuba.Asio.Runtime;
namespace Yothuba.Asio.Editor
{

    
    [CustomEditor(typeof(AsioAudioSync))]
    public class AsioAudioSyncEditor : UnityEditor.Editor
    {
        AutoProperty _asioDriver;
        AutoProperty channelIndex;
        AutoProperty channelName;
        AutoProperty cube;
        private string[] names;
        void ShowChannelNameDropdown(Rect rect)
        {
            var audioSync = target as AsioAudioSync;
            var menu = new GenericMenu();

            names = audioSync._asioDriver.GetInputChannelsName();
            if (names.Any())
            {
                for (int i = 0; i < names.Length; i++)
                {
                    menu.AddItem(new GUIContent(names[i]), false, OnSelectName,i);
                }
            }
            else
            {
                menu.AddItem(new GUIContent("Cant Find Channels"), false, null);
            }
            menu.DropDown(rect);
        }
        
        void OnSelectName(object index)
        {
            serializedObject.Update();
            
            channelIndex.Target.intValue = (int)index;
            channelName.Target.stringValue = names[channelIndex.Target.intValue];
            serializedObject.ApplyModifiedProperties();
        }
        void OnEnable() => AutoProperty.Scan(this);

        
        public override void OnInspectorGUI()
        {
            AsioAudioSync audioSync = target as AsioAudioSync;
            serializedObject.Update();
            EditorGUILayout.PropertyField(_asioDriver);
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
            if(audioSync._asioDriver is null) EditorGUILayout.HelpBox("AsioMangerを指定してください", MessageType.Error);
            using(new EditorGUILayout.HorizontalScope())
            {
                
                EditorGUILayout.DelayedTextField(channelName, new GUIContent("ChannelName"));
                var rect = EditorGUILayout.GetControlRect(false, GUILayout.Width(60));
                if (EditorGUI.DropdownButton(rect, new GUIContent(), FocusType.Keyboard))
                {
                    if (!(audioSync is null))
                    {
                        ShowChannelNameDropdown(rect);
                    }
                }
            }
           EditorGUILayout.PropertyField(cube);
            serializedObject.ApplyModifiedProperties();
        }
    }
}