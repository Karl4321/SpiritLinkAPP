using Assets.Core.Scripts.PuzzleSystem;
using UnityEngine;

namespace Core.Scripts.PuzzleSystem
{
    public class PuzzleSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _puzzleItemPrefab;

        public PuzzleItem SpawnedPuzzle { get; private set; }

        public PuzzleGameManager PuzzleGameManager { private get; set; }

        private void OnEnable()
        {
            PuzzleItem.OnPuzzleReturnedToSpawner += PuzzleItem_OnPuzzleReturnedToSpawner;
        }

        private void OnDisable()
        {
            PuzzleItem.OnPuzzleReturnedToSpawner += PuzzleItem_OnPuzzleReturnedToSpawner;
        }

        public void CreatePuzzle(PuzzleItemDataStruct puzzleItemDataStruct)
        {
            if (SpawnedPuzzle != null)
            {
                Debug.LogError("There is already a puzzle in this spawner");
                return;
            }

            var puzzle = Instantiate(_puzzleItemPrefab, transform).GetComponent<PuzzleItem>();
            puzzle.SetSpawner(this);
            puzzle.Initialize(puzzleItemDataStruct.isActive, puzzleItemDataStruct.indexInSequence,
                            puzzleItemDataStruct.puzzleText, puzzleItemDataStruct.puzzleShortText, puzzleItemDataStruct.puzzleInfo);
            InstantiatePuzzleContainer(puzzle, transform.position, transform);

            SpawnedPuzzle = puzzle;
        }


        private void PuzzleItem_OnPuzzleReturnedToSpawner(PuzzleItem puzzle, PuzzleSpawner spawner)
        {
            if (spawner != this) return;

            InstantiatePuzzleContainer(puzzle, transform.position, transform);
        }

        private GameObject InstantiatePuzzleContainer(PuzzleItem nestedPuzzle, Vector3 position, Transform parent = null)
        {
            return PuzzleGameManager.InstantiateContainer(new PuzzleItem[] { nestedPuzzle }, position, parent);
        }

        public PuzzleGameManager GetPuzzleGameManager()
        {
            return PuzzleGameManager;
        }
    }
}