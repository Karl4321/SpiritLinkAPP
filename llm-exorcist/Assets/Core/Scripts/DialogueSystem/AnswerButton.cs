using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AnswerButton : MonoBehaviour
{
    [SerializeField] private TMP_Text _textChamber;
    public Button Button { get; private set; }
    public TMP_Text TextChamber
    {
        get => _textChamber;
    }

    private void OnEnable()
    {
        _textChamber.color = Color.black;
        _textChamber.text = string.Empty;
        Button = GetComponent<Button>();
    }

    private void OnDisable()
    {
        Button.onClick.RemoveAllListeners();
    }
}
