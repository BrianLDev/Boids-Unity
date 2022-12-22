﻿/*
ECX UTILITY SCRIPTS
Game Manager (Singleton)
Last updated: September 24, 2020
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcxUtilities
{
  public class GameManager : Singleton<GameManager>
  {
    private void Update() {
      if (Input.GetKeyDown(KeyCode.Escape)) {
        UIManager.Instance.ToggleOptionsMenu();
      }
    }
  }
}
