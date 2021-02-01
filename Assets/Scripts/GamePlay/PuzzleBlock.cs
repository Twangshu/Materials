using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleBlock : MonoBehaviour
{
    //以UIImage形式展现
    private Button m_thisButton;
    [SerializeField]
    public Tuple<int, int> position;
    public int desPosX;
    public int desPosY;
    public PuzzleGame puzzleGame;
    

    private void Awake()
    {
        m_thisButton.onClick.AddListener(() =>
        {
            var dest = puzzleGame.GetPuzzleBlocksDir(position);
            switch(dest)
            {
                case -1:
                    break;
                case 1://左
                    position.t1 -= 1;
                    break;
                case 2://右
                    position.t1 += 1;
                    break;
                case 3://上
                    position.t2 += 1;
                    break;
                case 4://下
                    position.t2 -= 1;
                    break;
                default: 
                    break;
            }
            if (dest != -1)
                puzzleGame.CheckFinish();
        }
        );
    }
}
