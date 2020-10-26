using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class mapmanager : MonoBehaviour
{
    static mapmanager ss = null;
    public static mapmanager instance
    {
        get
        {
            return ss;
        }
    }

    #region 변수선언부분
    GameObject WallTile;
    GameObject BackTile;
    public GameObject MainCam;
    public GameObject Background;
    GameObject[,] MapLocation = new GameObject[12, 26];
    public GameObject[,] _Map
    {
        get
        {
            return MapLocation;
        }
        set
        {
            MapLocation = value;
        }
    }

    [SerializeField] int[] shadowX = new int[4];
    [SerializeField] int[] shadowY = new int[4];
    [SerializeField] List<GameObject> shodowList = new List<GameObject>(4);

    Blockmanager BM;
    #endregion



    private void Awake()
    {
        ss = this;

        WallTile = Resources.Load("02prefab/tile001", typeof(GameObject)) as GameObject;
        BackTile = Resources.Load("02prefab/tile002", typeof(GameObject)) as GameObject;
        BM = FindObjectOfType<Blockmanager>();

        SetDefault(); // 맵 기본상태로 설정

        SetCameraPos(); // 카메라를 맵이 전부 보이게 변경
    }


    void Update()
    {
        PrintPreview(); // 아래쪽그림자호출
        PrintMap(); // 맵 변경사항 호출
    }

    bool CheckObjectInCamera(GameObject go) // 오브젝트가 카메라 안에 있는지 확인
    {
        Camera selectedCamera = Camera.main;
        Vector3 screenPoint = selectedCamera.WorldToViewportPoint(go.transform.position);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        return onScreen;
    }

    void SetCameraPos() // 카메라 위치를 조정
    {
        Vector3 aPos = instance._Map[0, 0].transform.position;
        Vector3 bPos = instance._Map[11, 20].transform.position;
        Vector3 Pos = (aPos + bPos) / 2 + new Vector3(0, 0, -10);
        MainCam.transform.position = Pos;
        Background.transform.position = Pos + new Vector3(0, 0, 10);

        if (!CheckObjectInCamera(instance._Map[0, 0]) || !CheckObjectInCamera(instance._Map[11, 20]))
        {
            while (!CheckObjectInCamera(instance._Map[0, 0]) || !CheckObjectInCamera(instance._Map[11, 20]))
            {
                Camera.main.orthographicSize += 0.1f;
            }
            Camera.main.orthographicSize += 1.0f;
        }
    }

    void SetDefault() // 기본맵을 깔아줌
    {
        for (int i = 0; i < 12; i++)
        {
            _Map[i, 0] = Instantiate(WallTile, transform);
        }
        for (int i = 1; i < 21; i++)
        {
            _Map[0, i] = Instantiate(WallTile, transform);
        }
        for (int i = 1; i < 21; i++)
        {
            _Map[11, i] = Instantiate(WallTile, transform);
        }
        for (int i = 1; i < 11; i++)
        {
            for (int j = 1; j < 21; j++)
            {
                _Map[i, j] = Instantiate(BackTile, transform);
            }
        }

        PrintMap();

        for (int i = 1; i < 11; i++)
        {
            for (int j = 1; j < 21; j++)
            {
                _Map[i, j] = null;
            }
        }
    }

    public void PrintMap() // 오브젝트를 위치시켜서 보여줌
    {
        for (int j = 0; j < 26; j++)
        {
            for (int i = 0; i < 12; i++)
            {
                if (_Map[i, j] != null)
                {
                    _Map[i, j].transform.position = new Vector3(-1.3f + 0.32f * i, -2.0f + 0.32f * j, 0);
                }
            }
        }
    }

    public void PrintPreview() // 아래쪽 그림자 위치를 정함
    {
        if (FindObjectOfType<Blockmanager>().end)
            return;

        for (int i = 0; i < 4; i++)
        {
            if (BM._Block[i].tag == "tetrino")
            {
                shadowX[i] = BM._Block[i].GetComponent<Tetrino>().x;
                shadowY[i] = BM._Block[i].GetComponent<Tetrino>().y;
            }
        }

        for (int j = 0; 20 > j; j++)
        {
            if (shadowY[0] - j > 0 && CheckShadow(shadowX[0], shadowY[0] - j))
            {
                if (shadowY[1] - j > 0 && CheckShadow(shadowX[1], shadowY[1] - j))
                {
                    if (shadowY[2] - j > 0 && CheckShadow(shadowX[2], shadowY[2] - j))
                    {
                        if (shadowY[3] - j > 0 && CheckShadow(shadowX[3], shadowY[3] - j))
                        {

                        }
                        else
                        {
                            DrawPreview(j);
                            return;
                        }
                    }
                    else
                    {
                        DrawPreview(j);
                        return;
                    }
                }
                else
                {
                    DrawPreview(j);
                    return;
                }
            }
            else
            {
                DrawPreview(j);
                return;
            }
        }

    }

    void DrawPreview(int j) // 그림자를 그려줌
    {
        for (int i = 0; i < 4; i++)
        {
            if (BM._Block[i].tag == "tetrino")
            {
                shadowX[i] = BM._Block[i].GetComponent<Tetrino>().x;
                shadowY[i] = BM._Block[i].GetComponent<Tetrino>().y;
            }
        }
        for (int i = 0; i < 4; i++)
        {
            if (_Map[shadowX[i], shadowY[i] - j] == null)
            {
                GameObject temp = Resources.Load("02prefab/tetrino", typeof(GameObject)) as GameObject;
                _Map[shadowX[i], shadowY[i] - j] = Instantiate<GameObject>(temp);
                _Map[shadowX[i], shadowY[i] - j].GetComponent<Tetrino>().x = shadowX[i];
                _Map[shadowX[i], shadowY[i] - j].GetComponent<Tetrino>().y = shadowY[i] - j;
                Color color = _Map[shadowX[i], shadowY[i] - j].GetComponent<SpriteRenderer>().color;
                color.a = 0.2f;
                _Map[shadowX[i], shadowY[i] - j].GetComponent<SpriteRenderer>().color = color;
                shodowList.Add(_Map[shadowX[i], shadowY[i] - j]);
            }
            if (shodowList.Count > 4)
            {
                for (int k = 0; k <= shodowList.Count - 4; k++)
                {
                    Destroy(shodowList[k]);

                    shodowList.RemoveAt(k);
                }
            }
        }

    }

    bool CheckShadow(int x, int y) // 그림자가 놓여질수 있는지 체크
    {
        if (_Map[x, y - 1] == null)
            return true;
        else if (_Map[x, y - 1] != null)
        {
            if (_Map[x, y - 1].CompareTag("tetrino"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    public void EndGame() // 게임종료
    {
        SceneManager.LoadScene("lobby");
    }


}
