//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System.IO;
//using System;
//using ExtensionMethods;

//public class TestScript : MonoBehaviour
//{

//    List<List<string>> waveComp = new List<List<string>>();

//    // Start is called before the first frame update
//    void Start()
//    {
//        StreamReader reader = new StreamReader(File.OpenRead(@"C:\Users\Kenneth\Dropbox\One Month Challenge\Season 1\SpawnTest.csv"));
//        // List<string> listA = new List<string>();

        

//        while (reader.EndOfStream == false)
//        {


//            waveComp.Add(new List<string> { reader.ReadLine() });
            
//        }
//    }

//    // Update is called once per frame
//    void Update()
//    {

        
//        Debug.Log(waveComp.Shuffle());
//    }

//}


//namespace ExtensionMethods
//{
//    public static class IntExtensions
//    {

//        private static System.Random rng = new System.Random();

//        public static void Shuffle<T>(this IList<T> list)
//        {
//            int n = list.Count;
//            while (n > 1)
//            {
//                n--;
//                int k = rng.Next(n + 1);
//                T value = list[k];
//                list[k] = list[n];
//                list[n] = value;
//            }


//        }

//    }

//}
