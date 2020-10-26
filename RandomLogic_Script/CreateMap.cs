using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LitJson;

namespace Logic
{
    public class CreateMap : MonoBehaviour
    {
        string[] maps;
        int size = 15;

        public void CreateStart()
        {
            maps = DataBase.instance.maps;
            if (!GameManager.instance.LoadMaps())
            {
                CreateMapdata();
                GameManager.instance.SaveMaps();
            }
        }

        public void CreateMapdata()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    maps[i + (j * size) + 0] = "00";

                    float temp = Random.Range(0, 100);
                    if (temp < 37)
                        maps[i + (j * size) + 0] = "01";
                }
            }

            Fill_Map();
        }

        void Fill_Map()
        {
            for (int i = 0; i < 10; i++)
            {
                Check_Map();
            }
            Delete_Map();
        }

        void Check_Map()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Check_Eight(i, j);
                }
            }
        }

        void Check_Eight(int i, int j)
        {
            if (maps[i + (j * size) + 0] != "00")
                return;

            int count = 0;
            if (i < size - 1 && j < size - 1)
                if (maps[(i + 1) + ((j + 1) * size) + 0] != "00")
                    count++;

            if (i < size - 1)
                if (maps[(i + 1) + (j * size) + 0] != "00")
                    count++;

            if (i < size - 1 && j > 0)
                if (maps[(i + 1) + ((j - 1) * size) + 0] != "00")
                    count++;

            if (j < size - 1)
                if (maps[i + ((j + 1) * size) + 0] != "00")
                    count++;

            if (j > 0)
                if (maps[i + ((j - 1) * size) + 0] != "00")
                    count++;

            if (i > 0 && j < size - 1)
                if (maps[(i - 1) + ((j + 1) * size) + 0] != "00")
                    count++;

            if (i > 0)
                if (maps[(i - 1) + (j * size) + 0] != "00")
                    count++;

            if (i > 0 && j > 0)
                if (maps[(i - 1) + ((j - 1) * size) + 0] != "00")
                    count++;

            if (count > 4)
            {
                maps[i + (j * size) + 0] = "01";
            }
        }

        void Delete_Map()
        {
            for (int i = 0; i < 10; i++)
            {
                Check_Delete();
            }
        }

        void Check_Delete()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Check_Four(i, j);
                }
            }
        }
        void Check_Four(int i, int j)
        {
            if (maps[i + (j * size) + 0] == null)
                return;

            int count = 0;

            if (i < size - 1)
                if (maps[(i + 1) + (j * size) + 0] == "00")
                    count++;

            if (j < size - 1)
                if (maps[i + ((j + 1) * size) + 0] == "00")
                    count++;

            if (i > 0)
                if (maps[(i - 1) + (j * size) + 0] == "00")
                    count++;

            if (j > 0)
                if (maps[i + ((j - 1) * size) + 0] == "00")
                    count++;

            if (count > 2)
            {
                maps[i + (j * size) + 0] = "00";
            }
        }
    }
}