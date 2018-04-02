using NUnit.Framework;
using SharpFileSystem.Collections;
using SharpFileSystem.FileSystems;

namespace SharpFileSystem.Tests.FileSystems
{
    [TestFixture]
    public class EntityMoverRegistrationTest
    {
        [SetUp]
        public void Initialize()
        {
            Registration = new TypeCombinationDictionary<IEntityMover>();
            Registration.AddLast(typeof(PhysicalFileSystem), typeof(PhysicalFileSystem), physicalEntityMover);
            Registration.AddLast(typeof(IFileSystem), typeof(IFileSystem), standardEntityMover);
        }

        private TypeCombinationDictionary<IEntityMover> Registration;
        private readonly IEntityMover physicalEntityMover = new PhysicalEntityMover();
        private readonly IEntityMover standardEntityMover = new StandardEntityMover();

        [Test]
        public void When_MovingFromGenericToGenericFileSystem_Expect_StandardEntityMover()
        {
            Assert.AreEqual(
                Registration.GetSupportedRegistration(typeof(IFileSystem), typeof(IFileSystem)).Value,
                standardEntityMover
            );
        }

        [Test]
        public void When_MovingFromOtherToPhysicalFileSystem_Expect_StandardEntityMover()
        {
            Assert.AreEqual(
                Registration.GetSupportedRegistration(typeof(IFileSystem), typeof(PhysicalFileSystem)).Value,
                standardEntityMover
            );
        }

        [Test]
        public void When_MovingFromPhysicalToGenericFileSystem_Expect_StandardEntityMover()
        {
            Assert.AreEqual(
                Registration.GetSupportedRegistration(typeof(PhysicalFileSystem), typeof(IFileSystem)).Value,
                standardEntityMover
            );
        }

        [Test]
        public void When_MovingFromPhysicalToPhysicalFileSystem_Expect_PhysicalEntityMover()
        {
            Assert.AreEqual(
                Registration.GetSupportedRegistration(typeof(PhysicalFileSystem), typeof(PhysicalFileSystem)).Value,
                physicalEntityMover
            );
        }
    }
}
