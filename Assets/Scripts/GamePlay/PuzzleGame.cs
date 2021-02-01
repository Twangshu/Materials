using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleGame : MonoBehaviour
{
    private int puzzleLength=3;
    private bool [,] puzzleState;

    public PuzzleBlock[] puzzleBlocks;

    private void Awake()
    {
        puzzleState = new bool[puzzleLength, puzzleLength];
        for (int i = 0; i <= puzzleLength; i++)
        {
            for (int j = 0; j <= puzzleLength; j++)
            {
                puzzleState[i, j] = false;
            }
        }

        puzzleState[puzzleLength - 1, puzzleLength - 1] = true; //设置一个空格

        //初始化拼图格子ee
        InitPuzzleBlocks();
    }

    private void InitPuzzleBlocks()
    {
        var index = 0;
        for (int i = 0; i < puzzleLength; i++)
        {
            for (int j = 0; j < puzzleLength; j++)
            {
                puzzleBlocks[index].puzzleGame = this;
                puzzleBlocks[index++].position = new Tuple<int,int>(i, j);

            }
        }
    }

    public int GetPuzzleBlocksDir(Tuple<int,int> pos)
    {
        puzzleState[pos.t1, pos.t2] = true;
        //向左
        if (puzzleState[pos.t1 - 1, pos.t2] == true)
        {
            puzzleState[pos.t1 - 1, pos.t2] = false;
            return 1;
        }
        //向右
        if (puzzleState[pos.t1 + 1, pos.t2] == true)
        {
            puzzleState[pos.t1 + 1, pos.t2] = false;
            return 2;
        }
        //向上
        if (puzzleState[pos.t1, pos.t2 + 1] == true)
        {
            puzzleState[pos.t1, pos.t2 + 1] = false;
            return 3;
        }
        //向下
        if (puzzleState[pos.t1, pos.t2 - 1] == true)
        {
            puzzleState[pos.t1, pos.t2 - 1] = false;
            return 4;
        }
        puzzleState[pos.t1, pos.t2] = false;
        //不能移动
        return -1;
    }

    public void CheckFinish()
    {
        foreach (var item in puzzleBlocks)
        {
            if(!(item.position.t1.Equals(item.desPosX)&&item.position.t2.Equals(item.desPosY)))
            {
                return;
            }
        }

        //Finish
    }

}
