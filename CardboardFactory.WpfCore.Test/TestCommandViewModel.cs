using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace CardboardFactory.WpfCore.Test {
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class TestCommandViewModel {
        [Test]
        public void CommandViewModelThrowsException() {
            Assert.Throws<ArgumentNullException>(() => new CommandViewModel(null, null));
        }

        [Test]
        public void CommandViewModelNotThrowsException() {
            var command = new RelayCommand(o => { });
            var commandVievMaodel = new CommandViewModel(null, command);
            Assert.NotNull(commandVievMaodel);
            Assert.AreEqual(command, commandVievMaodel.Command);
        }

        [Test]
        public void CommandViewModelHasDisplayName() {
            var commandVievMaodel = new CommandViewModel("CommandName", new RelayCommand(o => { }));
            Assert.NotNull(commandVievMaodel);
            Assert.AreEqual("CommandName", commandVievMaodel.DisplayName);
        }
    }
}
