using System.Reflection;
using NUnit.Framework;
using UnityEngine;

public class CharacterTests
{
    [Test]
    public void Update_BeforeOnInit_DoesNotThrow()
    {
        var gameObject = new GameObject("CharacterTest");
        try
        {
            var character = gameObject.AddComponent<Character>();
            var awakeMethod = typeof(Character).GetMethod("Awake", BindingFlags.Instance | BindingFlags.NonPublic);
            var updateMethod = typeof(Character).GetMethod("Update", BindingFlags.Instance | BindingFlags.NonPublic);

            awakeMethod?.Invoke(character, null);

            Assert.DoesNotThrow(() => updateMethod!.Invoke(character, null));
        }
        finally
        {
            Object.DestroyImmediate(gameObject);
        }
    }
}
