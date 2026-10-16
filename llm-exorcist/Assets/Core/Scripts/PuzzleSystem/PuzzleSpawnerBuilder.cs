using Core.Scripts.PuzzleSystem;

namespace Assets.Core.Scripts.PuzzleSystem
{
    public class PuzzleSpawnerBuilder
    {
        public PuzzleSpawnerBuilder(PuzzleGameManager puzzleGameManager, PuzzleSpawner puzzleSpawner)
        {
            puzzleSpawner.PuzzleGameManager = puzzleGameManager;
        }
    }
}
