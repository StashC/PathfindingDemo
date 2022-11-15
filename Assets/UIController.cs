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
        walkableButton.clicked += WalkabeButtonPressed;
        setStartButton.clicked += SetStartButtonPresed;
        setTargetButton.clicked += SetTargetButtonPressed;
        clearButton.clicked += ClearButtonPressed;

        speedSlider = root.Q<SliderInt>("speed-slider");
        
    }

    void StartButtonPressed() {

    }
    
    void ResetButtonPressed() {

    }

    void WalkabeButtonPressed() {

    }

    void SetStartButtonPresed() {

    }

    void SetTargetButtonPressed() {

    }

    void ClearButtonPressed() {

    }


}
