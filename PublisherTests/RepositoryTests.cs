using NUnit.Framework;
using Caros.CI.Publisher;
using System;
using System.IO;

namespace Caros.CI.PublisherTests
{
    [TestFixture]
    public class RepositoryTests
    {
        public readonly static string DirtyRepoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestRepos/dirty-repo/");
        public readonly static string CleanRepoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestRepos/clean-repo/");
        public readonly static string NoRepoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestRepos/no-repo/");

        [TestCase]
        public void ShouldDetermineDirtyRepo()
        {
            var repo = new Repository(DirtyRepoPath);
            Assert.IsFalse(repo.IsClean);
        }

        [TestCase]
        public void ShouldDetermineCleanRepo()
        {
            var repo = new Repository(CleanRepoPath);
            Assert.IsTrue(repo.IsClean);
        }

        [TestCase]
        public void ShouldDetermineNoRepo()
        {
            var repo = new Repository(CleanRepoPath);
            Assert.IsTrue(repo.Exists);
        }
    }
}
