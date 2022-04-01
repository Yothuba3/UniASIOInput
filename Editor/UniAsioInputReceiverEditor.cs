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

    
    [CustomEditor(typeof(UniAsioInputReceiver))]
    public class UniAsioInputReceiverEditor : UnityEditor.Editor
    {
        AutoProperty asioManager;
        AutoProperty channelIndex; 
        AutoProperty cube;
        AutoProperty OnUniAsioInputEvent;
        private string[] names;
        private string channelName;
        void ShowChannelNameDropdown(Rect rect)
        {
            var audioSync = target as UniAsioInputReceiver;
            var menu = new GenericMenu();

            names = audioSync.asioManager.nameOfChannels;
            if (names.Any())
            {
                for (int i = 0; i < names.Length; i++)
                {
                    var name = names[i].Replace("/","\u2215");
                    menu.AddItem(new GUIContent(name), false, OnSelectName,i);
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
            channelName = names[channelIndex.Target.intValue];
            serializedObject.ApplyModifiedProperties();
        }
        void OnEnable() => AutoProperty.Scan(this);

        
        public override void OnInspectorGUI()
        {
            UniAsioInputReceiver receiver = target as UniAsioInputReceiver;
            serializedObject.Update();
            EditorGUILayout.PropertyField(asioManager);
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
            if(receiver.asioManager is null) EditorGUILayout.HelpBox("AsioMangerを指定してください", MessageType.Error);
            using(new EditorGUILayout.HorizontalScope())
            {
                
                EditorGUILayout.DelayedTextField("channelName", channelName);
                var rect = EditorGUILayout.GetControlRect( GUILayout.Width(60));
                if (EditorGUI.DropdownButton(rect, new GUIContent(), FocusType.Keyboard))
                {
                    ShowChannelNameDropdown(rect);
                }
            }
           EditorGUILayout.PropertyField(cube);
           EditorGUILayout.PropertyField(OnUniAsioInputEvent);
            serializedObject.ApplyModifiedProperties();
        }
    }
}