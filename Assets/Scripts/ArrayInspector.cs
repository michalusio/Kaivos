using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class RawImageMult : IEnumerable<RawImage>
{
    [SerializeField] public RawImage[] levelArray;
        
    public RawImage this[int key]
    {
        get
        {
            return levelArray[key];
        }
        set
        {
            levelArray[key] = value;
        }
    }

    public int Length => levelArray.Length;

    public IEnumerator<RawImage> GetEnumerator()
    {
        return ((IEnumerable<RawImage>)levelArray).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable<RawImage>)levelArray).GetEnumerator();
    }
}

[Serializable]
public class TextMult : IEnumerable<Text>
{
    [SerializeField] public Text[] levelArray;
        
    public Text this[int key]
    {
        get
        {
            return levelArray[key];
        }
        set
        {
            levelArray[key] = value;
        }
    }

    public int Length => levelArray.Length;

    public IEnumerator<Text> GetEnumerator()
    {
        return ((IEnumerable<Text>)levelArray).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable<Text>)levelArray).GetEnumerator();
    }
}