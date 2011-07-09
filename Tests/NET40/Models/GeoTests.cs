using System;
using MahApps.Twitter.Models;
using NUnit.Framework;

namespace MahApps.Twitter.NET40.UnitTests.Models
{
    [TestFixture]
    public class GeoTests
    {
        [Test]
        public void Long_WhenNotSet_ReturnsNull()
        {
            var geo = new Geo();
            geo.Coordinates = null;

            Assert.That(geo.Long, Is.Null);
        }

        [Test]
        public void Lat_WhenNotSet_ReturnsNull()
        {
            var geo = new Geo();
            geo.Coordinates = null;

            Assert.That(geo.Lat, Is.Null);
        }

        [Test]
        public void Long_WhenSet_ReturnsValue()
        {
            var geo = new Geo();
            geo.Coordinates = new[] { 10.0, 15.0 };

            Assert.That(geo.Long == 15.0);
        }

        [Test]
        public void Lat_WhenSet_ReturnsValue()
        {
            var geo = new Geo();
            geo.Coordinates = new[] {10.0, 15.0};

            Assert.That(geo.Lat == 10.0);
        }

        [Test]
        public void Lat_WhenOneSet_ReturnsValue()
        {
            var geo = new Geo();
            geo.Coordinates = new[] {10.0 };

            Assert.That(geo.Lat == 10.0);
        }

        [Test]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void Long_WhenOneSet_ThrowsException()
        {
            var geo = new Geo();
            geo.Coordinates = new[] { 10.0 };

            Assert.That(geo.Long == 10.0);
        }
    }
}