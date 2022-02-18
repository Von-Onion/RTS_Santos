using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitSelectionHandler : MonoBehaviour
{
    [SerializeField] private RectTransform unitSelectionArea = null;

    [SerializeField] private LayerMask layerMask = new LayerMask();

    private Vector2 startPosition;

    private RTSPlayer player;
    private Camera mainCamera;

    public List<Unit> SelectedUnits { get; } = new List<Unit>();

    private void Start() {
        mainCamera = Camera.main;
        player = NetworkClient.connection.identity.GetComponent<RTSPlayer>();
    }

    private void Update() {
        if(Mouse.current.leftButton.wasPressedThisFrame) {
            StartSelectionArea();
        }
        else if(Mouse.current.leftButton.wasReleasedThisFrame) {
            ClearSelectionArea();
        }
        else if(Mouse.current.leftButton.isPressed) {
            UpdateSelectionArea();
        }
    }

    private void StartSelectionArea() {
        foreach(Unit selectedUnit in SelectedUnits) {
        selectedUnit.Deselect();
        }

        SelectedUnits.Clear();

        unitSelectionArea.gameObject.SetActive(true);

        startPosition = Mouse.current.position.ReadValue();
    }

    private void UpdateSelectionArea() {

    }

    private void ClearSelectionArea()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if(!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask)) { return; }

        if(!hit.collider.TryGetComponent<Unit>(out Unit unit)) { return; }

        if(!unit.hasAuthority) { return; }

        SelectedUnits.Add(unit);

        foreach(Unit selectedUnit in SelectedUnits) {
            selectedUnit.Select();
        }
    }
}