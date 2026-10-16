using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Scripts.PuzzleSystem
{
    public class PuzzleContainer : MonoBehaviour
    {
        public static event Action<PuzzleContainer> OnContainerDestroyed;

        [SerializeField] private List<PuzzleItem> _nestedPuzzles = new();
        [SerializeField] private GameObject _freeSpaceItemPrefab;

        public PuzzleItem[] GetNestedPuzzlesArray() => _nestedPuzzles.ToArray();

        private GameObject _freeSpaceItem;

        public void HandleBeginDrag(PuzzleItem firstPuzzle)
        {
            if (!_nestedPuzzles.Contains(firstPuzzle))
            {
                Debug.LogError("There is no such puzzle in container watafuk");
                return;
            }
            var selectedPuzzleIndex = _nestedPuzzles.IndexOf(firstPuzzle);

            if (selectedPuzzleIndex > 0)
            {
                var grabbablePuzzles = FormPuzzleArrayFromPuzzle(selectedPuzzleIndex);

                // set a parent of the created container not to affect this container but make the new one overlap other puzzles
                _nestedPuzzles[0].GetPuzzleGameManager().InstantiateContainer(grabbablePuzzles, transform.position);
                RemovePuzzle(grabbablePuzzles);
            }

            //make this not to be overlapped by other containers
            else
                _nestedPuzzles[0].GetPuzzleGameManager().SetToRoot(transform);
        }

        public void HandleDrag(PuzzleItem firstPuzzle)
        {
            transform.position = Input.mousePosition;
        }

        public void HandleEndDrag(PuzzleItem firstPuzzle, PuzzleItem targetPuzzle)
        {
            firstPuzzle.GetPuzzleGameManager().SetToWorkspace(transform);

            if (targetPuzzle != null)
            {
                //maybe not the best solution..:
                targetPuzzle.ParentContainer.OnPuzzleOverlapStopped(targetPuzzle, firstPuzzle);

                var targetContainerPuzzlesArray = targetPuzzle.ParentContainer.GetNestedPuzzlesArray();

                var index = Array.FindIndex(targetContainerPuzzlesArray, x => x == targetPuzzle) + 1;
                var startIndex = index == targetContainerPuzzlesArray.Length ? -1 : index;

                firstPuzzle.GetPuzzleGameManager().MergeContainers(this, targetPuzzle.ParentContainer, startIndex);
            }

            //gotta be working perfectly cause by the time this method is being invoked, RescalableWorkspace 
            //will rescale the workspace properly
            foreach (var puzzle in firstPuzzle.ParentContainer.GetNestedPuzzlesArray())
                puzzle.CheckLocation();
        }

        public void OnPuzzleOverlap(PuzzleItem childPuzzle, PuzzleItem otherPuzzle)
        {
            var size = otherPuzzle.GetComponent<RectTransform>().rect.size;
            if (otherPuzzle.TrySetOverlappingPuzzle(childPuzzle))
                HighlightFreeSpace(childPuzzle, size);

            //experimental:
            RecalculateRect();
        }

        public void OnPuzzleOverlapStopped(PuzzleItem childPuzzle, PuzzleItem otherPuzzle)
        {
            if (otherPuzzle.TryRemoveOverlappingPuzzle(childPuzzle))
                RemoveFreeSpace(childPuzzle);

            //experimental:
            RecalculateRect();
        }

        public void PushPuzzle(PuzzleItem[] puzzles, int startIndex)
        {
            foreach (var puzzle in puzzles)
            {
                PushPuzzle(puzzle, startIndex++);
            }
        }

        public void PushPuzzle(PuzzleItem[] puzzles)
        {
            foreach (var puzzle in puzzles)
            {
                PushPuzzle(puzzle);
            }
        }

        public void PushPuzzle(PuzzleItem puzzle, int startIndex)
        {
            if (_nestedPuzzles.Contains(puzzle))
                return;

            puzzle.SetContainer(this);
            puzzle.transform.SetParent(transform);

            puzzle.transform.SetSiblingIndex(startIndex);
            _nestedPuzzles.Insert(startIndex, puzzle);
        }

        public void PushPuzzle(PuzzleItem puzzle)
        {
            if (_nestedPuzzles.Contains(puzzle))
                return;

            puzzle.SetContainer(this);
            puzzle.transform.SetParent(transform);
            _nestedPuzzles.Add(puzzle);
        }

        public void RemovePuzzle(PuzzleItem[] puzzles)
        {
            foreach (var puzzle in puzzles)
            {
                RemovePuzzle(puzzle);
            }
        }

        public void RemovePuzzle(PuzzleItem puzzle)
        {
            _nestedPuzzles.Remove(puzzle);

            if (_nestedPuzzles.Count == 0)
            {
                OnContainerDestroyed?.Invoke(this);
                Destroy(gameObject);
            }
        }

        public Vector2 GetBottomPoint() => _nestedPuzzles.Last().GetBottomPoint();

        private void HighlightFreeSpace(PuzzleItem connectionPuzzle, Vector2 size)
        {
            if (_freeSpaceItem != null)
            {
                Debug.LogError("free space item is not nul watafuk");
                return;
            }

            _freeSpaceItem = Instantiate(_freeSpaceItemPrefab, transform);
            _freeSpaceItem.GetComponent<RectTransform>().sizeDelta = size;
            _freeSpaceItem.transform.SetSiblingIndex(_nestedPuzzles.IndexOf(connectionPuzzle) + 1);
        }

        private void RemoveFreeSpace(PuzzleItem connectionPuzzle)
        {
            if (_freeSpaceItem == null)
            {
                Debug.LogError("free space item is null watafuk");
                return;
            }

            Destroy(_freeSpaceItem);
        }

        private PuzzleItem[] FormPuzzleArrayFromPuzzle(int startIndex)
        {
            return _nestedPuzzles.Skip(startIndex).ToArray();
        }

        private void RecalculateRect()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        }
    }
}