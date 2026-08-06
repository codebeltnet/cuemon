using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Cuemon.Reflection;

namespace Cuemon.Extensions.Xml.Assets;
public class HierarchyExample
{
    public HierarchyExample()
    {
        Id = Guid.Empty;
    }

    [XmlAttribute]
    public Guid Id { get; }

    public IEnumerable<Animal> Animals
    {
        get
        {
            yield return new Dog();
            yield return new Cat();
            yield return new Pig();
        }
    }

    public Person Owner => new Person(
        new Address()
        {
            City = "Gilleleje",
            PostalCode = "3250"
        })
    {
        Age = 42,
        Name = "Gimlichael"
    };

    public NugetPackage Cuemon => new NugetPackage()
    {
        Name = "Cuemon for .NET",
        Version = new VersionResult("6.0.0"),
        Tags = "* 42 Infinity"
    };
}

public abstract class Animal
{
    [XmlAttribute]
    public abstract string Output { get; }
}

public class Dog : Animal
{
    public override string Output => "Vooooof";
}

public class Cat : Animal
{
    public override string Output => "Mioauw";
}

public class Pig : Animal
{
    public override string Output => "Oink";
}

public class Person
{
    public Person(Address address)
    {
        Address = address;
    }

    public string Name { get; set; }

    [XmlAttribute]
    public int Age { get; set; }

    public Address Address { get; }
}

public class Address
{
    public string City { get; set; }

    public string PostalCode { get; set; }
}

public class NugetPackage
{
    [XmlAttribute]
    public string Name { get; set; }

    [XmlAttribute]
    public string Tags { get; set; }

    public VersionResult Version { get; set; }
}
