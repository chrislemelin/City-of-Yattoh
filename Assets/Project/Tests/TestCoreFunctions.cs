using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using Placeholdernamespace.Battle;
using Placeholdernamespace.Battle.Env;

namespace Placeholdernamespace.Tests
{

    public class TestCoreFunctions : TestCore
    {

        [UnityTest]
        public IEnumerator TestAttack()
        {
            yield return SetUp();
            Assert.NotNull(GameObject.FindObjectOfType<GameMaster>());
            GameObject.FindObjectOfType<ScenePropertyManager>().testingPlayer.BasicAttack.Action(
                GameObject.FindObjectOfType<TileManager>().GetTile(new Position(1, 0)));

        }

        [UnityTest]
        public IEnumerator TestMovement()
        {
            yield return SetUp();
            Assert.NotNull(GameObject.FindObjectOfType<GameMaster>());
            GameObject.FindObjectOfType<ScenePropertyManager>().testingPlayer.BasicAttack.Action(
                GameObject.FindObjectOfType<TileManager>().GetTile(new Position(1, 0)));

        }
    }
}

