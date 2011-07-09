using MahApps.Twitter.Extensions;
using MahApps.Twitter.Models;
using NUnit.Framework;

namespace MahApps.Twitter.Tests.Extensions
{
    [TestFixture]
    public class JsonExtensionTests
    {
        class SomeObject : ITwitterResponse { }
        class SomeObjectWithValue : ITwitterResponse
        {
            public int X { get; set; }
        }

        [Test]
        public void Deserialize_WithEmptyString_ReturnsNull()
        {
            // act
            var response = "".Deserialize<SomeObject>();

            // assert
            Assert.That(response, Is.Null);
        }

        [Test]
        public void Deserialize_WithNullString_ReturnsException()
        {
            // act
            string str = null;
            var response = str.Deserialize<SomeObject>();

            // assert
            var error = response as ExceptionResponse;
            Assert.That(error, Is.Not.Null);
            Assert.That(!string.IsNullOrWhiteSpace(error.ErrorMessage));
        }

        [Test]
        public void Deserialize_WithInvalidJson_ReturnsException()
        {
            // act
            var response = "{ X : 'A' } ".Deserialize<SomeObjectWithValue>();

            // assert
            var error = response as ExceptionResponse;
            Assert.That(error, Is.Not.Null);
            Assert.That(error.ErrorMessage == "Error converting value \"A\" to type 'System.Int32'.");
        }

        [Test]
        public void Deserialize_WithBracesUncloses_ReturnsException()
        {
            // act
            var response = "{ ".Deserialize<ITwitterResponse>();

            // assert
            var error = response as ExceptionResponse;
            Assert.That(error, Is.Not.Null);
            Assert.That(error.ErrorMessage == "Unexpected end when deserializing object.");
        }
    }
}