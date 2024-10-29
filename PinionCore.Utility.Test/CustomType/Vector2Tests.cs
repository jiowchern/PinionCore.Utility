namespace PinionCore.Utility.Tests
{

    public class Vector2Tests
    {
        [NUnit.Framework.Test]
        public void VectorToAngleTest()
        {
            var vec = Vector2.AngleToVector(45.0f);
            var angle = Vector2.VectorToAngle(vec);
            NUnit.Framework.Assert.AreEqual(45.0f, angle);
        }
    }
}
