using System;
using NUnit.Framework;

namespace CardboardFactory.WpfCore.Test {
    [TestFixture]
    public class RelayCommandTest {
        [Test]
        public void RelayCommandThrowsException() {
            Assert.Throws<ArgumentNullException>(() => new RelayCommand(null));
        }

        [Test]
        public void RelayCommandExecutes() {
            bool commandExecuted = false;
            var command = new RelayCommand(o => commandExecuted = true);
            command.Execute(null);
            Assert.True(commandExecuted);
        }
    }
}
