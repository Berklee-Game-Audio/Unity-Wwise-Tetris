using UnityEngine;
using TetrisEngine.TetriminosPiece;
using System.Collections.Generic;
using pooling;

namespace TetrisEngine
{
    //This class is responsable for conecting the engine to the view
    //It is also responsable for calling Playfield.Step
    public class GameLogic : MonoBehaviour
    {
        private const string JSON_PATH = @"Assets/SupportFiles/GameSettings.json";

        public GameObject tetriminoBlockPrefab;
        public Transform tetriminoParent;

        [Header("This property will be overriten by GameSettings.json file.")]
        [Space(-10)]
        [Header("You can play with it while the game is in Play-Mode.")]

        public float timeToStep = 2f;

        [Header("Set rows cleared as game levels.")]
        public int[] levels = new int[3];
        private int currentLevel = 0;
        private GameSettings mGameSettings;
        private Playfield mPlayfield;
        private List<TetriminoView> mTetriminos = new List<TetriminoView>();
        private float mTimer = 0f;

        [Space(10)]
        [SerializeField]
        protected bool enableStartScreen = true;
        private Pooling<TetriminoBlock> mBlockPool = new Pooling<TetriminoBlock>();
        private Pooling<TetriminoView> mTetriminoPool = new Pooling<TetriminoView>();

        private Tetrimino mCurrentTetrimino
        {
            get
            {
                return (mTetriminos.Count > 0 && !mTetriminos[mTetriminos.Count - 1].isLocked) ? mTetriminos[mTetriminos.Count - 1].currentTetrimino : null;
            }
        }

        private TetriminoView mPreview;
        private bool mRefreshPreview;
        private bool mGameIsOver = true;

        public void Start()
        {

            if (!enableStartScreen)
            {
                StartGame.instance.HideScreen(0f);
                Begin();
                return;
            }
            GameOver.instance.HideScreen(0f);
            Score.instance.HideScreen(0f);
            LevelUp.instance.HideScreen(0f);
            StartGame.instance.ShowScreen(0f);
        }
        // Initiates pooling systems and playfield.
        public void Begin()
        {
            mBlockPool.createMoreIfNeeded = true;
            mBlockPool.Initialize(tetriminoBlockPrefab, null);

            mTetriminoPool.createMoreIfNeeded = true;
            mTetriminoPool.Initialize(new GameObject("BlockHolder", typeof(RectTransform)), tetriminoParent);
            mTetriminoPool.OnObjectCreationCallBack += x =>
            {
                x.OnDestroyTetrimoView = DestroyTetrimino;
                x.blockPool = mBlockPool;
            };

            LoadSettings();

            mPlayfield = new Playfield(mGameSettings);
            mPlayfield.OnCurrentPieceReachBottom = CreateTetrimino;
            mPlayfield.OnGameOver = SetGameOver;
            mPlayfield.OnDestroyLine = DestroyLine;

            RestartGame();
        }

        //Called when the game starts and when user click Restart Game on GameOver screen
        //Responsable for restaring all necessary components
        public void RestartGame()
        {
            GameOver.instance.HideScreen(0f);
            Score.instance.HideScreen(0f);
            LevelUp.instance.HideScreen(0f);
            StartGame.instance.HideScreen(1f);
            Score.instance.ResetScore();


            mTimer = 0f;

            mPlayfield.ResetGame();
            mTetriminoPool.ReleaseAll();
            mTetriminos.Clear();

            CreateTetrimino();
            mGameIsOver = false;
        }

        private void LoadSettings()
        {
            // Checks for the json file
            if (!System.IO.File.Exists(JSON_PATH))
                throw new System.Exception(string.Format("GameSettings.json could not be found inside {0}. Create one in Window>GameSettings Creator.", JSON_PATH));

            // Loads the GameSettings Json
            var json = System.IO.File.ReadAllText(JSON_PATH);
            mGameSettings = JsonUtility.FromJson<GameSettings>(json);
            mGameSettings.CheckValidSettings();
            timeToStep = mGameSettings.timeToStep;
        }

        //Callback from Playfield to destroy a line in view
        private void DestroyLine(int y)
        {
            mTetriminos.ForEach(x => x.DestroyLine(y));
            mTetriminos.RemoveAll(x => x == null);

            Score.instance.AddPoints(mGameSettings.pointsByBreakingLine);

            int rowsCleared = Score.instance.PlayerScore / mGameSettings.pointsByBreakingLine;

            if (rowsCleared >= levels[levels.Length - 1])
            {
                // Wwise integration for all levels cleared.
                SetGameOver(true);
                return;
            }
            for (int i = levels.Length - 2; i >= 0; i--)
            {
                if (rowsCleared >= levels[i])
                {
                    // Wwise integration for level cleared.
                    currentLevel = i;
                    if (mGameSettings.debugMode)
                    {
                        Debug.Log("Level " + currentLevel + " cleared");
                    }
                    LevelUp.instance.SetLevel(i);
                    break;
                }
            }
        }

