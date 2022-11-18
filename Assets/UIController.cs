using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    public Button startButton;
    public Button resetButton;
    public Button walkableButton;
    public Button setStartButton;
    public Button setTargetButton;
    public Button clearButton;

    public SliderInt speedSlider;

    private Master _master;


    void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        startButton     = root.Q<Button>("start-button");
        resetButton     = root.Q<Button>("reset-button");
        walkableButton  = root.Q<Button>("walkable-button");
        setStartButton  = root.Q<Button>("set-start-button");
        setTargetButton = root.Q<Button>("set-target-button");
        clearButton     = root.Q<Button>("clear-button");

        startButton.clicked += StartButtonPressed;
        resetButton.clicked += ResetButtonPressed;
        walkableButton.clicked += WalkableButtonPressed;
        setStartButton.clicked += SetStartButtonPresed;
        setTargetButton.clicked += SetTargetButtonPressed;
        clearButton.clicked += ClearButtonPressed;

        speedSlider = root.Q<SliderInt>("speed-slider");
        speedSlider.RegisterCallback<ChangeEvent<int>>(changeSpeed);

        _master = GameObject.FindGameObjectWithTag("Master").GetComponent<Master>();        
    }

    private void changeSpeed(ChangeEvent<int> evt) {
        Debug.Log(evt.newValue);
        _master.setRunSpeed(evt.newValue);
    }

 

    void StartButtonPressed() {
        _master.startSearch();
    }
    
    void ResetButtonPressed() {
        _master.resetSearch();
    }

    void WalkableButtonPressed() {
        _master.currButton = "ToggleWalkable";
    }

    void SetStartButtonPresed() {
        _master.currButton = "SetStart";
    }

    void SetTargetButtonPressed() {
        _master.currButton = "SetTarget";
    }

    void ClearButtonPressed() {
        _master.clearGrid();
    }


}
