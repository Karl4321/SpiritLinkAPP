using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.Scripts.UI
{
    public class HoverableObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Objects")]
        [SerializeField] private GameObject _onHoverHint;
        [Header("UI elements")]
        [SerializeField] private Button _textButton;
        [SerializeField] private Button _infoButton;
        [Header("Settings")]
        [SerializeField] private string _textString;
        [SerializeField] private string _infoString;
        [SerializeField] private float _hintShowDelay = 0.5f;
        [SerializeField] private Transform _globalTransform;

        private float _hintShowTimer;
        private bool _isTimerCounting;
        private bool _isTracking = true;
        private TextMeshProUGUI _hintText;
        private Transform _beforeHintParent;

        private void Start()
        {
            HideHint();
            _hintText = _onHoverHint.GetComponentInChildren<TextMeshProUGUI>();

            _textButton.onClick.AddListener(SetHintLayoutText);
            _infoButton.onClick.AddListener(SetHintLayoutInfo);

            SetHintLayoutText();
        }

        private void Update()
        {
            var hintRect = _hintText.transform.parent.GetComponent<RectTransform>();
            if (_hintText.isTextOverflowing)
            {
                hintRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, hintRect.sizeDelta.y + 5);
                Debug.Log("rescale");
            }

            if (_isTimerCounting && _hintShowTimer < _hintShowDelay)
            {
                _hintShowTimer += Time.deltaTime;
                if (_hintShowTimer > _hintShowDelay)
                {
                    ShowHint();
                }
            }
        }

        public void SetHintText(string extendedText, string infoText)
        {
            _textString = extendedText;
            _infoString = infoText;
        }

        public void SetHoverTracking(bool isTracking)
        {
            _isTracking = isTracking;
            _isTimerCounting = isTracking;

            if (!isTracking)
            {
                _hintShowTimer = 0f;
                HideHint();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_isTracking) return;

            _hintShowTimer = 0f;
            _isTimerCounting = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_isTracking) return;

            _hintShowTimer = 0f;
            _isTimerCounting = false;
            HideHint();
        }

        public void SetGlobalTransform(Transform globalTransform)
        {
            _globalTransform = globalTransform;
        }

        private void ShowHint()
        {
            // change transform's parent to avoid other puzzle's overlapping
            _beforeHintParent = transform.parent;
            transform.SetParent(_globalTransform);
            _onHoverHint.SetActive(true);
        }

        private void HideHint()
        {
            if (_beforeHintParent != null)
            {
                transform.SetParent(_beforeHintParent);
            }
            _onHoverHint.SetActive(false);
        }

        private void SetHintLayoutText()
        {
            _hintText.text = _textString;
            _textButton.GetComponent<Image>().color = _textButton.colors.pressedColor;
            _infoButton.GetComponent<Image>().color = _infoButton.colors.normalColor;
        }

        private void SetHintLayoutInfo()
        {
            _hintText.text = _infoString;
            _textButton.GetComponent<Image>().color = _textButton.colors.normalColor;
            _infoButton.GetComponent<Image>().color = _infoButton.colors.pressedColor;
        }
    }
}