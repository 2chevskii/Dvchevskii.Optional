# Dvchevskii.Option

Provides utility type `Option<T>` for optional value handling

## Installation

### Package manager console

`Install-Package Dvchevskii.Optional`

### .NET CLI

`dotnet add package Dvchevskii.Optional`

## Usage

### Create an `Option<T>` from `T value`

```cs

Option<int> option = Option.Some(42);
Assert.IsTrue(option.IsSome());

```

### Create an empty `Option<T>`

```cs

Option<int> option = Option.None<int>();
Assert.IsTrue(option.IsNone());

```

### Mapping

```cs

string MapOption(Option<int> option)
{
  return option.MapOrElse(
    value => $"I have value: {value}",
    () => "I have no value =("
  );
}

Assert.AreEqual(
  MapOption(Option.Some(42)),
  "I have value: 42"
);

Assert.AreEqual(
  MapOption(Option.None<int>()),
  "I have no value =("
);

```

### Pattern matching

```cs

int PatternMatchOption<T>(Option<T> option) =>
  option switch {
    ISome => 42,
    INone => 69
  };

bool IsOptionSome<T>(Option<T> option) => option is ISome;

```
