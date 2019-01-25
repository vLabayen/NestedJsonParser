using System.Collections;
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
    TestObject1 obj = JsonParser.FromJson<TestObject1>(jsonString);
    Debug.Log(obj.success);
    Debug.Log(obj.hoursSpent);
    Debug.Log(obj.author);
  }
  void Test2(){
    string jsonString = "{\"success\":[false,false,false,true],\"hoursSpent\":[5,10,15,20],\"author\":[\"vic\",\"vic\",\"vic\",\"vic\"]}";
    TestObject2 obj = JsonParser.FromJson<TestObject2>(jsonString);
    for(int i = 0; i < obj.success.Length; i++) Debug.Log(obj.success[i]);
    for(int i = 0; i < obj.hoursSpent.Length; i++) Debug.Log(obj.hoursSpent[i]);
    for(int i = 0; i < obj.author.Length; i++) Debug.Log(obj.author[i]);
  }
  void Test3(){
    string jsonString = "{\"success\":[false,false,false,true],\"hoursSpent\":[15,20],\"author\":[]}";
    TestObject3 obj = JsonParser.FromJson<TestObject3>(jsonString);
    for(int i = 0; i < obj.success.Length; i++) Debug.Log(obj.success[i]);
    for(int i = 0; i < obj.hoursSpent.Length; i++) Debug.Log(obj.hoursSpent[i]);
    for(int i = 0; i < obj.author.Length; i++) Debug.Log(obj.author[i]);
  }
  void Test4(){
    string jsonString = "{\"nested\":{\"name\":\"vic\",\"id\":32}}";
    TestObject4 obj = JsonParser.FromJson<TestObject4>(jsonString);
    Debug.Log(obj.nested.name);
    Debug.Log(obj.nested.id);
  }
  void Test5(){
    string jsonString = "{\"nesteds\":[{\"name\":\"vic\",\"id\":32},{\"name\":\"yuan\",\"id\":97}]}";
    TestObject5 obj = JsonParser.FromJson<TestObject5>(jsonString);
    for (int i = 0; i < obj.nesteds.Length; i++) {
      Debug.Log(obj.nesteds[i].name);
      Debug.Log(obj.nesteds[i].id);
    }
  }
  void Test6(){
    string jsonString = "{\"nested\":{\"nested_arr\":[{\"name\":\"vic\",\"id\":0},{\"name\":\"yuan\",\"id\":1000}],\"nested_test\":{\"nesteds\":[{\"name\":\"vic\",\"id\":0},{\"name\":\"yuan\",\"id\":1000},{\"name\":\"pepe\",\"id\":-30}]}}}";
    TestObject6 obj = JsonParser.FromJson<TestObject6>(jsonString);
    for (int i = 0; i < obj.nested.nested_arr.Length; i++) {
      Debug.Log(obj.nested.nested_arr[i].name);
      Debug.Log(obj.nested.nested_arr[i].id);
    }
    for (int i = 0; i < obj.nested.nested_test.nesteds.Length; i++){
      Debug.Log(obj.nested.nested_test.nesteds[i].name);
      Debug.Log(obj.nested.nested_test.nesteds[i].id);
    }
  }
  void Test7(){
    string jsonString = "{\"test_arr\":[{\"nested\":{\"nested_arr\":[{\"name\":\"vic\",\"id\":0},{\"name\":\"yuan\",\"id\":1000}],\"nested_test\":{\"nesteds\":[{\"name\":\"vic\",\"id\":0},{\"name\":\"yuan\",\"id\":1000},{\"name\":\"pepe\",\"id\":-30}]}}},{\"nested\":{\"nested_arr\":[{\"name\":\"vic\",\"id\":0},{\"name\":\"yuan\",\"id\":1000}],\"nested_test\":{\"nesteds\":[{\"name\":\"vic\",\"id\":0},{\"name\":\"yuan\",\"id\":1000},{\"name\":\"pepe\",\"id\":-30}]}}}]}";

    TestObject7 obj = JsonParser.FromJson<TestObject7>(jsonString);
    for (int j = 0; j < obj.test_arr.Length; j++){
      for (int i = 0; i < obj.test_arr[j].nested.nested_arr.Length; i++) {
        Debug.Log(obj.test_arr[j].nested.nested_arr[i].name);
        Debug.Log(obj.test_arr[j].nested.nested_arr[i].id);
      }
      for (int i = 0; i < obj.test_arr[j].nested.nested_test.nesteds.Length; i++){
        Debug.Log(obj.test_arr[j].nested.nested_test.nesteds[i].name);
        Debug.Log(obj.test_arr[j].nested.nested_test.nesteds[i].id);
      }
    }
  }
}
