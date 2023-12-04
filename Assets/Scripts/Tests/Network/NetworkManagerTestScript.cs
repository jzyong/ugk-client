using System.Collections;
using Network;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.Network
{
    public class NetworkManagerTestScript
    {

        [SetUp]
        public void Init()
        {
            Debug.Log("启动网络测试");
        }
        // A Test behaves as an ordinary method
        [Test]
        public void ConnectServer()
        {
            // Use the Assert class to test conditions
            NetworkManager.Instance.Start();
            bool connect = NetworkManager.Instance.isNetworkActive;
            Assert.AreEqual(true,connect);
            Assert.That(true==connect,"服务器连接失败");
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator NetworkManagerTestScriptWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            // Use the Assert class to test conditions
            NetworkManager.Instance.Start();
            bool connect = NetworkManager.Instance.isNetworkActive;
            Assert.AreEqual(true,connect);
            Assert.That(true==connect,"服务器连接失败");
            yield return null;
        }

        [TearDown]
        public void Destroy()
        {
            Debug.Log("网络测试结束");
        }
    }
}
