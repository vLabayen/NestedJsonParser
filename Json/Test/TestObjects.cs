using System;
using System.Collections.Generic;
using UnityEngine;

public class TestObject1{
  public TestObject1(){}
  public TestObject1(bool success, int hoursSpent, string author){
    this.success = success;
    this.hoursSpent = hoursSpent;
    this.author = author;
  }

  public bool success { get; set; }
  public int hoursSpent { get; set; }
  public string author { get; set; }

  public bool Equals(TestObject1 other){
    if (this.success != other.success){
      Debug.LogWarning("TestObject1 : Field success does not match. Type bool");
      return false;
    }
    if (this.hoursSpent != other.hoursSpent){
      Debug.LogWarning("TestObject1 : Field hoursSpent does not match. Type int");
      return false;
    }
    if (this.author != other.author){
      Debug.LogWarning("TestObject1 : Field author does not match. Type string");
      return false;
    }
    return true;
  }
}
public class TestObject2{
  public TestObject2(){}
  public TestObject2(bool[] success, float[] hoursSpent, string[] author){
    this.success = success;
    this.hoursSpent = hoursSpent;
    this.author = author;
  }

  public bool[] success { get; set; }
  public float[] hoursSpent { get; set; }
  public string[] author { get; set; }

  public bool Equals(TestObject2 other){
    if (this.success.Length != other.success.Length) {
      Debug.LogWarning("TestObject2 : Lengths of success does not match. Type bool[]");
      return false;
    }
    if (this.hoursSpent.Length != other.hoursSpent.Length) {
      Debug.LogWarning("TestObject2 : Lengths of hoursSpent does not match. Type float[]");
      return false;
    }
    if (this.author.Length != other.author.Length) {
      Debug.LogWarning("TestObject2 : Lengths of author does not match. Type string[]");
      return false;
    }
    for(int i = 0; i < this.success.Length; i++) if (this.success[i] != other.success[i]) {
      Debug.LogWarning(String.Format("TestObject2 : Field success[{0}] does not match. Type bool[]", i));
      return false;
    }
    for(int i = 0; i < this.hoursSpent.Length; i++) if (this.hoursSpent[i] != other.hoursSpent[i]) {
      Debug.LogWarning(String.Format("TestObject2 : Field hoursSpent[{0}] does not match. Type float[]", i));
      return false;
    }
    for(int i = 0; i < this.author.Length; i++) if (this.author[i] != other.author[i]) {
      Debug.LogWarning(String.Format("TestObject2 : Field author[{0}] does not match. Type string[]", i));
      return false;
    }
    return true;
  }
}
public class TestObject3{
  public TestObject3(){}
  public TestObject3(bool[] success, int[] hoursSpent, string[] author){
    this.success = success;
    this.hoursSpent = hoursSpent;
    this.author = author;
  }

  public bool[] success { get; set; }
  public int[] hoursSpent { get; set; }
  public string[] author { get; set; }

  public bool Equals(TestObject3 other){
    if (this.success.Length != other.success.Length) {
      Debug.LogWarning("TestObject3 : Lengths of success does not match. Type bool[]");
      return false;
    }
    if (this.hoursSpent.Length != other.hoursSpent.Length) {
      Debug.LogWarning("TestObject3 : Lengths of hoursSpent does not match. Type int[]");
      return false;
    }
    if (this.author.Length != other.author.Length) {
      Debug.LogWarning("TestObject3 : Lengths of author does not match. Type string[]");
      return false;
    }
    for(int i = 0; i < this.success.Length; i++) if (this.success[i] != other.success[i]) {
      Debug.LogWarning(String.Format("TestObject3 : Field success[{0}] does not match. Type bool[]", i));
      return false;
    }
    for(int i = 0; i < this.hoursSpent.Length; i++) if (this.hoursSpent[i] != other.hoursSpent[i]) {
      Debug.LogWarning(String.Format("TestObject3 : Field hoursSpent[{0}] does not match. Type int[]", i));
      return false;
    }
    for(int i = 0; i < this.author.Length; i++) if (this.author[i] != other.author[i]) {
      Debug.LogWarning(String.Format("TestObject3 : Field author[{0}] does not match. Type string[]", i));
      return false;
    }
    return true;
  }
}
public class TestObject4{
  public TestObject4(){}
  public TestObject4(NestedObject nested){
    this.nested = nested;
  }

