public class TestObject1{
  public bool success { get; set; }
  public int hoursSpent { get; set; }
  public string author { get; set; }
}
public class TestObject2{
  public bool[] success { get; set; }
  public int[] hoursSpent { get; set; }
  public string[] author { get; set; }
}
public class TestObject3{
  public bool[] success { get; set; }
  public int[] hoursSpent { get; set; }
  public string[] author { get; set; }
}
public class TestObject4{
  public NestedObject nested { get; set; }
}
public class TestObject5{
  public NestedObject[] nesteds { get; set; }
}
public class TestObject6{
  public NestedObject2 nested { get; set; }
}
public class TestObject7{
  public TestObject6[] test_arr { get; set; }
}

public class NestedObject {
  public string name { get; set; }
  public int id { get; set; }
}
public class NestedObject2 {
  public NestedObject[] nested_arr { get; set; }
  public TestObject5 nested_test { get; set; }
}
