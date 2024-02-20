using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private Button endTurnButton;
    [SerializeField] private TextMeshProUGUI turnNumberText;
    [SerializeField] private Transform enemyTurnVusualGameObject;

    private void Start()
    {
        UpdateText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

        endTurnButton.onClick.AddListener(() =>
        {
            TurnSystem.Instance.NextTurn();
        });
    }

    private void TurnSystem_OnTurnChanged(object sender, System.EventArgs e)
    {
        UpdateText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
    }

    private void UpdateText()
    {
        turnNumberText.text = "TURN " + TurnSystem.Instance.GetTurnNumber();
    }

    private void UpdateEnemyTurnVisual()
    {
        enemyTurnVusualGameObject.gameObject.SetActive(!TurnSystem.Instance.IsPlayerTurn());
    }

    private void UpdateEndTurnButtonVisibility()
    {
        endTurnButton.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
    }
}
