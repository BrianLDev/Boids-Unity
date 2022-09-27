/*
ECX UTILITY SCRIPTS
UI Sfx Manager (Scriptable Object)
Last updated: Apr 16, 2022
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcxUtilities {

    /// <summary>
    /// This UI SfxManager is primarily used as a readily accessible UI SFX AudioClip storage.
    /// Other scripts can access audioclips as needed using UISfxManager.xxx
    /// </summary>
    [CreateAssetMenu(fileName = "UISfxManager", menuName = "ECX Utilities/UISfxManager", order = 1)] 
    public class UISfxManager : ScriptableObject {

        [Header("Standard UI SFX")]
        public AudioClip mouseOver;
        public AudioClip buttonClick;
        public AudioClip errorSound;

        // [Header("Game Specific UI SFX")] // UNCOMMENT THIS HEADER, RENAME IT, AND ADD ANY ADDITIONAL AUDIO CLIPS BELOW. THEN DRAG/DROP THEM IN THE UNITY EDITOR.


        // METHODS:
        // THE AUDIOMANAGER WILL HANDLE PLAYING MOST OF THE CLIPS, BUT IF YOU HAVE ANY CUSTOM SFX METHODS NEEDED, ADD THEM BELOW AS PUBLIC METHODS


    }
}