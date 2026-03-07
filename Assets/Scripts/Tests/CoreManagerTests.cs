using NUnit.Framework;
using UnityEngine;

public class CoreManagerTests
{
    [Test]
    public void TestManagersInitialization()
    {
        Assert.IsNotNull(GameManager.Instance);
        Assert.IsNotNull(PoolManager.Instance);
        Assert.IsNotNull(SaveManager.Instance);
        Assert.IsNotNull(UIManager.Instance);
    }
}
