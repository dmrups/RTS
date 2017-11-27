using System;
using Xunit;
using Server.Entities;
using System.Numerics;
using FluentAssertions;

namespace Server.Tests.Entities
{
    public class UnitTest
    {
        [Fact]
        public void Move_ShouldBeAtDestination()
        {
            // arrange
            var unit = new Unit();
            unit.Position = new Vector2(0, 0);
            unit.Speed = 9;
            unit.Size = 1;
            unit.Destination = new Vector2(1, 1);

            // act
            unit.Move(new Map());

            // assert
            unit.Position.Should().Be(new Vector2(1, 1));
        }

        [Fact]
        public void Move_ShouldBeOnTheWay()
        {
            // arrange
            var unit = new Unit();
            unit.Position = new Vector2(0, 0);
            unit.Speed = 1;
            unit.Size = 1;
            unit.Destination = new Vector2(1, 1);

            // act
            unit.Move(new Map());

            // assert
            unit.Position.Should().Be(new Vector2(0.707106769F, 0.707106769F));
        }
    }
}
