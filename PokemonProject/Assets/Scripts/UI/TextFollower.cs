using TMPro;
using UnityEngine;

namespace UI
{
    [ExecuteInEditMode, RequireComponent(typeof(TextMeshProUGUI))]
    public class TextFollower : MonoBehaviour
    {
        private TextMeshProUGUI _text;
        private RectTransform _rectTransform;
        
        [SerializeField] private TextMeshProUGUI textToFollow;
        private RectTransform _textToFollowTransform;
        [Space] 
        [SerializeField] private bool followFontSize = true;
        [SerializeField] private bool followSize = true;


        private void OnEnable()
        {
            if(!_text)_text = GetComponent<TextMeshProUGUI>();
            if(!_rectTransform)_rectTransform = GetComponent<RectTransform>();
            if(!textToFollow)textToFollow = transform.parent.GetComponent<TextMeshProUGUI>();
            if(!_textToFollowTransform)_textToFollowTransform = textToFollow.GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (!_rectTransform) _rectTransform = textToFollow.GetComponent<RectTransform>();   
            _text.text = textToFollow.text;
            if (followFontSize) _text.fontSize = textToFollow.fontSize;
            if (followSize) _rectTransform.sizeDelta = _textToFollowTransform.sizeDelta;
        }
    }
}