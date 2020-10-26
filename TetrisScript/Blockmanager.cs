using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

enum State
{
    A,
    B,
    C,
    D,
}


public class Blockmanager : MonoBehaviour
{
    #region 변수선언부분
    [SerializeField] Tetrino[] Block = new Tetrino[4];
    public Tetrino[] _Block
    {
        get
        {
            return Block;
        }
    }
    
    [SerializeField] int x;
    [SerializeField] int y;
    [SerializeField] int FixCount;
    int Blocktype;
    int Fullcount;
    int lastIdx;
    
    [SerializeField] float Inputdelay;
    float Inputtimer;
    float cool;
    public float cooltime;
    
    bool Move;
    public bool end = false;
    State BlockState;

    public GameObject EndCanvas;
    
    List<int> idxList = new List<int>();
    [SerializeField] List<int> nextIdx = new List<int>();
    public Stack<GameObject> returnList = new Stack<GameObject>();
    public Image nextImage;
    #endregion


    void Start()
    {
        Move = true;
        cooltime = 1.0f;
        Inputdelay = 0.3f;
        CreateTet();
        BlockState = State.A;
    }

    void Update()
    {
        if (Move)
        {
            DownSelf(); // 한칸내리기
            PC_KeyControl(); // PC용 키보드컨트롤
        }
    }

    void DownSelf() // 쿨타임마다 블럭이 한칸씩 내려가는 함수
    {
        cool += Time.deltaTime;
        if (cool > cooltime && MoveCondition(1))
        {
            for (int i = 0; i < 4; i++)
            {
                mapmanager.instance._Map[Block[i].x, Block[i].y] = null;
            }
            for (int i = 0; i < 4; i++)
            {
                mapmanager.instance._Map[Block[i].x, Block[i].y - 1] = Block[i].gameObject;
                Block[i].y--;
            }
            y--;
            cool = 0;
        }
    }

