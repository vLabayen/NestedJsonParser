using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
  void Start()
  {
    Test1();
    Test2();
    Test3();
    Test4();
    Test5();
    Test6();
    Test7();

  }

  void Test1(){
    string jsonString = "{\"success\":true,\"hoursSpent\":20,\"author\":\"vic\"}";
    TestObject1 expectedResult = new TestObject1(true, 20, "vic");

    TestObject1 obj = JsonParser.FromJson<TestObject1>(jsonString);
    if (expectedResult.Equals(obj)) Debug.Log("Reader Test 1 : Success");
    else Debug.LogWarning("Reader Test 1 : Failure");

    string json = JsonParser.ToJson<TestObject1>(obj);
    if (jsonString == json) Debug.Log("Writer Test 1 : Success");
    else Debug.LogWarning(String.Format("Writer Test 1 : Failure : {0}", json));
  }
  void Test2(){
    string jsonString = "{\"success\":[false,false,false,true],\"hoursSpent\":[5,10.2,15,2],\"author\":[\"vic\",\"vic\",\"vic\",\"vic\"]}";
    TestObject2 expectedResult = new TestObject2(new bool[] {false, false, false, true}, new float[] {5f, 10.2f, 15f, 2f}, new string[] {"vic","vic","vic","vic"});

    TestObject2 obj = JsonParser.FromJson<TestObject2>(jsonString);
    if (expectedResult.Equals(obj)) Debug.Log("Reader Test 2 : Success");
    else Debug.LogWarning("Reader Test 2 : Failure");

    string json = JsonParser.ToJson<TestObject2>(obj);
    if (jsonString == json) Debug.Log("Writer Test 2 : Success");
    else Debug.LogWarning(String.Format("Writer Test 2 : Failure : {0}", json));
  }
  void Test3(){
    string jsonString = "{\"success\":[false,false,false,true],\"hoursSpent\":[15,20],\"author\":[]}";
    TestObject3 expectedResult = new TestObject3(new bool[] {false, false, false, true}, new int[] {15, 20}, new string[] {});

    TestObject3 obj = JsonParser.FromJson<TestObject3>(jsonString);
    if (expectedResult.Equals(obj)) Debug.Log("Reader Test 3 : Success");
    else Debug.LogWarning("Reader Test 3 : Failure");

    string json = JsonParser.ToJson<TestObject3>(obj);
    if (jsonString == json) Debug.Log("Writer Test 3 : Success");
    else Debug.LogWarning(String.Format("Writer Test 3 : Failure : {0}", json));
  }
  void Test4(){
    string jsonString = "{\"nested\":{\"name\":\"vic\",\"id\":32}}";
    TestObject4 expectedResult = new TestObject4(new NestedObject("vic", 32));

    TestObject4 obj = JsonParser.FromJson<TestObject4>(jsonString);
    if (expectedResult.Equals(obj)) Debug.Log("Reader Test 4 : Success");
    else Debug.LogWarning("Reader Test 4 : Failure");

    string json = JsonParser.ToJson<TestObject4>(obj);
    if (jsonString == json) Debug.Log("Writer Test 4 : Success");
    else Debug.LogWarning(String.Format("Writer Test 4 : Failure : {0}", json));

  }
  void Test5(){
    string jsonString = "{\"nesteds\":[{\"name\":\"vic\",\"id\":32},{\"name\":\"yuan\",\"id\":97}]}";
    TestObject5 expectedResult = new TestObject5(new NestedObject[] {new NestedObject("vic", 32), new NestedObject("yuan", 97)});

    TestObject5 obj = JsonParser.FromJson<TestObject5>(jsonString);
    if (expectedResult.Equals(obj)) Debug.Log("Reader Test 5 : Success");
    else Debug.LogWarning("Reader Test 5 : Failure");

    string json = JsonParser.ToJson<TestObject5>(obj);
    if (jsonString == json) Debug.Log("Writer Test 5 : Success");
    else Debug.LogWarning(String.Format("Writer Test 5 : Failure : {0}", json));
  }
  void Test6(){
    string jsonString = "{\"nested\":{\"nested_arr\":[{\"name\":\"vic\",\"id\":0},{\"name\":\"yuan\",\"id\":1000}],\"nested_test\":{\"nesteds\":[{\"name\":\"vic\",\"id\":0},{\"name\":\"yuan\",\"id\":1000},{\"name\":\"pepe\",\"id\":-30}]}}}";
    TestObject6 expectedResult = new TestObject6(new NestedObject2(
      new NestedObject[] {new NestedObject("vic", 0), new NestedObject("yuan", 1000)},
      new TestObject5(new NestedObject[] {new NestedObject("vic", 0), new NestedObject("yuan", 1000), new NestedObject("pepe", -30)})
    ));

    TestObject6 obj = JsonParser.FromJson<TestObject6>(jsonString);
    if (expectedResult.Equals(obj)) Debug.Log("Reader Test 6 : Success");
    else Debug.LogWarning("Reader Test 6 : Failure");

    string json = JsonParser.ToJson<TestObject6>(obj);
    if (jsonString == json) Debug.Log("Writer Test 6 : Success");
    else Debug.LogWarning(String.Format("Writer Test 6 : Failure : {0}", json));

  }
  void Test7(){
    string jsonString = "{\"test_arr\":[{\"nested\":{\"nested_arr\":[{\"name\":\"vic\",\"id\":0},{\"name\":\"yuan\",\"id\":1000}],\"nested_test\":{\"nesteds\":[{\"name\":\"vic\",\"id\":0},{\"name\":\"yuan\",\"id\":1000},{\"name\":\"pepe\",\"id\":-30}]}}},{\"nested\":{\"nested_arr\":[{\"name\":\"vic\",\"id\":0},{\"name\":\"yuan\",\"id\":1000}],\"nested_test\":{\"nesteds\":[{\"name\":\"vic\",\"id\":0},{\"name\":\"yuan\",\"id\":1000},{\"name\":\"pepe\",\"id\":-30}]}}}]}";
    TestObject7 expectedResult = new TestObject7(new TestObject6[] {
      new TestObject6(new NestedObject2(
        new NestedObject[] {new NestedObject("vic", 0), new NestedObject("yuan", 1000)},
        new TestObject5(new NestedObject[] {new NestedObject("vic", 0), new NestedObject("yuan", 1000), new NestedObject("pepe", -30)})
      )),
      new TestObject6(new NestedObject2(
        new NestedObject[] {new NestedObject("vic", 0), new NestedObject("yuan", 1000)},
        new TestObject5(new NestedObject[] {new NestedObject("vic", 0), new NestedObject("yuan", 1000), new NestedObject("pepe", -30)})
      ))
    });

    TestObject7 obj = JsonParser.FromJson<TestObject7>(jsonString);
    if (expectedResult.Equals(obj)) Debug.Log("Reader Test 7 : Success");
    else Debug.LogWarning("Reader Test 7 : Failure");

    string json = JsonParser.ToJson<TestObject7>(obj);
    if (jsonString == json) Debug.Log("Writer Test 7 : Success");
    else Debug.LogWarning(String.Format("Writer Test 7 : Failure : {0}", json));
  }
}
