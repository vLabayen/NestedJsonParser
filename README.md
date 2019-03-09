# NestedJsonParser
2 classes that allow to cast a json to any provided generic type.
The parser and the reader are not Unity dependent while the tests are.
The parser is implemented using reflection. Both are recusive aproaches

To import it, just download JsonReader & JsonParser to your proyect folder
Usage example in Test and TestObjects class.


One usage example (extracted from the tests) : 

Given the following structure :

public class TestObject7{
  public TestObject6[] test_arr { get; set; }
}
public class TestObject6{
  public NestedObject2 nested { get; set; }
}
public class NestedObject2 {
  public NestedObject[] nested_arr { get; set; }
  public TestObject5 nested_test { get; set; }
}
public class TestObject5{
  public NestedObject[] nesteds { get; set; }
}
public class NestedObject {
  public string name { get; set; }
  public int id { get; set; }
}

And given a json string like :
{"test_arr":[{
  nested:{
    nested_arr:[{
      name:vic,
      id:0
    },{
      name:yuan,
      id:1000
    }],
    nested_test:{
      nesteds:[{
        name:vic,
        id:0
      },{
        name:yuan,
        id:1000
      },{
        name:pepe,
        id:-30
      }]
    }
  }
},{
  nested:{
    nested_arr:[{
      name:vic,
      id:0
    },{
      name:yuan,
      id:1000
    }],
    nested_test:{
      nesteds:[{
        name:vic,
        id:0
      },{
        name:yuan,
        id:1000
      },{
        name:pepe,
        id:-30
      }]
    }
  }
}]}"

We can parse it to a TestObject7 instance like :
TestObject7 obj = JsonParser.FromJson<TestObject7>(jsonString);

More examples can be found in the test class. They are Unity dependent to run, but they can easily parsed to a console application