    void PC_KeyControl() // 키보드입력받는 함수
    {
        if (Move)
        {
            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                DownMove();
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                Inputtimer += Time.deltaTime;

                if (Inputdelay < Inputtimer)
                {
                    DownMove();
                    Inputdelay = 0.06f;
                }
            }
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                LeftMove();
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                Inputtimer += Time.deltaTime;
                if (Inputdelay < Inputtimer)
                {
                    LeftMove();
                    Inputdelay = 0.06f;

                }
            }
            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                RightMove();
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                Inputtimer += Time.deltaTime;
                if (Inputdelay < Inputtimer)
                {
                    RightMove();
                    Inputdelay = 0.06f;
                }
            }
            if (Input.GetKeyDown(KeyCode.Space) && MoveCondition(1))
            {
                cooltime = 0;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                UpButton();
            }

        }
        if (EndCanvas.activeInHierarchy == true && Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("lobby");
        }
    }

    #region 블럭생성함수
    void CreateTet() // 테트리스블럭 생성하는 함수
    {
        if (end)
            return;

        GameObject Tetrino = Resources.Load("02prefab/tetrino", typeof(GameObject)) as GameObject;
        int idx = 99;

        while (nextIdx.Count < 2)
        {
            do
            {
                if (idxList.Count > 13)
                    idxList.Clear();

                int rand = Random.Range(0, 70);
                idx = rand % 7;
                if (idxList.Count == 13 && idxList.FindAll(item => item == idx).Count == 1 && idx == lastIdx)
                    break;
            }
            while (idxList.FindAll(item => item == idx).Count > 1 || idx == lastIdx);

            nextIdx.Add(idx);
            idxList.Add(idx);
            lastIdx = idx;
        }

        // 넥스트idx 1번째를 다음블럭으로 보여주고
        // 넥스트 0번째를 밑에 전달하고
        // 넥스트 0번째를 삭제한다
        Sprite sprite = Resources.Load("01img/nextblock/" + nextIdx[1], typeof(Sprite)) as Sprite;
        nextImage.sprite = sprite;

        int blocknumber = nextIdx[0];
        nextIdx.RemoveAt(0);

        switch (blocknumber)
        {
            case 1:// I자 블럭 생성하는 부분
                for (int i = 0; i < 4; i++)
                {
                    x = 4;
                    y = 20;
                    Tetrino.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
                    if (mapmanager.instance._Map[i + x, y] != null)
                    {
                        if (mapmanager.instance._Map[i + x, y].CompareTag("fixed"))
                        {
                            Move = false;
                            EndGame();
                            return;
                        }
                    }
                    else
                    {
                        Block[i] = Instantiate(Tetrino, transform).GetComponent<Tetrino>();
                        mapmanager.instance._Map[i + x, y] = Block[i].gameObject;
                        Block[i].x = x + i;
                        Block[i].y = y;
                    }
                }
                Blocktype = 1;
                break;
            case 2: // L자 블럭 생성하는 부분
                {
                    for (int i = 0; i < 4; i++)
                    {
                        x = 4;
                        y = 19;
                        Tetrino.GetComponent<SpriteRenderer>().color = new Color(1, 0.435f, 0);
                        if (mapmanager.instance._Map[i + x, y] != null)
                        {
                            if (mapmanager.instance._Map[i + x, y].CompareTag("fixed"))
                            {
                                Move = false;
                                EndGame();
                                return;
                            }
                        }
                        else
                        {
                            Block[i] = Instantiate(Tetrino, transform).GetComponent<Tetrino>();
                        }
                    }
                    Block[0].x = x;
                    Block[0].y = y;
                    mapmanager.instance._Map[x, y] = Block[0].gameObject;
                    Block[1].x = x + 1;
                    Block[1].y = y;
                    mapmanager.instance._Map[x + 1, y] = Block[1].gameObject;
                    Block[2].x = x + 2;
                    Block[2].y = y;
                    mapmanager.instance._Map[x + 2, y] = Block[2].gameObject;
                    Block[3].x = x + 2;
                    Block[3].y = y + 1;
                    mapmanager.instance._Map[x + 2, y + 1] = Block[3].gameObject;

                    Blocktype = 2;
                    break;

                }
            case 3: // LL자 블럭 생성하는 부분
                {
                    for (int i = 0; i < 4; i++)
                    {
                        x = 4;
                        y = 20;
                        Tetrino.GetComponent<SpriteRenderer>().color = new Color(1, 1, 0);
                        if (mapmanager.instance._Map[i + x, y] != null)
                        {
                            if (mapmanager.instance._Map[i + x, y].CompareTag("fixed"))
                            {
                                Move = false;
                                EndGame();
                                return;

                            }
                        }
                        else
                        {
                            Block[i] = Instantiate(Tetrino, transform).GetComponent<Tetrino>();
                        }
                    }
                    Block[0].x = x;
                    Block[0].y = y;
                    mapmanager.instance._Map[x, y] = Block[0].gameObject;
                    Block[1].x = x + 1;
                    Block[1].y = y;
                    mapmanager.instance._Map[x + 1, y] = Block[1].gameObject;
                    Block[2].x = x + 2;
                    Block[2].y = y;
                    mapmanager.instance._Map[x + 2, y] = Block[2].gameObject;
                    Block[3].x = x + 2;
                    Block[3].y = y - 1;
                    mapmanager.instance._Map[x + 2, y - 1] = Block[3].gameObject;

                    Blocktype = 3;
                    break;
                }
            case 4: // T자 블럭 생성하는 부분
                {
                    for (int i = 0; i < 4; i++)
                    {
                        x = 4;
                        y = 19;
                        Tetrino.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0);
                        if (mapmanager.instance._Map[i + x, y] != null)
                        {
                            if (mapmanager.instance._Map[i + x, y].CompareTag("fixed"))
                            {
                                Move = false;
                                EndGame();
                                return;
                            }
                        }
                        else
                        {
                            Block[i] = Instantiate(Tetrino, transform).GetComponent<Tetrino>();
                        }
                    }
                    Block[0].x = x;
                    Block[0].y = y;
                    mapmanager.instance._Map[x, y] = Block[0].gameObject;
                    Block[1].x = x + 1;
                    Block[1].y = y;
                    mapmanager.instance._Map[x + 1, y] = Block[1].gameObject;
                    Block[2].x = x + 2;
                    Block[2].y = y;
                    mapmanager.instance._Map[x + 2, y] = Block[2].gameObject;
                    Block[3].x = x + 1;
                    Block[3].y = y + 1;
                    mapmanager.instance._Map[x + 1, y + 1] = Block[3].gameObject;

                    Blocktype = 4;
                    break;
                }
            case 5: // M자 블럭 생성하는 부분
                {
                    for (int i = 0; i < 4; i++)
                    {
                        x = 4;
                        y = 19;
                        Tetrino.GetComponent<SpriteRenderer>().color = new Color(0, 0.729f, 1);
                        if (mapmanager.instance._Map[i + x, y] != null)
                        {
                            if (mapmanager.instance._Map[i + x, y].CompareTag("fixed"))
                            {
                                Move = false;
                                EndGame();
                                return;
                            }
                        }
                        else
                        {
                            Block[i] = Instantiate(Tetrino, transform).GetComponent<Tetrino>();
                        }
                    }
                    Block[0].x = x;
                    Block[0].y = y;
                    mapmanager.instance._Map[x, y] = Block[0].gameObject;
                    Block[1].x = x + 1;
                    Block[1].y = y;
                    mapmanager.instance._Map[x + 1, y] = Block[1].gameObject;
                    Block[2].x = x;
                    Block[2].y = y + 1;
                    mapmanager.instance._Map[x, y + 1] = Block[2].gameObject;
                    Block[3].x = x + 1;
                    Block[3].y = y + 1;
                    mapmanager.instance._Map[x + 1, y + 1] = Block[3].gameObject;

                    Blocktype = 5;
                    break;
                }
            case 6: // Z자 블럭 생성하는 부분
                {
                    for (int i = 0; i < 4; i++)
                    {
                        x = 4;
                        y = 19;
                        Tetrino.GetComponent<SpriteRenderer>().color = new Color(0, 0.172f, 1);
                        if (mapmanager.instance._Map[i + x, y] != null)
                        {
                            if (mapmanager.instance._Map[i + x, y].CompareTag("fixed"))
                            {
                                Move = false;
                                EndGame();
                                return;
                            }
                        }
                        else
                        {
                            Block[i] = Instantiate(Tetrino, transform).GetComponent<Tetrino>();
                        }
                    }
                    Block[0].x = x;
                    Block[0].y = y;
                    mapmanager.instance._Map[x, y] = Block[0].gameObject;
                    Block[1].x = x + 1;
                    Block[1].y = y;
                    mapmanager.instance._Map[x + 1, y] = Block[1].gameObject;
                    Block[2].x = x + 1;
                    Block[2].y = y + 1;
                    mapmanager.instance._Map[x + 1, y + 1] = Block[2].gameObject;
                    Block[3].x = x + 2;
                    Block[3].y = y + 1;
                    mapmanager.instance._Map[x + 2, y + 1] = Block[3].gameObject;

                    Blocktype = 6;
                    break;
                }
            case 0: // ZZ자 블럭 생성하는 부분
                {
                    for (int i = 0; i < 4; i++)
                    {
                        x = 4;
                        y = 19;
                        Tetrino.GetComponent<SpriteRenderer>().color = new Color(0.741f, 0, 1);
                        if (mapmanager.instance._Map[i + x, y] != null)
                        {
                            if (mapmanager.instance._Map[i + x, y].CompareTag("fixed"))
                            {
                                Move = false;
                                EndGame();
                                return;
                            }
                        }
                        else
                        {
                            Block[i] = Instantiate(Tetrino, transform).GetComponent<Tetrino>();
                        }
                    }
                    Block[0].x = x;
                    Block[0].y = y + 1;
                    mapmanager.instance._Map[x, y + 1] = Block[0].gameObject;
                    Block[1].x = x + 1;
                    Block[1].y = y + 1;
                    mapmanager.instance._Map[x + 1, y + 1] = Block[1].gameObject;
                    Block[2].x = x + 1;
                    Block[2].y = y;
                    mapmanager.instance._Map[x + 1, y] = Block[2].gameObject;
                    Block[3].x = x + 2;
                    Block[3].y = y;
                    mapmanager.instance._Map[x + 2, y] = Block[3].gameObject;

                    Blocktype = 7;
                    break;
                }
        }
        
        mapmanager.instance.PrintMap();

    }
    #endregion

    void FixedBlocks() // 블럭 고정
    {
        if (FixCount == 1)
        {
            for (int i = 0; i < 4; i++)
            {
                Block[i].tag = "fixed";
                returnList.Push(Block[i].gameObject);
                Block[i] = null;
            }
            cooltime = 1;
            FixCount = 0;
            BlockState = State.A;
            DelFull();
            GameObject.Find("FixedSound").GetComponent<AudioSource>().Play();
            CreateTet();
        }
        FixCount++;
    }

    public bool MoveCondition(int a) // 이동가능한지 판단
    {
        if (CheckWall(a, Block[0].x, Block[0].y))
            if (CheckWall(a, Block[1].x, Block[1].y))
                if (CheckWall(a, Block[2].x, Block[2].y))
                    if (CheckWall(a, Block[3].x, Block[3].y))
                        return true;

        return false;
    }

    bool CheckWall(int a, int x, int y) // 막히는 곳이 있는지 판단
    {
        switch (a)
        {
            case 1: // 아래쪽 판단
                if (mapmanager.instance._Map[x, y - 1] == null)
                    return true;
                else if (mapmanager.instance._Map[x, y - 1] != null)
                {
                    if (mapmanager.instance._Map[x, y - 1].CompareTag("tetrino"))
                    {
                        return true;
                    }
                    else
                    {
                        FixedBlocks();
                    }
                }

                break;
            case 2: // 왼쪽판단
                if (mapmanager.instance._Map[x - 1, y] == null || mapmanager.instance._Map[x - 1, y].CompareTag("tetrino"))
                    return true;
                break;
            case 3: // 오른쪽판단
                if (mapmanager.instance._Map[x + 1, y] == null || mapmanager.instance._Map[x + 1, y].CompareTag("tetrino"))
                    return true;
                break;
        }
        return false;
    }

    void DelFull() // 가득찬 줄을 삭제함
    {
        for (int j = 1; j < 20; j++)
        {
            for (int i = 1; i < 11; i++)
            {
                if (mapmanager.instance._Map[i, j] != null)
                {
                    if (mapmanager.instance._Map[i, j].CompareTag("fixed"))
                        Fullcount++;
                }
            }
            if (Fullcount == 10)
            {
                for (int i = 1; i < 11; i++)
                {
                    Destroy(mapmanager.instance._Map[i, j]);
                    mapmanager.instance._Map[i, j] = null;
                }
                ScoreManager._SM.AddScore();
                GameObject.Find("DeleteSound").GetComponent<AudioSource>().Play();
                DownAll(j);
                j--;
            }
            Fullcount = 0;
        }

    }

    void DownAll(int j) // j줄 이상을 한칸씩 내려줌
    {
        for (; j < 20; j++)
        {
            for (int i = 1; i < 11; i++)
            {
                mapmanager.instance._Map[i, j] = mapmanager.instance._Map[i, j + 1];
                mapmanager.instance._Map[i, j + 1] = null;
            }
        }
    }

    #region 블럭회전함수
    void RollBlockI()
    {

        switch (BlockState)
        {
            case State.A:
                if (mapmanager.instance._Map[Block[0].x + 3, Block[0].y + 3] == null)
                {
                    if (mapmanager.instance._Map[Block[1].x + 2, Block[1].y + 2] == null)
                    {
                        if (mapmanager.instance._Map[Block[2].x + 1, Block[2].y + 1] == null)
                        {
                            mapmanager.instance._Map[Block[0].x, Block[0].y] = null;
                            mapmanager.instance._Map[Block[1].x, Block[1].y] = null;
                            mapmanager.instance._Map[Block[2].x, Block[2].y] = null;


                            Block[0].x += 3;
                            Block[0].y += 3;
                            Block[2].x += 1;
                            Block[2].y += 1;
                            Block[1].x += 2;
                            Block[1].y += 2;
                            mapmanager.instance._Map[Block[0].x, Block[0].y] = Block[0].gameObject;
                            mapmanager.instance._Map[Block[1].x, Block[1].y] = Block[1].gameObject;
                            mapmanager.instance._Map[Block[2].x, Block[2].y] = Block[2].gameObject;
                            BlockState = State.B;
                        }
                    }
                }
                break;
            case State.B:
                if (Block[3].x <= 4)
                {
                    int num = 4 - Block[3].x;
                    for (int i = 0; i < num; i++)
                    {
                        RightMove();
                    }
                }
                if (mapmanager.instance._Map[Block[0].x - 3, Block[0].y - 3] == null)
                {
                    if (mapmanager.instance._Map[Block[1].x - 2, Block[1].y - 2] == null)
                    {
                        if (mapmanager.instance._Map[Block[2].x - 1, Block[2].y - 1] == null)
                        {
                            mapmanager.instance._Map[Block[0].x, Block[0].y] = null;
                            mapmanager.instance._Map[Block[1].x, Block[1].y] = null;
                            mapmanager.instance._Map[Block[2].x, Block[2].y] = null;


                            Block[0].x -= 3;
                            Block[0].y -= 3;
                            Block[2].x -= 1;
                            Block[2].y -= 1;
                            Block[1].x -= 2;
                            Block[1].y -= 2;
                            mapmanager.instance._Map[Block[0].x, Block[0].y] = Block[0].gameObject;
                            mapmanager.instance._Map[Block[1].x, Block[1].y] = Block[1].gameObject;
                            mapmanager.instance._Map[Block[2].x, Block[2].y] = Block[2].gameObject;
                            BlockState = State.A;
                        }
                    }
                }

                break;
        }
    }


    void RollBlockIL()
    {
        switch (BlockState)
        {
            case State.A:

                if (mapmanager.instance._Map[Block[0].x + 1, Block[0].y + 1] == null)
                {
                    if (mapmanager.instance._Map[Block[3].x - 1, Block[3].y + 1] == null)
                    {
                        mapmanager.instance._Map[Block[0].x, Block[0].y] = null;
                        mapmanager.instance._Map[Block[3].x, Block[3].y] = null;

                        Block[0].y += 1;
                        Block[0].x += 1;
                        Block[3].y += 1;
                        Block[3].x -= 1;
                        mapmanager.instance._Map[Block[0].x, Block[0].y] = Block[0].gameObject;
                        mapmanager.instance._Map[Block[3].x, Block[3].y] = Block[3].gameObject;
                        BlockState = State.B;
                    }
                }
                break;
            case State.B:
                if (Block[3].x == 1 || Block[1].x == 1)
                    RightMove();
                if (mapmanager.instance._Map[Block[3].x - 1, Block[3].y - 1] == null)
                {
                    if (mapmanager.instance._Map[Block[1].x - 1, Block[1].y] == null)
                    {
                        if (mapmanager.instance._Map[Block[2].x, Block[2].y + 1] == null)
                        {
                            mapmanager.instance._Map[Block[3].x, Block[3].y] = null;
                            mapmanager.instance._Map[Block[1].x, Block[1].y] = null;
                            mapmanager.instance._Map[Block[2].x, Block[2].y] = null;


                            Block[3].y -= 1;
                            Block[3].x -= 1;
                            Block[1].x -= 1;
                            Block[2].y += 1;
                            mapmanager.instance._Map[Block[3].x, Block[3].y] = Block[3].gameObject;
                            mapmanager.instance._Map[Block[1].x, Block[1].y] = Block[1].gameObject;
                            mapmanager.instance._Map[Block[2].x, Block[2].y] = Block[2].gameObject;
                            BlockState = State.C;
                        }
                    }
                }
                break;
            case State.C:
                if (mapmanager.instance._Map[Block[3].x, Block[3].y + 1] == null)
                {
                    if (mapmanager.instance._Map[Block[1].x + 1, Block[1].y] == null)
                    {
                        if (mapmanager.instance._Map[Block[2].x - 1, Block[2].y + 1] == null)
                        {
                            mapmanager.instance._Map[Block[3].x, Block[3].y] = null;
                            mapmanager.instance._Map[Block[1].x, Block[1].y] = null;
                            mapmanager.instance._Map[Block[2].x, Block[2].y] = null;


                            Block[3].y += 1;
                            Block[1].x += 1;
                            Block[2].x -= 1;
                            Block[2].y += 1;
                            mapmanager.instance._Map[Block[3].x, Block[3].y] = Block[3].gameObject;
                            mapmanager.instance._Map[Block[1].x, Block[1].y] = Block[1].gameObject;
                            mapmanager.instance._Map[Block[2].x, Block[2].y] = Block[2].gameObject;
                            BlockState = State.D;
                        }
                    }
                }
                break;
            case State.D:
                if (Block[0].x == 10)
                    LeftMove();
                if (mapmanager.instance._Map[Block[3].x + 2, Block[3].y - 1] == null)
                {
                    if (mapmanager.instance._Map[Block[0].x - 1, Block[0].y - 1] == null)
                    {
                        if (mapmanager.instance._Map[Block[2].x + 1, Block[2].y - 2] == null)
                        {
                            mapmanager.instance._Map[Block[3].x, Block[3].y] = null;
                            mapmanager.instance._Map[Block[0].x, Block[0].y] = null;
                            mapmanager.instance._Map[Block[2].x, Block[2].y] = null;


                            Block[3].y -= 1;
                            Block[3].x += 2;
                            Block[2].y -= 2;
                            Block[2].x += 1;
                            Block[0].y -= 1;
                            Block[0].x -= 1;
                            mapmanager.instance._Map[Block[3].x, Block[3].y] = Block[3].gameObject;
                            mapmanager.instance._Map[Block[0].x, Block[0].y] = Block[0].gameObject;
                            mapmanager.instance._Map[Block[2].x, Block[2].y] = Block[2].gameObject;
                            BlockState = State.A;
                        }
                    }
                }

                break;
        }
    }

    void RollBlockILL()

    {
        switch (BlockState)
        {
            case State.A:
                if (mapmanager.instance._Map[Block[0].x + 1, Block[0].y - 1] == null)
                {
                    if (mapmanager.instance._Map[Block[1].x + 1, Block[1].y + 1] == null)
                    {
                        mapmanager.instance._Map[Block[0].x, Block[0].y] = null;
                        mapmanager.instance._Map[Block[1].x, Block[1].y] = null;

                        Block[0].x += 1;
                        Block[0].y -= 1;
                        Block[1].x += 1;
                        Block[1].y += 1;
                        mapmanager.instance._Map[Block[0].x, Block[0].y] = Block[0].gameObject;
                        mapmanager.instance._Map[Block[1].x, Block[1].y] = Block[1].gameObject;
                        BlockState = State.B;
                    }
                }
                break;
            case State.B:
                if (Block[2].x == 2 || Block[1].x == 2)
                    RightMove();
                if (mapmanager.instance._Map[Block[2].x - 2, Block[2].y - 1] == null)
                {
                    if (mapmanager.instance._Map[Block[1].x - 2, Block[1].y - 1] == null)
                    {
                        mapmanager.instance._Map[Block[2].x, Block[2].y] = null;
                        mapmanager.instance._Map[Block[1].x, Block[1].y] = null;

                        Block[2].x -= 2;
                        Block[2].y -= 1;
                        Block[1].x -= 2;
                        Block[1].y -= 1;
                        mapmanager.instance._Map[Block[2].x, Block[2].y] = Block[2].gameObject;
                        mapmanager.instance._Map[Block[1].x, Block[1].y] = Block[1].gameObject;
                        BlockState = State.C;
                    }
                }
                break;
            case State.C:
                if (mapmanager.instance._Map[Block[1].x + 1, Block[1].y + 1] == null)
                {
                    if (mapmanager.instance._Map[Block[3].x, Block[3].y + 2] == null)
                    {
                        if (mapmanager.instance._Map[Block[2].x + 1, Block[2].y + 1] == null)
                        {
                            mapmanager.instance._Map[Block[1].x, Block[1].y] = null;
                            mapmanager.instance._Map[Block[3].x, Block[3].y] = null;
                            mapmanager.instance._Map[Block[2].x, Block[2].y] = null;


                            Block[1].y += 1;
                            Block[1].x += 1;
                            Block[2].y += 1;
                            Block[2].x += 1;
                            Block[3].y += 2;

                            mapmanager.instance._Map[Block[1].x, Block[1].y] = Block[1].gameObject;
                            mapmanager.instance._Map[Block[3].x, Block[3].y] = Block[3].gameObject;
                            mapmanager.instance._Map[Block[2].x, Block[2].y] = Block[2].gameObject;
                            BlockState = State.D;
                        }
                    }
                }
                break;
            case State.D:
                if (mapmanager.instance._Map[Block[1].x, Block[1].y - 1] == null
                    || mapmanager.instance._Map[Block[1].x, Block[1].y - 1].CompareTag("tetrino"))
                {
                    if (mapmanager.instance._Map[Block[3].x - 1, Block[3].y + 1] == null
                        || mapmanager.instance._Map[Block[3].x - 1, Block[3].y + 1].CompareTag("tetrino"))
                    {
                        if (mapmanager.instance._Map[Block[2].x + 1, Block[2].y] == null
                            || mapmanager.instance._Map[Block[2].x + 1, Block[2].y].CompareTag("tetrino"))
                        {
                            if (mapmanager.instance._Map[Block[0].x, Block[0].y - 2] == null
                                || mapmanager.instance._Map[Block[0].x, Block[0].y - 2].CompareTag("tetrino"))
                            {
                                if (Block[0].x == 1)
                                    RightMove();
                                {
                                    mapmanager.instance._Map[Block[1].x, Block[1].y] = null;
                                    mapmanager.instance._Map[Block[3].x, Block[3].y] = null;
                                    mapmanager.instance._Map[Block[2].x, Block[2].y] = null;
                                    mapmanager.instance._Map[Block[0].x, Block[0].y] = null;


                                    Block[1].y -= 1;
                                    Block[0].y += 1;
                                    Block[0].x -= 1;
                                    Block[2].x += 1;
                                    Block[3].y -= 2;
                                    mapmanager.instance._Map[Block[1].x, Block[1].y] = Block[1].gameObject;
                                    mapmanager.instance._Map[Block[0].x, Block[0].y] = Block[0].gameObject;
                                    mapmanager.instance._Map[Block[3].x, Block[3].y] = Block[3].gameObject;
                                    mapmanager.instance._Map[Block[2].x, Block[2].y] = Block[2].gameObject;
                                    BlockState = State.A;
                                }
                            }
                        }
                    }
                }
                break;
        }
    }


    void RollBlockT()
    {
        switch (BlockState)
        {
            case State.A:
                if (mapmanager.instance._Map[Block[2].x, Block[2].y + 1] == null)
                {
                    if (mapmanager.instance._Map[Block[0].x + 1, Block[0].y + 2] == null)
                    {
                        mapmanager.instance._Map[Block[2].x, Block[2].y] = null;
                        mapmanager.instance._Map[Block[0].x, Block[0].y] = null;

                        Block[2].x += 0;
                        Block[2].y += 1;
                        Block[0].x += 1;
                        Block[0].y += 2;
                        mapmanager.instance._Map[Block[2].x, Block[2].y] = Block[2].gameObject;
                        mapmanager.instance._Map[Block[0].x, Block[0].y] = Block[0].gameObject;
                        BlockState = State.B;
                    }
                }
                break;
            case State.B:
                if (Block[0].x == 1)
                    RightMove();
                if (mapmanager.instance._Map[Block[0].x - 1, Block[0].y - 1] == null)
                {
                    mapmanager.instance._Map[Block[0].x, Block[0].y] = null;

                    Block[0].x -= 1;
                    Block[0].y -= 1;
                    mapmanager.instance._Map[Block[0].x, Block[0].y] = Block[0].gameObject;
                    BlockState = State.C;
                }

                break;
            case State.C:
                if (mapmanager.instance._Map[Block[2].x - 1, Block[2].y + 1] == null)
                {
                    mapmanager.instance._Map[Block[2].x, Block[2].y] = null;

                    Block[2].x -= 1;
                    Block[2].y += 1;
                    mapmanager.instance._Map[Block[2].x, Block[2].y] = Block[2].gameObject;
                    BlockState = State.D;
                }

                break;
            case State.D:
                if (Block[2].x == 10)
                    LeftMove();
                if (mapmanager.instance._Map[Block[2].x + 1, Block[2].y - 2] == null)
                {
                    if (mapmanager.instance._Map[Block[0].x, Block[0].y - 1] == null)
                    {
                        mapmanager.instance._Map[Block[2].x, Block[2].y] = null;
                        mapmanager.instance._Map[Block[0].x, Block[0].y] = null;

                        Block[2].x += 1;
                        Block[2].y -= 2;
                        Block[0].x -= 0;
                        Block[0].y -= 1;
                        mapmanager.instance._Map[Block[2].x, Block[2].y] = Block[2].gameObject;
                        mapmanager.instance._Map[Block[0].x, Block[0].y] = Block[0].gameObject;
                        BlockState = State.A;
                    }
                }
                break;
        }
    }

    void RollBlockZZ()
    {
        switch (BlockState)
        {
            case State.A:
                if (mapmanager.instance._Map[Block[0].x, Block[0].y + 1] == null)
                {
                    if (mapmanager.instance._Map[Block[3].x - 2, Block[3].y + 1] == null)
                    {

                        mapmanager.instance._Map[Block[0].x, Block[0].y] = null;
                        mapmanager.instance._Map[Block[3].x, Block[3].y] = null;


                        Block[0].y += 1;
                        Block[0].x += 0;
                        Block[3].y += 1;
                        Block[3].x -= 2;
                        mapmanager.instance._Map[Block[0].x, Block[0].y] = Block[0].gameObject;
                        mapmanager.instance._Map[Block[3].x, Block[3].y] = Block[3].gameObject;
                        BlockState = State.B;

                    }
                }
                break;
            case State.B:
                if (Block[3].x == 9)
                    LeftMove();
                if (mapmanager.instance._Map[Block[0].x, Block[0].y - 1] == null)
                {
                    if (mapmanager.instance._Map[Block[3].x + 2, Block[3].y - 1] == null)
                    {

                        mapmanager.instance._Map[Block[0].x, Block[0].y] = null;
                        mapmanager.instance._Map[Block[3].x, Block[3].y] = null;


                        Block[0].y -= 1;
                        Block[0].x -= 0;
                        Block[3].y -= 1;
                        Block[3].x += 2;
                        mapmanager.instance._Map[Block[0].x, Block[0].y] = Block[0].gameObject;
                        mapmanager.instance._Map[Block[3].x, Block[3].y] = Block[3].gameObject;
                        BlockState = State.A;

                    }
                }


                break;
        }
    }

    void RollBlockZ()
    {
        switch (BlockState)
        {
            case State.A:
                if (mapmanager.instance._Map[Block[0].x + 2, Block[0].y + 1] == null)
                {
                    if (mapmanager.instance._Map[Block[3].x, Block[3].y + 1] == null)
                    {

                        mapmanager.instance._Map[Block[0].x, Block[0].y] = null;
                        mapmanager.instance._Map[Block[3].x, Block[3].y] = null;


                        Block[0].y += 1;
                        Block[0].x += 2;
                        Block[3].y += 1;
                        Block[3].x += 0;
                        mapmanager.instance._Map[Block[0].x, Block[0].y] = Block[0].gameObject;
                        mapmanager.instance._Map[Block[3].x, Block[3].y] = Block[3].gameObject;
                        BlockState = State.B;

                    }
                }
                break;
            case State.B:
                if (Block[0].x == 2)
                    RightMove();
                if (mapmanager.instance._Map[Block[0].x - 2, Block[0].y - 1] == null)
                {
                    if (mapmanager.instance._Map[Block[3].x, Block[3].y - 1] == null)
                    {

                        mapmanager.instance._Map[Block[0].x, Block[0].y] = null;
                        mapmanager.instance._Map[Block[3].x, Block[3].y] = null;


                        Block[0].y -= 1;
                        Block[0].x -= 2;
                        Block[3].y -= 1;
                        Block[3].x -= 0;
                        mapmanager.instance._Map[Block[0].x, Block[0].y] = Block[0].gameObject;
                        mapmanager.instance._Map[Block[3].x, Block[3].y] = Block[3].gameObject;
                        BlockState = State.A;

                    }
                }


                break;
        }
    }
    #endregion

    void EndGame() // 게임종료
    {
        for (int j = 1; j < 21; j++)
        {
            for (int i = 1; i < 11; i++)
            {
                if (mapmanager.instance._Map[i, j] != null)
                {
                    mapmanager.instance._Map[i, j].GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.3f, 0.3f);
                }
            }
        }
        EndCanvas.SetActive(true);
        end = true;
    }

    public void LeftMove() // 왼쪽으로 블럭이동
    {
        if (cooltime == 0)
            return;
        if (MoveCondition(2))
        {
            for (int i = 0; i < 4; i++)
            {
                mapmanager.instance._Map[Block[i].x, Block[i].y] = null;
            }
            for (int i = 0; i < 4; i++)
            {
                mapmanager.instance._Map[Block[i].x - 1, Block[i].y] = Block[i].gameObject;
                Block[i].x--;
            }
            x--;
            Inputdelay = 0.3f;
            Inputtimer = 0;
        }
    }
    public void RightMove() // 오른쪽으로 블럭이동
    {
        if (cooltime == 0)
            return;
        if (MoveCondition(3))
        {
            for (int i = 0; i < 4; i++)
            {
                mapmanager.instance._Map[Block[i].x, Block[i].y] = null;
            }
            for (int i = 0; i < 4; i++)
            {
                mapmanager.instance._Map[Block[i].x + 1, Block[i].y] = Block[i].gameObject;
                Block[i].x++;
            }
            x++;
            Inputdelay = 0.3f;
            Inputtimer = 0;
        }
    }

    public void SpaceButton() // pc용 줄내리기
    {
        cooltime = 0;
    }

    public void UpButton() // pc용 블럭회전
    {
        switch (Blocktype)
        {
            case 1:
                RollBlockI();
                break;
            case 2:
                RollBlockIL();
                break;
            case 3:
                RollBlockILL();
                break;
            case 4:
                RollBlockT();
                break;
            case 5:

                break;
            case 6:
                RollBlockZZ();
                break;
            case 7:
                RollBlockZ();
                break;
        }
    }


    public void DownMove() // 한칸 내리기
    {
        if (MoveCondition(1))
        {
            for (int i = 0; i < 4; i++)
            {
                mapmanager.instance._Map[Block[i].x, Block[i].y] = null;
            }
            for (int i = 0; i < 4; i++)
            {
                mapmanager.instance._Map[Block[i].x, Block[i].y - 1] = Block[i].gameObject;
                Block[i].y--;
            }
            y--;
            Inputdelay = 0.3f;
            Inputtimer = 0;
        }
    }
}
