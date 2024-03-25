using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Reflection;

public class Test : MonoBehaviour
{
    public void Awake()
    {
        string location = Assembly.GetExecutingAssembly().Location;
        string text = Path.GetDirectoryName(location) + "\\temp";
        string text2 = Path.Combine(location, text);

        // create temp if not there already
        if (!Directory.Exists(text2))
        {
            Directory.CreateDirectory(text2);
        }

        string[] files = Directory.GetFiles(text2, "*.dll");
        string[] array = files;
        for (int i = 0; i < array.Length; i++)
        {
            Assembly.LoadFile(array[i]);
            Debug.Log(array[i]);
        }
        using (StreamWriter writer = File.AppendText(Path.Combine(text2, "Log.txt")))
        {
            writer.WriteLine("butt");
        }
    }
}
