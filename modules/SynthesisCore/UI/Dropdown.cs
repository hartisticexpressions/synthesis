﻿using System;
using System.Collections.Generic;
using SynthesisAPI.AssetManager;
using SynthesisAPI.EventBus;
using SynthesisAPI.UIManager.VisualElements;
using static SynthesisAPI.UIManager.VisualElements.ListView;

namespace SynthesisCore.UI
{
    public class Dropdown
    {
        #region UIElements

        private static VisualElementAsset _dropdownAsset;

        private VisualElement _visualElement;

        private Button _button;

        private VisualElement _buttonIcon;

        private ListView _listView;

        #endregion

        public static implicit operator VisualElement(Dropdown d) => d._visualElement;

        private List<string> _options;

        private bool _isListViewVisible = false;

        #region Properties

        public int Count { get => Selected == null ? _options.Count : _options.Count+1; }

        public string Selected { get; private set; }

        public IEnumerable<string> Options { get {
                if (Selected == null)
                    return _options;
                else
                {
                    List<string> lst = new List<string>(_options);
                    lst.Add(Selected);
                    return lst;
                } } }

        public int ItemHeight { get => _listView.ItemHeight; set { _listView.ItemHeight = value; RefreshListView(); } }

        public delegate void SubscribeEvent(string s);

        public event SubscribeEvent OnValueChanged;

        #endregion

        public Dropdown(string name)
        {
            Selected = null;
            _options = new List<string>();
            Init(name);
        }

        private void Init(string name)
        {
            if (_dropdownAsset == null)
                _dropdownAsset = AssetManager.GetAsset<VisualElementAsset>("/modules/synthesis_core/UI/uxml/Dropdown.uxml");
            _visualElement = _dropdownAsset.GetElement(name);
            CreateButton();
            CreateListView();
        }

        private void CreateButton()
        {
            //init visual elements
            _button = (Button)_visualElement.Get("selected-button");
            _buttonIcon = _button.Get("dropdown-icon");
            //default properties
            _button.Text = " "; //default start text
            _buttonIcon.SetStyleProperty("background-image", "/modules/synthesis_core/UI/images/toolbar-show-icon.png"); //add arrow icon
            _button.Subscribe(x =>
            {
                //toggle icon
                _buttonIcon.SetStyleProperty("background-image", _isListViewVisible ?
                    "/modules/synthesis_core/UI/images/toolbar-show-icon.png" :
                    "/modules/synthesis_core/UI/images/toolbar-hide-icon.png");
                //toggle list view
                if (_isListViewVisible)
                    _visualElement.Remove(_listView); //hides list view
                else
                    _visualElement.Add(_listView); //shows list view
                _isListViewVisible = !_isListViewVisible;
            });
        }

        private void CreateListView()
        {
            //init visual elements
            _listView = (ListView)_visualElement.Get("options-list");
            //hide list view on start
            _listView.RemoveFromHierarchy();
            //default height property
            _listView.ItemHeight = 30;
            //link list view population
            _listView.Populate(_options,
                                () => new Button(),
                                (element, index) =>
                                {
                                    var button = element as Button;
                                    button.Name = $"{_listView.Name}-{_options[index]}";
                                    button.Text = _options[index];
                                    button.Subscribe(x => OnOptionClick(button,index));
                                    button.SetStyleProperty("border-top-width", "0");
                                    button.SetStyleProperty("border-bottom-width", "0");
                                    button.SetStyleProperty("border-right-width", "0");
                                    button.SetStyleProperty("border-left-width", "0");
                                    button.SetStyleProperty("border-top-left-radius", "0");
                                    button.SetStyleProperty("border-top-right-radius", "0");
                                    button.SetStyleProperty("border-bottom-left-radius", "0");
                                    button.SetStyleProperty("border-bottom-right-radius", "0");
                                });
        }
        private void OnOptionClick(Button button,int index)
        {
            var _tmp = button.Text;
            if (Selected == null)
            {
                _options.RemoveAt(index);
                RefreshListView();
            } else
            {
                _options[index] = Selected;
                button.Text = Selected;
            }
            Selected = _tmp;
            RefreshButton();
            OnValueChanged?.Invoke(Selected);
        }

        public bool Add(string option)
        {
            if (_options.Contains(option) || Selected == option)
                return false;
            if (Selected == null)
            {
                Selected = option;
                RefreshButton();
            } else
            {
                _options.Add(option);
                RefreshListView();
            }
            return true;
        }
        public bool Remove(string option)
        {
            if (Selected == option)
            {
                Selected = null;
                RefreshButton();
                return true;
            } else if (_options.Remove(option))
            {
                RefreshListView();
                return true;
            }
            return false;
        }
        private void RefreshButton()
        {
            _button.Text = Selected == null ? " " : Selected;           
        }
        private void RefreshListView()
        {
            _listView.Refresh();
            UpdateContainerHeight();
        }
        private void RefreshAll()
        {
            RefreshButton();
            RefreshListView();
        }
        private void UpdateContainerHeight()
        {
            int listViewHeight = _options.Count *_listView.ItemHeight;
            _listView.SetStyleProperty("height", listViewHeight.ToString() + "px");
        }
    }
}
