using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Networking.UnityWebRequest;

namespace SoftwareKingdom.UnityTools
{
    public class ReadFromColumn : MonoBehaviour
    {
        //Settings

        public string textFilePath;
        // Connections

        [TextArea(10, 10)]
        public string inputText;
        public float[] floatArray;
        public int[] intArray;
        // State Variables dafasdf


        [ContextMenu("Get Float Values")]
        void GetFloatValues()
        {
            Debug.Log("Test");
            floatArray = GetFloatValuesToArray();
        }

        [ContextMenu("Get Integer Values")]
        void GetIntValues()
        {
            Debug.Log("Test");
            intArray = GetIntValuesToArray();
        }



        public float[] GetFloatValuesToArray()
        {
            string[] lines = inputText.Split(new[] { '\n' });
            List<float> outList = new List<float>();
            for (int i = 0; i < lines.Length; i++)
            {
                outList.Add(Convert.ToSingle(lines[i]));
            }
            return outList.ToArray();
        }

        public int[] GetIntValuesToArray()
        {
            string[] lines = inputText.Split(new[] { '\n' });
            List<int> outList = new List<int>();
            for (int i = 0; i < lines.Length; i++)
            {
                outList.Add(Convert.ToInt32(lines[i]));
            }
            return outList.ToArray();
        }


        public float[] GetArray(string filePath)
        {
            List<float> outList = new List<float>();

            StreamReader reader = new StreamReader(filePath);


            string currentLine = reader.ReadLine();
            while (currentLine != null)
            {
                float value = Convert.ToSingle(currentLine);
                outList.Add(value);
                currentLine = reader.ReadLine();
            }

            reader.Dispose();

            return outList.ToArray();
        }

    }

}


