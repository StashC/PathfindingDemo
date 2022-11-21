using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    private Button startButton;
    private Button resetButton;
    private Button walkableButton;
    private Button setStartButton;
    private Button setTargetButton;
    private Button clearButton;
    private Button genMazeButton;

    public SliderInt speedSlider;

    private Master _master;


    void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        _master = GameObject.FindGameObjectWithTag("Master").GetComponent<Master>();
        startButton = root.Q<Button>("start-button");
        resetButton     = root.Q<Button>("reset-button");
        walkableButton  = root.Q<Button>("walkable-button");
        setStartButton  = root.Q<Button>("set-start-button");
        setTargetButton = root.Q<Button>("set-target-button");
        clearButton     = root.Q<Button>("clear-button");
        genMazeButton = root.Q<Button>("generate-maze-button");

        startButton.clicked += StartButtonPressed;
        resetButton.clicked += ResetButtonPressed;
        walkableButton.clicked += WalkableButtonPressed;
        setStartButton.clicked += SetStartButtonPresed;
        setTargetButton.clicked += SetTargetButtonPressed;
        clearButton.clicked += ClearButtonPressed;
        genMazeButton.clicked += GenMazeButtonPressed;

        speedSlider = root.Q<SliderInt>("speed-slider");
        speedSlider.RegisterCallback<ChangeEvent<int>>(changeSpeed);
        _master.setRunSpeed(speedSlider.value);
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

    void GenMazeButtonPressed() {
        _master.genMaze();
    }


}
