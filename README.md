# Express Mapper

Copies values between two type instances that shares the same 
Copies values between between two object instances that do share the same interface.
And, if there is no common interface, it copies values between source and target have for same properties - 
property name and type must be equal.

It is using reflection. To improve performance, type properties properties are cached after first round. 
The property info info is read from cache and to not call reflection a second time for same property or field.

#### Example 1:

```C#
public class Foo : IModel { ... }

public class Baar : IModel { ... }

InterfacePropertiesClone.CopyValues<Foo, Baar, IModel>(source, target);
```

#### Example 2:

```C#
public class Foo { ... }

public class Baar : IModel { ... }

InterfacePropertiesClone.CopyValues<Foo, Baar, IModel>(source, target);
```

The target instance gets populated with same values for all properties from IModel.  
Reference Type properties and Value Type properties are supported by boxing.

## Limitations
- In opposite to other mappers like AutoMapper, it's not configurable.
- not tested with record types

# Build and Test
Run all test methods. Test methods covers almost 100% of the possible cases

# Contribute
Create your branch dev/<your name>, enhance the code in your branch and create a pull request.

### The pull request and approval chain is: 
- individual branch ==> qa (pre-release nuget) ==> master (release nuget)