  public NestedObject nested { get; set; }

  public bool Equals(TestObject4 other){
    if (!this.nested.Equals(other.nested)){
      Debug.LogWarning("TestObject4 : Field nested does not match. Type NestedObject");
      return false;
    }
    return true;
  }
}
public class TestObject5{
  public TestObject5(){}
  public TestObject5(NestedObject[] nesteds){
    this.nesteds = nesteds;
  }

  public NestedObject[] nesteds { get; set; }

  public bool Equals(TestObject5 other){
    if (this.nesteds.Length != other.nesteds.Length) {
      Debug.LogWarning("TestObject5 : Lengths of nesteds does not match. Type NestedObject[]");
      return false;
    }
    for (int i = 0; i < this.nesteds.Length; i++) if (!this.nesteds[i].Equals(other.nesteds[i])) {
      Debug.LogWarning(String.Format("TestObject5 : Field nesteds[{0}] does not match. Type NestedObject[]", i));
      return false;
    }
    return true;
  }
}
public class TestObject6{
  public TestObject6(){}
  public TestObject6(NestedObject2 nested){
    this.nested = nested;
  }

  public NestedObject2 nested { get; set; }

  public bool Equals(TestObject6 other){
    if (!this.nested.Equals(other.nested)){
      Debug.LogWarning("TestObject6 : Field nested does not match. Type NestedObject2");
      return false;
    }
    return true;
  }
}
public class TestObject7{
  public TestObject7(){}
  public TestObject7(TestObject6[] test_arr){
    this.test_arr = test_arr;
  }

  public TestObject6[] test_arr { get; set; }

  public bool Equals(TestObject7 other){
    if (this.test_arr.Length != other.test_arr.Length) {
      Debug.LogWarning("TestObject7 : Lengths of test_arr does not match. Type TestObject6[]");
      return false;
    }
    for (int i = 0; i < this.test_arr.Length; i++) if (!this.test_arr[i].Equals(other.test_arr[i])) {
      Debug.LogWarning(String.Format("TestObject7 : Field test_arr[{0}] does not match. Type TestObject6[]", i));
      return false;
    }
    return true;
  }
}

public class NestedObject {
  public NestedObject(){}
  public NestedObject(string name, int id){
    this.name = name;
    this.id = id;
  }

  public string name { get; set; }
  public int id { get; set; }

  public bool Equals(NestedObject other){
    if (this.name != other.name){
      Debug.LogWarning("NestedObject : Field name does not match. Type string");
      return false;
    }
    if (this.id != other.id){
      Debug.LogWarning("NestedObject : Field id does not match. Type int");
      return false;
    }
    return true;
  }
}
public class NestedObject2 {
  public NestedObject2(){}
  public NestedObject2(NestedObject[] nested_arr, TestObject5 nested_test){
    this.nested_arr = nested_arr;
    this.nested_test = nested_test;
  }

  public NestedObject[] nested_arr { get; set; }
  public TestObject5 nested_test { get; set; }

  public bool Equals(NestedObject2 other){
    if (this.nested_arr.Length != other.nested_arr.Length) {
      Debug.LogWarning("NestedObject2 : Lengths of nested_arr does not match. Type NestedObject[]");
      return false;
    }
    for (int i = 0; i < this.nested_arr.Length; i++) if (!this.nested_arr[i].Equals(other.nested_arr[i])) {
      Debug.LogWarning(String.Format("NestedObject2 : Field nested_arr[{0}] does not match. Type NestedObject[]", i));
      return false;
    };
    if (!this.nested_test.Equals(other.nested_test)) {
      Debug.LogWarning("NestedObject2 : Field nested_test does not match. Type TestObject5");
      return false;
    };
    return true;
  }
}