        //Callback from Playfield to show game over in view
        private void SetGameOver(bool isWin = false)
        {
            mGameIsOver = true;
            GameOver.instance.ShowScreen(isWin, levels.Length);
        }

        //Call to the engine to create a new piece and create a representation of the random piece in view
        private void CreateTetrimino()
        {
            if (mCurrentTetrimino != null)
                mCurrentTetrimino.isLocked = true;

            var tetrimino = mPlayfield.CreateTetrimo();
            var tetriminoView = mTetriminoPool.Collect();
            tetriminoView.InitiateTetrimino(tetrimino);
            mTetriminos.Add(tetriminoView);

            if (mPreview != null)
                mTetriminoPool.Release(mPreview);

            mPreview = mTetriminoPool.Collect();
            mPreview.InitiateTetrimino(tetrimino, true);
            mRefreshPreview = true;
        }

        //When all the blocks of a piece is destroyed, we must release ("destroy") it.
        private void DestroyTetrimino(TetriminoView obj)
        {
            var index = mTetriminos.FindIndex(x => x == obj);
            mTetriminoPool.Release(obj);
            mTetriminos[index] = null;
        }

        //Regular Unity Update method
        //Responsable for counting down and calling Step
        //Also responsable for gathering users input
        public void Update()
        {
            if (mGameIsOver) return;

            mTimer += Time.deltaTime;
            if (mTimer > timeToStep)
            {
                mTimer = 0;
                mPlayfield.Step();
            }

            if (mCurrentTetrimino == null) return;

            //Rotate Right
            if (Input.GetKeyDown(mGameSettings.rotateRightKey))
            {
                if (mPlayfield.IsPossibleMovement(mCurrentTetrimino.currentPosition.x,
                                    mCurrentTetrimino.currentPosition.y,
                                    mCurrentTetrimino,
                                                    mCurrentTetrimino.NextRotation))
                {
                    mCurrentTetrimino.currentRotation = mCurrentTetrimino.NextRotation;
                    mRefreshPreview = true;
                }
            }

            //Rotate Left
            if (Input.GetKeyDown(mGameSettings.rotateLeftKey))
            {
                if (mPlayfield.IsPossibleMovement(mCurrentTetrimino.currentPosition.x,
                                                  mCurrentTetrimino.currentPosition.y,
                                                  mCurrentTetrimino,
                                            mCurrentTetrimino.PreviousRotation))
                {
                    mCurrentTetrimino.currentRotation = mCurrentTetrimino.PreviousRotation;
                    mRefreshPreview = true;
                }
            }

            //Move piece to the left
            if (Input.GetKeyDown(mGameSettings.moveLeftKey))
            {
                if (mPlayfield.IsPossibleMovement(mCurrentTetrimino.currentPosition.x - 1,
                                                  mCurrentTetrimino.currentPosition.y,
                                                  mCurrentTetrimino,
                                                  mCurrentTetrimino.currentRotation))
                {
                    mCurrentTetrimino.currentPosition = new Vector2Int(mCurrentTetrimino.currentPosition.x - 1, mCurrentTetrimino.currentPosition.y);
                    mRefreshPreview = true;
                }
            }

            //Move piece to the right
            if (Input.GetKeyDown(mGameSettings.moveRightKey))
            {
                if (mPlayfield.IsPossibleMovement(mCurrentTetrimino.currentPosition.x + 1,
                                                  mCurrentTetrimino.currentPosition.y,
                                                  mCurrentTetrimino,
                                                  mCurrentTetrimino.currentRotation))
                {
                    mCurrentTetrimino.currentPosition = new Vector2Int(mCurrentTetrimino.currentPosition.x + 1, mCurrentTetrimino.currentPosition.y);
                    mRefreshPreview = true;
                }
            }

            //Make the piece fall faster
            //this is the only input with GetKey instead of GetKeyDown, because most of the time, users want to keep this button pressed and make the piece fall
            if (Input.GetKey(mGameSettings.moveDownKey))
            {
                if (mPlayfield.IsPossibleMovement(mCurrentTetrimino.currentPosition.x,
                                                  mCurrentTetrimino.currentPosition.y + 1,
                                                  mCurrentTetrimino,
                                          mCurrentTetrimino.currentRotation))
                {
                    mCurrentTetrimino.currentPosition = new Vector2Int(mCurrentTetrimino.currentPosition.x, mCurrentTetrimino.currentPosition.y + 1);
                }
            }

            //This part is responsable for rendering the preview piece in the right position
            if (mRefreshPreview)
            {
                var y = mCurrentTetrimino.currentPosition.y;
                while (mPlayfield.IsPossibleMovement(mCurrentTetrimino.currentPosition.x,
                                                          y,
                                                          mCurrentTetrimino,
                                                          mCurrentTetrimino.currentRotation))
                {
                    y++;
                }

                mPreview.ForcePosition(mCurrentTetrimino.currentPosition.x, y - 1);
                mRefreshPreview = false;
            }
        }
    }
}
