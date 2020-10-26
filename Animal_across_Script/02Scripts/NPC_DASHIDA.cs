using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Reflection;

[Serializable]
public class ActObject
{
    public string className;
    public string methodName;
}

public class NPC_DASHIDA : MonoBehaviour
{
    public Queue<Action> actQueue = new Queue<Action>();
    public List<ActObject> actobject = new List<ActObject>();

    private void Start()
    {
        LoadObject();
        ResetQueue();
    }

    public void RunAction()
    {
        if (actQueue.Count <= 0)
            return;

        actQueue.Dequeue()();
        actobject.RemoveAt(0);
        JsonData questData = JsonMapper.ToJson(actobject);
        File.WriteAllText(Application.persistentDataPath + "/actqueue.json", questData.ToString());
    }

    public void AddObject(string className, string questName)
    {
        for (int i = 0; i < actobject.Count; i++)
            if (actobject[i].className == className && actobject[i].methodName == questName)
                return;

        ActObject aobj = new ActObject();
        aobj.className = className;
        aobj.methodName = questName;

        actobject.Add(aobj);
        JsonData questData = JsonMapper.ToJson(actobject);
        File.WriteAllText(Application.persistentDataPath + "/actqueue.json", questData.ToString());

        ResetQueue();
    }

    void LoadObject()
    {
        if (!File.Exists(Application.persistentDataPath + "/actqueue.json"))
            return;
        string json = File.ReadAllText(Application.persistentDataPath + "/actqueue.json");
        actobject = JsonMapper.ToObject<List<ActObject>>(json);
    }

    void ResetQueue()
    {
        actQueue.Clear();
        foreach(ActObject ao in actobject)
            EnqueueAction(ao);
    }

    void EnqueueAction(ActObject aobj)
    {
        Type type = Type.GetType(aobj.className);
        object obj = FindObjectOfType(type);
        MethodInfo method = type.GetMethod(aobj.methodName);

        actQueue.Enqueue(() => method.Invoke(obj, null));
    }

}