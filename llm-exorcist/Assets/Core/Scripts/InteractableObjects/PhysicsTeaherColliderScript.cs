using System.Collections.Generic;
using UnityEngine;
using static DialogueCharacter;

public class PhysicsTeaherColliderScript : MonoBehaviour
{
    [SerializeField] private SetAnimParameter _animParameterForCollider;
    [SerializeField] private List<Collider2D> _colliders;
    [Space(30)]
    [SerializeField] private Animator _animator;
    [SerializeField] private DialogueViewer _dialogueViewer;

    private void Start()
    {
        if (_animParameterForCollider.inputText == null)
        {
            enabled = false;
        }
    }

    void Update()
    {
        if (!_dialogueViewer.IsCurrentViewerActive())
        {
            return;
        }
        if (_animParameterForCollider.typeOfDialogue == _dialogueViewer.CurrentDialogueElement.TypeOfDialogue && _animParameterForCollider.inputText == _dialogueViewer.CurrentDialogueElement.simplePhrase.InputText)
        {
            foreach(Collider2D collider in _colliders)
            {
                collider.enabled = false;
            }
            _dialogueViewer = null;
            enabled = false;
        }
    }
}
