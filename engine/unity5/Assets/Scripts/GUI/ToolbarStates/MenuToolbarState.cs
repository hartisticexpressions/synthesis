﻿using Synthesis.FSM;
using Synthesis.States;
using Synthesis.Utils;
using UnityEngine;
using UnityEngine.UI;
using Synthesis.Sensors;
using Synthesis.Camera;
using Synthesis.GUI;

namespace Assets.Scripts.GUI
{
    /// <summary>
    /// Menu toolbar
    /// </summary>
    public class MenuToolbarState : State
    {
        GameObject canvas;

        GameObject menuPanel;

        private MenuUI menuUI;

        public override void Start()
        {
            canvas = GameObject.Find("Canvas");

            menuPanel = Auxiliary.FindObject(canvas, "MenuPanel");

            menuUI = StateMachine.SceneGlobal.GetComponent<MenuUI>();

            OnRobotControlsButtonClicked();
        }

        /// <summary>
        /// Opens the robot controls panel
        /// </summary>
        public void OnRobotControlsButtonClicked()
        {
            MenuUI.instance.CheckUnsavedControls(() =>
            {
                menuUI.SwitchRobotControls();
            });
        }

        /// <summary>
        /// Opens the global controls panel
        /// </summary>
        public void OnGlobalControlsButtonClicked()
        {
            MenuUI.instance.CheckUnsavedControls(() =>
            {
                menuUI.SwitchGlobalControls();
            });
        }

        /// <summary>
        /// Opens the settings panel
        /// </summary>
        public void OnSettingsButtonClicked()
        {
            MenuUI.instance.CheckUnsavedControls(() =>
            {
                menuUI.SwitchSettings();
            });
        }

        /// <summary>
        /// Opens the view replays panel
        /// </summary>
        public void OnViewReplaysButtonClicked()
        {
            MenuUI.instance.CheckUnsavedControls(() =>
            {
                menuUI.SwitchViewReplays();
            });
        }

        /// <summary>
        /// Opens the help panel
        /// </summary>
        public void OnHelpButtonClicked()
        {
            MenuUI.instance.CheckUnsavedControls(() =>
            {
                menuUI.SwitchHelp();
            });
        }

        public override void End()
        {
            menuUI.EndOtherProcesses();
        }
    }
}