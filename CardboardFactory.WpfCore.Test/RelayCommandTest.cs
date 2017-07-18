using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace CardboardFactory.WpfCore.Test {
    [TestFixture]
    [ExcludeFromCodeCoverage]
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
