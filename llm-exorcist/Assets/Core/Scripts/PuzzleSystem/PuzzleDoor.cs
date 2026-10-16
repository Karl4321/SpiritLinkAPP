using System;
using System.Collections;
using UnityEngine;

public class PuzzleDoor : MonoBehaviour
{
    public event Action OnPuzzleStart;
    public event Action OnPuzzleEnd;

    [SerializeField] private FadeScreen _fadeScreen;
    [SerializeField] private Transform _afterSolvedPuzzleTransform;
    [Space(20)]
    [SerializeField] private GameObject _locationBeforePuzzle;
    [SerializeField] private GameObject _locationAfterPuzzle;

    private void Start()
    {
        _locationBeforePuzzle.SetActive(true);
        _locationAfterPuzzle.SetActive(false);
    }

    [ContextMenu("Open Door")]
    public void OpenDoor()
    {
        IEnumerator FadeToChangeLocation()
        {
            if (Player.Instance != null)
            {
                Player.Instance.CanAct = false;
            }
            yield return StartCoroutine(_fadeScreen.Fade());
            _locationBeforePuzzle.SetActive(false);
            _locationAfterPuzzle.SetActive(true);
            Player.Instance.transform.position = _afterSolvedPuzzleTransform.position;
            StartCoroutine(_fadeScreen.Appear());
            if (Player.Instance != null)
            {
                Player.Instance.CanAct = true;
            }
        }

        StartCoroutine(FadeToChangeLocation());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnPuzzleStart?.Invoke();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        OnPuzzleEnd.Invoke();
    }
}
