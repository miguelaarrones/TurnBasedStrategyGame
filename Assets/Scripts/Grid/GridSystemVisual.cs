using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using static GridSystemVisual;

public class GridSystemVisual : MonoBehaviour
{
    public static GridSystemVisual Instance {  get; private set; }

    [Serializable]
    public struct GridVisualTypeMaterial
    {
        public GridVisualType gridVisualType;
        public Material material;   
    }

    public enum GridVisualType
    {
        White,
        Blue,
        Red,
        RedSoft,
        Yellow
    }

    [SerializeField] private Transform gridSystemVisualSinglePrefab;
    [SerializeField] private List<GridVisualTypeMaterial> gridVisualTypeMaterialList;

    private GridSystemVisualSingle[,] gridSystemVisualSingleArray;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log($"There's more than one GridSystemVisual! {transform} - {Instance}");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        gridSystemVisualSingleArray = new GridSystemVisualSingle[LevelGrid.Instance.GetWidth(), LevelGrid.Instance.GetHeight()];

        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Transform gridSystemVisualSingleTransform = Instantiate(gridSystemVisualSinglePrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);

                gridSystemVisualSingleArray[x, z] = gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();
            }
        }

        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        LevelGrid.Instance.OnAnyUnitMoveGridPosition += LevelGrid_OnAnyUnitMoveGridPosition;

        UpdateGridVisual();
    }

    public void HideAllGridPosition()
    {
        foreach(GridSystemVisualSingle gridSystemVisualSingle in gridSystemVisualSingleArray)
        {
            gridSystemVisualSingle.Hide();
        }
    }

    private void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {
        List<GridPosition> gridPositionList = new List<GridPosition>();

        for (int x = -range; x <= range; x++)
        {
            for (int z  = -range; z <= range; z++)
            {
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > range)
                {
                    continue;
                }

                gridPositionList.Add(testGridPosition);
            }
        }

        ShowGridPositionList(gridPositionList, gridVisualType);
    }

    private void ShowGridPositionRangeSquare(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {
        List<GridPosition> gridPositionList = new List<GridPosition>();

        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <= range; z++)
            {
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                gridPositionList.Add(testGridPosition);
            }
        }

        ShowGridPositionList(gridPositionList, gridVisualType);
    }

    public void ShowGridPositionList(List<GridPosition> gridPositionList, GridVisualType gridVisualType)
    {
        foreach(GridPosition gridPosition in gridPositionList)
        {
            gridSystemVisualSingleArray[gridPosition.x, gridPosition.z].Show(GetGridVisualTypeMaterial(gridVisualType));
        }
    }

    private void UpdateGridVisual()
    {
        HideAllGridPosition();

        Unit selectedunit = UnitActionSystem.Instance.GetSelectedUnit();
        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();

        GridVisualType gridVisualType;
        switch (selectedAction)
        {
            default:
            case MoveAction moveAction:
                gridVisualType = GridVisualType.White;
                break;
            case SpinAction spinAction:
                gridVisualType = GridVisualType.Blue;
                break;
            case ShootAction shootAction:
                gridVisualType = GridVisualType.Red;

                ShowGridPositionRange(selectedunit.GetGridPosition(), shootAction.GetMaxShootDistance(), GridVisualType.RedSoft);
                break;
            case GrenadeAction grenadeAction:
                gridVisualType = GridVisualType.Yellow;
                break;
            case SwordAction swordAction:
                gridVisualType = GridVisualType.Red;
                break;
            case InteractAction interactAction:
                gridVisualType = GridVisualType.Blue;
                break;

        }
        ShowGridPositionList(selectedAction.GetValidActionGridPositionList(), gridVisualType);
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, System.EventArgs e)
    {
        UpdateGridVisual();
    }

    private void LevelGrid_OnAnyUnitMoveGridPosition(object sender, System.EventArgs e)
    {
        UpdateGridVisual();
    }

    private Material GetGridVisualTypeMaterial(GridVisualType gridVisualType)
    {
        foreach (GridVisualTypeMaterial gridVisualTypeMaterial in gridVisualTypeMaterialList)
        {
            if (gridVisualTypeMaterial.gridVisualType == gridVisualType)
            {
                return gridVisualTypeMaterial.material;
            }
        }

        Debug.LogError("Could not find GridVisualTypeMaterial for GridVisualType " + gridVisualType);
        return null;
    }
}
